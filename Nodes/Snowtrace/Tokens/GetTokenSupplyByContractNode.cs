using Newtonsoft.Json;
using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NodeBlock.Plugin.Avalanche.Nodes.SnowTrace.Tokens
{
    [NodeDefinition("GetTokenSupplyByContractNode", "Get ERC-20 Token Supply", NodeTypeEnum.Function, "Avalanche.SnowTrace")]
    [NodeGraphDescription("Get ERC-20 Token Supply From Contract")]
    public class GetTokenSupplyByContractNode : Node
    {
        public class AvaxTokenSupplyByContractHttpResult
        {
            public string status { get; set; }
            public string message { get; set; }
            public string result { get; set; }
        }

        private readonly HttpClient client = new HttpClient();

        public GetTokenSupplyByContractNode(string id, BlockGraph graph)
           : base(id, graph, typeof(GetTokenSupplyByContractNode).Name)
        {
            this.InParameters.Add("snowtraceConnection", new NodeParameter(this, "snowtraceConnection", typeof(SnowTraceConnectorNode), true));
            this.InParameters.Add("contractAddress", new NodeParameter(this, "contractAddress", typeof(string), true));

            this.OutParameters.Add("totalSupply", new NodeParameter(this, "totalSupply", typeof(string), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var apiKey = (this.InParameters["snowtraceConnection"].GetValue() as SnowTraceConnectorNode).InParameters["apiKey"].GetValue();
            var contractAddress = (this.InParameters["contractAddress"].GetValue());
            var response = client.GetAsync("https://api.snowtrace.io/api?module=stats&action=tokensupply&contractaddress=" + contractAddress + "&apikey=" + apiKey).Result;
            var avaxTokenSupplyByContractResponse = JsonConvert.DeserializeObject<AvaxTokenSupplyByContractHttpResult>(response.Content.ReadAsStringAsync().Result);

            this.OutParameters["totalSupply"].SetValue(avaxTokenSupplyByContractResponse.result);

            return true;
        }
    }
}
