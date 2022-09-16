using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.HikiukeshoReport
{
    public class HikiukeshoHelper
    {
        public IStoredProcBuilder CreateStoredBuilder(string storedProcedureName, KobodbContext context, TransportationContractFormData transportationContract)
        {
            var storedBuilder = context.LoadStoredProc(storedProcedureName);
            storedBuilder.AddParam("@TenantCdSeq", new ClaimModel().TenantID);

            if (transportationContract.DateTypeContract == (int)DateTypeContract.Dispatch)
            {
                storedBuilder.AddParam("@StartDispatchDate", transportationContract.DateFrom?.ToString(Commons.DateTimeFormat.yyyyMMdd));
                storedBuilder.AddParam("@EndDispatchDate", transportationContract.DateTo?.ToString(Commons.DateTimeFormat.yyyyMMdd));
                storedBuilder.AddParam("@StartArrivalDate", "");
                storedBuilder.AddParam("@EndArrivalDate", "");
                storedBuilder.AddParam("@StartReservationDate", "");
                storedBuilder.AddParam("@EndReservationDate", "");
            }
            else if (transportationContract.DateTypeContract == (int)DateTypeContract.Arrival)
            {
                storedBuilder.AddParam("@StartArrivalDate", transportationContract.DateFrom?.ToString(Commons.DateTimeFormat.yyyyMMdd));
                storedBuilder.AddParam("@EndArrivalDate", transportationContract.DateTo?.ToString(Commons.DateTimeFormat.yyyyMMdd));
                storedBuilder.AddParam("@StartDispatchDate", "");
                storedBuilder.AddParam("@EndDispatchDate", "");
                storedBuilder.AddParam("@StartReservationDate", "");
                storedBuilder.AddParam("@EndReservationDate", "");
            }
            else
            {
                storedBuilder.AddParam("@StartReservationDate", transportationContract.DateFrom?.ToString(Commons.DateTimeFormat.yyyyMMdd));
                storedBuilder.AddParam("@EndReservationDate", transportationContract.DateTo?.ToString(Commons.DateTimeFormat.yyyyMMdd));
                storedBuilder.AddParam("@StartArrivalDate", "");
                storedBuilder.AddParam("@EndArrivalDate", "");
                storedBuilder.AddParam("@StartDispatchDate", "");
                storedBuilder.AddParam("@EndDispatchDate", "");
            }
            storedBuilder.AddParam("@YoyaKbnList", transportationContract.YoyakuKbnList != null ? String.Join(',', transportationContract.YoyakuKbnList.Select(x => x.YoyaKbn)) : "")
                .AddParam("@UkeEigCd", transportationContract.UketsukeEigyoJo?.EigyoCd)
                .AddParam("@EigSyainCd", transportationContract.EigyoTantoSha?.SyainCd)
                .AddParam("@InpSyainCd", transportationContract.InpSyainCd?.SyainCd)
                .AddParam("@GyosyaCd", transportationContract.Gyosya?.GyosyaCd)
                .AddParam("@TokuiCd", transportationContract.TokuiSaki?.TokuiCd)
                .AddParam("@SitenCd", transportationContract.TokuiSiten?.SitenCd)
                .AddParam("@UkeNo", (String.IsNullOrEmpty(transportationContract.UkeNumber) ? "" : transportationContract.UkeNumber))
                .AddParam("@UnkRen", (string.IsNullOrEmpty(transportationContract.UnkRen) ? "" : transportationContract.UnkRen))
                .AddParam("@OutSelect", transportationContract.OutputSelection)
                .AddParam("@NenKeiyakuOutFlg", transportationContract.YearlyContract)
                .AddParam("@OutputUnit", transportationContract.OutputUnit);
            return storedBuilder;
        }
    }
}
