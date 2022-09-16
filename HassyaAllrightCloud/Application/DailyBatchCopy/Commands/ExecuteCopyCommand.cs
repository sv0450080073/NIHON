using HassyaAllrightCloud.Commons.Constants;
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

namespace HassyaAllrightCloud.Application.DailyBatchCopy.Commands
{
    public class ExecuteCopyCommand : IRequest<bool>
    {
        public List<string> listDate { get; set; }
        public List<DailyBatchCopyData> listData { get; set; }
        public DailyBatchCopySearchModel searchModel { get; set; }
        public class Handler : IRequestHandler<ExecuteCopyCommand, bool>
        {
            private readonly KobodbContext _context;
            private string UpdYmd = DateTime.Now.ToString(CommonConstants.FormatYMD);
            private string UpdTime = DateTime.Now.ToString(CommonConstants.FormatHMS);
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(ExecuteCopyCommand request, CancellationToken cancellationToken)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var searchModel = request.searchModel;
                        var maxUkeCode = _context.TkdYyksho.Max(_ => _.UkeCd);

                        foreach (var item in request.listData)
                        {
                            foreach(var date in request.listDate)
                            {
                                var inputDate = date.Replace("/", string.Empty);
                                var yyksho = _context.TkdYyksho.FirstOrDefault(_ => _.UkeNo == item.UkeNo);
                                if (yyksho != null)
                                {
                                    TkdYyksho model = new TkdYyksho();
                                    MapYyksho(model, yyksho, inputDate, ref maxUkeCode);
                                    _context.TkdYyksho.Add(model);

                                    ExecuteUnkobi(item.UkeNo, model.UkeNo, inputDate);

                                    ExecuteYykSyu(item.UkeNo, model.UkeNo);

                                    ExecuteHaisha(item.UkeNo, model.UkeNo, inputDate);

                                    ExecuteMishum(item.UkeNo, model.UkeNo);

                                    //await Task.WhenAll(taskUnkobi, taskYykSyu, taskHaisha, taskMishum);

                                    if (searchModel.IsProcess)
                                    {
                                        ExecuteProcess(item.UkeNo, model.UkeNo);
                                    }

                                    if (searchModel.IsArrangement)
                                    {
                                        ExecuteArrangement(item.UkeNo, model.UkeNo);
                                    }

                                    if (searchModel.IsLoadedGoods)
                                    {
                                        ExecuteLoadedGoods(item.UkeNo, model.UkeNo, inputDate);
                                    }

                                    if (searchModel.IsIncidental)
                                    {
                                        ExecuteIncidental(item.UkeNo, model.UkeNo, inputDate);
                                    }
                                }
                            }
                        }
                        
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }

            private void MapYyksho(TkdYyksho model, TkdYyksho yyksho, string startDate, ref int maxUkeCode)
            {
                model.TenantCdSeq       = yyksho.TenantCdSeq;
                maxUkeCode = maxUkeCode + 1;
                model.UkeNo             = string.Format($"{model.TenantCdSeq:00000}{maxUkeCode:0000000000}");
                model.UkeCd             = maxUkeCode;
                model.HenKai            = 0;
                model.UkeYmd            = yyksho.UkeYmd;
                model.YoyaSyu           = yyksho.YoyaSyu;
                model.YoyaKbnSeq        = yyksho.YoyaKbnSeq;
                model.KikakuNo          = yyksho.KikakuNo;
                model.TourCd            = yyksho.TourCd;
                model.KasTourCdSeq      = yyksho.KasTourCdSeq;
                model.UkeEigCdSeq       = yyksho.UkeEigCdSeq;
                model.SeiEigCdSeq       = yyksho.SeiEigCdSeq;
                model.IraEigCdSeq       = yyksho.IraEigCdSeq;
                model.EigTanCdSeq       = yyksho.EigTanCdSeq;
                model.InTanCdSeq        = yyksho.InTanCdSeq;
                model.YoyaNm            = yyksho.YoyaNm;
                model.YoyaKana          = yyksho.YoyaKana;
                model.TokuiSeq          = yyksho.TokuiSeq;
                model.SitenCdSeq        = yyksho.SitenCdSeq;
                model.SirCdSeq          = yyksho.SirCdSeq;
                model.SirSitenCdSeq     = yyksho.SirSitenCdSeq;
                model.TokuiTel          = yyksho.TokuiTel;
                model.TokuiTanNm        = yyksho.TokuiTanNm;
                model.TokuiFax          = yyksho.TokuiFax;
                model.TokuiMail         = yyksho.TokuiMail;
                model.UntKin            = yyksho.UntKin;
                model.ZeiKbn            = yyksho.ZeiKbn;
                model.Zeiritsu          = yyksho.Zeiritsu;
                model.ZeiRui            = yyksho.ZeiRui;
                model.TaxTypeforGuider  = yyksho.TaxTypeforGuider;
                model.TaxGuider         = yyksho.TaxGuider;
                model.TesuRitu          = yyksho.TesuRitu;
                model.TesuRyoG          = yyksho.TesuRyoG;
                model.FeeGuiderRate     = yyksho.FeeGuiderRate;
                model.FeeGuider         = yyksho.FeeGuider;
                model.GuitKin           = yyksho.GuitKin;
                model.SeiKyuKbnSeq      = yyksho.SeiKyuKbnSeq;
                model.SeikYm            = startDate.Substring(0, 6);
                model.SeiTaiYmd         = startDate;
                model.CanRit            = yyksho.CanRit;
                model.CanUnc            = yyksho.CanUnc;
                model.CanZkbn           = yyksho.CanZkbn;
                model.CanSyoR           = yyksho.CanSyoR;
                model.CanSyoG           = yyksho.CanSyoG;
                model.CanYmd            = yyksho.CanYmd;
                model.CanTanSeq         = yyksho.CanTanSeq;
                model.CanRiy            = yyksho.CanRiy;
                model.CanFuYmd          = yyksho.CanFuYmd;
                model.CanFuTanSeq       = yyksho.CanFuTanSeq;
                model.CanFuRiy          = yyksho.CanFuRiy;
                model.BikoTblSeq        = yyksho.BikoTblSeq;
                model.Kskbn             = yyksho.Kskbn;
                model.KhinKbn           = yyksho.KhinKbn;
                model.KaknKais          = yyksho.KaknKais;
                model.KaktYmd           = yyksho.KaktYmd;
                model.HaiSkbn           = yyksho.HaiSkbn;
                model.HaiIkbn           = yyksho.HaiIkbn;
                model.GuiWnin           = yyksho.GuiWnin;
                model.NippoKbn          = yyksho.NippoKbn;
                model.YouKbn            = yyksho.YouKbn;
                model.NyuKinKbn         = yyksho.NyuKinKbn;
                model.NcouKbn           = yyksho.NcouKbn;
                model.SihKbn            = yyksho.SihKbn;
                model.ScouKbn           = yyksho.ScouKbn;
                model.SiyoKbn           = yyksho.SiyoKbn;
                model.UpdYmd            = UpdYmd;
                model.UpdTime           = UpdTime;
                model.UpdSyainCd        = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                model.UpdPrgId          = yyksho.UpdPrgId;
            }

            private void ExecuteUnkobi(string UkeNo, string ukeNo, string startDate)
            {
                var listUnkobi = _context.TkdUnkobi.Where(_ => _.UkeNo == UkeNo).ToList();
                foreach(var unkobi in listUnkobi)
                {
                    TkdUnkobi model = new TkdUnkobi();
                    MapUnkobi(model, unkobi, ukeNo, startDate);
                    _context.TkdUnkobi.Add(model);
                }
            }

            private void MapUnkobi(TkdUnkobi model, TkdUnkobi unkobi, string ukeNo, string startDate)
            {
                model.UkeNo                     = ukeNo;
                model.UnkRen                    = unkobi.UnkRen;
                model.HenKai                    = 0;
                model.HaiSymd                   = startDate;
                model.HaiStime                  = unkobi.HaiStime;
                var Origin                      = (DateTime.ParseExact(unkobi.TouYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture) - DateTime.ParseExact(unkobi.HaiSymd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture)).TotalDays;
                model.TouYmd                    = DateTime.ParseExact(startDate, CommonConstants.FormatYMD, CultureInfo.InvariantCulture).AddDays(Origin).ToString(CommonConstants.FormatYMD);
                model.TouChTime                 = unkobi.TouChTime;
                var OriginDispTou               = (DateTime.ParseExact(unkobi.DispTouYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture) - DateTime.ParseExact(unkobi.HaiSymd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture)).TotalDays;
                model.DispTouYmd                = DateTime.ParseExact(startDate, CommonConstants.FormatYMD, CultureInfo.InvariantCulture).AddDays(OriginDispTou).ToString(CommonConstants.FormatYMD); ;
                model.DispTouChTime             = unkobi.DispTouChTime;
                var OriginSyuPa                 = (DateTime.ParseExact(unkobi.SyuPaYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture) - DateTime.ParseExact(unkobi.HaiSymd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture)).TotalDays;
                model.SyuPaYmd                  = DateTime.ParseExact(startDate, CommonConstants.FormatYMD, CultureInfo.InvariantCulture).AddDays(OriginSyuPa).ToString(CommonConstants.FormatYMD);
                model.SyuPaTime                 = unkobi.SyuPaTime;
                var OriginDispSyuPa             = (DateTime.ParseExact(unkobi.DispSyuPaYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture) - DateTime.ParseExact(unkobi.HaiSymd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture)).TotalDays;
                model.DispSyuPaYmd              = DateTime.ParseExact(startDate, CommonConstants.FormatYMD, CultureInfo.InvariantCulture).AddDays(OriginDispSyuPa).ToString(CommonConstants.FormatYMD);
                model.DispSyuPaTime             = unkobi.DispSyuPaTime;
                model.UriYmd                    = startDate;
                model.KanJnm                    = unkobi.KanJnm;
                model.KanjJyus1                 = unkobi.KanjJyus1;
                model.KanjJyus2                 = unkobi.KanjJyus2;
                model.KanjTel                   = unkobi.KanjTel;
                model.KanjFax                   = unkobi.KanjFax;
                model.KanjKeiNo                 = unkobi.KanjKeiNo;
                model.KanjMail                  = unkobi.KanjMail;
                model.KanDmhflg                 = unkobi.KanDmhflg;
                model.DanTaNm                   = unkobi.DanTaNm;
                model.DanTaKana                 = unkobi.DanTaKana;
                model.IkMapCdSeq                = unkobi.IkMapCdSeq;
                model.IkNm                      = unkobi.IkNm;
                model.HaiScdSeq                 = unkobi.HaiScdSeq;
                model.HaiSnm                    = unkobi.HaiSnm;
                model.HaiSjyus1                 = unkobi.HaiSjyus1;
                model.HaiSjyus2                 = unkobi.HaiSjyus2;
                model.HaiSkouKcdSeq             = unkobi.HaiSkouKcdSeq;
                model.HaiSbinCdSeq              = unkobi.HaiSbinCdSeq;
                model.HaiSsetTime               = unkobi.HaiSsetTime;
                model.TouCdSeq                  = unkobi.TouCdSeq;
                model.TouNm                     = unkobi.TouNm;
                model.TouJyusyo1                = unkobi.TouJyusyo1;
                model.TouJyusyo2                = unkobi.TouJyusyo2;
                model.TouKouKcdSeq              = unkobi.TouKouKcdSeq;
                model.TouBinCdSeq               = unkobi.TouBinCdSeq;
                model.TouSetTime                = unkobi.TouSetTime;
                model.AreaMapSeq                = unkobi.AreaMapSeq;
                model.AreaNm                    = unkobi.AreaNm;
                model.HasMapCdSeq               = unkobi.HasMapCdSeq;
                model.HasNm                     = unkobi.HasNm;
                model.JyoKyakuCdSeq             = unkobi.JyoKyakuCdSeq;
                model.JyoSyaJin                 = unkobi.JyoSyaJin;
                model.PlusJin                   = unkobi.PlusJin;
                model.DrvJin                    = unkobi.DrvJin;
                model.GuiSu                     = unkobi.GuiSu;
                model.OthJinKbn1                = unkobi.OthJinKbn1;
                model.OthJin1                   = unkobi.OthJin1;
                model.OthJinKbn2                = unkobi.OthJinKbn2;
                model.OthJin2                   = unkobi.OthJin2;
                model.Kskbn                     = unkobi.Kskbn;
                model.KhinKbn                   = unkobi.KhinKbn;
                model.HaiSkbn                   = unkobi.HaiSkbn;
                model.HaiIkbn                   = unkobi.HaiIkbn;
                model.GuiWnin                   = unkobi.GuiWnin;
                model.NippoKbn                  = unkobi.NippoKbn;
                model.YouKbn                    = unkobi.YouKbn;
                model.UkeJyKbnCd                = unkobi.UkeJyKbnCd;
                model.SijJoKbn1                 = unkobi.SijJoKbn1;
                model.SijJoKbn2                 = unkobi.SijJoKbn2;
                model.SijJoKbn3                 = unkobi.SijJoKbn3;
                model.SijJoKbn4                 = unkobi.SijJoKbn4;
                model.SijJoKbn5                 = unkobi.SijJoKbn5;
                model.RotCdSeq                  = unkobi.RotCdSeq;
                model.ZenHaFlg                  = unkobi.ZenHaFlg;
                model.KhakFlg                   = unkobi.KhakFlg;
                model.UnkoJkbn                  = unkobi.UnkoJkbn;
                var OriginSyuKo                 = (DateTime.ParseExact(unkobi.HaiSymd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture) - DateTime.ParseExact(unkobi.SyukoYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture)).TotalDays;
                model.SyukoYmd                  = DateTime.ParseExact(startDate, CommonConstants.FormatYMD, CultureInfo.InvariantCulture).AddDays(OriginSyuKo).ToString(CommonConstants.FormatYMD);
                model.SyuKoTime                 = unkobi.SyuKoTime;
                var OrginKik                    = (DateTime.ParseExact(unkobi.KikYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture) - DateTime.ParseExact(unkobi.HaiSymd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture)).TotalDays;
                model.KikYmd                    = DateTime.ParseExact(startDate, CommonConstants.FormatYMD, CultureInfo.InvariantCulture).AddDays(OrginKik).ToString(CommonConstants.FormatYMD);
                model.KikTime                   = unkobi.KikTime;
                var OriginDispKik               = (DateTime.ParseExact(unkobi.DispKikYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture) - DateTime.ParseExact(unkobi.HaiSymd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture)).TotalDays;
                model.DispKikYmd                = DateTime.ParseExact(startDate, CommonConstants.FormatYMD, CultureInfo.InvariantCulture).AddDays(OriginDispKik).ToString(CommonConstants.FormatYMD);
                model.DispKikTime               = unkobi.DispKikTime;
                model.BikoTblSeq                = unkobi.BikoTblSeq;
                model.BikoNm                    = unkobi.BikoNm;
                model.HaiSkouKnm                = unkobi.HaiSkouKnm;
                model.HaiSbinNm                 = unkobi.HaiSbinNm;
                model.TouSkouKnm                = unkobi.TouSkouKnm;
                model.TouSbinNm                 = unkobi.TouSbinNm;
                model.UnsoOutputYmd             = unkobi.UnsoOutputYmd;
                model.UnsoAllKiro               = unkobi.UnsoAllKiro;
                model.UnsoJitKiro               = unkobi.UnsoJitKiro;
                model.UnsoAllTim                = unkobi.UnsoAllTim;
                model.UnsoJitTim                = unkobi.UnsoJitTim;
                model.UnsoShinSoTime            = unkobi.UnsoShinSoTime;
                model.UnsoChangeFlg             = unkobi.UnsoChangeFlg;
                model.UnsoSpecialFlg            = unkobi.UnsoSpecialFlg;
                model.UnsoSanSyuEigCdSeq        = unkobi.UnsoSanSyuEigCdSeq;
                model.UnsoUnkStaTime            = unkobi.UnsoUnkStaTime;
                model.UnsoUnkEndTime            = unkobi.UnsoUnkEndTime;
                model.SinSoRyokin               = unkobi.SinSoRyokin;
                model.ChangeRyokin              = unkobi.ChangeRyokin;
                model.SpecialRyokin             = unkobi.SpecialRyokin;
                model.WaribikiKbn               = unkobi.WaribikiKbn;
                model.UnsoNenKeiyakuFlg         = unkobi.UnsoNenKeiyakuFlg;
                model.UpperLimitFare            = unkobi.UpperLimitFare;
                model.LowerLimitFare            = unkobi.LowerLimitFare;
                model.UpperLimitFee             = unkobi.UpperLimitFee;
                model.LowerLimitFee             = unkobi.LowerLimitFee;
                model.OtherSystemCoperateCode   = unkobi.OtherSystemCoperateCode;
                model.CustomItems1              = unkobi.CustomItems1;
                model.CustomItems2              = unkobi.CustomItems2;
                model.CustomItems3              = unkobi.CustomItems3;
                model.CustomItems4              = unkobi.CustomItems4;
                model.CustomItems5              = unkobi.CustomItems5;
                model.CustomItems6              = unkobi.CustomItems6;
                model.CustomItems7              = unkobi.CustomItems7;
                model.CustomItems8              = unkobi.CustomItems8;
                model.CustomItems9              = unkobi.CustomItems9;
                model.CustomItems10             = unkobi.CustomItems10;
                model.CustomItems11             = unkobi.CustomItems11;
                model.CustomItems12             = unkobi.CustomItems12;
                model.CustomItems13             = unkobi.CustomItems13;
                model.CustomItems14             = unkobi.CustomItems14;
                model.CustomItems15             = unkobi.CustomItems15;
                model.CustomItems16             = unkobi.CustomItems16;
                model.CustomItems17             = unkobi.CustomItems17;
                model.CustomItems18             = unkobi.CustomItems18;
                model.CustomItems19             = unkobi.CustomItems19;
                model.CustomItems20             = unkobi.CustomItems20;
                model.SiyoKbn                   = unkobi.SiyoKbn;
                model.UpdYmd                    = UpdYmd;
                model.UpdTime                   = UpdTime;
                model.UpdSyainCd                = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                model.UpdPrgId                  = unkobi.UpdPrgId;
            }

            private void ExecuteYykSyu(string UkeNo, string ukeNo)
            {
                var listYykSyu = _context.TkdYykSyu.Where(_ => _.UkeNo == UkeNo).ToList();
                foreach(var yyksyu in listYykSyu)
                {
                    TkdYykSyu model = new TkdYykSyu();
                    MapYykSyu(model, yyksyu, ukeNo);
                    _context.TkdYykSyu.Add(model);
                }
            }

            private void MapYykSyu(TkdYykSyu model, TkdYykSyu yyksyu, string ukeNo)
            {
                model.UkeNo             = ukeNo;
                model.UnkRen            = yyksyu.UnkRen;
                model.SyaSyuRen         = yyksyu.SyaSyuRen;
                model.HenKai            = 0;
                model.SyaSyuCdSeq       = yyksyu.SyaSyuCdSeq;
                model.KataKbn           = yyksyu.KataKbn;
                model.SyaSyuDai         = yyksyu.SyaSyuDai;
                model.SyaSyuTan         = yyksyu.SyaSyuTan;
                model.SyaRyoUnc         = yyksyu.SyaRyoUnc;
                model.DriverNum         = yyksyu.DriverNum;
                model.UnitBusPrice      = yyksyu.UnitBusPrice;
                model.UnitBusFee        = yyksyu.UnitBusFee;
                model.GuiderNum         = yyksyu.GuiderNum;
                model.UnitGuiderPrice   = yyksyu.UnitGuiderPrice;
                model.UnitGuiderFee     = yyksyu.UnitGuiderFee;
                model.SiyoKbn           = yyksyu.SiyoKbn;
                model.UpdYmd            = UpdYmd;
                model.UpdTime           = UpdTime;
                model.UpdSyainCd        = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                model.UpdPrgId          = yyksyu.UpdPrgId;
            }

            private void ExecuteHaisha(string UkeNo, string ukeNo, string startDate)
            {
                var listHaisha = _context.TkdHaisha.Where(_ => _.UkeNo == UkeNo).ToList();
                string HaiSYmdOld = string.Empty;
                foreach(var haisha in listHaisha)
                {
                    TkdHaisha model = new TkdHaisha();
                    MapHaisha(model, haisha, ukeNo, startDate, HaiSYmdOld);
                    HaiSYmdOld = listHaisha.FirstOrDefault(_ => _.BunkRen == 1)?.HaiSymd ?? string.Empty;
                    _context.TkdHaisha.Add(model);
                }
            }

            private void MapHaisha(TkdHaisha model, TkdHaisha haisha, string ukeNo, string startDate, string HaiSYmdOld)
            {
                model.UkeNo                 = ukeNo;
                model.UnkRen                = haisha.UnkRen;
                model.SyaSyuRen             = haisha.SyaSyuRen;
                model.TeiDanNo              = haisha.TeiDanNo;
                model.BunkRen               = haisha.BunkRen;
                model.HenKai                = 0;
                model.GoSya                 = haisha.GoSya;
                model.GoSyaJyn              = haisha.GoSyaJyn;
                model.BunKsyuJyn            = haisha.BunKsyuJyn;
                model.SyuEigCdSeq           = haisha.SyuEigCdSeq;
                model.KikEigSeq             = haisha.KikEigSeq;
                model.HaiSsryCdSeq          = haisha.HaiSsryCdSeq;
                model.KssyaRseq             = haisha.KssyaRseq;
                model.DanTaNm2              = haisha.DanTaNm2;
                model.IkMapCdSeq            = haisha.IkMapCdSeq;
                model.IkNm                  = haisha.IkNm;
                if (string.IsNullOrEmpty(HaiSYmdOld))
                {
                    model.HaiSymd           = startDate;
                }
                else
                {
                    var Origin              = (DateTime.ParseExact(haisha.HaiSymd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture) - DateTime.ParseExact(HaiSYmdOld, CommonConstants.FormatYMD, CultureInfo.InvariantCulture)).TotalDays;
                    model.HaiSymd           = DateTime.ParseExact(startDate, CommonConstants.FormatYMD, CultureInfo.InvariantCulture).AddDays(Origin).ToString(CommonConstants.FormatYMD);
                }
                var OriginHai               = (DateTime.ParseExact(haisha.HaiSymd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture) - DateTime.ParseExact(haisha.SyuKoYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture)).TotalDays;
                model.SyuKoYmd              = DateTime.ParseExact(model.HaiSymd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture).AddDays(OriginHai).ToString(CommonConstants.FormatYMD);
                model.SyuKoTime             = haisha.SyuKoTime;
                model.SyuPaTime             = haisha.SyuPaTime;
                model.HaiStime              = haisha.HaiStime;
                model.HaiScdSeq             = haisha.HaiScdSeq;
                model.HaiSnm                = haisha.HaiSnm;
                model.HaiSjyus1             = haisha.HaiSjyus1;
                model.HaiSjyus2             = haisha.HaiSjyus2;
                model.HaiSkigou             = haisha.HaiSkigou;
                model.HaiSkouKcdSeq         = haisha.HaiSkouKcdSeq;
                model.HaiSkouKnm            = haisha.HaiSkouKnm;
                model.HaiSbinCdSeq          = haisha.HaiSbinCdSeq;
                model.HaiSbinNm             = haisha.HaiSbinNm;
                model.HaiSsetTime           = haisha.HaiSsetTime;
                var OriginKik               = (DateTime.ParseExact(haisha.KikYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture) - DateTime.ParseExact(haisha.HaiSymd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture)).TotalDays;
                model.KikYmd                = DateTime.ParseExact(model.HaiSymd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture).AddDays(OriginKik).ToString(CommonConstants.FormatYMD);
                model.KikTime               = haisha.KikTime;
                var OriginTou               = (DateTime.ParseExact(haisha.TouYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture) - DateTime.ParseExact(haisha.HaiSymd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture)).TotalDays;
                model.TouYmd                = DateTime.ParseExact(model.HaiSymd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture).AddDays(OriginTou).ToString(CommonConstants.FormatYMD);
                model.TouChTime             = haisha.TouChTime;
                model.TouCdSeq              = haisha.TouCdSeq;
                model.TouNm                 = haisha.TouNm;
                model.TouJyusyo1            = haisha.TouJyusyo1;
                model.TouJyusyo2            = haisha.TouJyusyo2;
                model.TouKigou              = haisha.TouKigou;
                model.TouKouKcdSeq          = haisha.TouKouKcdSeq;
                model.TouSkouKnm            = haisha.TouSkouKnm;
                model.TouBinCdSeq           = haisha.TouBinCdSeq;
                model.TouBinNm              = haisha.TouBinNm;
                model.TouSbinNm             = haisha.TouSbinNm;
                model.TouSetTime            = haisha.TouSetTime;
                model.JyoSyaJin             = haisha.JyoSyaJin;
                model.PlusJin               = haisha.PlusJin;
                model.DrvJin                = haisha.DrvJin;
                model.GuiSu                 = haisha.GuiSu;
                model.GuideSiteiEigyoCdSeq  = haisha.GuideSiteiEigyoCdSeq;
                model.SyukinTime            = haisha.SyukinTime;
                model.OthJinKbn1            = haisha.OthJinKbn1;
                model.OthJin1               = haisha.OthJin1;
                model.OthJinKbn2            = haisha.OthJinKbn2;
                model.OthJin2               = haisha.OthJin2;
                model.Kskbn                 = haisha.Kskbn;
                model.KhinKbn               = haisha.KhinKbn;
                model.HaiSkbn               = haisha.HaiSkbn;
                model.HaiIkbn               = haisha.HaiIkbn;
                model.GuiWnin               = haisha.GuiWnin;
                model.NippoKbn              = haisha.NippoKbn;
                model.YouTblSeq             = haisha.YouTblSeq;
                model.YouKataKbn            = haisha.YouKataKbn;
                model.SyaRyoUnc             = haisha.SyaRyoUnc;
                model.SyaRyoSyo             = haisha.SyaRyoSyo;
                model.SyaRyoTes             = haisha.SyaRyoTes;
                model.YoushaUnc             = haisha.YoushaUnc;
                model.YoushaSyo             = haisha.YoushaSyo;
                model.YoushaTes             = haisha.YoushaTes;
                model.PlatNo                = haisha.PlatNo;
                model.UkeJyKbnCd            = haisha.UkeJyKbnCd;
                model.SijJoKbn1             = haisha.SijJoKbn1;
                model.SijJoKbn2             = haisha.SijJoKbn2;
                model.SijJoKbn3             = haisha.SijJoKbn3;
                model.SijJoKbn4             = haisha.SijJoKbn4;
                model.SijJoKbn5             = haisha.SijJoKbn5;
                model.RotCdSeq              = haisha.RotCdSeq;
                model.BikoTblSeq            = haisha.BikoTblSeq;
                model.HaiCom                = haisha.HaiCom;
                model.BikoNm                = haisha.BikoNm;
                model.CustomItems1          = haisha.CustomItems1;
                model.CustomItems2          = haisha.CustomItems1;
                model.CustomItems3          = haisha.CustomItems1;
                model.CustomItems4          = haisha.CustomItems1;
                model.CustomItems5          = haisha.CustomItems1;
                model.CustomItems6          = haisha.CustomItems1;
                model.CustomItems7          = haisha.CustomItems1;
                model.CustomItems8          = haisha.CustomItems1;
                model.CustomItems9          = haisha.CustomItems1;
                model.CustomItems10         = haisha.CustomItems1;
                model.CustomItems11         = haisha.CustomItems1;
                model.CustomItems12         = haisha.CustomItems1;
                model.CustomItems13         = haisha.CustomItems1;
                model.CustomItems14         = haisha.CustomItems1;
                model.CustomItems15         = haisha.CustomItems1;
                model.CustomItems16         = haisha.CustomItems1;
                model.CustomItems17         = haisha.CustomItems1;
                model.CustomItems18         = haisha.CustomItems1;
                model.CustomItems19         = haisha.CustomItems1;
                model.CustomItems20         = haisha.CustomItems1;
                model.SiyoKbn               = haisha.SiyoKbn;
                model.UpdYmd                = UpdYmd;
                model.UpdTime               = UpdTime;
                model.UpdSyainCd            = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                model.UpdPrgId              = haisha.UpdPrgId;
            }

            private void ExecuteMishum(string UkeNo, string ukeNo)
            {
                var listMishum = _context.TkdMishum.Where(_ => _.UkeNo == UkeNo).ToList();
                foreach(var mishum in listMishum)
                {
                    TkdMishum model = new TkdMishum();
                    MapMishum(model, mishum, ukeNo);
                    _context.TkdMishum.Add(model);
                }
            }

            private void MapMishum(TkdMishum model, TkdMishum mishum, string ukeNo)
            {
                model.UkeNo      = ukeNo;
                model.MisyuRen   = mishum.MisyuRen;
                model.HenKai     = mishum.HenKai;
                model.SeiFutSyu  = mishum.SeiFutSyu;
                model.UriGakKin  = mishum.UriGakKin;
                model.SyaRyoSyo  = mishum.SyaRyoSyo;
                model.SyaRyoTes  = mishum.SyaRyoTes;
                model.SeiKin     = mishum.SeiKin;
                model.NyuKinRui  = mishum.NyuKinRui;
                model.CouKesRui  = mishum.CouKesRui;
                model.FutuUnkRen = mishum.FutuUnkRen;
                model.FutTumRen  = mishum.FutTumRen;
                model.SiyoKbn    = mishum.SiyoKbn;
                model.UpdYmd     = UpdYmd;
                model.UpdTime    = UpdTime;
                model.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                model.UpdPrgId   = mishum.UpdPrgId;
            }

            private void ExecuteProcess(string UkeNo, string ukeNo)
            {
                var listKotei = _context.TkdKotei.Where(_ => _.UkeNo == UkeNo).ToList();
                var listKoteiK = _context.TkdKoteik.Where(_ => _.UkeNo == UkeNo).ToList();

                foreach (var kotei in listKotei)
                {
                    TkdKotei model = new TkdKotei();
                    MapKotei(model, kotei, ukeNo);
                    _context.TkdKotei.Add(model);
                }
               
                foreach (var koteiK in listKoteiK)
                {
                    TkdKoteik model = new TkdKoteik();
                    MapKoteiK(model, koteiK, ukeNo);
                    _context.TkdKoteik.Add(model);
                }
            }

            private void MapKotei(TkdKotei model, TkdKotei kotei, string ukeNo)
            {
                model.UkeNo         = ukeNo;
                model.UnkRen        = kotei.UnkRen;
                model.TeiDanNo      = kotei.TeiDanNo;
                model.BunkRen       = kotei.BunkRen;
                model.TomKbn        = kotei.TomKbn;
                model.Nittei        = kotei.Nittei;
                model.KouRen        = kotei.KouRen;
                model.HenKai        = 0;
                model.TeiDanNittei  = kotei.TeiDanNittei;
                model.TeiDanTomKbn  = kotei.TeiDanTomKbn;
                model.Koutei        = kotei.Koutei;
                model.SiyoKbn       = kotei.SiyoKbn;
                model.UpdYmd        = UpdYmd;
                model.UpdTime       = UpdTime;
                model.UpdSyainCd    = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                model.UpdPrgId      = kotei.UpdPrgId;
            }

            private void MapKoteiK(TkdKoteik model, TkdKoteik koteiK, string ukeNo)
            {
                model.UkeNo         = ukeNo;
                model.UnkRen        = koteiK.UnkRen;
                model.TeiDanNo      = koteiK.TeiDanNo;
                model.BunkRen       = koteiK.BunkRen;
                model.TomKbn        = koteiK.TomKbn;
                model.Nittei        = koteiK.Nittei;
                model.HenKai        = koteiK.HenKai;
                model.TeiDanNittei  = koteiK.TeiDanNittei;
                model.TeiDanTomKbn  = koteiK.TeiDanTomKbn;
                model.SyuEigCdSeq   = koteiK.SyuEigCdSeq;
                model.SyukoNm       = koteiK.SyukoNm;
                model.HaiStime      = koteiK.HaiStime;
                model.SyuPaCdSeq    = koteiK.SyuPaCdSeq;
                model.SyuPaNm       = koteiK.SyuPaNm;
                model.SyuPaTime     = koteiK.SyuPaTime;
                model.KeiyuMapCdSeq = koteiK.KeiyuMapCdSeq;
                model.KeiyuNm       = koteiK.KeiyuNm;
                model.TouCdSeq      = koteiK.TouCdSeq;
                model.TouNm         = koteiK.TouNm;
                model.TouChTime     = koteiK.TouChTime;
                model.KikEigSeq     = koteiK.KikEigSeq;
                model.KikoNm        = koteiK.KikoNm;
                model.ShakuMapCdSeq = koteiK.ShakuMapCdSeq;
                model.ShakuNm       = koteiK.ShakuNm;
                model.TaikTime      = koteiK.TaikTime;
                model.KyuKmapCdSeq  = koteiK.KyuKmapCdSeq;
                model.KyuKnm        = koteiK.KyuKnm;
                model.KyuKtime      = koteiK.KyuKtime;
                model.KyuKstaTime   = koteiK.KyuKstaTime;
                model.KyuKendTime   = koteiK.KyuKendTime;
                model.BikoNm        = koteiK.BikoNm;
                model.JisaIpkm      = koteiK.JisaIpkm;
                model.JisaKskm      = koteiK.JisaKskm;
                model.KisoIpkm      = koteiK.KisoIpkm;
                model.KisoKokm      = koteiK.KisoKokm;
                model.SiyoKbn       = koteiK.SiyoKbn;
                model.UpdYmd        = UpdYmd;
                model.UpdTime       = UpdTime;
                model.UpdSyainCd    = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                model.UpdPrgId      = koteiK.UpdPrgId;
            }

            private void ExecuteArrangement(string UkeNo, string ukeNo)
            {
                var listTehai = _context.TkdTehai.Where(_ => _.UkeNo == UkeNo).ToList();
                foreach (var tehai in listTehai)
                {
                    TkdTehai model = new TkdTehai();
                    MapTeihai(model, tehai, ukeNo);
                    _context.TkdTehai.Add(model);
                }
            }

            private void MapTeihai(TkdTehai model, TkdTehai tehai, string ukeNo)
            {
                model.UkeNo         = ukeNo;
                model.UnkRen        = tehai.UnkRen;
                model.TeiDanNo      = tehai.TeiDanNo;
                model.BunkRen       = tehai.BunkRen;
                model.TehRen        = tehai.TehRen;
                model.HenKai        = 0;
                model.Nittei        = tehai.Nittei;
                model.TomKbn        = tehai.TomKbn;
                model.TeiDanNittei  = tehai.TeiDanNittei;
                model.TeiDanTomKbn  = tehai.TeiDanTomKbn;
                model.TehMapCdSeq   = tehai.TehMapCdSeq;
                model.TehNm         = tehai.TehNm;
                model.TehJyus1      = tehai.TehJyus1;
                model.TehJyus2      = tehai.TehJyus2;
                model.TehTel        = tehai.TehTel;
                model.TehFax        = tehai.TehFax;
                model.TehTan        = tehai.TehTan;
                model.TehaiCdSeq    = tehai.TehaiCdSeq;
                model.TouChTime     = tehai.TouChTime;
                model.SyuPaTime     = tehai.SyuPaTime;
                model.BikoNm        = tehai.BikoNm;
                model.SiyoKbn       = tehai.SiyoKbn;
                model.UpdYmd        = UpdYmd;
                model.UpdTime       = UpdTime;
                model.UpdSyainCd    = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                model.UpdPrgId      = tehai.UpdPrgId;
            }

            private void ExecuteLoadedGoods(string UkeNo, string ukeNo, string startDate)
            {
                var listFutTum = _context.TkdFutTum.Where(_ => _.UkeNo == UkeNo && _.FutTumKbn == 2).ToList();
                foreach (var futTum in listFutTum)
                {
                    TkdFutTum model = new TkdFutTum();
                    MapFutTum(model, futTum, startDate, ukeNo);
                    _context.TkdFutTum.Add(model);
                }
            }

            private void ExecuteIncidental(string UkeNo, string ukeNo, string startDate)
            {
                var listFutTum = _context.TkdFutTum.Where(_ => _.UkeNo == UkeNo && _.FutTumKbn == 1).ToList();
                foreach (var futTum in listFutTum)
                {
                    TkdFutTum model = new TkdFutTum();
                    MapFutTum(model, futTum, startDate, ukeNo);
                    _context.TkdFutTum.Add(model);
                }
            }

            private void MapFutTum(TkdFutTum model, TkdFutTum futTum, string startDate, string ukeNo)
            {
                model.UkeNo         = ukeNo;
                model.UnkRen        = futTum.UnkRen;
                model.FutTumKbn     = futTum.FutTumKbn;
                model.FutTumRen     = futTum.FutTumRen;
                model.HenKai        = futTum.HenKai;
                model.Nittei        = futTum.Nittei;
                model.TomKbn        = futTum.TomKbn;
                model.FutTumCdSeq   = futTum.FutTumCdSeq;
                model.FutTumNm      = futTum.FutTumNm;
                model.HasYmd        = startDate;
                model.IriRyoChiCd   = futTum.IriRyoChiCd;
                model.IriRyoCd      = futTum.IriRyoCd;
                model.IriRyoNm      = futTum.IriRyoNm;
                model.DeRyoChiCd    = futTum.DeRyoChiCd;
                model.DeRyoCd       = futTum.DeRyoCd;
                model.DeRyoNm       = futTum.DeRyoNm;
                model.SeisanCdSeq   = futTum.SeisanCdSeq;
                model.SeisanNm      = futTum.SeisanNm;
                model.SeisanKbn     = futTum.SeisanKbn;
                model.TanKa         = futTum.TanKa;
                model.Suryo         = futTum.Suryo;
                model.UriGakKin     = futTum.UriGakKin;
                model.ZeiKbn        = futTum.ZeiKbn;
                model.Zeiritsu      = futTum.Zeiritsu;
                model.SyaRyoSyo     = futTum.SyaRyoSyo;
                model.TesuRitu      = futTum.TesuRitu;
                model.SyaRyoTes     = futTum.SyaRyoTes;
                model.NyuKinKbn     = futTum.NyuKinKbn;
                model.NcouKbn       = futTum.NcouKbn;
                model.BikoNm        = futTum.BikoNm;
                model.ExpItem       = futTum.ExpItem;
                model.SortJun       = futTum.SortJun;
                model.SirSitenCdSeq = futTum.SirSitenCdSeq;
                model.SirTanKa      = futTum.SirTanKa;
                model.SirSuryo      = futTum.SirSuryo;
                model.SirGakKin     = futTum.SirGakKin;
                model.SirZeiKbn     = futTum.SirZeiKbn;
                model.SirZeiritsu   = futTum.SirZeiritsu;
                model.SirSyaRyoSyo  = futTum.SirSyaRyoSyo;
                model.SiyoKbn       = futTum.SiyoKbn;
                model.UpdYmd        = UpdYmd;
                model.UpdTime       = UpdTime;
                model.UpdSyainCd    = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                model.UpdPrgId      = futTum.UpdPrgId;
                model.SireCdSeq     = futTum.SireCdSeq;
            }
        }
    }
}
