using System.Collections.Generic;
using Dominisoft.Nokates.Common.Infrastructure.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dominisoft.NoKates.EventRouter.Controllers
{
    [Route("")]
    [ApiController]
    public class EventController : ControllerBase
    {
        [HttpGet("{routingKey}")]
        [EndpointGroup("Events")]
        [NoAuth]
        public bool PublishEventByRoutingKey(string routingKey, [FromBody] string eventDetails)
        {
            return true;
        }
    }
}
