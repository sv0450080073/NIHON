using Amazon.Route53.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.AWS.Services
{
    public interface IAWSSubDomainService
    {
        Task<long> CountListHosted();
        Task<List<string>> GetListHosted();
        Task<int> createOrUpdateRecord(string strName);
        Task<bool> deleteRecord(string stfName);
    }

    public class AWSSubDomainService : IAWSSubDomainService
    {
        private readonly IRoute53Service _route53Hosted;
        private readonly IConfiguration configuration;
        public AWSSubDomainService(IRoute53Service route53, IConfiguration Configuration)
        {
            _route53Hosted = route53;
            configuration = Configuration;
        }

        public async Task<long> CountListHosted()
        {
            var hostedNumber = await _route53Hosted.getHostedList();
            return hostedNumber;
        }

        public async Task<List<string>> GetListHosted()
        {
            List<string> output = new List<string>();
            List<ResourceRecordSet> listResourceRecordSetsResponse = await _route53Hosted.GetListRecordSet();
            if (listResourceRecordSetsResponse.Count > 0)
            {
                foreach (ResourceRecordSet tmp in listResourceRecordSetsResponse)
                {
                    output.Add(tmp.Name);
                }
            }
            return output;
        }

        public async Task<int> createOrUpdateRecord(string strName)
        {
            string ip = configuration.GetSection("AWS:IpAddress").Value;
            int tmp = await _route53Hosted.Route53RecordSetChange(strDnsName: strName, timeTTL: 300, strValue: ip, rRType: string.Empty); // "18.178.152.3"
            return tmp;
        }

        public async Task<bool> deleteRecord(string strName)
        {
            string ip = configuration.GetSection("AWS:IpAddress").Value;
            bool tmp = await _route53Hosted.Route53RecordSetDelete(strDnsName: strName, timeTTL: 300, strValue: ip, rRType: string.Empty); // "18.178.152.3"
            return tmp;
        }
    }
}
