using Newtonsoft.Json;
using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NodeBlock.Plugin.Avalanche.Nodes.SnowTrace.Accounts
{
    [NodeDefinition("GetAvaxBalanceByAddressNode", "Get AVAX Balance Single Address", NodeTypeEnum.Function, "Avalanche.SnowTrace")]
    [NodeGraphDescription("Get AVAX Balance for a single Address")]
    public class GetAvaxBalanceByAddressNode : Node
    {
        public class AvaxBalanceByAddressHttpResult
        {
            public string status { get; set; }
            public string message { get; set; }
            public string result { get; set; }
        }

        private readonly HttpClient client = new HttpClient();

        public GetAvaxBalanceByAddressNode(string id, BlockGraph graph)
           : base(id, graph, typeof(GetAvaxBalanceByAddressNode).Name)
        {
            this.InParameters.Add("snowtraceConnection", new NodeParameter(this, "snowtraceConnection", typeof(SnowTraceConnectorNode), true));
            this.InParameters.Add("walletAddress", new NodeParameter(this, "walletAddress", typeof(string), true));

            this.OutParameters.Add("walletBalance", new NodeParameter(this, "walletBalance", typeof(string), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var apiKey = (this.InParameters["snowtraceConnection"].GetValue() as SnowTraceConnectorNode).InParameters["apiKey"].GetValue();
            var walletAddress = (this.InParameters["walletAddress"].GetValue());
            var response = client.GetAsync("https://api.snowtrace.io/api?module=account&action=balance&tag=latest&address=" + walletAddress + "&apikey=" + apiKey).Result;
            var avaxBalanceByAddressResponse = JsonConvert.DeserializeObject<AvaxBalanceByAddressHttpResult>(response.Content.ReadAsStringAsync().Result);

            this.OutParameters["walletBalance"].SetValue(avaxBalanceByAddressResponse.result);

            return true;
        }
    }
}
