using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FilterValue.Commands
{
    public class SaveCustomFilterAndConditionsCommand : IRequest<Unit>
    {
        private readonly int _employeeId;
        private readonly int _filterId;
        private readonly string _filterName;
        private readonly string _formName;
        private readonly Dictionary<string, string> _keyValueFilterPairs;

        public SaveCustomFilterAndConditionsCommand(int employeeId, int filterId, string filterName, string formName, 
            Dictionary<string, string> keyValueFilterPairs)
        {
            if (employeeId < 0)
                throw new ArgumentOutOfRangeException(nameof(employeeId));
            if (filterId < 0)
                throw new ArgumentOutOfRangeException(nameof(filterId));
            if (string.IsNullOrEmpty(filterName))
                throw new ArgumentException($"'{nameof(filterName)}' cannot be null or empty", nameof(filterName));
            if (string.IsNullOrEmpty(formName))
                throw new ArgumentException($"'{nameof(formName)}' cannot be null or empty", nameof(formName));

            _employeeId = employeeId;
            _filterId = filterId;
            _filterName = filterName;
            _formName = formName;
            _keyValueFilterPairs = keyValueFilterPairs ?? throw new ArgumentNullException(nameof(keyValueFilterPairs));
        }

        public class Handler : IRequestHandler<SaveCustomFilterAndConditionsCommand, Unit>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<SaveCustomFilterAndConditionsCommand> _logger;

            public Handler(KobodbContext context, ILogger<SaveCustomFilterAndConditionsCommand> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<Unit> Handle(SaveCustomFilterAndConditionsCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    using(var trans = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            var tkdFilter = await CollectDataForTkdFilter(request);
                            var tkdInpCons = CollectDataForTkdInpCon(request);

                            if(tkdFilter  != null)
                                _context.TkdFilter.Add(tkdFilter);

                            _context.TkdInpCon.AddRange(tkdInpCons);

                            await _context.SaveChangesAsync();
                            await trans.CommitAsync();

                            return await Task.FromResult(Unit.Value);
                        }
                        catch (Exception ex)
                        {
                            await trans.RollbackAsync();
                            throw ex;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());

                    return await Task.FromResult(Unit.Value);
                }
            }

            private async Task<TkdFilter> CollectDataForTkdFilter(SaveCustomFilterAndConditionsCommand request)
            {
                var existFilter = await
                        _context.TkdFilter.Where(x => x.SyainCdSeq == request._employeeId &&
                                                       x.FormNm == request._formName &&
                                                       x.FilterId == request._filterId &&
                                                       x.SiyoKbn == 1)
                                           .FirstOrDefaultAsync();
                if (existFilter == null)
                {
                    TkdFilter tkdFilter = new TkdFilter()
                    {
                        FilterId = request._filterId,
                        FilterName = request._filterName,
                        FormNm = request._formName,
                        SiyoKbn = 1,
                        SyainCdSeq = request._employeeId,
                        UpdPrgId = Common.UpdPrgId,
                        UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                        UpdYmd = DateTime.Now.ToString("yyyyMMdd"),
                        UpdTime = DateTime.Now.ToString("HHmm"),
                    };
                    return tkdFilter;
                }
                else
                {
                    existFilter.FilterName = request._filterName;
                    _context.TkdFilter.Update(existFilter);
                }

                return null;
            }

            private List<TkdInpCon> CollectDataForTkdInpCon(SaveCustomFilterAndConditionsCommand request)
            {
                //remove old condition value
                var exists = _context.TkdInpCon.Where(_ => _.FormNm == request._formName && request._filterId == _.FilterId && _.SyainCdSeq == request._employeeId);
                _context.TkdInpCon.RemoveRange(exists);
                int effect = _context.SaveChanges();

                //add new condition value
                List<TkdInpCon> tkdInpCons = new List<TkdInpCon>();
                foreach (var item in request._keyValueFilterPairs)
                {
                    tkdInpCons.Add(new TkdInpCon()
                    {
                        SyainCdSeq = request._employeeId,
                        FilterId = request._filterId,
                        FormNm = request._formName,
                        ItemNm = item.Key,
                        NodeFlg = 0,
                        UpdPrgId = Common.UpdPrgId,
                        UpdYmd = DateTime.Now.ToString("yyyyMMdd"),
                        UpdTime = DateTime.Now.ToString("HHmm"),
                        UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                        JoInput = item.Value
                    });
                }
                return tkdInpCons;
            }
        }
    }
}
