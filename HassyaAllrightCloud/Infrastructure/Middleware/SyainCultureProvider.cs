using HassyaAllrightCloud.Application.SyainCulture.Queries;
using HassyaAllrightCloud.Commons.Constants;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;

namespace HassyaAllrightCloud.Infrastructure.Middleware
{
    public class SyainCultureProvider : RequestCultureProvider
    {
        private IMediator _mediator;
        private IMemoryCache MemoryCache { get; set; }
        public SyainCultureProvider()
        {
        }
        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            MemoryCache = httpContext.RequestServices.GetService<IMemoryCache>();
            return await MemoryCache.GetOrCreateAsync($"{new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq}", async e =>
             {
                 e.SetOptions(new MemoryCacheEntryOptions
                 {
                     AbsoluteExpirationRelativeToNow =
                     TimeSpan.FromSeconds(10)
                 });
                 var claim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                 _mediator = httpContext.RequestServices.GetService<IMediator>();
                // TODO when SSO is applied => Get SyainCdSeq from httpContext.User.Claims... values
                string culture = await _mediator.Send(new GetSyainCultureQuery(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq));

                 if (httpContext == null)
                     throw new ArgumentNullException();
                 return new ProviderCultureResult(culture);
             });
        }
    }
}
