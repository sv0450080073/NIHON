using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Constants;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HassyaAllrightCloud.Application.LockBooking.Queries
{
    public class GetLockBookingQuery : IRequest<List<LockBookingDetailData>>
    {
        private readonly int _page;
        private readonly byte _size;

        public GetLockBookingQuery(int page, byte size)
        {
            this._page = page;
            this._size = size;
        }

        public class Handler : IRequestHandler<GetLockBookingQuery, List<LockBookingDetailData>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<LockBookingDetailData>> Handle(GetLockBookingQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    int skip = (request._page - 1) * request._size;
                    int take = request._size;

                    var result = new List<LockBookingDetailData>();
                    result = await (from lockTable in _context.TkdLockTable
                                    join eigo in _context.VpmEigyos on lockTable.EigyoCdSeq equals eigo.EigyoCdSeq into eigoGr
                                    from eigoSub in eigoGr.DefaultIfEmpty()
                                    join syain in _context.VpmSyain on lockTable.UpdSyainCd equals syain.SyainCdSeq into syainGr
                                    from syainSub in syainGr.DefaultIfEmpty()
                                    where lockTable.TenantCdSeq == new ClaimModel().TenantID
                                    select new LockBookingDetailData()
                                    {
                                        SalesOfficeCode = eigoSub.EigyoCd.ToString("D5"),
                                        SalesOfficeName = eigoSub.EigyoNm,
                                        LockDate = DateTime.ParseExact(lockTable.LockYmd, "yyyyMMdd", CultureInfo.InvariantCulture),
                                        LastUpdatedDate = DateTime.ParseExact(lockTable.UpdYmd, "yyyyMMdd", CultureInfo.InvariantCulture),
                                        LastUpdatedTime = DateTime.ParseExact(lockTable.UpdTime, "HHmmss", CultureInfo.InvariantCulture),
                                        LastUpdatedPerson = syainSub.SyainNm,
                                    }).ToListAsync();
                    // var connection = _context.Database.GetDbConnection();
                    // using (SqlCommand command = new SqlCommand())
                    // {
                    //     command.Connection = (SqlConnection)connection;
                    //     await command.Connection.OpenAsync();// move
                    //     command.CommandType = CommandType.StoredProcedure;
                    //     command.CommandText = "Pro_GetLockBooking";
                    //     command.Parameters.AddWithValue("@skip", skip);
                    //     command.Parameters.AddWithValue("@take", take);
                    //     using (SqlDataReader reader = command.ExecuteReader())
                    //     {
                    //         while (reader.Read())
                    //         {
                    //             var item = new LockBookingDetailData();
                    //             item.SalesOfficeCode = Convert.ToInt32(reader["EigyoCd"]).ToString("D5");
                    //             item.SalesOfficeName = reader["EigyoNm"].ToString();
                    //             item.LockDate = DateTime.ParseExact(reader["LockYmd"].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                    //             item.LastUpdatedDate = DateTime.ParseExact(reader["UpdYmd"].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                    //             item.LastUpdatedTime = DateTime.ParseExact(reader["UpdTime"].ToString(), "HHmmss", CultureInfo.InvariantCulture);
                    //             item.LastUpdatedPerson = reader["SyainNm"].ToString();
                    //             result.Add(item);
                    //         }
                    //     }
                    // }

                    return await Task.FromResult(result);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
