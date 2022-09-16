using HassyaAllrightCloud.Application.CarCooperation.Queries;
using HassyaAllrightCloud.Application.YoushaNotice.Queries;
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
    public interface ICarCooperationListService
    {
        bool CheckDataYoushaNotice (int motoTenantCdSeq,string motoUkeNo,short motoUnkRen,int motoYouTblSeq );
        Task<CarCooperationData> Getdata(string UkeNo, short UnkRen, int YouTblSeq);
        Task<List<YoushaNoticeData>> GetYoushaNoticeData(int _tenantCdSeq);
        Task<YoushaNoticeData> GetItemYoushaNotice(int motoTenantCdSeq,string motoUkeNo,short motoUnkRen,int motoYouTblSeq);
        void UpdateUnReadKbnYoushaNotice(int MotoTenantCdSeq, string MotoUkeNo, short MotoUnkRen, int MotoYouTblSeq);
        void UpdateYoushaNotice(int MotoTenantCdSeq, string MotoUkeNo, short MotoUnkRen, int MotoYouTblSeq);
    }

        public class CarCooperationService:ICarCooperationListService
    {
        private readonly KobodbContext _context;
        private readonly IMediator _mediatR;
        private readonly ILogger<TPM_CodeKbService> _logger;
        public IMemoryCache MemoryCache { get; }
        public CarCooperationService(KobodbContext context,
           IMediator mediatR,
           ILogger<TPM_CodeKbService> logger,
           IMemoryCache memoryCache,
           ITPM_CodeSyService codeSyuService)
        {
            _context = context;
            _mediatR = mediatR;
            _logger = logger;
            MemoryCache = memoryCache;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="motoTenantCdSeq"></param>
        /// <param name="motoUkeNo"></param>
        /// <param name="motoUnkRen"></param>
        /// <param name="motoYouTblSeq"></param>
        /// <returns></returns>
        public bool CheckDataYoushaNotice(int motoTenantCdSeq,string motoUkeNo,short motoUnkRen,int motoYouTblSeq)
        {
            bool checkdata = false;
            if(_context.TkdYoushaNotice.Where(t=>t.MotoTenantCdSeq==motoTenantCdSeq&&t.MotoUkeNo==motoUkeNo&&t.MotoUnkRen==motoUnkRen&&t.MotoYouTblSeq==motoYouTblSeq).ToList().Count()>0)
            {
                checkdata = true;
            }    
            return checkdata;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UkeNo"></param>
        /// <param name="UnkRen"></param>
        /// <param name="YouTblSeq"></param>
        /// <returns></returns>
         public async Task<CarCooperationData> Getdata(string UkeNo, short UnkRen, int YouTblSeq)
        {
            return await _mediatR.Send(new GetCarCooperation(UkeNo, UnkRen,YouTblSeq));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_tenantCdSeq"></param>
        /// <returns></returns>
        public async Task<List<YoushaNoticeData>> GetYoushaNoticeData(int _tenantCdSeq)
        {
            return await _mediatR.Send(new GetDataYoushaNoticeQuery(_tenantCdSeq));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MotoTenantCdSeq"></param>
        /// <param name="MotoUkeNo"></param>
        /// <param name="MotoUnkRen"></param>
        /// <param name="MotoYouTblSeq"></param>
        public void UpdateUnReadKbnYoushaNotice(int MotoTenantCdSeq, string MotoUkeNo, short MotoUnkRen, int MotoYouTblSeq)
        {
            var updateUnReadKbnYoushaNotice = _context.TkdYoushaNotice.Find(MotoTenantCdSeq, MotoUkeNo,MotoUnkRen,MotoYouTblSeq);
            updateUnReadKbnYoushaNotice.UnReadKbn = 2;
            _context.TkdYoushaNotice.Update(updateUnReadKbnYoushaNotice);
            _context.SaveChanges();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MotoTenantCdSeq"></param>
        /// <param name="MotoUkeNo"></param>
        /// <param name="MotoUnkRen"></param>
        /// <param name="MotoYouTblSeq"></param>
        public void UpdateYoushaNotice(int MotoTenantCdSeq, string MotoUkeNo, short MotoUnkRen, int MotoYouTblSeq)
        {
            var updateUnReadKbnYoushaNotice = _context.TkdYoushaNotice.Find(MotoTenantCdSeq, MotoUkeNo, MotoUnkRen, MotoYouTblSeq);
            updateUnReadKbnYoushaNotice.UnReadKbn = 2;
            updateUnReadKbnYoushaNotice.RegiterKbn = 2;
            _context.TkdYoushaNotice.Update(updateUnReadKbnYoushaNotice);
            _context.SaveChanges();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="motoTenantCdSeq"></param>
        /// <param name="motoUkeNo"></param>
        /// <param name="motoUnkRen"></param>
        /// <param name="motoYouTblSeq"></param>
        /// <returns></returns>
        public async Task<YoushaNoticeData> GetItemYoushaNotice(int motoTenantCdSeq,string motoUkeNo,short motoUnkRen,int motoYouTblSeq)
            {
            return await _mediatR.Send(new GetItemYoushaNoticeQuery(motoTenantCdSeq,motoUkeNo,motoUnkRen,motoYouTblSeq));
        }
    }
}
