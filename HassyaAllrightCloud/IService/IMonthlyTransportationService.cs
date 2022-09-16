using DevExpress.Data.ODataLinq.Helpers;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.TransportationSummary.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.Reports.DataSource;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IMonthlyTransportationService : IReportService
    {
        Task<CommonListitems> GetCommonListItems();
        Task<List<JitHouReports>> GetJitHouDataReport(SearchParam jitHouParam);
        Task<List<JitHouReports>> GetJitHousAnnual(SearchParam jitHouParam);
        Task<IEnumerable<TKDJitHouResultInput>> GetJitHouDataResultInput(SearchParam searchParam);
        Task SaveJitHou(SearchParam searchParam, MonthlyTransportationModel model);
    }
    public class MonthlyTransportationService : IMonthlyTransportationService
    {
        private readonly IMediator _mediator;
        private readonly KobodbContext _context;

        public MonthlyTransportationService(IMediator mediator, KobodbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task<CommonListitems> GetCommonListItems()
        {
            return await _mediator.Send(new GetCommonListItems());
        }

        public async Task<List<JitHouReports>> GetJitHouDataReport(SearchParam searchParam)
        {
            return await _mediator.Send(new GetJitHous { SearchParam = searchParam });
        }
        public async Task<List<JitHouReports>> GetJitHousAnnual(SearchParam searchParam)
        {
            return await _mediator.Send(new GetJitHousAnnual { SearchParam = searchParam });
        }

        public async Task<IEnumerable<TKDJitHouResultInput>> GetJitHouDataResultInput(SearchParam searchParam)
        {
            return await _mediator.Send(new GetJitHouDataResultInput { SearchParam = searchParam });
        }

        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            var searchParams = EncryptHelper.DecryptFromUrl<SearchParam>(queryParams);

            XtraReport reportJitHou = new Reports.JitHouReport();
            ObjectDataSource dataSourceJitHou = new ObjectDataSource();
            var dataJitHou = await GetJitHouDataReport(searchParams);
            Parameter param = new Parameter()
            {
                Name = "data",
                Type = typeof(List<JitHouReports>),
                Value = dataJitHou
            };
            dataSourceJitHou.Name = "objectDataSource1";
            dataSourceJitHou.DataSource = typeof(JitHouReportReportDS);
            dataSourceJitHou.Constructor = new ObjectConstructorInfo(param);
            dataSourceJitHou.DataMember = "_data";
            reportJitHou.DataSource = dataSourceJitHou;
            return reportJitHou;
        }

        public async Task SaveJitHou(SearchParam searchParam, MonthlyTransportationModel model)
        {
            using (IDbContextTransaction dbTran = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var curentUser = new ClaimModel();

                    var jitHouRemoves = (from j in _context.TkdJitHou
                                         join e in _context.VpmEigyos on j.EigyoCdSeq equals e.EigyoCdSeq into eigyosTmp
                                         from e in eigyosTmp.DefaultIfEmpty()
                                         join c in _context.VpmCompny on e.CompanyCdSeq equals c.CompanyCdSeq
                                         where e.SiyoKbn == 1 && c.SiyoKbn == 1
                                            && j.SyoriYm == searchParam.StrDate
                                            && j.EigyoCdSeq == searchParam.StrEigyoCdSeq
                                            && j.UnsouKbn == searchParam.StrUnsouKbn
                                            && c.TenantCdSeq == curentUser.TenantID
                                         select j).ToList();

                    _context.TkdJitHou.RemoveRange(jitHouRemoves);

                    ////Big
                    //軽油
                    var tkdJitHouBigLightOil = new TkdJitHou
                    {
                        UnsouKbn = (byte)searchParam.StrUnsouKbn,
                        SyoriYm = DateTime.ParseExact(searchParam?.StrDate, Formats.yyyyMM, null).ToString(Formats.yyyyMM),
                        EigyoCdSeq = searchParam.StrEigyoCdSeq,
                        KataKbn = (int)KataKbn.Big,
                        NenRyoKbn = (int)NenRyoKbn.LightOil,
                        NobeJyoCnt = model.LargeOil_NobeJyoCnt ?? 0,
                        NobeRinCnt = model.LargeOil_NobeRinCnt ?? 0,
                        NobeSumCnt = model.LargeOil_NobeSumCnt ?? 0,
                        NobeJitCnt = model.LargeOil_NobeJitCnt ?? 0,
                        JitudoRitu = model?.LargeOil_JitudoRitu ?? 0,
                        RinjiRitu = model.LargeOil_RinjiRitu ?? 0,
                        JitJisaKm = model.LargeOil_JitJisaKm ?? 0,
                        JitKisoKm = model.LargeOil_JitKisoKm ?? 0,
                        YusoJin = model.LargeOil_YusoJin ?? 0,
                        UnkoCnt = model.LargeOil_UnkoCnt ?? 0,
                        UnkoKikak1Cnt = model.LargeOil_UnkoKikak1Cnt ?? 0,
                        UnkoKikak2Cnt = model.LargeOil_UnkoKikak2Cnt ?? 0,
                        UnkoOthCnt = model.LargeOil_UnkoOthCnt ?? 0,
                        UnsoSyu = model.LargeOil_UnsoSyu ?? 0,
                        DayTotalKm = model.LargeOil_DayTotalKm ?? 0,
                        DayYusoJin = model.LargeOil_DayYusoJin ?? 0,
                        DayUnsoSyu = model.LargeOil_DayUnsoSyu ?? 0,
                        DayJisaKm = model.LargeOil_DayJisaKm ?? 0,
                        UnsoZaSyu = model?.LargeOil_UnsoZaSyu ?? 0,
                        UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd),
                        UpdTime = DateTime.Now.ToString(Formats.HHmmss),
                        UpdSyainCd = curentUser.SyainCdSeq,
                        UpdPrgId = Common.UpdPrgId
                    };
                    _context.TkdJitHou.Add(tkdJitHouBigLightOil);

                    //ガソリン
                    var tkdJitHouLargeGasoline = new TkdJitHou
                    {
                        UnsouKbn = (byte)searchParam.StrUnsouKbn,
                        SyoriYm = DateTime.ParseExact(searchParam?.StrDate, Formats.yyyyMM, null).ToString(Formats.yyyyMM),
                        EigyoCdSeq = searchParam.StrEigyoCdSeq,
                        KataKbn = (int)KataKbn.Big,
                        NenRyoKbn = (int)NenRyoKbn.Gasoline,
                        NobeJyoCnt = model.LargeGasoline_NobeJyoCnt ?? 0,
                        NobeRinCnt = model.LargeGasoline_NobeRinCnt ?? 0,
                        NobeSumCnt = model.LargeGasoline_NobeSumCnt ?? 0,
                        NobeJitCnt = model.LargeGasoline_NobeJitCnt ?? 0,
                        JitudoRitu = model?.LargeGasoline_JitudoRitu ?? 0,
                        RinjiRitu = model.LargeGasoline_RinjiRitu ?? 0,
                        JitJisaKm = model.LargeGasoline_JitJisaKm ?? 0,
                        JitKisoKm = model.LargeGasoline_JitKisoKm ?? 0,
                        YusoJin = model.LargeGasoline_YusoJin ?? 0,
                        UnkoCnt = model.LargeGasoline_UnkoCnt ?? 0,
                        UnkoKikak1Cnt = model.LargeGasoline_UnkoKikak1Cnt ?? 0,
                        UnkoKikak2Cnt = model.LargeGasoline_UnkoKikak2Cnt ?? 0,
                        UnkoOthCnt = model.LargeGasoline_UnkoOthCnt ?? 0,
                        UnsoSyu = model.LargeGasoline_UnsoSyu ?? 0,
                        DayTotalKm = model.LargeGasoline_DayTotalKm ?? 0,
                        DayYusoJin = model.LargeGasoline_DayYusoJin ?? 0,
                        DayUnsoSyu = model.LargeGasoline_DayUnsoSyu ?? 0,
                        DayJisaKm = model.LargeGasoline_DayJisaKm ?? 0,
                        UnsoZaSyu = model?.LargeGasoline_UnsoZaSyu ?? 0,
                        UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd),
                        UpdTime = DateTime.Now.ToString(Formats.HHmmss),
                        UpdSyainCd = curentUser.SyainCdSeq,
                        UpdPrgId = Common.UpdPrgId
                    };
                    _context.TkdJitHou.Add(tkdJitHouLargeGasoline);

                    //LPG
                    var tkdJitHouLargeLPG = new TkdJitHou
                    {
                        UnsouKbn = (byte)searchParam.StrUnsouKbn,
                        SyoriYm = DateTime.ParseExact(searchParam?.StrDate, Formats.yyyyMM, null).ToString(Formats.yyyyMM),
                        EigyoCdSeq = searchParam.StrEigyoCdSeq,
                        KataKbn = (int)KataKbn.Big,
                        NenRyoKbn = (int)NenRyoKbn.LPG,
                        NobeJyoCnt = model.LargeLPG_NobeJyoCnt ?? 0,
                        NobeRinCnt = model.LargeLPG_NobeRinCnt ?? 0,
                        NobeSumCnt = model.LargeLPG_NobeSumCnt ?? 0,
                        NobeJitCnt = model.LargeLPG_NobeJitCnt ?? 0,
                        JitudoRitu = model?.LargeLPG_JitudoRitu ?? 0,
                        RinjiRitu = model.LargeLPG_RinjiRitu ?? 0,
                        JitJisaKm = model.LargeLPG_JitJisaKm ?? 0,
                        JitKisoKm = model.LargeLPG_JitKisoKm ?? 0,
                        YusoJin = model.LargeLPG_YusoJin ?? 0,
                        UnkoCnt = model.LargeLPG_UnkoCnt ?? 0,
                        UnkoKikak1Cnt = model.LargeLPG_UnkoKikak1Cnt ?? 0,
                        UnkoKikak2Cnt = model.LargeLPG_UnkoKikak2Cnt ?? 0,
                        UnkoOthCnt = model.LargeLPG_UnkoOthCnt ?? 0,
                        UnsoSyu = model.LargeLPG_UnsoSyu ?? 0,
                        DayTotalKm = model.LargeLPG_DayTotalKm ?? 0,
                        DayYusoJin = model.LargeLPG_DayYusoJin ?? 0,
                        DayUnsoSyu = model.LargeLPG_DayUnsoSyu ?? 0,
                        DayJisaKm = model.LargeLPG_DayJisaKm ?? 0,
                        UnsoZaSyu = model?.LargeLPG_UnsoZaSyu ?? 0,
                        UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd),
                        UpdTime = DateTime.Now.ToString(Formats.HHmmss),
                        UpdSyainCd = curentUser.SyainCdSeq,
                        UpdPrgId = Common.UpdPrgId
                    };
                    _context.TkdJitHou.Add(tkdJitHouLargeLPG);

                    //ガスタービン
                    var tkdJitHouLargeGasTurbine = new TkdJitHou
                    {
                        UnsouKbn = (byte)searchParam.StrUnsouKbn,
                        SyoriYm = DateTime.ParseExact(searchParam?.StrDate, Formats.yyyyMM, null).ToString(Formats.yyyyMM),
                        EigyoCdSeq = searchParam.StrEigyoCdSeq,
                        KataKbn = (int)KataKbn.Big,
                        NenRyoKbn = (int)NenRyoKbn.GasTurbine,
                        NobeJyoCnt = model.LargeGasTurbine_NobeJyoCnt ?? 0,
                        NobeRinCnt = model.LargeGasTurbine_NobeRinCnt ?? 0,
                        NobeSumCnt = model.LargeGasTurbine_NobeSumCnt ?? 0,
                        NobeJitCnt = model.LargeGasTurbine_NobeJitCnt ?? 0,
                        JitudoRitu = model?.LargeGasTurbine_JitudoRitu ?? 0,
                        RinjiRitu = model.LargeGasTurbine_RinjiRitu ?? 0,
                        JitJisaKm = model.LargeGasTurbine_JitJisaKm ?? 0,
                        JitKisoKm = model.LargeGasTurbine_JitKisoKm ?? 0,
                        YusoJin = model.LargeGasTurbine_YusoJin ?? 0,
                        UnkoCnt = model.LargeGasTurbine_UnkoCnt ?? 0,
                        UnkoKikak1Cnt = model.LargeGasTurbine_UnkoKikak1Cnt ?? 0,
                        UnkoKikak2Cnt = model.LargeGasTurbine_UnkoKikak2Cnt ?? 0,
                        UnkoOthCnt = model.LargeGasTurbine_UnkoOthCnt ?? 0,
                        UnsoSyu = model.LargeGasTurbine_UnsoSyu ?? 0,
                        DayTotalKm = model.LargeGasTurbine_DayTotalKm ?? 0,
                        DayYusoJin = model.LargeGasTurbine_DayYusoJin ?? 0,
                        DayUnsoSyu = model.LargeGasTurbine_DayUnsoSyu ?? 0,
                        DayJisaKm = model.LargeGasTurbine_DayJisaKm ?? 0,
                        UnsoZaSyu = model?.LargeGasTurbine_UnsoZaSyu ?? 0,
                        UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd),
                        UpdTime = DateTime.Now.ToString(Formats.HHmmss),
                        UpdSyainCd = curentUser.SyainCdSeq,
                        UpdPrgId = Common.UpdPrgId
                    };
                    _context.TkdJitHou.Add(tkdJitHouLargeGasTurbine);

                    //その他
                    var tkdJitHouLargeOther = new TkdJitHou
                    {
                        UnsouKbn = (byte)searchParam.StrUnsouKbn,
                        SyoriYm = DateTime.ParseExact(searchParam?.StrDate, Formats.yyyyMM, null).ToString(Formats.yyyyMM),
                        EigyoCdSeq = searchParam.StrEigyoCdSeq,
                        KataKbn = (int)KataKbn.Big,
                        NenRyoKbn = (int)NenRyoKbn.Other,
                        NobeJyoCnt = model.LargeOther_NobeJyoCnt ?? 0,
                        NobeRinCnt = model.LargeOther_NobeRinCnt ?? 0,
                        NobeSumCnt = model.LargeOther_NobeSumCnt ?? 0,
                        NobeJitCnt = model.LargeOther_NobeJitCnt ?? 0,
                        JitudoRitu = model?.LargeOther_JitudoRitu ?? 0,
                        RinjiRitu = model.LargeOther_RinjiRitu ?? 0,
                        JitJisaKm = model.LargeOther_JitJisaKm ?? 0,
                        JitKisoKm = model.LargeOther_JitKisoKm ?? 0,
                        YusoJin = model.LargeOther_YusoJin ?? 0,
                        UnkoCnt = model.LargeOther_UnkoCnt ?? 0,
                        UnkoKikak1Cnt = model.LargeOther_UnkoKikak1Cnt ?? 0,
                        UnkoKikak2Cnt = model.LargeOther_UnkoKikak2Cnt ?? 0,
                        UnkoOthCnt = model.LargeOther_UnkoOthCnt ?? 0,
                        UnsoSyu = model.LargeOther_UnsoSyu ?? 0,
                        DayTotalKm = model.LargeOther_DayTotalKm ?? 0,
                        DayYusoJin = model.LargeOther_DayYusoJin ?? 0,
                        DayUnsoSyu = model.LargeOther_DayUnsoSyu ?? 0,
                        DayJisaKm = model.LargeOther_DayJisaKm ?? 0,
                        UnsoZaSyu = model?.LargeOther_UnsoZaSyu ?? 0,
                        UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd),
                        UpdTime = DateTime.Now.ToString(Formats.HHmmss),
                        UpdSyainCd = curentUser.SyainCdSeq,
                        UpdPrgId = Common.UpdPrgId
                    };
                    _context.TkdJitHou.Add(tkdJitHouLargeOther);

                    ////Medium
                    //軽油
                    var tkdJitHouMediumLightOil = new TkdJitHou
                    {
                        UnsouKbn = (byte)searchParam.StrUnsouKbn,
                        SyoriYm = DateTime.ParseExact(searchParam?.StrDate, Formats.yyyyMM, null).ToString(Formats.yyyyMM),
                        EigyoCdSeq = searchParam.StrEigyoCdSeq,
                        KataKbn = (int)KataKbn.Medium,
                        NenRyoKbn = (int)NenRyoKbn.LightOil,
                        NobeJyoCnt = model.MediumOil_NobeJyoCnt ?? 0,
                        NobeRinCnt = model.MediumOil_NobeRinCnt ?? 0,
                        NobeSumCnt = model.MediumOil_NobeSumCnt ?? 0,
                        NobeJitCnt = model.MediumOil_NobeJitCnt ?? 0,
                        JitudoRitu = model?.MediumOil_JitudoRitu ?? 0,
                        RinjiRitu = model.MediumOil_RinjiRitu ?? 0,
                        JitJisaKm = model.MediumOil_JitJisaKm ?? 0,
                        JitKisoKm = model.MediumOil_JitKisoKm ?? 0,
                        YusoJin = model.MediumOil_YusoJin ?? 0,
                        UnkoCnt = model.MediumOil_UnkoCnt ?? 0,
                        UnkoKikak1Cnt = model.MediumOil_UnkoKikak1Cnt ?? 0,
                        UnkoKikak2Cnt = model.MediumOil_UnkoKikak2Cnt ?? 0,
                        UnkoOthCnt = model.MediumOil_UnkoOthCnt ?? 0,
                        UnsoSyu = model.MediumOil_UnsoSyu ?? 0,
                        DayTotalKm = model.MediumOil_DayTotalKm ?? 0,
                        DayYusoJin = model.MediumOil_DayYusoJin ?? 0,
                        DayUnsoSyu = model.MediumOil_DayUnsoSyu ?? 0,
                        DayJisaKm = model.MediumOil_DayJisaKm ?? 0,
                        UnsoZaSyu = model?.MediumOil_UnsoZaSyu ?? 0,
                        UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd),
                        UpdTime = DateTime.Now.ToString(Formats.HHmmss),
                        UpdSyainCd = curentUser.SyainCdSeq,
                        UpdPrgId = Common.UpdPrgId
                    };
                    _context.TkdJitHou.Add(tkdJitHouMediumLightOil);

                    //ガソリン
                    var tkdJitHouMediumGasoline = new TkdJitHou
                    {
                        UnsouKbn = (byte)searchParam.StrUnsouKbn,
                        SyoriYm = DateTime.ParseExact(searchParam?.StrDate, Formats.yyyyMM, null).ToString(Formats.yyyyMM),
                        EigyoCdSeq = searchParam.StrEigyoCdSeq,
                        KataKbn = (int)KataKbn.Medium,
                        NenRyoKbn = (int)NenRyoKbn.Gasoline,
                        NobeJyoCnt = model.MediumGasoline_NobeJyoCnt ?? 0,
                        NobeRinCnt = model.MediumGasoline_NobeRinCnt ?? 0,
                        NobeSumCnt = model.MediumGasoline_NobeSumCnt ?? 0,
                        NobeJitCnt = model.MediumGasoline_NobeJitCnt ?? 0,
                        JitudoRitu = model?.MediumGasoline_JitudoRitu ?? 0,
                        RinjiRitu = model.MediumGasoline_RinjiRitu ?? 0,
                        JitJisaKm = model.MediumGasoline_JitJisaKm ?? 0,
                        JitKisoKm = model.MediumGasoline_JitKisoKm ?? 0,
                        YusoJin = model.MediumGasoline_YusoJin ?? 0,
                        UnkoCnt = model.MediumGasoline_UnkoCnt ?? 0,
                        UnkoKikak1Cnt = model.MediumGasoline_UnkoKikak1Cnt ?? 0,
                        UnkoKikak2Cnt = model.MediumGasoline_UnkoKikak2Cnt ?? 0,
                        UnkoOthCnt = model.MediumGasoline_UnkoOthCnt ?? 0,
                        UnsoSyu = model.MediumGasoline_UnsoSyu ?? 0,
                        DayTotalKm = model.MediumGasoline_DayTotalKm ?? 0,
                        DayYusoJin = model.MediumGasoline_DayYusoJin ?? 0,
                        DayUnsoSyu = model.MediumGasoline_DayUnsoSyu ?? 0,
                        DayJisaKm = model.MediumGasoline_DayJisaKm ?? 0,
                        UnsoZaSyu = model?.MediumGasoline_UnsoZaSyu ?? 0,
                        UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd),
                        UpdTime = DateTime.Now.ToString(Formats.HHmmss),
                        UpdSyainCd = curentUser.SyainCdSeq,
                        UpdPrgId = Common.UpdPrgId
                    };
                    _context.TkdJitHou.Add(tkdJitHouMediumGasoline);

                    //LPG
                    var tkdJitHouMediumLPG = new TkdJitHou
                    {
                        UnsouKbn = (byte)searchParam.StrUnsouKbn,
                        SyoriYm = DateTime.ParseExact(searchParam?.StrDate, Formats.yyyyMM, null).ToString(Formats.yyyyMM),
                        EigyoCdSeq = searchParam.StrEigyoCdSeq,
                        KataKbn = (int)KataKbn.Medium,
                        NenRyoKbn = (int)NenRyoKbn.LPG,
                        NobeJyoCnt = model.MediumLPG_NobeJyoCnt ?? 0,
                        NobeRinCnt = model.MediumLPG_NobeRinCnt ?? 0,
                        NobeSumCnt = model.MediumLPG_NobeSumCnt ?? 0,
                        NobeJitCnt = model.MediumLPG_NobeJitCnt ?? 0,
                        JitudoRitu = model?.MediumLPG_JitudoRitu ?? 0,
                        RinjiRitu = model.MediumLPG_RinjiRitu ?? 0,
                        JitJisaKm = model.MediumLPG_JitJisaKm ?? 0,
                        JitKisoKm = model.MediumLPG_JitKisoKm ?? 0,
                        YusoJin = model.MediumLPG_YusoJin ?? 0,
                        UnkoCnt = model.MediumLPG_UnkoCnt ?? 0,
                        UnkoKikak1Cnt = model.MediumLPG_UnkoKikak1Cnt ?? 0,
                        UnkoKikak2Cnt = model.MediumLPG_UnkoKikak2Cnt ?? 0,
                        UnkoOthCnt = model.MediumLPG_UnkoOthCnt ?? 0,
                        UnsoSyu = model.MediumLPG_UnsoSyu ?? 0,
                        DayTotalKm = model.MediumLPG_DayTotalKm ?? 0,
                        DayYusoJin = model.MediumLPG_DayYusoJin ?? 0,
                        DayUnsoSyu = model.MediumLPG_DayUnsoSyu ?? 0,
                        DayJisaKm = model.MediumLPG_DayJisaKm ?? 0,
                        UnsoZaSyu = model?.MediumLPG_UnsoZaSyu ?? 0,
                        UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd),
                        UpdTime = DateTime.Now.ToString(Formats.HHmmss),
                        UpdSyainCd = curentUser.SyainCdSeq,
                        UpdPrgId = Common.UpdPrgId
                    };
                    _context.TkdJitHou.Add(tkdJitHouMediumLPG);

                    //ガスタービン
                    var tkdJitHouMediumGasTurbine = new TkdJitHou
                    {
                        UnsouKbn = (byte)searchParam.StrUnsouKbn,
                        SyoriYm = DateTime.ParseExact(searchParam?.StrDate, Formats.yyyyMM, null).ToString(Formats.yyyyMM),
                        EigyoCdSeq = searchParam.StrEigyoCdSeq,
                        KataKbn = (int)KataKbn.Medium,
                        NenRyoKbn = (int)NenRyoKbn.GasTurbine,
                        NobeJyoCnt = model.MediumGasTurbine_NobeJyoCnt ?? 0,
                        NobeRinCnt = model.MediumGasTurbine_NobeRinCnt ?? 0,
                        NobeSumCnt = model.MediumGasTurbine_NobeSumCnt ?? 0,
                        NobeJitCnt = model.MediumGasTurbine_NobeJitCnt ?? 0,
                        JitudoRitu = model?.MediumGasTurbine_JitudoRitu ?? 0,
                        RinjiRitu = model.MediumGasTurbine_RinjiRitu ?? 0,
                        JitJisaKm = model.MediumGasTurbine_JitJisaKm ?? 0,
                        JitKisoKm = model.MediumGasTurbine_JitKisoKm ?? 0,
                        YusoJin = model.MediumGasTurbine_YusoJin ?? 0,
                        UnkoCnt = model.MediumGasTurbine_UnkoCnt ?? 0,
                        UnkoKikak1Cnt = model.MediumGasTurbine_UnkoKikak1Cnt ?? 0,
                        UnkoKikak2Cnt = model.MediumGasTurbine_UnkoKikak2Cnt ?? 0,
                        UnkoOthCnt = model.MediumGasTurbine_UnkoOthCnt ?? 0,
                        UnsoSyu = model.MediumGasTurbine_UnsoSyu ?? 0,
                        DayTotalKm = model.MediumGasTurbine_DayTotalKm ?? 0,
                        DayYusoJin = model.MediumGasTurbine_DayYusoJin ?? 0,
                        DayUnsoSyu = model.MediumGasTurbine_DayUnsoSyu ?? 0,
                        DayJisaKm = model.MediumGasTurbine_DayJisaKm ?? 0,
                        UnsoZaSyu = model?.MediumGasTurbine_UnsoZaSyu ?? 0,
                        UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd),
                        UpdTime = DateTime.Now.ToString(Formats.HHmmss),
                        UpdSyainCd = curentUser.SyainCdSeq,
                        UpdPrgId = Common.UpdPrgId
                    };
                    _context.TkdJitHou.Add(tkdJitHouMediumGasTurbine);

                    //その他
                    var tkdJitHouMediumOther = new TkdJitHou
                    {
                        UnsouKbn = (byte)searchParam.StrUnsouKbn,
                        SyoriYm = DateTime.ParseExact(searchParam?.StrDate, Formats.yyyyMM, null).ToString(Formats.yyyyMM),
                        EigyoCdSeq = searchParam.StrEigyoCdSeq,
                        KataKbn = (int)KataKbn.Medium,
                        NenRyoKbn = (int)NenRyoKbn.Other,
                        NobeJyoCnt = model.MediumOther_NobeJyoCnt ?? 0,
                        NobeRinCnt = model.MediumOther_NobeRinCnt ?? 0,
                        NobeSumCnt = model.MediumOther_NobeSumCnt ?? 0,
                        NobeJitCnt = model.MediumOther_NobeJitCnt ?? 0,
                        JitudoRitu = model?.MediumOther_JitudoRitu ?? 0,
                        RinjiRitu = model.MediumOther_RinjiRitu ?? 0,
                        JitJisaKm = model.MediumOther_JitJisaKm ?? 0,
                        JitKisoKm = model.MediumOther_JitKisoKm ?? 0,
                        YusoJin = model.MediumOther_YusoJin ?? 0,
                        UnkoCnt = model.MediumOther_UnkoCnt ?? 0,
                        UnkoKikak1Cnt = model.MediumOther_UnkoKikak1Cnt ?? 0,
                        UnkoKikak2Cnt = model.MediumOther_UnkoKikak2Cnt ?? 0,
                        UnkoOthCnt = model.MediumOther_UnkoOthCnt ?? 0,
                        UnsoSyu = model.MediumOther_UnsoSyu ?? 0,
                        DayTotalKm = model.MediumOther_DayTotalKm ?? 0,
                        DayYusoJin = model.MediumOther_DayYusoJin ?? 0,
                        DayUnsoSyu = model.MediumOther_DayUnsoSyu ?? 0,
                        DayJisaKm = model.MediumOther_DayJisaKm ?? 0,
                        UnsoZaSyu = model?.MediumOther_UnsoZaSyu ?? 0,
                        UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd),
                        UpdTime = DateTime.Now.ToString(Formats.HHmmss),
                        UpdSyainCd = curentUser.SyainCdSeq,
                        UpdPrgId = Common.UpdPrgId
                    };
                    _context.TkdJitHou.Add(tkdJitHouMediumOther);

                    ////Small
                    //軽油
                    var tkdJitHouSmallLightOil = new TkdJitHou
                    {
                        UnsouKbn = (byte)searchParam.StrUnsouKbn,
                        SyoriYm = DateTime.ParseExact(searchParam?.StrDate, Formats.yyyyMM, null).ToString(Formats.yyyyMM),
                        EigyoCdSeq = searchParam.StrEigyoCdSeq,
                        KataKbn = (int)KataKbn.Small,
                        NenRyoKbn = (int)NenRyoKbn.LightOil,
                        NobeJyoCnt = model.SmallOil_NobeJyoCnt ?? 0,
                        NobeRinCnt = model.SmallOil_NobeRinCnt ?? 0,
                        NobeSumCnt = model.SmallOil_NobeSumCnt ?? 0,
                        NobeJitCnt = model.SmallOil_NobeJitCnt ?? 0,
                        JitudoRitu = model?.SmallOil_JitudoRitu ?? 0,
                        RinjiRitu = model.SmallOil_RinjiRitu ?? 0,
                        JitJisaKm = model.SmallOil_JitJisaKm ?? 0,
                        JitKisoKm = model.SmallOil_JitKisoKm ?? 0,
                        YusoJin = model.SmallOil_YusoJin ?? 0,
                        UnkoCnt = model.SmallOil_UnkoCnt ?? 0,
                        UnkoKikak1Cnt = model.SmallOil_UnkoKikak1Cnt ?? 0,
                        UnkoKikak2Cnt = model.SmallOil_UnkoKikak2Cnt ?? 0,
                        UnkoOthCnt = model.SmallOil_UnkoOthCnt ?? 0,
                        UnsoSyu = model.SmallOil_UnsoSyu ?? 0,
                        DayTotalKm = model.SmallOil_DayTotalKm ?? 0,
                        DayYusoJin = model.SmallOil_DayYusoJin ?? 0,
                        DayUnsoSyu = model.SmallOil_DayUnsoSyu ?? 0,
                        DayJisaKm = model.SmallOil_DayJisaKm ?? 0,
                        UnsoZaSyu = model?.SmallOil_UnsoZaSyu ?? 0,
                        UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd),
                        UpdTime = DateTime.Now.ToString(Formats.HHmmss),
                        UpdSyainCd = curentUser.SyainCdSeq,
                        UpdPrgId = Common.UpdPrgId
                    };
                    _context.TkdJitHou.Add(tkdJitHouSmallLightOil);

                    //ガソリン
                    var tkdJitHouSmallGasoline = new TkdJitHou
                    {
                        UnsouKbn = (byte)searchParam.StrUnsouKbn,
                        SyoriYm = DateTime.ParseExact(searchParam?.StrDate, Formats.yyyyMM, null).ToString(Formats.yyyyMM),
                        EigyoCdSeq = searchParam.StrEigyoCdSeq,
                        KataKbn = (int)KataKbn.Small,
                        NenRyoKbn = (int)NenRyoKbn.Gasoline,
                        NobeJyoCnt = model.SmallGasoline_NobeJyoCnt ?? 0,
                        NobeRinCnt = model.SmallGasoline_NobeRinCnt ?? 0,
                        NobeSumCnt = model.SmallGasoline_NobeSumCnt ?? 0,
                        NobeJitCnt = model.SmallGasoline_NobeJitCnt ?? 0,
                        JitudoRitu = model?.SmallGasoline_JitudoRitu ?? 0,
                        RinjiRitu = model.SmallGasoline_RinjiRitu ?? 0,
                        JitJisaKm = model.SmallGasoline_JitJisaKm ?? 0,
                        JitKisoKm = model.SmallGasoline_JitKisoKm ?? 0,
                        YusoJin = model.SmallGasoline_YusoJin ?? 0,
                        UnkoCnt = model.SmallGasoline_UnkoCnt ?? 0,
                        UnkoKikak1Cnt = model.SmallGasoline_UnkoKikak1Cnt ?? 0,
                        UnkoKikak2Cnt = model.SmallGasoline_UnkoKikak2Cnt ?? 0,
                        UnkoOthCnt = model.SmallGasoline_UnkoOthCnt ?? 0,
                        UnsoSyu = model.SmallGasoline_UnsoSyu ?? 0,
                        DayTotalKm = model.SmallGasoline_DayTotalKm ?? 0,
                        DayYusoJin = model.SmallGasoline_DayYusoJin ?? 0,
                        DayUnsoSyu = model.SmallGasoline_DayUnsoSyu ?? 0,
                        DayJisaKm = model.SmallGasoline_DayJisaKm ?? 0,
                        UnsoZaSyu = model?.SmallGasoline_UnsoZaSyu ?? 0,
                        UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd),
                        UpdTime = DateTime.Now.ToString(Formats.HHmmss),
                        UpdSyainCd = curentUser.SyainCdSeq,
                        UpdPrgId = Common.UpdPrgId
                    };
                    _context.TkdJitHou.Add(tkdJitHouSmallGasoline);

                    //LPG
                    var tkdJitHouSmallLPG = new TkdJitHou
                    {
                        UnsouKbn = (byte)searchParam.StrUnsouKbn,
                        SyoriYm = DateTime.ParseExact(searchParam?.StrDate, Formats.yyyyMM, null).ToString(Formats.yyyyMM),
                        EigyoCdSeq = searchParam.StrEigyoCdSeq,
                        KataKbn = (int)KataKbn.Small,
                        NenRyoKbn = (int)NenRyoKbn.LPG,
                        NobeJyoCnt = model.SmallLPG_NobeJyoCnt ?? 0,
                        NobeRinCnt = model.SmallLPG_NobeRinCnt ?? 0,
                        NobeSumCnt = model.SmallLPG_NobeSumCnt ?? 0,
                        NobeJitCnt = model.SmallLPG_NobeJitCnt ?? 0,
                        JitudoRitu = model?.SmallLPG_JitudoRitu ?? 0,
                        RinjiRitu = model.SmallLPG_RinjiRitu ?? 0,
                        JitJisaKm = model.SmallLPG_JitJisaKm ?? 0,
                        JitKisoKm = model.SmallLPG_JitKisoKm ?? 0,
                        YusoJin = model.SmallLPG_YusoJin ?? 0,
                        UnkoCnt = model.SmallLPG_UnkoCnt ?? 0,
                        UnkoKikak1Cnt = model.SmallLPG_UnkoKikak1Cnt ?? 0,
                        UnkoKikak2Cnt = model.SmallLPG_UnkoKikak2Cnt ?? 0,
                        UnkoOthCnt = model.SmallLPG_UnkoOthCnt ?? 0,
                        UnsoSyu = model.SmallLPG_UnsoSyu ?? 0,
                        DayTotalKm = model.SmallLPG_DayTotalKm ?? 0,
                        DayYusoJin = model.SmallLPG_DayYusoJin ?? 0,
                        DayUnsoSyu = model.SmallLPG_DayUnsoSyu ?? 0,
                        DayJisaKm = model.SmallLPG_DayJisaKm ?? 0,
                        UnsoZaSyu = model?.SmallLPG_UnsoZaSyu ?? 0,
                        UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd),
                        UpdTime = DateTime.Now.ToString(Formats.HHmmss),
                        UpdSyainCd = curentUser.SyainCdSeq,
                        UpdPrgId = Common.UpdPrgId
                    };
                    _context.TkdJitHou.Add(tkdJitHouSmallLPG);

                    //ガスタービン
                    var tkdJitHouSmallGasTurbine = new TkdJitHou
                    {
                        UnsouKbn = (byte)searchParam.StrUnsouKbn,
                        SyoriYm = DateTime.ParseExact(searchParam?.StrDate, Formats.yyyyMM, null).ToString(Formats.yyyyMM),
                        EigyoCdSeq = searchParam.StrEigyoCdSeq,
                        KataKbn = (int)KataKbn.Small,
                        NenRyoKbn = (int)NenRyoKbn.GasTurbine,
                        NobeJyoCnt = model.SmallGasTurbine_NobeJyoCnt ?? 0,
                        NobeRinCnt = model.SmallGasTurbine_NobeRinCnt ?? 0,
                        NobeSumCnt = model.SmallGasTurbine_NobeSumCnt ?? 0,
                        NobeJitCnt = model.SmallGasTurbine_NobeJitCnt ?? 0,
                        JitudoRitu = model?.SmallGasTurbine_JitudoRitu ?? 0,
                        RinjiRitu = model.SmallGasTurbine_RinjiRitu ?? 0,
                        JitJisaKm = model.SmallGasTurbine_JitJisaKm ?? 0,
                        JitKisoKm = model.SmallGasTurbine_JitKisoKm ?? 0,
                        YusoJin = model.SmallGasTurbine_YusoJin ?? 0,
                        UnkoCnt = model.SmallGasTurbine_UnkoCnt ?? 0,
                        UnkoKikak1Cnt = model.SmallGasTurbine_UnkoKikak1Cnt ?? 0,
                        UnkoKikak2Cnt = model.SmallGasTurbine_UnkoKikak2Cnt ?? 0,
                        UnkoOthCnt = model.SmallGasTurbine_UnkoOthCnt ?? 0,
                        UnsoSyu = model.SmallGasTurbine_UnsoSyu ?? 0,
                        DayTotalKm = model.SmallGasTurbine_DayTotalKm ?? 0,
                        DayYusoJin = model.SmallGasTurbine_DayYusoJin ?? 0,
                        DayUnsoSyu = model.SmallGasTurbine_DayUnsoSyu ?? 0,
                        DayJisaKm = model.SmallGasTurbine_DayJisaKm ?? 0,
                        UnsoZaSyu = model?.SmallGasTurbine_UnsoZaSyu ?? 0,
                        UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd),
                        UpdTime = DateTime.Now.ToString(Formats.HHmmss),
                        UpdSyainCd = curentUser.SyainCdSeq,
                        UpdPrgId = Common.UpdPrgId
                    };
                    _context.TkdJitHou.Add(tkdJitHouSmallGasTurbine);

                    //その他
                    var tkdJitHouSmallOther = new TkdJitHou
                    {
                        UnsouKbn = (byte)searchParam.StrUnsouKbn,
                        SyoriYm = DateTime.ParseExact(searchParam?.StrDate, Formats.yyyyMM, null).ToString(Formats.yyyyMM),
                        EigyoCdSeq = searchParam.StrEigyoCdSeq,
                        KataKbn = (int)KataKbn.Small,
                        NenRyoKbn = (int)NenRyoKbn.Other,
                        NobeJyoCnt = model.SmallOther_NobeJyoCnt ?? 0,
                        NobeRinCnt = model.SmallOther_NobeRinCnt ?? 0,
                        NobeSumCnt = model.SmallOther_NobeSumCnt ?? 0,
                        NobeJitCnt = model.SmallOther_NobeJitCnt ?? 0,
                        JitudoRitu = model?.SmallOther_JitudoRitu ?? 0,
                        RinjiRitu = model.SmallOther_RinjiRitu ?? 0,
                        JitJisaKm = model.SmallOther_JitJisaKm ?? 0,
                        JitKisoKm = model.SmallOther_JitKisoKm ?? 0,
                        YusoJin = model.SmallOther_YusoJin ?? 0,
                        UnkoCnt = model.SmallOther_UnkoCnt ?? 0,
                        UnkoKikak1Cnt = model.SmallOther_UnkoKikak1Cnt ?? 0,
                        UnkoKikak2Cnt = model.SmallOther_UnkoKikak2Cnt ?? 0,
                        UnkoOthCnt = model.SmallOther_UnkoOthCnt ?? 0,
                        UnsoSyu = model.SmallOther_UnsoSyu ?? 0,
                        DayTotalKm = model.SmallOther_DayTotalKm ?? 0,
                        DayYusoJin = model.SmallOther_DayYusoJin ?? 0,
                        DayUnsoSyu = model.SmallOther_DayUnsoSyu ?? 0,
                        DayJisaKm = model.SmallOther_DayJisaKm ?? 0,
                        UnsoZaSyu = model?.SmallOther_UnsoZaSyu ?? 0,
                        UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd),
                        UpdTime = DateTime.Now.ToString(Formats.HHmmss),
                        UpdSyainCd = curentUser.SyainCdSeq,
                        UpdPrgId = Common.UpdPrgId
                    };
                    _context.TkdJitHou.Add(tkdJitHouSmallOther);

                    await _context.SaveChangesAsync();
                    await dbTran.CommitAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    dbTran.Rollback();
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                }
            }
        }
    }
}
