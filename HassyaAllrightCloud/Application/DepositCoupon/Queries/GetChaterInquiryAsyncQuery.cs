using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.Domain.Dto.DepositCoupon;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BillPrint.Queries
{
    public class GetChaterInquiryAsyncQuery : IRequest<List<ChaterInquiryGrid>>
    {
        public int tenantCdSeq { get; set; }
        public OutDataTable outDataTable { get; set; }
        public class Handler : IRequestHandler<GetChaterInquiryAsyncQuery, List<ChaterInquiryGrid>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<ChaterInquiryGrid>> Handle(GetChaterInquiryAsyncQuery request, CancellationToken cancellationToken = default)
            {
                if (request.outDataTable == null)
                {
                    return new List<ChaterInquiryGrid>();
                }

                var chaterInquiryGrids = new List<ChaterInquiryGrid>();

                if (request.outDataTable.SeiFutSyu == (byte)1)
                {
                    await _context.LoadStoredProc("PK_dChaterInquiryOption1_R")
                        .AddParam("@TenantCdSeq", request.tenantCdSeq)
                        .AddParam("@UkeNo", request.outDataTable.UkeNo)
                        .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                        .ExecAsync(async r => chaterInquiryGrids = await r.ToListAsync<ChaterInquiryGrid>());
                }
                else
                {
                    await _context.LoadStoredProc("PK_dChaterInquiryOtherOption_R")
                        .AddParam("@TenantCdSeq", request.tenantCdSeq)
                        .AddParam("@SeiFutSyu", request.outDataTable.SeiFutSyu)
                        .AddParam("@FutuUnkRen", request.outDataTable.FutuUnkRen)
                        .AddParam("@UkeNo", request.outDataTable.UkeNo)
                        .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                        .ExecAsync(async r =>
                        {
                            while (await r.ReadAsync())
                            {
                                ChaterInquiryGrid chaterInquiryGrid = new ChaterInquiryGrid();
                                chaterInquiryGrid.GyosyaCd = (short)r["GyosyaCd"];
                                chaterInquiryGrid.TokuiCd = (short)r["TokuiCd"];
                                chaterInquiryGrid.SitenCd = (short)r["SitenCd"];
                                chaterInquiryGrid.TokuiRyak = (string)r["TokuiRyak"];
                                chaterInquiryGrid.SitenRyak = (string)r["SitenRyak"];
                                chaterInquiryGrid.SyaSyuDai = (int)r["SyaSyuDai"];
                                chaterInquiryGrid.HaseiKin = (int)r["HaseiKin"];
                                chaterInquiryGrid.SyaRyoSyo = (int)r["SyaRyoSyo"];
                                chaterInquiryGrid.SyaRyoTes = (int)r["SyaRyoTes"];
                                chaterInquiryGrid.Zeiritsu = (decimal)r["Zeiritsu"];
                                chaterInquiryGrid.TesuRitu = (decimal)r["TesuRitu"];
                                chaterInquiryGrid.YoushaGak = (int)r["YoushaGak"];
                                chaterInquiryGrid.SihRaiRui = (decimal)r["SihRaiRui"];
                                chaterInquiryGrids.Add(chaterInquiryGrid);
                            }
                            await r.CloseAsync();
                        });
                }

                return chaterInquiryGrids;
            }
        }
    }
}
