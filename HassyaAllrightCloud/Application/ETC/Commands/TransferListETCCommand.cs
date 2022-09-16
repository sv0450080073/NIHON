using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.ETC.Commands
{
    public class TransferListETCCommand : IRequest<bool>
    {
        public List<ETCData> ListModel { get; set; }
        public int TenantCdSeq { get; set; }
        public class Handler : IRequestHandler<TransferListETCCommand, bool>
        {
            private readonly KobodbContext _context;
            private readonly IMediator _mediator;
            public Handler(KobodbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }

            public async Task<bool> Handle(TransferListETCCommand request, CancellationToken cancellationToken)
            {
                var currentDate = DateTime.Now;
                var isYykshoUpdated = false;
                var isFutumAdded = false;
                var isMishumAdded = false;
                var listETC = request.ListModel;

                foreach (var item in listETC)
                {
                    isFutumAdded = false;
                    isMishumAdded = false;
                    isYykshoUpdated = false;
                    var mishum = new TkdMishum();
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            // insert FutTum
                            var yyksho = _context.TkdYyksho.FirstOrDefault(_ => _.UkeNo == item.UkeNo && _.UpdYmd == item.UpdYmd && _.UpdTime == item.UpdTime && _.TenantCdSeq == request.TenantCdSeq);
                            var futTum = new TkdFutTum();
                            MapFutTum(futTum, item, currentDate, yyksho, request.TenantCdSeq); ;
                            _context.TkdFutTum.Add(futTum);
                            isFutumAdded = true;
                            _context.SaveChanges();

                            if (futTum.SeisanKbn == 1)
                            {
                                // insert Mishum
                                var listFutai = _context.VpmFutai.Where(_ => _.FutaiCdSeq == item.FutTumCdSeq && _.TenantCdSeq == request.TenantCdSeq).ToList();
                                if (listFutai.Count == 0)
                                {
                                    item.FutGuiKbn = 0;
                                }
                                else
                                {
                                    item.FutGuiKbn = listFutai[0].FutGuiKbn;
                                }
                                
                                MapMishum(mishum, item, futTum, currentDate, request.TenantCdSeq);
                                _context.TkdMishum.Add(mishum);
                                isMishumAdded = true;
                                _context.SaveChanges();
                            }

                            // update Yyksho
                            if (yyksho != null)
                            {
                                yyksho.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                                yyksho.UpdTime = currentDate.ToString(CommonConstants.FormatHms);
                                yyksho.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                                yyksho.UpdPrgId = Constants.ETCTransferUpdPrgIdKJ1300P;
                                isYykshoUpdated = true;
                                _context.SaveChanges();
                            }

                            #region yellow
                            // 00
                            var listFutai1 = _context.VpmFutai.ToList();

                            // 01
                            short maxMihRen = _context.TkdMihrim.Where(_ => _.UkeNo == futTum.UkeNo).OrderByDescending(e => e.MihRen).Select(e => e.MihRen).FirstOrDefault();

                            // 02
                            TkdMfutTu futTu = new TkdMfutTu();

                            var k = _context.TkmKasSet.Where(e => e.CompanyCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID).FirstOrDefault();
                            if (k != null && k.ExpItem != null && k.ExpItem.Length >= 8 && k.ExpItem.Substring(8, 1) == 1.ToString())
                            {
                                pf02GetFutTumData01(item, futTu, futTum, currentDate);
                            }
                           
                            // 03 - pf03GetYouTblSeq
                            // get list haisha
                            var listHaisha = (from h in _context.TkdHaisha.Where(_ => _.UkeNo == futTum.UkeNo && _.UnkRen == futTum.UnkRen && _.SiyoKbn == CommonConstants.SiyoKbn)
                                              join y in _context.TkdYousha.Where(_ => _.SiyoKbn == CommonConstants.SiyoKbn)
                                              on new { h.UkeNo, h.UnkRen, h.YouTblSeq } equals new { y.UkeNo, y.UnkRen, y.YouTblSeq } into tempY
                                              from data in tempY.DefaultIfEmpty()
                                              select new ETCHaisha()
                                              {
                                                  UkeNo = h.UkeNo,
                                                  BunkRen = h.BunkRen,
                                                  TeiDanNo = h.TeiDanNo,
                                                  UnkRen = h.UnkRen,
                                                  You_JitaFlg = data.JitaFlg,
                                                  You_YouTblSeq = data.YouTblSeq,
                                              }).ToList();

                            if (!listHaisha.Any())
                            {
                                if(isFutumAdded)
                                    _context.Entry(futTum).State = EntityState.Detached;
                                if(isMishumAdded)
                                    _context.Entry(mishum).State = EntityState.Detached;
                                if(isYykshoUpdated)
                                    _context.Entry(yyksho).State = EntityState.Detached;
                                throw new Exception($"EtcImport entity has no Haisha with UkeNo = {futTum.UkeNo}, UnkRen = {futTum.UnkRen}");
                            }
                            listHaisha = listHaisha.Where(data => data.You_YouTblSeq != 0 && data.You_JitaFlg == 1).ToList();
                            var listJitaYouTblSeqBefore = new List<ETCJitaYouTblSeq>();
                            var listJitaYouTblSeqAfter = new List<ETCJitaYouTblSeq>();

                            pf03GetYouTblSeq(listJitaYouTblSeqBefore, listJitaYouTblSeqAfter, listHaisha, futTum);

                            // 04 - ps04GetFuzui
                            byte mFutSyu = 0;
                            var listMishum = ps04GetFuzui(futTum, listFutai1, ref mFutSyu, currentDate);

                            // 05 - pf05GetMFutTu
                            var listMFutTu = _context.TkdMfutTu.Where(_ => _.UkeNo == futTum.UkeNo
                                                                        && _.UnkRen == futTum.UnkRen
                                                                        && _.FutTumKbn == futTum.FutTumKbn
                                                                        && _.FutTumRen == futTum.FutTumRen).ToList();
                            var listMFutTuUpdate = new List<TkdMfutTu>();
                            var listMFutTuInsert = new List<TkdMfutTu>();

                            pf05GetMFutTu(listMFutTu, futTu, listMFutTuInsert, listMFutTuUpdate, currentDate);

                            // 06 - ps06GetYoushaMihrim
                            var listYouTblSeqTemp = listJitaYouTblSeqBefore.Select(_ => _.YouTblSeq).ToList();
                            var listYFutTuBefore = new List<TkdYfutTu>();
                            var listYFutTuAfter = new List<TkdYfutTu>();
                            var listMihrimBefore = new List<TkdMihrim>();
                            var listMihrimAfter = new List<TkdMihrim>();

                            ps06GetYoushaMihrim(futTum, listYouTblSeqTemp, listYFutTuBefore, listYFutTuAfter, listMihrimBefore, listMihrimAfter, mFutSyu, currentDate);

                            // 07 - ps07DeleteFutTu01
                            bool rpbolDelete = false;

                            if (futTum.SiyoKbn != 1)
                            {
                                ps07DeleteFutTu01(listMishum, listMFutTuInsert, listMFutTuUpdate, listYFutTuBefore, listMihrimBefore, currentDate);
                                rpbolDelete = true;
                            }

                            if (!rpbolDelete)
                            {
                                var listYFutTuAfterInsert = new List<TkdYfutTu>();
                                var listMihrimAfterInsert = new List<TkdMihrim>();
                                // 08 : Ignore

                                // 11 - ps11UriDivide
                                ps11UriDivide(futTum, listMFutTu, currentDate, listMFutTuUpdate, listMFutTuInsert);

                                // 12 - ps12Kasan
                                ps12Kasan(futTum, listMFutTu, listHaisha, listYFutTuBefore, listYFutTuAfter, listYFutTuAfterInsert, listJitaYouTblSeqAfter, listMihrimAfter,
                                    listMihrimAfterInsert, mFutSyu, currentDate, maxMihRen);

                                // 15
                                _context.TkdMfutTu.AddRange(listMFutTuInsert);
                                _context.TkdYfutTu.AddRange(listYFutTuAfterInsert);
                                _context.TkdMihrim.AddRange(listMihrimAfterInsert);
                            }
                            _context.SaveChanges();

                            #endregion

                            //var haitaHenkai = _context.TkdHenko.Where(_ => _.UkeNo == item.UkeNo && _.YykSiyoFlg == 0).OrderByDescending(e => e.HaitaHenKai).Select(e => e.HaitaHenKai).FirstOrDefault();
                            //haitaHenkai = (short)(haitaHenkai == 0 ? 1 : haitaHenkai + 1);
                            //var henko = new TkdHenko();
                            //henko.UkeNo = item.UkeNo;
                            //henko.YykSiyoFlg = 0;
                            //henko.HaitaHenKai = haitaHenkai;
                            //henko.HenYmd = currentDate.ToString(CommonConstants.FormatYMD);
                            //henko.HenTime = currentDate.ToString(CommonConstants.FormatHms);
                            //henko.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                            //henko.UpdTime = currentDate.ToString(CommonConstants.FormatHms);
                            //henko.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            //henko.UpdPrgId = Constants.ETCTransferUpdPrgIdPS7000P;
                            //_context.TkdHenko.Add(henko);
                            //_context.SaveChanges();

                            item.TensoKbn = 1;
                            await _mediator.Send(new UpdateListETCCommand() { ListModel = new List<ETCData>() { item }, IsFromTranfer = true });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }

                        transaction.Commit();
                    }
                };

                return true;
            }

            private void MapFutTum(TkdFutTum model, ETCData item, DateTime currentDate, TkdYyksho yyksho, int tenantCdSeq)
            {
                model.UkeNo = item.UkeNo;
                model.UnkRen = item.UnkRen;
                model.FutTumKbn = 1;
                short futtumRen = (short)((from ftum in _context.TkdFutTum.Where(_ => _.UkeNo == item.UkeNo &&
                                                                                      _.UnkRen == item.UnkRen &&
                                                                                      _.FutTumKbn == model.FutTumKbn)
                                    join ftai in _context.VpmFutai.Where(_ => _.SiyoKbn == CommonConstants.SiyoKbn)
                                    on ftum.FutTumCdSeq equals ftai.FutaiCdSeq
                                    where ftai.TenantCdSeq == tenantCdSeq
                                    orderby ftum.FutTumRen descending
                                    select ftum.FutTumRen).FirstOrDefault() + 1);

                //model.FutTumRen = (short)(_context.TkdFutTum.Where(e => e.UkeNo == item.UkeNo && 
                //                                                        e.UnkRen == item.UnkRen && 
                //                                                        e.FutTumKbn == model.FutTumKbn)
                //                                            .OrderByDescending(e => e.FutTumRen)
                //                                            .Select(e => e.FutTumRen).FirstOrDefault() + 1);

                model.FutTumRen = futtumRen;

                model.HenKai = 0;
                model.Nittei = item.Nittei;
                model.TomKbn = item.TomKbn;
                model.FutTumCdSeq = item.FutTumCdSeq;
                model.FutTumNm = item.FutTumNm;
                model.HasYmd = item.UnkYmd;
                model.IriRyoChiCd = item.IriRyoChiCd;
                model.IriRyoCd = item.IriRyoCd;
                model.IriRyoNm = item.IriRyokinNm;
                model.DeRyoChiCd = item.DeRyoChiCd;
                model.DeRyoCd = item.DeRyoCd;
                model.DeRyoNm = item.DeRyokinNm;
                model.SeisanCdSeq = item.SeisanCdSeq;
                model.SeisanNm = item.SeisanNm;
                model.SeisanKbn = item.SeisanKbn;
                model.TanKa = item.TanKa;
                model.Suryo = item.Suryo;
                if (item.ZeiKbn == 2)
                {
                    model.UriGakKin = item.UriGakKin - item.SyaRyoSyo;
                }
                else
                {
                    model.UriGakKin = item.UriGakKin;
                }
                model.ZeiKbn = item.ZeiKbn;
                model.Zeiritsu = item.ZeiRitu;
                model.SyaRyoSyo = item.SyaRyoSyo;
                model.TesuRitu = item.TesuRitu;
                model.SyaRyoTes = item.SyaRyoTes;
                model.NyuKinKbn = 1;
                model.NcouKbn = 1;
                model.BikoNm = item.BikoNm ?? string.Empty;
                model.ExpItem = "999";
                model.SiyoKbn = 1;
                model.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                model.UpdTime = currentDate.ToString(CommonConstants.FormatHms);
                model.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                model.UpdPrgId = Constants.ETCTransferUpdPrgIdKJ1300P;

                model.SortJun = 0;
                model.SirSitenCdSeq = yyksho?.SirSitenCdSeq ?? 0;
                model.SirTanKa = model.TanKa;
                model.SirSuryo = model.Suryo;
                model.SirGakKin = model.UriGakKin;
                model.SirZeiKbn = model.ZeiKbn;
                model.SirZeiritsu = model.Zeiritsu;
                model.SirSyaRyoSyo = model.SyaRyoSyo;
            }

            private void MapMishum(TkdMishum model, ETCData item, TkdFutTum futTum, DateTime currentDate, int tenantCdSeq)
            {
                model.UkeNo = item.UkeNo;

                MishumData mishumData = new MishumData();
                _context.LoadStoredProc("PK_GetMishumDataForTransferETC_R")
                        .AddParam("@UkeNo", item.UkeNo)
                        .AddParam("@TenantCdSeq", tenantCdSeq)
                        .Exec(rows => mishumData = rows.FirstOrDefault<MishumData>());
                model.MisyuRen = (short)(mishumData?.MisyuRen + 1 ?? 0);

                //model.MisyuRen = (short)(_context.TkdMishum.Where(_ => _.UkeNo == item.UkeNo).OrderByDescending(e => e.MisyuRen).Select(e => e.MisyuRen).FirstOrDefault() + 1);
                
                model.HenKai = 0;
                model.SeiFutSyu = item.FutGuiKbn;
                if (item.ZeiKbn == 2)
                {
                    model.UriGakKin = item.UriGakKin - item.SyaRyoSyo;
                }
                else
                {
                    model.UriGakKin = item.UriGakKin;
                }
                model.SyaRyoSyo = item.SyaRyoSyo;
                model.SyaRyoTes = item.SyaRyoTes;
                model.SeiKin = item.UriGakKin + item.SyaRyoSyo - item.SyaRyoTes;
                model.NyuKinRui = 0;
                model.CouKesRui = 0;
                model.FutuUnkRen = item.UnkRen;
                model.FutTumRen = futTum.FutTumRen;
                model.SiyoKbn = 1;
                model.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                model.UpdTime = currentDate.ToString(CommonConstants.FormatHms);
                model.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                model.UpdPrgId = Constants.ETCTransferUpdPrgIdKJ1300P;
            }

            private int KingakuChosei(int vintChoseiGaku, int vintTankaGaku, int vintTeidanSuryo, ref int rintOvreGaku)
            {
                if (rintOvreGaku != 0)
                {
                    var num = (rintOvreGaku - vintChoseiGaku) >= 0 ? vintChoseiGaku : rintOvreGaku;
                    rintOvreGaku -= num;
                    return vintTankaGaku * vintTeidanSuryo + num;
                }
                else
                {
                    return vintTankaGaku * vintTeidanSuryo;
                }
            }

            private void pf02GetFutTumData01(ETCData item, TkdMfutTu futTu, TkdFutTum futTum, DateTime currentDate)
            {
                futTu.UkeNo = item.UkeNo;
                futTu.UnkRen = item.UnkRen;
                futTu.FutTumKbn = 1;
                futTu.FutTumRen = futTum.FutTumRen;
                futTu.TeiDanNo = item.TeiDanNo;
                futTu.BunkRen = item.BunkRen;
                futTu.Suryo = item.Suryo;
                if (item.ZeiKbn == 2)
                {
                    futTu.UriGakKin = item.UriGakKin - item.SyaRyoSyo;
                }
                else
                {
                    futTu.UriGakKin = item.UriGakKin;
                }
                futTu.SyaRyoSyo = item.SyaRyoSyo;
                futTu.SyaRyoTes = item.SyaRyoTes;
                futTu.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                futTu.UpdTime = currentDate.ToString(CommonConstants.FormatHms);
                futTu.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
            }

            private void pf03GetYouTblSeq(List<ETCJitaYouTblSeq> listBefore, List<ETCJitaYouTblSeq> listAfter, List<ETCHaisha> listHaisha, TkdFutTum futTum)
            {
                if (listHaisha.Count > 0)
                {
                    var listYouTblSeq = listHaisha.Select(_ => _.You_YouTblSeq).ToList();

                    var sql = from y in _context.TkdYousha
                              where y.UkeNo == futTum.UkeNo
                                  && y.UnkRen == futTum.UnkRen
                                  && listYouTblSeq.Contains(y.YouTblSeq)
                              select new ETCJitaYouTblSeq()
                              {
                                  YouTblSeq = y.YouTblSeq,
                                  YouSitCdSeq = y.YouSitCdSeq,
                                  YouCdSeq = y.YouCdSeq,
                                  HasYmd = y.HasYmd
                              };
                    
                    // get list haisha before
                    listBefore.AddRange(sql.ToList());

                    // get list haisha after
                    listAfter.AddRange(sql.ToList());
                }
            }

            private List<TkdMishum> ps04GetFuzui(TkdFutTum futTum, List<VpmFutai> listFutai1, ref byte mFutSyu, DateTime currentDate)
            {
                var tempFutai = listFutai1.Where(_ => _.FutaiCdSeq == futTum.FutTumCdSeq).ToList();
                if (tempFutai.Count > 0)
                {
                    mFutSyu = tempFutai[0].FutGuiKbn;
                }

                var mFutSyuTemp = mFutSyu;

                var listMishum = _context.TkdMishum.Where(_ => _.UkeNo == futTum.UkeNo
                                                            && _.SeiFutSyu >= (mFutSyuTemp == 6 ? 6 : 2)
                                                            && _.SeiFutSyu <= (mFutSyuTemp == 6 ? 6 : 5)
                                                            && _.FutuUnkRen == futTum.UnkRen
                                                            && _.FutTumRen == futTum.FutTumRen
                                                            && _.SiyoKbn == CommonConstants.SiyoKbn
                                                         ).ToList();

                if (listMishum.Count > 0)
                {
                    if (futTum.SiyoKbn == 2 || futTum.SeisanKbn == 2)
                    {
                        listMishum[0].SiyoKbn = 2;
                        listMishum[0].UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                        listMishum[0].UpdTime = currentDate.ToString(CommonConstants.FormatHms);
                        listMishum[0].UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        listMishum[0].UpdPrgId = Constants.ETCTransferUpdPrgIdKK9210P;
                    }
                }

                return listMishum;
            }

            private void pf05GetMFutTu(List<TkdMfutTu> listMFutTu, TkdMfutTu futTu, List<TkdMfutTu> listMFutTuInsert, List<TkdMfutTu> listMFutTuUpdate, DateTime currentDate)
            {
                if (listMFutTu.Count > 0)
                {
                    listMFutTu.ForEach(mfuttu =>
                    {
                        if (futTu != null && (futTu.UkeNo != mfuttu.UkeNo || futTu.UnkRen != mfuttu.UnkRen || futTu.FutTumKbn != mfuttu.FutTumKbn
                        || futTu.FutTumRen != mfuttu.FutTumRen || futTu.TeiDanNo != mfuttu.TeiDanNo || futTu.BunkRen != mfuttu.BunkRen))
                        {
                            mfuttu.Suryo = 0;
                            mfuttu.UriGakKin = 0;
                            mfuttu.SyaRyoSyo = 0;
                            mfuttu.SyaRyoTes = 0;
                            mfuttu.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                            mfuttu.UpdTime = currentDate.ToString(CommonConstants.FormatHms);
                            mfuttu.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            mfuttu.UpdPrgId = Constants.ETCTransferUpdPrgIdKK9210P;
                            listMFutTuUpdate.Add(mfuttu);
                        }
                        //else
                        //{
                        //    mfuttu.Suryo = futTu.Suryo;
                        //}
                    });
                }

                if(!string.IsNullOrEmpty(futTu.UkeNo))
                {
                    var listMFutTuTemp = listMFutTu.Where(_ => _.UkeNo == futTu.UkeNo
                                                           && _.UnkRen == futTu.UnkRen
                                                           && _.FutTumKbn == futTu.FutTumKbn
                                                           && _.FutTumRen == futTu.FutTumRen
                                                           && _.TeiDanNo == futTu.TeiDanNo
                                                           && _.BunkRen == futTu.BunkRen).ToList();

                    if (listMFutTuTemp.Count == 0)
                    {
                        var model = new TkdMfutTu();
                        model.UkeNo = futTu.UkeNo;
                        model.UnkRen = futTu.UnkRen;
                        model.FutTumKbn = futTu.FutTumKbn;
                        model.FutTumRen = futTu.FutTumRen;
                        model.TeiDanNo = futTu.TeiDanNo;
                        model.BunkRen = futTu.BunkRen;
                        model.HenKai = 0;
                        model.Suryo = futTu.Suryo;
                        model.UriGakKin = 0;
                        model.SyaRyoSyo = 0;
                        model.SyaRyoTes = 0;
                        model.SiyoKbn = 1;
                        model.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                        model.UpdTime = currentDate.ToString(CommonConstants.FormatHms);
                        model.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        model.UpdPrgId = Constants.ETCTransferUpdPrgIdKK9210P;
                        listMFutTu.Add(model);
                        listMFutTuInsert.Add(model);
                    }
                }
            }

            private void ps06GetYoushaMihrim(TkdFutTum futTum, List<int> listYouTblSeqTemp, List<TkdYfutTu> listFutTuBefore, List<TkdYfutTu> listFutTuAfter,
                List<TkdMihrim> listMihrimBefore, List<TkdMihrim> listMihrimAfter, byte mFutSyu, DateTime currentDate)
            {
                if (listYouTblSeqTemp.Count > 0)
                {
                    var sql = _context.TkdYfutTu.Where(_ => _.UkeNo == futTum.UkeNo
                                                                 && _.UnkRen == futTum.UnkRen
                                                                 && _.FutTumKbn == futTum.FutTumKbn
                                                                 && _.YouFutTumRen == futTum.FutTumRen
                                                                 && _.SiyoKbn == CommonConstants.SiyoKbn
                                                                 && listYouTblSeqTemp.Contains(_.YouTblSeq));
                    listFutTuBefore.AddRange(sql.ToList());

                    listFutTuAfter.AddRange(sql.Where(_ => _.SihKbn == 1 && _.ScouKbn == 1).ToList());

                    listFutTuAfter.ForEach(futTu =>
                    {
                        MapFuttu(futTu, futTum, currentDate);
                    });

                    var mihrimSql = _context.TkdMihrim.Where(_ => _.UkeNo == futTum.UkeNo
                                                                    && _.SihFutSyu >= (mFutSyu == 6 ? 6 : 2)
                                                                    && _.SihFutSyu <= (mFutSyu == 6 ? 6 : 5)
                                                                    && _.UnkRen == futTum.UnkRen
                                                                    && _.YouFutTumRen == futTum.FutTumRen
                                                                    && _.SiyoKbn == CommonConstants.SiyoKbn
                                                                 );

                    var listYouTblSeqFutTuBefore = listFutTuBefore.Select(_ => _.YouTblSeq).ToList();

                    if (listYouTblSeqFutTuBefore.Count > 0)
                    {
                        listMihrimBefore.AddRange(mihrimSql.Where(_ => listYouTblSeqFutTuBefore.Contains(_.YouTblSeq)).ToList());
                    }

                    var listYouTblSeqFutTuAfter = listFutTuAfter.Select(_ => _.YouTblSeq).ToList();

                    if (listYouTblSeqFutTuAfter.Count > 0)
                    {
                        listMihrimAfter.AddRange(mihrimSql.Where(_ => listYouTblSeqFutTuBefore.Contains(_.YouTblSeq)).ToList());

                        if (futTum.SeisanKbn == 1)
                        {
                            listMihrimAfter.ForEach(mihrim =>
                            {
                                UpdateMihrim(0, mihrim, currentDate);
                            });
                        }
                        else
                        {
                            listMihrimAfter.ForEach(mihrim =>
                            {
                                UpdateMihrim(1, mihrim, currentDate);
                            });
                        }
                    }
                }
            }

            private void MapFuttu(TkdYfutTu futTu, TkdFutTum futTum, DateTime currentDate)
            {
                futTu.Nittei = futTum.Nittei;
                futTu.TomKbn = futTum.TomKbn;
                futTu.FutTumCdSeq = futTum.FutTumCdSeq;
                futTu.FutTumNm = futTum.FutTumNm;
                futTu.HasYmd = futTum.HasYmd;
                futTu.IriRyoChiCd = futTum.IriRyoChiCd;
                futTu.IriRyoCd = futTum.IriRyoCd;
                futTu.IriRyoNm = futTum.IriRyoNm;
                futTu.DeRyoChiCd = futTum.DeRyoChiCd;
                futTu.DeRyoCd = futTum.DeRyoCd;
                futTu.DeRyoNm = futTum.DeRyoNm;
                futTu.SeisanCdSeq = futTum.SeisanCdSeq;
                futTu.SeisanNm = futTum.SeisanNm;
                futTu.SeisanKbn = futTum.SeisanKbn;
                futTu.TanKa = futTum.TanKa;
                futTu.Suryo = 0;
                futTu.HaseiKin = 0;
                futTu.ZeiKbn = futTum.ZeiKbn;
                futTu.Zeiritsu = futTum.Zeiritsu;
                futTu.SyaRyoSyo = 0;
                futTu.TesuRitu = futTum.TesuRitu;
                futTu.SyaRyoTes = 0;
                futTu.SihKbn = 1;
                futTu.ScouKbn = 1;
                futTu.SiyoKbn = CommonConstants.SiyoKbn;
                futTu.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                futTu.UpdTime = currentDate.ToString(CommonConstants.FormatHms);
                futTu.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                futTu.UpdPrgId = Constants.ETCTransferUpdPrgIdKK9210P;
            }

            private void UpdateMihrim(byte type, TkdMihrim mihrim, DateTime currentDate)
            {
                if (type == 0)
                {
                    mihrim.HaseiKin = 0;
                    mihrim.SyaRyoSyo = 0;
                    mihrim.SyaRyoTes = 0;
                    mihrim.YoushaGak = 0;
                    mihrim.SiyoKbn = 1;
                }
                else
                {
                    mihrim.SiyoKbn = 2;
                }
                mihrim.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                mihrim.UpdTime = currentDate.ToString(CommonConstants.FormatHms);
                mihrim.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                mihrim.UpdPrgId = Constants.ETCTransferUpdPrgIdKK9210P;
            }

            private void ps07DeleteFutTu01(List<TkdMishum> listMishum, List<TkdMfutTu> listMFutTuInsert, List<TkdMfutTu> listMFutTuUpdate,
                List<TkdYfutTu> listYFutTuBefore, List<TkdMihrim> listMihrimBefore, DateTime currentDate)
            {
                listMishum.ForEach(mishum =>
                {
                    mishum.SiyoKbn = 2;
                    mishum.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                    mishum.UpdTime = currentDate.ToString(CommonConstants.FormatHms);
                    mishum.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    mishum.UpdPrgId = Constants.ETCTransferUpdPrgIdKK9210P;
                });

                listMFutTuUpdate.ForEach(mfuttu =>
                {
                    mfuttu.Suryo = 0;
                    mfuttu.UriGakKin = 0;
                    mfuttu.SyaRyoSyo = 0;
                    mfuttu.SyaRyoTes = 0;
                    mfuttu.SiyoKbn = 2;
                    mfuttu.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                    mfuttu.UpdTime = currentDate.ToString(CommonConstants.FormatHms);
                    mfuttu.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    mfuttu.UpdPrgId = Constants.ETCTransferUpdPrgIdKK9210P;
                });

                listMFutTuInsert.ForEach(mfuttu =>
                {
                    mfuttu.Suryo = 0;
                    mfuttu.UriGakKin = 0;
                    mfuttu.SyaRyoSyo = 0;
                    mfuttu.SyaRyoTes = 0;
                    mfuttu.SiyoKbn = 2;
                    mfuttu.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                    mfuttu.UpdTime = currentDate.ToString(CommonConstants.FormatHms);
                    mfuttu.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    mfuttu.UpdPrgId = Constants.ETCTransferUpdPrgIdKK9210P;
                });

                _context.TkdMfutTu.AddRange(listMFutTuInsert);

                listYFutTuBefore.ForEach(yfuttu =>
                {
                    yfuttu.SiyoKbn = 2;
                    yfuttu.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                    yfuttu.UpdTime = currentDate.ToString(CommonConstants.FormatHms);
                    yfuttu.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    yfuttu.UpdPrgId = Constants.ETCTransferUpdPrgIdKK9210P;
                });

                listMihrimBefore.ForEach(mihrim =>
                {
                    mihrim.SiyoKbn = 2;
                    mihrim.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                    mihrim.UpdTime = currentDate.ToString(CommonConstants.FormatHms);
                    mihrim.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    mihrim.UpdPrgId = Constants.ETCTransferUpdPrgIdKK9210P;
                });
            }

            private void ps11UriDivide(TkdFutTum futTum, List<TkdMfutTu> listMFutTu, DateTime currentDate, List<TkdMfutTu> listMFutTuUpdate, List<TkdMfutTu> listMFutTuInsert)
            {
                var listMFutTuTemp11 = listMFutTu.Where(_ => _.UkeNo == futTum.UkeNo
                                                          && _.UnkRen == futTum.UnkRen
                                                          && _.FutTumKbn == futTum.FutTumKbn
                                                          && _.FutTumRen == futTum.FutTumRen
                                                          && _.SiyoKbn == CommonConstants.SiyoKbn).ToList();

                if (listMFutTuTemp11.Count > 0)
                {
                    var listFutTum1 = new List<ETCFutTum>();

                    listMFutTuTemp11.ForEach(mfuttu =>
                    {
                        var futtumTemp = new ETCFutTum()
                        {
                            TeiDanNo = mfuttu.TeiDanNo,
                            BunkRen = mfuttu.BunkRen,
                            Suryo = mfuttu.Suryo,
                            Num = futTum.ZeiKbn != 2 ? mfuttu.UriGakKin : mfuttu.UriGakKin + mfuttu.SyaRyoSyo,
                            SyaRyoSyo = mfuttu.SyaRyoSyo,
                            SyaRyoTes = mfuttu.SyaRyoTes
                        };
                        listFutTum1.Add(futtumTemp);
                    });

                    var vintUriGaku = futTum.ZeiKbn != 2 ? futTum.UriGakKin : futTum.UriGakKin + futTum.SyaRyoSyo;

                    int vintTankaGaku1 = (int)Math.Round(vintUriGaku * 1.0 / futTum.Suryo);
                    int rintOvreGaku1 = vintUriGaku - vintTankaGaku1 * futTum.Suryo;

                    int vintTankaGaku2 = (int)Math.Round(futTum.SyaRyoSyo * 1.0 / futTum.Suryo);
                    int rintOvreGaku2 = futTum.SyaRyoSyo - vintTankaGaku2 * futTum.Suryo;

                    int vintTankaGaku3 = (int)Math.Round(futTum.SyaRyoTes * 1.0 / futTum.Suryo);
                    int rintOvreGaku3 = futTum.SyaRyoTes - vintTankaGaku3 * futTum.Suryo;

                    listFutTum1.OrderByDescending(_ => _.Num).ThenBy(_ => _.TeiDanNo).ThenBy(_ => _.BunkRen).ToList().ForEach(futtum1 =>
                    {
                        if (futTum.Suryo == 0)
                        {
                            futtum1.Num = 0;
                            futtum1.SyaRyoSyo = 0;
                            futtum1.SyaRyoTes = 0;
                        }
                        else
                        {
                            var vintChoseiGaku = Math.Abs(futtum1.Suryo);
                            futtum1.Num = KingakuChosei(vintChoseiGaku, vintTankaGaku1, futtum1.Suryo, ref rintOvreGaku1);
                            futtum1.SyaRyoSyo = KingakuChosei(vintChoseiGaku, vintTankaGaku1, futtum1.Suryo, ref rintOvreGaku2);
                            futtum1.SyaRyoTes = KingakuChosei(vintChoseiGaku, vintTankaGaku1, futtum1.Suryo, ref rintOvreGaku3);
                        }
                    });

                    listFutTum1.ForEach(futtum1 =>
                    {
                        var mfuttuTemp = listMFutTuUpdate.FirstOrDefault(_ => _.TeiDanNo == futtum1.TeiDanNo
                                                                           && _.BunkRen == futtum1.BunkRen);

                        if (mfuttuTemp == null)
                        {
                            mfuttuTemp = listMFutTuInsert.FirstOrDefault(_ => _.TeiDanNo == futtum1.TeiDanNo
                                                                           && _.BunkRen == futtum1.BunkRen);
                        }

                        if (mfuttuTemp != null)
                        {
                            mfuttuTemp.UriGakKin = futTum.ZeiKbn == 2 ? futtum1.Num : futtum1.Num - futtum1.SyaRyoSyo;
                            mfuttuTemp.SyaRyoSyo = futtum1.SyaRyoSyo;
                            mfuttuTemp.SyaRyoTes = futtum1.SyaRyoTes;
                            mfuttuTemp.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                            mfuttuTemp.UpdTime = currentDate.ToString(CommonConstants.FormatHms);
                            mfuttuTemp.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            mfuttuTemp.UpdPrgId = Constants.ETCTransferUpdPrgIdKK9210P;
                        }
                    });
                }
            }

            private void ps12Kasan(TkdFutTum futTum, List<TkdMfutTu> listMFutTu, List<ETCHaisha> listHaisha, List<TkdYfutTu> listYFutTuBefore, List<TkdYfutTu> listYFutTuAfter,
                List<TkdYfutTu> listYFutTuAfterInsert, List<ETCJitaYouTblSeq> listJitaYouTblSeqAfter, List<TkdMihrim> listMihrimAfter, List<TkdMihrim> listMihrimAfterInsert,
                byte mFutSyu, DateTime currentDate, short maxMihRen)
            {
                var listMFutTuTemp12 = listMFutTu.Where(_ => _.SiyoKbn == CommonConstants.SiyoKbn).ToList();

                listMFutTuTemp12.ForEach(mfuttu =>
                {
                    var listAfterHaishaData = listHaisha.Where(_ => _.UkeNo == mfuttu.UkeNo
                                                                 && _.UnkRen == mfuttu.UnkRen
                                                                 && _.TeiDanNo == mfuttu.TeiDanNo
                                                                 && _.BunkRen == mfuttu.BunkRen).ToList();

                    listAfterHaishaData.ForEach(haisha =>
                    {
                        var countListYfuttuBefore = listYFutTuBefore.Count(_ => _.UkeNo == mfuttu.UkeNo
                                                                             && _.UnkRen == mfuttu.UnkRen
                                                                             && _.YouTblSeq == haisha.You_YouTblSeq
                                                                             && _.FutTumKbn == mfuttu.FutTumKbn
                                                                             && _.YouFutTumRen == mfuttu.FutTumRen
                                                                             && (_.SihKbn != 1 || _.ScouKbn != 1));

                        if (countListYfuttuBefore == 0)
                        {
                            var listYfuttuAfter = listYFutTuAfter.Where(_ => _.UkeNo == mfuttu.UkeNo
                                                                          && _.UnkRen == mfuttu.UnkRen
                                                                          && _.YouTblSeq == haisha.You_YouTblSeq
                                                                          && _.FutTumKbn == mfuttu.FutTumKbn
                                                                          && _.YouFutTumRen == mfuttu.FutTumRen).ToList();

                            if (listYfuttuAfter.Count == 0)
                            {
                                var futtuItem = MapTkdYfutTu(futTum, mfuttu, haisha, currentDate);
                                listYFutTuAfter.Add(futtuItem);
                                listYFutTuAfterInsert.Add(futtuItem);
                            }
                            else
                            {
                                listYfuttuAfter[0].HaseiKin = listYfuttuAfter[0].HaseiKin + mfuttu.UriGakKin;
                                listYfuttuAfter[0].SyaRyoSyo = listYfuttuAfter[0].SyaRyoSyo + mfuttu.SyaRyoSyo;
                                listYfuttuAfter[0].SyaRyoTes = listYfuttuAfter[0].SyaRyoTes + mfuttu.SyaRyoTes;
                                listYfuttuAfter[0].UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                                listYfuttuAfter[0].UpdTime = currentDate.ToString(CommonConstants.FormatHms);
                                listYfuttuAfter[0].UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                                listYfuttuAfter[0].UpdPrgId = Constants.ETCTransferUpdPrgIdKK9210P;
                                listYfuttuAfter[0].Suryo = (short)(listYfuttuAfter[0].Suryo + mfuttu.Suryo);
                            }
                        }
                    });
                });

                if (futTum.SeisanKbn == 1)
                {
                    var listYfuttuAfter = listYFutTuAfter.Where(_ => _.UkeNo == futTum.UkeNo
                                                                  && _.UnkRen == futTum.UnkRen
                                                                  && _.FutTumKbn == futTum.FutTumKbn
                                                                  && _.YouFutTumRen == futTum.FutTumRen).ToList();

                    listYfuttuAfter.ForEach(yfuttu =>
                    {
                        var jitaYouTblSeqAfter = listJitaYouTblSeqAfter.Where(_ => _.YouTblSeq == yfuttu.YouTblSeq).ToList();
                        if (jitaYouTblSeqAfter.Count > 0)
                        {
                            var TesKbn = _context.VpmTokiSt.FirstOrDefault(_ => _.TokuiSeq == jitaYouTblSeqAfter[0].YouCdSeq
                                                                             && _.SitenCdSeq == jitaYouTblSeqAfter[0].YouSitCdSeq
                                                                             && _.SiyoStaYmd.CompareTo(jitaYouTblSeqAfter[0].HasYmd) < 1
                                                                             && _.SiyoEndYmd.CompareTo(jitaYouTblSeqAfter[0].HasYmd) > -1);

                            if (TesKbn != null)
                            {
                                int rpTesKbnFut = TesKbn.TesKbnFut;
                                int rpTesKbnGui = TesKbn.TesKbnGui;
                                var listMihrimTemp = listMihrimAfter.Where(_ => _.UkeNo == yfuttu.UkeNo
                                                                             && _.UnkRen == yfuttu.UnkRen
                                                                             && _.YouFutTumRen == yfuttu.YouFutTumRen
                                                                             && _.YouTblSeq == yfuttu.YouTblSeq
                                                                             && _.SihFutSyu >= (mFutSyu == 6 ? 6 : 2)
                                                                             && _.SihFutSyu <= (mFutSyu == 6 ? 6 : 5)).ToList();

                                if (listMihrimTemp.Count == 0)
                                {
                                    if (yfuttu.HaseiKin != 0 || yfuttu.SyaRyoSyo != 0 || yfuttu.SyaRyoTes != 0)
                                    {
                                        var mihrimTemp = MapTkdMihrim(yfuttu, mFutSyu, rpTesKbnFut, rpTesKbnGui, currentDate, ++maxMihRen);
                                        listMihrimAfterInsert.Add(mihrimTemp);
                                    }
                                }
                                else if (yfuttu.HaseiKin != 0 || yfuttu.SyaRyoSyo != 0 || yfuttu.SyaRyoTes != 0)
                                {
                                    int num = listMihrimTemp[0].HaseiKin + yfuttu.HaseiKin + listMihrimTemp[0].SyaRyoSyo + yfuttu.SyaRyoSyo;
                                    bool flag = true;
                                    if (flag != (mFutSyu == 5 && rpTesKbnGui == 1))
                                    {
                                        if (flag == (mFutSyu == 5 && rpTesKbnGui == 2)
                                        || flag == (mFutSyu != 5 && rpTesKbnFut == 1)
                                        || flag == (mFutSyu != 5 && rpTesKbnFut == 2))
                                        {
                                            num -= (yfuttu.SyaRyoTes + listMihrimTemp[0].SyaRyoTes);
                                        }
                                    }
                                    UpdateMihrim(0, listMihrimTemp[0], yfuttu, currentDate, num);
                                }
                                else
                                {
                                    UpdateMihrim(1, listMihrimTemp[0], yfuttu, currentDate, 0);
                                }
                            }
                        }
                    });
                }
            }

            private TkdYfutTu MapTkdYfutTu(TkdFutTum futTum, TkdMfutTu mfuttu, ETCHaisha haisha, DateTime currentDate)
            {
                var item = new TkdYfutTu();
                item.Nittei = futTum.Nittei;
                item.TomKbn = futTum.TomKbn;
                item.FutTumCdSeq = futTum.FutTumCdSeq;
                item.FutTumNm = futTum.FutTumNm;
                item.HasYmd = futTum.HasYmd;
                item.IriRyoChiCd = futTum.IriRyoChiCd;
                item.IriRyoCd = futTum.IriRyoCd;
                item.IriRyoNm = futTum.IriRyoNm;
                item.DeRyoChiCd = futTum.DeRyoChiCd;
                item.DeRyoCd = futTum.DeRyoCd;
                item.DeRyoNm = futTum.DeRyoNm;
                item.SeisanCdSeq = futTum.SeisanCdSeq;
                item.SeisanNm = futTum.SeisanNm;
                item.SeisanKbn = futTum.SeisanKbn;
                item.TanKa = futTum.TanKa;
                item.ZeiKbn = futTum.ZeiKbn;
                item.Zeiritsu = futTum.Zeiritsu;
                item.TesuRitu = futTum.TesuRitu;
                item.SihKbn = 1;
                item.ScouKbn = 1;
                item.SiyoKbn = 1;
                item.UkeNo = mfuttu.UkeNo;
                item.UnkRen = mfuttu.UnkRen;
                item.YouTblSeq = haisha.You_YouTblSeq;
                item.FutTumKbn = mfuttu.FutTumKbn;
                item.YouFutTumRen = mfuttu.FutTumRen;
                item.HenKai = 0;
                item.HaseiKin = mfuttu.UriGakKin;
                item.SyaRyoSyo = mfuttu.SyaRyoSyo;
                item.SyaRyoTes = mfuttu.SyaRyoTes;
                item.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                item.UpdTime = currentDate.ToString(CommonConstants.FormatHms);
                item.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                item.UpdPrgId = Constants.ETCTransferUpdPrgIdKK9210P;
                item.Suryo = mfuttu.Suryo;
                return item;
            }

            private TkdMihrim MapTkdMihrim(TkdYfutTu yfuttu, byte mFutSyu, int rpTesKbnGui, int rpTesKbnFut, DateTime currentDate, short maxMihRen)
            {
                var mihrimTemp = new TkdMihrim();
                mihrimTemp.UkeNo = yfuttu.UkeNo;
                mihrimTemp.MihRen = maxMihRen;
                mihrimTemp.HenKai = 0;
                mihrimTemp.SihFutSyu = mFutSyu;
                mihrimTemp.UnkRen = yfuttu.UnkRen;
                mihrimTemp.YouTblSeq = yfuttu.YouTblSeq;
                mihrimTemp.HaseiKin = yfuttu.HaseiKin;
                mihrimTemp.SyaRyoSyo = yfuttu.SyaRyoSyo;
                mihrimTemp.SyaRyoTes = yfuttu.SyaRyoTes;
                var num = yfuttu.HaseiKin + yfuttu.SyaRyoSyo;
                bool flag = true;
                if (flag != (mFutSyu == 5 && rpTesKbnGui == 1))
                {
                    if (flag == (mFutSyu == 5 && rpTesKbnGui == 2)
                    || flag == (mFutSyu != 5 && rpTesKbnFut == 1)
                    || flag == (mFutSyu != 5 && rpTesKbnFut == 2))
                    {
                        num -= yfuttu.SyaRyoTes;
                    }
                }
                mihrimTemp.YoushaGak = num;
                mihrimTemp.SihRaiRui = 0;
                mihrimTemp.CouKesRui = 0;
                mihrimTemp.YouFutTumRen = yfuttu.YouFutTumRen;
                mihrimTemp.SiyoKbn = CommonConstants.SiyoKbn;
                mihrimTemp.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                mihrimTemp.UpdTime = currentDate.ToString(CommonConstants.FormatHms);
                mihrimTemp.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                mihrimTemp.UpdPrgId = Constants.ETCTransferUpdPrgIdKK9210P;
                return mihrimTemp;
            }

            private void UpdateMihrim(byte type, TkdMihrim mihrimTemp, TkdYfutTu yfuttu, DateTime currentDate, int num)
            {
                if (type == 0)
                {
                    mihrimTemp.HaseiKin = mihrimTemp.HaseiKin + yfuttu.HaseiKin;
                    mihrimTemp.SyaRyoSyo = mihrimTemp.SyaRyoSyo + yfuttu.SyaRyoSyo;
                    mihrimTemp.SyaRyoTes = mihrimTemp.SyaRyoTes + yfuttu.SyaRyoTes;
                    mihrimTemp.YoushaGak = num;
                    mihrimTemp.SiyoKbn = CommonConstants.SiyoKbn;
                }
                else
                {
                    mihrimTemp.HaseiKin = 0;
                    mihrimTemp.SyaRyoSyo = 0;
                    mihrimTemp.SyaRyoTes = 0;
                    mihrimTemp.YoushaGak = 0;
                    mihrimTemp.SiyoKbn = 2;
                }
                mihrimTemp.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                mihrimTemp.UpdTime = currentDate.ToString(CommonConstants.FormatHms);
                mihrimTemp.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                mihrimTemp.UpdPrgId = Constants.ETCTransferUpdPrgIdKK9210P;
            }
        }
    }
}
