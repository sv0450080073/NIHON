using HassyaAllrightCloud.Application.BatchKobanInput.Queries;
using HassyaAllrightCloud.Application.BatchKobanInput.Commands;
using HassyaAllrightCloud.Application.DepositList.Queries;
using HassyaAllrightCloud.Domain.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HassyaAllrightCloud.Commons.Helpers.BookingInputHelper;

namespace HassyaAllrightCloud.IService
{
    public interface IBatchKobanInputService
    {
        Task<List<CompanyData>> GetCompanyData(int tenantCdSeq);
        Task<List<LoadSaleBranchList>> GetSaleBranchData(int tenantCdSeq, int companyCdSeq);
        Task<List<Staffs>> GetStaffs(int tenantCdSeq, int companyCdSeq, BatchKobanInputFilterModel model);
        Task<List<KobanDataGridModel>> GetKobanDataGrids(BatchKobanInputFilterModel model, int tenantCdSeq);
        Task<List<TaskModel>> GetTasks(int tenantCdSeq);
        Task<List<WorkHolidayTypeDataModel>> GetWorkHolidayTypes(int tenantCdSeq);
        Task<bool> DeleteKinkyuj(string targetYmd, int syainCdSeq);
        Task<bool> DeleteKoban(string targetYmd, int syainCdSeq);
        Task<(bool,int)> SaveKinkyuj(CellModel cell, WorkHolidayTypeDataModel holidayType, MyTime startTime, MyTime endTime);
        Task<bool> SaveKoban(CellModel cell, WorkHolidayTypeDataModel holidayType, MyTime startTime, MyTime endTime, int KinKyuTblCdSeq);
    }
    public class BatchKobanInputService : IBatchKobanInputService
    {
        private IMediator _mediator;
        public BatchKobanInputService(IMediator mediatR)
        {
            _mediator = mediatR;
        }

        public async Task<bool> DeleteKinkyuj(string targetYmd, int syainCdSeq)
        {
            return await _mediator.Send(new DeleteKikyuj() { SyainCdSeq = syainCdSeq, TargetYmd = targetYmd });
        }

        public async Task<bool> DeleteKoban(string targetYmd, int syainCdSeq)
        {
            return await _mediator.Send(new DeleteKoban() { TargetYmd = targetYmd, SyainCdSeq = syainCdSeq });
        }

        public async Task<List<CompanyData>> GetCompanyData(int tenantCdSeq)
        {
            return await _mediator.Send(new GetCompanyData() { TenantCdSeq = tenantCdSeq});
        }

        public async Task<List<KobanDataGridModel>> GetKobanDataGrids(BatchKobanInputFilterModel model, int tenantCdSeq)
        {
            return await _mediator.Send(new GetKobanDataGrid() { Model = model, TenantCdSeq = tenantCdSeq });
        }

        public async Task<List<LoadSaleBranchList>> GetSaleBranchData(int tenantCdSeq, int companyCdSeq)
        {
            return await _mediator.Send(new GetOffices() { TenantCdSeq = tenantCdSeq, CompanyCdSeq = companyCdSeq });
        }

        public async Task<List<Staffs>> GetStaffs(int tenantCdSeq, int companyCdSeq, BatchKobanInputFilterModel model)
        {
            return await _mediator.Send(new GetStaffs() { TenantCdSeq = tenantCdSeq, CompanyCdSeq = companyCdSeq, Model = model });
        }

        public async Task<List<TaskModel>> GetTasks(int tenantCdSeq)
        {
            return await _mediator.Send(new GetTask() { TenantCdSeq = tenantCdSeq});
        }

        public async Task<List<WorkHolidayTypeDataModel>> GetWorkHolidayTypes(int tenantCdSeq)
        {
            return await _mediator.Send(new GetKinKyuType() { TennantCdSeq = tenantCdSeq });
        }

        public async Task<(bool,int)> SaveKinkyuj(CellModel cell, WorkHolidayTypeDataModel holidayType, MyTime startTime, MyTime endTime)
        {
            return await _mediator.Send(new SaveKikyuj() { Cell = cell, EndTime = endTime, HolidayType = holidayType, StartTime = startTime });
        }

        public async Task<bool> SaveKoban(CellModel cell, WorkHolidayTypeDataModel holidayType, MyTime startTime, MyTime endTime, int KinKyuTblCdSeq)
        {
            return await _mediator.Send(new SaveKoban() { StartTime = startTime, Cell = cell, EndTime = endTime, HolidayType = holidayType, KinKyuTblCdSeq = KinKyuTblCdSeq });
        }
    }
}
