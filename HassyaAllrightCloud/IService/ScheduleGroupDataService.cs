using DevExpress.ClipboardSource.SpreadsheetML;
using HassyaAllrightCloud.Application.ScheduleGroupData.Commands;
using HassyaAllrightCloud.Application.ScheduleGroupData.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IScheduleGroupDataService
    {
        Task<IEnumerable<StaffScheduleGroupData>> GetStaffScheduleGroupDatas(int groupId, string fromDate, string toDate, bool isCustomGr);
        Task<IEnumerable<StaffScheduleGroupData>> GetStaffScheduleGroupDatasCustom(int groupId, string fromDate, string toDate, bool isCustomGr);
        Task<IEnumerable<ScheduleDataModel>> GetScheduleDataModels(int groupId, string fromDate, string toDate);
        Task<IEnumerable<ScheduleDataModel>> GetBookingScheduleOfStaffFullDay(IEnumerable<StaffScheduleGroupData> scheduleData);
        Task<IEnumerable<ScheduleDataModel>> GetBookingScheduleOfStaffFullDayCustom(IEnumerable<StaffScheduleGroupData> scheduleData);
        Task<IEnumerable<ScheduleDataModel>> GetScheduleDataModelsCustom(int groupId, string fromDate, string toDate);
        Task<IEnumerable<ScheduleDataModel>> GetScheduleDataModelsFullDay(int groupId, string fromDate, string toDate);
        Task<IEnumerable<ScheduleDataModel>> GetScheduleDataModelsFullDayCustom(int groupId, string fromDate, string toDate);
        Task<BookedScheduleFeedback> GetBookedScheduleFeedback(AppointmentList data);
        Task<bool> SubmitScheduleFeedback(AppointmentList schedule, int value);
    }
    public class ScheduleGroupDataService : IScheduleGroupDataService
    {
        private readonly IMediator _mediator;
        public ScheduleGroupDataService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<StaffScheduleGroupData>> GetStaffScheduleGroupDatas(int groupId, string fromDate, string toDate, bool isCustomGr)
        {
            return await _mediator.Send(new GetStaffScheduleGroupDatas() { groupId = groupId, fromDate = fromDate, toDate = toDate, isCustomGr = isCustomGr });
        }

        public async Task<IEnumerable<StaffScheduleGroupData>> GetStaffScheduleGroupDatasCustom(int groupId, string fromDate, string toDate, bool isCustomGr)
        {
            return await _mediator.Send(new GetStaffScheduleGroupDatas() { groupId = groupId, fromDate = fromDate, toDate = toDate, isCustomGr = isCustomGr });
        }
        public Task<IEnumerable<ScheduleDataModel>> GetBookingScheduleOfStaffFullDay(IEnumerable<StaffScheduleGroupData> scheduleData)
        {
            return _mediator.Send(new GetBookingScheduleOfStaffFullDay() { scheduleData = scheduleData });
        }
        public Task<IEnumerable<ScheduleDataModel>> GetBookingScheduleOfStaffFullDayCustom(IEnumerable<StaffScheduleGroupData> scheduleData)
        {
            return _mediator.Send(new GetBookingScheduleOfStaffFullDay() { scheduleData = scheduleData });
        }
        public async Task<IEnumerable<ScheduleDataModel>> GetScheduleDataModels(int groupId, string fromDate, string toDate)
        {
            var scheduleData = await GetStaffScheduleGroupDatas(groupId, fromDate, toDate, false);
            var bookedScheduleData = await GetBookingScheduleOfStaffFullDay(scheduleData);
            return await _mediator.Send(new GetScheduleDataModels() { fromDate = fromDate, groupId = groupId, scheduleData = scheduleData, bookedScheduleData = bookedScheduleData, toDate = toDate });
        }

        public async Task<IEnumerable<ScheduleDataModel>> GetScheduleDataModelsCustom(int groupId, string fromDate, string toDate)
        {
            var scheduleData = await GetStaffScheduleGroupDatasCustom(groupId, fromDate, toDate, true);
            var bookedScheduleData = await GetBookingScheduleOfStaffFullDayCustom(scheduleData);
            return await _mediator.Send(new GetScheduleDataModels() { fromDate = fromDate, groupId = groupId, scheduleData = scheduleData, bookedScheduleData = bookedScheduleData, toDate = toDate });
        }

        public async Task<IEnumerable<StaffScheduleGroupData>> GetStaffScheduleGroupDatasFullDay(int groupId, string fromDate, string toDate)
        {
            var schedule = await GetStaffScheduleGroupDatas(groupId, fromDate, toDate, false);
            return await _mediator.Send(new GetStaffScheduleGroupDatasFullDay() { schedule = schedule });
        }

        public async Task<IEnumerable<StaffScheduleGroupData>> GetStaffScheduleGroupDatasFullDayCustom(int groupId, string fromDate, string toDate)
        {
            var schedule = await GetStaffScheduleGroupDatasCustom(groupId, fromDate, toDate, true);
            return await _mediator.Send(new GetStaffScheduleGroupDatasFullDay() { schedule = schedule });
            return await _mediator.Send(new GetStaffScheduleGroupDatasFullDay() { schedule = schedule });
        }

        public async Task<IEnumerable<ScheduleDataModel>> GetScheduleDataModelsFullDay(int groupId, string fromDate, string toDate)
        {
            var scheduleData = await GetStaffScheduleGroupDatasFullDay(groupId, fromDate, toDate);
            var bookedScheduleData = await GetBookingScheduleOfStaffFullDay(scheduleData);
            return await _mediator.Send(new GetScheduleDataModelsFullDay() { scheduleData = scheduleData, bookedScheduleData = bookedScheduleData});
        }

        public async Task<IEnumerable<ScheduleDataModel>> GetScheduleDataModelsFullDayCustom(int groupId, string fromDate, string toDate)
        {
            var scheduleData = await GetStaffScheduleGroupDatasFullDayCustom(groupId, fromDate, toDate);
            var bookedScheduleData = await GetBookingScheduleOfStaffFullDayCustom(scheduleData);
            return await _mediator.Send(new GetScheduleDataModelsFullDay() { scheduleData = scheduleData, bookedScheduleData = bookedScheduleData });
        }
     
        public async Task<BookedScheduleFeedback> GetBookedScheduleFeedback(AppointmentList data)
        {
            return await _mediator.Send(new GetBookedScheduleFeedback() { data = data });
        }

        public async Task<bool> SubmitScheduleFeedback(AppointmentList schedule, int value)
        {
            return await _mediator.Send(new SubmitScheduleFeedback() { schedule = schedule, value = value });
        }
    }
}
