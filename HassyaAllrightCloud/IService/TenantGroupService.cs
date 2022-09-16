using HassyaAllrightCloud.Application.CarCooperation.Queries;
using HassyaAllrightCloud.Application.TenantGroup.Queries;
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
    public interface ITenantGroupServiceService
    {
        Task<TenantGroupData> GetdataReception(int TenantCdSeq,int TokuiSeq,int SitenCdSeq);
        Task<TenantGroupData> GetdataReceptionCustomer(int TenantCdSeq,int TenantGroupCdSeq,int SitenCdSeqTenantCdSeq);
        Task<string> GetTenantName(int TenantCdSeq);
        Task<ClientData> GetClienData(int TenantCdSeq, int TokuiSeq, int SitenCdSeq);
        Task<int> GetUnreadNotifications(int TenantCdSeq);
    }
    public class TenantGroupService: ITenantGroupServiceService
    {

        private readonly KobodbContext _context;
        private readonly IMediator _mediatR;
        private readonly ILogger<TPM_CodeKbService> _logger;
        public IMemoryCache MemoryCache { get; }
        public TenantGroupService(KobodbContext context,
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
        /// <param name="TenantCdSeq"></param>
        /// <param name="TokuiSeq"></param>
        /// <param name="SitenCdSeq"></param>
        /// <returns></returns>
        public async Task<TenantGroupData> GetdataReception(int TenantCdSeq,int TokuiSeq,int SitenCdSeq)
        {
            return await _mediatR.Send(new GetReceptionTenantQuery(TenantCdSeq, TokuiSeq, SitenCdSeq));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TenantCdSeq"></param>
        /// <param name="TenantGroupCdSeq"></param>
        /// <param name="SitenCdSeqTenantCdSeq"></param>
        /// <returns></returns>
        public async Task<TenantGroupData> GetdataReceptionCustomer(int TenantCdSeq,int TenantGroupCdSeq,int SitenCdSeqTenantCdSeq)
        {
            return await _mediatR.Send(new GetReceptionCustomerTenantQuery(TenantCdSeq, TenantGroupCdSeq, SitenCdSeqTenantCdSeq));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TenantCdSeq"></param>
        /// <returns></returns>
        public async Task<string> GetTenantName(int TenantCdSeq)
        {
            return await _mediatR.Send(new GetTenantNameQuery(TenantCdSeq));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TenantCdSeq"></param>
        /// <param name="TokuiSeq"></param>
        /// <param name="SitenCdSeq"></param>
        /// <returns></returns>
        public async Task<ClientData> GetClienData(int TenantCdSeq, int TokuiSeq, int SitenCdSeq)
        {
            return await _mediatR.Send(new GetClientDataQuery(TenantCdSeq,TokuiSeq,SitenCdSeq));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TenantCdSeq"></param>
        /// <returns></returns>
        public async Task<int> GetUnreadNotifications(int TenantCdSeq)
        {
            return await _mediatR.Send(new GetUnreadNotificationsQuery(TenantCdSeq));
        }

    }
}
