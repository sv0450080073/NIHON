using Microsoft.EntityFrameworkCore;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;

namespace HassyaAllrightCloud.IService
{
    public interface ITKD_KoteiDataListService
    {
        Task<List<TKD_KoteiData>> GetDataKotei(bool common, string ukeno, bool isZenHaFlg, bool isKhakFlg, short teiDanNo, short UnkRen, short bunkRen, short Nittei);
        Task<LatestUpdYmdTime> GetLatestUpdYmd(string ukeNo, short unkRen, short teiDanNo, short bunkRen, byte tomKbn, short nittei);
    }
    public class TKD_KoteiDataService : ITKD_KoteiDataListService
    {
        private readonly KobodbContext _dbContext;

        public TKD_KoteiDataService(KobodbContext context)
        {
            _dbContext = context;
        }
        /// <summary>
        /// get data Kotei
        /// </summary>
        /// <param name="common"></param>
        /// <param name="ukeno"></param>
        /// <param name="isZenHaFlg"></param>
        /// <param name="isKhakFlg"></param>
        /// <param name="teiDanNo"></param>
        /// <param name="UnkRen"></param>
        /// <param name="bunkRen"></param>
        /// <param name="nittei"></param>
        /// <returns></returns>
        public async Task<List<TKD_KoteiData>> GetDataKotei(bool common, string ukeno, bool isZenHaFlg, bool isKhakFlg, short teiDanNo, short UnkRen, short bunkRen, short nittei)
        {
            return await (from TKD_Kotei in _dbContext.TkdKotei
                          join TKD_Koteik in _dbContext.TkdKoteik
                                on new { TKD_Kotei.UkeNo, TKD_Kotei.UnkRen, TKD_Kotei.TeiDanNo, TKD_Kotei.BunkRen, TKD_Kotei.TomKbn, TKD_Kotei.Nittei }
                            equals new { TKD_Koteik.UkeNo, TKD_Koteik.UnkRen, TKD_Koteik.TeiDanNo, TKD_Koteik.BunkRen, TKD_Koteik.TomKbn, TKD_Koteik.Nittei } into TKD_Koteik_join
                          from TKD_Koteik in TKD_Koteik_join.DefaultIfEmpty()
                          where
                          (common ?
                          (TKD_Koteik.TeiDanTomKbn == 9 && TKD_Koteik.TeiDanNo == 0) :
                          (TKD_Koteik.TeiDanNo == teiDanNo && TKD_Koteik.BunkRen == bunkRen)) &&
                          (isZenHaFlg ? TKD_Koteik.TomKbn == 2 : isKhakFlg ? TKD_Koteik.TomKbn == 3 : TKD_Koteik.TomKbn == 1) &&
                          TKD_Koteik.Nittei == nittei &&
                          TKD_Koteik.UnkRen == UnkRen &&
                          TKD_Kotei.UkeNo == ukeno &&
                          TKD_Kotei.SiyoKbn == 1 &&
                          TKD_Koteik.SiyoKbn == 1
                          select new TKD_KoteiData
                          {
                              Kotei_UkeNo = TKD_Kotei.UkeNo,
                              Kotei_UnkRen = TKD_Kotei.UnkRen,
                              Kotei_TeiDanNo = TKD_Kotei.TeiDanNo,
                              Kotei_BunkRen = TKD_Kotei.BunkRen,
                              Kotei_TomKbn = TKD_Kotei.TomKbn,
                              Kotei_Nittei = TKD_Kotei.Nittei,
                              Kotei_KouRen = TKD_Kotei.KouRen,
                              Kotei_Koutei = TKD_Kotei.Koutei,
                              Koteik_SyuPaTime = TKD_Koteik.SyuPaTime,
                              Koteik_TouChTime = TKD_Koteik.TouChTime,
                              Koteik_JisaIPKm = TKD_Koteik.JisaIpkm,
                              Koteik_JisaKSKm = TKD_Koteik.JisaKskm,
                              Koteik_KisoIPkm = TKD_Koteik.KisoIpkm,
                              Koteik_KisoKOKm = TKD_Koteik.KisoKokm,
                              Koteik_SyukoTime = TKD_Koteik.SyukoTime,
                              Koteik_HaiSTime = TKD_Koteik.HaiStime,
                              Koteik_KikTime = TKD_Koteik.KikTime,
                          }).ToListAsync();
        }

        public async Task<LatestUpdYmdTime> GetLatestUpdYmd(string ukeNo, short unkRen, short teiDanNo, short bunkRen, byte tomKbn, short nittei)
        {
            var koteik = await _dbContext.TkdKoteik.Where(e => e.UkeNo == ukeNo && e.UnkRen == unkRen && 
                                                                e.TeiDanNo == teiDanNo && e. BunkRen == bunkRen &&
                                                                e.TomKbn == tomKbn && e.Nittei == nittei)
                                                    .OrderByDescending(e => e.UpdYmd + e.UpdTime).FirstOrDefaultAsync();
            if(koteik != null)
                _dbContext.Entry(koteik).Reload();
            var kotei = await _dbContext.TkdKotei.Where(e => e.UkeNo == ukeNo && e.UnkRen == unkRen && 
                                                            e.TeiDanNo == teiDanNo && e.BunkRen == bunkRen &&
                                                            e.TomKbn == tomKbn && e.Nittei == nittei)
                                                    .OrderByDescending(e => e.UpdYmd + e.UpdTime).FirstOrDefaultAsync();
            if (kotei != null)
                _dbContext.Entry(kotei).Reload();
            var result = new LatestUpdYmdTime();
            long maxKoteikYmdTime = 0;
            long maxKoteiYmdTime = 0;
            if (koteik != null)
                long.TryParse(koteik.UpdYmd + koteik.UpdTime, out maxKoteikYmdTime);
            if (kotei != null)
                long.TryParse(kotei.UpdYmd + kotei.UpdTime, out maxKoteiYmdTime);

            return new LatestUpdYmdTime()
            {
                MaxKoteikYmdTime = maxKoteikYmdTime,
                MaxKoteiYmdTime = maxKoteiYmdTime
            };
        }
    }
}
