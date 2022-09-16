using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using System.Threading;
using HassyaAllrightCloud.Infrastructure.Persistence;
using StoredProcedureEFCore;
using Newtonsoft.Json;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Application.StaffSchedule.Queries
{
    public class GetAppointmentList : IRequest<List<AppointmentList>>
    {
        public int EmployeeId { get; set; }
        public int TenantCdSeq { get; set; }
        public int CompanyId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<PlanType> PlanTypes { get; set; }
        public List<AppointmentLabel> AppointmentLabels { get; set; }
        public class Handler : IRequestHandler<GetAppointmentList, List<AppointmentList>>
        {
            private readonly KobodbContext _dbcontext;
            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }

            public async Task<List<AppointmentList>> Handle(GetAppointmentList request, CancellationToken cancellationToken)
            {
                List<AppointmentList> result = new List<AppointmentList>();

                List<AppointmentList> yoteiData = GetYoteiData(request.FromDate, request.ToDate, request.EmployeeId, request.PlanTypes, request.AppointmentLabels);
                List<AppointmentList> finalYoteiData = GetSchYotKSyaFbData(request.FromDate, request.ToDate, request.EmployeeId, yoteiData);
                List<AppointmentList> kinkyuData = GetKinkyujData(request.TenantCdSeq, request.EmployeeId, request.FromDate, request.ToDate);
                List<AppointmentList> haiinData = GetHaiinData(request.TenantCdSeq, request.EmployeeId, request.FromDate, request.ToDate);
                List<AppointmentList> finalHaiinData = GetUnkobiFile(request.TenantCdSeq, request.EmployeeId, request.FromDate, request.ToDate, haiinData);
                List<AppointmentList> birthdayData = GetBirthDayData(request.FromDate, request.ToDate, request.TenantCdSeq);
                List<AppointmentList> dateCommentData = GetDateComment(request.CompanyId);

                result.AddRange(finalYoteiData);
                result.AddRange(finalHaiinData);
                result.AddRange(kinkyuData);
                result.AddRange(birthdayData);
                result.AddRange(dateCommentData);

                return result;
            }

            public List<AppointmentList> GetYoteiData(string fromDate, string toDate, int employeeId, List<PlanType> planTypes, List<AppointmentLabel> appointmentLabels)
            {
                List<AppointmentList> result = new List<AppointmentList>();

                List<YoteiDataModel> dataModels = new List<YoteiDataModel>();

                _dbcontext.LoadStoredProc("PK_dSchYotei_R")
                          .AddParam("EmployeeId", employeeId)
                          .AddParam("FromDate", fromDate)
                          .AddParam("ToDate", toDate)

                          .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                          .Exec(r => dataModels = r.ToList<YoteiDataModel>());

                

                foreach(var item in dataModels)
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
                    var employeeList = item.YotKSya.Split(',').Select(Int32.Parse).ToList();
                    var removed = employeeList.FirstOrDefault(x => x == new ClaimModel().SyainCdSeq);
                    if(removed != 0)
                    {
                        employeeList.Remove(removed);
                    }
                    result.Add(new AppointmentList()
                    {
                        SyainCdSeq = item.SyainCdSeq,
                        KankSya = item.YotKSya.Split(',').Select(Int32.Parse).ToList(),
                        Staffs = employeeList,
                        DataType = 1,
                        VacationType = item.KinKyuCdSeq,
                        DisplayType = item.CalendarSeq == 0 ? string.IsNullOrEmpty(planTypes.Where(x => x.CodeKbnSeq == item.YoteiType).FirstOrDefault()?.Id) ? 0 : Int32.Parse(planTypes.Where(x => x.CodeKbnSeq == item.YoteiType).FirstOrDefault().Id) : 91,
                        Text = item.Title,
                        Typelabel = new LabelList()
                        {
                            LabelType = string.IsNullOrEmpty(planTypes.Where(x => x.CodeKbnSeq == item.YoteiType).FirstOrDefault()?.Id) ? 0 : Int32.Parse(planTypes.Where(x => x.CodeKbnSeq == item.YoteiType).FirstOrDefault().Id),
                            LabelText = planTypes.Where(x => x.CodeKbnSeq == item.YoteiType).FirstOrDefault()?.Text
                        },
                        StartDateInDatetimeType = DateTime.ParseExact(item.YoteiSYmd + $"{Int32.Parse(string.IsNullOrWhiteSpace(item.YoteiSTime) ? "0" : item.YoteiSTime):000000}", "yyyyMMddHHmmss", null),
                        EndDateInDatetimeType = DateTime.ParseExact(item.YoteiEYmd + $"{Int32.Parse(string.IsNullOrWhiteSpace(item.YoteiETime) ? "0" : item.YoteiETime):000000}", "yyyyMMddHHmmss", null),
                        AllDayKbn = item.AllDayKbn,
                        KuriKbn = item.KuriKbn,
                        RecurrenceRule = item.KuriRule,
                        IsPublic = item.GaiKkKbn,
                        RecurrenceException = item.KuriReg,
                        Description = item.YoteiBiko,
                        AllDay = item.AllDayKbn == 1 ? true : false,
                        CalendarSeq = item.CalendarSeq,
                        StartDateDisplay = DateTime.ParseExact(item.YoteiSYmd + $"{Int32.Parse(string.IsNullOrWhiteSpace(item.YoteiSTime) ? "0" : item.YoteiSTime):000000}", "yyyyMMddHHmmss", null),
                        EndDateDisplay = DateTime.ParseExact(item.YoteiEYmd + $"{Int32.Parse(string.IsNullOrWhiteSpace(item.YoteiETime) ? "0" : item.YoteiETime):000000}", "yyyyMMddHHmmss", null),
                        Syainnm = item.SyainNm,
                        ScheduleIdMobileDP = item.YoteiSeq.ToString(),
                        AttachedLable = labels,
                        DetailAppoitment = item.YoteiBiko,
                        YoteiInfo = new YoteiInfo()
                        {
                            YoteiSeq = item.YoteiSeq,
                            CalendarSeq = item.CalendarSeq,
                            CreatorCdSeq = item.SyainCdSeq,
                            CreatorNm = item.SyainCd + "-" + item.SyainNm,
                            YoteiType = item.YoteiType,
                            YoteiTypeKbn = string.IsNullOrEmpty(planTypes.Where(x => x.CodeKbnSeq == item.YoteiType).FirstOrDefault()?.Id) ? 0 : Int32.Parse(planTypes.Where(x => x.CodeKbnSeq == item.YoteiType).FirstOrDefault().Id),
                            KinKyuCdSeq = item.KinKyuCdSeq,
                            KinKyuTblCdSeq = item.KinKyuTblCdSeq,
                            TukiLabelArray = labels,
                            Note = item.YoteiBiko,
                            YoteiShoKbn = item.YoteiShoKbn.ToString(),
                            ShoSyainCdSeq = item.ShoSyainCdSeq,
                            ShoSyainNm = item.ShoSyainNm,
                            ShoDateTime = item.ShoUpdYmd.Insert(4, "/").Insert(7, "/") + " (火) " + item.ShoUpdTime.Substring(0,4).Insert(2,":"),
                            ShoRejBiko = item.ShoRejBiko,
                            ParticipantByTimeArray = new List<ParticipantByTime>()
                        },
                    });
                }

                return result;
            }

            public List<AppointmentList> GetSchYotKSyaFbData(string fromDate, string toDate, int employeeId, List<AppointmentList> yoteis)
            {
                List<SchYotKSyaFbDataModel> dataModels = new List<SchYotKSyaFbDataModel>();

                _dbcontext.LoadStoredProc("PK_dSchYotKSyaFb_R")
                          .AddParam("EmployeeId", employeeId)
                          .AddParam("FromDate", fromDate)
                          .AddParam("ToDate", toDate)

                          .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                          .Exec(r => dataModels = r.ToList<SchYotKSyaFbDataModel>());


                foreach(var item in dataModels)
                {
                    List<Participant> participants = string.IsNullOrEmpty(item.EventParticipant) ? new List<Participant>() : JsonConvert.DeserializeObject<List<Participant>>(item.EventParticipant);
                    var appointment = yoteis.Where(x => x.YoteiInfo.YoteiSeq == item.YoteiSeq).FirstOrDefault();
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

            public List<AppointmentList> GetKinkyujData(int tenantCdSeq, int employeeId, string fromDate, string toDate)
            {
                List<AppointmentList> result = new List<AppointmentList>();

                List<KinkyuDataModel> dataModels = new List<KinkyuDataModel>();

                _dbcontext.LoadStoredProc("PK_dGetKikyuj_R")
                          .AddParam("EmployeeId", employeeId)
                          .AddParam("FromDate", fromDate)
                          .AddParam("ToDate", toDate)
                          .AddParam("TenantCdSeq", tenantCdSeq)

                          .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                          .Exec(r => dataModels = r.ToList<KinkyuDataModel>());

                foreach(var item in dataModels)
                {
                    if (string.IsNullOrWhiteSpace(item.KinKyuSYmd) || string.IsNullOrWhiteSpace(item.KinKyuEYmd))
                    {
                        continue;
                    }
                    LabelList label = new LabelList();
                    if(item.KinKyuKbn == 1)
                    {
                        label.LabelType = 4;
                        label.LabelText = StaffScheduleConstants.Work;
                    }
                    if(item.KinKyuKbn == 2)
                    {
                        label.LabelType = 5;
                        label.LabelText = StaffScheduleConstants.Holiday;
                    }
                    result.Add(new AppointmentList()
                    {
                        SyainCdSeq = item.SyainCdSeq,
                        KankSya = new List<int>() { item.SyainCdSeq },
                        DataType = 2,
                        DisplayType = label.LabelType,
                        Text = item.Title,
                        Typelabel = label,
                        StartDateInDatetimeType = DateTime.ParseExact(item.KinKyuSYmd + $"{Int32.Parse(string.IsNullOrWhiteSpace(item.KinKyuSTime) ? "0" : item.KinKyuSTime):0000}", "yyyyMMddHHmm", null),
                        EndDateInDatetimeType = DateTime.ParseExact(item.KinKyuEYmd + $"{Int32.Parse(string.IsNullOrWhiteSpace(item.KinKyuETime) ? "0" : item.KinKyuETime):0000}", "yyyyMMddHHmm", null),
                        AllDayKbn = item.KinKyuSTime == "0000" && item.KinKyuETime == "2359" ? 1 : 0,
                        AllDay = item.KinKyuSTime == "0000" && item.KinKyuETime == "2359" ? true : false,
                        KuriKbn = 0,
                        StartDateDisplay = DateTime.ParseExact(item.KinKyuSYmd + $"{Int32.Parse(string.IsNullOrWhiteSpace(item.KinKyuSTime) ? "0" : item.KinKyuSTime):000000}", "yyyyMMddHHmmss", null),
                        EndDateDisplay = DateTime.ParseExact(item.KinKyuEYmd + $"{Int32.Parse(string.IsNullOrWhiteSpace(item.KinKyuETime) ? "0" : item.KinKyuETime):000000}", "yyyyMMddHHmmss", null),
                        Syainnm = item.SyainNm,
                        ScheduleIdMobileDP = item.KinKyuCdSeq.ToString(),
                        DetailAppoitment = item.BikoNm,
                        KinKyuInfo = new KinKyuInfo()
                        {
                            KinKyuCdSeq = item.KinKyuCdSeq,
                            KinKyuTblCdSeq = item.KinKyuTblCdSeq,
                            SyainNm = item.SyainCd+"-"+item.SyainNm,
                            KinkyuKbnNm = item.KinKyuKbnNm,
                            KinkyuNm = item.KinKyuNm,
                            BikoNm = item.BikoNm,
                            ReadKbn = item.SchReadKbn
                        }
                    });
                }

                return result;
            }

            public List<AppointmentList> GetHaiinData(int tenantCdSeq, int employeeId, string fromDate, string toDate)
            {
                List<AppointmentList> result = new List<AppointmentList>();

                List<HaiinDataModel> dataModels = new List<HaiinDataModel>();

                _dbcontext.LoadStoredProc("PK_dGetHaiin_R")
                         .AddParam("EmployeeId", employeeId)
                          .AddParam("FromDate", fromDate)
                          .AddParam("ToDate", toDate)
                          .AddParam("TenantCdSeq", tenantCdSeq)

                         .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                         .Exec(r => dataModels = r.ToList<HaiinDataModel>());

                foreach(var item in dataModels)
                {
                    if (string.IsNullOrWhiteSpace(item.SyuKoYmd) || string.IsNullOrWhiteSpace(item.KikYmd))
                    {
                        continue;
                    }
                    result.Add(new AppointmentList()
                    {
                        SyainCdSeq = item.SyainCdSeq,
                        KankSya = new List<int>() { item.SyainCdSeq },
                        DataType = 3,
                        DisplayType = 6,
                        Text = item.Title,
                        StartDateInDatetimeType = DateTime.ParseExact(item.SyuKoYmd + $"{Int32.Parse(string.IsNullOrWhiteSpace(item.SyuKoTime) ? "0" : item.SyuKoTime):0000}", "yyyyMMddHHmm", null),
                        EndDateInDatetimeType = DateTime.ParseExact(item.KikYmd + $"{Int32.Parse(string.IsNullOrWhiteSpace(item.KikTime) ? "0" : item.KikTime):0000}", "yyyyMMddHHmm", null),
                        Typelabel = new LabelList()
                        {
                            LabelType = 6,
                            LabelText = StaffScheduleConstants.Board
                        },
                        AllDayKbn = item.SyuKoTime == "0000" && item.KikTime == "2359" ? 1 : 0,
                        AllDay = item.SyuKoTime == "0000" && item.KikTime == "2359" ? true : false,
                        KuriKbn = 0,
                        StartDateDisplay = DateTime.ParseExact(item.SyuKoYmd + $"{Int32.Parse(string.IsNullOrWhiteSpace(item.SyuKoTime) ? "0" : item.SyuKoTime):000000}", "yyyyMMddHHmmss", null),
                        EndDateDisplay = DateTime.ParseExact(item.KikYmd + $"{Int32.Parse(string.IsNullOrWhiteSpace(item.KikTime) ? "0" : item.KikTime):000000}", "yyyyMMddHHmmss", null),
                        Syainnm = item.SyainNm,
                        ScheduleIdMobileDP = (item.UkeNo + item.UnkRen.ToString() + item.TeiDanNo.ToString() + item.BunkRen.ToString() + item.HaiInRen.ToString()).ToString(),
                        DetailAppoitment = item.IkNm,
                        HaiinInfo = new HaiinInfo()
                        {
                            UkeNo = item.UkeNo,
                            UkeCd = item.UkeCd,
                            UnkRen = item.UnkRen,
                            TeiDanNo = item.TeiDanNo,
                            BunkRen = item.BunkRen,
                            HaiInRen = item.HaiInRen,
                            SyainNm = item.SyainCd +"-"+ item.SyainNm,
                            Gosya = item.GoSya,
                            SyaSyuNm = item.SyaSyuNm,
                            HaiSInfo = string.IsNullOrWhiteSpace(item.HaiSTime) ? string.Empty : item.HaiSTime.Insert(2, ":") + (!string.IsNullOrEmpty(item.HaiSNm) ? "(" + item.HaiSNm + ")" : ""),
                            TouInfo = string.IsNullOrWhiteSpace(item.TouChTime) ? string.Empty : item.TouChTime.Insert(2, ":") + (!string.IsNullOrEmpty(item.TouNm) ? "(" + item.TouNm + ")" : ""),
                            DantaNm = item.DanTaNm,
                            IkNm = item.IkNm,
                            TeiCnt = item.TeiCnt,
                            ReadKbn = item.SchReadKbn,
                            AttachFileArray = new List<AttachFile>()
                        }
                    });
                }

                return result;
            }

            public List<AppointmentList> GetUnkobiFile(int tenantCdSeq, int employeeId, string fromDate, string toDate, List<AppointmentList> haiins)
            {
                List<UnkoFileDataModel> dataModels = new List<UnkoFileDataModel>();

                _dbcontext.LoadStoredProc("PK_dGetUnkobiFile_R")
                         .AddParam("EmployeeId", employeeId)
                          .AddParam("FromDate", fromDate)
                          .AddParam("ToDate", toDate)
                          .AddParam("TenantCdSeq", tenantCdSeq)

                         .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                         .Exec(r => dataModels = r.ToList<UnkoFileDataModel>());

                foreach(var item in dataModels)
                {
                    var appointment = haiins.Where(x => x.HaiinInfo.UkeNo == item.UkeNo && x.HaiinInfo.UnkRen == item.UnkRen).FirstOrDefault();
                    if(appointment != null)
                    {
                        AttachFile attachFile = new AttachFile()
                        {
                            FileNo = item.FileNo,
                            FileLink = item.FileLink,
                            FileName = item.FileNm
                        };
                        appointment.HaiinInfo.AttachFileArray.Add(attachFile);
                    }
                }

                return haiins;
            }

            public List<AppointmentList> GetBirthDayData(string fromDate, string toDate, int tenantCdSeq)
            {
                List<AppointmentList> result = new List<AppointmentList>();

                List<BirthDayDataModel> dataModels = new List<BirthDayDataModel>();

                _dbcontext.LoadStoredProc("PK_dBirthDay_R")
                         .AddParam("FromDate", fromDate)
                         .AddParam("ToDate", toDate)
                         .AddParam("TenantCdSeq", tenantCdSeq)

                         .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                         .Exec(r => dataModels = r.ToList<BirthDayDataModel>());

                foreach(var item in dataModels)
                {
                    if (string.IsNullOrWhiteSpace(item.BirthYmd))
                    {
                        continue;
                    }
                    result.Add(new AppointmentList()
                    {
                        SyainCdSeq = item.SyainCdSeq,
                        KankSya = new List<int>() { item.SyainCdSeq },
                        DataType = 4,
                        DisplayType = 7,
                        Typelabel = new LabelList()
                        {
                            LabelType = 7,
                            LabelText = StaffScheduleConstants.BirthDay
                        },
                        StartDateInDatetimeType = DateTime.ParseExact(item.BirthYmd, "yyyyMMdd", null),
                        EndDateInDatetimeType = DateTime.ParseExact(item.BirthYmd, "yyyyMMdd", null),
                        AllDayKbn = 1,
                        KuriKbn = 1,
                        Text = item.SyainNm,
                        StartDateDisplay = DateTime.ParseExact(item.BirthYmd, "yyyyMMdd", null),
                        EndDateDisplay = DateTime.ParseExact(item.BirthYmd, "yyyyMMdd", null),
                        //RecurrenceRule =  $"FREQ=YEARLY;BYMONTHDAY={(string.IsNullOrWhiteSpace(item.BirthYmd) ? string.Empty : item.BirthYmd.Substring(6,2))};BYMONTH={(string.IsNullOrWhiteSpace(item.BirthYmd) ? string.Empty : item.BirthYmd.Substring(4,2))}",
                        BirthDayInfo = new BirthDayInfo()
                        {
                            SyainCd = item.SyainCd,
                            BirthYmd = item.BirthYmd,
                            SyainNm = item.SyainNm,
                            SyainSyokumuKbn = item.SyokumuKbn
                        }
                    });
                }

                return result;
            }

            public List<AppointmentList> GetDateComment(int companyCdSeq)
            {
                List<AppointmentList> result = new List<AppointmentList>();

                List<DateCommentDataModel> dataModels = new List<DateCommentDataModel>();

                var dataCalends = _dbcontext.TkdCalend.Where(x => x.CompanyCdSeq == companyCdSeq &&
                x.CalenSyu == 1 && x.CalenCom.Replace(" ", string.Empty).Length > 0
                ).ToList();

                foreach(var item in dataCalends)
                {
                    dataModels.Add(new DateCommentDataModel()
                    {
                        CalenYmd = item.CalenYmd,
                        CalenCom = item.CalenCom
                    });
                }

                foreach(var item in dataModels)
                {
                    if (string.IsNullOrWhiteSpace(item.CalenYmd))
                    {
                        continue;
                    }
                    result.Add(new AppointmentList()
                    {
                        SyainCdSeq = 0,
                        KankSya = new List<int>() {0},
                        DataType = 5,
                        DisplayType = 8,
                        Text = item.CalenCom,
                        Typelabel = new LabelList()
                        {
                            LabelType = 8,
                            LabelText = StaffScheduleConstants.DataComment
                        },
                        StartDateInDatetimeType = DateTime.ParseExact(item.CalenYmd, "yyyyMMdd", null),
                        EndDateInDatetimeType = DateTime.ParseExact(item.CalenYmd, "yyyyMMdd", null),
                        AllDayKbn = 1,
                        StartDateDisplay = DateTime.ParseExact(item.CalenYmd, "yyyyMMdd", null),
                        EndDateDisplay = DateTime.ParseExact(item.CalenYmd, "yyyyMMdd", null),
                        DateCommentInfo = new DateCommentInfo()
                        {
                            CalenYmd = item.CalenYmd,
                            CalenCom = item.CalenCom
                        }
                    });
                }

                return result;
            }
        }
    }
}
