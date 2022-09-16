using StoredProcedureEFCore;
using System.Linq;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System.Threading.Tasks;
using System.Collections.Generic;
using SharedLibraries.UI.Models;
using System.Threading;

namespace HassyaAllrightCloud.Application.KoboMenu.Queries
{
    public class GetMenuItems : IRequest<List<KoboMenuItemModel>>
    {
        private int _syainCdSeq { get; set; }
        private int _tenantCdSeq { get; set; }
        private int _serviceCdSeq { get; set; }
        private string _langCode { get; set; }
        public GetMenuItems(int syainCdSeq, int tenantCdSeq, int serviceCdSeq, string langCode)
        {
            _syainCdSeq = syainCdSeq;
            _tenantCdSeq = tenantCdSeq;
            _serviceCdSeq = serviceCdSeq;
            _langCode = langCode;
        }
        public class Handler : IRequestHandler<GetMenuItems, List<KoboMenuItemModel>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<KoboMenuItemModel>> Handle(GetMenuItems request, CancellationToken cancellationToken)
            {
                var menuItems = new List<KoboMenuItemModel>();
                await _context.LoadStoredProc("PK_MenuItems_R")
                        .AddParam("@SyainCdSeq", request._syainCdSeq)
                        .AddParam("@TenantCdSeq", request._tenantCdSeq)
                        .AddParam("@ServiceCdSeq", request._serviceCdSeq)
                        .AddParam("@LangCode", request._langCode)
                        .ExecAsync(async e => menuItems = await e.ToListAsync<KoboMenuItemModel>());
                var result = menuItems.Where(e => e.ParentMenuCdSeq == 0);
                foreach (var i in result)
                {
                    BuildTree(menuItems, i);
                }

                return result.ToList();
            }

            protected void BuildTree(List<KoboMenuItemModel> nodes, KoboMenuItemModel parentNode)
            {
                foreach (var node in nodes)
                {
                    if (node.ParentMenuCdSeq == parentNode.MenuCdSeq)
                    {
                        if (parentNode.Children == null)
                            parentNode.Children = new List<KoboMenuItemModel>();
                        parentNode.Children.Add(node);
                        BuildTree(nodes, node);
                    }
                }
            }
        }
    }
}
