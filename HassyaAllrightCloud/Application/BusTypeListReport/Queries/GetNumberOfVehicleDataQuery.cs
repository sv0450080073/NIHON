using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading;
using HassyaAllrightCloud.Commons.Helpers;
namespace HassyaAllrightCloud.Application.BusTypeListReport.Queries
{
    public class GetNumberOfVehicleDataQuery : IRequest<List<BusTypeDetailDataReport>>
    {
        public BusTypeListData BusTypeListDataParam { get; set; }       
        public class Handler : IRequestHandler<GetNumberOfVehicleDataQuery, List<BusTypeDetailDataReport>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetNumberOfVehicleDataQuery> _logger;
            private readonly ITPM_CodeSyService _codeSyuService;
            public Handler(KobodbContext context, ILogger<GetNumberOfVehicleDataQuery> logger, ITPM_CodeSyService codeSyuService)
            {
                _context = context;
                _logger = logger;
                _codeSyuService = codeSyuService ?? throw new ArgumentNullException(nameof(codeSyuService));
            }

            public async Task<List<BusTypeDetailDataReport>> Handle(GetNumberOfVehicleDataQuery request, CancellationToken cancellationToken)
            {             
                List<BusTypeDetailDataReport> result = new List<BusTypeDetailDataReport>();
                try
                {
                    var paramSearch = request.BusTypeListDataParam;
                    int valueKataKbn = 0;
                    if (!int.TryParse(paramSearch.BusType.CodeKbn, out valueKataKbn))
                    {
                        valueKataKbn = 0;
                    }
                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Pro_GetBusTypeDetailList_R";
                    command.Parameters.AddWithValue("@DateHaiSha", paramSearch.StartDate.ToString("yyyyMMdd"));
                    command.Parameters.AddWithValue("@BookingTypeList", string.Join('-', paramSearch.ReservationList.Select(c => c.YoyaKbnSeq)));
                    command.Parameters.AddWithValue("@CompanyCdSeq", paramSearch.Company?.CompanyCdSeq ?? 0); 
                    command.Parameters.AddWithValue("@EigyoCdFrom", paramSearch.BranchStart?.EigyoCd ?? 0);
                    command.Parameters.AddWithValue("@EigyoCdTo", paramSearch.BranchEnd?.EigyoCd??0);
                    command.Parameters.AddWithValue("@SyainCdFrom", paramSearch.SalesStaffStart.SyainCd??"0");
                    command.Parameters.AddWithValue("@SyainCdTo", paramSearch.SalesStaffEnd?.SyainCd ?? "0");
                    command.Parameters.AddWithValue("@PersonInputFrom", paramSearch.PersonInputStart?.SyainCd ?? "0");
                    command.Parameters.AddWithValue("@PersonInputTo", paramSearch.PersonInputEnd?.SyainCd ?? "0");
                    command.Parameters.AddWithValue("@KataKbn", valueKataKbn);
                    command.Parameters.AddWithValue("@SyaSyuCdFrom", paramSearch.VehicleFrom?.SyaSyuCd??0);
                    command.Parameters.AddWithValue("@SyaSyuCdTo", paramSearch.VehicleTo?.SyaSyuCd??0);
                    command.Parameters.AddWithValue("@BasyoMapCdFrom", paramSearch.DestinationStart?.BasyoMapCd??"0");
                    command.Parameters.AddWithValue("@BasyoMapCdTo", paramSearch.DestinationEnd?.BasyoMapCd ?? "0");
                    command.Parameters.AddWithValue("@NumberDayLoop", paramSearch.numberDay);                   
                    command.Parameters.AddWithValue("@TenantCdSeq", paramSearch.TenantCdSeq);
                   
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    await command.Connection.CloseAsync();
                    result = MapTableToObjectHelper.ConvertDataTable<BusTypeDetailDataReport>(dt);
                    return result;
                }
                catch (Exception ex)
                {
                    return result;
                }
            }
        }
    }
}
