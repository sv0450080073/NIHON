using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Validation
{
    public class HomeValidator
    {
    }

    public class MenuPopupValidator : AbstractValidator<TKD_FavoriteMenuData>
    {
        public MenuPopupValidator()
        {
            RuleFor(t => t.FavoriteMenu_MenuUrl).NotEmpty().WithMessage(Constants.ErrorMessage.MenuLinkEmpty);
            RuleFor(t => t.FavoriteMenu_MenuTitle).NotEmpty().WithMessage(Constants.ErrorMessage.MenuContentEmpty);
            RuleFor(t => t.FavoriteMenu_MenuUrl).MaximumLength(250).WithMessage(Constants.ErrorMessage.MenuLinkMaxLength);
            RuleFor(t => t.FavoriteMenu_MenuTitle).MaximumLength(50).WithMessage(Constants.ErrorMessage.MenuContentMaxLength);
        }
    }

    public class SitePopupValidator : AbstractValidator<TKD_FavoriteSiteData>
    {
        public SitePopupValidator()
        {
            RuleFor(t => t.FavoriteSite_SiteUrl).NotEmpty().WithMessage(Constants.ErrorMessage.SiteLinkEmpty);
            RuleFor(t => t.FavoriteSite_SiteTitle).NotEmpty().WithMessage(Constants.ErrorMessage.SiteContentEmpty);
            RuleFor(t => t.FavoriteSite_SiteUrl).MaximumLength(250).WithMessage(Constants.ErrorMessage.SiteLinkMaxLength);
            RuleFor(t => t.FavoriteSite_SiteTitle).MaximumLength(50).WithMessage(Constants.ErrorMessage.SiteContentMaxLength);
        }
    }

    public class NoteFormValidator : AbstractValidator<TKD_PersonalNoteData>
    {
        public NoteFormValidator()
        {
            RuleFor(t => t.PersonalNote_Note).NotEmpty().WithMessage(Constants.ErrorMessage.NoteContentEmpty);
        }
    }
    public class NoticeFormValidator : AbstractValidator<Tkd_NoticeDto>
    {
        public NoticeFormValidator()
        {
            RuleFor(t => t.NoticeDisplayKbn).NotEmpty().WithMessage(Constants.ErrorMessage.NoticeDisplayKbnEmpty);
            RuleFor(t => t.NoticeContent).NotEmpty().WithMessage(Constants.ErrorMessage.NoticeContentEmpty);
        }
    }
}
