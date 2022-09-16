using DevExpress.XtraCharts.Native;
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
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.VehicleDailyReport.Queries
{
    public class GetListVehicleDailyReportQuery : IRequest<List<VehicleDailyReportModel>>
    {
        public VehicleDailyReportSearchParam searchParams { get; set; }
        public class Handler : IRequestHandler<GetListVehicleDailyReportQuery, List<VehicleDailyReportModel>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            /// <summary>
            /// Get list vehicle daily report
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<List<VehicleDailyReportModel>> Handle(GetListVehicleDailyReportQuery request, CancellationToken cancellationToken)
            {
                var searchParam = request.searchParams;
                if(searchParam.OutputKbn.Value == 0)
                {
                    searchParam.StrYmd = "19800101";
                    searchParam.EndYmd = "20791231";
                }
                else
                {
                    if (!string.IsNullOrEmpty(searchParam.selectedUnkYmd))
                    {
                        searchParam.StrYmd = searchParam.selectedUnkYmd.Replace("/", "");
                        searchParam.EndYmd = searchParam.selectedUnkYmd.Replace("/", "");
                    }
                }

                if(searchParam.OutputSetting == 1 || searchParam.OutputSetting == 2 || searchParam.OutputSetting == 3 || searchParam.OutputSetting == 4)
                {
                    searchParam.StrYmd = string.Empty;
                    searchParam.EndYmd = string.Empty;
                }

                List<VehicleDailyReportModel> result = new List<VehicleDailyReportModel>();

                var connection = _context.Database.GetDbConnection();
                SqlCommand command = new SqlCommand();
                command.Connection = (SqlConnection)connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PK_dVehicleDailyReports_R";

                command.Parameters.AddWithValue("@NipoYmdStr", searchParam.ScheduleYmdStart == null ? string.Empty : searchParam.ScheduleYmdStart.Value.ToString("yyyyMMdd"));
                command.Parameters.AddWithValue("@NipoYmdEnd", searchParam.ScheduleYmdEnd == null ? string.Empty : searchParam.ScheduleYmdEnd.Value.ToString("yyyyMMdd"));
                command.Parameters.AddWithValue("@EigyoCompnyCd", searchParam.CompanyCdSeq);
                command.Parameters.AddWithValue("@SyaEigyoCdStr", searchParam.selectedBusSaleStart?.EigyoCd ?? 0);
                command.Parameters.AddWithValue("@SyaEigyoCdEnd", searchParam.selectedBusSaleEnd?.EigyoCd ?? 0);
                command.Parameters.AddWithValue("@SyaRyoCdStr", searchParam.selectedBusCodeStart?.SyaRyoCd ?? 0);
                command.Parameters.AddWithValue("@SyaRyoCdEnd", searchParam.selectedBusCodeEnd?.SyaRyoCd ?? 0);
                command.Parameters.AddWithValue("@UkeNoStr", searchParam.ReceptionStart);
                command.Parameters.AddWithValue("@UkeNoEnd", searchParam.ReceptionEnd);
                command.Parameters.AddWithValue("@YoyaKbnStr", searchParam.selectedReservationStart?.YoyaKbn ?? 0);
                command.Parameters.AddWithValue("@YoyaKbnEnd", searchParam.selectedReservationEnd?.YoyaKbn ?? 0);
                command.Parameters.AddWithValue("@SyaRyoCdSeq", searchParam.SyaRyoCdSeq);
                command.Parameters.AddWithValue("@YmdStr", searchParam.StrYmd);
                command.Parameters.AddWithValue("@YmdEnd", searchParam.EndYmd);
                command.Parameters.AddWithValue("@TenantCdSeq", searchParam.TenantCdSeq);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                result = MapTableToObjectHelper.ConvertDataTable<VehicleDailyReportModel>(dt);

                await command.Connection.CloseAsync();
                adapter.Dispose();

                if (searchParam.OutputSetting == 0)
                {
                    if (searchParam.OutputKbn.Value == 0)
                        result = result.OrderBy(_ => _.UnkYmd).ToList();
                    else
                        result = result.OrderBy(_ => _.SyaRyoCd).ToList();
                }
                else
                {
                    result = result.OrderBy(_ => _.SyaRyoCd).ThenBy(_ => _.UnkYmd).ThenBy(_ => _.EigyoCd).ToList();
                }

                return result;
            }
        }
    }
}
