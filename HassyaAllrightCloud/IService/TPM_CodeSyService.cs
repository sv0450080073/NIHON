using HassyaAllrightCloud.Application.CodeKb.Queries;
using HassyaAllrightCloud.Application.CodeSy.Queries;
using HassyaAllrightCloud.Domain.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ITPM_CodeSyService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeSy"></param>
        /// <returns></returns>
        Task<TPM_CodeSyData> GetKanriKbnAsync(string codeSyu);
        /// <summary>
        /// Check what tenant will be used.
        /// <para>If <see cref="TPM_CodeSyData.KanriKbn"/> != 1 => return tenantCdSq input</para>
        /// <para>Otherwise return <c>0</c></para>
        /// </summary>
        /// <param name="tenantCdSq">Tenant to check</param>
        /// <param name="codeSyu">Condition to check tenant</param>
        /// <returns>Tenant will be used</returns>
        Task<int> CheckTenantByKanriKbnAsync(int tenantCdSq, string codeSyu);
        /// <summary>
        /// Check Codekbn had value or not
        /// </summary>
        /// <param name="tenantCdSq">TenantId</param>
        /// <param name="codeSyu">CodeSyu</param>
        /// <returns><c>true</c> if exists CodeKbn, Otherwise not found</returns>
        Task<bool> CodeKbnHadValueAsync(int tenantCdSq, string codeSyu);
        /// <summary>
        /// Get CodeKbn by tenantId and codeSyu.
        /// Check tenantId to decide what tenantId should get.
        /// <para>If tenantId is default tenant => get default TenantId has value = 0, after get if result is empty => get by tenant input before.</para>
        /// <para>Else get by tenant input, after get if result is empty => get default TenantId has value = 0.</para>
        /// </summary>
        /// <typeparam name="TResult">Type of output</typeparam>
        /// <param name="codeAction">Code had access data in VPM_CodeKbn. 
        /// Input: (tenantId, codeSyu)
        /// Example input:
        /// <example>
        ///     (tenantId, codeSyu) =>{ 
        ///         //dosomething 
        ///         return null;
        ///     }
        /// </example>
        /// </param>
        /// <param name="tenantId">TenantId for CodeKbn. Only use for get CodeKbn</param>
        /// <param name="codeSyu">Condition: CodeSyu</param>
        /// <returns>Collection CodeKbn type after filter</returns>
        Task<List<TResult>> FilterTenantIdByCodeSyu<TResult>(Func<int, string, Task<List<TResult>>> codeAction, int tenantId, string codeSyu) where TResult : class;
        /// <summary>
        /// Get CodeKbn by tenantId and codeSyu.
        /// Check tenantId to decide what tenantId should get.
        /// <para>If tenantId is default tenant => get default TenantId has value = 0, after get if result is empty => get by tenant input before.</para>
        /// <para>Else get by tenant input, after get if result is empty => get default TenantId has value = 0.</para>
        /// </summary>
        /// <typeparam name="TResult">Type of output</typeparam>
        /// <param name="codeAction">Code had access data in VPM_CodeKbn. 
        /// Input: (tenantId, codeSyu)
        /// Example input:
        /// <example>
        ///     (tenantIds, codeSyus) =>{ 
        ///         //dosomething 
        ///         return null;
        ///     }
        /// </example>
        /// </param>
        /// <param name="tenantId">TenantId for CodeKbn. Only use for get CodeKbn</param>
        /// <param name="codeSyu">Condition: CodeSyu</param>
        /// <returns>Collection CodeKbn type after filter</returns>
        Task<List<TResult>> FilterTenantIdByListCodeSyu<TResult>(Func<List<int>, List<string>, Task<List<TResult>>> codeAction, int tenantId, List<string> codeSyus) where TResult : class;
    }
    public class TPM_CodeSyService : ITPM_CodeSyService
    {
        private IMediator _mediatR;
        public TPM_CodeSyService(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        public async Task<int> CheckTenantByKanriKbnAsync(int tenantCdSq, string codeSyu)
        {
            var id = await CheckTenantByCodeSyAsync(tenantCdSq, codeSyu);
            bool codeKbHadValue = await CodeKbnHadValueAsync(id, codeSyu);
            bool isSystem = id == 0;
            if (codeKbHadValue)
            {
                return id;
            }
            // if (isSystem)
            // {
            //     return tenantCdSq;
            // }
            // else
            // {
            //     return 0;
            // }
            return tenantCdSq;
        }

        public async Task<bool> CodeKbnHadValueAsync(int tenantCdSq, string codeSyu)
        {
            if (codeSyu is null)
                throw new ArgumentNullException(nameof(codeSyu));

            return (await _mediatR.Send(new GetCodeKbByCodeSyu(tenantCdSq, codeSyu))).Any();
        }

        public async Task<List<TResult>> FilterTenantIdByCodeSyu<TResult>(Func<int, string, Task<List<TResult>>> codeAction, int tenantId, string codeSyu) where TResult : class
        {
            if (codeAction == null)
                throw new ArgumentNullException(nameof(codeAction));
            if (codeSyu == null)
                throw new ArgumentNullException(nameof(codeSyu));
            if (!codeSyu.Any())
                throw new ArgumentException("Please make sure input codeSyu.");

            var id = await CheckTenantByKanriKbnAsync(tenantId, codeSyu);
            return await codeAction.Invoke(id, codeSyu);
        }

        public async Task<List<TResult>> FilterTenantIdByListCodeSyu<TResult>(Func<List<int>, List<string>, Task<List<TResult>>> codeAction, int tenantId, List<string> codeSyus) where TResult : class
        {
            if (codeAction == null)
                throw new ArgumentNullException(nameof(codeAction));
            if (codeSyus == null)
                throw new ArgumentNullException(nameof(codeSyus));
            if (!codeSyus.Any())
                throw new ArgumentException("Please make sure input codeSyu");

            List<int> tenantIds = new List<int>();

            foreach (var code in codeSyus)
            {
                int oldTenant = tenantId;
                int newTenantId = await CheckTenantByCodeSyAsync(tenantId, code);
                bool codeKbHadValue = await CodeKbnHadValueAsync(newTenantId, code);

                tenantIds.Add(codeKbHadValue ? newTenantId : oldTenant);
            }

            return await codeAction.Invoke(tenantIds, codeSyus);
        }

        public async Task<TPM_CodeSyData> GetKanriKbnAsync(string codeSyu)
        {
            return await _mediatR.Send(new GetKanriKbnQuery(codeSyu));
        }

        private async Task<int> CheckTenantByCodeSyAsync(int tenantCdSq, string codeSyu)
        {
            if (codeSyu is null)
                throw new ArgumentNullException(nameof(codeSyu));

            TPM_CodeSyData status = await _mediatR.Send(new GetKanriKbnQuery(codeSyu));

            if (status != null && status.KanriKbn == 1)
            {
                return 0;
            }
            else
            {
                return tenantCdSq;
            }
        }
    }
}
 