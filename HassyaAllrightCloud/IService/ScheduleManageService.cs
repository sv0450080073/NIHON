using HassyaAllrightCloud.Application.LeaveApplicationManagement.Commands;
using HassyaAllrightCloud.Application.LeaveApplicationManagement.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IScheduleManageService
    {
        Task<(IEnumerable<ScheduleManageGridData>, int)> GetScheduleDataGrid(ScheduleManageForm searchModel);
        Task<IEnumerable<Staffs>> GetScheduleStaff();
        Task<IEnumerable<CustomGroup>> GetScheduleCustomGroup();
        Task<IEnumerable<Branch>> GetStaffOffice();
        Task<ScheduleDetail> GetScheduleDeTail(int scheduleId);
        Task<bool> UpdateScheduleDetail(ScheduleDetail scheduleDetail);
        Task<Staffs> GetApprover(int syainCdSeq);
        Task<string> GetScheduleFeedback(ScheduleDataModel schedule);
        Task<Dictionary<string, int>> GetScheduleFeedbackGroup(ScheduleDataModel schedule);
        Task<string> GetMemberOfTranning(ScheduleDataModel schedule);
    }
    public class ScheduleManageService : IScheduleManageService
    {
        private readonly IMediator _mediator;
        public ScheduleManageService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Staffs> GetApprover(int syainCdSeq)
        {
            return await _mediator.Send(new GetApprover() { syainCdSeq = syainCdSeq });
        }

        public async Task<string> GetMemberOfTranning(ScheduleDataModel schedule)
        {
            return await _mediator.Send(new GetMemberOfTranning() { schedule = schedule });
        }

        public async Task<IEnumerable<CustomGroup>> GetScheduleCustomGroup()
        {
            return await _mediator.Send(new GetScheduleCustomGroup());
        }

        public async Task<(IEnumerable<ScheduleManageGridData>, int)> GetScheduleDataGrid(ScheduleManageForm searchModel)
        {
            return await _mediator.Send(new GetScheduleDataGrid() { searchModel = searchModel});
        }

        public async Task<ScheduleDetail> GetScheduleDeTail(int scheduleId)
        {
            return await _mediator.Send(new GetScheduleDeTail() { scheduleId = scheduleId });
        }

        public async Task<string> GetScheduleFeedback(ScheduleDataModel schedule)
        {
            return await _mediator.Send(new GetScheduleFeedback() { schedule = schedule });
        }

        public async Task<Dictionary<string, int>> GetScheduleFeedbackGroup(ScheduleDataModel schedule)
        {
            return await _mediator.Send(new GetScheduleFeedBackOfGroupQuery() { schedule = schedule });
        }

        public async Task<IEnumerable<Staffs>> GetScheduleStaff()
        {
            return await _mediator.Send(new GetScheduleStaff() { });
        }

        public async Task<IEnumerable<Branch>> GetStaffOffice()
        {
            return await _mediator.Send(new GetStaffOffice());
        }

        public async Task<bool> UpdateScheduleDetail(ScheduleDetail scheduleDetail)
        {
            return await _mediator.Send(new UpdateScheduleDetail() { scheduleDetail = scheduleDetail });
        }
    }
}
