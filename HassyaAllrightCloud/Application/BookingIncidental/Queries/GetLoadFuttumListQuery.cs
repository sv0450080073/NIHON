using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BookingIncidental.Queries
{
    public class GetLoadFuttumListQuery : IRequest<List<LoadFuttum>>
    {
        private readonly string _ukeNo;
        private readonly IncidentalViewMode _viewMode;
        private readonly IncidentalBooking _incidental;

        public GetLoadFuttumListQuery(string ukeNo, IncidentalViewMode viewMode, IncidentalBooking incidental = null)
        {
            _ukeNo = ukeNo;
            _viewMode = viewMode;
            _incidental = incidental;
        }

        public class Handler : IRequestHandler<GetLoadFuttumListQuery, List<LoadFuttum>>
        {
            private readonly KobodbContext _context;
            private readonly IRoundSettingsService _roundSettingsService;

            public Handler(KobodbContext context, IRoundSettingsService roundSettingsService)
            {
                _context = context;
                _roundSettingsService = roundSettingsService;
            }

            /// <summary>
            /// Get futtum list for futai component
            /// </summary>
            /// <param name="ukeNo"></param>
            /// <param name="viewMode">Futai or Tsumi or get All</param>
            /// <param name="incidental">, if get for booking input tab => not need to pass this parameter</param>
            /// <returns>List<LoadFuttum></returns>
            public async Task<List<LoadFuttum>> Handle(GetLoadFuttumListQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var viewMode = request._viewMode;
                    var ukeNo = request._ukeNo;
                    var incidental = request._incidental;

                    var result = new List<LoadFuttum>();
                    var tkdFuttumList = _context.TkdFutTum.Where(f => f.UkeNo == ukeNo && f.SiyoKbn == 1).OrderBy(f => f.Nittei).ToList();
                    if (viewMode != IncidentalViewMode.All)
                    {
                        tkdFuttumList = tkdFuttumList.Where(f => f.FutTumKbn == (int)viewMode).ToList();
                    }
                    //var seisanKbnList = await _context.VpmCodeKb.Where(c => c.CodeSyu == "SEISANKBN" && c.TenantCdSeq == new ClaimModel().TenantID).ToListAsync();
                    var mFuttuList = await _context.TkdMfutTu.Where(e => e.UkeNo == ukeNo)
                        .Select(e => new { e.FutTumRen, e.TeiDanNo, e.Suryo, e.FutTumKbn, e.BunkRen, e.UnkRen }).ToListAsync();
                    //var roundCode = _context.TkmKasSet.SingleOrDefault(c => c.CompanyCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID).SyohiHasu;
                    var roundCode = (byte)(await _roundSettingsService.GetHasuSettings(new ClaimModel().CompanyID)).TaxSetting;
                    int index = 0;
                    foreach (var tkdFuttumItem in tkdFuttumList)
                    {
                        var loadFuttum = new LoadFuttum();

                        loadFuttum.FutTumRen = tkdFuttumItem.FutTumRen;
                        loadFuttum.Index = ++index;
                        loadFuttum.CanDelete = tkdFuttumItem.NyuKinKbn == 1 && tkdFuttumItem.NcouKbn == 1;
                        loadFuttum.RoundType = (RoundTaxAmountType)roundCode;
                        loadFuttum.FuttumKbnMode = (IncidentalViewMode)tkdFuttumItem.FutTumKbn;
                        loadFuttum.ScheduleDate = new Commons.Helpers.ScheduleSelectorModel();
                        var tempDt = new DateTime();
                        if (DateTime.TryParseExact(tkdFuttumItem.HasYmd, "yyyyMMdd", null, DateTimeStyles.None, out tempDt))
                        {
                            loadFuttum.ScheduleDate.Date = tempDt;
                            loadFuttum.ScheduleDate.Nittei = tkdFuttumItem.Nittei;
                            loadFuttum.ScheduleDate.TomKbn = tkdFuttumItem.TomKbn;
                        }
                        loadFuttum.TaxType = new TaxTypeList();
                        loadFuttum.TaxType.IdValue = tkdFuttumItem.ZeiKbn;
                        loadFuttum.SirTaxType = new TaxTypeList();
                        loadFuttum.SirTaxType.IdValue = tkdFuttumItem.SirZeiKbn;
                        loadFuttum.FutTumNm = tkdFuttumItem.FutTumNm;
                        loadFuttum.SeisanNm = tkdFuttumItem.SeisanNm;
                        loadFuttum.SelectedLoadSeisanKbn = new LoadSeisanKbn() { CodeKbn = tkdFuttumItem.SeisanKbn.ToString() };
                        loadFuttum.IriRyoNm = tkdFuttumItem.IriRyoNm;
                        loadFuttum.DeRyoNm = tkdFuttumItem.DeRyoNm;
                        loadFuttum.Suryo = tkdFuttumItem.Suryo.ToString();
                        loadFuttum.Tanka = tkdFuttumItem.TanKa.ToString();
                        //loadFuttum.UriGakKin = tkdFuttumItem.UriGakKin.ToString();
                        loadFuttum.Zeiritsu = tkdFuttumItem.Zeiritsu.ToString();
                        //loadFuttum.SyaRyoSyo = tkdFuttumItem.SyaRyoSyo.ToString();
                        loadFuttum.TesuRitu = tkdFuttumItem.TesuRitu.ToString();
                        //loadFuttum.SyaRyoTes = tkdFuttumItem.SyaRyoTes.ToString();
                        loadFuttum.NoteInput = tkdFuttumItem.BikoNm;
                        loadFuttum.SirSuryo = tkdFuttumItem.SirSuryo.ToString();
                        loadFuttum.SirTanka = tkdFuttumItem.SirTanKa.ToString();
                        //loadFuttum.SirGakKin = tkdFuttumItem.SirGakKin.ToString();
                        loadFuttum.SirZeiritsu = tkdFuttumItem.SirZeiritsu.ToString();
                        //loadFuttum.SirSyaRyoSyo = tkdFuttumItem.SirSyaRyoSyo.ToString();

                        loadFuttum.SelectedLoadNyuRyokinName = new LoadNyuRyokinName();
                        loadFuttum.SelectedLoadNyuRyokinName.RyokinTikuCd = tkdFuttumItem.IriRyoChiCd.ToString();
                        loadFuttum.SelectedLoadNyuRyokinName.RyokinCd = tkdFuttumItem.IriRyoCd;

                        loadFuttum.SelectedLoadShuRyokin = new LoadNyuRyokinName();
                        loadFuttum.SelectedLoadShuRyokin.RyokinTikuCd = tkdFuttumItem.DeRyoChiCd.ToString();
                        loadFuttum.SelectedLoadShuRyokin.RyokinCd = tkdFuttumItem.DeRyoCd;


                        loadFuttum.SelectedLoadSeisanCd = new LoadSeisanCd();
                        loadFuttum.SelectedLoadSeisanCd.SeisanCdSeq = tkdFuttumItem.SeisanCdSeq;

                        loadFuttum.SelectedCustomer = new LoadCustomerList();
                        loadFuttum.SelectedCustomer.SitenCdSeq = tkdFuttumItem.SirSitenCdSeq;
                        loadFuttum.SelectedCustomer.TokuiSeq = tkdFuttumItem.SireCdSeq;

                        if ((IncidentalViewMode)tkdFuttumItem.FutTumKbn == IncidentalViewMode.Futai)
                        {
                            loadFuttum.SelectedLoadFutai = new LoadFutai();
                            loadFuttum.SelectedLoadFutai.FutaiCdSeq = tkdFuttumItem.FutTumCdSeq;
                        }
                        else
                        {
                            loadFuttum.SelectedLoadTsumi = new LoadTsumi();
                            loadFuttum.SelectedLoadTsumi.CodeKbnSeq = tkdFuttumItem.FutTumCdSeq;
                        }

                        if (incidental != null)
                        {
                            foreach (var item in incidental.SettingQuantityList)
                            {
                                if (loadFuttum.ScheduleDate.Date.IsInRange(item.GarageLeaveDate, item.GarageReturnDate))
                                {
                                    var settingQuantity = new SettingQuantity();
                                    settingQuantity.SimpleCloneProperties(item);
                                    var quantity = mFuttuList.SingleOrDefault(e => e.TeiDanNo == item.TeiDanNo
                                                                && e.UnkRen == item.UnkRen
                                                                && e.BunkRen == item.BunkRen
                                                                && e.FutTumRen == tkdFuttumItem.FutTumRen
                                                                && tkdFuttumItem.FutTumKbn == e.FutTumKbn)?.Suryo ?? 0;
                                    settingQuantity.Suryo = quantity.ToString();
                                    loadFuttum.SettingQuantityList.Add(settingQuantity);
                                }
                            }
                        }

                        result.Add(loadFuttum);
                    }

                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
