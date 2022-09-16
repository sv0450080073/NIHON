using HassyaAllrightCloud.Application.CouponPayment.Commands;
using HassyaAllrightCloud.Application.CouponPayment.Queries;
using HassyaAllrightCloud.Application.TransportActualResult.Queries;
using HassyaAllrightCloud.Domain.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ICouponPaymentService
    {
        Task<List<EigyoListItem>> GetDepositOffice(int tenantCd, CancellationToken cancellationToken);
        Task<List<YoyaKbnItem>> GetYoyKbn(CancellationToken cancellationToken);
        Task<IEnumerable<CodeKbDataItem>> GetCodeKb(int tenantId);
        Task<(List<CouponPaymentGridItem>, CouponPaymentSummary, int)> GetCouponPayment(CouponPaymentFormModel model, bool isGetSingle, CouponPaymentGridItem item, CancellationToken cancellationToken);
        int GetPreNextIndex<T>(bool isPre, List<T> list, T val);
        Task<List<BillAddressItem>> GetBillAddress(CouponPaymentFormModel model, CancellationToken cancellationToken);
        Task<List<CouponPaymentFormGridItem>> GetCouponPaymentFormGridItem(int currentTenant, CouponPaymentGridItem item, CancellationToken cancellationToken);
        Task<bool> SaveCouponPayment(CouponPaymentPopupFormModel model, bool isUpdate, CouponPaymentGridItem gridItem, int currentUserId, int currentTenant, CancellationToken cancellationToken);
        Task<bool> SaveMultiCouponPayment(List<CouponPaymentGridItem> gridItem, CouponPaymentPopupFormModel model, int currentUserId, int currentTenant, CancellationToken cancellationToken);
        Task<bool> Delete(CouponPaymentFormGridItem item, CouponPaymentGridItem gridItem, int currentUserId, CancellationToken cancellationToken, int tenantId);
        Task<List<BankTransferItem>> GetBankTransferItems(CancellationToken cancellationToken);
        Task<CommonLastUpdatedYmdTime> GetCommonLastUpdatedYmdTime(CouponPaymentGridItem item, int tenantCdSeq, CancellationToken token);
        Task<SpecLastUpdatedYmdTime> GetSpecLastUpdatedYmdTime(CouponPaymentGridItem item, int nyuSihTblSeq, short nyuSihRen, int tenantCdSeq, CancellationToken token);
        bool CompareLatestUpdYmdTime(byte seiFutSyu, CommonLastUpdatedYmdTime ori, CommonLastUpdatedYmdTime cur);
    }

    public class CouponPaymentService : ICouponPaymentService
    {
        private IMediator _mediator { get; set; }
        public CouponPaymentService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<List<EigyoListItem>> GetDepositOffice(int tenantCd, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetEigyosByTenantCd() { TenantCdSeq = tenantCd }, cancellationToken);
        }

        public async Task<List<YoyaKbnItem>> GetYoyKbn(CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetYoyKbn(), cancellationToken);
        }
        public async Task<IEnumerable<CodeKbDataItem>> GetCodeKb(int tenantId)
        {
            return await _mediator.Send(new GetCodeKb() { TenantId = tenantId, CodeSyu = "SEIFUTSYU" });
        }

        public async Task<(List<CouponPaymentGridItem>, CouponPaymentSummary, int)> GetCouponPayment(CouponPaymentFormModel model, bool isGetSingle, CouponPaymentGridItem item, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetCouponPayment() { Model = model, IsGetSingle = isGetSingle, Item = item }, cancellationToken);
        }

        public int GetPreNextIndex<T>(bool isPre, List<T> list, T val)
        {
            var index = list.IndexOf(val);

            return isPre ? (index > 0 ? index - 1 : -1) : (index < list.Count - 1 ? index + 1 : -1);
        }

        public async Task<List<BillAddressItem>> GetBillAddress(CouponPaymentFormModel model, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetBillAddressList() { Model = model }, cancellationToken);
        }

        public async Task<List<CouponPaymentFormGridItem>> GetCouponPaymentFormGridItem(int currentTenant, CouponPaymentGridItem item, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetNyShmi() { CurrentTenant = currentTenant, SelectedItem = item }, cancellationToken);
        }

        public async Task<bool> SaveCouponPayment(CouponPaymentPopupFormModel model, bool isUpdate, CouponPaymentGridItem gridItem, int currentUserId, int currentTenant, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new SaveCouponPayment() { CurrentTenant = currentTenant, CurrentUserId = currentUserId, GridItem = gridItem, IsUpdate = isUpdate, Model = model }, cancellationToken);
        }

        public async Task<bool> Delete(CouponPaymentFormGridItem item, CouponPaymentGridItem gridItem, int currentUserId, CancellationToken cancellationToken, int tenantId)
        {
            return await _mediator.Send(new DeleteCouponPayment() { CurrentUserId = currentUserId, Item = item, CurrentTenantId = tenantId, GridItem = gridItem }, cancellationToken);
        }

        public async Task<List<BankTransferItem>> GetBankTransferItems(CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetBankTransferItems(), cancellationToken);
        }

        public async Task<bool> SaveMultiCouponPayment(List<CouponPaymentGridItem> gridItems, CouponPaymentPopupFormModel model, int currentUserId, int currentTenant, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new SaveMultiCouponPayment() { CurrentTenant = currentTenant, CurrentUserId = currentUserId, GridItems = gridItems, Model = model }, cancellationToken);
        }

        public async Task<CommonLastUpdatedYmdTime> GetCommonLastUpdatedYmdTime(CouponPaymentGridItem item, int tenantCdSeq, CancellationToken token)
        {
            return await _mediator.Send(new GetCommonLastUpdatedYmdTime(tenantCdSeq, item.UkeNo, item.SeiFutSyu, item.UnkRen, item.FutTumRen, item.SeiFutSyu == 6 ? (byte)2 : (byte)1), token);
        }
        public async Task<SpecLastUpdatedYmdTime> GetSpecLastUpdatedYmdTime(CouponPaymentGridItem item, int nyuSihTblSeq, short nyuSihRen, int tenantCdSeq, CancellationToken token)
        {
            return await _mediator.Send(new GetSpecLastUpdatedYmdTime(tenantCdSeq, item.UkeNo, item.SeiFutSyu, item.UnkRen, item.FutTumRen, item.SeiFutSyu == 6 ? (byte)2 : (byte)1, nyuSihTblSeq, nyuSihRen), token);
        }

        public bool CompareLatestUpdYmdTime(byte seiFutSyu, CommonLastUpdatedYmdTime ori, CommonLastUpdatedYmdTime cur)
        {
            return ((seiFutSyu == 1 || seiFutSyu == 7) ?
                                        cur.YykshoUpdYmd == ori.YykshoUpdYmd && cur.YykshoUpdTime == ori.YykshoUpdTime :
                                        cur.FuttumUpdYmd == ori.FuttumUpdYmd && cur.FuttumUpdTime == ori.FuttumUpdTime) &&
                                        cur.MishumUpdYmd == ori.MishumUpdYmd && cur.MishumUpdTime == ori.MishumUpdTime;
        }
    }
}
