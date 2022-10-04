using System.Collections.Generic;
using System.Threading;
using Dominisoft.Nokates.Common.Infrastructure.Attributes;
using Dominisoft.Nokates.Common.Infrastructure.Configuration;
using Dominisoft.Nokates.Common.Infrastructure.Controllers;
using Dominisoft.Nokates.Common.Infrastructure.Extensions;
using Dominisoft.NoKates.EventRouter.Common;
using Dominisoft.NoKates.EventRouter.Helper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Dominisoft.NoKates.EventRouter.Controllers
{
    [Route("")]
    [ApiController]
    public class EventController : NokatesControllerBase
    {
        [HttpPost("{routingKey}")]
        [EndpointGroup("Events")]
        [NoAuth]
        public bool PublishEventByRoutingKey(string routingKey)
        {
            var requestId = Thread.CurrentThread.GetRequestId();
            var eventDetails = Request.GetRawBody();

            EventRouter.ProcessEvent(routingKey,eventDetails, requestId);
            return true;
        }

        [HttpGet("Reload")]
        [EndpointGroup("Events")]
        [NoAuth]
        public ActionResult<List<RoutingDefinition>> ReloadEventDefinitions()
        {
            var filePath = ConfigurationValues.Values["RoutingDefinitionFilePath"];
            var routingDefinitions = System.IO.File.ReadAllText(filePath).Deserialize<List<RoutingDefinition>>();
            EventRouter.SetRoutingDefinitions(routingDefinitions);
            return routingDefinitions;
        }
    }
}
