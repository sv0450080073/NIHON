using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace HassyaAllrightCloud.Application.HaitaCheck.Queries
{
    public class GetHaitaCheckQuery : IRequest<bool>
    {
        public List<HaitaModelCheck> HaitaModelsToCheck { get; set; }
        public class Handler : IRequestHandler<GetHaitaCheckQuery, bool>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext context)
            {
                _dbContext = context;
            }
            public async Task<bool> Handle(GetHaitaCheckQuery request, CancellationToken cancellationToken)
            {
                foreach (HaitaModelCheck HaitaModelToCheck in request.HaitaModelsToCheck)
                {
                    string Query = "SELECT MAX(CONCAT(UpdYmd, UpdTime)) AS UpdYmdTime FROM " + HaitaModelToCheck.TableName;
                    if (HaitaModelToCheck.PrimaryKeys.Count > 0)
                    {
                        Query += " WHERE ";
                        Query += String.Join(" AND ", HaitaModelToCheck.PrimaryKeys.Select(e => e.PrimaryKey + " " + e.Value).ToArray());
                        using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                        {
                            command.CommandText = Query;
                            _dbContext.Database.OpenConnection();
                            var totalRow = await command.ExecuteReaderAsync();
                            totalRow.Read();
                            if ((HaitaModelToCheck.CurrentUpdYmdTime == null && !totalRow.IsDBNull(0)) ||
                                (HaitaModelToCheck.CurrentUpdYmdTime != null && totalRow.IsDBNull(0)) ||
                                (!totalRow.IsDBNull(0) && (string)totalRow["UpdYmdTime"] != HaitaModelToCheck.CurrentUpdYmdTime))
                            {
                                return false;
                            }
                            totalRow.Close();
                            _dbContext.Database.CloseConnection();
                        }
                    }
                }
                return true;
            }
        }
    }
}
