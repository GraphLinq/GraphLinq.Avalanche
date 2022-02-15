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
    [NodeDefinition("GetTokenBalanceForContractNode", "Get ERC-20 Balance For Contract", NodeTypeEnum.Function, "Avalanche.SnowTrace")]
    [NodeGraphDescription("Get Token Balance For Contract")]
    public class GetTokenBalanceForContractNode : Node
    {
        public class GetTokenBalanceForContractHttpResult
        {
            public string status { get; set; }
            public string message { get; set; }
            public string result { get; set; }
        }

        private readonly HttpClient client = new HttpClient();

        public GetTokenBalanceForContractNode(string id, BlockGraph graph)
           : base(id, graph, typeof(GetTokenBalanceForContractNode).Name)
        {
            this.InParameters.Add("snowtraceConnection", new NodeParameter(this, "snowtraceConnection", typeof(SnowTraceConnectorNode), true));
            this.InParameters.Add("contractAddress", new NodeParameter(this, "contractAddress", typeof(string), true));
            this.InParameters.Add("tokenAddress", new NodeParameter(this, "tokenAddress", typeof (string), true));

            this.OutParameters.Add("accountBalance", new NodeParameter(this, "accountBalance", typeof(string), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var apiKey = (this.InParameters["snowtraceConnection"].GetValue() as SnowTraceConnectorNode).InParameters["apiKey"].GetValue();
            var contractAddress = (this.InParameters["contractAddress"].GetValue());
            var tokenAddress = (this.InParameters["tokenAddress"].GetValue());
            var response = client.GetAsync("https://api.snowtrace.io/api?module=account&action=tokenbalance&contractaddress=" + contractAddress + "&address=" + tokenAddress +"&apikey=" + apiKey).Result;
            var avaxTokenBalanceForContractResponse = JsonConvert.DeserializeObject<GetTokenBalanceForContractHttpResult>(response.Content.ReadAsStringAsync().Result);

            this.OutParameters["accountBalance"].SetValue(avaxTokenBalanceForContractResponse.result);

            return true;
        }
    }
}
