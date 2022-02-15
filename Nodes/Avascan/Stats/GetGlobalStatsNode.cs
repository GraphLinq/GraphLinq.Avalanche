using Newtonsoft.Json;
using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NodeBlock.Plugin.Avalanche.Nodes.Avascan.Stats
{
    [NodeDefinition("GetAvascanGlobalStatsNode", "Get Avascan Global Stats", NodeTypeEnum.Function, "Avalanche.Avascan")]
    [NodeGraphDescription("Get Global Stats From Avascan")]
    public class GetAvascanGlobalStatsNode : Node
    {
        public class AvascanGlobalStatsHttpResult
        {
            public string blockchains { get; set; }
            public string validators { get; set; }
            public string stakingRatio { get; set; }
            public string stakingRewards { get; set; }
            public string price { get; set; }
            public string marketcapByCirculatingSupply { get; set; }
            public string marketcapByTotalSupply { get; set; }
            public string circulatingSupply { get; set; }
            public string lastTransactions24h { get; set; }
            public string lastAvgTps24h { get; set; }
            //public string timeToFinality { get; set; }
            public string assetsAndTokens { get; set; }
            public string burnedSinceLaunch { get; set; }

        }

        private readonly HttpClient client = new HttpClient();

        public GetAvascanGlobalStatsNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetAvascanGlobalStatsNode).Name)
        {
            this.OutParameters.Add("blockchains", new NodeParameter(this, "blockchains", typeof(string), true));
            this.OutParameters.Add("validators", new NodeParameter(this, "validators", typeof(string), true));
            this.OutParameters.Add("stakingRatio", new NodeParameter(this, "stakingRatio", typeof(string), true));
            this.OutParameters.Add("stakingRewards", new NodeParameter(this, "stakingRewards", typeof(string), true));
            this.OutParameters.Add("price", new NodeParameter(this, "price", typeof(string), true));
            this.OutParameters.Add("marketcapByCirculatingSupply", new NodeParameter(this, "marketcapByCirculatingSupply", typeof(string), true));
            this.OutParameters.Add("marketcapByTotalSupply", new NodeParameter(this, "marketcapByTotalSupply", typeof(string), true));
            this.OutParameters.Add("circulatingSupply", new NodeParameter(this, "circulatingSupply", typeof(string), true));
            this.OutParameters.Add("lastTransactions24h", new NodeParameter(this, "lastTransactions24h", typeof(string), true));
            this.OutParameters.Add("lastAvgTps24h", new NodeParameter(this, "lastAvgTps24h", typeof(string), true));
            //this.OutParameters.Add("timeToFinality", new NodeParameter(this, "timeToFinality", typeof(string), true));
            this.OutParameters.Add("assetsAndTokens", new NodeParameter(this, "assetsAndTokens", typeof(string), true));
            this.OutParameters.Add("burnedSinceLaunch", new NodeParameter(this, "burnedSinceLaunch", typeof(string), true));
        }

        public override bool CanBeExecuted => true;
        public override bool CanExecute => true;
        public override bool OnExecution()
        {
            var response = client.GetAsync("https://avascan.info/api/v1/home/statistics").Result;
            var avascanGlobalStatsResponse = JsonConvert.DeserializeObject<AvascanGlobalStatsHttpResult>(response.Content.ReadAsStringAsync().Result);

            this.OutParameters["blockchains"].SetValue(avascanGlobalStatsResponse.blockchains);
            this.OutParameters["validators"].SetValue(avascanGlobalStatsResponse.validators);
            this.OutParameters["stakingRatio"].SetValue(avascanGlobalStatsResponse.stakingRatio);
            this.OutParameters["stakingRewards"].SetValue(avascanGlobalStatsResponse.stakingRewards);
            this.OutParameters["price"].SetValue(avascanGlobalStatsResponse.price);
            this.OutParameters["marketcapByCirculatingSupply"].SetValue(avascanGlobalStatsResponse.marketcapByCirculatingSupply);
            this.OutParameters["marketcapByTotalSupply"].SetValue(avascanGlobalStatsResponse.marketcapByTotalSupply);
            this.OutParameters["circulatingSupply"].SetValue(avascanGlobalStatsResponse.circulatingSupply);
            this.OutParameters["lastTransactions24h"].SetValue(avascanGlobalStatsResponse.lastTransactions24h);
            this.OutParameters["lastAvgTps24h"].SetValue(avascanGlobalStatsResponse.lastAvgTps24h);
            //this.OutParameters["timeToFinality"].SetValue(avascanGlobalStatsResponse.timeToFinality);
            this.OutParameters["assetsAndTokens"].SetValue(avascanGlobalStatsResponse.assetsAndTokens);
            this.OutParameters["burnedSinceLaunch"].SetValue(avascanGlobalStatsResponse.burnedSinceLaunch);

            return true;
        }
    }
}
