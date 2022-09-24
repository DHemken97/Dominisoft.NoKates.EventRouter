using System.Text.RegularExpressions;
using Dominisoft.Nokates.Common.Infrastructure.Helpers;
using Newtonsoft.Json.Linq;

namespace Dominisoft.NoKates.EventRouter.Helper
{
    public static class TransformHelper
    {
        public static string ReplaceValues(string template, string eventDetails)
        {
            var result = template;

            Regex expression = new Regex(@"\[(?<Identifier>[A-Za-z0-9.]*)\]");
            var results = expression.Matches(result);
            LoggingHelper.LogMessage($"Found {results.Count} Replacements in {eventDetails}");
            foreach (Match match in results)
            {
                var name = match.Groups["Identifier"].Value;
                var j = JObject.Parse(eventDetails);
                var val = j[name];
                //var token = j.SelectToken(name);
                LoggingHelper.LogMessage($"Replacing {name} with {val.ToString()}");
                result = result.Replace($"[{name}]", val.ToString());
            }

            return result;
        }
    }
}
