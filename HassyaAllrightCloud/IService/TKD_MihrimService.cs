using System.Collections.Generic;
using System;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Linq;
using System.Globalization;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.IService
{
    public interface ITKD_MihrimService
    {
        void DeleteMihrim(string ukeno, short unkRen, int YouTblSeq);
    }
    public class TKD_MihrimService : ITKD_MihrimService
    {
        private readonly KobodbContext _dbContext;
        public TKD_MihrimService(KobodbContext context)
        {
            _dbContext = context;
        }
        public void DeleteMihrim(string ukeno, short unkRen, int YouTblSeq)
        {
            var getdataMihrim = _dbContext.TkdMihrim.Where(t => t.UkeNo == ukeno && t.UnkRen == unkRen && t.YouTblSeq == YouTblSeq).ToList();
            if(getdataMihrim.Count()>0)
            {
                foreach(var item in getdataMihrim)
                {
                    var updateMihrim = _dbContext.TkdMihrim.Find(item.UkeNo, item.MihRen);
                    updateMihrim.SiyoKbn = 2;
                    updateMihrim.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    updateMihrim.UpdTime = DateTime.Now.ToString("HHmmss");
                    updateMihrim.UpdPrgId = "KU1300";
                    updateMihrim.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    updateMihrim.HenKai = (short)(updateMihrim.HenKai + 1);
                    _dbContext.TkdMihrim.Update(updateMihrim);
                    _dbContext.SaveChanges();
                }    
            }    
        }
    }
}
