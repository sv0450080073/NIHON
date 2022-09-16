using HassyaAllrightCloud.Application.EditReservationMobile.Commands;
using HassyaAllrightCloud.Application.EditReservationMobile.Queries;
using HassyaAllrightCloud.Domain.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IEditReservationMobileService
    {
        Task<List<ReservationTokiskData>> GetListTokisk(int tenantCdSeq);
        Task<List<ReservationCodeKbnData>> GetListCodeKb();
        Task<List<ReservationSyaSyuData>> GetListSyaSyu(int tenantCdSeq);
        Task<ReservationMobileData> GetReservationData(int ukeCd, int tenantCdSeq);
        Task<List<ReservationMobileChildItemData>> GetListChildItem(int ukeCd, int tenantCdSeq);
        Task<int> GetSyaSyuCdSeq(int syaRyoCdSeq);
        Task<string> InsertReservation(ReservationMobileData Data, int CompanyCdSeq, int SyaRyoCdSeq, int TenantCdSeq, int SyainCdSeq, int EigyoCdSeq);
        Task<bool> UpdateReservation(ReservationMobileData Data, List<ReservationMobileChildItemData> ListDelete, int CompanyCdSeq, int TenantCdSeq, int SyainCdSeq);
    }

    public class EditReservationMobileService : IEditReservationMobileService
    {
        private readonly IMediator _mediator;
        public EditReservationMobileService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<List<ReservationTokiskData>> GetListTokisk(int tenantCdSeq)
        {
            return await _mediator.Send(new GetListTokiskQuery() { TenantCdSeq = tenantCdSeq });
        }

        public async Task<List<ReservationCodeKbnData>> GetListCodeKb()
        {
            return await _mediator.Send(new GetListCodeKbQuery());
        }

        public async Task<List<ReservationSyaSyuData>> GetListSyaSyu(int tenantCdSeq)
        {
            return await _mediator.Send(new GetListSyaSyuQuery() { TenantCdSeq = tenantCdSeq });
        }

        public async Task<ReservationMobileData> GetReservationData(int ukeCd, int tenantCdSeq)
        {
            return await _mediator.Send(new GetReservationDataQuery() { UkeCd = ukeCd, TenantCdSeq = tenantCdSeq });
        }

        public async Task<List<ReservationMobileChildItemData>> GetListChildItem(int ukeCd, int tenantCdSeq)
        {
            return await _mediator.Send(new GetListChildItemQuery() { UkeCd = ukeCd, TenantCdSeq = tenantCdSeq });
        }

        public async Task<int> GetSyaSyuCdSeq(int syaRyoCdSeq)
        {
            return await _mediator.Send(new GetSyaSyuCdSeqQuery() { SyaRyoCdSeq = syaRyoCdSeq });
        }

        public async Task<string> InsertReservation(ReservationMobileData Data, int CompanyCdSeq, int SyaRyoCdSeq, int TenantCdSeq, int SyainCdSeq, int EigyoCdSeq)
        {
            return await _mediator.Send(new RegisterReservationCommand() { Data = Data, CompanyCdSeq = CompanyCdSeq, EigyoCdSeq = EigyoCdSeq, SyainCdSeq = SyainCdSeq, SyaRyoCdSeq = SyaRyoCdSeq, TenantCdSeq = TenantCdSeq });
        }

        public async Task<bool> UpdateReservation(ReservationMobileData Data, List<ReservationMobileChildItemData> ListDelete, int CompanyCdSeq, int TenantCdSeq, int SyainCdSeq)
        {
            return await _mediator.Send(new UpdateReservationCommand() { Data = Data, ListDelete = ListDelete, CompanyCdSeq = CompanyCdSeq, TenantCdSeq = TenantCdSeq, SyainCdSeq = SyainCdSeq });
        }
    }
}
