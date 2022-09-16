using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.StaffSchedule.Queries
{
    public class GetGroupScheduleInfo : IRequest<IEnumerable<CompanyScheduleInfo>>
    {
        public int Tenant { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public class Handler : IRequestHandler<GetGroupScheduleInfo, IEnumerable<CompanyScheduleInfo>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;

            public async Task<IEnumerable<CompanyScheduleInfo>> Handle(GetGroupScheduleInfo request, CancellationToken cancellationToken)
            {
                List<LoadServiceOffice> OfficeServicesInfo = await new ServiceOfficeService(_dbContext).Get(request.Tenant);
                List<CompanyScheduleInfo> Result = new List<CompanyScheduleInfo>();
                foreach (LoadServiceOffice OfficeServiceInfo in OfficeServicesInfo)
                {
                    if (Result.Count() == 0 || OfficeServiceInfo.CompanyCd != Result[Result.Count() - 1].CompanyId)
                    {
                        Result.Add(new CompanyScheduleInfo()
                        {
                            CompanyId = OfficeServiceInfo.CompanyCd,
                            CompanyName = OfficeServiceInfo.CompanyName,
                            GroupInfo = new List<GroupScheduleInfo>()
                        });
                    }
                    Result[Result.Count() - 1].GroupInfo.Add(new GroupScheduleInfo()
                    {
                        CompanyId = OfficeServiceInfo.CompanyCd,
                        GroupId = OfficeServiceInfo.OfficeCdSeq,
                        GroupName = OfficeServiceInfo.OfficeName,
                        MembersId = new List<int>()
                    });
                }
                List<GroupScheduleInfo> CustomGroupsInfo = await new ScheduleCustomGroupService(_dbContext).Get(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
                Result.Add(new CompanyScheduleInfo()
                {
                    CompanyId = 0,
                    CompanyName = null,
                    GroupInfo = CustomGroupsInfo
                });
                return await Task.FromResult(Result);
            }
        }
    }
}
