using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using HassyaAllrightCloud.Commons.Helpers;
using Microsoft.AspNetCore.Http;

namespace HassyaAllrightCloud.Application.OperatingInstructionReport.Queries
{
    public class GetListOperatingInstruction: IRequest<List<CurrentOperatingInstruction>>
    {
        public OperatingInstructionReportData searchParams { get; set; }
        public class Handler : IRequestHandler<GetListOperatingInstruction, List<CurrentOperatingInstruction>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<CurrentOperatingInstruction>> Handle(GetListOperatingInstruction request, CancellationToken cancellationToken)
            {
                 var searchParams = request.searchParams;
                List<CurrentOperatingInstruction> result = new List<CurrentOperatingInstruction>();
                var tenantId = new ClaimModel().TenantID;
                try
                { 
                    string datedefault = (new DateTime()).ToString("yyyyMMdd");
                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "RP_UnkoushijishoReportNew";
                    command.Parameters.AddWithValue("@TenantCdSeq", tenantId);
                    command.Parameters.AddWithValue("@SyuKoYmd",searchParams.DeliveryDate.ToString("yyyyMMdd")==datedefault?"":searchParams.DeliveryDate.ToString("yyyyMMdd"));
                    command.Parameters.AddWithValue("@UkeCdFrom",searchParams.ReceiptNumberFrom == ""||long.Parse(searchParams.ReceiptNumberFrom)>=int.MaxValue ? 0 : int.Parse(searchParams.ReceiptNumberFrom) );
                    command.Parameters.AddWithValue("@UkeCdTo",searchParams.ReceiptNumberTo == ""|| long.Parse(searchParams.ReceiptNumberTo)>=int.MaxValue? int.MaxValue : int.Parse(searchParams.ReceiptNumberTo) );
                    command.Parameters.AddWithValue("@YoyakuFrom", (searchParams.YoyakuFrom == null || searchParams.YoyakuFrom.YoyaKbnSeq == 0 ) ? "0" : searchParams.YoyakuFrom.YoyaKbnSeq.ToString());
                    command.Parameters.AddWithValue("@YoyakuTo", (searchParams.YoyakuTo == null || searchParams.YoyakuTo.YoyaKbnSeq == 0) ? "0" : searchParams.YoyakuTo.YoyaKbnSeq.ToString());
                    command.Parameters.AddWithValue("@SyuEigCdSeq", searchParams.DepartureOffice.EigyoCdSeq);
                    command.Parameters.AddWithValue("@TeiDanNo", searchParams.TeiDanNo);
                    command.Parameters.AddWithValue("@UnkRen", searchParams.UnkRen);
                    command.Parameters.AddWithValue("@BunkRen", searchParams.BunkRen);
                    command.Parameters.AddWithValue("@SortOrder", searchParams.OutputOrder.IdValue);
                    command.Parameters.AddWithValue("@UkenoList", searchParams.UkenoList==null?"":searchParams.UkenoList);
                    command.Parameters.AddWithValue("@FormOutput", searchParams.FormOutput);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    await command.Connection.CloseAsync();
                    result = MapTableToObjectHelper.ConvertDataTable<CurrentOperatingInstruction>(dt);
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
