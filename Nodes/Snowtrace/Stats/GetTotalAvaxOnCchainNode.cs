using Newtonsoft.Json;
using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NodeBlock.Plugin.Avalanche.Nodes.SnowTrace.Stats
{
    [NodeDefinition("GetTotalAvaxOnCchainNode", "Get Total AVAX on C-Chain", NodeTypeEnum.Function, "Avalanche.SnowTrace")]
    [NodeGraphDescription("Get Total Supply of AVAX on the Avalanche C-Chain")]
    [NodeIDEParameters(Hidden = true)] // Endpoint broken on SnowTrace

    public class GetTotalAvaxOnCchainNode : Node
    {
        public class GetTotalAvaxOnCchainHttpResult
        {
            public string status { get; set; }
            public string message { get; set; }
            public string result { get; set; }
        }

        private readonly HttpClient client = new HttpClient();

        public GetTotalAvaxOnCchainNode(string id, BlockGraph graph)
           : base(id, graph, typeof(GetTotalAvaxOnCchainNode).Name)
        {
            this.InParameters.Add("snowtraceConnection", new NodeParameter(this, "snowtraceConnection", typeof(SnowTraceConnectorNode), true));

            this.OutParameters.Add("totalSupply", new NodeParameter(this, "totalSupply", typeof(string), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var apiKey = (this.InParameters["snowtraceConnection"].GetValue() as SnowTraceConnectorNode).InParameters["apiKey"].GetValue();
            var response = client.GetAsync("https://api.snowtrace.io/api?module=stats&action=AVAXsupply&apikey=" + apiKey).Result;
            var avaxTotalSupplyResponse = JsonConvert.DeserializeObject<GetTotalAvaxOnCchainHttpResult>(response.Content.ReadAsStringAsync().Result);

            this.OutParameters["totalSupply"].SetValue(avaxTotalSupplyResponse.result);

            return true;
        }
    }
}
