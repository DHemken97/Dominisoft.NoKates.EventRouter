using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Dominisoft.Nokates.Common.Infrastructure.Extensions;
using Dominisoft.Nokates.Common.Infrastructure.Helpers;
using Dominisoft.NoKates.EventRouter.Common;
using Dominisoft.NoKates.EventRouter.Helper;

namespace Dominisoft.NoKates.EventRouter
{
    public static class EventRouter
    {
        private static List<RoutingDefinition> _routingDefinitions;

        public static void SetRoutingDefinitions(List<RoutingDefinition> routingDefinitions)
            => _routingDefinitions = routingDefinitions;
        public static async Task<bool> ProcessEvent(string routingKey, string eventDetails,Guid requestId)
        {
           return await Task.Run(() =>
            {
                try
                {
                    var matchingDefinitions = _routingDefinitions.Where(rd => rd.RoutingKey == routingKey).ToList();
                    if (!matchingDefinitions.Any())
                    {
                        var message = $"Could not find any matching Routing Definitions for '{routingKey}'";
                        Logger.LogDebug(message);
                        return true;
                    }
                     Logger.LogDebug("EventDetails:"+eventDetails);
                     Logger.LogDebug($"{routingKey} : {matchingDefinitions.Count} Routes Found");
                    foreach (var matchingDefinition in matchingDefinitions)
                    {
                        CreateRequestFromDefinition(matchingDefinition, eventDetails, requestId);
                    }
                }
                catch (Exception e)
                {
                     Logger.LogDebug(e.Message);
                }


                return true;
            });
        }

        private static void CreateRequestFromDefinition(RoutingDefinition matchingDefinition, string eventDetails,
            Guid requestId)
        {
            var path = TransformHelper.ReplaceValues(matchingDefinition.RequestUri, eventDetails);
            var body = TransformHelper.ReplaceValues(matchingDefinition.RequestBody, eventDetails);
            var obj = body.Deserialize<dynamic>();
            Thread.CurrentThread.SetRequestId(requestId);
             Logger.LogDebug($"{matchingDefinition.DefinitionName} : {matchingDefinition.RequestType} {path}");
            switch (matchingDefinition.RequestType)
            {
                case "GET":
                    HttpHelper.Get(path);
                    break;
                case "POST":
                    HttpHelper.Post(path, obj);
                    break;
                case "PUT":
                    HttpHelper.Put(path, obj);
                    break;
                case "DELETE":
                    HttpHelper.Delete(path, obj);
                    break;
                default:
                    throw new ArgumentException($"Unsupported Request Type '{matchingDefinition.RequestType}' in Routing Definition '{matchingDefinition.DefinitionName}'");
            }
            
        }
    }
}
