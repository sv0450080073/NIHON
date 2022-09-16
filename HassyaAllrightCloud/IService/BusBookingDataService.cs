using DevExpress.CodeParser;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain;
using MediatR;
using HassyaAllrightCloud.Application.BusSchedule.Queries;
using HassyaAllrightCloud.Application.BookingInput.Queries;

namespace HassyaAllrightCloud.IService
{
    public interface IBusBookingDataListService
    {
        Task<List<BookingData>> GetBookingData(string ukeNo, short UnkRen);
        Task<List<Driverlst>> Getbusdriver(string ukeNo, short UnkRen, short TeiDanNo, short BunkRen);
        Task<List<BusBookingData>> Getbusdatabooking(DateTime busstardate, DateTime busenddate, int YoyaKbnSeq, int TenantCdSeq);
        Task<List<Driverlst>> GetbusdriverbyHaiSha(string ukeNo, short UnkRen, short TeiDanNo, short BunkRen, string date);
        Task<List<JourneysData>> GetJourneysData(string ukeNo, short unkRen);      
        Task<List<BusBookingDataAllocation>> GetBusBookingDataAllocation(string DateSpecified, DateTime pickupDate, string startTime, string endTime, int bookingfrom, int bookingto, int ReservationClassification1, int ReservationClassification2, int VehicleAffiliation1, int VehicleAffiliation2, string UnprovisionedVehicle1, int UnprovisionedVehicle2, int tenantCdSeq);
        Task<BusBookingDataAllocation> GetBusBookingDataAllocationItem(string DateSpecified, DateTime pickupDate, string startTime, string endTime, int bookingfrom, int bookingto, int ReservationClassification1, int ReservationClassification2, int VehicleAffiliation1, int VehicleAffiliation2, string UnprovisionedVehicle1, int UnprovisionedVehicle2, int tenantCdSeq, int ukecd, int TeiDanNo,int UnkRen, int BunkRen);
        /// <summary>
        /// Get CodeKbn infor by code and tenantId
        /// </summary>
        /// <param name="code">Specify for <see cref="VpmCodeKb.CodeKbn"/></param>
        /// <param name="tenantId">Specify for <see cref="VpmCodeKb.TenantCdSeq"/></param>
        /// <returns>
        /// Result matched as <see cref="TPM_CodeKbData"/>.
        /// Not found => <see cref="null"/>
        /// </returns>
        Task<TPM_CodeKbData> GetCodeKbnByCodeAndTenantId(int code, int tenantId);
        Task<VpmRepair> GetrepairAndTenantId(int code, int tenantId);

        SijJoKnmData GetSijJoKnm(string SetteiCd, int TenantCdSeq);
        int Getcompany(string date, int syainCdSeq);
        Dictionary<string, string> GetFieldValues(BusScheduleFilter scheduleFilter);
        Task<BookingRemarkHaitaCheck> GetBookingRemarkHaiCheck(string UkeNo, short UnkRen);
        void UpdateUnkobi(string ukeno, short unkren, string FormNm);
        void UpdateHaiSha(string ukeno, short unkren, short bunkren, short TenantCdSeq, string FormNm, string date, bool isFromBusAllocation = false, bool updateKhinKbnOnly = false);
        void UpdateYyksho(string ukeno, int tenantID, string FormNm);
        void UpdateBunKSyuJyndata(string UkeNo, short UnkRen, short TeiDanNo, int userlogin);
        void ApplyFilter(ref BusScheduleFilter scheduleFilter, Dictionary<string, string> filterValues);
        int CheckGreenline(string ukeno, short unkren);
        Task<string> GetBikoNm(string ukeNo, bool isUnkobi, short unkRen);
    }
    public class BusBookingDataService : IBusBookingDataListService
    {
        private IMediator _mediatR;
        private readonly KobodbContext _dbContext;
        private readonly ITPM_CodeSyService _codeSyuService;
        private readonly ILogger<BusBookingDataService> _logger;
        private readonly BusScheduleHelper _helper;

        public BusBookingDataService(IMediator mediatR, KobodbContext context, BusScheduleHelper helper,
            ITPM_CodeSyService codeSyuService,
            ILogger<BusBookingDataService> logger)
        {
            _mediatR = mediatR;
            _dbContext = context;
            _codeSyuService = codeSyuService;
            _logger = logger;
            _helper = helper;

        }

        public async Task<List<BookingData>> GetBookingData(string ukeNo, short unkRen)
        {
            return (from TKD_Haisha in _dbContext.TkdHaisha
                    join TKD_Yyksho in _dbContext.TkdYyksho on TKD_Haisha.UkeNo equals TKD_Yyksho.UkeNo into TKD_Yyksho_join
                    from TKD_Yyksho in TKD_Yyksho_join.DefaultIfEmpty()
                    join TKD_Unkobi in _dbContext.TkdUnkobi
                          on new { TKD_Haisha.UkeNo, TKD_Haisha.UnkRen }
                      equals new { TKD_Unkobi.UkeNo, TKD_Unkobi.UnkRen } into TKD_Unkobi_join
                    from TKD_Unkobi in TKD_Unkobi_join.DefaultIfEmpty()
                    where
                      TKD_Haisha.UkeNo == ukeNo
                      && TKD_Haisha.UnkRen == unkRen
                      && TKD_Haisha.SiyoKbn == 1
                      && TKD_Yyksho.TenantCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().TenantID
                    select new BookingData
                    {
                         UkeNo=TKD_Haisha.UkeNo,
                         UnkRen=TKD_Haisha.UnkRen,
                         BunkRen=TKD_Haisha.BunkRen,
                         TeiDanNo=TKD_Haisha.TeiDanNo,
                         HaiSYmd=TKD_Haisha.HaiSymd
                    }).ToList();
        }
        public void UpdateBunKSyuJyndata(string UkeNo, short UnkRen, short TeiDanNo, int userlogin)
        {
            var updatecutline = _dbContext.TkdHaisha.Where(t => t.UkeNo == UkeNo && t.UnkRen == UnkRen && t.TeiDanNo == TeiDanNo && t.SiyoKbn == 1).OrderBy(t => t.HaiSymd).ThenBy(t => t.HaiStime).ToList();
            if (updatecutline.Count == 1)
            {
                var stdupdate = _dbContext.TkdHaisha.Find(updatecutline.First().UkeNo, updatecutline.First().UnkRen, updatecutline.First().TeiDanNo, updatecutline.First().BunkRen);
                stdupdate.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                stdupdate.UpdTime = DateTime.Now.ToString("HHmmss");
                stdupdate.UpdPrgId = "KU0100";
                stdupdate.UpdSyainCd = userlogin;
                stdupdate.HenKai = (short)(stdupdate.HenKai + 1);
                stdupdate.BunKsyuJyn = 0;
                _dbContext.Update(stdupdate);
                _dbContext.SaveChanges();
            }
            else
            {
                short i = 1;
                foreach (var item in updatecutline)
                {
                    var stdupdate = _dbContext.TkdHaisha.Find(item.UkeNo, item.UnkRen, item.TeiDanNo, item.BunkRen);
                    stdupdate.BunKsyuJyn = i;
                    stdupdate.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    stdupdate.UpdTime = DateTime.Now.ToString("HHmmss");
                    stdupdate.UpdPrgId = "KU0100";
                    stdupdate.UpdSyainCd = userlogin;
                    stdupdate.HenKai = (short)(stdupdate.HenKai + 1);
                    _dbContext.Update(stdupdate);
                    _dbContext.SaveChanges();
                    i++;
                }
            }
        }
        public void UpdateHaiSha(string ukeno, short unkren, short teiDanNo, short bunkren, string FormNm, string date, bool isFromBusAllocation = false, bool updateKhinKbnOnly = false)
        {
            //# 3069 work around
            var list = _dbContext.TkdHaisha.Where(h => h.UkeNo == ukeno && h.UnkRen == unkren && h.TeiDanNo == teiDanNo && h.SiyoKbn == 1).Select(h => new { h.SyaRyoUnc, h.SyaRyoSyo, h.SyaRyoTes, h.KikYmd, h.KikTime, h.TouYmd, h.TouChTime, h.BunkRen });
            var updateHaisha = _dbContext.TkdHaisha.Find(ukeno, unkren,teiDanNo,bunkren);
            if(isFromBusAllocation)
                _dbContext.Entry(updateHaisha).Reload();
            var obj = list.Where(h => h.BunkRen == bunkren).Single();
            //# 3069 work around
            var checkHaiIKbn = _dbContext.TkdHaiin.Where(t => t.UkeNo == ukeno && t.UnkRen == unkren && t.TeiDanNo==teiDanNo && t.BunkRen==bunkren && t.SiyoKbn==1).Select(t=>t.SyainCdSeq).ToList();
            
            int checkGuiSu = 0;
            int checkDrvJin = 0;
            foreach(var item in checkHaiIKbn)
            {
                var syokumuKbn = GetSyokumuCdSeq(date, item);
                if (syokumuKbn == 1|| syokumuKbn == 2)
                {
                    checkDrvJin++;
                }
                if(syokumuKbn == 3|| syokumuKbn == 4)
                {
                    checkGuiSu++;
                }    
            }

            if (isFromBusAllocation)
            {
                if (updateKhinKbnOnly)
                {
                    updateHaisha.KhinKbn = 3;
                    updateHaisha.HaiIkbn = 1;
                }
                else
                    updateHaisha.KhinKbn = updateHaisha.HaiIkbn = 3;

                if (checkHaiIKbn.Count() == 0)
                {
                    if (updateKhinKbnOnly)
                        updateHaisha.KhinKbn = 1;
                    else
                        updateHaisha.KhinKbn = updateHaisha.HaiIkbn = 1;
                }
                if (checkGuiSu >= updateHaisha.GuiSu && checkDrvJin >= updateHaisha.DrvJin)
                {
                    if (updateKhinKbnOnly)
                        updateHaisha.KhinKbn = 2;
                    else
                        updateHaisha.KhinKbn = updateHaisha.HaiIkbn = 2;
                }
            }
            else
            {
                updateHaisha.HaiIkbn = 3;

                if (checkHaiIKbn.Count() == 0)
                {
                    updateHaisha.HaiIkbn = 1;
                }
                if (checkGuiSu >= updateHaisha.GuiSu && checkDrvJin >= updateHaisha.DrvJin)
                {
                    updateHaisha.HaiIkbn = 2;
                }
            }
                
            updateHaisha.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            updateHaisha.UpdTime = DateTime.Now.ToString("HHmmss");
            updateHaisha.UpdPrgId = Common.UpdPrgId;
            updateHaisha.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
            updateHaisha.UpdPrgId = FormNm;
            updateHaisha.HenKai = updateHaisha.HenKai++;
            // #3069 work around
            updateHaisha.SyaRyoUnc = obj.SyaRyoUnc;
            updateHaisha.SyaRyoSyo = obj.SyaRyoSyo;
            updateHaisha.SyaRyoTes = obj.SyaRyoTes;
            updateHaisha.KikYmd = obj.KikYmd;
            updateHaisha.KikTime = obj.KikTime;
            updateHaisha.TouYmd = obj.TouYmd;
            updateHaisha.TouChTime = obj.TouChTime;
            // #3069 work around
            _dbContext.Update(updateHaisha);
            _dbContext.SaveChanges();
            
        }
        public void UpdateUnkobi(string ukeno, short unkren, string FormNm)
        {
            var updateUnkobi = _dbContext.TkdUnkobi.Find(ukeno, unkren);
             var checkHaiSKbn = _dbContext.TkdHaisha.Where(t => t.UkeNo == ukeno && t.UnkRen == unkren && t.SiyoKbn==1).Select(t => t.HaiSkbn).Distinct().ToList();
            updateUnkobi.HaiSkbn =3;
            if(checkHaiSKbn.Count()==1 && checkHaiSKbn.First()==1)
            {
                updateUnkobi.HaiSkbn =1;
            } 
            if(checkHaiSKbn.Count()==1 && checkHaiSKbn.First()==2)
            {
                updateUnkobi.HaiSkbn =2;
            }  
             var checkHaiIKbn = _dbContext.TkdHaisha.Where(t => t.UkeNo == ukeno && t.UnkRen == unkren && t.SiyoKbn==1).Select(t => t.HaiIkbn).Distinct().ToList();
            updateUnkobi.HaiIkbn = 3;
            if (checkHaiIKbn.Count() == 1 && checkHaiIKbn.First() == 1)
            {
                updateUnkobi.HaiIkbn = 1;
            }
            if (checkHaiIKbn.Count() == 1 && checkHaiIKbn.First() == 2)
            {
                updateUnkobi.HaiIkbn = 2;
            }
            var checkNippoKbn = _dbContext.TkdHaisha.Where(t => t.UkeNo == ukeno && t.UnkRen == unkren && t.SiyoKbn == 1).Select(t => t.NippoKbn).Distinct().ToList();
            updateUnkobi.NippoKbn = 3;
            if (checkNippoKbn.Count() == 1 && checkNippoKbn.First() == 1)
            {
                updateUnkobi.NippoKbn = 1;
            }
            if (checkNippoKbn.Count() == 1 && checkNippoKbn.First() == 2)
            {
                updateUnkobi.NippoKbn = 2;
            }
            var checkKSKbn = _dbContext.TkdHaisha.Where(t => t.UkeNo == ukeno && t.UnkRen == unkren && t.SiyoKbn == 1).Select(t => t.Kskbn).Distinct().ToList();
            updateUnkobi.Kskbn = 3;
            if (checkKSKbn.Count() == 1 && checkKSKbn.First() == 1)
            {
                updateUnkobi.Kskbn = 1;
            }
            if (checkKSKbn.Count() == 1 && checkKSKbn.First() == 2)
            {
                updateUnkobi.Kskbn = 2;
            }
            var checkYouKbn=_dbContext.TkdHaisha.Where(t => t.UkeNo == ukeno && t.UnkRen == unkren && t.SiyoKbn==1).Select(t => t.YouTblSeq).Distinct().ToList();
            updateUnkobi.YouKbn = 2;
            if (checkYouKbn.Count() == 1 && checkYouKbn.Contains(0))
            {
                updateUnkobi.YouKbn = 1;
            }
            else
            if (checkYouKbn.Count() >2 && checkYouKbn.Contains(0))
            {
                updateUnkobi.YouKbn = 1;
            }    
            updateUnkobi.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            updateUnkobi.UpdTime = DateTime.Now.ToString("HHmmss");
            updateUnkobi.UpdPrgId = Common.UpdPrgId;
            updateUnkobi.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
            updateUnkobi.UpdPrgId = FormNm;
            updateUnkobi.HenKai = updateUnkobi.HenKai++;
            _dbContext.TkdUnkobi.Update(updateUnkobi);
            _dbContext.SaveChanges();

        }

        public void UpdateYyksho(string ukeno, int tenantID,string FormNm)
        {
            var updateYyksho = _dbContext.TkdYyksho.Find(tenantID,ukeno);
           var checkHaiSKbn = _dbContext.TkdUnkobi.Where(t => t.UkeNo == ukeno && t.SiyoKbn==1).Select(t => t.HaiSkbn).Distinct().ToList();
            updateYyksho.HaiSkbn = 3;
            if (checkHaiSKbn.Count() == 1 && checkHaiSKbn.First() == 1)
            {
                updateYyksho.HaiSkbn = 1;
            }
            if (checkHaiSKbn.Count() == 1 && checkHaiSKbn.First() == 2)
            {
                updateYyksho.HaiSkbn = 2;
            }
             var checkHaiIKbn = _dbContext.TkdUnkobi.Where(t => t.UkeNo == ukeno && t.SiyoKbn==1).Select(t => t.HaiIkbn).Distinct().ToList();
            updateYyksho.HaiIkbn = 3;
            if (checkHaiIKbn.Count() == 1 && checkHaiIKbn.First() == 1)
            {
                updateYyksho.HaiIkbn = 1;
            }
            if (checkHaiIKbn.Count() == 1 && checkHaiIKbn.First() == 2)
            {
                updateYyksho.HaiIkbn = 2;
            }
            var checkNippoKbn = _dbContext.TkdUnkobi.Where(t => t.UkeNo == ukeno && t.SiyoKbn == 1).Select(t => t.NippoKbn).Distinct().ToList();
            updateYyksho.NippoKbn = 3;
            if (checkNippoKbn.Count() == 1 && checkNippoKbn.First() == 1)
            {
                updateYyksho.NippoKbn = 1;
            }
            if (checkNippoKbn.Count() == 1 && checkNippoKbn.First() == 2)
            {
                updateYyksho.NippoKbn = 2;
            }
            var checkKSKbn = _dbContext.TkdUnkobi.Where(t => t.UkeNo == ukeno && t.SiyoKbn == 1).Select(t => t.Kskbn).Distinct().ToList();
            updateYyksho.Kskbn = 3;
            if (checkKSKbn.Count() == 1 && checkKSKbn.First() == 1)
            {
                updateYyksho.Kskbn = 1;
            }
            if (checkKSKbn.Count() == 1 && checkKSKbn.First() == 2)
            {
                updateYyksho.Kskbn = 2;
            }
            var checkYouKbn = _dbContext.TkdHaisha.Where(t => t.UkeNo == ukeno && t.SiyoKbn == 1).Select(t => t.YouTblSeq).Distinct().ToList();
            updateYyksho.YouKbn = 2;
            if (checkYouKbn.Count() == 1 && checkYouKbn.Contains(0))
            {
                updateYyksho.YouKbn = 1;
            }
            else
            if (checkYouKbn.Count() > 2 && checkYouKbn.Contains(0))
            {
                updateYyksho.YouKbn = 1;
            }
            updateYyksho.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            updateYyksho.UpdTime = DateTime.Now.ToString("HHmmss");
            updateYyksho.UpdPrgId = Common.UpdPrgId;
            updateYyksho.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
            updateYyksho.HenKai = updateYyksho.HenKai++;
            updateYyksho.UpdPrgId = FormNm;
            _dbContext.TkdYyksho.Update(updateYyksho);
            _dbContext.SaveChanges();
        }
        public int GetSyokumuCdSeq(string date, int syainCdSeq)
        {
            return (from KYOSHE in _dbContext.VpmKyoShe
                    join SYOKUM in _dbContext.VpmSyokum on KYOSHE.SyokumuCdSeq equals SYOKUM.SyokumuCdSeq 
                    where string.Compare(KYOSHE.StaYmd, date) <= 0 &&
                                                         string.Compare(KYOSHE.EndYmd, date) >= 0 &&
                                                         KYOSHE.SyainCdSeq == syainCdSeq && SYOKUM.TenantCdSeq== new ClaimModel().TenantID
                    select new
                    {
                        SYOKUM.SyokumuKbn
                    }).First().SyokumuKbn;
        }
        public int Getcompany(string date, int syainCdSeq)
        {
            return (from KYOSHE in _dbContext.VpmKyoShe
                    join SYAIN in _dbContext.VpmSyain on KYOSHE.SyainCdSeq equals SYAIN.SyainCdSeq into SYAIN_join
                    from SYAIN in SYAIN_join.DefaultIfEmpty()
                    join EIGYOS in _dbContext.VpmEigyos on KYOSHE.EigyoCdSeq equals EIGYOS.EigyoCdSeq into EIGYOS_join
                    from EIGYOS in EIGYOS_join.DefaultIfEmpty()
                    join KAISHA in _dbContext.VpmCompny
                          on new { EIGYOS.CompanyCdSeq, TenantCdSeq = new ClaimModel().TenantID }
                             equals new
                             {
                                 KAISHA.CompanyCdSeq,
                                 KAISHA.TenantCdSeq
                             }
                    where
                                                         string.Compare(KYOSHE.StaYmd, date) <= 0 &&
                                                         string.Compare(KYOSHE.EndYmd, date) >= 0 &&
                                                         KYOSHE.SyainCdSeq == syainCdSeq
                    select new
                    {
                        KAISHA.CompanyCdSeq,
                    }).First().CompanyCdSeq;
        }

        public int CheckGreenline(string ukeno, short unkren)
        {
            return _dbContext.TkdHaisha.Where(t => t.UkeNo == ukeno && t.UnkRen == unkren && t.YouTblSeq != 0).Count();
        }
        /// <summary>
        /// get data Journey
        /// </summary>
        /// <param name="UkeNo"></param>
        /// <returns></returns>
        public async Task<List<JourneysData>> GetJourneysData(string ukeNo, short unkRen)
        {
            var tenantCdSeq = new ClaimModel().TenantID;
            return (from TKD_Haisha in _dbContext.TkdHaisha
                    join TKD_Yyksho in _dbContext.TkdYyksho on TKD_Haisha.UkeNo equals TKD_Yyksho.UkeNo into TKD_Yyksho_join
                    from TKD_Yyksho in TKD_Yyksho_join.DefaultIfEmpty()
                    join TKD_Unkobi in _dbContext.TkdUnkobi
                          on new { TKD_Haisha.UkeNo, TKD_Haisha.UnkRen }
                      equals new { TKD_Unkobi.UkeNo, TKD_Unkobi.UnkRen } into TKD_Unkobi_join
                    from TKD_Unkobi in TKD_Unkobi_join.DefaultIfEmpty()
                    where
                      TKD_Haisha.UkeNo == ukeNo
                      && TKD_Haisha.UnkRen==unkRen
                      && TKD_Haisha.SiyoKbn == 1
                      && TKD_Yyksho.TenantCdSeq == tenantCdSeq
                    orderby
                      TKD_Haisha.GoSya
                    select new JourneysData
                    {
                        Haisha_UkeNo = TKD_Haisha.UkeNo,
                        Yyksho_UkeCd = TKD_Yyksho.UkeCd,
                        Haisha_UnkRen = TKD_Haisha.UnkRen,
                        Haisha_SyaSyuRen = TKD_Haisha.SyaSyuRen,
                        Haisha_TeiDanNo = TKD_Haisha.TeiDanNo,
                        Haisha_BunkRen = TKD_Haisha.BunkRen,
                        Haisha_GoSya = TKD_Haisha.GoSya,
                        Haisha_BunKSyuJyn = TKD_Haisha.BunKsyuJyn,
                        Haisha_HaiSYmd = TKD_Haisha.HaiSymd,
                        Haisha_HaiSTime = TKD_Haisha.HaiStime,
                        Haisha_TouYmd = TKD_Haisha.TouYmd,
                        Haisha_SyuPaTime = TKD_Haisha.SyuPaTime,
                        Haisha_TouChTime = TKD_Haisha.TouChTime,
                        Haisha_SyukoTime = TKD_Haisha.SyuKoTime,
                        Haisha_KikTime = TKD_Haisha.KikTime,
                        Unkobi_TouChTime = TKD_Unkobi.TouChTime,
                        Unkobi_SyuPaTime = TKD_Unkobi.SyuPaTime,
                        Unkobi_HaiSTime = TKD_Unkobi.HaiStime,
                        Unkobi_SyukoTime = TKD_Unkobi.SyuKoTime,
                        Unkobi_KikTime = TKD_Unkobi.KikTime,
                        Unkobi_HaiSYmd = TKD_Unkobi.HaiSymd,
                        Unkobi_SyukoYmd = TKD_Unkobi.SyukoYmd,
                        Unkobi_KhakFlg = TKD_Unkobi.KhakFlg,
                        Unkobi_TouYmd = TKD_Unkobi.TouYmd,
                        Unkobi_ZenHaFlg = TKD_Unkobi.ZenHaFlg,
                        Unkobi_KikYmd = TKD_Unkobi.KikYmd,
                    }).ToList();
        }       
        /// <summary>
        /// get info driver by haisha
        /// </summary>
        /// <param name="UkeNo"></param>
        /// <param name="UnkRen"></param>
        /// <param name="TeiDanNo"></param>
        /// <param name="BunkRen"></param>
        /// <returns></returns>
        public async Task<List<Driverlst>> Getbusdriver(string ukeNo, short UnkRen, short TeiDanNo, short BunkRen)
        {
            return await _dbContext.TkdHaiin
                .Where(t => t.UkeNo == ukeNo && 
                t.UnkRen == UnkRen && 
                t.TeiDanNo == TeiDanNo && 
                t.BunkRen == BunkRen && 
                t.SiyoKbn == 1)
                .Select(
                t => new Driverlst 
                { HaiInRen = t.HaiInRen, 
                    SyainCdSeq = t.SyainCdSeq, 
                    SiyoKbn = t.SiyoKbn, 
                    StartComment = t.Syukinbasy, 
                    EndComment = t.TaiknBasy, 
                    StartTime = t.SyukinTime, 
                    EndTime = t.TaiknTime, 
                    StartTimestr = t.SyukinTime, 
                    EndTimestr = t.TaiknTime,
                    SyokumuKbn = t.SyokumuKbn
                }).ToListAsync();
        }

        /// <summary>
        /// get info driver by haisha
        /// </summary>
        /// <param name="UkeNo"></param>
        /// <param name="UnkRen"></param>
        /// <param name="TeiDanNo"></param>
        /// <param name="BunkRen"></param>
        /// <returns></returns>
        public async Task<List<Driverlst>> GetbusdriverbyHaiSha(string ukeNo, short UnkRen, short TeiDanNo, short BunkRen, string date)
        {
            return await _dbContext.TkdHaiin
                .Where(t => t.UkeNo == ukeNo &&
                t.UnkRen == UnkRen &&
                t.TeiDanNo == TeiDanNo &&
                t.BunkRen == BunkRen &&
                t.SiyoKbn == 1)
                .Select(
                t => new Driverlst
                {
                    HaiInRen = t.HaiInRen,
                    SyainCdSeq = t.SyainCdSeq,
                    SiyoKbn = t.SiyoKbn,
                    StartComment = t.Syukinbasy,
                    EndComment = t.TaiknBasy,
                    StartTime =t.SyukinTime,
                    EndTime = t.TaiknTime,
                    StartTimestr = t.SyukinTime,
                    EndTimestr = t.TaiknTime,
                    CompanyCdSeq = (from KYOSHE in _dbContext.VpmKyoShe
                                    join SYAIN in _dbContext.VpmSyain on KYOSHE.SyainCdSeq equals SYAIN.SyainCdSeq into SYAIN_join
                                    from SYAIN in SYAIN_join.DefaultIfEmpty()
                                    join EIGYOS in _dbContext.VpmEigyos on KYOSHE.EigyoCdSeq equals EIGYOS.EigyoCdSeq into EIGYOS_join
                                    from EIGYOS in EIGYOS_join.DefaultIfEmpty()
                                    join KAISHA in _dbContext.VpmCompny
                                          on new { EIGYOS.CompanyCdSeq, TenantCdSeq = new ClaimModel().TenantID }
                                      equals new { KAISHA.CompanyCdSeq, KAISHA.TenantCdSeq }
                                    where
                                     string.Compare(KYOSHE.StaYmd, date) <= 0 &&
                                     string.Compare(KYOSHE.EndYmd, date) >= 0 &&
                                     KYOSHE.SyainCdSeq==t.SyainCdSeq
                                    select new
                                    {
                                        KAISHA.CompanyCdSeq
                                    }).First().CompanyCdSeq
                }).ToListAsync();
        }
        /// <summary>
        /// get all data bus allocation
        /// </summary>
        /// <param name="DateSpecified"></param>
        /// <param name="pickupDate"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="bookingfrom"></param>
        /// <param name="bookingto"></param>
        /// <param name="ReservationClassification1"></param>
        /// <param name="ReservationClassification2"></param>
        /// <param name="VehicleAffiliation1"></param>
        /// <param name="VehicleAffiliation2"></param>
        /// <param name="UnprovisionedVehicle1"></param>
        /// <param name="UnprovisionedVehicle2"></param>
        /// <returns></returns>
        public async Task<List<BusBookingDataAllocation>> GetBusBookingDataAllocation(string DateSpecified, DateTime pickupDate, string startTime, string endTime, int bookingfrom, int bookingto, int ReservationClassification1, int ReservationClassification2, int VehicleAffiliation1, int VehicleAffiliation2, string UnprovisionedVehicle1, int UnprovisionedVehicle2, int tenantCdSeq)
        {
            var vpmSyasyu = from s in _dbContext.VpmSyaSyu where s.TenantCdSeq == tenantCdSeq select s;
            string codeSyu = "KATAKBN";

            Func<int, string, Task<List<BusBookingDataAllocation>>> get = (tenantId, codeSyu) => {
                var result =
                    (from TKD_Haisha in _dbContext.TkdHaisha
                     join TKD_Yyksho in _dbContext.TkdYyksho on TKD_Haisha.UkeNo equals TKD_Yyksho.UkeNo into TKD_Yyksho_join
                     from TKD_Yyksho in TKD_Yyksho_join.DefaultIfEmpty()
                     join TKD_Unkobi in _dbContext.TkdUnkobi
                           on new { TKD_Haisha.UkeNo, TKD_Haisha.UnkRen }
                       equals new { TKD_Unkobi.UkeNo, TKD_Unkobi.UnkRen } into TKD_Unkobi_join
                     from TKD_Unkobi in TKD_Unkobi_join.DefaultIfEmpty()
                     join TKD_YykSyu in _dbContext.TkdYykSyu
                           on new { TKD_Haisha.UkeNo, TKD_Haisha.UnkRen, TKD_Haisha.SyaSyuRen }
                       equals new { TKD_YykSyu.UkeNo, TKD_YykSyu.UnkRen, TKD_YykSyu.SyaSyuRen } into TKD_YykSyu_join
                     from TKD_YykSyu in TKD_YykSyu_join.DefaultIfEmpty()
                     join TPM_TokiSt in _dbContext.VpmTokiSt
                           on new { TKD_Yyksho.TokuiSeq, TKD_Yyksho.SitenCdSeq }
                       equals new { TPM_TokiSt.TokuiSeq, TPM_TokiSt.SitenCdSeq } into TPM_TokiSt_join
                     from TPM_TokiSt in TPM_TokiSt_join.DefaultIfEmpty()
                         //            join TKD_HaishaExp in _dbContext.TkdHaishaExp
                         //    on new { TKD_Haisha.UkeNo, TKD_Haisha.UnkRen, TKD_Haisha.TeiDanNo, TKD_Haisha.BunkRen }
                         //equals new { TKD_HaishaExp.UkeNo, TKD_HaishaExp.UnkRen, TKD_HaishaExp.TeiDanNo, TKD_HaishaExp.BunkRen } into TKD_HaishaExp_join
                         //            from TKD_HaishaExp in TKD_HaishaExp_join.DefaultIfEmpty()
                     join TPM_SyaRyo in _dbContext.VpmSyaRyo on new { SyaRyoCdSeq = TKD_Haisha.HaiSsryCdSeq } equals new { TPM_SyaRyo.SyaRyoCdSeq } into TPM_SyaRyo_join
                     from TPM_SyaRyo in TPM_SyaRyo_join.DefaultIfEmpty()
                     join TPM_SyaSyu_SyaRyo in vpmSyasyu on TPM_SyaRyo.SyaSyuCdSeq equals TPM_SyaSyu_SyaRyo.SyaSyuCdSeq into TPM_SyaSyu_SyaRyo_join
                     from TPM_SyaSyu_SyaRyo in TPM_SyaSyu_SyaRyo_join.DefaultIfEmpty()
                     join TPM_SyaSyu_YykSyu in vpmSyasyu on TKD_YykSyu.SyaSyuCdSeq equals TPM_SyaSyu_YykSyu.SyaSyuCdSeq into TPM_SyaSyu_YykSyu_join
                     from TPM_SyaSyu_YykSyu in TPM_SyaSyu_YykSyu_join.DefaultIfEmpty()
                     join TPM_HenSya in _dbContext.VpmHenSya on new { KSSyaRSeq = TKD_Haisha.KssyaRseq } equals new { KSSyaRSeq = TPM_HenSya.SyaRyoCdSeq } into TPM_HenSya_join
                     from TPM_HenSya in TPM_HenSya_join.DefaultIfEmpty()
                     join TPM_Eigyos in _dbContext.VpmEigyos on TPM_HenSya.EigyoCdSeq equals TPM_Eigyos.EigyoCdSeq into TPM_Eigyos_join
                     from TPM_Eigyos in TPM_Eigyos_join.DefaultIfEmpty()
                     join TPM_CodeKb in _dbContext.VpmCodeKb on Convert.ToString(TPM_SyaSyu_SyaRyo.KataKbn) equals TPM_CodeKb.CodeKbn into TPM_CodeKb_join
                     from TPM_CodeKb in TPM_CodeKb_join.DefaultIfEmpty()
                     join TPM_CodeKb_YykSyu in _dbContext.VpmCodeKb on Convert.ToString(TKD_YykSyu.KataKbn) equals TPM_CodeKb_YykSyu.CodeKbn into TPM_CodeKb2_join
                     from TPM_CodeKb_YykSyu in TPM_CodeKb2_join.DefaultIfEmpty()
                     where

                       (bookingto > 0 ? TKD_Yyksho.UkeCd >= bookingfrom && TKD_Yyksho.UkeCd <= bookingto : TKD_Yyksho.UkeNo != null && TKD_Yyksho.UkeNo != null) &&
                       (bookingto != bookingfrom ? DateSpecified == "配車年月日" ? TKD_Unkobi.HaiSymd == pickupDate.ToString("yyyyMMdd") &&
                       (
                       TKD_Unkobi.HaiStime == startTime ? 0 :
                       string.Compare(TKD_Unkobi.HaiStime, startTime) > 0 ? 1 :
                       string.Compare(TKD_Unkobi.HaiStime, startTime) < 0 ? (long?)-1 : null) >= 0 &&
                       (
                       TKD_Unkobi.HaiStime == endTime ? 0 :
                       string.Compare(TKD_Unkobi.HaiStime, endTime) > 0 ? 1 :
                       string.Compare(TKD_Unkobi.HaiStime, endTime) < 0 ? (long?)-1 : null) <= 0 :

                       TKD_Unkobi.TouYmd == pickupDate.ToString("yyyyMMdd") &&
                       (
                       TKD_Unkobi.TouChTime == startTime ? 0 :
                       string.Compare(TKD_Unkobi.TouChTime, startTime) > 0 ? 1 :
                       string.Compare(TKD_Unkobi.TouChTime, startTime) < 0 ? (long?)-1 : null) >= 0 &&
                       (
                       TKD_Unkobi.TouChTime == endTime ? 0 :
                       string.Compare(TKD_Unkobi.TouChTime, endTime) > 0 ? 1 :
                       string.Compare(TKD_Unkobi.TouChTime, endTime) < 0 ? (long?)-1 : null) <= 0 : TKD_Yyksho.UkeCd >= bookingfrom && TKD_Yyksho.UkeCd <= bookingto)
                       &&
                       TPM_CodeKb.CodeSyu == codeSyu &&
                       TPM_CodeKb_YykSyu.CodeSyu == codeSyu &&
                       (ReservationClassification2 > 0 ? TKD_Yyksho.YoyaKbnSeq >= ReservationClassification1 && TKD_Yyksho.YoyaKbnSeq <= ReservationClassification2 : TKD_Yyksho.YoyaKbnSeq != null && TKD_Yyksho.YoyaKbnSeq != null) &&
                       (VehicleAffiliation1 > 0 ? TPM_Eigyos.CompanyCdSeq == VehicleAffiliation1 : TPM_Eigyos.CompanyCdSeq != null) &&
                       (VehicleAffiliation2 > 0 ? TPM_Eigyos.EigyoCdSeq == 1 : TPM_Eigyos.EigyoCdSeq != null) &&
                       (UnprovisionedVehicle1 == "無" ? TKD_Haisha.Kskbn != 1 : TKD_Haisha.Kskbn != null) &&
                       TKD_Yyksho.TenantCdSeq == tenantCdSeq &&
                    //    TPM_SyaSyu_SyaRyo.TenantCdSeq == tenantId &&
                    //    TPM_SyaSyu_YykSyu.TenantCdSeq == tenantId
                    //    &&
                       TKD_Yyksho.YoyaSyu == 1 &&
                       TKD_Haisha.SiyoKbn == 1 &&
                       TPM_Eigyos.SiyoKbn == 1 &&
                       TKD_Unkobi.SiyoKbn == 1 &&
                       TKD_YykSyu.SiyoKbn == 1 &&
                       TKD_Yyksho.SiyoKbn == 1 &&
                       TPM_CodeKb.TenantCdSeq == tenantId
                     select new BusBookingDataAllocation
                     {
                         Yyksho_UkeCd = TKD_Yyksho.UkeCd,
                         Haisha_UkeNo = TKD_Haisha.UkeNo,
                         Haisha_UnkRen = TKD_Haisha.UnkRen,
                         Haisha_SyaSyuRen = TKD_Haisha.SyaSyuRen,
                         Haisha_TeiDanNo = TKD_Haisha.TeiDanNo,
                         Haisha_BunkRen = TKD_Haisha.BunkRen,
                         Haisha_GoSya = int.Parse(TKD_Haisha.GoSya),
                         Haisha_HaiSSryCdSeq = TKD_Haisha.HaiSsryCdSeq,
                         Haisha_SyuEigCdSeq = TKD_Haisha.SyuEigCdSeq,
                         Haisha_KikEigSeq = TKD_Haisha.KikEigSeq,
                         Haisha_IkNm = TKD_Haisha.IkNm,
                         Haisha_IkMapCdSeq = TKD_Haisha.IkMapCdSeq,
                         Haisha_SyuKoYmd = TKD_Haisha.SyuKoYmd,
                         Haisha_SyuKoTime = TKD_Haisha.SyuKoTime,
                         Haisha_SyuPaTime = TKD_Haisha.SyuPaTime,
                         Haisha_HaiSYmd = TKD_Haisha.HaiSymd,
                         Haisha_HaiSTime = TKD_Haisha.HaiStime,
                         Haisha_HaiSCdSeq = TKD_Haisha.HaiScdSeq,
                         Haisha_HaiSNm = TKD_Haisha.HaiSnm,
                         Haisha_HaiSJyus1 = TKD_Haisha.HaiSjyus1,
                         Haisha_HaiSJyus2 = TKD_Haisha.HaiSjyus2,
                         Haisha_HaiSKigou = TKD_Haisha.HaiSkigou,
                         Haisha_HaiSKouKCdSeq = TKD_Haisha.HaiSkouKcdSeq,
                         Haisha_HaiSBinCdSeq = TKD_Haisha.HaiSbinCdSeq,
                         Haisha_HaiSSetTime = _helper.ConvertTime(TKD_Haisha.HaiSsetTime),
                         Haisha_KikYmd = TKD_Haisha.KikYmd,
                         Haisha_KikTime = TKD_Haisha.KikTime,
                         Haisha_TouYmd = TKD_Haisha.TouYmd,
                         Haisha_TouChTime = TKD_Haisha.TouChTime,
                         Haisha_TouCdSeq = TKD_Haisha.TouCdSeq,
                         Haisha_TouNm = TKD_Haisha.TouNm,
                         Haisha_TouJyusyo1 = TKD_Haisha.TouJyusyo1,
                         Haisha_TouJyusyo2 = TKD_Haisha.TouJyusyo2,
                         Haisha_TouKigou = TKD_Haisha.TouKigou,
                         Haisha_TouKouKCdSeq = TKD_Haisha.TouKouKcdSeq,
                         Haisha_TouBinCdSeq = TKD_Haisha.TouBinCdSeq,
                         Haisha_TouSetTime = _helper.ConvertTime(TKD_Haisha.TouSetTime),
                         Haisha_JyoSyaJin = TKD_Haisha.JyoSyaJin,
                         Haisha_PlusJin = TKD_Haisha.PlusJin,
                         Haisha_DrvJin = TKD_Haisha.DrvJin,
                         Haisha_GuiSu = TKD_Haisha.GuiSu,
                         Haisha_OthJinKbn1 = TKD_Haisha.OthJinKbn1,
                         Haisha_OthJin1 = TKD_Haisha.OthJin1,
                         Haisha_OthJinKbn2 = TKD_Haisha.OthJinKbn2,
                         Haisha_OthJin2 = TKD_Haisha.OthJin2,
                         Haisha_PlatNo = TKD_Haisha.PlatNo,
                         TokiSt_RyakuNm = TPM_TokiSt.RyakuNm,
                         Yyksho_TokuiSeq = TKD_Yyksho.TokuiSeq,
                         Haisha_DanTaNm2 = TKD_Haisha.DanTaNm2,
                         Unkobi_DanTaNm = TKD_Unkobi.DanTaNm,
                         Unkobi_HaiSYmd = TKD_Unkobi.HaiSymd,
                         Unkobi_HaiSTime = TKD_Unkobi.HaiStime,
                         Unkobi_TouYmd = TKD_Unkobi.TouYmd,
                         Unkobi_TouChTime = TKD_Unkobi.TouChTime,
                         SyaRyo_SyaRyoCd = TPM_SyaRyo.SyaRyoCd,
                         SyaRyo_SyaRyoNm = TPM_SyaRyo.SyaRyoNm,
                         SyaRyo_SyainCdSeq = TPM_SyaRyo.SyainCdSeq,
                         SyaSyu_SyaSyuNm = TPM_SyaSyu_SyaRyo.SyaSyuNm,
                         YykSyu_SyaSyuCdSeq = TPM_SyaSyu_SyaRyo.SyaSyuCdSeq,
                         YykSyu_KataKbn = TPM_SyaSyu_SyaRyo.KataKbn,
                         SyaSyu_SyaSyuNm_YykSyu = TPM_SyaSyu_YykSyu.SyaSyuNm,
                         YykSyu_SyaSyuCdSeq_YykSyu = TPM_SyaSyu_YykSyu.SyaSyuCdSeq,
                         YykSyu_KataKbn_YykSyu = TPM_SyaSyu_YykSyu.KataKbn,
                         YykSyu_SyaSyuDai = TKD_YykSyu.SyaSyuDai,
                         CodeKb_RyakuNm = TPM_CodeKb.RyakuNm,
                         CodeKb_YykSyu_RyakuNm = TPM_CodeKb_YykSyu.RyakuNm,
                         Eigyos_RyakuNm = TPM_Eigyos.RyakuNm,
                         HaishaExp_HaiSKouKNm = TKD_Haisha.HaiSkouKnm,
                         HaishaExp_HaisBinNm = TKD_Haisha.HaiSbinNm,
                         HaishaExp_TouSKouKNm = TKD_Haisha.TouSkouKnm,
                         HaishaExp_TouSBinNm = TKD_Haisha.TouSbinNm,
                         HenSya_TenkoNo = TPM_HenSya.TenkoNo
                     }).Distinct().AsEnumerable().Select((r, i) => new BusBookingDataAllocation
                     {
                         row = i,
                         Yyksho_UkeCd = r.Yyksho_UkeCd,
                         Haisha_UkeNo = r.Haisha_UkeNo,
                         Haisha_UnkRen = r.Haisha_UnkRen,
                         Haisha_SyaSyuRen = r.Haisha_SyaSyuRen,
                         Haisha_TeiDanNo = r.Haisha_TeiDanNo,
                         Haisha_BunkRen = r.Haisha_BunkRen,
                         Haisha_GoSya = r.Haisha_GoSya,
                         Haisha_HaiSSryCdSeq = r.Haisha_HaiSSryCdSeq,
                         Haisha_SyuEigCdSeq = r.Haisha_SyuEigCdSeq,
                         Haisha_KikEigSeq = r.Haisha_KikEigSeq,
                         Haisha_IkNm = r.Haisha_IkNm,
                         Haisha_IkMapCdSeq = r.Haisha_IkMapCdSeq,
                         Haisha_SyuKoYmd = r.Haisha_SyuKoYmd,
                         Haisha_SyuKoTime = r.Haisha_SyuKoTime,
                         Haisha_SyuPaTime = r.Haisha_SyuPaTime,
                         Haisha_HaiSYmd = r.Haisha_HaiSYmd,
                         Haisha_HaiSTime = r.Haisha_HaiSTime,
                         Haisha_HaiSCdSeq = r.Haisha_HaiSCdSeq,
                         Haisha_HaiSNm = r.Haisha_HaiSNm,
                         Haisha_HaiSJyus1 = r.Haisha_HaiSJyus1,
                         Haisha_HaiSJyus2 = r.Haisha_HaiSJyus2,
                         Haisha_HaiSKigou = r.Haisha_HaiSKigou,
                         Haisha_HaiSKouKCdSeq = r.Haisha_HaiSBinCdSeq,
                         Haisha_HaiSBinCdSeq = r.Haisha_HaiSBinCdSeq,
                         Haisha_HaiSSetTime = r.Haisha_HaiSSetTime,
                         Haisha_KikYmd = r.Haisha_KikYmd,
                         Haisha_KikTime = r.Haisha_KikTime,
                         Haisha_TouYmd = r.Haisha_TouYmd,
                         Haisha_TouChTime = r.Haisha_TouChTime,
                         Haisha_TouCdSeq = r.Haisha_TouCdSeq,
                         Haisha_TouNm = r.Haisha_TouNm,
                         Haisha_TouJyusyo1 = r.Haisha_TouJyusyo1,
                         Haisha_TouJyusyo2 = r.Haisha_TouJyusyo2,
                         Haisha_TouKigou = r.Haisha_TouKigou,
                         Haisha_TouKouKCdSeq = r.Haisha_TouBinCdSeq,
                         Haisha_TouBinCdSeq = r.Haisha_TouBinCdSeq,
                         Haisha_TouSetTime = r.Haisha_TouSetTime,
                         Haisha_JyoSyaJin = r.Haisha_JyoSyaJin,
                         Haisha_PlusJin = r.Haisha_PlusJin,
                         Haisha_DrvJin = r.Haisha_DrvJin,
                         Haisha_GuiSu = r.Haisha_GuiSu,
                         Haisha_OthJinKbn1 = r.Haisha_OthJinKbn1,
                         Haisha_OthJin1 = r.Haisha_OthJin1,
                         Haisha_OthJinKbn2 = r.Haisha_OthJinKbn2,
                         Haisha_OthJin2 = r.Haisha_OthJin2,
                         Haisha_PlatNo = r.Haisha_PlatNo,
                         Yyksho_TokuiSeq = r.Yyksho_TokuiSeq,
                         Haisha_DanTaNm2 = r.Haisha_DanTaNm2,
                         TokiSt_RyakuNm = r.TokiSt_RyakuNm,
                         Unkobi_DanTaNm = r.Unkobi_DanTaNm,
                         Unkobi_HaiSYmd = r.Unkobi_HaiSYmd,
                         Unkobi_HaiSTime = r.Unkobi_HaiSTime,
                         Unkobi_TouYmd = r.Unkobi_TouYmd,
                         Unkobi_TouChTime = r.Unkobi_TouChTime,
                         SyaRyo_SyaRyoCd = r.SyaRyo_SyaRyoCd,
                         SyaRyo_SyaRyoNm = r.SyaRyo_SyaRyoNm,
                         SyaRyo_SyainCdSeq = r.SyaRyo_SyainCdSeq,
                         SyaSyu_SyaSyuNm = r.SyaSyu_SyaSyuNm,
                         YykSyu_SyaSyuCdSeq = r.YykSyu_SyaSyuCdSeq,
                         YykSyu_KataKbn = r.YykSyu_KataKbn,
                         SyaSyu_SyaSyuNm_YykSyu = r.SyaSyu_SyaSyuNm_YykSyu,
                         YykSyu_SyaSyuCdSeq_YykSyu = r.YykSyu_SyaSyuCdSeq_YykSyu,
                         YykSyu_KataKbn_YykSyu = r.YykSyu_KataKbn_YykSyu,
                         YykSyu_SyaSyuDai = r.YykSyu_SyaSyuDai,
                         CodeKb_RyakuNm = r.CodeKb_RyakuNm,
                         CodeKb_YykSyu_RyakuNm = r.CodeKb_RyakuNm,
                         Eigyos_RyakuNm = r.Eigyos_RyakuNm,
                         HaishaExp_HaiSKouKNm = r.HaishaExp_HaiSKouKNm,
                         HaishaExp_HaisBinNm = r.HaishaExp_HaisBinNm,
                         HaishaExp_TouSKouKNm = r.HaishaExp_TouSKouKNm,
                         HaishaExp_TouSBinNm = r.HaishaExp_TouSBinNm,
                         HenSya_TenkoNo = r.HenSya_TenkoNo,
                         DepartureTime = _helper.ConvertTime(r.Haisha_SyuKoTime),
                         DeliveryTime = _helper.ConvertTime(r.Haisha_HaiSTime),
                         ArrivalTime = _helper.ConvertTime(r.Haisha_TouChTime),
                         ReturnTime = _helper.ConvertTime(r.Haisha_KikTime),
                         StartTime = r.Haisha_SyuPaTime != "    " ? _helper.ConvertTime(r.Haisha_SyuPaTime) : _helper.ConvertTime(r.Haisha_SyuKoTime)
                     }).ToList();
                return Task.FromResult(result);
            };

            return await _codeSyuService.FilterTenantIdByCodeSyu(get, tenantCdSeq, codeSyu);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DateSpecified"></param>
        /// <param name="pickupDate"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="bookingfrom"></param>
        /// <param name="bookingto"></param>
        /// <param name="ReservationClassification1"></param>
        /// <param name="ReservationClassification2"></param>
        /// <param name="VehicleAffiliation1"></param>
        /// <param name="VehicleAffiliation2"></param>
        /// <param name="UnprovisionedVehicle1"></param>
        /// <param name="UnprovisionedVehicle2"></param>
        /// <param name="tenantCdSeq"></param>
        /// <returns></returns>
        public async Task<BusBookingDataAllocation> GetBusBookingDataAllocationItem(string DateSpecified, DateTime pickupDate, string startTime, string endTime, int bookingfrom, int bookingto, int ReservationClassification1, int ReservationClassification2, int VehicleAffiliation1, int VehicleAffiliation2, string UnprovisionedVehicle1, int UnprovisionedVehicle2, int tenantCdSeq, int ukecd,int TeiDanNo,int UnkRen, int BunkRen)
        {
            var vpmSyasyu = from s in _dbContext.VpmSyaSyu where s.TenantCdSeq == tenantCdSeq select s;
            string codeSyu = "KATAKBN";

            Func<int, string, Task<List<BusBookingDataAllocation>>> get = (tenantId, codeSyu) => {

                var result =
                    (from TKD_Haisha in _dbContext.TkdHaisha
                     join TKD_Yyksho in _dbContext.TkdYyksho on TKD_Haisha.UkeNo equals TKD_Yyksho.UkeNo into TKD_Yyksho_join
                     from TKD_Yyksho in TKD_Yyksho_join.DefaultIfEmpty()
                     join TKD_Unkobi in _dbContext.TkdUnkobi
                           on new { TKD_Haisha.UkeNo, TKD_Haisha.UnkRen }
                       equals new { TKD_Unkobi.UkeNo, TKD_Unkobi.UnkRen } into TKD_Unkobi_join
                     from TKD_Unkobi in TKD_Unkobi_join.DefaultIfEmpty()
                     join TKD_YykSyu in _dbContext.TkdYykSyu
                           on new { TKD_Haisha.UkeNo, TKD_Haisha.UnkRen, TKD_Haisha.SyaSyuRen }
                       equals new { TKD_YykSyu.UkeNo, TKD_YykSyu.UnkRen, TKD_YykSyu.SyaSyuRen } into TKD_YykSyu_join
                     from TKD_YykSyu in TKD_YykSyu_join.DefaultIfEmpty()
                     join TPM_TokiSt in _dbContext.VpmTokiSt
                           on new { TKD_Yyksho.TokuiSeq, TKD_Yyksho.SitenCdSeq }
                       equals new { TPM_TokiSt.TokuiSeq, TPM_TokiSt.SitenCdSeq } into TPM_TokiSt_join
                     from TPM_TokiSt in TPM_TokiSt_join.DefaultIfEmpty()
                         //            join TKD_HaishaExp in _dbContext.TkdHaishaExp
                         //    on new { TKD_Haisha.UkeNo, TKD_Haisha.UnkRen, TKD_Haisha.TeiDanNo, TKD_Haisha.BunkRen }
                         //equals new { TKD_HaishaExp.UkeNo, TKD_HaishaExp.UnkRen, TKD_HaishaExp.TeiDanNo, TKD_HaishaExp.BunkRen } into TKD_HaishaExp_join
                         //            from TKD_HaishaExp in TKD_HaishaExp_join.DefaultIfEmpty()
                     join TPM_SyaRyo in _dbContext.VpmSyaRyo on new { SyaRyoCdSeq = TKD_Haisha.HaiSsryCdSeq } equals new { TPM_SyaRyo.SyaRyoCdSeq } into TPM_SyaRyo_join
                     from TPM_SyaRyo in TPM_SyaRyo_join.DefaultIfEmpty()
                     join TPM_SyaSyu_SyaRyo in vpmSyasyu on TPM_SyaRyo.SyaSyuCdSeq equals TPM_SyaSyu_SyaRyo.SyaSyuCdSeq into TPM_SyaSyu_SyaRyo_join
                     from TPM_SyaSyu_SyaRyo in TPM_SyaSyu_SyaRyo_join.DefaultIfEmpty()
                     join TPM_SyaSyu_YykSyu in vpmSyasyu on TKD_YykSyu.SyaSyuCdSeq equals TPM_SyaSyu_YykSyu.SyaSyuCdSeq into TPM_SyaSyu_YykSyu_join
                     from TPM_SyaSyu_YykSyu in TPM_SyaSyu_YykSyu_join.DefaultIfEmpty()
                     join TPM_HenSya in _dbContext.VpmHenSya on new { KSSyaRSeq = TKD_Haisha.KssyaRseq } equals new { KSSyaRSeq = TPM_HenSya.SyaRyoCdSeq } into TPM_HenSya_join
                     from TPM_HenSya in TPM_HenSya_join.DefaultIfEmpty()
                     join TPM_Eigyos in _dbContext.VpmEigyos on TPM_HenSya.EigyoCdSeq equals TPM_Eigyos.EigyoCdSeq into TPM_Eigyos_join
                     from TPM_Eigyos in TPM_Eigyos_join.DefaultIfEmpty()
                     join TPM_CodeKb in _dbContext.VpmCodeKb on Convert.ToString(TPM_SyaSyu_SyaRyo.KataKbn) equals TPM_CodeKb.CodeKbn into TPM_CodeKb_join
                     from TPM_CodeKb in TPM_CodeKb_join.DefaultIfEmpty()
                     join TPM_CodeKb_YykSyu in _dbContext.VpmCodeKb on Convert.ToString(TKD_YykSyu.KataKbn) equals TPM_CodeKb_YykSyu.CodeKbn into TPM_CodeKb2_join
                     from TPM_CodeKb_YykSyu in TPM_CodeKb2_join.DefaultIfEmpty()
                     join TPM_Tokisk in _dbContext.VpmTokisk on TKD_Yyksho.TokuiSeq equals TPM_Tokisk.TokuiSeq
                     into TPM_Tokisk_join
                     from TPM_Tokisk in TPM_Tokisk_join.DefaultIfEmpty()
                     where

                       (bookingto > 0 ? TKD_Yyksho.UkeCd >= bookingfrom && TKD_Yyksho.UkeCd <= bookingto : TKD_Yyksho.UkeNo != null && TKD_Yyksho.UkeNo != null) &&
                       (bookingto != bookingfrom ? DateSpecified == "配車年月日" ? TKD_Unkobi.HaiSymd == pickupDate.ToString("yyyyMMdd") &&
                       (
                       TKD_Unkobi.HaiStime == startTime ? 0 :
                       string.Compare(TKD_Unkobi.HaiStime, startTime) > 0 ? 1 :
                       string.Compare(TKD_Unkobi.HaiStime, startTime) < 0 ? (long?)-1 : null) >= 0 &&
                       (
                       TKD_Unkobi.HaiStime == endTime ? 0 :
                       string.Compare(TKD_Unkobi.HaiStime, endTime) > 0 ? 1 :
                       string.Compare(TKD_Unkobi.HaiStime, endTime) < 0 ? (long?)-1 : null) <= 0 :

                       TKD_Unkobi.TouYmd == pickupDate.ToString("yyyyMMdd") &&
                       (
                       TKD_Unkobi.TouChTime == startTime ? 0 :
                       string.Compare(TKD_Unkobi.TouChTime, startTime) > 0 ? 1 :
                       string.Compare(TKD_Unkobi.TouChTime, startTime) < 0 ? (long?)-1 : null) >= 0 &&
                       (
                       TKD_Unkobi.TouChTime == endTime ? 0 :
                       string.Compare(TKD_Unkobi.TouChTime, endTime) > 0 ? 1 :
                       string.Compare(TKD_Unkobi.TouChTime, endTime) < 0 ? (long?)-1 : null) <= 0 : TKD_Yyksho.UkeCd >= bookingfrom && TKD_Yyksho.UkeCd <= bookingto)
                       &&
                       TPM_CodeKb.CodeSyu == codeSyu &&
                       TPM_CodeKb_YykSyu.CodeSyu == codeSyu &&
                       (ReservationClassification2 > 0 ? TKD_Yyksho.YoyaKbnSeq >= ReservationClassification1 && TKD_Yyksho.YoyaKbnSeq <= ReservationClassification2 : TKD_Yyksho.YoyaKbnSeq != null && TKD_Yyksho.YoyaKbnSeq != null) &&
                       (VehicleAffiliation1 > 0 ? TPM_Eigyos.CompanyCdSeq == VehicleAffiliation1 : TPM_Eigyos.CompanyCdSeq != null) &&
                       (VehicleAffiliation2 > 0 ? TPM_Eigyos.EigyoCdSeq == 1 : TPM_Eigyos.EigyoCdSeq != null) &&
                       (UnprovisionedVehicle1 == "無" ? TKD_Haisha.Kskbn != 1 : TKD_Haisha.Kskbn != null) &&
                       TKD_Yyksho.TenantCdSeq == tenantCdSeq
                       &&
                       TKD_Yyksho.YoyaSyu == 1 &&
                       TKD_Haisha.SiyoKbn == 1 &&
                       TPM_Eigyos.SiyoKbn == 1 &&
                       TKD_Unkobi.SiyoKbn == 1 &&
                       TKD_YykSyu.SiyoKbn == 1 &&
                       TKD_Yyksho.SiyoKbn == 1 &&
                       TKD_Yyksho.UkeCd == ukecd &&
                       TKD_Haisha.TeiDanNo == TeiDanNo &&
                       TKD_Haisha.BunkRen == BunkRen &&
                       TKD_Haisha.UnkRen == UnkRen &&
                       TPM_CodeKb.TenantCdSeq == tenantId //&&
                    //    TPM_SyaSyu_SyaRyo.TenantCdSeq==tenantId&&
                    //    TPM_SyaSyu_YykSyu.TenantCdSeq==tenantId
                     select new BusBookingDataAllocation
                     {
                         Yyksho_UkeCd = TKD_Yyksho.UkeCd,
                         Haisha_UkeNo = TKD_Haisha.UkeNo,
                         Haisha_UnkRen = TKD_Haisha.UnkRen,
                         Haisha_SyaSyuRen = TKD_Haisha.SyaSyuRen,
                         Haisha_TeiDanNo = TKD_Haisha.TeiDanNo,
                         Haisha_BunkRen = TKD_Haisha.BunkRen,
                         Haisha_GoSya = int.Parse(TKD_Haisha.GoSya),
                         Haisha_HaiSSryCdSeq = TKD_Haisha.HaiSsryCdSeq,
                         Haisha_SyuEigCdSeq = TKD_Haisha.SyuEigCdSeq,
                         Haisha_KikEigSeq = TKD_Haisha.KikEigSeq,
                         Haisha_IkNm = TKD_Haisha.IkNm,
                         Haisha_IkMapCdSeq = TKD_Haisha.IkMapCdSeq,
                         Haisha_SyuKoYmd = TKD_Haisha.SyuKoYmd,
                         Haisha_SyuKoTime = TKD_Haisha.SyuKoTime,
                         Haisha_SyuPaTime = TKD_Haisha.SyuPaTime,
                         Haisha_HaiSYmd = TKD_Haisha.HaiSymd,
                         Haisha_HaiSTime = TKD_Haisha.HaiStime,
                         Haisha_HaiSCdSeq = TKD_Haisha.HaiScdSeq,
                         Haisha_HaiSNm = TKD_Haisha.HaiSnm,
                         Haisha_HaiSJyus1 = TKD_Haisha.HaiSjyus1,
                         Haisha_HaiSJyus2 = TKD_Haisha.HaiSjyus2,
                         Haisha_HaiSKigou = TKD_Haisha.HaiSkigou,
                         Haisha_HaiSKouKCdSeq = TKD_Haisha.HaiSkouKcdSeq,
                         Haisha_HaiSBinCdSeq = TKD_Haisha.HaiSbinCdSeq,
                         Haisha_HaiSSetTime = _helper.ConvertTime(TKD_Haisha.HaiSsetTime),
                         Haisha_KikYmd = TKD_Haisha.KikYmd,
                         Haisha_KikTime = TKD_Haisha.KikTime,
                         Haisha_TouYmd = TKD_Haisha.TouYmd,
                         Haisha_TouChTime = TKD_Haisha.TouChTime,
                         Haisha_TouCdSeq = TKD_Haisha.TouCdSeq,
                         Haisha_TouNm = TKD_Haisha.TouNm,
                         Haisha_TouJyusyo1 = TKD_Haisha.TouJyusyo1,
                         Haisha_TouJyusyo2 = TKD_Haisha.TouJyusyo2,
                         Haisha_TouKigou = TKD_Haisha.TouKigou,
                         Haisha_TouKouKCdSeq = TKD_Haisha.TouKouKcdSeq,
                         Haisha_TouBinCdSeq = TKD_Haisha.TouBinCdSeq,
                         Haisha_TouSetTime = _helper.ConvertTime(TKD_Haisha.TouSetTime),
                         Haisha_JyoSyaJin = TKD_Haisha.JyoSyaJin,
                         Haisha_PlusJin = TKD_Haisha.PlusJin,
                         Haisha_DrvJin = TKD_Haisha.DrvJin,
                         Haisha_GuiSu = TKD_Haisha.GuiSu,
                         Haisha_OthJinKbn1 = TKD_Haisha.OthJinKbn1,
                         Haisha_OthJin1 = TKD_Haisha.OthJin1,
                         Haisha_OthJinKbn2 = TKD_Haisha.OthJinKbn2,
                         Haisha_OthJin2 = TKD_Haisha.OthJin2,
                         Haisha_PlatNo = TKD_Haisha.PlatNo,
                         TokiSt_RyakuNm = TPM_TokiSt.RyakuNm,
                         Yyksho_TokuiSeq = TKD_Yyksho.TokuiSeq,
                         Haisha_DanTaNm2 = TKD_Haisha.DanTaNm2,
                         Unkobi_DanTaNm = TKD_Unkobi.DanTaNm,
                         Unkobi_HaiSYmd = TKD_Unkobi.HaiSymd,
                         Unkobi_HaiSTime = TKD_Unkobi.HaiStime,
                         Unkobi_TouYmd = TKD_Unkobi.TouYmd,
                         Unkobi_TouChTime = TKD_Unkobi.TouChTime,
                         SyaRyo_SyaRyoCd = TPM_SyaRyo.SyaRyoCd,
                         SyaRyo_SyaRyoNm = TPM_SyaRyo.SyaRyoNm,
                         SyaRyo_SyainCdSeq = TPM_SyaRyo.SyainCdSeq,
                         SyaSyu_SyaSyuNm = TPM_SyaSyu_SyaRyo.SyaSyuNm,
                         YykSyu_SyaSyuCdSeq = TPM_SyaSyu_SyaRyo.SyaSyuCdSeq,
                         YykSyu_KataKbn = TPM_SyaSyu_SyaRyo.KataKbn,
                         SyaSyu_SyaSyuNm_YykSyu = TPM_SyaSyu_YykSyu.SyaSyuNm,
                         YykSyu_SyaSyuCdSeq_YykSyu = TPM_SyaSyu_YykSyu.SyaSyuCdSeq,
                         YykSyu_KataKbn_YykSyu = TPM_SyaSyu_YykSyu.KataKbn,
                         YykSyu_SyaSyuDai = TKD_YykSyu.SyaSyuDai,
                         CodeKb_RyakuNm = TPM_CodeKb.RyakuNm,
                         CodeKb_YykSyu_RyakuNm = TPM_CodeKb_YykSyu.RyakuNm,
                         Eigyos_RyakuNm = TPM_Eigyos.RyakuNm,
                         HaishaExp_HaiSKouKNm = TKD_Haisha.HaiSkouKnm,
                         HaishaExp_HaisBinNm = TKD_Haisha.HaiSbinNm,
                         HaishaExp_TouSKouKNm = TKD_Haisha.TouSkouKnm,
                         HaishaExp_TouSBinNm = TKD_Haisha.TouSbinNm,
                         HenSya_TenkoNo = TPM_HenSya.TenkoNo,
                         DepartureTime = _helper.ConvertTime(TKD_Haisha.SyuKoTime),
                         DeliveryTime = _helper.ConvertTime(TKD_Haisha.HaiStime),
                         ArrivalTime = _helper.ConvertTime(TKD_Haisha.TouChTime),
                         ReturnTime = _helper.ConvertTime(TKD_Haisha.KikTime),
                         StartTime = TKD_Haisha.SyuPaTime != "    " ? _helper.ConvertTime(TKD_Haisha.SyuPaTime) : _helper.ConvertTime(TKD_Haisha.SyuKoTime),
                         Tokisk_RyakuNm = TPM_Tokisk.RyakuNm,
                     }).ToList();
                return Task.FromResult(result);
            };

            return (await _codeSyuService.FilterTenantIdByCodeSyu(get, tenantCdSeq, codeSyu)).FirstOrDefault();
        }
        /// <summary>
        /// get bus data booking
        /// </summary>
        /// <param name="busstardate"></param>
        /// <param name="busenddate"></param>
        /// <param name="YoyaKbnSeq"></param>
        /// <returns></returns>
        public async Task<List<BusBookingData>> Getbusdatabooking(DateTime busstardate, DateTime busenddate, int YoyaKbnSeq, int TenantCdSeq)
        {
            string DateStarAsString = busstardate.ToString("yyyyMMdd");
            string DateEndAsString = busenddate.ToString("yyyyMMdd");
            var vpmSyasyu = from s in _dbContext.VpmSyaSyu where s.TenantCdSeq == TenantCdSeq select s;
            return await (from Yoyakusho in _dbContext.TkdYyksho
                          join Unkobi in _dbContext.TkdUnkobi on Yoyakusho.UkeNo equals Unkobi.UkeNo into Unkobi_join
                          from Unkobi in Unkobi_join.DefaultIfEmpty()
                          join Haisha in _dbContext.TkdHaisha
                                on new { Unkobi.UkeNo, Unkobi.UnkRen }
                            equals new { Haisha.UkeNo, Haisha.UnkRen } into Haisha_join
                          from Haisha in Haisha_join.DefaultIfEmpty()
                          join EIGYOSHOS in _dbContext.VpmEigyos
                          on new { EigyoCdSeq = Haisha.SyuEigCdSeq, SiyoKbn = 1 }
                          equals new { EIGYOSHOS.EigyoCdSeq, SiyoKbn = (int)EIGYOSHOS.SiyoKbn } into EIGYOSHOS_join
                                              from EIGYOSHOS in EIGYOSHOS_join.DefaultIfEmpty()
                          join KAISHA in _dbContext.VpmCompny
                          on new { EIGYOSHOS.CompanyCdSeq, TenantCdSeq = 12, SiyoKbn = 1 }
                         equals new { KAISHA.CompanyCdSeq, KAISHA.TenantCdSeq, SiyoKbn = (int)KAISHA.SiyoKbn } into KAISHA_join
                          from KAISHA in KAISHA_join.DefaultIfEmpty()
                          join EIGYOSHOS1 in _dbContext.VpmEigyos on new { EigyoCdSeq = Haisha.KikEigSeq } equals new { EigyoCdSeq = EIGYOSHOS1.EigyoCdSeq } into EIGYOSHOS1_join
                          from EIGYOSHOS1 in EIGYOSHOS1_join.DefaultIfEmpty()
                          join KAISHA1 in _dbContext.VpmCompny
                                on new { EIGYOSHOS1.CompanyCdSeq, TenantCdSeq = 12 }
                            equals new { KAISHA1.CompanyCdSeq, KAISHA1.TenantCdSeq } into KAISHA1_join
                          from KAISHA1 in KAISHA1_join.DefaultIfEmpty()
                          join YoyakuSyasyu in _dbContext.TkdYykSyu
                                on new { Haisha.UkeNo, Haisha.UnkRen, Haisha.SyaSyuRen }
                            equals new { YoyakuSyasyu.UkeNo, YoyakuSyasyu.UnkRen, YoyakuSyasyu.SyaSyuRen } into YoyakuSyasyu_join
                          from YoyakuSyasyu in YoyakuSyasyu_join.DefaultIfEmpty()
                          join UketukeEigyosho in _dbContext.VpmEigyos on new { EigyoCdSeq = Yoyakusho.UkeEigCdSeq } equals new { UketukeEigyosho.EigyoCdSeq } into UketukeEigyosho_join
                          from UketukeEigyosho in UketukeEigyosho_join.DefaultIfEmpty()
                          join TokuisakiMaster in _dbContext.VpmTokisk on Yoyakusho.TokuiSeq equals TokuisakiMaster.TokuiSeq into TokuisakiMaster_join
                          from TokuisakiMaster in TokuisakiMaster_join.DefaultIfEmpty()
                          join TokuisakiSitenMaster in _dbContext.VpmTokiSt
                                on new { Yoyakusho.TokuiSeq, Yoyakusho.SitenCdSeq }
                            equals new { TokuisakiSitenMaster.TokuiSeq, TokuisakiSitenMaster.SitenCdSeq } into TokuisakiSitenMaster_join
                          from TokuisakiSitenMaster in TokuisakiSitenMaster_join.DefaultIfEmpty()

                          join SyaSyu in vpmSyasyu on YoyakuSyasyu.SyaSyuCdSeq equals SyaSyu.SyaSyuCdSeq into SyaSyu_join
                          from SyaSyu in SyaSyu_join.DefaultIfEmpty()
                          join UKEJOKEN in _dbContext.VpmCodeKb
                              on new { CodeKbn = Haisha.UkeJyKbnCd, TenantCdSeq = 0, CodeSyu = "UKEJYKBNCD", SiyoKbn = 1 }
                          equals new { CodeKbn = Convert.ToByte(UKEJOKEN.CodeKbn), UKEJOKEN.TenantCdSeq, UKEJOKEN.CodeSyu, SiyoKbn = (int)UKEJOKEN.SiyoKbn } into UKEJOKEN_join
                          from UKEJOKEN in UKEJOKEN_join.DefaultIfEmpty()
                          join Yousha in _dbContext.TkdYousha
                               on new { Haisha.UkeNo, Haisha.UnkRen, Haisha.YouTblSeq, SiyoKbn = 1 }
                           equals new { Yousha.UkeNo, Yousha.UnkRen, Yousha.YouTblSeq, SiyoKbn = (int)Yousha.SiyoKbn } into Yousha_join
                          from Yousha in Yousha_join.DefaultIfEmpty()
                          join SyaRyo in _dbContext.VpmSyaRyo on new { HaiSSryCdSeq = Haisha.HaiSsryCdSeq } equals new { HaiSSryCdSeq = SyaRyo.SyaRyoCdSeq } into SyaRyo_join
                          from SyaRyo in SyaRyo_join.DefaultIfEmpty()
                          join SyaSyu1 in vpmSyasyu on SyaRyo.SyaSyuCdSeq equals SyaSyu1.SyaSyuCdSeq into SyaSyu1_join
                          from SyaSyu1 in SyaSyu1_join.DefaultIfEmpty()
                          join SIJJOKBN1 in _dbContext.VpmCodeKb
                              on new { CodeKbn = Haisha.SijJoKbn1, TenantCdSeq = 0, CodeSyu = "SIJJOKBN1", SiyoKbn = 1 }
                          equals new { CodeKbn = Convert.ToByte(SIJJOKBN1.CodeKbn), SIJJOKBN1.TenantCdSeq, SIJJOKBN1.CodeSyu, SiyoKbn = (int)SIJJOKBN1.SiyoKbn } into SIJJOKBN1_join
                          from SIJJOKBN1 in SIJJOKBN1_join.DefaultIfEmpty()
                          join SIJJOKBN2 in _dbContext.VpmCodeKb
                                on new { CodeKbn = Haisha.SijJoKbn2, TenantCdSeq = 0, CodeSyu = "SIJJOKBN2", SiyoKbn = 1 }
                            equals new { CodeKbn = Convert.ToByte(SIJJOKBN2.CodeKbn), SIJJOKBN2.TenantCdSeq, SIJJOKBN2.CodeSyu, SiyoKbn = (int)SIJJOKBN2.SiyoKbn } into SIJJOKBN2_join
                          from SIJJOKBN2 in SIJJOKBN2_join.DefaultIfEmpty()
                          join SIJJOKBN3 in _dbContext.VpmCodeKb
                                on new { CodeKbn = Haisha.SijJoKbn3, TenantCdSeq = 0, CodeSyu = "SIJJOKBN3", SiyoKbn = 1 }
                            equals new { CodeKbn = Convert.ToByte(SIJJOKBN3.CodeKbn), SIJJOKBN3.TenantCdSeq, SIJJOKBN3.CodeSyu, SiyoKbn = (int)SIJJOKBN3.SiyoKbn } into SIJJOKBN3_join
                          from SIJJOKBN3 in SIJJOKBN3_join.DefaultIfEmpty()
                          join SIJJOKBN4 in _dbContext.VpmCodeKb
                                on new { CodeKbn = Haisha.SijJoKbn4, TenantCdSeq = 0, CodeSyu = "SIJJOKBN4", SiyoKbn = 1 }
                            equals new { CodeKbn = Convert.ToByte(SIJJOKBN4.CodeKbn), SIJJOKBN4.TenantCdSeq, SIJJOKBN4.CodeSyu, SiyoKbn = (int)SIJJOKBN4.SiyoKbn } into SIJJOKBN4_join
                          from SIJJOKBN4 in SIJJOKBN4_join.DefaultIfEmpty()
                          join SIJJOKBN5 in _dbContext.VpmCodeKb
                                on new { CodeKbn = Haisha.SijJoKbn5, TenantCdSeq = 0, CodeSyu = "SIJJOKBN5", SiyoKbn = 1 }
                            equals new { CodeKbn = Convert.ToByte(SIJJOKBN5.CodeKbn), SIJJOKBN5.TenantCdSeq, SIJJOKBN5.CodeSyu, SiyoKbn = (int)SIJJOKBN5.SiyoKbn } into SIJJOKBN5_join
                          from SIJJOKBN5 in SIJJOKBN5_join.DefaultIfEmpty()
                          join SYAIN in _dbContext.VpmSyain on new { SyainCdSeq = Yoyakusho.InTanCdSeq } equals new { SyainCdSeq = SYAIN.SyainCdSeq } into SYAIN_join
                          from SYAIN in SYAIN_join.DefaultIfEmpty()
                          join EIGYOTAN in _dbContext.VpmSyain on new { SyainCdSeq = Yoyakusho.EigTanCdSeq } equals new { SyainCdSeq = EIGYOTAN.SyainCdSeq } into EIGYOTAN_join
                          from EIGYOTAN in EIGYOTAN_join.DefaultIfEmpty()
                              /*Update by M*/
                          join SyaSyu_01 in vpmSyasyu
                          on new {SyaRyo.SyaSyuCdSeq, S2 = TenantCdSeq }
                          equals new {SyaSyu_01.SyaSyuCdSeq,S2= SyaSyu_01.TenantCdSeq } into SyaSyu_join01
                          from SyaSyu_01 in SyaSyu_join01.DefaultIfEmpty()                         
                              /*End Update by M*/
                          where
                           string.Compare(Haisha.KikYmd, DateStarAsString) >= 0 &&
                           string.Compare(Haisha.SyuKoYmd, DateEndAsString) <= 0
                            //||(String.Compare(Haisha.TouChTime, "2359") > 0&&String.Compare(Haisha.HaiSymd, DateEndAsString) <= 0)
                            &&
                           string.Compare(TokuisakiSitenMaster.SiyoStaYmd, DateStarAsString) <= 0 &&
                           string.Compare(TokuisakiSitenMaster.SiyoEndYmd, DateStarAsString) >= 0 &&
                           ((String.Compare(Haisha.KikYmd, DateStarAsString) >= 0 &&
                           String.Compare(Haisha.SyuKoYmd, DateEndAsString) <= 0)
                           //||(String.Compare(Haisha.TouChTime, "2359") > 0&&String.Compare(Haisha.HaiSymd, DateEndAsString) <= 0) todo
                           ) &&
                           String.Compare(TokuisakiSitenMaster.SiyoStaYmd, DateStarAsString) <= 0 &&
                           String.Compare(TokuisakiSitenMaster.SiyoEndYmd, DateStarAsString) >= 0 &&
                            Yoyakusho.TenantCdSeq == TenantCdSeq
                            &&
                            Yoyakusho.YoyaSyu == 1 &&
                            Yoyakusho.SiyoKbn == 1 &&
                            Haisha.SiyoKbn == 1 &&
                            YoyakuSyasyu.SiyoKbn == 1 //&&
                            //SyaSyu.TenantCdSeq==TenantCdSeq &&
                            //SyaSyu_01.TenantCdSeq==TenantCdSeq
                          select new BusBookingData
                          {
                              Yyksho_UkeNo = Yoyakusho.UkeNo,
                              Yyksho_UkeCd = Yoyakusho.UkeCd,
                              Yyksho_UkeEigCdSeq = Yoyakusho.UkeEigCdSeq,
                              Yyksho_YoyaKbnSeq = Yoyakusho.YoyaKbnSeq,
                              Yyksho_Zeiritsu = Yoyakusho.Zeiritsu,
                              Yyksho_InTanCdSeq = Yoyakusho.InTanCdSeq,
                              Yyksho_KaktYmd = Yoyakusho.KaktYmd,
                              Yyksho_SeiTaiYmd = Yoyakusho.SeiTaiYmd,
                              Yyksho_UpdYmd = Yoyakusho.UpdYmd,
                              Yyksho_UpdTime = Yoyakusho.UpdTime,
                              Unkobi_UpdYmd = Unkobi.UpdYmd,
                              Unkobi_UpdTime = Unkobi.UpdTime,
                              Unkobi_UnkRen = Unkobi.UnkRen,
                              Unkobi_HaiSYmd = Unkobi.HaiSymd,
                              Unkobi_HaiSTime = Unkobi.HaiStime,
                              Unkobi_TouYmd = Unkobi.TouYmd,
                              Unkobi_TouChTime = Unkobi.TouChTime,
                              Unkobi_SyuPaTime = Unkobi.SyuPaTime,
                              Unkobi_DanTaNm = Unkobi.DanTaNm,
                              Unkobi_IkNm = Unkobi.IkNm,
                              Unkobi_HaiSNm = Unkobi.HaiSnm,
                              Unkobi_KSKbn = Unkobi.Kskbn,
                              Unkobi_YouKbn = Unkobi.YouKbn,
                              Unkobi_SyuKoTime = Unkobi.SyuKoTime,
                              Unkobi_KikTime = Unkobi.KikTime,
                              YykSyu_UnkRen = YoyakuSyasyu.UnkRen,
                              YykSyu_SyaSyuRen = YoyakuSyasyu.SyaSyuRen,
                              YykSyu_KataKbn = YoyakuSyasyu.KataKbn,
                              Haisha_UkeNo = Haisha.UkeNo,
                              Haisha_UnkRen = Haisha.UnkRen,
                              Haisha_TeiDanNo = Haisha.TeiDanNo,
                              Haisha_BunkRen = Haisha.BunkRen,
                              Haisha_GoSya = Haisha.GoSya,
                              Haisha_HenKai = Haisha.HenKai,
                              Haisha_BunKSyuJyn = Haisha.BunKsyuJyn,
                              Haisha_HaiSSryCdSeq = Haisha.HaiSsryCdSeq,
                              Haisha_KSSyaRSeq = Haisha.KssyaRseq,
                              Haisha_SyuKoYmd = Haisha.SyuKoYmd,
                              Haisha_SyuKoTime = Haisha.SyuKoTime,
                              Haisha_SyuEigCdSeq = Haisha.SyuEigCdSeq,
                              Haisha_KikEigSeq = Haisha.KikEigSeq,
                              Haisha_HaiSKigou = Haisha.HaiSkigou,
                              Haisha_HaiSSetTime = Haisha.HaiSsetTime,
                              Haisha_IkNm = Haisha.IkNm,
                              Haisha_HaiSYmd = Haisha.HaiSymd,
                              Haisha_HaiSTime = Haisha.HaiStime,
                              Haisha_HaiSNm = Haisha.HaiSnm,
                              Haisha_HaiSJyus1 = Haisha.HaiSjyus1,
                              Haisha_HaiSJyus2 = Haisha.HaiSjyus2,
                              Haisha_TouYmd = Haisha.TouYmd,
                              Haisha_TouChTime = Haisha.TouChTime,
                              Haisha_TouNm = Haisha.TouNm,
                              Haisha_TouKigou = Haisha.TouKigou,
                              Haisha_TouSetTime = Haisha.TouSetTime,
                              Haisha_TouJyusyo1 = Haisha.TouJyusyo1,
                              Haisha_TouJyusyo2 = Haisha.TouJyusyo2,
                              Haisha_SyuPaTime = Haisha.SyuPaTime,
                              Haisha_JyoSyaJin = Haisha.JyoSyaJin,
                              Haisha_PlusJin = Haisha.PlusJin,
                              Haisha_DrvJin = Haisha.DrvJin,
                              Haisha_GuiSu = Haisha.GuiSu,
                              Haisha_OthJin1 = Haisha.OthJin1,
                              Haisha_OthJin2 = Haisha.OthJin2,
                              Haisha_PlatNo = Haisha.PlatNo,
                              Haisha_HaiCom = Haisha.HaiCom,
                              Haisha_KikYmd = Haisha.KikYmd,
                              Haisha_KikTime = Haisha.KikTime,
                              Haisha_KSKbn = Haisha.Kskbn,
                              Haisha_YouKataKbn = Haisha.YouKataKbn,
                              Haisha_YouTblSeq = Haisha.YouTblSeq,
                              Haisha_SyaRyoUnc = Haisha.SyaRyoUnc,
                              Haisha_HaiSKbn = Haisha.HaiSkbn,
                              Haisha_HaiIKbn = Haisha.HaiIkbn,
                              Haisha_NippoKbn = Haisha.NippoKbn,
                              SyaSyu_SyaSyuNm = SyaSyu1.SyaSyuNm,                              
                              SyaSyu_SyaSyuKigo = SyaSyu1.SyaSyuKigo,
                              SyaRyo_SyaRyoNm = SyaRyo.SyaRyoNm,
                              SyaSyu_SyaSyuCdSeq = SyaRyo.SyaSyuCdSeq,
                              SyaRyo_TeiCnt = SyaRyo.TeiCnt,
                              TokiSk_TokuiSeq = TokuisakiMaster.TokuiSeq,
                              TokiSk_RyakuNm = TokuisakiMaster.RyakuNm,
                              TokiSt_TokuiSeq = TokuisakiSitenMaster.TokuiSeq,
                              TokiSt_RyakuNm = TokuisakiSitenMaster.RyakuNm,
                              Yousha_YouTblSeq = Yousha.YouTblSeq,
                              Yousha_YouCdSeq = Yousha.YouCdSeq,
                              Yousha_YouSitCdSeq = Yousha.YouSitCdSeq,
                              TextDisplay = "",
                              Syasyu_KataKbn =(int)SyaSyu_01.KataKbn,
                              Haisha_BikoNm = Haisha.BikoNm    ,
                              Yyksho_BikoNm = Yoyakusho.BikoNm    ,
                              Unkobi_BikoNm = Unkobi.BikoNm    ,
                              SyaRyo_NinKaKbn = SyaRyo.NinkaKbn,
                              Eigyos_RyakuNm=UketukeEigyosho.RyakuNm,
                              Syain_SyainNm=SYAIN.SyainNm,
                              Eigyoshos_RyakuNm=EIGYOSHOS.RyakuNm,
                              Eigyoshos1_RyakuNm=EIGYOSHOS1.RyakuNm,
                              Haisha_HaiSKouKNm=Haisha.HaiSkouKnm,
                              Haisha_HaisBinNm=Haisha.HaiSbinNm,
                              Haisha_TouSKouKNm=Haisha.TouSkouKnm,
                              Haisha_TouSBinNm=Haisha.TouBinNm,
                              UKEJOKEN_CodeKbnNm=UKEJOKEN.CodeKbnNm,
                              SIJJOKBN1_CodeKbnNm=SIJJOKBN1.CodeKbnNm,
                              SIJJOKBN2_CodeKbnNm=SIJJOKBN2.CodeKbnNm,
                              SIJJOKBN3_CodeKbnNm=SIJJOKBN3.CodeKbnNm,
                              SIJJOKBN4_CodeKbnNm=SIJJOKBN4.CodeKbnNm,
                              SIJJOKBN5_CodeKbnNm=SIJJOKBN5.CodeKbnNm,
                              YySyasyu_SyaSyuNm=SyaSyu.SyaSyuNm,
                              Eigyotan_SyainNm=EIGYOTAN.SyainNm
                          }).Distinct().ToListAsync();
        }
        public async Task<List<BusBookingData>> Getbusdatabooking(bool pickupdate, DateTime busstardate, DateTime busenddate, int bookingfrom, int bookingto, int reservationfrom, int reservationto, int company, int branch, bool isgray, string sort, int TenantCdSeq)
        {
            string DateStarAsString = busstardate.ToString("yyyyMMdd");
            string DateEndAsString = busenddate.ToString("yyyyMMdd");
            var vpmSyasyu = from s in _dbContext.VpmSyaSyu where s.TenantCdSeq == TenantCdSeq select s;
            return await (from Yoyakusho in _dbContext.TkdYyksho
                          join Unkobi in _dbContext.TkdUnkobi on Yoyakusho.UkeNo equals Unkobi.UkeNo into Unkobi_join
                          from Unkobi in Unkobi_join.DefaultIfEmpty()
                          join Haisha in _dbContext.TkdHaisha
                                on new { Unkobi.UkeNo, Unkobi.UnkRen }
                            equals new { Haisha.UkeNo, Haisha.UnkRen } into Haisha_join
                          from Haisha in Haisha_join.DefaultIfEmpty()
                          join EIGYOSHOS in _dbContext.VpmEigyos
                         on new { EigyoCdSeq = Haisha.SyuEigCdSeq, SiyoKbn = 1 }
                         equals new { EIGYOSHOS.EigyoCdSeq, SiyoKbn = (int)EIGYOSHOS.SiyoKbn } into EIGYOSHOS_join
                          from EIGYOSHOS in EIGYOSHOS_join.DefaultIfEmpty()
                          join KAISHA in _dbContext.VpmCompny
                          on new { EIGYOSHOS.CompanyCdSeq, TenantCdSeq = 12, SiyoKbn = 1 }
                         equals new { KAISHA.CompanyCdSeq, KAISHA.TenantCdSeq, SiyoKbn = (int)KAISHA.SiyoKbn } into KAISHA_join
                          from KAISHA in KAISHA_join.DefaultIfEmpty()
                          join EIGYOSHOS1 in _dbContext.VpmEigyos on new { EigyoCdSeq = Haisha.KikEigSeq } equals new { EigyoCdSeq = EIGYOSHOS1.EigyoCdSeq } into EIGYOSHOS1_join
                          from EIGYOSHOS1 in EIGYOSHOS1_join.DefaultIfEmpty()
                          join KAISHA1 in _dbContext.VpmCompny
                                on new { EIGYOSHOS1.CompanyCdSeq, TenantCdSeq = 12 }
                            equals new { KAISHA1.CompanyCdSeq, KAISHA1.TenantCdSeq } into KAISHA1_join
                          from KAISHA1 in KAISHA1_join.DefaultIfEmpty()
                          join YoyakuSyasyu in _dbContext.TkdYykSyu
                                on new { Haisha.UkeNo, Haisha.UnkRen, Haisha.SyaSyuRen }
                            equals new { YoyakuSyasyu.UkeNo, YoyakuSyasyu.UnkRen, YoyakuSyasyu.SyaSyuRen } into YoyakuSyasyu_join
                          from YoyakuSyasyu in YoyakuSyasyu_join.DefaultIfEmpty()
                          join UketukeEigyosho in _dbContext.VpmEigyos on new { EigyoCdSeq = Yoyakusho.UkeEigCdSeq } equals new { UketukeEigyosho.EigyoCdSeq } into UketukeEigyosho_join
                          from UketukeEigyosho in UketukeEigyosho_join.DefaultIfEmpty()
                          join TokuisakiMaster in _dbContext.VpmTokisk on Yoyakusho.TokuiSeq equals TokuisakiMaster.TokuiSeq into TokuisakiMaster_join
                          from TokuisakiMaster in TokuisakiMaster_join.DefaultIfEmpty()
                          join TokuisakiSitenMaster in _dbContext.VpmTokiSt
                                on new { Yoyakusho.TokuiSeq, Yoyakusho.SitenCdSeq }
                            equals new { TokuisakiSitenMaster.TokuiSeq, TokuisakiSitenMaster.SitenCdSeq } into TokuisakiSitenMaster_join
                          from TokuisakiSitenMaster in TokuisakiSitenMaster_join.DefaultIfEmpty()
                          join SyaSyu in vpmSyasyu on YoyakuSyasyu.SyaSyuCdSeq equals SyaSyu.SyaSyuCdSeq into SyaSyu_join
                          from SyaSyu in SyaSyu_join.DefaultIfEmpty()
                          join UKEJOKEN in _dbContext.VpmCodeKb
                             on new { CodeKbn = Haisha.UkeJyKbnCd, TenantCdSeq = 0, CodeSyu = "UKEJYKBNCD", SiyoKbn = 1 }
                         equals new { CodeKbn = Convert.ToByte(UKEJOKEN.CodeKbn), UKEJOKEN.TenantCdSeq, UKEJOKEN.CodeSyu, SiyoKbn = (int)UKEJOKEN.SiyoKbn } into UKEJOKEN_join
                          from UKEJOKEN in UKEJOKEN_join.DefaultIfEmpty()
                          join Yousha in _dbContext.TkdYousha
                               on new { Haisha.UkeNo, Haisha.UnkRen, Haisha.YouTblSeq, SiyoKbn = 1 }
                           equals new { Yousha.UkeNo, Yousha.UnkRen, Yousha.YouTblSeq, SiyoKbn = (int)Yousha.SiyoKbn } into Yousha_join
                          from Yousha in Yousha_join.DefaultIfEmpty()
                          join SyaRyo in _dbContext.VpmSyaRyo on new { HaiSSryCdSeq = Haisha.HaiSsryCdSeq } equals new { HaiSSryCdSeq = SyaRyo.SyaRyoCdSeq } into SyaRyo_join
                          from SyaRyo in SyaRyo_join.DefaultIfEmpty()
                          join SIJJOKBN1 in _dbContext.VpmCodeKb
                              on new { CodeKbn = Haisha.SijJoKbn1, TenantCdSeq = 0, CodeSyu = "SIJJOKBN1", SiyoKbn = 1 }
                          equals new { CodeKbn = Convert.ToByte(SIJJOKBN1.CodeKbn), SIJJOKBN1.TenantCdSeq, SIJJOKBN1.CodeSyu, SiyoKbn = (int)SIJJOKBN1.SiyoKbn } into SIJJOKBN1_join
                          from SIJJOKBN1 in SIJJOKBN1_join.DefaultIfEmpty()
                          join SIJJOKBN2 in _dbContext.VpmCodeKb
                                on new { CodeKbn = Haisha.SijJoKbn2, TenantCdSeq = 0, CodeSyu = "SIJJOKBN2", SiyoKbn = 1 }
                            equals new { CodeKbn = Convert.ToByte(SIJJOKBN2.CodeKbn), SIJJOKBN2.TenantCdSeq, SIJJOKBN2.CodeSyu, SiyoKbn = (int)SIJJOKBN2.SiyoKbn } into SIJJOKBN2_join
                          from SIJJOKBN2 in SIJJOKBN2_join.DefaultIfEmpty()
                          join SIJJOKBN3 in _dbContext.VpmCodeKb
                                on new { CodeKbn = Haisha.SijJoKbn3, TenantCdSeq = 0, CodeSyu = "SIJJOKBN3", SiyoKbn = 1 }
                            equals new { CodeKbn = Convert.ToByte(SIJJOKBN3.CodeKbn), SIJJOKBN3.TenantCdSeq, SIJJOKBN3.CodeSyu, SiyoKbn = (int)SIJJOKBN3.SiyoKbn } into SIJJOKBN3_join
                          from SIJJOKBN3 in SIJJOKBN3_join.DefaultIfEmpty()
                          join SIJJOKBN4 in _dbContext.VpmCodeKb
                                on new { CodeKbn = Haisha.SijJoKbn4, TenantCdSeq = 0, CodeSyu = "SIJJOKBN4", SiyoKbn = 1 }
                            equals new { CodeKbn = Convert.ToByte(SIJJOKBN4.CodeKbn), SIJJOKBN4.TenantCdSeq, SIJJOKBN4.CodeSyu, SiyoKbn = (int)SIJJOKBN4.SiyoKbn } into SIJJOKBN4_join
                          from SIJJOKBN4 in SIJJOKBN4_join.DefaultIfEmpty()
                          join SIJJOKBN5 in _dbContext.VpmCodeKb
                                on new { CodeKbn = Haisha.SijJoKbn5, TenantCdSeq = 0, CodeSyu = "SIJJOKBN5", SiyoKbn = 1 }
                            equals new { CodeKbn = Convert.ToByte(SIJJOKBN5.CodeKbn), SIJJOKBN5.TenantCdSeq, SIJJOKBN5.CodeSyu, SiyoKbn = (int)SIJJOKBN5.SiyoKbn } into SIJJOKBN5_join
                          from SIJJOKBN5 in SIJJOKBN5_join.DefaultIfEmpty()
                          join SYAIN in _dbContext.VpmSyain on new { SyainCdSeq = Yoyakusho.InTanCdSeq } equals new { SyainCdSeq = SYAIN.SyainCdSeq } into SYAIN_join
                          from SYAIN in SYAIN_join.DefaultIfEmpty()
                          join EIGYOTAN in _dbContext.VpmSyain on new { SyainCdSeq = Yoyakusho.EigTanCdSeq } equals new { SyainCdSeq = EIGYOTAN.SyainCdSeq } into EIGYOTAN_join
                          from EIGYOTAN in EIGYOTAN_join.DefaultIfEmpty()
                          where
                            (pickupdate == true ? string.Compare(Haisha.TouYmd, DateStarAsString) >= 0 &&
                           string.Compare(Haisha.HaiSymd, DateEndAsString) <= 0 : string.Compare(Haisha.TouYmd, DateStarAsString) >= 0 &&
                           string.Compare(Haisha.HaiSymd, DateEndAsString) <= 0) &&
                           Yoyakusho.TenantCdSeq == TenantCdSeq &&
                        //    SyaSyu.TenantCdSeq== TenantCdSeq
                        //    &&
                            Yoyakusho.YoyaSyu == 1 &&
                            Yoyakusho.SiyoKbn == 1 &&
                            Haisha.SiyoKbn == 1 &&
                            YoyakuSyasyu.SiyoKbn == 1
                          select new BusBookingData
                          {
                              Yyksho_UkeNo = Yoyakusho.UkeNo,
                              Yyksho_UkeCd = Yoyakusho.UkeCd,
                              Yyksho_UkeEigCdSeq = Yoyakusho.UkeEigCdSeq,
                              Yyksho_YoyaKbnSeq = Yoyakusho.YoyaKbnSeq,
                              Yyksho_Zeiritsu = Yoyakusho.Zeiritsu,
                              Yyksho_InTanCdSeq = Yoyakusho.InTanCdSeq,
                              Yyksho_KaktYmd = Yoyakusho.KaktYmd,
                              Unkobi_UnkRen = Unkobi.UnkRen,
                              Unkobi_HaiSYmd = Unkobi.HaiSymd,
                              Unkobi_HaiSTime = Unkobi.HaiStime,
                              Unkobi_TouYmd = Unkobi.TouYmd,
                              Unkobi_TouChTime = Unkobi.TouChTime,
                              Unkobi_SyuPaTime = Unkobi.SyuPaTime,
                              Unkobi_DanTaNm = Unkobi.DanTaNm,
                              Unkobi_IkNm = Unkobi.IkNm,
                              Unkobi_HaiSNm = Unkobi.HaiSnm,
                              Unkobi_KSKbn = Unkobi.Kskbn,
                              Unkobi_YouKbn = Unkobi.YouKbn,
                              Unkobi_SyuKoTime = Unkobi.SyuKoTime,
                              Unkobi_KikTime = Unkobi.KikTime,
                              YykSyu_UnkRen = YoyakuSyasyu.UnkRen,
                              YykSyu_SyaSyuRen = YoyakuSyasyu.SyaSyuRen,
                              YykSyu_KataKbn = YoyakuSyasyu.KataKbn,
                              Haisha_UkeNo = Haisha.UkeNo,
                              Haisha_UnkRen = Haisha.UnkRen,
                              Haisha_TeiDanNo = Haisha.TeiDanNo,
                              Haisha_BunkRen = Haisha.BunkRen,
                              Haisha_GoSya = Haisha.GoSya,
                              Haisha_HenKai = Haisha.HenKai,
                              Haisha_BunKSyuJyn = Haisha.BunKsyuJyn,
                              Haisha_HaiSSryCdSeq = Haisha.HaiSsryCdSeq,
                              Haisha_KSSyaRSeq = Haisha.KssyaRseq,
                              Haisha_SyuKoYmd = Haisha.SyuKoYmd,
                              Haisha_SyuKoTime = Haisha.SyuKoTime,
                              Haisha_SyuEigCdSeq = Haisha.SyuEigCdSeq,
                              Haisha_KikEigSeq = Haisha.KikEigSeq,
                              Haisha_HaiSKigou = Haisha.HaiSkigou,
                              Haisha_HaiSSetTime = Haisha.HaiSsetTime,
                              Haisha_IkNm = Haisha.IkNm,
                              Haisha_HaiSYmd = Haisha.HaiSymd,
                              Haisha_HaiSTime = Haisha.HaiStime,
                              Haisha_HaiSNm = Haisha.HaiSnm,
                              Haisha_HaiSJyus1 = Haisha.HaiSjyus1,
                              Haisha_HaiSJyus2 = Haisha.HaiSjyus2,
                              Haisha_TouYmd = Haisha.TouYmd,
                              Haisha_TouChTime = Haisha.TouChTime,
                              Haisha_TouNm = Haisha.TouNm,
                              Haisha_TouKigou = Haisha.TouKigou,
                              Haisha_TouSetTime = Haisha.TouSetTime,
                              Haisha_TouJyusyo1 = Haisha.TouJyusyo1,
                              Haisha_TouJyusyo2 = Haisha.TouJyusyo2,
                              Haisha_SyuPaTime = Haisha.SyuPaTime,
                              Haisha_JyoSyaJin = Haisha.JyoSyaJin,
                              Haisha_PlusJin = Haisha.PlusJin,
                              Haisha_DrvJin = Haisha.DrvJin,
                              Haisha_GuiSu = Haisha.GuiSu,
                              Haisha_OthJin1 = Haisha.OthJin1,
                              Haisha_OthJin2 = Haisha.OthJin2,
                              Haisha_PlatNo = Haisha.PlatNo,
                              Haisha_HaiCom = Haisha.HaiCom,
                              Haisha_KikYmd = Haisha.KikYmd,
                              Haisha_KikTime = Haisha.KikTime,
                              Haisha_KSKbn = Haisha.Kskbn,
                              Haisha_YouKataKbn = Haisha.YouKataKbn,
                              Haisha_YouTblSeq = Haisha.YouTblSeq,
                              Haisha_SyaRyoUnc = Haisha.SyaRyoUnc,
                              Haisha_HaiSKbn = Haisha.HaiSkbn,
                              Haisha_HaiIKbn = Haisha.HaiIkbn,
                              Haisha_NippoKbn = Haisha.NippoKbn,
                              SyaSyu_SyaSyuNm = SyaSyu.SyaSyuNm,
                              SyaSyu_SyaSyuKigo = SyaSyu.SyaSyuKigo,
                              SyaRyo_SyaRyoNm = SyaRyo.SyaRyoNm,
                              SyaRyo_TeiCnt = SyaRyo.TeiCnt,
                              TokiSk_TokuiSeq = TokuisakiMaster.TokuiSeq,
                              TokiSk_RyakuNm = TokuisakiMaster.RyakuNm,
                              TokiSt_TokuiSeq = TokuisakiSitenMaster.TokuiSeq,
                              TokiSt_RyakuNm = TokuisakiSitenMaster.RyakuNm,
                              Yousha_YouTblSeq = Yousha.YouTblSeq,
                              Yousha_YouCdSeq = Yousha.YouCdSeq,
                              Yousha_YouSitCdSeq = Yousha.YouSitCdSeq,
                              TextDisplay = "",
                              Eigyos_RyakuNm = UketukeEigyosho.RyakuNm,
                              Syain_SyainNm = SYAIN.SyainNm,
                              Eigyoshos_RyakuNm = EIGYOSHOS.RyakuNm,
                              Eigyoshos1_RyakuNm = EIGYOSHOS1.RyakuNm,
                              Haisha_HaiSKouKNm = Haisha.HaiSkouKnm,
                              Haisha_HaisBinNm = Haisha.HaiSbinNm,
                              Haisha_TouSKouKNm = Haisha.TouSkouKnm,
                              Haisha_TouSBinNm = Haisha.TouBinNm,
                              UKEJOKEN_CodeKbnNm = UKEJOKEN.CodeKbnNm,
                              SIJJOKBN1_CodeKbnNm = SIJJOKBN1.CodeKbnNm,
                              SIJJOKBN2_CodeKbnNm = SIJJOKBN2.CodeKbnNm,
                              SIJJOKBN3_CodeKbnNm = SIJJOKBN3.CodeKbnNm,
                              SIJJOKBN4_CodeKbnNm = SIJJOKBN4.CodeKbnNm,
                              SIJJOKBN5_CodeKbnNm = SIJJOKBN5.CodeKbnNm,
                              YySyasyu_SyaSyuNm = SyaSyu.SyaSyuNm,
                              Eigyotan_SyainNm = EIGYOTAN.SyainNm,
                              Haisha_BikoNm = Haisha.BikoNm,
                              Yyksho_BikoNm = Yoyakusho.BikoNm,
                              Unkobi_BikoNm = Unkobi.BikoNm,
                          }).Distinct().ToListAsync();
        }

        public SijJoKnmData GetSijJoKnm(string SetteiCd, int TenantCdSeq)
        {
            var SijJoKnm = _dbContext.VpmKyoSet.AsNoTracking().SingleOrDefault(x => x.SetteiCd == SetteiCd);
            var Result = new SijJoKnmData { SijJoKnm1 = SijJoKnm.SijJoKnm1, SijJoKnm2 = SijJoKnm.SijJoKnm2, SijJoKnm3 = SijJoKnm.SijJoKnm3, SijJoKnm4 = SijJoKnm.SijJoKnm4, SijJoKnm5 = SijJoKnm.SijJoKnm5 };
            return Result;
        }
        public async Task<VpmRepair> GetrepairAndTenantId(int code, int tenantId)
        {
            try
            {
                return _dbContext.VpmRepair.Where(t => (t.RepairCdSeq == code) &&
                                                     t.SiyoKbn == 1 &&
                                                     t.TenantCdSeq == tenantId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogTrace(ex.ToString());

                return null;
            }
        }
        public async Task<TPM_CodeKbData> GetCodeKbnByCodeAndTenantId(int code, int tenantId)
        {
            try
            {
                var kataList = await _codeSyuService.FilterTenantIdByCodeSyu(async (tenantId, codeSyu) =>
                {
                   return await
                        _dbContext.VpmCodeKb.Where(t => ((t.CodeSyu == codeSyu && t.CodeKbn == code.ToString()) &&
                                                        t.SiyoKbn == 1 &&
                                                        t.TenantCdSeq == tenantId))
                                                        .Select(_ => new TPM_CodeKbData
                                                        {
                                                            CodeKb_CodeKbn = _.CodeKbn,
                                                            CodeKb_CodeKbnSeq = _.CodeKbnSeq,
                                                            CodeKb_CodeSyu = _.CodeSyu,
                                                            CodeKb_RyakuNm = _.RyakuNm,
                                                        }).ToListAsync();
                }, tenantId, "KATAKBN");

                var shuriList = await _codeSyuService.FilterTenantIdByCodeSyu(async (tenantId, codeSyu) =>
                {
                    return await
                         _dbContext.VpmCodeKb.Where(t => ((t.CodeSyu == codeSyu && t.CodeKbn == code.ToString()) &&
                                                         t.SiyoKbn == 1 &&
                                                         t.TenantCdSeq == tenantId))
                                                         .Select(_ => new TPM_CodeKbData
                                                         {
                                                             CodeKb_CodeKbn = _.CodeKbn,
                                                             CodeKb_CodeKbnSeq = _.CodeKbnSeq,
                                                             CodeKb_CodeSyu = _.CodeSyu,
                                                             CodeKb_RyakuNm = _.RyakuNm,
                                                         }).ToListAsync();
                }, tenantId, "SHURICD");

                return kataList?.FirstOrDefault() ?? shuriList?.FirstOrDefault() ?? null;
            }
            catch (Exception ex)
            {
                _logger.LogTrace(ex.ToString());

                return null;
            }
        }

        public Dictionary<string, string> GetFieldValues(BusScheduleFilter scheduleFilter)
        {
            var result = new Dictionary<string, string>
            {
                [nameof(scheduleFilter.ScheduleDate)] = scheduleFilter.ScheduleDate.ToString("yyyyMMdd"),
                [nameof(scheduleFilter.LineDrawMode)] = scheduleFilter.LineDrawMode.ToString(),
                [nameof(scheduleFilter.GroupMode)] = scheduleFilter.GroupMode.ToString(),
                [nameof(scheduleFilter.DayMode)] = scheduleFilter.DayMode.ToString(),
                [nameof(scheduleFilter.TimeMode)] = scheduleFilter.TimeMode.ToString(),
                [nameof(scheduleFilter.SortVehicleLineMode)] = scheduleFilter.SortVehicleLineMode.ToString(),
                [nameof(scheduleFilter.SortVehicleNameMode)] = scheduleFilter.SortVehicleNameMode.ToString(),
                [nameof(scheduleFilter.DisplayLineMode)] = scheduleFilter.DisplayLineMode.ToString(),
                [nameof(scheduleFilter.ViewMode)] = scheduleFilter.ViewMode.ToString(),
                [nameof(scheduleFilter.EigyoCdSeqList)] = string.Join('-', scheduleFilter.EigyoCdSeqList),
                [nameof(scheduleFilter.CompanyCdSeqList)] = string.Join('-', scheduleFilter.CompanyCdSeqList),
                [nameof(scheduleFilter.ReservationYoyaKbnSeq)] = scheduleFilter.ReservationYoyaKbnSeq.ToString(),
            };
            return result;
        }

        public async Task<BookingRemarkHaitaCheck> GetBookingRemarkHaiCheck(string UkeNo, short UnkRen)
        {
            return await _mediatR.Send(new GetBookingRemarkHaiCheckQuery { UkeNo = UkeNo, UnkRen = UnkRen });
        }

        public void ApplyFilter(ref BusScheduleFilter scheduleFilter, Dictionary<string, string> filterValues)
        {
            string outValueString = string.Empty;
            DateTime dt = new DateTime();
            int outValueInt;
            var dataPropList = scheduleFilter
                .GetType()
                .GetProperties()
                .Where(d => d.CanWrite && d.CanRead)
                .ToList();
            foreach (var dataProp in dataPropList)
            {
                if (filterValues.TryGetValue(dataProp.Name, out outValueString))
                {
                    if (dataProp.PropertyType.IsGenericType || dataProp.PropertyType.IsClass)
                    {
                        continue;
                    }
                    dynamic setValue = null;
                    if (dataProp.PropertyType == typeof(int))
                    {
                        setValue = Convert.ChangeType(outValueString, typeof(int));
                    }
                    else if (dataProp.PropertyType == typeof(DateTime))
                    {
                        if (DateTime.TryParseExact(outValueString, "yyyyMMdd", null, DateTimeStyles.None, out dt))
                        {
                            setValue = dt;
                        }
                    }

                    dataProp.SetValue(scheduleFilter, setValue);
                }
            }

            scheduleFilter.EigyoCdSeqList.Clear();
            foreach (var branchId in filterValues[nameof(scheduleFilter.EigyoCdSeqList)].Split('-'))
            {
                if (int.TryParse(branchId, out outValueInt))
                {
                    scheduleFilter.EigyoCdSeqList.Add(outValueInt);
                }
            }
            scheduleFilter.CompanyCdSeqList.Clear();
            foreach (var comId in filterValues[nameof(scheduleFilter.CompanyCdSeqList)].Split('-'))
            {
                if (int.TryParse(comId, out outValueInt))
                {
                    scheduleFilter.CompanyCdSeqList.Add(outValueInt);
                }
            }
        }

        public async Task<string> GetBikoNm(string ukeNo, bool isUnkobi, short unkRen)
        {
            string bikoNm = await _mediatR.Send(new GetBikoNmQuery(ukeNo, isUnkobi, unkRen));
            return bikoNm;
        }
    }
}
