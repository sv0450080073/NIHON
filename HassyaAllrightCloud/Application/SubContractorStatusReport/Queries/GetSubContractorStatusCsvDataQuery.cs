using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.SubContractorStatus;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.SubContractorStatusReport.Queries
{
    public class GetSubContractorStatusCsvDataQuery : IRequest<List<SubContractorStatusExportCsvData>>
    {
        private readonly SubContractorStatusData _searchOption;
        private readonly int _tenantId;

        public GetSubContractorStatusCsvDataQuery(SubContractorStatusData searchOption, int tenantId)
        {
            _searchOption = searchOption ?? throw new ArgumentNullException(nameof(searchOption));
            _tenantId = tenantId;
        }

        public class Handler : IRequestHandler<GetSubContractorStatusCsvDataQuery, List<SubContractorStatusExportCsvData>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetSubContractorStatusCsvDataQuery> _logger;
            public Handler(KobodbContext context, ILogger<GetSubContractorStatusCsvDataQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<SubContractorStatusExportCsvData>> Handle(GetSubContractorStatusCsvDataQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var condition = request._searchOption;
                    var result = new List<SubContractorStatusExportCsvData>();

                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Pro_SubContractorStatus_R_Csv";
                    command.Parameters.AddWithValue("@startDate", condition.StartDate.ToString("yyyyMMdd"));
                    command.Parameters.AddWithValue("@endDate", condition.EndDate.ToString("yyyyMMdd"));
                    command.Parameters.AddWithValue("@dateType", condition.DateType == DateType.Dispatch ? 1 : (condition.DateType == DateType.Arrival ? 2 : 3));
                    command.Parameters.AddWithValue("@gyosyaFrom", condition?.SelectedGyosyaFrom?.GyosyaCd ?? 0);
                    command.Parameters.AddWithValue("@gyosyaTo", condition?.SelectedGyosyaTo?.GyosyaCd ?? 999);
                    command.Parameters.AddWithValue("@tokuiFrom", condition?.SelectedTokiskFrom?.TokuiCd ?? 0);
                    command.Parameters.AddWithValue("@tokuiTo", condition?.SelectedTokiskTo?.TokuiCd ?? 0);
                    command.Parameters.AddWithValue("@sitenFrom", condition?.SelectedTokiStFrom?.SitenCd ?? 0);
                    command.Parameters.AddWithValue("@sitenTo", condition?.SelectedTokiStTo?.SitenCd ?? 9999);
                    command.Parameters.AddWithValue("@companyIds", string.Join('-', condition.Companies.Where(_ => _ != null).Select(_ => _.CompanyCdSeq)));
                    command.Parameters.AddWithValue("@brandStart", condition.BranchStart?.EigyoCd ?? 0);
                    command.Parameters.AddWithValue("@brandEnd", condition.BranchEnd?.EigyoCd ?? int.MaxValue);
                    command.Parameters.AddWithValue("@staffFrom", condition.StaffStart?.SyainCd ?? "0");
                    command.Parameters.AddWithValue("@staffTo", condition.StaffEnd?.SyainCd ?? $"{int.MaxValue}");
                    //command.Parameters.AddWithValue("@bookingTypes", string.Join('-', YoyKbnHelper.GetListYoyKbnFromTo(condition.BookingTypeStart, condition.BookingTypeEnd, condition.BookingTypes.Where(_ => _ != null).ToList()).Select(_ => _.YoyaKbnSeq)));
                    command.Parameters.AddWithValue("@bookingTypeFrom", condition.RegistrationTypeFrom == null ? 0 : condition.RegistrationTypeFrom.YoyaKbn);
                    command.Parameters.AddWithValue("@bookingTypeTo", condition.RegistrationTypeTo == null ? 0 : condition.RegistrationTypeTo.YoyaKbn);
                    command.Parameters.AddWithValue("@ukeCdFrom", condition._ukeCdFrom == -1 ? "1" : condition.UkeCdFrom);
                    command.Parameters.AddWithValue("@ukeCdTo", condition._ukeCdTo == -1 ? int.MaxValue.ToString() : condition.UkeCdTo);
                    command.Parameters.AddWithValue("@jitaFlg", (int)condition.OwnCompanyType.Option);
                    command.Parameters.AddWithValue("@tenantId", request._tenantId);

                    await command.Connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        result = await MapTableToObjectHelper.ConvertToListObject<SubContractorStatusExportCsvData>(reader);
                    };

                    await command.Connection.CloseAsync();

                    var distinct = result.Select(_ => new { _.UkeNo, _.UnkRen }).Distinct();

                    foreach (var dis in distinct)
                    {
                        result.FirstOrDefault(_ => _.UkeNo == dis.UkeNo && _.UnkRen == dis.UnkRen).IsRepeatItem = false;
                    }

                    return result.Select((item) => { item.IsInHandleRepeatMode = false; return item; }).ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    return new List<SubContractorStatusExportCsvData>();
                }
            }
        }
    }
}
