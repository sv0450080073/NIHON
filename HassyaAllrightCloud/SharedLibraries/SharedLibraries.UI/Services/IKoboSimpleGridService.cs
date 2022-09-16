using Microsoft.JSInterop;
using Newtonsoft.Json;
using SharedLibraries.UI.Models;
using System.Threading.Tasks;

namespace SharedLibraries.UI.Services
{
    public interface IKoboSimpleGridService
    {
        Task<(HeaderTemplate, BodyTemplate)> InitGrid(IJSRuntime jsRuntime, string id, HeaderTemplate header, BodyTemplate body);
        Task SaveGrid(IJSRuntime jsRuntime, string id, HeaderTemplate header, BodyTemplate body);
    }
    public class KoboSimpleGridService : IKoboSimpleGridService
    {
        public async Task<(HeaderTemplate, BodyTemplate)> InitGrid(IJSRuntime jsRuntime, string id, HeaderTemplate header, BodyTemplate body)
        {
            var gridLayout = await jsRuntime.InvokeAsync<string>("KoboGrid.getGridLayout", id);
            if (gridLayout != null)
            {
                var grid = JsonConvert.DeserializeObject<GridTemplate>(gridLayout);
                grid.Body.CustomCssDelegate = body.CustomCssDelegate;
                var i = 0;
                foreach(var row in grid.Body.Rows)
                {
                    var j = 0;
                    foreach(var col in row.Columns)
                    {
                        col.CustomTextFormatDelegate = body.Rows[i].Columns[j].CustomTextFormatDelegate;
                        j++;
                    }
                    i++;
                }
                return (grid.Header, grid.Body);
            }
            else
            {
                return (header, body);
            }
        }

        public async Task SaveGrid(IJSRuntime jsRuntime, string id, HeaderTemplate header, BodyTemplate body)
        {
            await jsRuntime.InvokeAsync<string>("KoboGrid.saveGridLayout", id, JsonConvert.SerializeObject(new GridTemplate() { Header = header, Body = body }));
        }
    }
}
