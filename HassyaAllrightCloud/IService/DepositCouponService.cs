using System.Collections.Generic;
using System.Threading.Tasks;
using HassyaAllrightCloud.Application.BillPrint.Queries;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.Domain.Dto.DepositCoupon;
using HassyaAllrightCloud.Domain.Entities;
using MediatR;

namespace HassyaAllrightCloud.IService
{
    public interface IDepositCouponService
    {
        Task <DepositCouponResult> GetDepositCouponsAsync(DepositCouponFilter depositCouponFilter);
        Task<List<string>> GetDepositCouponCodesAsync(DepositCouponFilter depositCouponFilter);
        Task<bool> CheckOpenChaterInquiryPopUpAsync(OutDataTable outDataTable);
        Task<bool> CheckOpenCouponBalancePopUpAsync();
        Task<bool> CheckOpenCouponPopUpAsync(OutDataTable outDataTable);
        Task<List<DepositMethod>> GetDepositMethodAsync();
        Task<List<DepositOffice>> GetDepositOfficeAsync();
        Task<List<DepositTransferBank>> GetDepositTransferBankAsync();
        Task<List<CouponBalanceGrid>> GetCouponBalanceAsync(int tenantCdSeq);
        Task<List<CouponBalanceGrid>> GetCouponAsync(int tenantCdSeq, OutDataTable outDataTable);
        Task<List<ChaterInquiryGrid>> GetChaterInquiryAsync(int tenantCdSeq, OutDataTable outDataTable);
        Task<List<DepositPaymentGrid>> GetDepositPaymentGridAsync(DepositPaymentFilter depositPaymentFilter);
        Task<string> GetPaymentIncidentalTypeAsync(int tenantCdSeq, byte seiFutSyu);
        Task<DepositPaymentHaitaCheck> GetDepositPaymentHaitaCheckAsync(DepositCouponGrid depositCouponGrid, DepositPaymentGrid depositPaymentGrid);
        Task<List<DepositPaymentHaitaCheck>> GetDepositPaymentHaitaCheckListAsync(List<DepositCouponGrid> depositCouponGrids);
        Task<string> SavePaymentAsync(DepositCouponGrid depositCouponGrid, DepositCouponPayment depositCouponPayment, 
        DepositPaymentGrid depositPaymentGrid, bool isDeleted, DepositPaymentHaitaCheck depositPaymentHaitaCheck);
        Task<string> SaveLumpPaymentAsync(List<DepositCouponGrid> gridCheckDatas, DepositCouponPayment depositCouponPayment, List<DepositPaymentHaitaCheck> depositPaymentHaitaChecks);
    }

    public class DepositCouponService : IDepositCouponService
    {
        private IMediator mediatR;
        public DepositCouponService(IMediator mediatR)
        {
            this.mediatR = mediatR;
        }

        public async Task<DepositCouponResult> GetDepositCouponsAsync(DepositCouponFilter depositCouponFilter)
        {
            return await mediatR.Send(new GetDepositCouponsAsyncQuery { depositCouponFilter = depositCouponFilter});
        }

        public async Task<List<string>> GetDepositCouponCodesAsync(DepositCouponFilter depositCouponFilter)
        {
            return await mediatR.Send(new GetDepositCouponCodesAsyncQuery { depositCouponFilter = depositCouponFilter });
        }

        public async Task<bool> CheckOpenChaterInquiryPopUpAsync(OutDataTable outDataTable)
        {
            return await mediatR.Send(new CheckOpenChaterInquiryPopUpAsyncQuery { outDataTable = outDataTable });
        }

        public async Task<bool> CheckOpenCouponBalancePopUpAsync()
        {
            return await mediatR.Send(new CheckOpenCouponBalancePopUpAsyncQuery {});
        }

        public async Task<bool> CheckOpenCouponPopUpAsync(OutDataTable outDataTable)
        {
            return await mediatR.Send(new CheckOpenCouponPopUpAsyncQuery { OutDataTable = outDataTable });
        }

        public async Task<List<DepositMethod>> GetDepositMethodAsync()
        {
            return await mediatR.Send(new GetDepositMethodAsyncQuery { });
        }

        public async Task<List<DepositOffice>> GetDepositOfficeAsync()
        {
            return await mediatR.Send(new GetDepositOfficeAsyncQuery { });
        }

        public async Task<List<DepositTransferBank>> GetDepositTransferBankAsync()
        {
            return await mediatR.Send(new GetDepositTransferBankAsyncQuery { });
        }

        public async Task<List<CouponBalanceGrid>> GetCouponBalanceAsync(int tenantCdSeq)
        {
            return await mediatR.Send(new GetCouponBalanceAsyncQuery { tenantCdSeq = tenantCdSeq });
        }

        public async Task<List<CouponBalanceGrid>> GetCouponAsync(int tenantCdSeq, OutDataTable outDataTable)
        {
            return await mediatR.Send(new GetCouponAsyncQuery { tenantCdSeq = tenantCdSeq, outDataTable = outDataTable });
        }

        public async Task<List<ChaterInquiryGrid>> GetChaterInquiryAsync(int tenantCdSeq, OutDataTable outDataTable)
        {
            return await mediatR.Send(new GetChaterInquiryAsyncQuery { tenantCdSeq = tenantCdSeq, outDataTable = outDataTable });
        }

        public async Task<List<DepositPaymentGrid>> GetDepositPaymentGridAsync(DepositPaymentFilter depositPaymentFilter)
        {
            return await mediatR.Send(new GetDepositPaymentGridAsyncQuery { depositPaymentFilter = depositPaymentFilter });
        }

        public async Task<string> GetPaymentIncidentalTypeAsync(int tenantCdSeq, byte seiFutSyu)
        {
            return await mediatR.Send(new GetPaymentIncidentalTypeAsyncQuery { tenantCdSeq = tenantCdSeq, seiFutSyu = seiFutSyu });
        }

        public async Task<DepositPaymentHaitaCheck> GetDepositPaymentHaitaCheckAsync(DepositCouponGrid depositCouponGrid, DepositPaymentGrid depositPaymentGrid)
        {
            return await mediatR.Send(new GetDepositPaymentHaitaCheckAsyncQuery { depositCouponGrid = depositCouponGrid, depositPaymentGrid = depositPaymentGrid });
        }

        public async Task<List<DepositPaymentHaitaCheck>> GetDepositPaymentHaitaCheckListAsync(List<DepositCouponGrid> depositCouponGrids)
        {
            return await mediatR.Send(new GetDepositPaymentHaitaCheckListAsyncQuery { depositCouponGrids = depositCouponGrids });
        }

        public async Task<string> SavePaymentAsync(DepositCouponGrid depositCouponGrid, DepositCouponPayment depositCouponPayment, 
        DepositPaymentGrid depositPaymentGrid, bool isDeleted, DepositPaymentHaitaCheck depositPaymentHaitaCheck)
        {
            return await mediatR.Send(new SavePaymentAsyncCommand { depositCouponGrid = depositCouponGrid, depositCouponPayment = depositCouponPayment, 
            depositPaymentGrid = depositPaymentGrid, isDeleted = isDeleted, depositPaymentHaitaCheck = depositPaymentHaitaCheck });
        }

        public async Task<string> SaveLumpPaymentAsync(List<DepositCouponGrid> gridCheckDatas, DepositCouponPayment depositCouponPayment, List<DepositPaymentHaitaCheck> depositPaymentHaitaChecks)
        {
            return await mediatR.Send(new SaveLumpPaymentAsyncCommand { gridCheckDatas = gridCheckDatas, depositCouponPayment = depositCouponPayment, depositPaymentHaitaChecks = depositPaymentHaitaChecks });
        }
    }
}
