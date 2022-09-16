using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.VehicleStatisticsSurvey.Queries
{
    public class GetListVehicleStatisticsSurvey : IRequest<List<VehicleStatisticsSurveyData>>
    {
        public VehicleStatisticsSurveySearchParam searchParam { get; set; }
        public class Hanlder : IRequestHandler<GetListVehicleStatisticsSurvey, List<VehicleStatisticsSurveyData>>
        {
            private readonly KobodbContext _context;
            public Hanlder(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<VehicleStatisticsSurveyData>> Handle(GetListVehicleStatisticsSurvey request, CancellationToken cancellationToken)
            {
                var searchParam = request.searchParam;
                List<VehicleStatisticsSurveyData> result = new List<VehicleStatisticsSurveyData>();

                var connection = _context.Database.GetDbConnection();
                SqlCommand command = new SqlCommand();
                command.Connection = (SqlConnection)connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PK_dVehicleStatisticsSurveyReports_R";

                command.Parameters.AddWithValue("@Date", searchParam.ProcessingDate.ToString(CommonConstants.FormatYM));
                var endOfMonth = new DateTime(searchParam.ProcessingDate.Year, searchParam.ProcessingDate.Month, DateTime.DaysInMonth(searchParam.ProcessingDate.Year, searchParam.ProcessingDate.Month));
                command.Parameters.AddWithValue("@EndOfMonth", endOfMonth.ToString(CommonConstants.FormatYMD));
                var lastMonth = new DateTime(searchParam.ProcessingDate.Year, searchParam.ProcessingDate.Month, 1).AddDays(-1);
                command.Parameters.AddWithValue("@LastMonth", lastMonth.ToString(CommonConstants.FormatYM));
                command.Parameters.AddWithValue("@CompnyCd", searchParam.Company?.CompanyCd ?? 0);
                command.Parameters.AddWithValue("@StrEigyoCd", searchParam.EigyoFrom?.EigyoCd ?? 0);
                command.Parameters.AddWithValue("@EndEigyoCd", searchParam.EigyoTo?.EigyoCd ?? 0);
                command.Parameters.AddWithValue("@StrUnsouKbn", searchParam.ShippingFrom?.CodeKbn ?? 0);
                command.Parameters.AddWithValue("@EndUnsouKbn", searchParam.ShippingTo?.CodeKbn ?? 0);
                command.Parameters.AddWithValue("@TenantCdSeq", searchParam.TenantCdSeq);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                result = MapTableToObjectHelper.ConvertDataTable<VehicleStatisticsSurveyData>(dt);

                await command.Connection.CloseAsync();
                adapter.Dispose();

                return result;
            }
        }
    }
}
