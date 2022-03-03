using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

#if WINDOWS
using Microsoft.Identity.Client.Desktop;
#endif

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await GetATokenForGraph(bool.Parse(args[0]), bool.Parse(args[1]));
        }

        static async Task GetATokenForGraph(bool withBroker, bool withIWA)
        {
            string[] scopes = new string[] { "499b84ac-1321-427f-aa17-267ca6975798/.default" }; // AzDO
            var builder = PublicClientApplicationBuilder
                 .Create("872cd9fa-d31f-45e0-9eab-6e460a02d1f1") // AzDO client
                 .WithLogging(
                     (LogLevel level, string message, bool containsPii) =>
                     {
                         Console.WriteLine($"\tMSAL {level} {message}".Replace("\n","\n\t"));
                     },
                     LogLevel.Info,
                     true
                 )
                 .WithTenantId("72f988bf-86f1-41af-91ab-2d7cd011db47"); // @microsoft.com
            if (withBroker)
            {
#if WINDOWS
                builder = builder.WithWindowsBroker();
#endif
            }
            IPublicClientApplication app = builder.Build();

            AuthenticationResult result;
            if (withIWA)
            {
                result = await app.AcquireTokenByIntegratedWindowsAuth(scopes).ExecuteAsync(CancellationToken.None);
            }
            else
            {

                var accounts = await app.GetAccountsAsync();

                var account = accounts.FirstOrDefault();

#if WINDOWS
                account = account ?? PublicClientApplication.OperatingSystemAccount;
#endif

                if (account == null)
                {
                    Console.WriteLine("no accounts found");
                    result = null;
                }
                else
                {
                    result = await app.AcquireTokenSilent(scopes, account).ExecuteAsync();
                }
            }

            Console.WriteLine(result?.Account.Username);
        }
    }
}