using System.Collections.Generic;
using System.Threading;
using Dominisoft.Nokates.Common.Infrastructure.Attributes;
using Dominisoft.Nokates.Common.Infrastructure.Controllers;
using Dominisoft.Nokates.Common.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Dominisoft.NoKates.EventRouter.Controllers
{
    [Route("")]
    [ApiController]
    public class EventController : NokatesControllerBase
    {
        [HttpGet("{routingKey}")]
        [EndpointGroup("Events")]
        [NoAuth]
        public bool PublishEventByRoutingKey(string routingKey, [FromBody] Dictionary<string,string> eventDetails)
        {
            var requestId = Thread.CurrentThread.GetRequestId();
            EventRouter.ProcessEvent(routingKey,eventDetails.Serialize(), requestId);
            return true;
        }
    }
}
