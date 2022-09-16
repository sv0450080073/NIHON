using Blazor.DragDrop.Core;
using Amazon;
using Amazon.S3;
using Blazored.LocalStorage;
using DevExpress.AspNetCore;
using DevExpress.Blazor.Reporting;
using DevExpress.XtraReports.Web.Extensions;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.Infrastructure.Services;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.Reports.ReportFactory;
using HassyaAllrightCloud.Routing;
using LexLibrary.Line.NotifyBot;
using LexLibrary.Line.NotifyBot.Models;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Reflection;
using DevExpress.Blazor.Reporting;
using DevExpress.XtraReports.Web.Extensions;
using HassyaAllrightCloud.Reports.ReportFactory;
using LexLibrary.Line.NotifyBot;
using LexLibrary.Line.NotifyBot.Models;
using Blazored.LocalStorage;
using DevExpress.AspNetCore;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using HassyaAllrightCloud.Infrastructure.Middleware;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Server;
using BlazorDownloadFile;
using Microsoft.AspNetCore.HttpOverrides;
using Blazored.Modal;
using SharedLibraries.UI.Extensions;
using Radzen;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using HassyaAllrightCloud.IService.RegulationSetting;
using SharedLibraries.UI.Services;
using HassyaAllrightCloud.Application.HikiukeshoReport;
using HassyaAllrightCloud.IService.CommonComponents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace HassyaAllrightCloud
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<KobodbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SqlDbContext")), ServiceLifetime.Transient);
            //services.AddDbContext<KobodbContext>(options =>
            //{
            //    options.UseSqlServer(Configuration.GetConnectionString("SqlDbContext"));
            //    options.UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); }));
            //}, ServiceLifetime.Transient);

            // Add this
            services.ConfigureNonBreakingSameSiteCookies();

            services.AddServerSideBlazor().AddCircuitOptions(o => o.DetailedErrors = true);

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultAuthenticateScheme =
                     CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignInScheme =
                    CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme =
                   OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = Configuration.GetValue<string>("ServiceUrls:SSO");
                options.ClientId = Configuration.GetValue<string>("MySettings:ClientId");
                options.ClientSecret = Configuration.GetValue<string>("MySettings:ClientSecret");
                options.ResponseType = "code";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("UserInfo");
                options.ClaimActions.MapUniqueJsonKey("TenantCdSeq", "TenantCdSeq");
                options.ClaimActions.MapUniqueJsonKey("SyainCdSeq", "SyainCdSeq");
                options.ClaimActions.MapUniqueJsonKey("CompanyId", "CompanyId");
                options.ClaimActions.MapUniqueJsonKey("EigyoCdSeq", "EigyoCdSeq");
                options.ClaimActions.MapUniqueJsonKey("SyainCd", "SyainCd");
                options.ClaimActions.MapUniqueJsonKey("Name", "Name");
                options.ClaimActions.MapUniqueJsonKey("UserName", "UserName");
                options.UseTokenLifetime = false;
                options.RequireHttpsMetadata = false;
                options.Events = new OpenIdConnectEvents
                {
                    OnAccessDenied = context =>
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/");
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddMvcCore(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddDevExpressBlazor();
            services.AddHttpClient();
            services.AddBlazoredLocalStorage();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddHttpContextAccessor();
            services.AddScoped<HttpContextAccessor>();
            services.AddBlazoredModal();

            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<TooltipService>();
            services.AddScoped<ContextMenuService>();
            //services.AddSingleton<CustomHttpClient>();
            services.AddHttpClient<CustomHttpClient>(async (serviceProvider, client) =>
            {
                var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();

                var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var cookies = httpContextAccessor.HttpContext.Request.Cookies.Keys;
                foreach (var key in cookies)
                {
                    client.DefaultRequestHeaders.Add("Cookie", $"{key}={httpContextAccessor.HttpContext.Request.Cookies[key]}");
                }
            });
            services.AddScoped<AppSettingsService>();
            services.AddBlazorDragDrop();
            services.AddDevExpressBlazorReporting();
            services.AddDevExpressControls();
            //services.AddScoped<ReportStorageWebExtension, ReportStorageWebExtension1>();
            services.AddScoped<ReportStorageWebExtension, KashikiriReportStorageWebExtension>();
            services.AddSingleton<IKashikiryReportSource, KashikiriReportSourceFactory>();
            services.AddTransient<IWebDocumentViewerReportResolver, CustomWebDocumentViewerReportResolver>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            var supportedCultures = new List<CultureInfo> { new CultureInfo("en-US"), new CultureInfo("ja-JP"), new CultureInfo("vi-VN") };
            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    //options.SetDefaultCulture("en");
                    //options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("ja-JP");
                    options.SetDefaultCulture("ja-JP");
                    options.SupportedUICultures = supportedCultures;
                    options.AddInitialRequestCultureProvider(new SyainCultureProvider());
                });

            services.AddSingleton<BookingInputHelper>();
            services.AddSingleton<BusScheduleHelper>();

            services.AddTransient<IBookingTypeListService, BookingTypeService>();
            services.AddTransient<IScheduleManageService, ScheduleManageService>();
            services.AddTransient<ISaleBranchListService, SaleBranchService>();
            services.AddTransient<IStaffListService, StaffService>();
            services.AddTransient<ICustomerListService, CustomerService>();
            services.AddTransient<ITPM_CodeKbListService, TPM_CodeKbService>();
            services.AddTransient<ITPM_CodeSyService, TPM_CodeSyService>();
            services.AddTransient<ICustomerCLassificationListService, CustomerCLassificationService>();
            services.AddTransient<ILocationListService, LocationService>();
            services.AddTransient<IDispatchListService, DispatchService>();
            services.AddTransient<ITranSportationFreeRuleService, TranSportationFreeRuleService>();
            services.AddTransient<IBusTypeListService, BusTypeService>();
            services.AddTransient<ITKD_YykshoListService, TKD_YykshoService>();
            services.AddTransient<ITKD_YykSyuListService, TKD_YykSyuService>();
            services.AddTransient<ITKD_UnkobiDataListService, TKD_UnkobiDataService>();
            services.AddTransient<ITKM_KasSetDataListService, TKM_KasSetDataService>();
            services.AddTransient<TKD_MishumDataListService, TKD_MishumDataService>();
            services.AddTransient<ITKD_HaishaDataListService, TKD_HaishaDataService>();
            services.AddTransient<IVPM_SyaRyoListService, VPM_SyaRyoService>();
            services.AddTransient<IHyperDataService, HyperDataService>();
            services.AddTransient<IStaffScheduleService, StaffScheduleService>();
            services.AddTransient<IServiceOfficeService, ServiceOfficeService>();
            services.AddTransient<ILeaveDayTypeService, LeaveDayTypeService>();
            services.AddTransient<IDispatchScheduleService, DispatchScheduleService>();
            services.AddTransient<ILeaveDayService, LeaveDayService>();
            services.AddTransient<IScheduleCustomGroupService, ScheduleCustomGroupService>();
            services.AddTransient<IDisplaySettingService, DisplaySettingService>();
            services.AddTransient<IBusLineCssColorServices, BusLineCssColorServices>();
            services.AddTransient<IReceiptOutputService, ReceiptOutputService>();
            services.AddTransient<IFaresUpperAndLowerLimitsService, FaresUpperAndLowerLimitsService>();
            services.AddTransient<IFareFeeCorrectionService, FareFeeCorrectionService>();

            services.AddSingleton<BusScheduleHelper>();
            services.AddTransient<IStatusConfirmationService, StatusConfirmationService>();
            services.AddTransient<ISubContractorStatusReportService, SubContractorStatusReportService>();
            services.AddTransient<IVenderRequestService, VenderRequestService>();
            services.AddTransient<ICancelListReportService, CancelListReportService>();
            services.AddTransient<IAccessoryFeeListReportService, AccessoryFeeListReportService>();
            services.AddTransient<ITPM_YoyKbnDataListService, TPM_YoyKbnDataList>();
            services.AddTransient<ITPM_CompnyDataListService, TPM_CompnyDataService>();
            services.AddTransient<ITPM_EigyosDataListService, TPM_EigyosDataService>();
            services.AddTransient<IBusDataListService, BusDataService>();
            services.AddTransient<IBusBookingDataListService, BusBookingDataService>();
            services.AddTransient<ILoadBusTooltipListService, BusTooltipService>();
            services.AddTransient<ITPM_TokiskDataListService, TPM_TokiskDataService>();
            services.AddTransient<ITKD_YoshaDataListService, TKD_YoshaDataService>();
            services.AddTransient<ITKD_ShuriDataListService, TKD_ShuriDataService>();
            services.AddTransient<ITPM_CodeKbListService, TPM_CodeKbService>();
            services.AddTransient<ITPM_KyoSHeDataListService, TPM_KyoSHeDataService>();
            services.AddTransient<ITKD_KakninDataListService, TKD_KakninDataService>();
            services.AddTransient<ITPM_CalendDataListService, TPM_CalendDataService>();
            services.AddTransient<ITPM_SyokumDataListService, TPM_SyokumDataService>();
            services.AddTransient<ITKD_KoteiDataListService, TKD_KoteiDataService>();
            services.AddTransient<ISupplierService, SupplierService>();
            services.AddTransient<IConfirmationTabService, ConfirmationTabService>();
            services.AddTransient<ITKD_FavoriteMenuService, TKD_FavoriteMenuService>();
            services.AddTransient<ITKD_FavoriteSiteService, TKD_FavoriteSiteService>();
            services.AddTransient<ITKD_PersonalNoteDataService, TKD_PersonalNoteDataService>();
            services.AddTransient<INoticeService, NoticeService>();
            services.AddTransient<ITransportationSummaryService, TransportationSummaryService>();
            services.AddTransient<ITenkokirokuReportService, TenkokirokuReportService>();
            services.AddTransient<IUnkoushijishoReportService, UnkoushijishoReportService>();
            services.AddTransient<IMonthlyTransportationService, MonthlyTransportationService>();
            services.AddTransient<IMonthlyTransportationAnnualService, MonthlyTransportationAnnualService>();
            services.AddTransient<IHikiukeshoReportService, HikiukeshoReportService>();
            services.AddTransient<IVehicleDailyReportService, VehicleDailyReportService>();
            services.AddTransient<ILineService, LineService>();
            services.AddTransient<IBillCheckListService, BillCheckListService>();
            services.AddTransient<ITKD_KoteikDataListService, TKD_KoteikDataService>();
            services.AddTransient<IScheduleGroupDataService, ScheduleGroupDataService>();
            services.AddTransient<IDepositCouponService, DepositCouponService>();
            services.AddTransient<IRevenueSummaryService, RevenueSummaryService>();
            services.AddSingleton<IReportLoadingService, ReportLoadingService>();
            services.AddTransient<IFieldValueFilterCondition, FieldValueFilterCondition>();
            services.AddTransient<INotificationToStaffService, NotificationToStaffService>();
            services.AddTransient<INotificationTemplateService, NotificationTemplateService>();

            services.AddTransient<IDepositListService, DepositListService>();

            services.AddTransient<IReportLayoutSettingService, ReportLayoutSettingService>();
            services.AddTransient<IETCImportConditionSettingService, ETCImportConditionSettingService>();
            services.AddTransient<IBusReportService, BusReportService>();
            services.AddTransient<IFilterCondition, FilterCondition>();
            services.AddTransient<IGenerateFilterValueDictionary, GenerateFilterValueDictionary>();
            services.AddTransient<IGetFilterDataService, GetFilterDataService>();
            services.AddTransient<IGridLayoutService, GridLayoutService>();
            services.AddTransient<ICustomItemService, CustomItemService>();
            services.AddTransient<IAttendanceReportService, AttendanceReportService>();
            services.AddTransient<IReceivableListService, ReceivableListService>();
            services.AddTransient<IVehicleAvailabilityConfirmationMobileService, VehicleAvailabilityConfirmationMobileService>();
            services.AddTransient<IGeneralOutPutService, GeneralOutPutService>();
            services.AddTransient<IBatchKobanInputService, BatchKobanInputService>();

            services.AddTransient<IBusReportService, BusReportService>();
            services.AddTransient<ITransportActualResultService, TransportActualResultService>();
            services.AddTransient<ITransportDailyReportService, TransportDailyReportService>();
            services.AddTransient<IBusCoordinationReportService, BusCoordinationReportService>();
            services.AddTransient<IPartnerBookingInputService, PartnerBookingInputService>();
            services.AddTransient<IRoundSettingsService, RoundSettingsService>();
            services.AddTransient<IStaffsChartService, StaffsChartService>();
            services.AddTransient<ITKD_KikyujDataListService, TKD_KikyujDataService>();
            services.AddTransient<IVPM_KinKyuDataService, VPM_KinKyuDataService>();
            services.AddTransient<ITKD_KobanDataService, TKD_KobanDataService>();
            services.AddTransient<IAdvancePaymentDetailsService, AdvancePaymentDetailsService>();
            services.AddTransient<IETCService, ETCService>();
            services.AddTransient<IInvoiceIssueReleaseService, InvoiceIssueReleaseService>();
            services.AddTransient<IBusAllocationService, BusAllocationService>();
            services.AddTransient<IVPM_SyuTaikinCalculationTimeService, VPM_SyuTaikinCalculationTimeService>();
            services.AddTransient<ITKD_HaiinDataService, TKD_HaiinDataService>();
            services.AddTransient<IBillingListService, BillingListService>();
            services.AddTransient<IVehicleStatisticsSurveyService, VehicleStatisticsSurveyService>();
            services.AddTransient<IDailyBatchCopyService, DailyBatchCopyService>();
            services.AddScoped<BrowserResizeService>();
            services.AddScoped<ILoadingService, LoadingService>();
            services.AddTransient<ICouponPaymentService, CouponPaymentService>();
            services.AddSingleton<RouteManager>();
            services.AddTransient<IETCFormService, ETCFormService>();
            services.AddScoped<ETCDataTranferService>();
            services.AddTransient<ITKD_MihrimService, TKD_MihrimService>();
            services.AddTransient<ITKD_UnkobiFileService, TKD_UnkobiFileService>();
            services.AddAWSService<IAmazonS3>();
            services.AddScoped<IAwsS3FileManager, AwsS3FileManager>();
            services.AddScoped<IFileHandler, FileHandler>();
            services.AddTransient<IAlertSettingService, AlertSettingService>();
            services.AddScoped<IErrorHandlerService, ErrorHandlerService>();
            services.AddTransient<IBusTypeListReportService, BusTypeListReportService>();
            services.AddScoped<ICarCooperationListService, CarCooperationService>();
            services.AddScoped<ITenantGroupServiceService, TenantGroupService>();
            services.AddTransient<IVehicleSchedulerMobileService, VehicleSchedulerMobileService>();
            services.AddTransient<IEditReservationMobileService, EditReservationMobileService>();
            services.AddScoped<ISimpleQuotationService, SimpleQuotationService>();
            services.AddTransient<IRepairDivisionService, RepairDivisionService>();
            services.AddTransient<IRepairListReportService, RepairListReportService>();
            services.AddTransient<IAvailabilityCheckService, AvailabilityCheckService>();
            services.AddTransient<IFutaiService, FutaiService>();
            services.AddTransient<IHaitaCheckService, HaitaCheckService>();
            services.AddTransient<ITehaiService, TehaiService>();
            services.AddTransient<IRegulationSettingService, RegulationSettingService>();
            services.AddTransient<HikiukeshoHelper>();
            services.AddTransient<IUpdateBookingInputService, UpdateBookingInputService>();
            services.AddTransient<IBusScheduleService, BusScheduleService>();
            services.AddTransient<IKoboMenuService, MenuService>();
            services.AddTransient<ICustomerComponentService, CustomerComponentService>();
            services.AddTransient<IReservationClassComponentService, ReservationClassComponentService>();
            services.AddTransient<ITPM_TenantDataService, TPM_TenantDataService>();
            services.AddLibServices();
            services.AddScoped<HttpClient>((sp) =>
            {
                var httpClient = sp.GetService<IHttpClientFactory>().CreateClient();
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                return httpClient;
            });
            services.AddTransient<IBillPrintService, BillPrintService>();

            services.AddLineNotifyBot(new LineNotifyBotSetting
            {
                ClientID = "7cqzz9qNxkPQRh46zXljpW",
                ClientSecret = "RnvGMoj1vCixcufilZssFi9UkpHyWnyNbMcUyP6g6br",
                AuthorizeApi = "https://notify-bot.line.me/oauth/authorize",
                TokenApi = "https://notify-bot.line.me/oauth/token",
                NotifyApi = "https://notify-api.line.me/api/notify",
                StatusApi = "https://notify-api.line.me/api/status",
                RevokeApi = "https://notify-api.line.me/api/revoke"
            });

            services.AddBlazorContextMenu();

            services.AddSingleton<BlazorServerAuthStateCache>();
            services.AddScoped<AuthenticationStateProvider, BlazorServerAuthState>();
            services.AddBlazorDownloadFile(ServiceLifetime.Scoped);
            services.AddTransient<CustomNavigation>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ServiceActivator.Configure(app.ApplicationServices);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                //app.UseMiddleware<SecurityHeadersMiddleware>();
                //app.UseHttpsRedirection(); //Temporary disable due to server
            }
            if (env.IsProduction())
            {
                app.UsePathBase(Configuration.GetValue<string>("MySettings:BaseUrl"));
            }
            // Add this before any other middleware that might write cookies
            app.UseCookiePolicy();

            var fordwardedHeaderOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };
            fordwardedHeaderOptions.KnownNetworks.Clear();
            fordwardedHeaderOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(fordwardedHeaderOptions);

            app.UseMiddleware<SecurityHeadersMiddleware>();
            //app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value
            //RequestLocalizationOptions tmp = new RequestLocalizationOptions();
            //tmp.SetDefaultCulture("ja-JP");
            //app.UseRequestLocalization(tmp);

            //app.UseRequestLocalization("ja-JP");

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseDevExpressBlazorReporting();
            app.UseDevExpressControls();
            app.UseRouting();
            app.UseAuthorization();
            app.UseRequestLocalization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/_Host");
                //endpoints.MapFallbackToPage("/Logout", "/Logout");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
