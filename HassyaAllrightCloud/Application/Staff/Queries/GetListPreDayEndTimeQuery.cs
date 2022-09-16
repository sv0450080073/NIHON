﻿using HassyaAllrightCloud.Commons.Helpers;
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
    public class GetListPreDayEndTimeQuery : IRequest<List<PreDayEndTimeItem>>
    {
        public string PreviousYmd { get; set; }
        public string UnkYmd { get; set; }
        public int CompanyCdSeq { get; set; }
        public class Handler : IRequestHandler<GetListPreDayEndTimeQuery, List<PreDayEndTimeItem>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<PreDayEndTimeItem>> Handle(GetListPreDayEndTimeQuery request, CancellationToken cancellationToken)
            {
                List<PreDayEndTimeItem> result = new List<PreDayEndTimeItem>();

                try
                {
                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PK_dPreDayEndTimes_R";

                    command.Parameters.AddWithValue("@PreviousYmd", request.PreviousYmd);
                    command.Parameters.AddWithValue("@UnkYmd", request.UnkYmd);
                    command.Parameters.AddWithValue("@CompanyCdSeq", request.CompanyCdSeq);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    result = MapTableToObjectHelper.ConvertDataTable<PreDayEndTimeItem>(dt);

                    await command.Connection.CloseAsync();
                    adapter.Dispose();
                    return result;
                }
                catch(Exception ex)
                {
                    // TODO: write log
                    throw ex;
                }
            }
        }
    }
}
