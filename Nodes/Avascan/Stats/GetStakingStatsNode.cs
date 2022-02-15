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
    [NodeDefinition("GetAvascanStakingStatsNode", "Get Avascan Staking Stats", NodeTypeEnum.Function, "Avalanche.Avascan")]
    [NodeGraphDescription("Get Staking Stats From Avascan")]
    public class GetAvascanStakingStatsNode : Node
    {
        public class AvascanStakingStatsHttpResult
        {
            public string totalValidator { get; set; }
            public string totalDelegation { get; set; }
            public string totalStake { get; set; }
            public string totalValidationStake { get; set; }
            public string totalDelegatedStake { get; set; }
            public string stakingReward { get; set; }
            public string stakingRatio { get; set; }
        }

        private readonly HttpClient client = new HttpClient();

        public GetAvascanStakingStatsNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetAvascanStakingStatsNode).Name)
        {
            this.OutParameters.Add("totalValidator", new NodeParameter(this, "totalValidator", typeof(string), true));
            this.OutParameters.Add("totalDelegation", new NodeParameter(this, "totalDelegation", typeof(string), true));
            this.OutParameters.Add("totalStake", new NodeParameter(this, "totalStake", typeof(string), true));
            this.OutParameters.Add("totalValidationStake", new NodeParameter(this, "totalValidationStake", typeof(string), true));
            this.OutParameters.Add("totalDelegatedStake", new NodeParameter(this, "totalDelegatedStake", typeof(string), true));
            this.OutParameters.Add("stakingReward", new NodeParameter(this, "stakingReward", typeof(string), true));
            this.OutParameters.Add("stakingRatio", new NodeParameter(this, "stakingRatio", typeof(string), true));
        }

        public override bool CanBeExecuted => true;
        public override bool CanExecute => true;
        public override bool OnExecution()
        {
            var response = client.GetAsync("https://avascan.info/api/v1/statistics").Result;
            var avascanStakingStatsResponse = JsonConvert.DeserializeObject<AvascanStakingStatsHttpResult>(response.Content.ReadAsStringAsync().Result);

            this.OutParameters["totalValidator"].SetValue(avascanStakingStatsResponse.totalValidator);
            this.OutParameters["totalDelegation"].SetValue(avascanStakingStatsResponse.totalDelegation);
            this.OutParameters["totalStake"].SetValue(avascanStakingStatsResponse.totalStake);
            this.OutParameters["totalValidationStake"].SetValue(avascanStakingStatsResponse.totalValidationStake);
            this.OutParameters["totalDelegatedStake"].SetValue(avascanStakingStatsResponse.totalDelegatedStake);
            this.OutParameters["stakingReward"].SetValue(avascanStakingStatsResponse.stakingReward);
            this.OutParameters["stakingRatio"].SetValue(avascanStakingStatsResponse.stakingRatio);

            return true;
        }
    }
}
