using HassyaAllrightCloud.Application.CodeKb.Queries;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ITPM_CodeKbListService
    {
        /// <summary>
        /// Get CodeKb by CodeSuy and lisy codeKbn
        /// </summary>
        /// <param name="codeKbns"></param>
        /// <param name="codeSyu"></param>
        /// <param name="tenantCdSeq"></param>
        /// <returns></returns>
        Task<List<TPM_CodeKbData>> GetCodeKbByCodeSyuAndListCodeKbn(List<string> codeKbns, string codeSyu, int tenantCdSeq);
        Task<IEnumerable<TPM_CodeKbData>> Getdata(int TenantCdSeq);
        /// <summary>
        /// get codekbn KENCD
        /// </summary>
        /// <returns></returns>
        Task<List<TPM_CodeKbDataKenCD>> GetdataKenCD(int TenantCdSeq);
        Task<List<TPM_CodeKbDataKenCD>> GetdataKenCDYouSha(int TenantCdSeq);
        
        /// <summary>
        /// get codekbn BUNRUICD
        /// </summary>
        /// <returns></returns>
        Task<List<TPM_CodeKbDataBunruiCD>> GetdataBunruiCD(int tenantCdSeq);
        /// <summary>
        /// get codekbn OTHJINKBN
        /// </summary>
        /// <returns></returns>
        Task<List<TPM_CodeKbDataOTHJINKBN>> GetdataOTHJINKBN(int tenantCdSeq);
        /// <summary>
        /// get data 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<List<TPM_CodeKbDataDepot>> GetdataDepot(DateTime date, int TenantCdSeq);
        Task<List<CodeTypeData>> GetDantai(int Tenant);
        Task<List<VpmCodeKb>> GetJoken(int Tenant);
        Task<IEnumerable<TPM_CodeKbData>> Getdatabusrepair(int TenantCdSeq);
        Task<IEnumerable<VPM_RepairData>> GetDataBusRepairType(int TenantCdSeq);
        Task<List<VpmCodeKb>> GetScheduleType(int Tenant);
        Task<List<VpmCodeKb>> GetScheduleLabel(int Tenant);
        Task<List<TPM_CodeKbCodeSyuData>> GetDataByCodeSyu(string codeSyu, int TenantCdSeq);
        /// <summary>
        /// get code TENKOBOMOKUHYO
        /// </summary>
        /// <returns></returns>
        Task<List<TPM_CodeKbDataReport>> GetdataTENKOBOMOKUHYO(int tenantCdSeq);
        /// <summary>
        /// get code TENKOBOSHIJI
        /// </summary>
        /// <returns></returns>
        Task<List<TPM_CodeKbDataReport>> GetdataTENKOBOSHIJI(int tenantCdSeq);
        Task<IEnumerable<InvoiceType>> GetdataSEIKYUKBN(int tenantId);
        Task<List<CodeKbnBunruiDataPopup>> GetCodeKbnBunruiData(int tenantCdSeq);
        Task<List<VehicleDispatchPopup>> GetVehicleDispatchDataPopup(int tenantCdSeq, string date);

        Task<List<TPM_CodeKbData>> GetdataUKEJYKBNCD(int tenantCdSeq);
        /// <summary>
        /// Get code: KATAKBN by yousha codeKbn
        /// </summary>
        /// <param name="tenantCdSeq"></param>
        /// <param name="codeKbn"></param>
        /// <returns></returns>
        Task<List<TPM_CodeKbData>> GetCodeKATAKBNByYoushaCodeKbn(int tenantCdSeq, string codeKbn);

        Task<List<CodeDataModel>> GetDataByNameAsync(string name);

    }
    public class TPM_CodeKbService : ITPM_CodeKbListService
    {
        private readonly KobodbContext _context;
        private readonly IMediator _mediatR;
        private readonly ITPM_CodeSyService _codeSyuService;
        private readonly ILogger<TPM_CodeKbService> _logger;
        public IMemoryCache MemoryCache { get; }

        public TPM_CodeKbService(KobodbContext context,
            IMediator mediatR,
            ILogger<TPM_CodeKbService> logger,
            IMemoryCache memoryCache,
            ITPM_CodeSyService codeSyuService)
        {
            _context = context;
            _mediatR = mediatR;
            _logger = logger;
            _codeSyuService = codeSyuService;
            MemoryCache = memoryCache;
        }

        public async Task<List<TPM_CodeKbDataOTHJINKBN>> GetdataOTHJINKBN(int tenantCdSeq)
        {
            string codeSyu = "OTHJINKBN";

            return await _codeSyuService.FilterTenantIdByCodeSyu(async (tenantId, code) =>
            {
                return await
                    (from s in _context.VpmCodeKb
                     where s.CodeSyu == code && s.SiyoKbn == 1 && s.TenantCdSeq == tenantId
                     select new TPM_CodeKbDataOTHJINKBN
                     {
                         CodeKb_CodeKbnSeq = Convert.ToInt32(s.CodeKbnSeq),
                         CodeKb_CodeKbn = s.CodeKbn,
                         CodeKb_RyakuNm = s.RyakuNm,
                     }).ToListAsync();
            },
            tenantCdSeq, codeSyu);
        }

        public async Task<IEnumerable<TPM_CodeKbData>> Getdatabusrepair(int tenantCdSeq)
        {
            string codeSyu = "SHURICD";

            return await _codeSyuService.FilterTenantIdByCodeSyu(async (tenantId, code) =>
            {
                return await
                    (from s in _context.VpmCodeKb
                     where s.CodeSyu == codeSyu && s.SiyoKbn == 1 && s.TenantCdSeq == tenantId
                     select new TPM_CodeKbData
                     {
                         CodeKb_CodeKbnSeq = Convert.ToInt32(s.CodeKbnSeq),
                         CodeKb_CodeSyu = s.CodeSyu,
                         CodeKb_CodeKbn = s.CodeKbn,
                         CodeKb_RyakuNm = s.RyakuNm,
                     }).ToListAsync();
            },
            tenantCdSeq, codeSyu);
        }

        /// <summary>
        /// Get Data Bus Repair Type
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<VPM_RepairData>> GetDataBusRepairType(int TenantCdSeq)
        {
            return await (from s in _context.VpmRepair
                          where s.SiyoKbn == 1 && s.TenantCdSeq == TenantCdSeq
                          select new VPM_RepairData
                          {
                              RepairCdSeq = s.RepairCdSeq,
                              RepairCd = s.RepairCd,
                              RepairNm = s.RepairNm,
                              RepairSeiKbn = s.RepairSeiKbn
                          }).ToListAsync();

        }

        /// <summary>
        /// get data 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<List<TPM_CodeKbDataDepot>> GetdataDepot(DateTime date,int tenantCdSeq)
        {
            string codeSyu = "OTHJINKBN";

            return await _codeSyuService.FilterTenantIdByCodeSyu(async (tenantId, code) =>
            {
                string DateStarAsString = date.ToString("yyyyMMdd");
                return await
                    (from s in _context.VpmCodeKb
                     join k in _context.VpmKoutu on s.CodeKbnSeq equals k.BunruiCdSeq
                     join b in _context.VpmBin on k.KoukCdSeq equals b.KoukCdSeq
                     where
                     s.CodeSyu == codeSyu && s.SiyoKbn == 1 && k.SiyoKbn == 1 && s.TenantCdSeq == tenantId
                     && DateStarAsString.CompareTo(b.SiyoStaYmd) >= 0 &&
                           DateStarAsString.CompareTo(b.SiyoEndYmd) <= 0
                     orderby k.BunruiCdSeq, b.BinCdSeq
                     select new TPM_CodeKbDataDepot
                     {
                         CodeKbCodeKbnSeq = Convert.ToInt32(s.CodeKbnSeq),
                         CodeKbCodeKbnNm = s.CodeKbnNm,
                         KoutuKoukCd = k.KoukCd,
                         KoutuKoukCdSeq = k.KoukCdSeq,
                         KoutuRyakuNm = k.RyakuNm,
                         BinBinCd = b.BinCd,
                         BinBinCdSeq = b.BinCdSeq,
                         BinBinNm = b.BinNm,

                     }).ToListAsync();
            },
            tenantCdSeq, codeSyu);
        }

        public async Task<List<TPM_CodeKbDataKenCD>> GetdataKenCD(int tenantCdSeq)
        {
            string codeSyu = "KENCD";

            return await _codeSyuService.FilterTenantIdByCodeSyu(async (tenantId, code) =>
            {
                return await
                    (from s in _context.VpmCodeKb
                     join b in _context.VpmBasyo on s.CodeKbnSeq equals b.BasyoKenCdSeq
                     where
                     s.CodeSyu == codeSyu 
                     && s.SiyoKbn == 1
                     && b.SiyoKbn == 1 
                     && b.SiyoIkiKbn == 1
                     && s.TenantCdSeq == tenantId
                     orderby s.CodeKbnSeq, b.BunruiCdSeq
                     select new TPM_CodeKbDataKenCD
                     {
                         CodeKbCodeKbnSeq = Convert.ToInt32(s.CodeKbnSeq),
                         CodeKbCodeKbnNm = s.CodeKbnNm,
                         BasyoBasyoKenCdSeq = b.BasyoKenCdSeq,
                         BasyoBasyoMapCd = b.BasyoMapCd,
                         BasyoBasyoMapCdSeq = b.BasyoMapCdSeq,
                         BasyoBasyoNm = b.BasyoNm,
                     }).ToListAsync();
            },
            tenantCdSeq, codeSyu);
        }

        public async Task<List<TPM_CodeKbDataKenCD>> GetdataKenCDYouSha(int tenantCdSeq)
        {
            var result = new List<TPM_CodeKbDataKenCD>();
            string codeSyu = "KENCD";
            result = await
                      _codeSyuService.FilterTenantIdByCodeSyu((tenantId, codeSyu) =>
                      {
                          return
                               (from b in _context.VpmBasyo
                                join c in _context.VpmCodeKb
                                on new { KbSeq = b.BasyoKenCdSeq, TenantCdSeq = tenantId, CodeSyu = codeSyu, SiyoKbn = 1 } equals
                                new { KbSeq = c.CodeKbnSeq, TenantCdSeq = c.TenantCdSeq, CodeSyu = c.CodeSyu, SiyoKbn = (int)c.SiyoKbn }
                                where b.TenantCdSeq == new ClaimModel().TenantID
                                select new TPM_CodeKbDataKenCD()
                                {
                                    BasyoBasyoKenCdSeq = b.BasyoKenCdSeq,
                                    CodeKbCodeKbnNm =  c.CodeKbnNm ,
                                    BasyoBasyoMapCdSeq = b.BasyoMapCdSeq,
                                    BasyoBasyoMapCd = b.BasyoMapCd,
                                    BasyoBasyoNm = b.BasyoNm
                                }).ToListAsync();
                      }, tenantCdSeq, codeSyu);
            return result;
        }


        public async Task<List<TPM_CodeKbDataBunruiCD>> GetdataBunruiCD(int tenantCdSeq)
        {
            string codeSyu = "BUNRUICD";

            return await _codeSyuService.FilterTenantIdByCodeSyu(async (tenantId, code) =>
            {
                return await
                    (from s in _context.VpmCodeKb
                     join b in _context.VpmHaichi on s.CodeKbnSeq equals b.BunruiCdSeq
                     where
                     s.CodeSyu == codeSyu && s.SiyoKbn == 1 && b.SiyoKbn == 1 && s.TenantCdSeq == tenantId
                     orderby s.CodeKbnSeq, b.BunruiCdSeq
                     select new TPM_CodeKbDataBunruiCD
                     {
                         CodeKbCodeKbnSeq = Convert.ToInt32(s.CodeKbnSeq),
                         CodeKbCodeKbnNm = s.CodeKbnNm,
                         HaiChiBunruiCdSeq = b.BunruiCdSeq,
                         HaiChiHaiSCdSeq = b.HaiScdSeq,
                         HaiChiHaiSNm = b.HaiSnm,
                         HaiChiHaiSCd = b.HaiScd,
                     }).ToListAsync();
            },
            tenantCdSeq, codeSyu);
        }

        /// <summary>
        /// get codekbn KATAKBN
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TPM_CodeKbData>> Getdata(int TenantCdSeq)
        {
            return await (from s in _context.VpmCodeKb
                          where s.CodeSyu == "KATAKBN" && s.SiyoKbn == 1 && s.TenantCdSeq == TenantCdSeq
                          select new TPM_CodeKbData
                          {
                              CodeKb_CodeKbnSeq = Convert.ToInt32(s.CodeKbnSeq),
                              CodeKb_CodeSyu = s.CodeSyu,
                              CodeKb_CodeKbn = s.CodeKbn,
                              CodeKb_RyakuNm = s.RyakuNm,
                          }).ToListAsync();

        }

        /// <summary>
        /// get codekbn DANTAICD
        /// </summary>
        /// <returns></returns>
        public async Task<List<CodeTypeData>> GetDantai(int Tenant)
        {
            return await (from codeKb in _context.VpmCodeKb
                          join tmp_CodeSy01 in _context.VpmCodeSy
                              on codeKb.CodeSyu equals tmp_CodeSy01.CodeSyu into ps1
                          from tmp_CodeSy01 in ps1.DefaultIfEmpty()
                          where codeKb.CodeSyu == "DANTAICD"
                               && codeKb.SiyoKbn == 1
                               && ((tmp_CodeSy01.KanriKbn == 1
                                 && codeKb.TenantCdSeq == 0)
                                 || (tmp_CodeSy01.KanriKbn != 1
                                 && codeKb.TenantCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().TenantID))
                          orderby codeKb.CodeKbn
                          select new CodeTypeData(codeKb)).ToListAsync();
        }
        /// <summary>
        /// get code UKEJYKBNCD
        /// </summary>
        /// <returns></returns>
        public async Task<List<VpmCodeKb>> GetJoken(int Tenant)
        {
            return await (from codeKb in _context.VpmCodeKb
                          where codeKb.CodeSyu == "UKEJYKBNCD" && codeKb.SiyoKbn == 1 && codeKb.TenantCdSeq == Tenant
                          orderby codeKb.CodeKbn, codeKb.CodeKbnSeq
                          select codeKb).ToListAsync();
        }
        /// <summary>
        /// get codekbn YOTEITYPE
        /// </summary>
        /// <returns></returns>
        public async Task<List<VpmCodeKb>> GetScheduleType(int Tenant)
        {
            var data = (from codeKb in _context.VpmCodeKb
                              where codeKb.CodeSyu == "YOTEITYPE" && codeKb.SiyoKbn == 1 && codeKb.TenantCdSeq == Tenant
                              orderby codeKb.CodeKbn, codeKb.CodeKbnSeq
                              select codeKb).ToList();
            if(data == null || data.Count == 0)
            {
                data = (from codeKb in _context.VpmCodeKb
                              where codeKb.CodeSyu == "YOTEITYPE" && codeKb.SiyoKbn == 1 && codeKb.TenantCdSeq == 0
                              orderby codeKb.CodeKbn, codeKb.CodeKbnSeq
                              select codeKb).ToList();
            }
            return data;
        }
        /// <summary>
        /// get code YOTEILABKBN
        /// </summary>
        /// <returns></returns>
        public async Task<List<VpmCodeKb>> GetScheduleLabel(int Tenant)
        {
            var data = (from codeKb in _context.VpmCodeKb
                              where codeKb.CodeSyu == "YOTEILABKBN"
                              && codeKb.SiyoKbn == 1
                              && codeKb.TenantCdSeq == Tenant
                              orderby codeKb.CodeKbn, codeKb.CodeKbnSeq
                              select codeKb).ToList();
            if (data == null || data.Count == 0)
            {
                data = (from codeKb in _context.VpmCodeKb
                              where codeKb.CodeSyu == "YOTEILABKBN"
                              && codeKb.SiyoKbn == 1
                              && codeKb.TenantCdSeq == 0
                              orderby codeKb.CodeKbn, codeKb.CodeKbnSeq
                              select codeKb).ToList();
            }
            return data;
        }

        public async Task<List<TPM_CodeKbCodeSyuData>> GetDataByCodeSyu(string codeSyu, int TenantCdSeq)
        {
            return await _codeSyuService.FilterTenantIdByCodeSyu(async (tenantId, code) =>
            {
                return await (from s in _context.VpmCodeKb
                              where s.CodeSyu == codeSyu && s.TenantCdSeq == tenantId
                              select new TPM_CodeKbCodeSyuData
                              {
                                  CodeKbn = s.CodeKbn,
                                  RyakuNm = s.RyakuNm
                              }).ToListAsync();
            },
            TenantCdSeq, codeSyu);
        }

        public async Task<List<TPM_CodeKbDataReport>> GetdataTENKOBOMOKUHYO(int tenantCdSeq)
        {
            string codeSyu = "TENKOBOMOKUHYO";

            return await _codeSyuService.FilterTenantIdByCodeSyu(async (tenantId, code) =>
            {
                return 
                    await (from s in _context.VpmCodeKb
                           where s.CodeSyu == codeSyu && 
                                 s.SiyoKbn == 1 &&
                                 s.TenantCdSeq == tenantId
                           select new TPM_CodeKbDataReport
                           {
                               CodeKb_CodeKbnSeq = s.CodeKbnSeq,
                               CodeKb_CodeSyu = s.CodeSyu,
                               CodeKb_CodeKbn = s.CodeKbn,
                               CodeKb_CodeKbnNm = s.CodeKbnNm,
                           }).ToListAsync();
            },
            tenantCdSeq, codeSyu);
        }

        public async Task<List<TPM_CodeKbDataReport>> GetdataTENKOBOSHIJI(int tenantCdSeq)
        {
            string codeSyu = "TENKOBOSHIJI";

            return await _codeSyuService.FilterTenantIdByCodeSyu(async (tenantId, code) =>
            {
                return
                    await (from s in _context.VpmCodeKb
                           where s.CodeSyu == codeSyu && 
                                 s.SiyoKbn == 1 &&
                                 s.TenantCdSeq == tenantId
                           select new TPM_CodeKbDataReport
                           {
                               CodeKb_CodeKbnSeq = s.CodeKbnSeq,
                               CodeKb_CodeSyu = s.CodeSyu,
                               CodeKb_CodeKbn = s.CodeKbn,
                               CodeKb_CodeKbnNm = s.CodeKbnNm,
                           }).ToListAsync();
            },
            tenantCdSeq, codeSyu);
        }

        /// <summary>
        /// get code SEIKYUKBN
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<InvoiceType>> GetdataSEIKYUKBN(int tenantId)
        {
            return await(from s in _context.VpmCodeKb
                         where s.CodeSyu.Contains("SEIKYUKBN") && s.TenantCdSeq == tenantId && s.SiyoKbn == 1
                         select new InvoiceType
                         {
                             CodeKbnSeq = s.CodeKbnSeq,
                             CodeSyu = s.CodeSyu,
                             CodeKbn = s.CodeKbn,
                             CodeKbnNm = s.CodeKbnNm,
                             RyakuNm = s.RyakuNm,
                             CodeSeiKbn = s.CodeSeiKbn,
                             SiyoKbn = s.SiyoKbn
                         }).ToListAsync();
            //return MemoryCache.GetOrCreateAsync("AllInvoiceType" + tenantId, async e =>
            //{
            //    e.SetOptions(new MemoryCacheEntryOptions
            //    {
            //        AbsoluteExpirationRelativeToNow =
            //        TimeSpan.FromSeconds(10)
            //    });

            //    return await Task.FromResult((from s in _context.VpmCodeKb
            //                                  where s.CodeSyu.Contains("SEIKYUKBN") && s.TenantCdSeq == tenantId
            //                                  select new InvoiceType
            //                                  {
            //                                      CodeKbnSeq = s.CodeKbnSeq,
            //                                      CodeSyu = s.CodeSyu,
            //                                      CodeKbn = s.CodeKbn,
            //                                      CodeKbnNm = s.CodeKbnNm,
            //                                      RyakuNm = s.RyakuNm,
            //                                      CodeSeiKbn = s.CodeSeiKbn,
            //                                      SiyoKbn = s.SiyoKbn
            //                                  }).AsEnumerable());
            //});
        }

        public async Task<List<VehicleDispatchPopup>> GetVehicleDispatchDataPopup(int tenantCdSeq, string date)
        {
            var result = new List<VehicleDispatchPopup>();
            try
            {
                string codeSyuBUNRUICD = "BUNRUICD";
                int tenantBUNRUICD = await _codeSyuService.CheckTenantByKanriKbnAsync(tenantCdSeq, codeSyuBUNRUICD);
                result = (from KOUTU in _context.VpmKoutu
                          join BUNRUI in _context.VpmCodeKb
                          on new { K1 = KOUTU.BunruiCdSeq, K2 = 1, K3 = tenantBUNRUICD }
                          equals new { K1 = BUNRUI.CodeKbnSeq, K2 = (int)BUNRUI.SiyoKbn, K3 = BUNRUI.TenantCdSeq }
                          into BUNRUI_join
                          from BUNRUI in BUNRUI_join.DefaultIfEmpty()
                          join BIN in _context.VpmBin
                          on new { K1 = KOUTU.KoukCdSeq, K2=true ,K3=true, B4 = new ClaimModel().TenantID }
                          equals new { K1 = BIN.KoukCdSeq,K2= String.Compare(BIN.SiyoStaYmd, date) <= 0 , K3= String.Compare(BIN.SiyoEndYmd, date) >= 0 , B4 = BIN.TenantCdSeq }
                          into BIN_join
                          from BIN in BIN_join.DefaultIfEmpty()
                          where KOUTU.SiyoKbn == 1 && BUNRUI.CodeSyu == codeSyuBUNRUICD
                          select new VehicleDispatchPopup()
                          {
                              KOUTU_BunruiCdSeq = KOUTU.BunruiCdSeq,
                              KOUTU_KoukCdSeq = KOUTU.KoukCdSeq,
                              KOUTU_KoukCd = KOUTU.KoukCd,
                              KOUTU_KoukNm = KOUTU.KoukNm,
                              BUNRUI_CodeKbnNm = BUNRUI.CodeKbnNm,
                              BIN_BinCdSeq = BIN.BinCdSeq,
                              BIN_BinCd = BIN.BinCd,
                              BIN_BinNm = BIN.BinNm,
                              BIN_SyuPaTime = BIN.SyuPaTime,
                              BIN_TouChTime = BIN.TouChTime
                          }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }
        public async Task<List<CodeKbnBunruiDataPopup>> GetCodeKbnBunruiData(int tenantCdSeq)
        {
            var result = new List<CodeKbnBunruiDataPopup>();
            try
            {
                string codeSyuBUNRUICD = "BUNRUICD";
                int tenantBUNRUICD = await _codeSyuService.CheckTenantByKanriKbnAsync(tenantCdSeq, codeSyuBUNRUICD);
                result = (from VPM_Haichi in _context.VpmHaichi
                          join VPM_CodeKb in _context.VpmCodeKb
                          on new { C1 = VPM_Haichi.BunruiCdSeq, C2 = 1, C3 = tenantBUNRUICD }
                          equals new { C1 = VPM_CodeKb.CodeKbnSeq, C2 = (int)VPM_CodeKb.SiyoKbn, C3 = VPM_CodeKb.TenantCdSeq }
                          into VPM_CodeKb_join
                          from VPM_CodeKb in VPM_CodeKb_join.DefaultIfEmpty()
                          where VPM_Haichi.SiyoKbn == 1 && VPM_CodeKb.CodeSyu == codeSyuBUNRUICD
                          select new CodeKbnBunruiDataPopup()
                          {
                              HAICHI_BunruiCdSeq = VPM_Haichi.BunruiCdSeq,
                              HAICHI_HaiSCdSeq = VPM_Haichi.HaiScdSeq,
                              HAICHI_HaiSCd = VPM_Haichi.HaiScd,
                              HAICHI_RyakuNm = VPM_Haichi.RyakuNm,
                              HAICHI_Jyus1 = VPM_Haichi.Jyus1,
                              HAICHI_Jyus2 = VPM_Haichi.Jyus2,
                              HAICHI_HaiSKigou = VPM_Haichi.HaiSkigou,
                              BUNRUI_CodeKbnNm = VPM_CodeKb.CodeKbnNm
                          }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public async Task<List<TPM_CodeKbData>> GetdataUKEJYKBNCD(int tenantCdSeq)
        {
            string codeSyuUKEJYKBNCD = "UKEJYKBNCD";
                int tenantUKEJYKBNCD = await _codeSyuService.CheckTenantByKanriKbnAsync(tenantCdSeq, codeSyuUKEJYKBNCD);
            return await(from codeKb in _context.VpmCodeKb
                         where codeKb.CodeSyu == codeSyuUKEJYKBNCD && codeKb.SiyoKbn == 1 && codeKb.TenantCdSeq == tenantUKEJYKBNCD
                         orderby codeKb.CodeKbn, codeKb.CodeKbnSeq
                         select new TPM_CodeKbData()
                         { 
                             CodeKb_CodeKbn = codeKb.CodeKbn,
                             CodeKb_RyakuNm = codeKb.RyakuNm
                         }).ToListAsync();
        }

        public async Task<List<TPM_CodeKbData>> GetCodeKATAKBNByYoushaCodeKbn(int tenantCdSeq, string codeKbn)
        {
            if (codeKbn == null)
                throw new ArgumentNullException(nameof(codeKbn));

            return await _codeSyuService.FilterTenantIdByCodeSyu(async (tenantId, code) => { 
                return await
                    (from s in _context.VpmCodeKb
                     where s.CodeKbn == codeKbn &&
                           s.CodeSyu == code && 
                           s.SiyoKbn == 1 && 
                           s.TenantCdSeq == tenantId
                     select new TPM_CodeKbData
                     {
                         CodeKb_CodeKbnSeq = Convert.ToInt32(s.CodeKbnSeq),
                         CodeKb_CodeKbn = s.CodeKbn,
                         CodeKb_RyakuNm = s.RyakuNm,
                     }).ToListAsync();
            }, tenantCdSeq, "KATAKBN");
        }

        public async Task<List<TPM_CodeKbData>> GetCodeKbByCodeSyuAndListCodeKbn(List<string> codeKbns, string codeSyu, int tenantCdSeq)
        {
            return await _mediatR.Send(new GetCodeKbByCodeSyuAndListCodeKbnQuery(codeKbns, codeSyu, tenantCdSeq, _codeSyuService));
        }

        public async Task<List<CodeDataModel>> GetDataByNameAsync(string name)
        {
            return await _mediatR.Send(new GetDataByNameAsyncQuery { Name = name });
        }
    }
}
