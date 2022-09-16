using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Linq;
using HassyaAllrightCloud.Domain.Dto;
using System;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.IService
{
    public interface ITKD_YoshaDataListService
    {
        Task<IEnumerable<TKD_YoshaData>> Getdata(int YouCdSeq, int YouSitCdSeq, string ukeno);
        TKD_YoshaDataGreen GetdataYouSitCdSeq(string UkeNo, short UnkRen, int YouTblSeq);
        void DeleteYoshaData(string UkeNo, short UnkRen, int YouTblSeq,string UpdPrgID);
        void DeleteYouSyuData(string UkeNo, short UnkRen, int YouTblSeq,short SyaSyuRen,string UpdPrgID);
    }
    public class TKD_YoshaDataService : ITKD_YoshaDataListService
    {
        private readonly KobodbContext _context;

        public TKD_YoshaDataService(KobodbContext context)
        {
            _context = context;
        }
        public void DeleteYoshaData(string UkeNo, short UnkRen, int YouTblSeq,string UpdPrgID)
        {
            var updatedata = _context.TkdYousha.Find(UkeNo, UnkRen, YouTblSeq);
            if (updatedata != null)
            {
                updatedata.SiyoKbn = 2;
                updatedata.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                updatedata.UpdTime = DateTime.Now.ToString("HHmmss");
                updatedata.UpdPrgId = UpdPrgID;
                updatedata.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                updatedata.HenKai = (short)(updatedata.HenKai + 1);
                _context.TkdYousha.Update(updatedata);
                _context.SaveChanges();
            }
        }
        public void DeleteYouSyuData(string UkeNo, short UnkRen, int YouTblSeq, short SyaSyuRen, string UpdPrgID)
        {
            var updatedata = _context.TkdYouSyu.Find(UkeNo, UnkRen, YouTblSeq,SyaSyuRen);
            if (updatedata != null)
            {
                updatedata.SiyoKbn = 2;
                updatedata.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                updatedata.UpdTime = DateTime.Now.ToString("HHmmss");
                updatedata.UpdPrgId = UpdPrgID;
                updatedata.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                updatedata.HenKai = (short)(updatedata.HenKai + 1);
                _context.TkdYouSyu.Update(updatedata);
                _context.SaveChanges();
            }
        }
    /// <summary>
    /// get Yosha data
    /// </summary>
    /// <param name="YouCdSeq"></param>
    /// <param name="YouSitCdSeq"></param>
    /// <returns></returns>
    public async Task<IEnumerable<TKD_YoshaData>> Getdata(int YouCdSeq, int YouSitCdSeq, string ukeno)
        {
            return await (from s in _context.TkdYousha
                          where s.YouCdSeq == YouCdSeq && s.YouSitCdSeq == YouSitCdSeq && s.SiyoKbn==1 && s.UkeNo==ukeno
                          select new TKD_YoshaData
                          {
                              Yosha_UkeNo = s.UkeNo,
                              Yosha_UnkRen = s.UnkRen,
                              Yosha_YouTblSeq = s.YouTblSeq,
                              Yosha_YouCdSeq = s.YouCdSeq,
                              Tokisk_YouSRyakuNm = "",
                          }).ToListAsync();

        }
        public TKD_YoshaDataGreen GetdataYouSitCdSeq(string UkeNo, short UnkRen, int YouTblSeq)
        {
            return (from s in _context.TkdYousha
                    where s.UkeNo == UkeNo && s.UnkRen == UnkRen && s.YouTblSeq == YouTblSeq
                    select new TKD_YoshaDataGreen
                    {
                        Yosha_YouSitCdSeq = s.YouSitCdSeq,
                        Yosha_Zeiritsu = s.Zeiritsu,
                        Yosha_YouCdSeq = s.YouCdSeq,
                    }).First();
            ;
        }
    }
}
