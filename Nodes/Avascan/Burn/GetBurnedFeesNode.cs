using Newtonsoft.Json;
using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NodeBlock.Plugin.Avalanche.Nodes.Avascan.Burn
{
    [NodeDefinition("GetAvascanBurnedFeesNode", "Get Avascan Burned Fees", NodeTypeEnum.Function, "Avalanche.Avascan")]
    [NodeGraphDescription("Get Avascan Burned Fees")]
    public class GetAvascanBurnedFeesNode : Node
    {
        public class AvascanBurnedFeesHttpResult
        {
            public string X { get; set; }
            public string C { get; set; }
            public string XC { get; set; }
        }

        private readonly HttpClient client = new HttpClient();

        public GetAvascanBurnedFeesNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetAvascanBurnedFeesNode).Name)
        {
            this.OutParameters.Add("x", new NodeParameter(this, "x", typeof(string), true));
            this.OutParameters.Add("c", new NodeParameter(this, "c", typeof(string), true));
            this.OutParameters.Add("xc", new NodeParameter(this, "xc", typeof(string), true));
        }

        public override bool CanBeExecuted => true;
        public override bool CanExecute => true;
        public override bool OnExecution()
        {
            var response = client.GetAsync("https://avascan.info/api/v1/burned-fees").Result;
            var avascanBurnedFeesResponse = JsonConvert.DeserializeObject<AvascanBurnedFeesHttpResult>(response.Content.ReadAsStringAsync().Result);

            this.OutParameters["x"].SetValue(avascanBurnedFeesResponse.X);
            this.OutParameters["c"].SetValue(avascanBurnedFeesResponse.C);
            this.OutParameters["xc"].SetValue(avascanBurnedFeesResponse.XC);

            return true;
        }


    }
}
