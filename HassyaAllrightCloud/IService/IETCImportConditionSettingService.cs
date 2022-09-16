using HassyaAllrightCloud.Application.ETCImportConditionSetting.Commands;
using HassyaAllrightCloud.Application.ETCImportConditionSetting.Queries;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace HassyaAllrightCloud.IService
{
    public interface IETCImportConditionSettingService
    {
        /// <summary>
        /// Get Setting from TPM_EtcCsv.xml
        /// </summary>
        /// <param name="wh"></param>
        /// <returns></returns>
        ETCSettingModel GetETCSetting(IWebHostEnvironment wh);
        /// <summary>
        /// Check if ETC import exist or not
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> IsETCImportExist(ETCImportSearchModel model);
        /// <summary>
        /// Check if Ryokin exist or not 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> IsRyokinExist(RyokinSearchModel model);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<List<RyokinSplitModel>> GetRyokinSplit(RyokinSplitSearchModel model);
        Task<string> GetFutaiNmByFutaiCdSeq(int cdSeq);
        Task<bool> InsertOrUpdateEtcImport(TkdEtcImport model, bool isInsert);
        Task<bool> InsertRyokin(VpmRyokin model);
    }

    public class ETCImportConditionSettingService : IETCImportConditionSettingService
    {
        private IMediator _mediator;
        public ETCImportConditionSettingService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public ETCSettingModel GetETCSetting(IWebHostEnvironment wh)
        {
            var filePath = $"{wh.WebRootPath}/xml/TPM_EtcCsv.xml";
            var setting = new ETCSettingModel();
            using (XmlReader reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        //return only when you have START tag  
                        switch (reader.Name.ToString())
                        {
                            case "EtcCsv_SyaRyoCd":
                                setting.SyaRyoCdCol = int.Parse(reader.ReadString());
                                break;
                            case "EtcCsv_CardNo":
                                setting.CardNoCol = int.Parse(reader.ReadString());
                                break;
                            case "EtcCsv_UnkYmd":
                                setting.UnkYmdCol = int.Parse(reader.ReadString());
                                break;
                            case "EtcCsv_UnkTime":
                                setting.UnkTimeCol = int.Parse(reader.ReadString());
                                break;
                            case "EtcCsv_Tanka":
                                setting.TankaCol = int.Parse(reader.ReadString());
                                break;
                            case "EtcCsv_IriRyoKin":
                                setting.IriRyoKinCol = int.Parse(reader.ReadString());
                                break;
                            case "EtcCsv_DeRyoKin":
                                setting.DeRyoKinCol = int.Parse(reader.ReadString());
                                break;
                            case "EtcCsv_JigyosIri":
                                setting.JigyosIriCol = int.Parse(reader.ReadString());
                                break;
                            case "EtcCsv_JigyosDe":
                                setting.JigyosDeCol = int.Parse(reader.ReadString());
                                break;
                            case "EtcCsv_KinEtc":
                                setting.KinEtcVal = int.Parse(reader.ReadString());
                                break;
                            case "EtcCsv_FutEtc":
                                setting.FutEtcVal = int.Parse(reader.ReadString());
                                break;
                            case "EtcCsv_TateOutPutFlag":
                                setting.TateOutPutFlagVal = int.Parse(reader.ReadString());
                                break;
                            case "EtcCsv_TateOutPutChoice":
                                setting.TateOutPutChoiceVal = int.Parse(reader.ReadString());
                                break;
                        }
                    }
                }
            }
            return setting;
        }

        public async Task<string> GetFutaiNmByFutaiCdSeq(int cdSeq)
        {
            return await _mediator.Send(new GetFutaiNmByFutaiCdSeq() { CdSeq = cdSeq });
        }

        public async Task<bool> IsETCImportExist(ETCImportSearchModel model)
        {
            return await _mediator.Send(new IsETCImportExist() { Model = model });
        }

        public async Task<bool> IsRyokinExist(RyokinSearchModel model)
        {
            return await _mediator.Send(new IsRyokinExist() { Model = model });
        }

        public async Task<List<RyokinSplitModel>> GetRyokinSplit(RyokinSplitSearchModel model)
        {
            return await _mediator.Send(new GetRyokinSplit() { Model = model });
        }

        public async Task<bool> InsertOrUpdateEtcImport(TkdEtcImport model, bool isInsert)
        {
            if (model is null)
            {
                throw new System.ArgumentNullException(nameof(model));
            }

            return await _mediator.Send(new InsertOrUpdateEtcImport() { Model = model, IsInsert = isInsert });
        }

        public async Task<bool> InsertRyokin(VpmRyokin model)
        {
            if (model is null)
            {
                throw new System.ArgumentNullException(nameof(model));
            }

            return await _mediator.Send(new InsertRyokin() { Model = model });
        }
    }
}
