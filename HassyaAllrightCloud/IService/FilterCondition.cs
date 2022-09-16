using HassyaAllrightCloud.Application.FilterValue.Commands;
using HassyaAllrightCloud.Application.FilterValue.Queries;
using HassyaAllrightCloud.Application.SuperMenuFilterValue.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IFilterCondition
    {
        Task<bool> SaveFilterCondtion(Dictionary<string,string> keyValueFilterPairs, string formName, int filterId, int employeeId);
        Task<List<TkdInpCon>> GetFilterCondition(string formName,int filterId, int employeeId);
        Task<List<TkdInpCon>> GetFilterCondition(string formName, int employeeId);
        Task<int> GetMaxCustomFilerId(string formName, int employeeId);
        Task<bool> SaveCustomFiler(int employeeId, int filterId, string formName, string FilterName);
        Task<bool> DeleteFilterCondition(int employeeId, int filterId, string formName);
        Task<bool> DeleteCustomFilter(int employeeId, int filterId, string formName, string FilterName);
        Task<bool> DeleteFilter(int employeeId, int filterId, string formName);
        Task<bool> DeleteCustomFilerCondition(int employeeId, int filerId, string formName);
        Task<List<CustomFilerModel>> GetCustomFilters(int employeeId, string formName);

        /// <summary>
        /// Save custome filter and condition
        /// <para>Format FieldName: [FieldName]-[S/L][IndexIncreate]         S: default value for the first time save to database</para>
        /// </summary>
        /// <param name="keyValueFilterPairs"></param>
        /// <param name="formName"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        Task SaveCustomFilterAndConditions(Dictionary<string, string> keyValueFilterPairs, string formName, int employeeId);
    }
    public class FilterCondition : IFilterCondition
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FilterCondition> _logger;

        public FilterCondition(IMediator mediator, ILogger<FilterCondition> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        public async Task<bool> DeleteFilter(int employeeId, int filterId, string formName)
        {
            return await _mediator.Send(new DeleteFiler() { EmployeeId = employeeId, FilerId = filterId, FormName = formName });
        }
        public async Task<bool> DeleteCustomFilter(int employeeId, int filterId, string formName, string FilterName)
        {
            return await _mediator.Send(new DeleteCustomFiler() { EmployeeId = employeeId, FilerId = filterId, FilterName = FilterName, FormName = formName });
        }

        public async Task<bool> DeleteCustomFilerCondition(int employeeId, int filerId, string formName)
        {
            return await _mediator.Send(new DeleteCustomFilterConditon() { FormName = formName, EmployeeId = employeeId, FilterId = filerId });
        }

        public async Task<List<CustomFilerModel>> GetCustomFilters(int employeeId, string formName)
        {
            return await _mediator.Send(new GetCustomFilter() { EmployeeId = employeeId, FormName = formName });
        }

        public async Task<List<TkdInpCon>> GetFilterCondition(string formName, int filterId, int employeeId)
        {
            return _mediator.Send(new GetFilterValue() { formName = formName, filterId = filterId, employeeId = employeeId }).Result;
        }

        public async Task<List<TkdInpCon>> GetFilterCondition(string formName, int employeeId)
        {
            return await GetFilterCondition(formName, 1, employeeId);
        }

        public async Task<int> GetMaxCustomFilerId(string formName, int employeeId)
        {
            return await _mediator.Send(new GetMaxFieldId() { EmployeeId = employeeId, FormName = formName });
        }

        public async Task<bool> SaveCustomFiler(int employeeId, int filterId, string formName, string FilterName)
        {
            return await _mediator.Send(new SaveCustomFilter() { EmployeeId = employeeId, FilterID = filterId, FormName = formName, FilterName = FilterName });
        }

        public async Task<bool> SaveFilterCondtion(Dictionary<string, string> keyValueFilterPairs, string formName, int filterId, int employeeId)
        {

            List<TkdInpCon> tkdInpCons = new List<TkdInpCon>();

            foreach (var item in keyValueFilterPairs)
            {
                tkdInpCons.Add(new TkdInpCon()
                {
                    SyainCdSeq = employeeId,
                    FilterId = filterId,
                    FormNm = formName,
                    ItemNm = item.Key,
                    NodeFlg = 0,
                    UpdPrgId = Common.UpdPrgId,
                    UpdYmd = DateTime.Now.ToString().Substring(0, 10).Replace("/", string.Empty),
                    UpdTime = DateTime.Now.ToString().Substring(11).Replace(":", string.Empty),
                    UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                    JoInput = item.Value
                });
            }

            await new FieldValueFilterCondition(_mediator).SaveFilterValue(tkdInpCons);
            return true;
        }

        public async Task SaveCustomFilterAndConditions(Dictionary<string, string> keyValueFilterPairs, string formName, int employeeId)
        {
            try
            {
                //string filterName = formName;
                //int filterId = 1;
                //if (await SaveCustomFiler(employeeId, filterId, formName, filterName))
                //{
                //    await SaveFilterCondtion(keyValueFilterPairs, formName, filterId, employeeId);
                //}
                string filterName = formName;
                int filterId = 1;
                string times = "S";

                var conditionValues = await _mediator.Send(new GetFilterValue() { employeeId = employeeId, filterId = filterId, formName = formName });

                if (conditionValues?.FirstOrDefault()?.ItemNm?.Split("-").LastOrDefault()?.Contains(times) ?? false)
                {
                    times = "L";
                }

                var newKeyValues = keyValueFilterPairs.ConvertSingleToMultipleValues(times);

                await _mediator.Send(new SaveCustomFilterAndConditionsCommand(employeeId, filterId, filterName, formName, newKeyValues));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        public async Task<bool> DeleteFilterCondition(int employeeId, int filterId, string formName)
        {
            return await _mediator.Send(new DeleteFilterCondition() { EmployeeId = employeeId, FormName = formName });
        }
    }
}