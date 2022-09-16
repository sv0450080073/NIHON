using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.ETC.Commands
{
    public class UpdateListETCCommand : IRequest<bool>
    {
        public List<ETCData> ListModel { get; set; }
        public bool IsFromTranfer { get; set; } = false;
        public class Handler : IRequestHandler<UpdateListETCCommand, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(UpdateListETCCommand request, CancellationToken cancellationToken)
            {
                var currentDate = DateTime.Now;

                using var transaction = _context.Database.BeginTransaction();

                try
                {
                    request.ListModel.ForEach(item =>
                    {
                        var model = _context.TkdEtcImport.FirstOrDefault(_ => _.FileName == item.FileName && _.CardNo == item.CardNo && _.EtcRen == item.EtcRen && _.TenantCdSeq == new ClaimModel().TenantID);
                        if (model != null)
                        {
                            model.FutTumCdSeq = item.selectedFutai.FutaiCdSeq;
                            model.FutTumNm = item.selectedFutai.FutaiNm;
                            model.SeisanCdSeq = item.selectedSeisan.SeisanCdSeq;
                            model.SeisanKbn = item.selectedSeisan.SeisanKbn;
                            model.SeisanNm = item.selectedSeisan.SeisanNm;
                            model.TanKa = item.TanKa;
                            model.Suryo = item.Suryo;
                            model.TesuRitu = item.TesuRitu;

                            if (request.IsFromTranfer)
                            {
                                model.TensoKbn = item.TensoKbn;
                                model.UkeNo = item.UkeNo;
                                model.UnkRen = item.UnkRen;
                                model.TeiDanNo = item.TeiDanNo;
                                model.BunkRen = item.BunkRen;
                            }

                            if (string.IsNullOrEmpty(model.ExpItem) || model.ExpItem.Length < 3)
                                model.ExpItem = "999" + item.TesuKbn;
                            else if (model.ExpItem.Length > 3)
                                model.ExpItem = model.ExpItem.Substring(0, 3) + item.TesuKbn + model.ExpItem.Substring(3);
                            if(_context.Entry(model).State != EntityState.Unchanged)
                            {
                                model.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                                model.UpdTime = currentDate.ToString(CommonConstants.FormatHms);
                                model.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                                model.UpdPrgId = Constants.ETCUpdPrgId;
                                _context.SaveChanges();
                            }
                        }
                    });
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }

                transaction.Commit();
                return true;
            }
        }
    }
}
