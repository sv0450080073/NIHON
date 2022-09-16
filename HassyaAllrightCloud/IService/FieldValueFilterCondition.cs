using HassyaAllrightCloud.Application.FilterValue.Commands;
using HassyaAllrightCloud.Application.FilterValue.Queries;
using HassyaAllrightCloud.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IFieldValueFilterCondition
    {
        Task<bool> SaveFilterValue(List<TkdInpCon> tkdInpCon);
        Task<TkdInpCon> GetExistFilterValue(string formName, int employeeId, string fieldName, int filterId);
        Task<List<TkdInpCon>> GetFormFilterValue(string formName, int filterId, int employeeId);
    }
    public class FieldValueFilterCondition : IFieldValueFilterCondition
    {
        private readonly IMediator _mediator;
        public FieldValueFilterCondition(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<TkdInpCon> GetExistFilterValue(string formName, int employeeId, string fieldName, int filterId)
        {
            return await _mediator.Send(new GetExistFilterValue() { employeeId = employeeId, fieldName = fieldName, filterId = filterId, formName = formName});
        }

        public async Task<List<TkdInpCon>> GetFormFilterValue(string formName, int filterId, int employeeId)
        {
            return await _mediator.Send(new GetFormFilterValue() { formName = formName, filterId = filterId, employeeId = employeeId });
        }

        public async Task<bool> SaveFilterValue(List<TkdInpCon> tkdInpCon)
        {
            return await _mediator.Send(new SaveFilterValue() {tkdInpCon = tkdInpCon });
        }
    }
}
