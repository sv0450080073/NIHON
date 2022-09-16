using DevExpress.XtraRichEdit.Commands.Internal;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static HassyaAllrightCloud.Commons.Helpers.BookingInputHelper;

namespace HassyaAllrightCloud.Application.BookingArrangement.Queries
{
    public class GetBookingArrangementListQuery : IRequest<List<BookingArrangementData>>
    {
        private readonly string _ukeNo;
        private readonly short _unkRen;
        private readonly short _teiDanNo;
        private readonly short _bunkRen;

        public GetBookingArrangementListQuery(string ukeNo, short unkRen, short teiDanNo, short bunkRen)
        {
            _ukeNo = ukeNo;
            _unkRen = unkRen;
            _teiDanNo = teiDanNo;
            _bunkRen = bunkRen;
        }

        public class Handler : IRequestHandler<GetBookingArrangementListQuery, List<BookingArrangementData>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<BookingArrangementData>> Handle(GetBookingArrangementListQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = new List<BookingArrangementData>();
                    List<TkdTehai> tehaiList = new List<TkdTehai>();

                    // common car
                    if(request._teiDanNo == 0)
                    {
                        tehaiList = await _context.TkdTehai
                        .Where(t => t.UkeNo == request._ukeNo
                                    && t.TeiDanNo == 0
                                    && t.SiyoKbn == 1)
                        .OrderBy(t => t.TehRen)
                        .ToListAsync();
                    }
                    else
                    {
                        tehaiList = await _context.TkdTehai
                        .Where(t => t.UkeNo == request._ukeNo
                                    && t.UnkRen == request._unkRen
                                    && t.TeiDanNo == request._teiDanNo
                                    && t.BunkRen == request._bunkRen
                                    && t.SiyoKbn == 1)
                        .OrderBy(t => t.TehRen)
                        .ToListAsync();
                    }
                    
                    var haishaList = await _context.TkdHaisha
                                        .Where(h => h.UkeNo == request._ukeNo)
                                        .ToListAsync();

                    foreach (var tehai in tehaiList)
                    {
                        var arrangement = new BookingArrangementData();

                        arrangement.No = tehai.TehRen;

                        arrangement.TeiDanNo = tehai.TeiDanNo;
                        arrangement.UnkRen = tehai.UnkRen;
                        arrangement.BunkRen = tehai.BunkRen;
                        arrangement.LocationName = tehai.TehNm;
                        arrangement.Address1 = tehai.TehJyus1;
                        arrangement.Address2 = tehai.TehJyus2;
                        arrangement.Tel = tehai.TehTel.Trim();
                        arrangement.Fax = tehai.TehFax.Trim();
                        arrangement.InchargeStaff = tehai.TehTan;
                        arrangement.Comment = tehai.BikoNm;
                        arrangement.UpdYmd = tehai.UpdYmd;
                        arrangement.UpdTime = tehai.UpdTime;

                        // get date
                        if (tehai.TeiDanNo == 0)
                        {
                            var unkobi = _context.TkdUnkobi.SingleOrDefault(h => h.UkeNo == request._ukeNo && h.UnkRen == request._unkRen);
                            DateTime? journeyDate = GetScheduleDate(tehai.Nittei, tehai.TomKbn, unkobi.HaiSymd, unkobi.TouYmd);
                            if (journeyDate != null)
                            {
                                arrangement.Schedule = new Commons.Helpers.ScheduleSelectorModel();
                                arrangement.Schedule.Date = (DateTime)journeyDate;
                                arrangement.Schedule.Nittei = tehai.Nittei;
                                arrangement.Schedule.TomKbn = tehai.TomKbn;
                            }
                        }
                        else
                        {
                            var haisha = haishaList.SingleOrDefault(h => h.TeiDanNo == tehai.TeiDanNo
                                                                    && h.UnkRen == request._unkRen
                                                                    && h.BunkRen == request._bunkRen);
                            DateTime? journeyDate = GetScheduleDate(tehai.Nittei, tehai.TomKbn, haisha.HaiSymd, haisha.TouYmd);
                            if (journeyDate != null)
                            {
                                arrangement.Schedule = new Commons.Helpers.ScheduleSelectorModel();
                                arrangement.Schedule.Date = (DateTime)journeyDate;
                                arrangement.Schedule.Nittei = tehai.Nittei;
                                arrangement.Schedule.TomKbn = tehai.TomKbn;
                            }
                        }

                        if (DateTime.TryParseExact(tehai.SyuPaTime, "HHmm", null, DateTimeStyles.None, out DateTime dateTime))
                        {
                            arrangement.DepartureTime = new MyTime(dateTime.Hour, dateTime.Minute);
                        }
                        if (DateTime.TryParseExact(tehai.TouChTime, "HHmm", null, DateTimeStyles.None, out dateTime))
                        {
                            arrangement.ArrivalTime = new MyTime(dateTime.Hour, dateTime.Minute);
                        }

                        // load data code for fombobox
                        arrangement.SelectedArrangementType = new ArrangementType();
                        arrangement.SelectedArrangementType.TypeCode = tehai.TehaiCdSeq;

                        arrangement.SelectedArrangementLocation = new ArrangementLocation();
                        arrangement.SelectedArrangementLocation.BasyoMapCdSeq = tehai.TehMapCdSeq;

                        arrangement.SelectedArrangementCode = new ArrangementCode();
                        arrangement.SelectedArrangementCode.CodeKbnSeq = 0;

                        arrangement.SelectedArrangementPlaceType = new ArrangementPlaceType();
                        arrangement.SelectedArrangementPlaceType.CodeKbnSeq = 0;

                        result.Add(arrangement);
                    }

                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            private DateTime? GetScheduleDate(int nittei, int tomKbn, string startDate, string endDate)
            {
                DateTime result = new DateTime();
                if (DateTime.TryParseExact(startDate, "yyyyMMdd", null, DateTimeStyles.None, out DateTime dateTime))
                {
                    if (tomKbn == 1)
                    {
                        result = dateTime.AddDays(nittei - 1);
                    }
                    else if (tomKbn == 2)
                    {
                        result = dateTime.AddDays(-1);
                    }
                    else
                    {
                        if (DateTime.TryParseExact(endDate, "yyyyMMdd", null, DateTimeStyles.None, out dateTime))
                        {
                            result = dateTime.AddDays(1);
                        }
                    }
                }
                return result;
            }
        }
    }
}
