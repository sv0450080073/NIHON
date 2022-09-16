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

namespace HassyaAllrightCloud.Application.VehicleDailyReport.Queries
{
    public class GetListSyaRyoForSearchQuery : IRequest<List<CurrentBus>>
    {
        public VehicleDailyReportSearchParam searchParams { get; set; }
        public class Handler : IRequestHandler<GetListSyaRyoForSearchQuery, List<CurrentBus>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            /// <summary>
            /// Get list bus for search
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<List<CurrentBus>> Handle(GetListSyaRyoForSearchQuery request, CancellationToken cancellationToken)
            {
                var searchParam = request.searchParams;
                List<CurrentBus> result = new List<CurrentBus>();

                var connection = _context.Database.GetDbConnection();
                SqlCommand command = new SqlCommand();
                command.Connection = (SqlConnection)connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PK_dBuses_R";

                command.Parameters.AddWithValue("@NipoYmdStr", searchParam.ScheduleYmdStart == null ? string.Empty : searchParam.ScheduleYmdStart.Value.ToString("yyyyMMdd"));
                command.Parameters.AddWithValue("@NipoYmdEnd", searchParam.ScheduleYmdEnd == null ? string.Empty : searchParam.ScheduleYmdEnd.Value.ToString("yyyyMMdd"));
                command.Parameters.AddWithValue("@EigyoCompnyCd", searchParam.CompanyCd);
                command.Parameters.AddWithValue("@SyaEigyoCdStr", searchParam.selectedBusSaleStart?.EigyoCd ?? 0);
                command.Parameters.AddWithValue("@SyaEigyoCdEnd", searchParam.selectedBusSaleEnd?.EigyoCd ?? 0);
                command.Parameters.AddWithValue("@SyaRyoCdStr", searchParam.selectedBusCodeStart?.SyaRyoCd ?? 0);
                command.Parameters.AddWithValue("@SyaRyoCdEnd", searchParam.selectedBusCodeEnd?.SyaRyoCd ?? 0);
                command.Parameters.AddWithValue("@UkeNoStr", searchParam.ReceptionStart);
                command.Parameters.AddWithValue("@UkeNoEnd", searchParam.ReceptionEnd);
                command.Parameters.AddWithValue("@YoyaKbnStr", searchParam.selectedReservationStart?.YoyaKbn ?? 0);
                command.Parameters.AddWithValue("@YoyaKbnEnd", searchParam.selectedReservationEnd?.YoyaKbn ?? 0);
                command.Parameters.AddWithValue("@TenantCdSeq", searchParam.TenantCdSeq);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                await command.Connection.CloseAsync();

                result = MapTableToObjectHelper.ConvertDataTable<CurrentBus>(dt);

                return result;
            }
        }
    }
}
