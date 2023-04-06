using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System.ServiceModel.Description;
namespace APi_from_fetch_XML.Controllers
{
    public class ValuesController : ApiController
    {
        [HttpGet]
        [Route("api/getdata")]
        public IHttpActionResult GetData()
        {
            /* string serviceUrl = "https://crmuat.spectra.co/XRMServices/2011/OrganizationData.svc";
             string userName = "crm.deploy";
             string password = "Crdm@311#";*/
            string serviceUrl = "https://spectranetcrm.spectranet.in/XRMServices/2011/Organization.svc";
            string userName = "";
            string password = "";
            var clientCredentials = new ClientCredentials();
            clientCredentials.UserName.UserName = userName;
            clientCredentials.UserName.Password = password;

            var proxy = new OrganizationServiceProxy(new Uri(serviceUrl), null, clientCredentials, null);
            var service = (IOrganizationService)proxy;

            try
            {
                QueryExpression query = new QueryExpression("alletech_city");
                query.ColumnSet = new ColumnSet(true);

                EntityCollection results1 = service.RetrieveMultiple(query);

                var fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='alletech_city'>
                                    <attribute name='alletech_cityid' />
                                    <attribute name='alletech_cityname' />
                                    <attribute name='spectra_isoperationalcity' />
                                    <attribute name='alletech_cityno' />
                                    <order attribute='alletech_cityname' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='statecode' operator='eq' value='0' />
                                      <condition attribute='spectra_isoperationalcity' operator='eq' value='1' />
                                    </filter>
                                  </entity>
                                </fetch>";

                var fetchQuery = new FetchExpression(fetchXml);
                var results2 = service.RetrieveMultiple(fetchQuery);

                var data = results2.Entities.Select(e => new {
                    Id = e.GetAttributeValue<Guid>("alletech_cityid"),
                    Name = e.GetAttributeValue<string>("alletech_cityname"),
                    Number = e.GetAttributeValue<string>("alletech_cityno"),
                    IsOperationalCity = e.GetAttributeValue<bool>("spectra_isoperationalcity") ? "Yes" : "No"
                }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return InternalServerError(new System.Exception("Dynamics CRM service is not available.", ex));
            }
        }
    }
}
