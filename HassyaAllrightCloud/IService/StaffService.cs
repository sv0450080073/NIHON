using HassyaAllrightCloud.Application.Staff.Commands;
using HassyaAllrightCloud.Application.Staff.Queries;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons;
using System.Globalization;

namespace HassyaAllrightCloud.IService
{
    public interface IStaffListService
    {
        Task<List<StaffsData>> Get(int CompanyId, int ExceptStaffId = -1);
        Task<List<StaffData>> GetStafforStaffChart(DateTime date, int tenantCdSeq,List<int> comlst,List<int> branchlst);
        Task<List<LoadStaffList>> GetStaffReport(int TenantCdSeq, string DateStart);
        Task<List<EigyoStaffItem>> GetListEigyo(int companyCdSeq);
        Task<List<WorkLeaveItem>> GetListWorkLeave();
        Task<List<WorkHolidayItem>> GetListWorkHoliday(int companyCdSeq, string unkYmd);
        Task<List<VehicleAllocationItem>> GetListVehicleAllocation(int tenantCdSeq, string unkYmd);
        Task<List<CrewDataAcquisitionItem>> GetListCrewDataAcquisition(int companyCdSeq, string unkYmd);
        Task<List<NumberOfVehicle>> GetListNumberOfVehicle(int companyCdSeq, string unkYmd);
        Task<HaishaStaffItem> GetHaisha(string ukeNo, short unkRen, short teiDanNo, short bunkRen);
        Task<bool> HandleAssignWork(TkdHaiin haiin, List<TkdKoban> listKoban, JobItem job);
        Task<List<KoteikItem>> GetListKoteiK(string ukeNo, short unkRen, short teiDanNo, short bunkRen);
        Task<List<SyuTaikinCalculationTimeItem>> GetListSyuTaikinCalculationTime(int companyCdSeq);
        Task<bool> HandleSwapWork(TkdHaiin haiin, List<TkdKoban> listKoban, JobItem job, bool isSwapJob = false, TkdHaiin haiin1 = null, List<TkdKoban> listKoban1 = null, JobItem job1 = null);
        Task<bool> HandleUnassignWork(JobItem job);
        Task<bool> HandleWorkHoliday(int syainCdSeq, string unkYmd, TkdKoban koban, TkdKikyuj kikuyj);
        Task<List<PreDayEndTimeItem>> GetListPreDayEndTime(string previousYmd, string unkYmd, int companyCdSeq);
        Task<List<WorkTimeItem>> GetListWorkTime(string startYmd, string unkYmd);
        List<StaffModel> GetByGroup(int groupId, string currentDate, List<WorkHourModel> workHours);
        List<StaffModel> GetByCustomGroup(int groupId, string currentDate, List<WorkHourModel> workHours);
		Task<List<LoadStaff>> GetStaffByTenantIdAsync(int tentantId, string startDate, string endDate);
        Task<List<RestraintTime>> GetRestraintTime(TimeSearchParam searchParam, int period);
        Task<List<RestPeriod>> GetRestPeriod(TimeSearchParam searchParam);
        Task<List<Holiday>> GetHoliday(TimeSearchParam searchParam, int period);
        Task<List<PopupData>> GetListRule();
        Task<List<PopupValue>> GetListRuleValue(int tenantCdSeq);
        Task<(int, List<(int, string)>)> ValidateBeforeAssignJob(string selectedDate, int syainCdSeq, DataTable KobanTable,
            string delUkeNo = "", short delUnkRen = 0, short delTeiDanNo = 0, short delBunkRen = 0, bool breakIfError = true);
        Task GetTableKoban(JobItem Job, DataTable kobanTable);
        Task<StaffHaitaCheck> GetHaitaCheck(string ukeNo, short unkRen, short teiDanNo, short bunkRen);
        Task<string> GetHaitaCheckDelete(int syainCdSeq, string unkYmd);
    }
    public class StaffService : IStaffListService
    {
        private readonly KobodbContext _dbContext;
        private readonly IMediator _mediator;
        private readonly ITPM_CodeSyService _codeSyuService;

        public StaffService(KobodbContext context, IMediator mediator, ITPM_CodeSyService codeSyuService)
        {
            _dbContext = context;
            _mediator = mediator;
            _codeSyuService = codeSyuService;
        }
        public async Task<List<StaffData>> GetStafforStaffChart(DateTime date, int tenantCdSeq, List<int> comlst, List<int> branchlst)
        {
            string codeSyuSYOKUMUKBN = "SYOKUMUKBN";
            int tenantSYOKUMUKBN = await _codeSyuService.CheckTenantByKanriKbnAsync(tenantCdSeq, codeSyuSYOKUMUKBN);
            string DateAsString = date.ToString("yyyyMMdd");
            return await (from KYOSHE in _dbContext.VpmKyoShe
                          join SYAIN in _dbContext.VpmSyain on KYOSHE.SyainCdSeq equals SYAIN.SyainCdSeq into SYAIN_join
                          from SYAIN in SYAIN_join.DefaultIfEmpty()
                          join SYOKUM in _dbContext.VpmSyokum
                                on new { KYOSHE.SyokumuCdSeq, TenantCdSeq = tenantCdSeq }
                            equals new { SYOKUM.SyokumuCdSeq, SYOKUM.TenantCdSeq } into SYOKUM_join
                          from SYOKUM in SYOKUM_join.DefaultIfEmpty()
                          join SHOKUMU in _dbContext.VpmCodeKb on Convert.ToString(SYOKUM.SyokumuKbn) equals SHOKUMU.CodeKbn into SHOKUMU_join
                          from SHOKUMU in SHOKUMU_join.DefaultIfEmpty()
                          join EIGYOS in _dbContext.VpmEigyos on KYOSHE.EigyoCdSeq equals EIGYOS.EigyoCdSeq into EIGYOS_join
                          from EIGYOS in EIGYOS_join.DefaultIfEmpty()
                          join KAISHA in _dbContext.VpmCompny
                                on new { EIGYOS.CompanyCdSeq, TenantCdSeq = tenantCdSeq }
                            equals new { KAISHA.CompanyCdSeq, KAISHA.TenantCdSeq }
                          where
                            branchlst.Contains(KYOSHE.EigyoCdSeq) &&
                            comlst.Contains(KAISHA.CompanyCdSeq) &&
                            String.Compare(KYOSHE.StaYmd, DateAsString) <= 0 &&
                            String.Compare(KYOSHE.EndYmd, DateAsString) >= 0 &&
                            (new int[] { 1, 2, 3, 4, 5 }).Contains(SYOKUM.SyokumuKbn) &&
                            SHOKUMU.CodeSyu == codeSyuSYOKUMUKBN &&
                            SHOKUMU.TenantCdSeq == tenantSYOKUMUKBN
                          select new StaffData()
                          {
                              StaffID = SYAIN.SyainCdSeq.ToString(),
                              SyainCdSeq = KYOSHE.SyainCdSeq,
                              SyokumuCdSeq = KYOSHE.SyokumuCdSeq,
                              SyokumuNm = SYOKUM.SyokumuNm,
                              SyokumuKbn = SYOKUM.SyokumuKbn,
                              CodeKbnNm = SHOKUMU.CodeKbnNm,
                              TenkoNo = KYOSHE.TenkoNo,
                              EigyoCdSeq = KYOSHE.EigyoCdSeq,
                              EigyoCd = EIGYOS.EigyoCd,
                              RyakuNm = EIGYOS.RyakuNm,
                              SyainCd = SYAIN.SyainCd,
                              SyainNm = SYAIN.SyainNm,
                              StaYmd = KYOSHE.StaYmd,
                              EndYmd = KYOSHE.EndYmd,
                              CompanyCdSeq = KAISHA.CompanyCdSeq,
                              CompanyCd = KAISHA.CompanyCd,
                              CompanyNm = KAISHA.CompanyNm,
                              WorkNm = SYOKUM.SyokumuNm,
                              BigTypeDrivingFlg = KYOSHE.BigTypeDrivingFlg,
                              MediumTypeDrivingFlg = KYOSHE.MediumTypeDrivingFlg,
                              SmallTypeDrivingFlg = KYOSHE.SmallTypeDrivingFlg,
                              StaffVehicle = (SYOKUM.SyokumuKbn == 1 || SYOKUM.SyokumuKbn == 2) ? 1 : (SYOKUM.SyokumuKbn == 3 || SYOKUM.SyokumuKbn == 4) ? 2 : 0
                          }).ToListAsync();
        }
        public async Task<List<StaffsData>> Get(int CompanyId, int ExceptStaffId = -1)
        {
            string DateAsString = DateTime.Today.ToString("yyyyMMdd");
            return await (from syain in _dbContext.VpmSyain
                          from kyoshe in _dbContext.VpmKyoShe
                          where syain.SyainCdSeq == kyoshe.SyainCdSeq && DateAsString.CompareTo(kyoshe.StaYmd) >= 0 && DateAsString.CompareTo(kyoshe.EndYmd) <= 0
                          from eigyos in _dbContext.VpmEigyos
                          where kyoshe.EigyoCdSeq == eigyos.EigyoCdSeq
                              && eigyos.CompanyCdSeq == CompanyId
                          orderby syain.SyainCd, syain.SyainCdSeq
                          select new StaffsData(syain)).ToListAsync();
        }


        public async Task<List<LoadStaffList>> GetStaffReport(int TenantCdSeq, string DateStart)
        {
            var data = (from SYAIN in _dbContext.VpmSyain
                        join KYOSHE in _dbContext.VpmKyoShe
                        on SYAIN.SyainCdSeq
                        equals KYOSHE.SyainCdSeq
                        into KYOSHE_join
                        from KYOSHE in KYOSHE_join.DefaultIfEmpty()
                        join EIGYOS in _dbContext.VpmEigyos
                        on new { KYOSHE.EigyoCdSeq, E1 = 1 }
                        equals new { EIGYOS.EigyoCdSeq, E1 = (int)EIGYOS.SiyoKbn }
                        into EIGYOS_join
                        from EIGYOS in EIGYOS_join.DefaultIfEmpty()
                        join KAISHA in _dbContext.VpmCompny
                        on EIGYOS.CompanyCdSeq equals KAISHA.CompanyCdSeq
                        into KAISHA_join
                        from KAISHA in KAISHA_join.DefaultIfEmpty()
                        where KAISHA.TenantCdSeq == TenantCdSeq
                        && String.Compare(KYOSHE.StaYmd, DateStart) <= 0
                        && String.Compare(KYOSHE.EndYmd, DateStart) >= 0
                        orderby EIGYOS.EigyoCd, SYAIN.SyainCd
                        select new LoadStaffList()
                        {
                            EigyoCdSeq = EIGYOS.EigyoCdSeq,
                            EigyoCd = EIGYOS.EigyoCd,
                            RyakuNm = EIGYOS.RyakuNm,
                            SyainCdSeq = SYAIN.SyainCdSeq,
                            SyainCd = SYAIN.SyainCd,
                            SyainNm = SYAIN.SyainNm
                        }).ToList();
            return data;
        }

        public List<StaffModel> GetByGroup(int groupId, string currentDate, List<WorkHourModel> workHours)
        {
            var toDateDateTimeType = DateTime.ParseExact(currentDate, "yyyyMMdd", null);
            var pre7DayFromCurrentDate = toDateDateTimeType.AddDays(-7).ToString().Substring(0, 10).Replace("/", string.Empty);
            var pre28DayFromCurrentDate = toDateDateTimeType.AddDays(-27).ToString().Substring(0, 10).Replace("/", string.Empty);
            var result = new List<StaffModel>();

            string DateAsString = DateTime.Today.ToString("yyyyMMdd");
            var staffs = (from syain in _dbContext.VpmSyain
                          from kyoshe in _dbContext.VpmKyoShe
                          where syain.SyainCdSeq == kyoshe.SyainCdSeq && DateAsString.CompareTo(kyoshe.StaYmd) >= 0 && DateAsString.CompareTo(kyoshe.EndYmd) <= 0
                          && kyoshe.EigyoCdSeq == groupId
                          orderby syain.SyainCd, syain.SyainCdSeq
                          select new StaffsData(syain)).ToList();
            foreach (var item in staffs)
            {
                int workHourInOneWeek = 0;
                int workHourInFourWeek = 0;
                List<WorkHourModel> models = new List<WorkHourModel>();
                var staffOneWeekWorkHours = workHours.Where(x => x.SyainCdSeq == item.SyainCdSeq && x.UnkYmd.CompareTo(pre7DayFromCurrentDate) >= 0).OrderBy(x => x.UnkYmd).ThenBy(x => x.SyukinTime).Distinct().ToList();
                var staffFourWeekWorkHours = workHours.Where(x => x.SyainCdSeq == item.SyainCdSeq && x.UnkYmd.CompareTo(pre28DayFromCurrentDate) >= 0).OrderBy(x => x.UnkYmd).ThenBy(x => x.SyukinTime).Distinct().ToList();
                workHourInOneWeek = (int)GetStaffWorkHour(staffOneWeekWorkHours);
                workHourInFourWeek = (int)GetStaffWorkHour(staffFourWeekWorkHours);

                result.Add(new StaffModel()
                {
                    id = item.SyainCdSeq,
                    SyainCd = item.SyainCd,
                    EmployeeName = item.SyainNm,
                    OneWeekWorkingHour = workHourInOneWeek < 0 ? (-workHourInOneWeek).ToString() : workHourInOneWeek.ToString(),
                    FourWeekWorkingHour = workHourInFourWeek < 0 ? (-workHourInFourWeek).ToString() : workHourInFourWeek.ToString()
                });
            }

            return result;
        }

        private double GetStaffWorkHour(List<WorkHourModel> workHours)
        {
            double workHour = 0;

            foreach (var sItem in workHours)
            {
                var distinctWorkHourOfStaffs = workHours.Where(x => x.UnkYmd == sItem.UnkYmd).ToList();

                if (distinctWorkHourOfStaffs.Count == 1)
                {
                    var sDate = DateTime.ParseExact(distinctWorkHourOfStaffs[0].UnkYmd + (string.IsNullOrWhiteSpace(distinctWorkHourOfStaffs[0].SyukinTime) ? "0000" : distinctWorkHourOfStaffs[0].SyukinTime), "yyyyMMddHHmm", null);
                    var eDate = DateTime.ParseExact(distinctWorkHourOfStaffs[0].UnkYmd + (string.IsNullOrWhiteSpace(distinctWorkHourOfStaffs[0].TaiknTime) ? "0000" : distinctWorkHourOfStaffs[0].TaiknTime), "yyyyMMddHHmm", null);
                    if (sDate < eDate)
                    {
                        workHour += (sDate - eDate).TotalHours;
                    }
                }
                else if (distinctWorkHourOfStaffs.Count > 1)
                {
                    var totalWorkHour = (DateTime.ParseExact(distinctWorkHourOfStaffs[distinctWorkHourOfStaffs.Count - 1].UnkYmd + (string.IsNullOrWhiteSpace(distinctWorkHourOfStaffs[distinctWorkHourOfStaffs.Count - 1].TaiknTime) ? "0000" : distinctWorkHourOfStaffs[distinctWorkHourOfStaffs.Count - 1].TaiknTime), "yyyyMMddHHmm", null) - DateTime.ParseExact(distinctWorkHourOfStaffs[0].UnkYmd.Trim() + (string.IsNullOrWhiteSpace(distinctWorkHourOfStaffs[0].SyukinTime.Trim()) ? "0000" : distinctWorkHourOfStaffs[0].SyukinTime.Trim()), "yyyyMMddHHmm", null)).TotalHours;

                    double notWorkHourInDay = 0;
                    for (int i = 0; i < distinctWorkHourOfStaffs.Count - 1; i++)
                    {
                        if (DateTime.ParseExact(distinctWorkHourOfStaffs[i].UnkYmd + (string.IsNullOrWhiteSpace(distinctWorkHourOfStaffs[i].TaiknTime) ? "0000" : distinctWorkHourOfStaffs[i].TaiknTime), "yyyyMMddHHmm", null) < DateTime.ParseExact(distinctWorkHourOfStaffs[i + 1].UnkYmd + distinctWorkHourOfStaffs[i + 1].SyukinTime, "yyyyMMddHHmm", null))
                        {
                            notWorkHourInDay += (DateTime.ParseExact(distinctWorkHourOfStaffs[i + 1].UnkYmd + (string.IsNullOrWhiteSpace(distinctWorkHourOfStaffs[i + 1].SyukinTime) ? "0000" : distinctWorkHourOfStaffs[i + 1].SyukinTime), "yyyyMMddHHmm", null) - DateTime.ParseExact(distinctWorkHourOfStaffs[i].UnkYmd + (string.IsNullOrWhiteSpace(distinctWorkHourOfStaffs[i].TaiknTime) ? "0000" : distinctWorkHourOfStaffs[i].TaiknTime), "yyyyMMddHHmm", null)).TotalHours;
                        }
                    }
                    workHour += totalWorkHour - notWorkHourInDay;
                }
            }

            return workHour;
        }

        public List<StaffModel> GetByCustomGroup(int groupId, string currentDate, List<WorkHourModel> workHours)
        {
            var toDateDateTimeType = DateTime.ParseExact(currentDate, "yyyyMMdd", null);
            var pre7DayFromCurrentDate = toDateDateTimeType.AddDays(-7).ToString().Substring(0, 10).Replace("/", string.Empty);
            var pre28DayFromCurrentDate = toDateDateTimeType.AddDays(-27).ToString().Substring(0, 10).Replace("/", string.Empty);
            var result = new List<StaffModel>();

            string DateAsString = DateTime.Today.ToString("yyyyMMdd");

            var staffs = (from customgr in _dbContext.TkdSchCusGrpMem
                          join syain in _dbContext.VpmSyain
                          on customgr.SyainCdSeq equals syain.SyainCdSeq into cs
                          from csTemp in cs.DefaultIfEmpty()
                          where customgr.CusGrpSeq == groupId
                          select new StaffsData()
                          {
                              SyainCd = csTemp.SyainCd,
                              SyainNm = csTemp.SyainNm,
                              SyainCdSeq = csTemp.SyainCdSeq
                          }).ToList();
            foreach (var item in staffs)
            {
                int workHourInOneWeek = 0;
                int workHourInFourWeek = 0;
                List<WorkHourModel> models = new List<WorkHourModel>();
                var staffOneWeekWorkHours = workHours.Where(x => x.SyainCdSeq == item.SyainCdSeq && x.UnkYmd.CompareTo(pre7DayFromCurrentDate) >= 0).OrderBy(x => x.UnkYmd).ThenBy(x => x.SyukinTime).Distinct().ToList();
                var staffFourWeekWorkHours = workHours.Where(x => x.SyainCdSeq == item.SyainCdSeq && x.UnkYmd.CompareTo(pre28DayFromCurrentDate) >= 0).OrderBy(x => x.UnkYmd).ThenBy(x => x.SyukinTime).Distinct().ToList();
                workHourInOneWeek = (int)GetStaffWorkHour(staffOneWeekWorkHours);
                workHourInFourWeek = (int)GetStaffWorkHour(staffFourWeekWorkHours);

                result.Add(new StaffModel()
                {
                    id = item.SyainCdSeq,
                    SyainCd = item.SyainCd,
                    EmployeeName = item.SyainNm,
                    OneWeekWorkingHour = workHourInOneWeek < 0 ? (-workHourInOneWeek).ToString() : workHourInOneWeek.ToString(),
                    FourWeekWorkingHour = workHourInFourWeek < 0 ? (-workHourInFourWeek).ToString() : workHourInFourWeek.ToString()
                });
            }

            return result;
        }

        public async Task<List<EigyoStaffItem>> GetListEigyo(int companyCdSeq)
        {
            return await _mediator.Send(new GetEigyoForSearchQuery() { CompanyCdSeq = companyCdSeq });
        }

        public async Task<List<WorkLeaveItem>> GetListWorkLeave()
        {
            return await _mediator.Send(new GetWorkLeaveListQuery());
        }

        public async Task<List<WorkHolidayItem>> GetListWorkHoliday(int companyCdSeq, string unkYmd)
        {
            return await _mediator.Send(new GetWorkHolidayListQuery() { CompanyCdSeq = companyCdSeq, UnkYmd = unkYmd });
        }

        public async Task<List<VehicleAllocationItem>> GetListVehicleAllocation(int tenantCdSeq, string unkYmd)
        {
            return await _mediator.Send(new GetVehicleAllocationListQuery() { TenantCdSeq = tenantCdSeq, UnkYmd = unkYmd });
        }

        public async Task<List<CrewDataAcquisitionItem>> GetListCrewDataAcquisition(int companyCdSeq, string unkYmd)
        {
            return await _mediator.Send(new GetCrewDataAcquisitionListQuery() { CompanyCdSeq = companyCdSeq, UnkYmd = unkYmd });
        }

        public async Task<List<NumberOfVehicle>> GetListNumberOfVehicle(int companyCdSeq, string unkYmd)
        {
            return await _mediator.Send(new GetNumberOfVehicleQuery() { CompanyCdSeq = companyCdSeq, UnkYmd = unkYmd });
        }

        public async Task<HaishaStaffItem> GetHaisha(string ukeNo, short unkRen, short teiDanNo, short bunkRen)
        {
            return await _mediator.Send(new GetHaishaInfoQuery() { UkeNo = ukeNo, UnkRen = unkRen, TeiDanNo = teiDanNo, BunkRen = bunkRen });
        }

        public async Task<bool> HandleAssignWork(TkdHaiin haiin, List<TkdKoban> listKoban, JobItem job)
        {
            return await _mediator.Send(new HandleAssignWorkCommand() { haiin = haiin, listKoban = listKoban, job = job });
        }

        public async Task<List<KoteikItem>> GetListKoteiK(string ukeNo, short unkRen, short teiDanNo, short bunkRen)
        {
            return await _mediator.Send(new GetListKoteikQuery() { UkeNo = ukeNo, UnkRen = unkRen, TeiDanNo = teiDanNo, BunkRen = bunkRen });
        }

        public async Task<List<SyuTaikinCalculationTimeItem>> GetListSyuTaikinCalculationTime(int companyCdSeq)
        {
            return await _mediator.Send(new GetListSyuTaikinCalculationTimeQuery() { CompanyCdSeq = companyCdSeq });
        }

        public async Task<bool> HandleSwapWork(TkdHaiin haiin, List<TkdKoban> listKoban, JobItem job, bool isSwapJob = false, TkdHaiin haiin1 = null, List<TkdKoban> listKoban1 = null, JobItem job1 = null)
        {
            return await _mediator.Send(new HandleSwapWorkCommand() { haiin = haiin, listKoban = listKoban, job = job, isSwapJob = isSwapJob, haiin1 = haiin1, listKoban1 = listKoban1, job1 = job1 });
        }

        public async Task<bool> HandleUnassignWork(JobItem job)
        {
            return await _mediator.Send(new HandleUnassignWorkCommand() { job = job });
        }

        public async Task<bool> HandleWorkHoliday(int syainCdSeq, string unkYmd, TkdKoban koban, TkdKikyuj kikuyj)
        {
            return await _mediator.Send(new HandleWorkHolidayCommand() { SyainCdSeq = syainCdSeq, UnkYmd = unkYmd, koban = koban, kikyuj = kikuyj });
        }

        public async Task<List<PreDayEndTimeItem>> GetListPreDayEndTime(string previousYmd, string unkYmd, int companyCdSeq)
        {
            return await _mediator.Send(new GetListPreDayEndTimeQuery() { PreviousYmd = previousYmd, UnkYmd = unkYmd, CompanyCdSeq = companyCdSeq });
        }

        public async Task<List<WorkTimeItem>> GetListWorkTime(string startYmd, string unkYmd)
        {
            return await _mediator.Send(new GetListWorkTimeQuery() { StartYmd = startYmd, UnkYmd = unkYmd });
        }

		public async Task<List<LoadStaff>> GetStaffByTenantIdAsync(int tentantId, string startDate, string endDate)
        {
            return await _mediator.Send(new GetStaffByTenantIdQuery(tentantId, startDate, endDate));
        }
        public async Task<List<RestraintTime>> GetRestraintTime(TimeSearchParam searchParam, int period)
        {
            return await _mediator.Send(new GetRestraintTimeQuery() { SearchParam = searchParam, Period = period });
        }

        public async Task<List<RestPeriod>> GetRestPeriod(TimeSearchParam searchParam)
        {
            return await _mediator.Send(new GetRestPeriodQuery() { SearchParam = searchParam });
        }

        public async Task<List<Holiday>> GetHoliday(TimeSearchParam searchParam, int period)
        {
            return await _mediator.Send(new GetHolidayQuery() { SearchParam = searchParam, Period = period });
        }

        public async Task<List<PopupData>> GetListRule()
        {
            return await _mediator.Send(new GetPopupDataQuery());
        }

        public async Task<List<PopupValue>> GetListRuleValue(int tenantCdSeq)
        {
            return await _mediator.Send(new GetPopupValueQuery() { TenantCdSeq = tenantCdSeq });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedDate"></param>
        /// <param name="syainCdSeq"></param>
        /// <param name="KobanTable"></param>
        /// <returns>
        /// (int, List<int>)
        /// count error when KaizenKijunYmd is null.
        /// list (syainCdSeq, kijunNm) invalid.
        /// </returns>
        public async Task<(int, List<(int, string)>)> ValidateBeforeAssignJob(string selectedDate, int syainCdSeq, DataTable KobanTable,
            string delUkeNo = "", short delUnkRen = 0, short delTeiDanNo = 0, short delBunkRen = 0, bool breakIfError = true)
        {
            List<(int, string)> listError = new List<(int, string)>();
            int countError = 0;

            var timeSearchParam = new TimeSearchParam();
            timeSearchParam.Times = 1;
            timeSearchParam.DriverNaikinOnly = 1;
            timeSearchParam.UnkYmd = selectedDate;
            timeSearchParam.SyainCdSeq = syainCdSeq;
            timeSearchParam.KobanTableType = KobanTable;
            timeSearchParam.DelUkeNo = delUkeNo;
            timeSearchParam.DelUnkRen = delUnkRen;
            timeSearchParam.DelTeiDanNo = delTeiDanNo;
            timeSearchParam.DelBunkRen = delBunkRen;

            var taskRule = GetListRule();
            var taskRuleValue = GetListRuleValue(new ClaimModel().TenantID);

            await Task.WhenAll(taskRule, taskRuleValue);

            var listRule = taskRule.Result;
            var listRuleValue = taskRuleValue.Result;

            foreach (var item in listRule)
            {
                if (item.PeriodUnit == 1 || item.PeriodUnit == 2)
                {
                    var popupValue = listRuleValue.Where(_ => _.KijunSeq == item.KijunSeq).ToList();
                    foreach (var value in popupValue)
                    {
                        if (value.RestrictedTarget == 1)
                        {
                            countError += await HandleRestrictedTarget1(item, timeSearchParam, popupValue, 0, listError, breakIfError, syainCdSeq);
                        }
                        else if (value.RestrictedTarget == 2)
                        {
                            await HandleRestrictedTarget2(item, timeSearchParam, popupValue, listError, breakIfError, syainCdSeq, 0);
                        }
                        else if (value.RestrictedTarget == 3)
                        {
                            countError += await HandleRestrictedTarget3(item, timeSearchParam, popupValue, listError, breakIfError, syainCdSeq, 0);
                        }
                        if (breakIfError && (countError > 0 || listError.Any())) break;
                    }
                }
                else if (item.PeriodUnit == 3)
                {
                    var popupValue = listRuleValue.Where(_ => _.KijunSeq == item.KijunSeq).ToList();
                    foreach (var value in popupValue)
                    {
                        if (value.RestrictedTarget == 6)
                        {
                            var temp = listRuleValue.Where(_ => _.KijunSeq == item.KijunRef).ToList();
                            foreach (var t in temp)
                            {
                                if (t.RestrictedTarget == 1)
                                {
                                    countError += await HandleRestrictedTarget1(item, timeSearchParam, temp, 1, listError, breakIfError, syainCdSeq);
                                }
                                else if (t.RestrictedTarget == 2)
                                {
                                    await HandleRestrictedTarget2(item, timeSearchParam, temp, listError, breakIfError, syainCdSeq, 1);
                                }
                                else if (t.RestrictedTarget == 3)
                                {
                                    countError += await HandleRestrictedTarget3(item, timeSearchParam, temp, listError, breakIfError, syainCdSeq, 1);
                                }
                            }
                        }
                        if (breakIfError && (countError > 0 || listError.Any())) break;
                    }
                }
                if (breakIfError && (countError > 0 || listError.Any())) break;
            }

            return (countError, listError);
        }

        private async Task<int> HandleRestrictedTarget1(PopupData item, TimeSearchParam timeSearchParam, List<PopupValue> popupValue, byte type, List<(int, string)> listError,
            bool breakIfError, int syainCdSeq)
        {
            int count = 0;
            var periodUnit = type == 0 ? item.PeriodUnit : item.RefPeriodUnit;
            var periodValue = type == 0 ? item.PeriodValue : item.RefPeriodValue;
            if (periodUnit == 1)
            {
                var popupData = await GetRestraintTime(timeSearchParam, 7 * periodValue);
                if (popupData != null)
                {
                    var checkData = popupValue.Where(_ => _.RestrictedTarget == 1).ToList();
                    if (popupData.Any())
                    {
                        foreach (var p in popupData)
                        {
                            foreach (var check in checkData)
                            {
                                count += HandleAddSyainWithCount(check.RestrictedExp, check.RestrictedValue * 60, p.KousokuMinute, p.KousokuMinute, p.KousokuMinute, p.SyainCdSeq, item.KijunNm, listError, type);
                                if (breakIfError && listError.Any()) break;
                            }

                            if (type != 0)
                            {
                                var checkData3 = popupValue.Where(_ => _.RestrictedTarget == 6).ToList();
                                foreach (var check3 in checkData3)
                                {
                                    HandleAddSyain(check3.RestrictedExp, check3.RestrictedValue, count, count, count, p.SyainCdSeq, item.KijunNm, listError);
                                    if (breakIfError && listError.Any()) break;
                                }
                            }

                            if (breakIfError && listError.Any()) break;
                        }
                    }
                    else
                    {
                        foreach (var check in checkData)
                        {
                            count += HandleAddSyainWithCount(check.RestrictedExp, check.RestrictedValue * 60, 0, 0, 0, syainCdSeq, item.KijunNm, listError, type);
                            if (breakIfError && listError.Any()) break;
                        }

                        if (type != 0 && (!breakIfError || (breakIfError && !listError.Any())))
                        {
                            var checkData3 = popupValue.Where(_ => _.RestrictedTarget == 6).ToList();
                            foreach (var check3 in checkData3)
                            {
                                HandleAddSyain(check3.RestrictedExp, check3.RestrictedValue, 0, 0, 0, syainCdSeq, item.KijunNm, listError);
                                if (breakIfError && listError.Any()) break;
                            }
                        }
                    }
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else if (periodUnit == 2)
            {
                var popupData = await GetRestraintTime(timeSearchParam, periodValue);
                if (popupData != null)
                {
                    var checkData = popupValue.Where(_ => _.RestrictedTarget == 4).ToList();
                    if (popupData.Any())
                    {
                        var listDrvJin = popupData.Select(_ => _.DrvJin.Split(",")).SelectMany(_ => _).ToList();
                        int minDrvJin = int.Parse(string.IsNullOrEmpty(listDrvJin.Min(_ => _)) || string.IsNullOrWhiteSpace(listDrvJin.Min(_ => _)) ? "0" : listDrvJin.Min(_ => _));
                        int maxDrvJin = int.Parse(string.IsNullOrEmpty(listDrvJin.Max(_ => _)) || string.IsNullOrWhiteSpace(listDrvJin.Max(_ => _)) ? "0" : listDrvJin.Max(_ => _));
                        foreach (var p in popupData)
                        {
                            int countRestricted4 = 0;
                            foreach (var check in checkData)
                            {
                                switch (check.RestrictedExp)
                                {
                                    case 1:
                                        if (check.RestrictedValue == int.Parse(p.DrvJin)) countRestricted4++; break;
                                    case 2:
                                        if (check.RestrictedValue != int.Parse(p.DrvJin)) countRestricted4++; break;
                                    case 3:
                                        if (check.RestrictedValue < minDrvJin) countRestricted4++; break;
                                    case 4:
                                        if (check.RestrictedValue <= minDrvJin) countRestricted4++; break;
                                    case 5:
                                        if (check.RestrictedValue >= maxDrvJin) countRestricted4++; break;
                                    case 6:
                                        if (check.RestrictedValue > maxDrvJin) countRestricted4++; break;
                                }
                            }

                            if (countRestricted4 > 0)
                            {
                                var checkRestricted1 = popupValue.Where(_ => _.RestrictedTarget == 1).ToList();
                                foreach (var check in checkRestricted1)
                                {
                                    count += HandleAddSyainWithCount(check.RestrictedExp, check.RestrictedValue * 60, p.KousokuMinute, p.KousokuMinute, p.KousokuMinute, p.SyainCdSeq, item.KijunNm, listError, type);
                                }
                                if (breakIfError && listError.Any()) break;
                            }

                            if (type != 0)
                            {
                                var checkData3 = popupValue.Where(_ => _.RestrictedTarget == 6).ToList();
                                foreach (var check3 in checkData3)
                                {
                                    HandleAddSyain(check3.RestrictedExp, check3.RestrictedValue, count, count, count, p.SyainCdSeq, item.KijunNm, listError);
                                    if (breakIfError && listError.Any()) break;
                                }
                            }

                            if (breakIfError && listError.Any()) break;
                        }
                    }
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            return 0;
        }

        private async Task HandleRestrictedTarget2(PopupData item, TimeSearchParam timeSearchParam, List<PopupValue> popupValue, List<(int, string)> listError, bool breakIfError,
            int syainCdSeq, byte type)
        {
            var periodUnit = type == 0 ? item.PeriodUnit : item.RefPeriodUnit;
            if (periodUnit == 2)
            {
                var checkData = popupValue.Where(_ => _.RestrictedTarget == 4 || _.RestrictedTarget == 5).ToList();
                var listPeriod = await GetRestPeriod(timeSearchParam);
                if (listPeriod.Any())
                {
                    var listDrvJin = listPeriod.Select(_ => _.DrvJin.Split(",")).SelectMany(_ => _).ToList();
                    int minDrvJin = int.Parse(string.IsNullOrEmpty(listDrvJin.Min(_ => _)) || string.IsNullOrWhiteSpace(listDrvJin.Min(_ => _)) ? "0" : listDrvJin.Min(_ => _));
                    int maxDrvJin = int.Parse(string.IsNullOrEmpty(listDrvJin.Max(_ => _)) || string.IsNullOrWhiteSpace(listDrvJin.Max(_ => _)) ? "0" : listDrvJin.Max(_ => _));
                    foreach (var period in listPeriod)
                    {
                        int countRestricted45 = 0;
                        foreach (var check in checkData)
                        {
                            if (check.RestrictedTarget == 4)
                            {
                                switch (check.RestrictedExp)
                                {
                                    case 1:
                                        if (check.RestrictedValue == int.Parse(period.DrvJin)) countRestricted45++; break;
                                    case 2:
                                        if (check.RestrictedValue != int.Parse(period.DrvJin)) countRestricted45++; break;
                                    case 3:
                                        if (check.RestrictedValue < minDrvJin) countRestricted45++; break;
                                    case 4:
                                        if (check.RestrictedValue <= minDrvJin) countRestricted45++; break;
                                    case 5:
                                        if (check.RestrictedValue >= maxDrvJin) countRestricted45++; break;
                                    case 6:
                                        if (check.RestrictedValue > maxDrvJin) countRestricted45++; break;
                                }
                            }
                            else
                            {
                                switch (check.RestrictedExp)
                                {
                                    case 1:
                                        if (check.RestrictedValue == period.KyusoCnt) countRestricted45++; break;
                                    case 2:
                                        if (check.RestrictedValue != period.KyusoCnt) countRestricted45++; break;
                                    case 3:
                                        if (check.RestrictedValue < period.KyusoCnt) countRestricted45++; break;
                                    case 4:
                                        if (check.RestrictedValue <= period.KyusoCnt) countRestricted45++; break;
                                    case 5:
                                        if (check.RestrictedValue >= period.KyusoCnt) countRestricted45++; break;
                                    case 6:
                                        if (check.RestrictedValue > period.KyusoCnt) countRestricted45++; break;
                                }
                            }
                        }

                        if(countRestricted45 > 0)
                        {
                            var checkRestricted2 = popupValue.Where(_ => _.RestrictedTarget == 2).ToList();
                            foreach(var check in checkRestricted2)
                            {
                                HandleAddSyain(check.RestrictedExp, check.RestrictedValue * 60, period.KyusokuMinute, period.KyusokuMinute, period.KyusokuMinute, period.SyainCdSeq, item.KijunNm, listError);
                            }
                            if (breakIfError && listError.Any()) break;
                        }
                    }
                }
            }
        }

        private async Task<int> HandleRestrictedTarget3(PopupData item, TimeSearchParam timeSearchParam, List<PopupValue> popupValue, List<(int, string)> listError, bool breakIfError,
            int syainCdSeq, byte type)
        {
            var periodUnit = type == 0 ? item.PeriodUnit : item.RefPeriodUnit;
            var periodValue = type == 0 ? item.PeriodValue : item.RefPeriodValue;
            if (periodUnit == 1)
            {
                var popupData = await GetHoliday(timeSearchParam, 7 * periodValue);
                if (popupData != null)
                {
                    var checkData = popupValue.Where(_ => _.RestrictedTarget == 3).ToList();
                    if (popupData.Any())
                    {
                        foreach (var p in popupData)
                        {
                            foreach (var check in checkData)
                            {
                                HandleAddSyain(check.RestrictedExp, check.RestrictedValue, p.LeaveCnt, p.LeaveCnt, p.LeaveCnt, p.SyainCdSeq, item.KijunNm, listError);
                                if (breakIfError && listError.Any()) break;
                            }
                            if (breakIfError && listError.Any()) break;
                        }
                    }
                    else
                    {
                        foreach (var check in checkData)
                        {
                            HandleAddSyain(check.RestrictedExp, check.RestrictedValue, 0, 0, 0, syainCdSeq, item.KijunNm, listError);
                            if (breakIfError && listError.Any()) break;
                        }
                    }
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            return 0;
        }

        private void HandleAddSyain(byte RestrictedExp, int RestrictedValue, int data, int minData, int maxData, int syainCdSeq, string KijunNm, List<(int, string)> listError)
        {
            switch (RestrictedExp)
            {
                case 1:
                    if (RestrictedValue != data) listError.Add((syainCdSeq, KijunNm)); break;
                case 2:
                    if (RestrictedValue == data) listError.Add((syainCdSeq, KijunNm)); break;
                case 3:
                    if (RestrictedValue >= minData) listError.Add((syainCdSeq, KijunNm)); break;
                case 4:
                    if (RestrictedValue > minData) listError.Add((syainCdSeq, KijunNm)); break;
                case 5:
                    if (RestrictedValue < maxData) listError.Add((syainCdSeq, KijunNm)); break;
                case 6:
                    if (RestrictedValue <= maxData) listError.Add((syainCdSeq, KijunNm)); break;
            }
        }

        private int HandleAddSyainWithCount(byte RestrictedExp, int RestrictedValue, int data, int minData, int maxData, int syainCdSeq, string KijunNm, List<(int, string)> listError, byte type)
        {
            switch (RestrictedExp)
            {
                case 1:
                    if (RestrictedValue != data)
                    {
                        if(type == 0)
                            listError.Add((syainCdSeq, KijunNm));
                        return 0;
                    }
                    else return 1;
                case 2:
                    if (RestrictedValue == data)
                    {
                        if (type == 0)
                            listError.Add((syainCdSeq, KijunNm));
                        return 0;
                    }
                    else return 1;
                case 3:
                    if (RestrictedValue >= minData)
                    {
                        if (type == 0)
                            listError.Add((syainCdSeq, KijunNm));
                        return 0;
                    }
                    else return 1;
                case 4:
                    if (RestrictedValue > minData)
                    {
                        if (type == 0)
                            listError.Add((syainCdSeq, KijunNm));
                        return 0;
                    }
                    else return 1;
                case 5:
                    if (RestrictedValue < maxData)
                    {
                        if (type == 0)
                            listError.Add((syainCdSeq, KijunNm));
                        return 0;
                    }
                    else return 1;
                case 6:
                    if (RestrictedValue <= maxData)
                    {
                        if (type == 0)
                            listError.Add((syainCdSeq, KijunNm));
                        return 0;
                    }
                    else return 1;
            }
            return 0;
        }

        public async Task GetTableKoban(JobItem Job, DataTable kobanTable)
        {
            var taskHaisha = GetHaisha(Job.UkeNo, Job.UnkRen, Job.TeiDanNo, Job.BunkRen);
            var taskKoteik = GetListKoteiK(Job.UkeNo, Job.UnkRen, Job.TeiDanNo, Job.BunkRen);
            var taskSyuTaikinCalculationTime = GetListSyuTaikinCalculationTime(new ClaimModel().CompanyID);
            await Task.WhenAll(taskHaisha, taskKoteik, taskSyuTaikinCalculationTime);

            var haisha = taskHaisha.Result;
            var listKoteik = taskKoteik.Result;
            var listSyuTaikinCalculationTime = taskSyuTaikinCalculationTime.Result;

            if (haisha != null)
            {
                DateTime start = DateTime.MinValue;
                DateTime end = DateTime.MinValue;
                if (DateTime.TryParseExact(haisha.SyuKoYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture, DateTimeStyles.None, out start)
                && DateTime.TryParseExact(haisha.KikYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture, DateTimeStyles.None, out end))
                {
                    byte days = (byte)((end - start).TotalDays + 1);
                    for (int i = 0; i < days; i++)
                    {
                        TkdKoban koban = new TkdKoban();
                        MapKobanAssignWork(koban, i, start, end, haisha, listKoteik, listSyuTaikinCalculationTime, Job);
                        var dr = kobanTable.NewRow();
                        dr["UnkYmd"] = koban.UnkYmd;
                        dr["UkeNo"] = koban.UkeNo;
                        dr["UnkRen"] = koban.UnkRen;
                        dr["TeiDanNo"] = koban.TeiDanNo;
                        dr["BunkRen"] = koban.BunkRen;
                        dr["SyukinYmd"] = koban.SyukinYmd;
                        dr["SyukinTime"] = koban.SyukinTime;
                        dr["TaikinYmd"] = koban.TaikinYmd;
                        dr["TaiknTime"] = koban.TaiknTime;
                        kobanTable.Rows.Add(dr);
                    }
                }
            }
        }

        public void MapKobanAssignWork(TkdKoban koban, int dayAdd, DateTime start, DateTime end, HaishaStaffItem haisha,
            List<KoteikItem> listKoteik, List<SyuTaikinCalculationTimeItem> listSyuTaikinCalculationTime, JobItem Job)
        {
            koban.UnkYmd = start.AddDays(dayAdd).ToString(CommonConstants.FormatYMD);
            koban.UkeNo = Job.UkeNo;
            koban.UnkRen = Job.UnkRen;
            koban.TeiDanNo = Job.TeiDanNo;
            koban.BunkRen = Job.BunkRen;
            koban.SyukinYmd = koban.UnkYmd;
            koban.TaikinYmd = koban.UnkYmd;
            koban.TaiknTime = string.Empty;
            koban.KyuKtime = string.Empty;
            if (Job.SyuKoYmd == Job.KikYmd) koban.KouZokPtnKbn = 1;
            else if (Job.UnkoJKbn != 3 && Job.UnkoJKbn != 4 && Job.SyuKoYmd != Job.KikYmd && Job.SyuKoYmd == koban.UnkYmd) koban.KouZokPtnKbn = 2;
            else if (Job.SyuKoYmd != Job.KikYmd && Job.SyuKoYmd != koban.UnkYmd && Job.KikYmd != koban.UnkYmd) koban.KouZokPtnKbn = 3;
            else if (Job.UnkoJKbn != 3 && Job.UnkoJKbn != 4 && Job.HaiSYmd != Job.TouYmd && Job.KikYmd == koban.UnkYmd) koban.KouZokPtnKbn = 4;
            else if ((Job.UnkoJKbn == 3 || Job.UnkoJKbn == 4) && Job.SyuKoYmd != Job.KikYmd)
            {
                if (Job.SyuKoYmd == koban.UnkYmd) koban.KouZokPtnKbn = 5;
                else if (Job.KikYmd == koban.UnkYmd) koban.KouZokPtnKbn = 6;
                else koban.KouZokPtnKbn = 9;
            }
            else koban.KouZokPtnKbn = 99;
            var koteik = listKoteik.FirstOrDefault(_ => _.Nittei == short.Parse(koban.UnkYmd.Substring(6)));
            var syuTaikinCalculationTime = listSyuTaikinCalculationTime.FirstOrDefault(_ => _.KouZokPtnKbn == koban.KouZokPtnKbn);
            if (koteik != null)
            {
                if (syuTaikinCalculationTime != null)
                {
                    setTime(koban, koteik.SyukoTime, koteik.KikTime, syuTaikinCalculationTime.SyukinCalculationTimeMinutes, syuTaikinCalculationTime.TaikinCalculationTimeMinutes);
                }
                else
                {
                    koban.SyukinTime = koteik.SyukoTime;
                    koban.TaiknTime = koteik.KikTime;
                }
            }
            else
            {
                switch (koban.KouZokPtnKbn)
                {
                    case 2:
                    case 5:
                        if (syuTaikinCalculationTime != null)
                            setTime(koban, haisha.SyuKoTime, string.Empty, syuTaikinCalculationTime.SyukinCalculationTimeMinutes, syuTaikinCalculationTime.TaikinCalculationTimeMinutes);
                        else
                            koban.SyukinTime = haisha.SyuKoTime;
                        koban.TaiknTime = "2359";
                        break;
                    case 3:
                    case 9:
                        koban.SyukinTime = "0000";
                        koban.TaiknTime = "2359";
                        break;
                    case 4:
                    case 6:
                        koban.SyukinTime = "0000";
                        if (syuTaikinCalculationTime != null)
                            setTime(koban, string.Empty, haisha.KikTime, syuTaikinCalculationTime.SyukinCalculationTimeMinutes, syuTaikinCalculationTime.TaikinCalculationTimeMinutes);
                        else
                            koban.TaiknTime = haisha.KikTime;
                        break;
                    default:
                        if (syuTaikinCalculationTime != null)
                        {
                            setTime(koban, haisha.SyuKoTime, haisha.KikTime, syuTaikinCalculationTime.SyukinCalculationTimeMinutes, syuTaikinCalculationTime.TaikinCalculationTimeMinutes);
                        }
                        else
                        {
                            koban.SyukinTime = haisha.SyuKoTime;
                            koban.TaiknTime = haisha.KikTime;
                        }
                        break;
                }
            }
        }

        private void setTime(TkdKoban koban, string SyukoTime, string KikTime, int SyukinCalculationTimeMinutes, int TaikinCalculationTimeMinutes)
        {
            DateTime temp = DateTime.ParseExact(koban.UnkYmd, DateTimeFormat.yyyyMMdd, CultureInfo.InvariantCulture);
            if (DateTime.TryParseExact(SyukoTime, DateTimeFormat.HHmm, CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
            {
                var calTime = temp.AddMinutes(-1 * SyukinCalculationTimeMinutes);
                if (calTime.CompareTo(temp) < 0) koban.SyukinYmd = calTime.ToString(DateTimeFormat.yyyyMMdd);
                koban.SyukinTime = temp.AddMinutes(-1 * SyukinCalculationTimeMinutes).ToString(DateTimeFormat.HHmm);
            }
            if (DateTime.TryParseExact(KikTime, DateTimeFormat.HHmm, CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
            {
                var calTime = temp.AddMinutes(TaikinCalculationTimeMinutes);
                if (calTime.CompareTo(temp) > 0) koban.TaikinYmd = calTime.ToString(DateTimeFormat.yyyyMMdd);
                koban.TaiknTime = temp.AddMinutes(TaikinCalculationTimeMinutes).ToString(DateTimeFormat.HHmm);
            }
        }

        public async Task<StaffHaitaCheck> GetHaitaCheck(string ukeNo, short unkRen, short teiDanNo, short bunkRen)
        {
            return await _mediator.Send(new GetHaitaCheckQuery() { UkeNo = ukeNo, UnkRen = unkRen, TeiDanNo = teiDanNo, BunkRen = bunkRen });
        }

        public async Task<string> GetHaitaCheckDelete(int syainCdSeq, string unkYmd)
        {
            return await _mediator.Send(new GetHaitaCheckDeleteQuery() { SyainCdSeq = syainCdSeq, UnkYmd = unkYmd });
        }
    }
}
