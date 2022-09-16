using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages
{
    public class HomeMobileBase : ComponentBase
    {
        [Inject]
        protected IStringLocalizer<HomeMobile> Lang { get; set; }
        [Inject]
        public INoticeService noticeService { get; set; }
        [Inject] private ILoadingService loading { get; set; }
        [Inject]
        protected IErrorHandlerService errorModalService { get; set; }

        public const int FirstDisplayQuantity = 10;
        public const int NextPageQuantity = 5;

        public int SyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
        public List<Tkd_NoticeListDto> notices { get; set; } = new List<Tkd_NoticeListDto>();
        public List<Tkd_NoticeListDto> noticesDisplay { get; set; } = new List<Tkd_NoticeListDto>();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

        }
        protected override async Task OnInitializedAsync()
        {
            try
            {

            } catch(Exception ex)
            {
                errorModalService.HandleError(ex);
            }
            await loading.ShowAsync();
            notices = (await noticeService.GetNoticeList()).ToList();
            noticesDisplay = notices.Take(FirstDisplayQuantity).ToList();
            await loading.HideAsync();
        }

        public void UpdateFormValue(Tkd_NoticeListDto item, string propertyName, ChangeEventArgs value)
        {
            try
            {
                var propertyInfo = item.GetType().GetProperty(propertyName);
                propertyInfo.SetValue(item, (string)value.Value, null);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async Task SaveNotice(Tkd_NoticeListDto item)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(item.NoticeContent))
                {
                    return;
                }
                Tkd_NoticeDto tkd_NoticeDto = new Tkd_NoticeDto()
                {
                    NoticeCdSeq = item.NoticeCdSeq,
                    NoticeContent = item.NoticeContent.Replace("\n", Environment.NewLine),
                    NoticeDisplayKbn = item.NoticeDisplayKbn,
                    SiyoKbn = item.SiyoKbn,
                    SyainNm = item.SyainNm,
                    UpdPrgId = Common.UpdPrgId,
                    UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                    UpdTime = DateTime.Now.ToString(Formats.HHmmss),
                    UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd)
                };
                var result = await noticeService.Update(tkd_NoticeDto);
                item.NoticeContent = result.NoticeContent;
                item.UpdPrgId = result.UpdPrgId;
                item.UpdSyainCd = result.UpdSyainCd;
                item.UpdTime = string.IsNullOrWhiteSpace(result.UpdTime) ? string.Empty : result.UpdTime.Substring(0, 4).Insert(2, ":");
                item.UpdYmd = string.IsNullOrWhiteSpace(result.UpdYmd) ? string.Empty : result.UpdYmd.Insert(2, "/").Insert(5, "/");
                item.isEdit = false;
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public void Edit(bool edit, Tkd_NoticeListDto item = null)
        {
            try
            {
                if (!edit)
                {
                    item = noticesDisplay.Where(x => x.NoticeCdSeq == item.NoticeCdSeq).FirstOrDefault();
                }
                item.isEdit = edit;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
    }
}
