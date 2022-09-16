using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.ETCImportConditionSetting.Commands
{
    public class InsertOrUpdateEtcImport : IRequest<bool>
    {
        public TkdEtcImport? Model { get; set; }
        public bool IsInsert { get; set; }
        public class Handler : IRequestHandler<InsertOrUpdateEtcImport, bool>
        {
            KobodbContext _context;
            public Handler(KobodbContext context) => _context = context;
            public async Task<bool> Handle(InsertOrUpdateEtcImport request, CancellationToken cancellationToken)
            {
                if (request == null || request.Model == null) return false;
                if(request.IsInsert)
                    await _context.TkdEtcImport.AddAsync(request.Model, cancellationToken);
                else
                {
                    var etcImport = _context.TkdEtcImport.FirstOrDefault(e => e.FileName == request.Model.FileName &&
                                                              e.CardNo == request.Model.CardNo &&
                                                              e.EtcRen == request.Model.EtcRen &&
                                                              e.TenantCdSeq == new ClaimModel().TenantID);
                    if (etcImport == null) return false;
                    etcImport.UkeNo = request.Model.UkeNo;
                    etcImport.UnkRen = request.Model.UnkRen;
                    etcImport.TeiDanNo = request.Model.TeiDanNo;
                    etcImport.BunkRen = request.Model.BunkRen;
                    etcImport.UnkTime = request.Model.UnkTime;
                    etcImport.HenKai = request.Model.HenKai;
                    etcImport.FutTumCdSeq = request.Model.FutTumCdSeq;
                    etcImport.FutTumNm = request.Model.FutTumNm;
                    etcImport.IriRyoChiCd = request.Model.IriRyoChiCd;
                    etcImport.IriRyoCd = request.Model.IriRyoCd;
                    etcImport.DeRyoChiCd = request.Model.DeRyoChiCd;
                    etcImport.DeRyoCd = request.Model.DeRyoCd;
                    etcImport.SeisanCdSeq = request.Model.SeisanCdSeq;
                    etcImport.SeisanNm = request.Model.SeisanNm;
                    etcImport.SeisanKbn = request.Model.SeisanKbn;
                    etcImport.Suryo = request.Model.Suryo;
                    etcImport.TanKa = request.Model.TanKa;
                    etcImport.TesuRitu = request.Model.TesuRitu;
                    etcImport.SyaRyoTes = request.Model.SyaRyoTes;
                    etcImport.TensoKbn = request.Model.TensoKbn;
                    etcImport.ImportTanka = request.Model.ImportTanka;
                    etcImport.BikoNm = request.Model.BikoNm;
                    etcImport.ExpItem = request.Model.ExpItem;
                    etcImport.SiyoKbn = request.Model.SiyoKbn;
                    etcImport.UpdYmd = request.Model.UpdYmd;
                    etcImport.UpdTime = request.Model.UpdTime;
                    etcImport.UpdSyainCd = request.Model.UpdSyainCd;
                    etcImport.UpdPrgId = request.Model.UpdPrgId;
                }
                    
                return await _context.SaveChangesAsync(cancellationToken) == 1;
            }
        }
    }
}
