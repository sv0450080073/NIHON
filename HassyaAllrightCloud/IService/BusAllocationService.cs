using HassyaAllrightCloud.Application.BusAllocation.Queries;
using HassyaAllrightCloud.Application.CustomItem.Queries;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static HassyaAllrightCloud.Domain.Dto.BusAllocationData;

namespace HassyaAllrightCloud.IService
{
    public interface IBusAllocationService
    {
        Task<List<BusAllocationDataGrid>> GetBusGetBusAllocationDataGrid(int tenantCdSeq, BusAllocationSearch busAllocationSearch);
        Task<List<BusInfoData>> GetBusInfoData(int tenantCdSeq, string date);
        Task<bool> SyainIsAvailable(int syainId, int tenantId, string date);
        Task<List<BranchChartData>> GetBranchChartData(int tenantCdSeq);
        Task<List<TPM_CodeKbDataKenCD>> GetTPM_CodeKbDataKenCD(int tenantCdSeq);
        Task<List<TPM_CodeKbDataBunruiCD>> GetTPM_CodeKbDataBunruiCD(int tenantCdSeq);
        Task<List<TPM_CodeKbDataDepot>> GetTPM_CodeKbDataDepot(int tenantCdSeq, string date);
        Task<List<AssignedEmployee>> GetAssignedEmployee(string ukeno,short unkren, short bunkren, short teidano);
        Task<List<CarTypePopup>> GetYykSyuData(string ukeno, int tenantCdSeq);
        Task<Dictionary<string, string>> Generate(BusAllocationSearch busAllocationSearch);
        Task<Dictionary<string, string>> GetCustomHaiShaFieldQuery(int tenantCdSeq, BusAllocationDataGrid busAllocationDataGrid);
        Task<BusAllocationHaitaCheck> GetBusAllocationHaitaCheck(string UkeNo);

    }
    public class BusAllocationService : IBusAllocationService
    {

        private readonly IMediator _mediator;
        public BusAllocationService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<Dictionary<string, string>> Generate(BusAllocationSearch busAllocationSearch)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            var type = typeof(BusAllocationSearch);
            IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                object propValue = prop.GetValue(busAllocationSearch, null);
                bool allowKey = false;
                var fieldName = prop.Name;
                var JoInput = string.Empty;
                var value = propValue;

                if (fieldName == nameof(BusAllocationSearch.DateSpecified))
                {
                    allowKey = true;
                    JoInput = value != null ? value.ToString() : string.Empty;
                }
                if (fieldName == nameof(BusAllocationSearch.pickupDate))
                {
                    allowKey = true;
                    string valueParse = value != null ? value.ToString().Substring(0,10): string.Empty;
                    DateTime pickupDate=DateTime.Today;
                    if (DateTime.TryParseExact(valueParse, "yyyy/MM/dd", null, DateTimeStyles.None, out pickupDate))
                    {
                        JoInput = pickupDate.ToString("yyyyMMdd");
                    }
                }
                if (fieldName == nameof(BusAllocationSearch.BranchChart))
                {
                    allowKey = true;
                    if (value != null)
                    {
                        BranchChartData data = (BranchChartData)value;
                        JoInput = data.EigyoCdSeq.ToString();
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(BusAllocationSearch.bookingParam))
                {
                    allowKey = true;
                    JoInput = value != null ? value.ToString() : string.Empty;
                }
                if (fieldName == nameof(BusAllocationSearch.ReservationClassification))
                {
                    allowKey = true;
                    if (value != null)
                    {
                        ReservationData data = (ReservationData)value;
                        JoInput = data.YoyaKbnSeq.ToString();
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(BusAllocationSearch.outputSortOrder))
                {
                    allowKey = true;
                    JoInput = value != null ? value.ToString() : string.Empty;
                }
                if (fieldName == nameof(BusAllocationSearch.size))
                {
                    allowKey = true;
                    JoInput = value != null ? value.ToString() : string.Empty;
                }
                if (fieldName == nameof(BusAllocationSearch.BookingFrom))
                {
                    allowKey = true;
                        ReservationClassComponentData data = (ReservationClassComponentData)value;
                        if(data!=null)
                        {
                            JoInput = data.YoyaKbnSeq.ToString();
                        }
                        else
                        {
                            JoInput = string.Empty;
                        }
                }
                if (fieldName == nameof(BusAllocationSearch.BookingTo))
                {
                    allowKey = true;
                    ReservationClassComponentData data = (ReservationClassComponentData)value;
                    if (data != null)
                    {
                        JoInput = data.YoyaKbnSeq.ToString();
                    }
                    else
                    {
                        JoInput = string.Empty;
                    }
                }
                if (fieldName == nameof(BusAllocationSearch.CompanyChart))
                {
                    allowKey = true;
                    if (value != null)
                    {
                        CompanyChartData data = (CompanyChartData)value;
                        JoInput = data.CompanyCdSeq.ToString();
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(BusAllocationSearch.UnprovisionedVehicle1))
                {
                    allowKey = true;
                    JoInput = value != null ? value.ToString() : string.Empty;
                }
                if (allowKey)
                {
                    result.Add(fieldName, JoInput);
                }
            }
            return result;
        }
        public async Task<List<AssignedEmployee>> GetAssignedEmployee(string ukeno,short unkren, short bunkren, short teidano)
        {
            return await _mediator.Send(new GetAssignedEmployeeByUkenoQuery() { Ukeno = ukeno , UnkRen=unkren, BunkRen=bunkren, TeiDanNo=teidano});
        }
        public async Task<List<BranchChartData>> GetBranchChartData(int tenantCdSeq)
        {
            return await _mediator.Send(new GetBranchChartDataQuery() {TenantCdSeq = tenantCdSeq }); 
        }
        public async Task<List<BusAllocationDataGrid>> GetBusGetBusAllocationDataGrid(int tenantCdSeq, BusAllocationSearch busAllocationSearch )
        {
            return await _mediator.Send(new GetBusAllocationDataGridQuery() { BusAllocationSearch = busAllocationSearch,TenantCdSeq = tenantCdSeq });
        }
        public async Task<List<BusInfoData>> GetBusInfoData(int tenantCdSeq, string date)
        {
            return await _mediator.Send(new GetBusInfoDataQuery() { Date=date, TenantCdSeq = tenantCdSeq });
        }
        public async Task<List<TPM_CodeKbDataBunruiCD>> GetTPM_CodeKbDataBunruiCD(int tenantCdSeq)
        {
            return await _mediator.Send(new GetVPM_HaichiDataQuery() { TenantCdSeq = tenantCdSeq });
        }

        public async Task<List<TPM_CodeKbDataDepot>> GetTPM_CodeKbDataDepot(int tenantCdSeq, string date)
        {
             return await _mediator.Send(new GetTPM_CodeKbDataDepotQuery() { Date=date, TenantCdSeq = tenantCdSeq });
        }

        public async Task<List<TPM_CodeKbDataKenCD>> GetTPM_CodeKbDataKenCD(int tenantCdSeq)
        {
            return await _mediator.Send(new GetCodeKbDataKenCDQuery() { TenantCdSeq = tenantCdSeq });
        }

        public async Task<List<CarTypePopup>> GetYykSyuData(string ukeno, int tenantCdSeq)
        {
            return await _mediator.Send(new GetYykSyuDataQuery() { Ukeno =ukeno,TenantCdSeq = tenantCdSeq });
        }

        public async Task<Dictionary<string, string>> GetCustomHaiShaFieldQuery(int tenantCdSeq, BusAllocationDataGrid busAllocationDataGrid )
        {
            var param = busAllocationDataGrid;
            var fieldConfigs = await _mediator.Send(new GetCustomItemQuery(tenantCdSeq));
            return await _mediator.Send(new GetCustomHaiShaFieldQuery(fieldConfigs,param.YYKSHO_UkeNo,param.HAISHA_UnkRen,
                param.HAISHA_SyaSyuRen,param.HAISHA_TeiDanNo, param.HAISHA_BunkRen));
        }

        public async Task<bool> SyainIsAvailable(int syainId, int tenantId, string date)
        {
            return await _mediator.Send(new SyainIsAvailableQuery(syainId, tenantId, date));
        }

        public async Task<BusAllocationHaitaCheck> GetBusAllocationHaitaCheck(string UkeNo)
        {
            return await _mediator.Send(new GetBusAllocationHaitaCheckQuery() { UkeNo = UkeNo });
        }

    }
}
