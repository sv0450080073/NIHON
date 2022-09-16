using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Application.VPMTenantData.Queries;
using MediatR;

namespace HassyaAllrightCloud.IService
{
    public interface ITPM_TenantDataService
    {
        Task<List<VpmTenant>> GetVpmTenants();
    }
    public class TPM_TenantDataService : ITPM_TenantDataService
    {
        private IMediator mediatR;
        public TPM_TenantDataService(IMediator mediatR)
        {
            this.mediatR = mediatR;
        }
        public async Task<List<VpmTenant>> GetVpmTenants()
        {
            return await mediatR.Send(new GetVpmTenantData());
        }
    }
}
