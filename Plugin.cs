using Nethereum.JsonRpc.WebSocketStreamingClient;
using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Interop;
using NodeBlock.Engine.Interop.Plugin;
using System;
using System.Collections.Generic;
using System.Text;
using Nethereum.JsonRpc.Client.Streaming;
using Nethereum.RPC.Eth.Subscriptions;
using Nethereum.RPC.Eth.DTOs;
using System.Collections.Concurrent;
using System.Linq;
using NodeBlock.Plugin.Avalanche.Nodes;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NodeBlock.Engine.Storage.MariaDB;
using Microsoft.EntityFrameworkCore;

namespace NodeBlock.Plugin.Avalanche
{
    public class Plugin : BasePlugin
    {

        public static string WEB_3_API_URL_AVAX = "";
        public static string WEB_3_WS_URL_AVAX = "";

        public static object mutex = new object();


        public static Web3 Web3ClientAVAX { get; set; }
        public static StreamingWebSocketClient SocketClientAVAX { get; set; }


        public static ManagedAvaxEvents EventsManagerAvalanche { get; set; }

        public static bool PluginAlive = true;

        public static ServiceProvider Services;

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public override void Load()
        {


            // AVAX
            WEB_3_API_URL_AVAX = Environment.GetEnvironmentVariable("avalanche_api_http_url");
            WEB_3_WS_URL_AVAX = Environment.GetEnvironmentVariable("avalanche_api_ws_url");

            Web3ClientAVAX = new Web3(WEB_3_API_URL_AVAX);
            SocketClientAVAX = new StreamingWebSocketClient(WEB_3_WS_URL_AVAX);

            try
            {
                SocketClientAVAX.StartAsync().Wait();
                logger.Info("Success! Connected to AVALANCHE network");
            }
            catch (Exception exception)
            {
                logger.Error("Failed connecting to AVALANCHE network: {0}", exception.Message);
            }


            // Init database plugin
            Services = new ServiceCollection()
                .AddScoped(provider => provider.GetService<Storage.DatabaseStorage>())
                .AddDbContextPool<Storage.DatabaseStorage>(options =>
                {
                    options.UseMySQL(
                        Environment.GetEnvironmentVariable("mariadb_uri"));
                })
                .BuildServiceProvider();


            //Managers AutoManaged Events
            EventsManagerAvalanche = new ManagedAvaxEvents(SocketClientAVAX, new Web3(WEB_3_WS_URL_AVAX));
        }

    }
}
