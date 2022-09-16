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

namespace HassyaAllrightCloud.Application.Staff.Queries
{
    public class GetCrewDataAcquisitionListQuery : IRequest<List<CrewDataAcquisitionItem>>
    {
        public int CompanyCdSeq { get; set; }
        public string UnkYmd { get; set; }
        public class Hanlder : IRequestHandler<GetCrewDataAcquisitionListQuery, List<CrewDataAcquisitionItem>>
        {
            private readonly KobodbContext _context;
            public Hanlder(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<CrewDataAcquisitionItem>> Handle(GetCrewDataAcquisitionListQuery request, CancellationToken cancellationToken)
            {
                List<CrewDataAcquisitionItem> result = new List<CrewDataAcquisitionItem>();

                try
                {
                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PK_dCrewDataAcquisitions_R";

                    command.Parameters.AddWithValue("@CompanyCdSeq", request.CompanyCdSeq);
                    command.Parameters.AddWithValue("@UnkYmd", request.UnkYmd);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    result = MapTableToObjectHelper.ConvertDataTable<CrewDataAcquisitionItem>(dt);

                    await command.Connection.CloseAsync();
                    adapter.Dispose();
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
