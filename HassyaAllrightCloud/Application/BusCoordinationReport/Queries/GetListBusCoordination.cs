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

namespace HassyaAllrightCloud.Application.BusCoordinationReport.Queries
{
    public class GetListBusCoordination : IRequest<List<CurrentBusCoordination>>
    {
        public BusCoordinationSearchParam searchParams { get; set; } 

        public class Handler : IRequestHandler<GetListBusCoordination, List<CurrentBusCoordination>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<CurrentBusCoordination>> Handle(GetListBusCoordination request, CancellationToken cancellationToken)
            {
                List<CurrentBusCoordination> result = new List<CurrentBusCoordination>();
                var tenantId = new ClaimModel().TenantID;
                try
                {                 
                    var searchParam = request.searchParams;
                    //CheckUkeno
                    if ((searchParam.BookingFrom=="0" && searchParam.BookingTo =="0")||(searchParam.BookingFrom=="" && searchParam.BookingTo ==""))
                    {
                        searchParam.BookingFrom = "0";
                        searchParam.BookingTo = "2147483647";
                    }
                    else if((searchParam.BookingFrom !="0" && searchParam.BookingTo =="0")||(searchParam.BookingFrom !="" && searchParam.BookingTo ==""))
                    {
                        searchParam.BookingTo = "2147483647";
                    }
                    else if ((searchParam.BookingFrom == "0" && searchParam.BookingTo != "0")||(searchParam.BookingFrom == "" && searchParam.BookingTo != ""))
                    {
                        searchParam.BookingFrom = "1";
                    }
                    if((long.TryParse(searchParam.BookingFrom, out long valueFrom) ? valueFrom : 0) > 2147483647)
                    {
                        searchParam.BookingFrom = "2147483647";
                    }
                    if ((long.TryParse(searchParam.BookingTo, out long valueTo) ? valueTo : 0) > 2147483647)
                    {
                        searchParam.BookingTo = "2147483647";
                    }
                    string customerStr = "";
                    string customerEnd = "";
                    string SupplierStr = "";
                    string SupplierEnd = "";
                    if (searchParam.GyosyaTokuiSakiFrom == null || searchParam.GyosyaTokuiSakiFrom.GyosyaCdSeq == 0)
                    {
                        customerStr = "0";
                    }
                    else
                    {
                        customerStr = searchParam.GyosyaTokuiSakiFrom.GyosyaCd.ToString("D3") + (searchParam.TokiskTokuiSakiFrom == null ? "0000" : searchParam.TokiskTokuiSakiFrom.TokuiCd.ToString("D4")) + (searchParam.TokiStTokuiSakiFrom == null ? "0000" : searchParam.TokiStTokuiSakiFrom.SitenCd.ToString("D4"));
                    }
                    if (searchParam.GyosyaTokuiSakiTo == null || searchParam.GyosyaTokuiSakiTo.GyosyaCdSeq == 0)
                    {
                        customerEnd = "0";
                    }
                    else
                    {
                        customerEnd = searchParam.GyosyaTokuiSakiTo.GyosyaCd.ToString("D3") + ((searchParam.TokiskTokuiSakiTo == null || searchParam.TokiskTokuiSakiTo.TokuiSeq == 0) ? "9999" : searchParam.TokiskTokuiSakiTo.TokuiCd.ToString("D4")) + ((searchParam.TokiStTokuiSakiTo == null || searchParam.TokiStTokuiSakiTo.SitenCdSeq == 0) ? "9999" : searchParam.TokiStTokuiSakiTo.SitenCd.ToString("D4"));
                    }
                    if (searchParam.GyosyaShiireSakiFrom == null || searchParam.GyosyaShiireSakiFrom.GyosyaCdSeq == 0)
                    {
                        SupplierStr = "0";
                    }
                    else
                    {
                        SupplierStr = searchParam.GyosyaShiireSakiFrom.GyosyaCd.ToString("D3") + (searchParam.TokiskShiireSakiFrom == null ? "0000" : searchParam.TokiskShiireSakiFrom.TokuiCd.ToString("D4")) + (searchParam.TokiStShiireSakiFrom == null ? "0000" : searchParam.TokiStShiireSakiFrom.SitenCd.ToString("D4"));
                    }
                    if (searchParam.GyosyaShiireSakiTo == null || searchParam.GyosyaShiireSakiTo.GyosyaCdSeq == 0)
                    {
                        SupplierEnd = "0";
                    }
                    else
                    {
                        SupplierEnd = searchParam.GyosyaShiireSakiTo.GyosyaCd.ToString("D3") + ((searchParam.TokiskShiireSakiTo == null || searchParam.TokiskShiireSakiTo.TokuiSeq == 0) ? "9999" : searchParam.TokiskShiireSakiTo.TokuiCd.ToString("D4")) + ((searchParam.TokiStShiireSakiTo == null || searchParam.TokiStShiireSakiTo.SitenCdSeq == 0) ? "9999" : searchParam.TokiStShiireSakiTo.SitenCd.ToString("D4"));
                    }
                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Pro_BusCoordination_R";
                    command.Parameters.AddWithValue("@CheckDaySearch", searchParam.DateType.ToString() ?? "1");
                    command.Parameters.AddWithValue("@DateFrom",searchParam.StartDate.ToString("yyyyMMdd") ?? "");
                    command.Parameters.AddWithValue("@DateTo", searchParam.EndDate.ToString("yyyyMMdd") ?? "");
                    command.Parameters.AddWithValue("@YoyakuFrom", (searchParam.YoyakuFrom == null || searchParam.YoyakuFrom.YoyaKbnSeq == 0) ? "0" : searchParam.YoyakuFrom.YoyaKbn.ToString());
                    command.Parameters.AddWithValue("@YoyakuTo", (searchParam.YoyakuTo == null || searchParam.YoyakuTo.YoyaKbnSeq == 0) ? "0" : searchParam.YoyakuTo.YoyaKbn.ToString());
                    command.Parameters.AddWithValue("@CustomerFrom", customerStr);
                    command.Parameters.AddWithValue("@CustomerTo", customerEnd);
                    command.Parameters.AddWithValue("@SupplierFrom", SupplierStr);
                    command.Parameters.AddWithValue("@SupplierTo", SupplierEnd);
                    command.Parameters.AddWithValue("@SaleBranch", searchParam.SaleBranch.OfficeCdSeq.ToString() ?? "");
                    command.Parameters.AddWithValue("@BookingFrom", searchParam.BookingFrom ?? "");
                    command.Parameters.AddWithValue("@BookingTo", searchParam.BookingTo ?? "");
                    command.Parameters.AddWithValue("@Staff", searchParam.Staff.SyainCdSeq == -1 ? "0" : searchParam.Staff.SyainCdSeq.ToString() ?? "");
                    command.Parameters.AddWithValue("@PersonInput", searchParam.Staff.SyainCdSeq == -1 ? "0" : searchParam.Staff.SyainCdSeq.ToString() ?? "");
                    command.Parameters.AddWithValue("@TenantCdSeq", new ClaimModel().TenantID.ToString());
                    command.Parameters.AddWithValue("@UkenoList", searchParam.UkenoList==null?"":searchParam.UkenoList );
                    command.Parameters.AddWithValue("@FormOutput", searchParam.FormOutput);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    await command.Connection.CloseAsync();
                    result = MapTableToObjectHelper.ConvertDataTable<CurrentBusCoordination>(dt);
                    return result;

                }
               catch(Exception ex)
                {
                    return result;
                }
            }
        }
    }
}
