using HassyaAllrightCloud.Commons.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ClaimModel
    {
        public int TenantID { get; set; }
        public int CompanyID { get; set; }
        public int EigyoCdSeq { get; set; }
        public int SyainCdSeq { get; set; }
        public string SyainCd { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        private IEnumerable<Claim> claims;

        public ClaimModel(HttpContext httpContext)
        {
            claims = httpContext?.User?.Claims;
            AssignClaims();
            Name = httpContext?.User?.Claims.FirstOrDefault(x => x.Type.ToLower() == "name")?.Value;
        }

        public ClaimModel()
        {
            var serviceScope = ServiceActivator.GetScope();
            IHttpContextAccessor httpContextAccessor = (IHttpContextAccessor)serviceScope.ServiceProvider.GetService(typeof(IHttpContextAccessor));
            claims = httpContextAccessor?.HttpContext?.User?.Claims;

            AssignClaims();
            Name = httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type.ToLower() == "name")?.Value;
        }

        private void AssignClaims()
        {
            if (claims != null && claims.Any())
            {
                var tenantCdSeqParseResult = int.TryParse(claims.FirstOrDefault(x => x.Type == "TenantCdSeq")?.Value, out int tenantCdSeq);
                var syainCdSeqParseResult = int.TryParse(claims.FirstOrDefault(x => x.Type == "SyainCdSeq")?.Value, out int syainCdSeq);
                var companyIdParseResult = int.TryParse(claims.FirstOrDefault(x => x.Type == "CompanyId")?.Value, out int companyId);
                var eigyoCdSeqParseResult = int.TryParse(claims.FirstOrDefault(x => x.Type == "EigyoCdSeq")?.Value, out int eigyoCdSeq);

                TenantID = !tenantCdSeqParseResult ? 0 : tenantCdSeq;
                CompanyID = !companyIdParseResult ? 0 : companyId;
                EigyoCdSeq = !eigyoCdSeqParseResult ? 0 : eigyoCdSeq;
                SyainCdSeq = !syainCdSeqParseResult ? 0 : syainCdSeq;
                SyainCd = claims.FirstOrDefault(x => x.Type == "SyainCd")?.Value;
                UserName = claims.FirstOrDefault(x => x.Type == "UserName")?.Value;
                // Name = claims.FirstOrDefault(x => x.Type == "Name")?.Value;
            }

        }
    }
}
