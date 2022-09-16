using HassyaAllrightCloud.Application.UpdateBookingInput.Queries;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BookingInputData;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IUpdateBookingInputService
    {
        Task<List<HaishaInfoData>> GetHaishaInfoDatas(string UkeNo, short UnkRen);
    }
    public class UpdateBookingInputService : IUpdateBookingInputService
    {
        private IMediator _mediatR;
        public UpdateBookingInputService(IMediator MediatR)
        {
            _mediatR = MediatR;
        }
        public async Task<List<HaishaInfoData>> GetHaishaInfoDatas(string UkeNo, short UnkRen)
        {
            try
            {
                List<HaishaInfoData> result = new List<HaishaInfoData>();
                result = await _mediatR.Send(new GetHaishaInfoDataQuery { Ukeno = UkeNo, Unkren = UnkRen });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
