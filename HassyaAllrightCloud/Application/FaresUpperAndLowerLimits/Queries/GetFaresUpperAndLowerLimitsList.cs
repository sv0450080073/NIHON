using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FaresUpperAndLowerLimits.Queries
{
    public class GetFaresUpperAndLowerLimitsList : IRequest<List<FaresUpperAndLowerLimitObject>>
    {
        public FaresUpperAndLowerLimitsFormSearch SearchParam { get; set; }
        public class Handler : IRequestHandler<GetFaresUpperAndLowerLimitsList, List<FaresUpperAndLowerLimitObject>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<FaresUpperAndLowerLimitObject>> Handle(GetFaresUpperAndLowerLimitsList request, CancellationToken cancellationToken)
            {
                try
                {
                    var outputStartDate = request?.SearchParam?.OutputStartDate?.ToString(Formats.yyyyMMdd);
                    var outputEndDate = request?.SearchParam?.OutputEndDate?.ToString(Formats.yyyyMMdd);
                    var dateClassification = (int)request?.SearchParam?.DateClassification;

                    var rows = new List<FaresUpperAndLowerLimitObject>();
                    _context.LoadStoredProc("dbo.PK_FaresUpperAndLowerLimit")
                             .AddParam("@TenantCdSeq", new ClaimModel().TenantID.ToString())
                             .AddParam("@BackToGarageYmdStr", dateClassification == (int)DateClassification.BackToGarageDate ? outputStartDate : null)
                             .AddParam("@BackToGarageYmdEnd", dateClassification == (int)DateClassification.BackToGarageDate ? outputEndDate : null)
                             .AddParam("@DispatchYmdStr", dateClassification == (int)DateClassification.DipatchDate ? outputStartDate : null)
                             .AddParam("@DispatchYmdEnd", dateClassification == (int)DateClassification.DipatchDate ? outputEndDate : null)
                             .AddParam("@ArrivalYmdStr", dateClassification == (int)DateClassification.ArrivalDate ? outputStartDate : null)
                             .AddParam("@ArrivalYmdEnd", dateClassification == (int)DateClassification.ArrivalDate ? outputEndDate : null)
                             .AddParam("@EigyoCd", request?.SearchParam?.SaleOffice?.EigyoCd ?? null)
                             .AddParam("@EigSyainCd", request?.SearchParam?.SalePersonInCharge?.SyainCd ?? null)
                             .AddParam("@UkeNoStr", !string.IsNullOrEmpty(request?.SearchParam?.ReservationNumberStart) ? $"{new ClaimModel().TenantID.ToString("D5")}{Convert.ToInt64(request?.SearchParam?.ReservationNumberStart).ToString("D10")}" : null)
                             .AddParam("@UkeNoEnd", !string.IsNullOrEmpty(request?.SearchParam?.ReservationNumberEnd) ? $"{new ClaimModel().TenantID.ToString("D5")}{Convert.ToInt64(request?.SearchParam?.ReservationNumberEnd).ToString("D10")}" : null)
                             .AddParam("@Classification11", (int)request?.SearchParam?.Range == 1 && (int)request?.SearchParam?.ItemOutOfRange == 1 ? "11" : null)
                             .AddParam("@Classification12", (int)request?.SearchParam?.Range == 1 && (int)request?.SearchParam?.ItemOutOfRange == 2 ? "12" : null)
                             .AddParam("@Classification13", (int)request?.SearchParam?.Range == 1 && (int)request?.SearchParam?.ItemOutOfRange == 3 ? "13" : null)
                             .AddParam("@InOrOutPlan", request?.SearchParam?.Range == 2 ? request?.SearchParam?.Range.ToString() : null)
                             .AddParam("@HaveCauseOrNot", request?.SearchParam?.CauseInput == 2 ? request?.SearchParam?.CauseInput.ToString() : null)
                             .AddParam("@HaveCauseClassification11", (int)request?.SearchParam?.ChooseCause == 1 && request?.SearchParam?.CauseInput == 1 ? "11" : null)
                             .AddParam("@CauseClassification2", (int)request?.SearchParam?.ChooseCause == 2 ? "22" : null)
                             .AddParam("@CauseClassification3", (int)request?.SearchParam?.ChooseCause == 3 ? "33" : null)
                             .AddParam("@ROWCOUNT", out IOutParam<int> retParam)
                         .Exec(r => rows = r.ToList<FaresUpperAndLowerLimitObject>());
                    return rows;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
