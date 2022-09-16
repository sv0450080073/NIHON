using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Newtonsoft.Json;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.StaffSchedule.Queries
{
    public class GetAppointmentListsGroup : IRequest<List<AppointmentList>>
    {
        public int GroupId { get; set; }
        public int TenantCdSeq { get; set; }
        public int EmployeeId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<PlanType> PlanTypes { get; set; }
        public List<AppointmentLabel> AppointmentLabels { get; set; }
        public class Handler : IRequestHandler<GetAppointmentListsGroup, List<AppointmentList>>
        {
            private readonly KobodbContext _dbcontext;
            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }

            public async Task<List<AppointmentList>> Handle(GetAppointmentListsGroup request, CancellationToken cancellationToken)
            {
                List<AppointmentList> result = new List<AppointmentList>();

                List<AppointmentList> yoteiData = GetYoteiGroupData(request.FromDate, request.ToDate, request.EmployeeId, request.GroupId, request.PlanTypes, request.AppointmentLabels);
                List<AppointmentList> finalYoteiData = GetSchYotKSyaGroupFbData(request.FromDate, request.ToDate, request.GroupId, request.EmployeeId, yoteiData);
                List<AppointmentList> kinkyuData = GetKinkyujGroupData(request.GroupId, request.EmployeeId, request.FromDate, request.ToDate);
                List<AppointmentList> haiinData = GetHaiinGroupData(request.TenantCdSeq, request.EmployeeId, request.GroupId, request.FromDate, request.ToDate);

                result.AddRange(finalYoteiData);
                result.AddRange(haiinData);
                result.AddRange(kinkyuData);

                int i = 1;
                foreach(var item in result)
                {
                    item.ScheduleId = i;
                    i++;
                }

                return result;
            }

            public List<AppointmentList> GetYoteiGroupData(string fromDate, string toDate, int employeeId, int groupId, List<PlanType> planTypes, List<AppointmentLabel> appointmentLabels)
            {
                List<AppointmentList> result = new List<AppointmentList>();

                List<YoteiDataModel> dataModels = new List<YoteiDataModel>();

                _dbcontext.LoadStoredProc("PK_dSchYoteiGroup_R")
                          .AddParam("EmployeeId", employeeId)
                          .AddParam("GroupId", groupId)
                          .AddParam("FromDate", fromDate)
                          .AddParam("ToDate", toDate)

                          .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                          .Exec(r => dataModels = r.ToList<YoteiDataModel>());



                foreach (var item in dataModels)
                {
                    if (string.IsNullOrWhiteSpace(item.YoteiSYmd) || string.IsNullOrWhiteSpace(item.YoteiEYmd))
                    {
                        continue;
                    }
                    List<LabelList> labels = new List<LabelList>();
                    if (item.TukiLabKbn.Contains("1"))
                    {
                        labels.Add(new LabelList()
                        {
                            LabelType = 1,
                            LabelText = appointmentLabels.Where(x => x.Id.Equals("1")).FirstOrDefault()?.Text
                        });
                    }
                    if (item.TukiLabKbn.Contains("2"))
                    {
                        labels.Add(new LabelList()
                        {
                            LabelType = 2,
                            LabelText = appointmentLabels.Where(x => x.Id.Equals("2")).FirstOrDefault()?.Text
                        });
                    }
                    if (item.GaiKkKbn != 1 && !item.YotKSya.Contains(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq.ToString()))
                    {
                        string endDate = item.YoteiEYmd;
                        if (item.AllDayKbn == 0)
                        {
                            if ((DateTime.ParseExact(item.YoteiEYmd, "yyyyMMdd", null) - DateTime.ParseExact(item.YoteiSYmd, "yyyyMMdd", null)).TotalDays == 1 && item.YoteiETime == "000000")
                            {
                                endDate = item.YoteiSYmd;
                            }
                            else if(item.YoteiETime == "000000")
                            {
                                endDate = DateTime.ParseExact(item.YoteiEYmd, "yyyyMMdd", null).AddDays(-1).ToString("yyyyMMdd");
                            }
                        }
                        else
                        {
                            endDate = item.YoteiEYmd;
                        }
                        result.Add(new AppointmentList()
                        {
                            IsGroupData = true,
                            SyainCdSeq = item.SyainCdSeq,
                            KankSya = item.YotKSya.Split(',').Select(Int32.Parse).Distinct().ToList(),
                            Staffs = item.YotKSya.Split(',').Select(Int32.Parse).Distinct().ToList(),
                            DataType = 1,
                            DisplayType = 90,
                            Text = StaffScheduleConstants.HavePlan,
                            StartDateInDatetimeType = DateTime.ParseExact(item.YoteiSYmd + "000000", "yyyyMMddHHmmss", null),
                            EndDateInDatetimeType = DateTime.ParseExact(endDate + "235900", "yyyyMMddHHmmss", null),
                            StartDateDisplay = DateTime.ParseExact(item.YoteiSYmd + $"{Int32.Parse(string.IsNullOrWhiteSpace(item.YoteiSTime) ? "000000" : item.YoteiSTime):000000}", "yyyyMMddHHmmss", null),
                            EndDateDisplay = DateTime.ParseExact(item.YoteiEYmd + $"{Int32.Parse(string.IsNullOrWhiteSpace(item.YoteiETime) ? "235900" : item.YoteiETime):000000}", "yyyyMMddHHmmss", null),
                            AllDayKbn = item.AllDayKbn,
                            //AllDay = item.AllDayKbn == 1 ? true : false,
                            KuriKbn = item.KuriKbn,
                            RecurrenceRule = item.KuriRule,
                            RecurrenceException = item.KuriReg,
                            YoteiInfo = null,
                            Syainnm = item.SyainNm,
                            ScheduleIdMobileDP = item.YoteiSeq.ToString(),
                            AttachedLable = labels,
                        });
                    }
                    else
                    {
                        string endDate = item.YoteiEYmd;
                        if(item.AllDayKbn == 0)
                        {
                            if ((DateTime.ParseExact(item.YoteiEYmd, "yyyyMMdd", null) - DateTime.ParseExact(item.YoteiSYmd, "yyyyMMdd", null)).TotalDays == 1 && item.YoteiETime == "000000")
                            {
                                endDate = item.YoteiSYmd;
                            }
                            else if (item.YoteiETime == "000000")
                            {
                                endDate = DateTime.ParseExact(item.YoteiEYmd, "yyyyMMdd", null).AddDays(-1).ToString("yyyyMMdd");
                            }
                        }
                        else
                        {
                            endDate = item.YoteiEYmd;
                        }
                        var employeeList = item.YotKSya.Split(',').Select(Int32.Parse).ToList();
                        var removed = employeeList.FirstOrDefault(x => x == new ClaimModel().SyainCdSeq);
                        if(removed != 0)
                        {
                            employeeList.Remove(removed);
                        }
                        result.Add(new AppointmentList()
                        {
                            IsGroupData = true,
                            SyainCdSeq = item.SyainCdSeq,
                            KankSya = item.YotKSya.Split(',').Select(Int32.Parse).Distinct().ToList(),
                            Staffs = employeeList,
                            DataType = 1,
                            DisplayType = string.IsNullOrEmpty(planTypes.Where(x => x.CodeKbnSeq == item.YoteiType).FirstOrDefault()?.Id) ? 0 : Int32.Parse(planTypes.Where(x => x.CodeKbnSeq == item.YoteiType).FirstOrDefault().Id),
                            Text = item.Title,
                            VacationType = item.KinKyuCdSeq,
                            Typelabel = new LabelList()
                            {
                                LabelType = string.IsNullOrEmpty(planTypes.Where(x => x.CodeKbnSeq == item.YoteiType).FirstOrDefault()?.Id) ? 0 : Int32.Parse(planTypes.Where(x => x.CodeKbnSeq == item.YoteiType).FirstOrDefault().Id),
                                LabelText = planTypes.Where(x => x.CodeKbnSeq == item.YoteiType).FirstOrDefault()?.Text
                            },
                            StartDateInDatetimeType = DateTime.ParseExact(item.YoteiSYmd + "000000", "yyyyMMddHHmmss", null),
                            EndDateInDatetimeType = DateTime.ParseExact(endDate + "235900", "yyyyMMddHHmmss", null),
                            StartDateDisplay = DateTime.ParseExact(item.YoteiSYmd + $"{Int32.Parse(string.IsNullOrWhiteSpace(item.YoteiSTime) ? "000000" : item.YoteiSTime):000000}", "yyyyMMddHHmmss", null),
                            EndDateDisplay = DateTime.ParseExact(item.YoteiEYmd + $"{Int32.Parse(string.IsNullOrWhiteSpace(item.YoteiETime) ? "235900" : item.YoteiETime):000000}", "yyyyMMddHHmmss", null),
                            AllDayKbn = item.AllDayKbn,
                            KuriKbn = item.KuriKbn,
                            RecurrenceRule = item.KuriRule,
                            IsPublic = item.GaiKkKbn,
                            RecurrenceException = item.KuriReg,
                            Description = item.YoteiBiko,
                            //AllDay = item.AllDayKbn == 1 ? true : false,
                            Syainnm = item.SyainNm,
                            ScheduleIdMobileDP = item.YoteiSeq.ToString(),
                            AttachedLable = labels,
                            DetailAppoitment = item.YoteiBiko,
                            YoteiInfo = new YoteiInfo()
                            {
                                YoteiSeq = item.YoteiSeq,
                                CreatorCdSeq = item.SyainCdSeq,
                                CreatorNm = item.SyainCd + "-" + item.SyainNm,
                                YoteiType = item.YoteiType,
                                KinKyuCdSeq = item.KinKyuCdSeq,
                                KinKyuTblCdSeq = item.KinKyuTblCdSeq,
                                TukiLabelArray = labels,
                                Note = item.YoteiBiko,
                                YoteiShoKbn = item.YoteiShoKbn.ToString(),
                                ShoSyainCdSeq = item.ShoSyainCdSeq,
                                ShoSyainNm = item.ShoSyainNm,
                                ShoDateTime = item.ShoUpdYmd + item.ShoUpdTime,
                                ShoRejBiko = item.ShoRejBiko,
                                ParticipantByTimeArray = new List<ParticipantByTime>()
                            },
                        });
                    }
                }

                return result;
            }

            public List<AppointmentList> GetSchYotKSyaGroupFbData(string fromDate, string toDate, int groupId, int employeeId, List<AppointmentList> yoteis)
            {
                List<SchYotKSyaFbDataModel> dataModels = new List<SchYotKSyaFbDataModel>();

                _dbcontext.LoadStoredProc("PK_dSchYoteiGroupFb_R")
                          .AddParam("GroupId", groupId)
                          .AddParam("EmployeeId", employeeId)
                          .AddParam("FromDate", fromDate)
                          .AddParam("ToDate", toDate)

                          .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                          .Exec(r => dataModels = r.ToList<SchYotKSyaFbDataModel>());


                foreach (var item in dataModels)
                {
                    List<Participant> participants = string.IsNullOrEmpty(item.EventParticipant) ? new List<Participant>() : JsonConvert.DeserializeObject<List<Participant>>(item.EventParticipant);
                    var appointment = yoteis.Where(x => x.YoteiInfo != null && x.YoteiInfo.YoteiSeq == item.YoteiSeq).FirstOrDefault();
                    if (appointment != null)
                    {
                        ParticipantByTime participantByTime = new ParticipantByTime()
                        {
                            KuriKbn = item.KuriKbn,
                            YoteiSTime = item.YoteiSTime,
                            YoteiSYmd = item.YoteiSYmd,
                            ParticipantArray = participants
                        };
                        appointment.YoteiInfo.ParticipantByTimeArray.Add(participantByTime);
                    }
                }

                return yoteis;
            }

            public List<AppointmentList> GetKinkyujGroupData(int groupId, int employeeId, string fromDate, string toDate)
            {
                List<AppointmentList> result = new List<AppointmentList>();

                List<KinkyuDataModel> dataModels = new List<KinkyuDataModel>();

                _dbcontext.LoadStoredProc("PK_dGetKikyujGroup_R")
                          .AddParam("EmployeeId", employeeId)
                          .AddParam("FromDate", fromDate)
                          .AddParam("ToDate", toDate)
                          .AddParam("GroupId", groupId)
                          .AddParam("TenantCdSeq", new ClaimModel().TenantID)

                          .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                          .Exec(r => dataModels = r.ToList<KinkyuDataModel>());

                foreach (var item in dataModels)
                {
                    if (string.IsNullOrWhiteSpace(item.KinKyuSYmd) || string.IsNullOrWhiteSpace(item.KinKyuEYmd))
                    {
                        continue;
                    }
                    LabelList label = new LabelList();
                    if (item.KinKyuKbn == 1)
                    {
                        label.LabelType = 4;
                        label.LabelText = StaffScheduleConstants.Work;
                    }
                    if (item.KinKyuKbn == 2)
                    {
                        label.LabelType = 5;
                        label.LabelText = StaffScheduleConstants.Holiday;
                    }
                    result.Add(new AppointmentList()
                    {
                        IsGroupData = true,
                        SyainCdSeq = item.SyainCdSeq,
                        KankSya = new List<int>() { item.SyainCdSeq },
                        DataType = 2,
                        DisplayType = label.LabelType,
                        Text = item.Title,
                        Typelabel = label,
                        StartDateInDatetimeType = DateTime.ParseExact(item.KinKyuSYmd + "000000", "yyyyMMddHHmmss", null),
                        EndDateInDatetimeType = DateTime.ParseExact(item.KinKyuEYmd + "235900", "yyyyMMddHHmmss", null),
                        StartDateDisplay = DateTime.ParseExact(item.KinKyuSYmd + $"{Int32.Parse(string.IsNullOrWhiteSpace(item.KinKyuSTime) ? "0000" : item.KinKyuSTime):0000}", "yyyyMMddHHmm", null),
                        EndDateDisplay = DateTime.ParseExact(item.KinKyuEYmd + $"{Int32.Parse(string.IsNullOrWhiteSpace(item.KinKyuETime) ? "2359" : item.KinKyuETime):0000}", "yyyyMMddHHmm", null),
                        AllDayKbn = (item.KinKyuSTime == "0000" && item.KinKyuETime == "2359") || (string.IsNullOrWhiteSpace(item.KinKyuSTime) && string.IsNullOrWhiteSpace(item.KinKyuETime)) ? 1 : 0,
                        //AllDay = (item.KinKyuSTime == "0000" && item.KinKyuETime == "2359") || (string.IsNullOrWhiteSpace(item.KinKyuSTime) && string.IsNullOrWhiteSpace(item.KinKyuETime)) ? true : false,
                        KuriKbn = 0,
                        Syainnm = item.SyainNm,
                        ScheduleIdMobileDP = item.KinKyuCdSeq.ToString(),
                        DetailAppoitment = item.BikoNm,
                        KinKyuInfo = new KinKyuInfo()
                        {
                            KinKyuCdSeq = item.KinKyuCdSeq,
                            KinKyuTblCdSeq = item.KinKyuTblCdSeq,
                            SyainNm = item.SyainCd + "-" + item.SyainNm,
                            KinkyuKbnNm = item.KinKyuKbnNm,
                            KinkyuNm = item.KinKyuNm,
                            BikoNm = item.BikoNm,
                            ReadKbn = item.SchReadKbn
                        }
                    });
                }

                return result;
            }

            public List<AppointmentList> GetHaiinGroupData(int tenantCdSeq, int employeeId, int groupId,string fromDate, string toDate)
            {
                List<AppointmentList> result = new List<AppointmentList>();

                List<HaiinDataModel> dataModels = new List<HaiinDataModel>();

                _dbcontext.LoadStoredProc("PK_dGetHaiinGroup_R")
                         .AddParam("EmployeeId", employeeId)
                          .AddParam("FromDate", fromDate)
                          .AddParam("ToDate", toDate)
                          .AddParam("TennantCdSeq", tenantCdSeq)
                          .AddParam("GroupId", groupId)

                         .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                         .Exec(r => dataModels = r.ToList<HaiinDataModel>());

                foreach (var item in dataModels)
                {
                    if (string.IsNullOrWhiteSpace(item.SyuKoYmd) || string.IsNullOrWhiteSpace(item.KikYmd))
                    {
                        continue;
                    }
                    result.Add(new AppointmentList()
                    {
                        IsGroupData = true,
                        SyainCdSeq = item.SyainCdSeq,
                        KankSya = new List<int>() { item.SyainCdSeq },
                        DataType = 3,
                        DisplayType = 6,
                        Text = item.Title,
                        StartDateInDatetimeType = DateTime.ParseExact(item.SyuKoYmd + "000000", "yyyyMMddHHmmss", null),
                        EndDateInDatetimeType = DateTime.ParseExact(item.KikYmd + "235900", "yyyyMMddHHmmss", null),
                        StartDateDisplay = DateTime.ParseExact(item.SyuKoYmd + $"{Int32.Parse(string.IsNullOrWhiteSpace(item.SyuKoTime) ? "0000" : item.SyuKoTime):0000}", "yyyyMMddHHmm", null),
                        EndDateDisplay = DateTime.ParseExact(item.KikYmd + $"{Int32.Parse(string.IsNullOrWhiteSpace(item.KikTime) ? "2359" : item.KikTime):0000}", "yyyyMMddHHmm", null),
                        Typelabel = new LabelList()
                        {
                            LabelType = 6,
                            LabelText = StaffScheduleConstants.Board
                        },
                        AllDayKbn = (item.SyuKoTime == "0000" && item.KikTime == "2359") || (string.IsNullOrWhiteSpace(item.SyuKoTime) && string.IsNullOrWhiteSpace(item.KikTime)) ? 1 : 0,
                        //AllDay = (item.SyuKoTime == "0000" && item.KikTime == "2359") || (string.IsNullOrWhiteSpace(item.SyuKoTime) && string.IsNullOrWhiteSpace(item.KikTime)) ? true : false,
                        KuriKbn = 0,
                        Syainnm = item.SyainNm,
                        ScheduleIdMobileDP = (item.UkeNo + item.UnkRen.ToString() + item.TeiDanNo.ToString() + item.BunkRen.ToString() + item.HaiInRen.ToString()).ToString(),
                        DetailAppoitment = item.IkNm,
                        HaiinInfo = new HaiinInfo()
                        {
                            UkeNo = item.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq ? item.UkeNo : "0",
                            UkeCd = item.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq ? item.UkeCd : 0,
                            UnkRen = item.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq ? item.UnkRen : 0,
                            TeiDanNo = item.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq ? item.TeiDanNo : 0,
                            BunkRen = item.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq ? item.BunkRen : 0,
                            HaiInRen = item.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq ? item.HaiInRen : 0,
                            SyainNm = item.SyainCd + "-" + item.SyainNm,
                            Gosya = item.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq ? item.GoSya : "0",
                            SyaSyuNm = item.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq ? item.SyaSyuNm : "0",
                            HaiSInfo = item.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq ? (string.IsNullOrWhiteSpace(item.HaiSTime) ? string.Empty : item.HaiSTime.Insert(2, ":") + (!string.IsNullOrEmpty(item.HaiSNm) ? "(" + item.HaiSNm + ")" : "")) : "0",
                            TouInfo = item.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq ? (string.IsNullOrWhiteSpace(item.TouChTime) ? string.Empty : item.TouChTime.Insert(2, ":") + (!string.IsNullOrEmpty(item.TouNm) ? "(" + item.TouNm + ")" : "")) : "0",
                            DantaNm = item.DanTaNm,
                            IkNm = item.IkNm,
                            TeiCnt = item.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq ? item.TeiCnt : 0,
                            ReadKbn = item.SchReadKbn,
                            AttachFileArray = new List<AttachFile>()
                        }
                    });
                }

                return result;
            }
        }
    }
}
