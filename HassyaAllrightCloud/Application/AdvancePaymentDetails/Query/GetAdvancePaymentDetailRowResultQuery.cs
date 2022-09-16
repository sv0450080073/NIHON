using DevExpress.Charts.Native;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.AdvancePaymentDetails.Query
{
    public class GetAdvancePaymentDetailRowResultQuery : IRequest<int>
    {
        public AdvancePaymentDetailsSearchParam SearchParams { get; set; }
        public class Handler : IRequestHandler<GetAdvancePaymentDetailRowResultQuery, int>
        {
            private readonly KobodbContext context;
            public Handler(KobodbContext _context)
            {
                this.context = _context;
            }

            public async Task<int> Handle(GetAdvancePaymentDetailRowResultQuery request, CancellationToken cancellationToken)
            {
                DbParameter outputParam = null;
                var storedBuilder = context.LoadStoredProc("dbo.PK_dAdvancePaymentDetailResultsCount_R");
                storedBuilder.AddParam("@TenantCdSeq", request.SearchParams.TenantCdSeq);

                storedBuilder.AddParam("@ReceiptNumber", request.SearchParams.ReceptionNumber ?? string.Empty);
                
                storedBuilder.AddParam("@DispatchDate", request.SearchParams.ScheduleYmdStart?.ToString(Commons.DateTimeFormat.yyyyMMdd));
                storedBuilder.AddParam("@ArrivalDate", request.SearchParams.ScheduleYmdEnd?.ToString(Commons.DateTimeFormat.yyyyMMdd));
                
                if (request.SearchParams.AddressSpectify?.Value == 1)
                {
                    storedBuilder.AddParam("@StartBillAddress", request.SearchParams.StartAddress?.GyosyaCd.ToString().PadLeft(3, '0') + 
                        request.SearchParams.StartAddress?.TokuiCd.ToString().PadLeft(4, '0') + request.SearchParams.StartAddress?.SitenCd.ToString().PadLeft(4, '0'));
                    storedBuilder.AddParam("@EndBillAddress", request.SearchParams.EndAddress?.GyosyaCd.ToString().PadLeft(3, '0') +
                        request.SearchParams.EndAddress?.TokuiCd.ToString().PadLeft(4, '0') + request.SearchParams.EndAddress?.SitenCd.ToString().PadLeft(4, '0'));
                    storedBuilder.AddParam("@StartCustomer", string.Empty);
                    storedBuilder.AddParam("@EndCustomer", string.Empty);
                }
                else if (request.SearchParams.AddressSpectify?.Value == 2)
                {
                    storedBuilder.AddParam("@StartCustomer", request.SearchParams.StartAddress?.GyosyaCd.ToString().PadLeft(3, '0') +
                        request.SearchParams.StartAddress?.TokuiCd.ToString().PadLeft(4, '0') + request.SearchParams.StartAddress?.SitenCd.ToString().PadLeft(4, '0'));
                    storedBuilder.AddParam("@EndCustomer", request.SearchParams.EndAddress?.GyosyaCd.ToString().PadLeft(3, '0') +
                        request.SearchParams.EndAddress?.TokuiCd.ToString().PadLeft(4, '0') + request.SearchParams.EndAddress?.SitenCd.ToString().PadLeft(4, '0'));
                    storedBuilder.AddParam("@StartBillAddress", string.Empty);
                    storedBuilder.AddParam("@EndBillAddress", string.Empty);
                }
                else
                {
                    storedBuilder.AddParam("@StartCustomer", string.Empty);
                    storedBuilder.AddParam("@EndCustomer", string.Empty);
                    storedBuilder.AddParam("@StartBillAddress", string.Empty);
                    storedBuilder.AddParam("@EndBillAddress", string.Empty);
                }

                storedBuilder.ExecScalar<int>(out int Count);

                return Count;
            }
        }
    }
}
