using HassyaAllrightCloud.Application.GridLayout.Commands;
using HassyaAllrightCloud.Application.GridLayout.Queries;
using HassyaAllrightCloud.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IGridLayoutService
    {
        Task<List<TkdGridLy>> GetGridLayout(int employeeId, string formName, string gridName);
        Task<bool> SaveGridLayout(List<TkdGridLy> datas);
        Task<bool> DeleteSavedGridLayout(int employeeId, string formName, string gridName);
    }
    public class GridLayoutService : IGridLayoutService
    {
        private readonly IMediator _mediator;

        public GridLayoutService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<bool> DeleteSavedGridLayout(int employeeId, string formName, string gridName)
        {
            return await _mediator.Send(new DeleteGridLayout() { employeeId = employeeId, formName = formName, gridName = gridName });
        }

        public async Task<List<TkdGridLy>> GetGridLayout(int employeeId, string formName, string gridName)
        {
            return await _mediator.Send(new GetGridLayout() { gridName = gridName, formName = formName, employeeId = employeeId });
        }

        public async Task<bool> SaveGridLayout(List<TkdGridLy> datas)
        {
            return await _mediator.Send(new SaveGridLayout() { datas = datas });
        }
    }
}
