using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HassyaAllrightCloud.Infrastructure.Persistence;
using Microsoft.Extensions.Caching.Memory;
using HassyaAllrightCloud.Domain.Dto;
using DevExpress.XtraRichEdit.Commands.Internal;

namespace HassyaAllrightCloud.IService
{
    public interface IBusTypeListService
    {

        Task<IEnumerable<BusTypeData>> Get(int tenantId);
        Task<List<BusTypesData>> GetAll(int Tenant);
        Task<List<BusTypesData>> GetBusTypeBySyaSyuCd(int Tenant, int kataKbn);
    }
    public class BusTypeService : IBusTypeListService
    {
        private readonly KobodbContext _context;
        public IMemoryCache MemoryCache { get; }
        private readonly ITPM_CodeSyService _codeSyuService;

        public BusTypeService(KobodbContext context, IMemoryCache memoryCache, ITPM_CodeSyService codeSyuService)
        {
            _context = context;
            MemoryCache = memoryCache;
            _codeSyuService = codeSyuService;
        }
        public Task<IEnumerable<BusTypeData>> Get(int tenantId)
        {
            return MemoryCache.GetOrCreateAsync("AllBusType" + tenantId, async e =>
            {
                e.SetOptions(new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromSeconds(10)
                });
                string codeSyuKATAKBN = "KATAKBN";
                int tenantKATAKBN = await _codeSyuService.CheckTenantByKanriKbnAsync(tenantId, codeSyuKATAKBN);
                var data = (from s in _context.VpmSyaSyu
                            join c in _context.VpmCodeKb
                            on s.KataKbn.ToString() equals c.CodeKbn
                            where c.CodeSyu == codeSyuKATAKBN && s.SiyoKbn == 1 && s.TenantCdSeq == tenantId && c.TenantCdSeq == tenantKATAKBN
                            select new BusTypeData()
                            {
                                Katakbn = c.CodeKbn,
                                RyakuNm = c.RyakuNm,
                                SyaSyuCdSeq = s.SyaSyuCdSeq,
                                SyaSyuCd = s.SyaSyuCd,
                                SyaSyuNm = s.SyaSyuNm,
                            }).ToList();
                var nulllist = (from c in _context.VpmCodeKb
                                where c.CodeSyu == codeSyuKATAKBN && c.CodeKbn != "9" && c.TenantCdSeq == tenantKATAKBN
                                select new BusTypeData()
                                {
                                    Katakbn = c.CodeKbn,
                                    RyakuNm = c.RyakuNm,
                                    SyaSyuCdSeq = 0,
                                    SyaSyuCd = 0,
                                    SyaSyuNm = "指定なし",
                                }).ToList();
                var datafinnal = data.Concat(nulllist).OrderBy(c => c.Katakbn).ThenBy(n => n.SyaSyuCd).ToList();

                return datafinnal.AsEnumerable();
            });
        }
        public async Task<List<BusTypesData>> GetAll(int Tenant)
        {
            return await (from syaSyu in _context.VpmSyaSyu
                          where syaSyu.SiyoKbn == 1 && syaSyu.TenantCdSeq == Tenant
                          orderby syaSyu.SyaSyuCd
                          select new BusTypesData(syaSyu)
                ).ToListAsync();
        }

        public async Task<List<BusTypesData>> GetBusTypeBySyaSyuCd(int Tenant, int kataKbn)
        {
            return await(from syaSyu in _context.VpmSyaSyu
                         where syaSyu.SiyoKbn == 1
                         && syaSyu.TenantCdSeq == Tenant
                         && syaSyu.KataKbn== kataKbn
                         orderby  syaSyu.SyaSyuCd
                        select new BusTypesData(syaSyu)
               ).ToListAsync();
        }
    }
}
