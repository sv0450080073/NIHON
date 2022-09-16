using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ITKD_KoteikDataListService
    {
        TKD_KoteikData GetDataKoteik(string ukeno, short unkRen, short teiDanNo, short bunkRen, int teiDanTomKbn, int tomKbn, short nittei);
    }
    public class TKD_KoteikDataService : ITKD_KoteikDataListService
    {
        private readonly KobodbContext _dbContext;

        public TKD_KoteikDataService(KobodbContext context)
        {
            _dbContext = context;
        }
        public TKD_KoteikData GetDataKoteik(string ukeno, short unkRen, short teiDanNo, short bunkRen, int teiDanTomKbn, int tomKbn, short nittei)
        {
            var data = (from TKD_Koteik in _dbContext.TkdKoteik
                        where TKD_Koteik.UkeNo == ukeno
                        && TKD_Koteik.UnkRen == unkRen
                        && TKD_Koteik.TeiDanNo == teiDanNo
                        && TKD_Koteik.BunkRen == bunkRen
                        && TKD_Koteik.TeiDanTomKbn == teiDanTomKbn
                        && TKD_Koteik.TomKbn == tomKbn
                        && TKD_Koteik.Nittei == nittei
                        select new TKD_KoteikData()
                        {
                            UkeNo = TKD_Koteik.UkeNo,
                            UnkRen = TKD_Koteik.UnkRen,
                            TeiDanNo = TKD_Koteik.TeiDanNo,
                            SyuPaTime = TKD_Koteik.SyuPaTime,
                            TouChTime = TKD_Koteik.TouChTime,
                            SyukoTime = TKD_Koteik.SyukoTime,
                            HaiSTime = TKD_Koteik.HaiStime,
                            KikTime = TKD_Koteik.KikTime,
                        }).FirstOrDefault();
            return data;
        }
    }
}
