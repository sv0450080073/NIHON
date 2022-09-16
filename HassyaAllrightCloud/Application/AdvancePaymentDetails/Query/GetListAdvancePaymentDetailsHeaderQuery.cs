using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.AdvancePaymentDetails.Query
{
    public class GetListAdvancePaymentDetailsHeaderQuery : IRequest<List<AdvancePaymentDetailsModel>>
    {
        public AdvancePaymentDetailsSearchParam SearchParams { get; set; }

        public class Handlers : IRequestHandler<GetListAdvancePaymentDetailsHeaderQuery, List<AdvancePaymentDetailsModel>>
        {
            private readonly KobodbContext _context;
            public Handlers(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<AdvancePaymentDetailsModel>> Handle(GetListAdvancePaymentDetailsHeaderQuery request, CancellationToken cancellationToken)
            {
                List<AdvancePaymentDetailsModel> listAdvancePaymentDetailsHeader = new List<AdvancePaymentDetailsModel>();
                var storedBuilder = _context.LoadStoredProc("dbo.PK_dAdvancePaymentDetailHeaderList_R");

                storedBuilder.AddParam("@TenantCdSeq", request.SearchParams.TenantCdSeq);

                storedBuilder.AddParam("@ReceiptNumber", request.SearchParams.ReceptionNumber ?? string.Empty);

                storedBuilder.AddParam("@DispatchDate", request.SearchParams.ScheduleYmdStart?.ToString(Commons.DateTimeFormat.yyyyMMdd));
                storedBuilder.AddParam("@ArrivalDate", request.SearchParams.ScheduleYmdEnd?.ToString(Commons.DateTimeFormat.yyyyMMdd));

                if (request.SearchParams.AddressSpectify?.Value == 1)
                {
                    //storedBuilder.AddParam("@StartBillAddress", request.SearchParams.StartAddress?.GyosyaCd.ToString().PadLeft(3, '0') +
                    //    request.SearchParams.StartAddress?.TokuiCd.ToString().PadLeft(4, '0') + request.SearchParams.StartAddress?.SitenCd.ToString().PadLeft(4, '0'));
                    //storedBuilder.AddParam("@EndBillAddress", request.SearchParams.EndAddress?.GyosyaCd.ToString().PadLeft(3, '0') +
                    //    request.SearchParams.EndAddress?.TokuiCd.ToString().PadLeft(4, '0') + request.SearchParams.EndAddress?.SitenCd.ToString().PadLeft(4, '0'));
                    storedBuilder.AddParam("@StartCustomer", string.Empty);
                    storedBuilder.AddParam("@EndCustomer", string.Empty);
                    storedBuilder.AddParam("@GyosyaFrom", request.SearchParams?.CustomerModelFrom?.SelectedGyosya?.GyosyaCd ?? 0);
                    storedBuilder.AddParam("@GyosyaTo", request.SearchParams?.CustomerModelTo?.SelectedGyosya?.GyosyaCd ?? 999);
                    storedBuilder.AddParam("@TokuiFrom", request.SearchParams?.CustomerModelFrom?.SelectedTokisk?.TokuiCd ?? 0);
                    storedBuilder.AddParam("@TokuiTo", request.SearchParams?.CustomerModelTo?.SelectedTokisk?.TokuiCd ?? 9999);
                    storedBuilder.AddParam("@SitenFrom", request.SearchParams?.CustomerModelFrom?.SelectedTokiSt?.SitenCd ?? 0);
                    storedBuilder.AddParam("@SitenTo", request.SearchParams?.CustomerModelTo?.SelectedTokiSt?.SitenCd ?? 9999);
                }
                else if (request.SearchParams.AddressSpectify?.Value == 2)
                {
                    storedBuilder.AddParam("@StartCustomer", request.SearchParams.StartAddress?.GyosyaCd.ToString().PadLeft(3, '0') +
                        request.SearchParams.StartAddress?.TokuiCd.ToString().PadLeft(4, '0') + request.SearchParams.StartAddress?.SitenCd.ToString().PadLeft(4, '0'));
                    storedBuilder.AddParam("@EndCustomer", request.SearchParams.EndAddress?.GyosyaCd.ToString().PadLeft(3, '0') +
                        request.SearchParams.EndAddress?.TokuiCd.ToString().PadLeft(4, '0') + request.SearchParams.EndAddress?.SitenCd.ToString().PadLeft(4, '0'));
                    //storedBuilder.AddParam("@StartBillAddress", string.Empty);
                    //storedBuilder.AddParam("@EndBillAddress", string.Empty);
                    storedBuilder.AddParam("@GyosyaFrom", 0);
                    storedBuilder.AddParam("@GyosyaTo", 0);
                    storedBuilder.AddParam("@TokuiFrom", 0);
                    storedBuilder.AddParam("@TokuiTo", 0);
                    storedBuilder.AddParam("@SitenFrom", 0);
                    storedBuilder.AddParam("@SitenTo", 0);
                }
                else
                {
                    storedBuilder.AddParam("@StartCustomer", string.Empty);
                    storedBuilder.AddParam("@EndCustomer", string.Empty);
                    //storedBuilder.AddParam("@StartBillAddress", string.Empty);
                    //storedBuilder.AddParam("@EndBillAddress", string.Empty);
                    storedBuilder.AddParam("@GyosyaFrom", 0);
                    storedBuilder.AddParam("@GyosyaTo", 0);
                    storedBuilder.AddParam("@TokuiFrom", 0);
                    storedBuilder.AddParam("@TokuiTo", 0);
                    storedBuilder.AddParam("@SitenFrom", 0);
                    storedBuilder.AddParam("@SitenTo", 0);
                }
                await storedBuilder.ExecAsync(async rows => listAdvancePaymentDetailsHeader = await rows.ToListAsync<AdvancePaymentDetailsModel>());
                return listAdvancePaymentDetailsHeader;
            }
        }
    }
}
