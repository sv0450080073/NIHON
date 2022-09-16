using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using HassyaAllrightCloud.Commons.Helpers;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace HassyaAllrightCloud.Application.RepairListReport.Queries
{
    public class GetListBusPairForSearch : IRequest<List<CurrentRepairList>>
    {
        public RepairListData SearchParams { get; set; }
        public class Handler : IRequestHandler<GetListBusPairForSearch, List<CurrentRepairList>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<CurrentRepairList>> Handle(GetListBusPairForSearch request, CancellationToken cancellationToken)
            {
                List<CurrentRepairList> result = new List<CurrentRepairList>();
                try
                {
                    var searchParam = request.SearchParams;
                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Pro_RepairList_R";
                    command.Parameters.AddWithValue("@CompanyList", FormatListStringCompany(searchParam.CompanyChartData)??"");
                    command.Parameters.AddWithValue("@DateRepairFrom", searchParam.StartDate.ToString("yyyyMMdd") ?? "");
                    command.Parameters.AddWithValue("@DateRepairTo", searchParam.EndDate.ToString("yyyyMMdd") ?? "");
                    command.Parameters.AddWithValue("@EigyoCdFrom", searchParam.BranchFrom ==null ?0: searchParam.BranchFrom.EigyoCd );
                    command.Parameters.AddWithValue("@EigyoCdTo", searchParam.BranchTo==null ?0: searchParam.BranchTo.EigyoCd);
                    command.Parameters.AddWithValue("@SyaRyoCdFrom", searchParam.VehicleFrom==null? 0 : searchParam.VehicleFrom.SyaRyoCd);
                    command.Parameters.AddWithValue("@SyaRyoCdTo", searchParam.VehicleTo==null ?0: searchParam.VehicleTo.SyaRyoCd);
                    command.Parameters.AddWithValue("@RepairCdFrom", searchParam.RepairFrom==null ?0: searchParam.RepairFrom.RepairCd);
                    command.Parameters.AddWithValue("@RepairCdTo", searchParam.RepairTo==null?0: searchParam.RepairTo.RepairCd);
                    command.Parameters.AddWithValue("@TenantCdSeq", searchParam.TenantCdSeq);
                    command.Parameters.AddWithValue("@OrderParam", searchParam.OutputOrder.IdValue);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    await command.Connection.CloseAsync();
                    result = MapTableToObjectHelper.ConvertDataTable<CurrentRepairList>(dt);
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
                if (companyChartData.Count > 1 && companyChartData[0].CompanyCdSeq == 0) return "";
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
        }
    }
}
