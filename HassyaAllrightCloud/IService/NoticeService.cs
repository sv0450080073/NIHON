using HassyaAllrightCloud.Application.Notice.Commands;
using HassyaAllrightCloud.Application.Notice.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface INoticeService
    {
        Task<IEnumerable<Tkd_NoticeListDto>> GetNoticeList();
        Task<IEnumerable<NoticeDisplayKbnDto>> GetNoticeDisplayKbnList();
        Task<Tkd_NoticeDto> GetNoticeById(int id);
        Task<bool> Save(Tkd_NoticeDto dto);
        Task<TkdNotice> Update(Tkd_NoticeDto dto);

        Task<bool> DeleteNotice(int id);
    }

    public class NoticeService : INoticeService
    {
        private IMediator mediator;

        public NoticeService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task<Tkd_NoticeDto> GetNoticeById(int id)
        {
            return mediator.Send(new GetNoticeByIdQuery { Id = id });
        }

        public async Task<IEnumerable<NoticeDisplayKbnDto>> GetNoticeDisplayKbnList()
        {
            return await mediator.Send(new GetNoticeDisplayKbnListQuery());
        }

        public async Task<IEnumerable<Tkd_NoticeListDto>> GetNoticeList()
        {
            return await mediator.Send(new GetNoticeListQuery());
        }

        public async Task<bool> Save(Tkd_NoticeDto dto)
        {
            return await mediator.Send(new SaveNoticeCommand { NoticeDto = dto });
        }

        public async Task<TkdNotice> Update(Tkd_NoticeDto dto)
        {
            return await mediator.Send(new UpdateNoticeCommand { NoticeDto = dto });
        }

        public async Task<bool> DeleteNotice(int id)
        {
            return await mediator.Send(new DeleteNoticeCommand { Id = id });
        }
    }
}
