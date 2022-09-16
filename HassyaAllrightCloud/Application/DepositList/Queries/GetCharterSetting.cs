using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using System.Threading;
using HassyaAllrightCloud.Infrastructure.Persistence;

namespace HassyaAllrightCloud.Application.DepositList.Queries
{
    public class GetCharterSetting : IRequest<CharterSettingModel>
    {
        public int CompanyCdSeq { get; set; }
        public class Handler : IRequestHandler<GetCharterSetting, CharterSettingModel>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;

            public async Task<CharterSettingModel> Handle(GetCharterSetting request, CancellationToken cancellationToken)
            {
                return (from com in _dbContext.VpmCompny
                        join kas in _dbContext.TkmKasSet
                        on com.CompanyCdSeq equals kas.CompanyCdSeq into comkas
                        from comkasTemp in comkas
                        join kyo in _dbContext.VpmKyoSet
                        on new { key1 = "001" } equals new { key1 = kyo.SetteiCd } into comkaskyo
                        from comkaskyoTemp in comkaskyo
                        where com.CompanyCdSeq == request.CompanyCdSeq
                        select new CharterSettingModel()
                        {
                            SyoriYm = com.SyoriYm,
                            SeiKrksKbn = comkasTemp.SeiKrksKbn,
                            NyuSEtcNm1 = comkaskyoTemp.NyuSetcNm1,
                            NyuSEtcNm2 = comkaskyoTemp.NyuSetcNm2,
                            NyuSyoNm11 = comkaskyoTemp.NyuSyoNm11,
                            NyuSyoNm12 = comkaskyoTemp.NyuSyoNm12,
                            NyuSyoNm21 = comkaskyoTemp.NyuSyoNm21,
                            NyuSyoNm22 = comkaskyoTemp.NyuSyoNm22
                        }).FirstOrDefault();
            }
        }
    }
}
