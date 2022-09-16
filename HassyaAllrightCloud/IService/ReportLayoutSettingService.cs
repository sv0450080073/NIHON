using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.ReportLayout.Queries;
using HassyaAllrightCloud.Commons.Constants;
using MediatR;
using System;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IReportLayoutSettingService
    {
        Task<XtraReport> GetCurrentTemplate(ReportIdForSetting ReportId, string ReportTemplateNameSpace, int TenantCdSeq, int EigyouCdSeq, byte PaperSize);
    }

    public class ReportLayoutSettingService : IReportLayoutSettingService
    {
        private IMediator mediatR;
        public ReportLayoutSettingService(IMediator mediatR)
        {
            this.mediatR = mediatR;
        }

        public async Task<XtraReport> GetCurrentTemplate(ReportIdForSetting ReportId, string ReportTemplateNameSpace, int TenantCdSeq, int EigyouCdSeq, byte paperSize)
        {
            int CurrentTemplateId =  await mediatR.Send(new GetReportCurrentTemplateQuery { TenantCdSeq = TenantCdSeq, ReportId = ReportId, EigyouCdSeq = EigyouCdSeq });
            string ReportClassName = BaseNamespace.Report + ReportTemplateNameSpace + CurrentTemplateId;
            if (paperSize == (byte)PaperSize.A4)
            {
                ReportClassName += PageSizeName.A4;
            }
            else if (paperSize == (byte)PaperSize.A3)
            {
                ReportClassName += PageSizeName.A3;
            }
            else
            {
                ReportClassName += PageSizeName.B4;
            }
            return (XtraReport)Activator.CreateInstance(Type.GetType(ReportClassName));
        }
    }
}
