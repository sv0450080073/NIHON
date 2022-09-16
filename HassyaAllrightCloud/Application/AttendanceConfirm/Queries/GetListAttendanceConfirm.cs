using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using HassyaAllrightCloud.Commons.Helpers;

namespace HassyaAllrightCloud.Application.AttendanceConfirm.Queries
{
    public class GetListAttendanceConfirm : IRequest<List<CurrentAttendanceConfirm>>
    {
        public AttendanceConfirmReportData SearchParams { get; set; }
        public class Handler : IRequestHandler<GetListAttendanceConfirm, List<CurrentAttendanceConfirm>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<CurrentAttendanceConfirm>> Handle(GetListAttendanceConfirm request, CancellationToken cancellationToken)
            {
                List<CurrentAttendanceConfirm> result = new List<CurrentAttendanceConfirm>();
                try
                {
                    var searchParam = request.SearchParams;
                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Pro_GetInfoBooking_R";
                    command.Parameters.AddWithValue("@DateBooking", searchParam.OperationDate.ToString("yyyyMMdd") ?? "");
                    command.Parameters.AddWithValue("@ListCompany", FormatListStringCompany(searchParam.CompanyChartData) ?? "");
                    command.Parameters.AddWithValue("@BranchFrom", searchParam.VehicleDispatchOffice1.EigyoCd.ToString() ?? "");
                    command.Parameters.AddWithValue("@BranchTo", searchParam.VehicleDispatchOffice2.EigyoCd.ToString());
                    command.Parameters.AddWithValue("@StartYoyaKbn", Convert.ToInt32(searchParam.BookingTypeFrom?.YoyaKbnSeq).ToString());
                    command.Parameters.AddWithValue("@EndYoyaKbn", Convert.ToInt32(searchParam.BookingTypeTo?.YoyaKbnSeq).ToString());
                    //command.Parameters.AddWithValue("@BookingTypeList", FormatListStringCondition(searchParam.ReservationList.Select(_ => _.YoyaKbnSeq.ToString()).ToList())?? "");                    
                    command.Parameters.AddWithValue("@MihaisyaKbn", searchParam.Undelivered ?? "");
                    command.Parameters.AddWithValue("@Order", searchParam.OutputOrder.IdValue.ToString() ?? "");
                    command.Parameters.AddWithValue("@TenantCdSeq", searchParam.TenantCdSeq.ToString() ?? "");                 
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    await command.Connection.CloseAsync();
                    result = MapTableToObjectHelper.ConvertDataTable<CurrentAttendanceConfirm>(dt);
                    return result;
                }
                catch (Exception ex)
                {
                    return result;
                }
            }
            public string FormatListStringCompany(List<CompanyChartData> companyChartData)
            {
                if (companyChartData == null) return "";
                if (companyChartData.Count > 1 && companyChartData[0].CompanyCdSeq == 0) return "0";
                else
                {
                    string[] strFormatCompanyArr = new string[companyChartData.Count];
                    if (companyChartData.Count >= 1)
                    {
                        for (int i = 0; i < companyChartData.Count; i++)
                        {
                            if (companyChartData[i].CompanyCdSeq > 0)
                            {
                                strFormatCompanyArr[i] = companyChartData[i].CompanyCdSeq.ToString();
                            }
                        }
                        return String.Join("-", strFormatCompanyArr);
                    }
                    else
                    {
                        return companyChartData.First().CompanyCdSeq.ToString();
                    }
                }

            }
            public string FormatListStringCondition(List<string> keyObjectivesList)
            {
                if (keyObjectivesList == null || keyObjectivesList.Count == 0) return "";
                return String.Join("-", keyObjectivesList.ToArray());
            }
        }
    }
}
