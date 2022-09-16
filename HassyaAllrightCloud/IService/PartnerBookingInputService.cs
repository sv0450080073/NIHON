using HassyaAllrightCloud.Application.PartnerBookingInput.Queries;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IPartnerBookingInputService
    {
        Task<List<HaiShaDataTable>> GetHaiShaData(string ukeNo, string unkRen, int tenantCdSeq);
        Task<List<YouShaDataTable>> GetYouShaData(string ukeNo, string unkRen, int tenantCdSeq);
        Task<List<YyKSyuDataPopup>> GetYyKSyuDataPopup(string ukeNo, string unkRen, int tenantCdSeq, int youTblSeq);
        Task<YouShaDataPopup> GetYouShaDataPopup(string ukeNo, string unkRen, int tenantCdSeq, int youTblSeq);
        Task<List<TkdYouSyu>> GetYouSyuDataPopup(string ukeNo, string unkRen,  int youTblSeq);
        Task<List<TokistData>> GetTokiskData(string ukeNo, string unkRen, string dateParam, int tenantCdSeq);
        Task<List<CodeKbnDataPopup>> GetCodeKbnData(int tenantCdSeq);
        Task<VATDataPopup> GetVATPopup_HaiShaData(List<HaiShaDataTable> haiShaDataTables);
        Task<List<YouSyuTable>> GetYouSyuTableData(string ukeno, string unkren, int tenantCdSeq);
        Task<List<YouShaDataTable>> CheckDataYouSha(string ukeNo, string unkRen, int tenantCdSeq);
        Task<List<TKM_KasSetData>> GetTKM_KasSetData(int companyCdSeq);
        Task<ParterBookingInputHaita> GetHaitaCheck(string ukeno, short unkRen, int youTblSeq);
    }
    public class PartnerBookingInputService : IPartnerBookingInputService
    {
        private readonly IMediator _mediator;
        public PartnerBookingInputService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<List<HaiShaDataTable>> GetHaiShaData(string ukeNo, string unkRen, int tenantCdSeq)
        {
            return await _mediator.Send(new GetHaiShaDataTable() { Ukeno = ukeNo, Unkren = unkRen, TenantCdSeq = tenantCdSeq });
        }
        public async Task<List<YouShaDataTable>> GetYouShaData(string ukeNo, string unkRen, int tenantCdSeq)
        {
            return await _mediator.Send(new GetYouShaDataTable() { Ukeno = ukeNo, Unkren = unkRen, TenantCdSeq = tenantCdSeq });
        }
        public async Task<List<YyKSyuDataPopup>> GetYyKSyuDataPopup(string ukeNo, string unkRen, int tenantCdSeq, int youTblSeq)
        {
            return await _mediator.Send(new GetYyKSyuDataPopup() { Ukeno = ukeNo, Unkren = unkRen, TenantCdSeq = tenantCdSeq, YouTblSeq=youTblSeq });
        }
        public async Task<List<TokistData>> GetTokiskData(string ukeNo, string unkRen, string dateParam,int tenantCdSeq)
        {
            return await _mediator.Send(new GetTokiskData() { Ukeno = ukeNo, Unkren = unkRen, DateParam = dateParam, TenantCdSeq=tenantCdSeq });
        }
        public async Task<YouShaDataPopup>GetYouShaDataPopup(string ukeNo, string unkRen, int tenantCdSeq, int youTblSeq)
        {
            return await _mediator.Send(new GetYouShaDataPopup() { Ukeno = ukeNo, Unkren = unkRen, TenantCdSeq = tenantCdSeq, YouTblSeq = youTblSeq });
        }
        public async Task<List<CodeKbnDataPopup>> GetCodeKbnData(int tenantCdSeq)
        {
            return await _mediator.Send(new GetCodeKbnData() { TenantCdSeq = tenantCdSeq });
        }
        public async Task<VATDataPopup> GetVATPopup_HaiShaData(List<HaiShaDataTable> haiShaDataTables)
        {
            return await _mediator.Send(new GetVATHaiShaData() { HaiShaDataList=haiShaDataTables });
        }
        public async Task<List<TkdYouSyu>> GetYouSyuDataPopup(string ukeNo, string unkRen, int youTblSeq)
        {
            return await _mediator.Send(new GetYouSyuDataPopup() { Ukeno = ukeNo, Unkren = unkRen, YouTblSeq = youTblSeq });
        }
        public async Task<List<CodeKbnBunruiDataPopup>> GetCodeKbnBunruiData(int tenantCdSeq)
        {
            return await _mediator.Send(new GetCodeBunruiData() { TenantCdSeq = tenantCdSeq });
        }

        public async Task<List<YouSyuTable>> GetYouSyuTableData(string ukeno, string unkren, int tenantCdSeq)
        {
            return await _mediator.Send(new GetYouSyuTablePopup() { Ukeno =ukeno, Unkren=unkren ,TenantCdSeq = tenantCdSeq });
        }

        public async Task<List<YouShaDataTable>> CheckDataYouSha(string ukeNo, string unkRen, int tenantCdSeq)
        {
            return await _mediator.Send(new GetYouShaDataTable() { Ukeno = ukeNo, Unkren = unkRen, TenantCdSeq = tenantCdSeq });
        }

        public async Task<List<TKM_KasSetData>> GetTKM_KasSetData(int companyCdSeq)
        {
            return await _mediator.Send(new GetTKM_KasSetData() { CompanyCdSeq = companyCdSeq });
        }

        public async Task<ParterBookingInputHaita> GetHaitaCheck(string ukeno, short unkRen, int youTblSeq)
        {
            return await _mediator.Send(new GetLastUpdYmdTimeForPBI(ukeno, unkRen, youTblSeq));
        }
    }
}
