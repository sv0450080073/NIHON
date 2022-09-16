using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BookingArrangement.Commands
{
    public class UpdateBookingArrangementCommand : IRequest<Unit>
    {
        private readonly string _ukeNo;
        private readonly short _unkRen;
        private readonly short _teiDanNo;
        private readonly short _bunkRen;
        private readonly List<BookingArrangementData> _bookingArrangementList;

        public UpdateBookingArrangementCommand(string ukeNo, short unkRen, short teiDanNo, short bunkRen, List<BookingArrangementData> bookingArrangementList)
        {
            _ukeNo = ukeNo;
            _unkRen = unkRen;
            _teiDanNo = teiDanNo;
            _bunkRen = bunkRen;
            _bookingArrangementList = bookingArrangementList;
        }

        public class Handler : IRequestHandler<UpdateBookingArrangementCommand, Unit>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateBookingArrangementCommand request, CancellationToken cancellationToken)
            {
                using var trans = _context.Database.BeginTransaction();
                try
                {
                    // check paid or coupon
                    var yyksho = await _context.TkdYyksho.Where(x => x.UkeNo == request._ukeNo).FirstOrDefaultAsync();
                    // check lock table
                    var lockTable = _context.TkdLockTable.SingleOrDefault(l => l.TenantCdSeq == new ClaimModel().TenantID
                                                                                && l.EigyoCdSeq == yyksho.SeiEigCdSeq);
                    var checkEdit = BookingInputHelper.CheckEditable(yyksho, lockTable);
                    if (checkEdit.Contains(BookingDisableEditState.PaidOrCoupon))
                    {
                        throw new Exception("Booking has been paid or coupon");
                    }
                    if (checkEdit.Contains(BookingDisableEditState.Locked))
                    {
                        throw new Exception("Booking has been lock");
                    }

                    Dictionary<FormEditState, List<TkdTehai>> tehaiList = GetTehaiList(request);
                    short[] editDeleteTehRen = tehaiList[FormEditState.Edited].Union(tehaiList[FormEditState.Deleted]).Select(_ => _.TehRen).ToArray();
                    var tehaiListDb = await GetTehaiListDb(request._ukeNo, request._unkRen, request._teiDanNo, request._bunkRen, editDeleteTehRen);

                    if (tehaiList[FormEditState.Added].Count > 0)
                    {
                        await _context.TkdTehai.AddRangeAsync(tehaiList[FormEditState.Added]);
                    }
                    if (tehaiList[FormEditState.Deleted].Count > 0)
                    {
                        var deletedTehRenList = tehaiList[FormEditState.Deleted].Select(f => f.TehRen).ToList();

                        var deletedTehaiListDb = tehaiListDb
                            .Where(_ => deletedTehRenList.Contains(_.TehRen))
                            .Select(f => MarkAsDeletedTehai(f))
                            .ToList();

                        _context.TkdTehai.UpdateRange(deletedTehaiListDb);
                    }
                    if (tehaiList[FormEditState.Edited].Count > 0)
                    {
                        var editedTehaiListDb = tehaiList[FormEditState.Edited]
                        .Join(tehaiListDb, _ => _.TehRen, _ => _.TehRen, (s1, s2) => UpdateTkdTehai(s2, s1))
                        .ToList();
                        _context.TkdTehai.UpdateRange(editedTehaiListDb);
                    }

                    await _context.SaveChangesAsync();
                    await trans.CommitAsync();
                    return Unit.Value;
                }
                catch (Exception)
                {
                    await trans.RollbackAsync();
                    throw;
                }
            }

            private async Task<List<TkdTehai>> GetTehaiListDb(string ukeNo, short unkRen, short teiDanNo, short bunkRen, params short[] tehRenList)
            {
                var tehaiList = await (from tehai in _context.TkdTehai
                                       where tehai.UkeNo == ukeNo
                                                && tehai.UnkRen == unkRen
                                                && tehai.TeiDanNo == teiDanNo
                                                && tehai.BunkRen == bunkRen
                                                && tehRenList.Contains(tehai.TehRen)
                                       select tehai).ToListAsync();

                return tehaiList;
            }

            private Dictionary<FormEditState, List<TkdTehai>> GetTehaiList(UpdateBookingArrangementCommand request)
            {
                var tehaiListInsert = new Dictionary<FormEditState, List<TkdTehai>>()
                {
                    [FormEditState.Added] = new List<TkdTehai>(),
                    [FormEditState.Edited] = new List<TkdTehai>(),
                    [FormEditState.Deleted] = new List<TkdTehai>(),
                    [FormEditState.None] = new List<TkdTehai>(),
                };

                short tehRenMax = (short)_context.TkdTehai
                        .Where(f => f.UkeNo == request._ukeNo
                                    && f.UnkRen == request._unkRen
                                    && f.TeiDanNo == request._teiDanNo
                                    && f.BunkRen == request._bunkRen)
                        .Select(m => Convert.ToInt32(m.TehRen))
                        .ToList()
                        .DefaultIfEmpty(0)
                        .Max();

                foreach (var arrangement in request._bookingArrangementList)
                {
                    short setTehRenMax = arrangement.EditState == FormEditState.Added ? ++tehRenMax : arrangement.No;
                    var tehai = new TkdTehai();

                    tehai.UkeNo = request._ukeNo;
                    tehai.UnkRen = arrangement.UnkRen;
                    tehai.TeiDanNo = arrangement.TeiDanNo;
                    tehai.BunkRen = arrangement.BunkRen;
                    tehai.TehRen = setTehRenMax;
                    tehai.HenKai = 0;
                    tehai.Nittei = arrangement.Schedule.Nittei;
                    tehai.TomKbn = arrangement.Schedule.TomKbn;
                    tehai.TeiDanNittei = arrangement.TeiDanNo == 0 ? (short)0 : tehai.Nittei;
                    tehai.TeiDanTomKbn = arrangement.TeiDanNo == 0 ? (byte)9 : tehai.TomKbn;
                    tehai.TehMapCdSeq = arrangement.SelectedArrangementLocation == null ? 0:arrangement.SelectedArrangementLocation.BasyoMapCdSeq;
                    tehai.TehNm = arrangement.LocationName;
                    tehai.TehJyus1 = arrangement.Address1 ?? string.Empty;
                    tehai.TehJyus2 = arrangement.Address2 ?? string.Empty;
                    tehai.TehTel = arrangement.Tel ?? string.Empty;
                    tehai.TehFax = arrangement.Fax ?? string.Empty;
                    tehai.TehTan = arrangement.InchargeStaff ?? string.Empty;
                    tehai.TehaiCdSeq = arrangement.SelectedArrangementType?.TypeCode ?? 0;
                    tehai.TouChTime = arrangement.ArrivalTime.ToStringWithoutDelimiter();
                    tehai.SyuPaTime = arrangement.DepartureTime.ToStringWithoutDelimiter();
                    tehai.BikoNm = arrangement.Comment ?? string.Empty;
                    tehai.SiyoKbn = 1;

                    tehai.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    tehai.UpdTime = DateTime.Now.ToString("HHmmss");
                    tehai.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    tehai.UpdPrgId = "KJ2000";

                    tehaiListInsert[arrangement.EditState].Add(tehai);
                }

                return tehaiListInsert;
            }

            private TkdTehai MarkAsDeletedTehai(TkdTehai tkdTehai)
            {
                tkdTehai.SiyoKbn = 2;
                tkdTehai.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                tkdTehai.UpdTime = DateTime.Now.ToString("HHmmss");
                tkdTehai.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                tkdTehai.UpdPrgId = "KJ2000";
                return tkdTehai;
            }

            private TkdTehai UpdateTkdTehai(TkdTehai oldTehai, TkdTehai newTehai)
            {
                newTehai.UkeNo = oldTehai.UkeNo;
                newTehai.UnkRen = oldTehai.UnkRen;
                newTehai.TeiDanNo = oldTehai.TeiDanNo;
                newTehai.BunkRen = oldTehai.BunkRen;
                newTehai.HenKai = ++oldTehai.HenKai;
                newTehai.SiyoKbn = 1;
                newTehai.UpdPrgId = "KJ2000";

                return oldTehai.SimpleCloneProperties(newTehai);
            }
        }
    }
}
