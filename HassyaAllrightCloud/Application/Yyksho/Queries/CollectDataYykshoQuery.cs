using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using MediatR;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Text;
using HassyaAllrightCloud.IService;

namespace HassyaAllrightCloud.Application.Yyksho.Queries
{
    public class CollectDataYykshoQuery : IRequest<TkdYyksho>
    {
        public BusscheduleData ScheduleFormData { get; set; }

        public class Handler : IRequestHandler<CollectDataYykshoQuery, TkdYyksho>
        {

            private readonly int _maxUkeCd;
            private readonly string _newUkeNo;
            private readonly KobodbContext _context;
            public Handler(KobodbContext context, ITKD_YykshoListService yykshoService)
            {
                _context = context;
                (_maxUkeCd, _newUkeNo) = yykshoService.GetNewUkeNo(new ClaimModel().TenantID).Result;
            }
            
            public async Task<TkdYyksho> Handle(CollectDataYykshoQuery request, CancellationToken cancellationToken)
            {
                Random rand = new Random();
                TkdYyksho yyksho = new TkdYyksho();
                yyksho.TenantCdSeq = new ClaimModel().TenantID;
                yyksho.UkeCd = _maxUkeCd+1;
                yyksho.UkeNo = _newUkeNo;
                yyksho.HenKai = 0;
                yyksho.UkeYmd = DateTime.Now.ToString("yyyyMMdd");
                yyksho.YoyaSyu = 1; //1:予約 2:キャンセル
                yyksho.YoyaKbnSeq = 1;
                yyksho.KikakuNo = 0;
                yyksho.TourCd = "";
                yyksho.KasTourCdSeq = 0;
                yyksho.UkeEigCdSeq = new ClaimModel().EigyoCdSeq;
                yyksho.SeiEigCdSeq = new ClaimModel().EigyoCdSeq;
                yyksho.IraEigCdSeq = new ClaimModel().EigyoCdSeq;
                yyksho.EigTanCdSeq = new ClaimModel().SyainCdSeq;
                yyksho.InTanCdSeq = new ClaimModel().SyainCdSeq;
                yyksho.YoyaNm = request.ScheduleFormData.BookingName;
                yyksho.YoyaKana = "";
                yyksho.TokuiSeq = request.ScheduleFormData.Customerlst.TokuiSeq;
                yyksho.SitenCdSeq = request.ScheduleFormData.Customerlst.SitenCdSeq;
                yyksho.SirCdSeq = request.ScheduleFormData.Customerlst.TokuiSeq;
                yyksho.SirSitenCdSeq = request.ScheduleFormData.Customerlst.SitenCdSeq;
                yyksho.TokuiTel = request.ScheduleFormData.Customerlst.TokuiTel;
                yyksho.TokuiTanNm = request.ScheduleFormData.Customerlst.TokuiTanNm;
                yyksho.TokuiFax = request.ScheduleFormData.Customerlst.TokuiFax;
                yyksho.TokuiMail = request.ScheduleFormData.Customerlst.TokuiMail;
                yyksho.UntKin = 0;
                yyksho.ZeiKbn = 1;
                yyksho.Zeiritsu = 10;
                yyksho.ZeiRui = 0;
                yyksho.TaxTypeforGuider = 3;
                yyksho.TaxGuider = 0;
                yyksho.TesuRitu = request.ScheduleFormData.Customerlst.TesuRitu;
                yyksho.TesuRyoG = 0;
                yyksho.FeeGuiderRate = 0;
                yyksho.FeeGuider = 0;
                yyksho.SeiKyuKbnSeq = 0;
                yyksho.SeikYm = DateTime.Now.ToString("yyyyMM");
                yyksho.SeiTaiYmd = DateTime.Now.ToString("yyyyMMdd");
                yyksho.CanRit = 0;
                yyksho.CanUnc = 0;
                yyksho.CanZkbn = 3;
                yyksho.CanSyoR = 0;
                yyksho.CanSyoG = 0;
                yyksho.CanYmd = "";
                yyksho.CanTanSeq = 0;
                yyksho.CanRiy = "";
                yyksho.CanFuYmd = "";
                yyksho.CanFuTanSeq = 0;
                yyksho.CanFuRiy = "";
                yyksho.BikoTblSeq = 0;
                yyksho.Kskbn = 2;
                yyksho.KhinKbn = 1;
                yyksho.KaknKais = 0;
                yyksho.KaktYmd = "";
                yyksho.HaiSkbn = 1;
                yyksho.HaiIkbn = 1;
                yyksho.GuiWnin = 0;
                yyksho.NippoKbn = 1;
                yyksho.YouKbn = 1;
                yyksho.NyuKinKbn = 1;
                yyksho.NcouKbn = 1;
                yyksho.SihKbn = 1;
                yyksho.ScouKbn = 1;
                yyksho.SiyoKbn = 1;
                yyksho.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                yyksho.UpdTime = DateTime.Now.ToString("HHmmss");
                yyksho.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                yyksho.UpdPrgId = "KU0100";
                return await Task.FromResult(yyksho);
            }
        }
    }
}