using DevExpress.XtraPrinting.BarCode;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class PaymentRequestReportDS
    {
        public List<PaymentRequestReport> _data { get; set; }
        public PaymentRequestReportDS(List<PaymentRequestReport> data)
        {
            _data = data;
        }
    }
}
