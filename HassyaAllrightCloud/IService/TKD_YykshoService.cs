using System.Threading.Tasks;
using System;
using MediatR;
using Microsoft.Extensions.Logging;
using HassyaAllrightCloud.Application.Yyksho.Queries;
using System.Text;

namespace HassyaAllrightCloud.IService
{
    public interface ITKD_YykshoListService
    {
        Task<(int, string)> GetNewUkeNo(int tenantId);
        Task<(int, string[])> GetNewUkeNo(int tenantId, int numberOfBookingCopy);
    }
    public class TKD_YykshoService : ITKD_YykshoListService
    {
        private readonly IMediator _mediatR;
        private readonly ILogger<TKD_YykshoService> _logger;

        public TKD_YykshoService(IMediator mediatR, ILogger<TKD_YykshoService> logger)
        {
            _mediatR = mediatR;
            _logger = logger;
        }

        /// <summary>
        /// Get Max UkeCd of provided tenant and new UkeNo
        /// </summary>
        /// <param name="tenantId">TenantCdSeq</param>
        /// <returns>(MaxUkeCd: int, UkeNo: string)</returns>
        public async Task<(int, string)> GetNewUkeNo(int tenantId)
        {
            try
            {
                int maxUkeCd = await _mediatR.Send(new GetMaxUkeCdQuery(tenantId));
                StringBuilder stringBuilder = new StringBuilder()
                    .Append(tenantId.ToString("D5"))
                    .Append((maxUkeCd + 1).ToString("D10"));
                return (maxUkeCd, stringBuilder.ToString());
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get Max UkeCd of provided tenant and list of new UkeNo
        /// </summary>
        /// <param name="tenantId">TenantCdSeq</param>
        /// <param name="numberOfBookingCopy">Total new booking to create</param>
        /// <returns>(MaxUkeCd: int, ListUkeNo: string[])</returns>
        public async Task<(int, string[])> GetNewUkeNo(int tenantId, int numberOfBookingCopy)
        {
            try
            {
                string[] ukeNoListResult = new string[numberOfBookingCopy];
                int maxUkeCd = await _mediatR.Send(new GetMaxUkeCdQuery(tenantId));

                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < numberOfBookingCopy; i++)
                {
                    stringBuilder
                        .Clear()
                        .Append(tenantId.ToString("D5"))
                        .Append((maxUkeCd + i + 1).ToString("D10"));
                    ukeNoListResult[i] = stringBuilder.ToString();
                }

                return (maxUkeCd, ukeNoListResult);
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
