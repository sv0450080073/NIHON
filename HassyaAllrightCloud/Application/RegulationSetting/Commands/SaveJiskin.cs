using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;

namespace HassyaAllrightCloud.Application.RegulationSetting.Commands
{
    public class SaveJiskin : IRequest<bool>
    {
        public byte JiskinCd { get; set; }
        public int CompanyCdSeq { get; set; }
        public List<WorkHolidayType> WorkHolidayTypes { get; set; }
        public class Handler : IRequestHandler<SaveJiskin, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<bool> Handle(SaveJiskin request, CancellationToken cancellationToken)
            {
                var existJiskin = _context.TkmJisKin.Where(x => x.JisKinKyuCd == request.JiskinCd).ToList();
                if(existJiskin.Count == 0 && request.WorkHolidayTypes.Count > 0)
                {
                    foreach(var item in request.WorkHolidayTypes)
                    {
                        _context.TkmJisKin.Add(new Domain.Entities.TkmJisKin() { 
                            CompanyCdSeq = request.CompanyCdSeq,
                            JisKinKyuCd = request.JiskinCd,
                            KinKyuCdSeq = item.KinKyuCdSeq,
                            UpdPrgId = "",
                            UpdSyainCd = new ClaimModel().SyainCdSeq,
                            UpdTime = DateTime.Now.ToString("hhmmss"),
                            UpdYmd = DateTime.Now.ToString("yyyyMMdd")
                        });
                    }
                }
                else
                {
                    List<TkmJisKin> removeds = _context.TkmJisKin.Where(x => x.JisKinKyuCd == request.JiskinCd).ToList();
                    _context.TkmJisKin.RemoveRange(removeds);
                    foreach (var item in request.WorkHolidayTypes)
                    {
                        _context.TkmJisKin.Add(new Domain.Entities.TkmJisKin()
                        {
                            CompanyCdSeq = request.CompanyCdSeq,
                            JisKinKyuCd = request.JiskinCd,
                            KinKyuCdSeq = item.KinKyuCdSeq,
                            UpdPrgId = "",
                            UpdSyainCd = new ClaimModel().SyainCdSeq,
                            UpdTime = DateTime.Now.ToString("hhmmss"),
                            UpdYmd = DateTime.Now.ToString("yyyyMMdd")
                        });
                    }
                }
                _context.SaveChanges();

                return true;
            }
        }
    }
}
