using Newtonsoft.Json;
using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NodeBlock.Plugin.Avalanche.Nodes.Avascan.Supply
{
    [NodeDefinition("GetAvascanSupplyNode", "Get Avascan Supply", NodeTypeEnum.Function, "Avalanche.Avascan")]
    [NodeGraphDescription("Get Supply From Avascan")]
    public class GetAvascanSupplyNode : Node
    {
        public class AvascanSupplyHttpResult
        {
            public string genesisUnlock { get; set; }
            public string stakingRewards { get; set; }
            public string lastUpdate { get; set; }
            public string circulatingSupply { get; set; }
            public string totalSupply { get; set; }
        }

        private readonly HttpClient client = new HttpClient();

        public  GetAvascanSupplyNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetAvascanSupplyNode).Name)
        {
            this.OutParameters.Add("genesisUnlock", new NodeParameter(this, "genesisUnlock", typeof(string), true));
            this.OutParameters.Add("stakingRewards", new NodeParameter(this, "stakingRewards", typeof(string), true));
            this.OutParameters.Add("lastUpdate", new NodeParameter(this, "lastUpdate", typeof(string), true));
            this.OutParameters.Add("circulatingSupply", new NodeParameter(this, "circulatingSupply", typeof(string), true));
            this.OutParameters.Add("totalSupply", new NodeParameter(this, "totalSupply", typeof(string), true));
        }

        public override bool CanBeExecuted => true;
        public override bool CanExecute => true;
        public override bool OnExecution()
        {
            var response = client.GetAsync("https://avascan.info/api/v1/supply").Result;
            var avascanSupplyResponse = JsonConvert.DeserializeObject<AvascanSupplyHttpResult>(response.Content.ReadAsStringAsync().Result);

            this.OutParameters["genesisUnlock"].SetValue(avascanSupplyResponse.genesisUnlock);
            this.OutParameters["stakingRewards"].SetValue(avascanSupplyResponse.stakingRewards);
            this.OutParameters["lastUpdate"].SetValue(avascanSupplyResponse.lastUpdate);
            this.OutParameters["circulatingSupply"].SetValue(avascanSupplyResponse.circulatingSupply);
            this.OutParameters["totalSupply"].SetValue(avascanSupplyResponse.totalSupply);

            return true;
        }
    }
}
