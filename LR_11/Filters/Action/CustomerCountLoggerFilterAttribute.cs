using LR_11.Loggers.FileLogger;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.RegularExpressions;

namespace LR_11.Filters.Action
{
    public class CustomerCountLoggerFilterAttribute : LogFilterAttributeBase<CustomerCountLoggerFilterAttribute>, IActionFilter
    {
        private readonly HashSet<string> _uniqueAddresses = new();

        private string? GetIPV6Address(HttpContext context) => context.Connection.RemoteIpAddress?.MapToIPv6().ToString();

        private HashSet<string> initAddressCount(string path)
        {
            var addressess = new HashSet<string>();
            var ipv6Regex = new Regex(
                $@"(([0-9a-fA-F]{{1,4}}:){{7,7}}[0-9a-fA-F]{{1,4}}|([0-9a-fA-F]{{1,4}}:){{1,7}}:|([0-9a-fA-F]{{1,4}}:){{1,6}}:[0-9a-fA-F]{{1,4}}|([0-9a-fA-F]{{1,4}}:){{1,5}}(:[0-9a-fA-F]{{1,4}}){{1,2}}|([0-9a-fA-F]{{1,4}}:){{1,4}}(:[0-9a-fA-F]{{1,4}}){{1,3}}|([0-9a-fA-F]{{1,4}}:){{1,3}}(:[0-9a-fA-F]{{1,4}}){{1,4}}|([0-9a-fA-F]{{1,4}}:){{1,2}}(:[0-9a-fA-F]{{1,4}}){{1,5}}|[0-9a-fA-F]{{1,4}}:((:[0-9a-fA-F]{{1,4}}){{1,6}})|:((:[0-9a-fA-F]{{1,4}}){{1,7}}|:)|fe80:(:[0-9a-fA-F]{{0,4}}){{0,4}}%[0-9a-zA-Z]{{1,}}|::(ffff(:0{{1,4}}){{0,1}}:){{0,1}}((25[0-5]|(2[0-4]|1{{0,1}}[0-9]){{0,1}}[0-9])\.){{3,3}}(25[0-5]|(2[0-4]|1{{0,1}}[0-9]){{0,1}}[0-9])|([0-9a-fA-F]{{1,4}}:){{1,4}}:((25[0-5]|(2[0-4]|1{{0,1}}[0-9]){{0,1}}[0-9])\.){{3,3}}(25[0-5]|(2[0-4]|1{{0,1}}[0-9]){{0,1}}[0-9]))"
                );
            var logLines = File.ReadAllLines(path);

            foreach (var line in logLines)
            {
                var ipv6Match = ipv6Regex.Match(line);
                if (ipv6Match.Success && !String.IsNullOrEmpty(ipv6Match.Value))
                {
                    addressess.Add(ipv6Match.Value);
                }
            }

            return addressess;
        }
        public CustomerCountLoggerFilterAttribute(IConfiguration configuration): base(configuration.GetSection("CUSTOMER_COUNT_LOG_FILE_PATH").Value ?? "./customer_count_log.txt")
        {
            if (File.Exists(LogPath))
            {
                _uniqueAddresses = initAddressCount(LogPath);
            }

        }

        private void LogCount(string? address)
        {
            if (String.IsNullOrEmpty(address) || _uniqueAddresses.Contains(address))
            {
                return;
            }

            _uniqueAddresses.Add(address);

            Logger.LogInformation($"{address} (# {_uniqueAddresses.Count.ToString()})");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var address = GetIPV6Address(context.HttpContext);
            LogCount(address);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
