using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public class BlazorServerAuthState
    : RevalidatingServerAuthenticationStateProvider
    {
        private readonly BlazorServerAuthStateCache Cache;

        public BlazorServerAuthState(
            ILoggerFactory loggerFactory,
            BlazorServerAuthStateCache cache)
            : base(loggerFactory)
        {
            Cache = cache;
        }

        protected override TimeSpan RevalidationInterval => TimeSpan.FromSeconds(10);

        protected override Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            var sid = authenticationState.User.Claims
                        .Where(c => c.Type.Equals("sid"))
                        .Select(c => c.Value)
                        .FirstOrDefault();

            if (sid != null && Cache.HasSubjectId(sid))
            {
                var data = Cache.Get(sid);
                if (DateTimeOffset.UtcNow >= data.Expiration)
                {
                    Cache.Remove(sid);
                    return Task.FromResult(false);
                }
            }
            return Task.FromResult(true);
        }
    }
}
