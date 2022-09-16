using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.ScheduleGroupData.Queries
{
    public class GetBookedScheduleFeedback : IRequest<BookedScheduleFeedback>
    {
        public AppointmentList data { get; set; }
        public class Handler : IRequestHandler<GetBookedScheduleFeedback, BookedScheduleFeedback>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;

            public async Task<BookedScheduleFeedback> Handle(GetBookedScheduleFeedback request, CancellationToken cancellationToken)
            {
                TkdSchYotKsyaFb currentUSerFb = new TkdSchYotKsyaFb();
                List<ParticipantFbStatus> participantFbStatus = new List<ParticipantFbStatus>();

                if (request.data.RecurrenceRule != string.Empty)
                {
                    currentUSerFb = _dbContext.TkdSchYotKsyaFb.Where(x => x.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq && x.YoteiSeq == request.data.YoteiInfo.YoteiSeq && x.KuriKbn == 1).FirstOrDefault();
                    foreach (var par in request.data.YoteiInfo.ParticipantByTimeArray[0].ParticipantArray)
                    {
                        var parFb = _dbContext.TkdSchYotKsyaFb.Where(x => x.SyainCdSeq == par.SyainCdSeq && x.YoteiSeq == request.data.YoteiInfo.YoteiSeq && x.KuriKbn == 1).FirstOrDefault();
                        if (parFb != null)
                        {
                            participantFbStatus.Add(new ParticipantFbStatus()
                            {
                                ParticipantName = par.SyainNm,
                                FeedbackStaus = parFb.AcceptKbn
                            });
                        }
                        else
                        {
                            participantFbStatus.Add(new ParticipantFbStatus()
                            {
                                ParticipantName = par.SyainNm,
                                FeedbackStaus = 2
                            });
                        }
                    }
                }
                else
                {
                    currentUSerFb = _dbContext.TkdSchYotKsyaFb.Where(x => x.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq && x.YoteiSeq == request.data.YoteiInfo.YoteiSeq && x.YoteiSymd == request.data.StartDate.Substring(0, 10)
                        .Replace("/", string.Empty).Replace("-", string.Empty) && x.YoteiStime == request.data.StartDate.Substring(11, 8)
                        .Replace(":", string.Empty)).FirstOrDefault();
                    foreach (var par in request.data.YoteiInfo.ParticipantByTimeArray[0].ParticipantArray)
                    {
                        var parFb = _dbContext.TkdSchYotKsyaFb.Where(x => x.SyainCdSeq == par.SyainCdSeq && x.YoteiSeq == request.data.YoteiInfo.YoteiSeq && x.YoteiSymd == request.data.StartDate.Substring(0, 10)
                        .Replace("/", string.Empty).Replace("-", string.Empty) && x.YoteiStime == request.data.StartDate.Substring(11, 8)
                        .Replace(":", string.Empty)).FirstOrDefault();
                        if (parFb != null)
                        {
                            participantFbStatus.Add(new ParticipantFbStatus()
                            {
                                ParticipantName = par.SyainNm,
                                FeedbackStaus = parFb.AcceptKbn
                            });
                        }
                        else
                        {
                            participantFbStatus.Add(new ParticipantFbStatus()
                            {
                                ParticipantName = par.SyainNm,
                                FeedbackStaus = 2
                            });
                        }
                    }
                }
                var listStaffFBResult = new Dictionary<string, int>();

                return new BookedScheduleFeedback()
                {
                    Title = request.data.Text,
                    Creator = request.data.YoteiInfo.CreatorNm,
                    EndDate = request.data.EndDate,
                    StartDate = request.data.StartDate,
                    IsAccept = currentUSerFb != null ? currentUSerFb.AcceptKbn == 1 ? true : false : false,
                    IsRefuse = currentUSerFb != null ? currentUSerFb.AcceptKbn == 2 ? true : false : true,
                    IsPending = currentUSerFb != null ? currentUSerFb.AcceptKbn == 3 ? true : false : false,
                    Note = request.data.YoteiInfo.Note,
                    ParticipantFbStatuses = participantFbStatus,
                    listStaffFB = listStaffFBResult
                };
            }
        }
    }
}
