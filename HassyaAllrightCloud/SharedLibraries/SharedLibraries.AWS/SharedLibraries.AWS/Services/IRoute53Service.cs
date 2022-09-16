using Amazon.Route53;
using Amazon.Route53.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.AWS.Services
{
    public interface IRoute53Service
    {
        Task<List<ResourceRecordSet>> GetListRecordSet();
        Task<int> Route53RecordSetChange(string strDnsName, int timeTTL, string strValue, string rRType = "");
        Task<bool> Route53RecordSetDelete(string strDnsName, int timeTTL, string strValue, string rRType);
        Task<long> getHostedList();
    }

    public class Route53Service : IRoute53Service
    {
        string domainName = "hassyaallright.com";
        private readonly IAmazonRoute53 route53Client;
        private readonly IConfiguration configuration;

        public Route53Service(IAmazonRoute53 client, IConfiguration Configuration)
        {
            route53Client = client;
            configuration = Configuration;
            domainName = configuration.GetSection("AWS:DomainName").Value;
        }

        public async Task<long> getHostedList()
        {
            var value = await route53Client.GetHostedZoneCountAsync();
            return value.HostedZoneCount;
        }

        public async Task<List<ResourceRecordSet>> GetListRecordSet()
        {
            ListHostedZonesByNameRequest lhtn = new ListHostedZonesByNameRequest();
            lhtn.DNSName = this.domainName;

            var lshossted = route53Client.ListHostedZonesByNameAsync(lhtn);

            //CreateHostedZoneRequest zoneRequest = new CreateHostedZoneRequest { Name = this.domainName, CallerReference = "Application change request" };
            string strHostedIdFound = string.Empty;
            ListHostedZonesResponse lsr = await route53Client.ListHostedZonesAsync();
            if (lsr.HostedZones.Count > 0)
            {
                foreach (var tmp in lsr.HostedZones)
                {
                    var lrrsc = tmp.ResourceRecordSetCount;
                    string strrrsc = tmp.Id;
                    // "Z00426051EA9DXSD467YW"
                    if (strrrsc.IndexOf(configuration.GetSection("AWS:HostedZoneId").Value) > -1)
                    {
                        strHostedIdFound = strrrsc;
                        break;
                    }
                }
            }
            if (strHostedIdFound != string.Empty)
            {
                ListResourceRecordSetsRequest lrrsre = new ListResourceRecordSetsRequest(strHostedIdFound);
                ListResourceRecordSetsResponse lrrsro = await route53Client.ListResourceRecordSetsAsync(lrrsre);
                if (lrrsro.ResourceRecordSets.Count > 0)
                {
                    return lrrsro.ResourceRecordSets;
                }

            }
            return null;
        }

        public async Task<int> Route53RecordSetChange(string strDnsName, int timeTTL, string strValue, string rRType = "")
        {
            ResourceRecordSet recordSet = new ResourceRecordSet
            {
                Name = string.Format("{0}.{1}", strDnsName, configuration["AWS:DomainName"]),
                TTL = timeTTL,
                // Type = RRType.A,
                Type = RRType.CNAME,
                ResourceRecords = new List<ResourceRecord> { new ResourceRecord { Value = strValue } }
            };

            Change changeDNS = new Change
            {
                ResourceRecordSet = recordSet,
                Action = ChangeAction.UPSERT
            };

            ChangeBatch changeBatch = new ChangeBatch
            {
                Comment = "Add - edit record dsn kashikiri service",
                Changes = new List<Change> { changeDNS }
            };

            ChangeResourceRecordSetsRequest resourceRecordSetsRequest = new ChangeResourceRecordSetsRequest
            {
                HostedZoneId = configuration.GetSection("AWS:HostedZoneId").Value, //"Z00426051EA9DXSD467YW", //zoneResponse.HostedZone.Id,
                ChangeBatch = changeBatch
            };

            try
            {
                ChangeResourceRecordSetsResponse recordSetsResponse = await route53Client.ChangeResourceRecordSetsAsync(resourceRecordSetsRequest);
                GetChangeRequest changeRequest = new GetChangeRequest
                {
                    Id = recordSetsResponse.ChangeInfo.Id
                };
                var changeInfo = await this.route53Client.GetChangeAsync(changeRequest);
            }
            catch (InvalidChangeBatchException ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }

            //while (changeInfo.ChangeInfo.Status == ChangeStatus.PENDING)
            //{
            //    Thread.Sleep(TimeSpan.FromSeconds(10));
            //}

            return 1;
        }
        public async Task<bool> Route53RecordSetDelete(string strDnsName, int timeTTL, string strValue, string rType = "")
        {
            ResourceRecordSet recordSet = new ResourceRecordSet
            {
                Name = string.Format("{0}.{1}", strDnsName, configuration["AWS:DomainName"]),
                TTL = timeTTL,
                Type = RRType.A,
                ResourceRecords = new List<ResourceRecord> { new ResourceRecord { Value = strValue } }
            };

            Change changeDNS = new Change
            {
                ResourceRecordSet = recordSet,
                Action = ChangeAction.DELETE
            };

            ChangeBatch changeBatch = new ChangeBatch
            {
                Comment = "Delete record dsn kashikiri service",
                Changes = new List<Change> { changeDNS }
            };

            ChangeResourceRecordSetsRequest resourceRecordSetsRequest = new ChangeResourceRecordSetsRequest
            {
                HostedZoneId = configuration.GetSection("AWS:HostedZoneId").Value, //"Z00426051EA9DXSD467YW", //zoneResponse.HostedZone.Id,
                ChangeBatch = changeBatch
            };

            try
            {
                ChangeResourceRecordSetsResponse recordSetsResponse = await route53Client.ChangeResourceRecordSetsAsync(resourceRecordSetsRequest);
                GetChangeRequest changeRequest = new GetChangeRequest
                {
                    Id = recordSetsResponse.ChangeInfo.Id
                };
                var changeInfo = await this.route53Client.GetChangeAsync(changeRequest);
            }
            catch (InvalidChangeBatchException ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }
    }
}
