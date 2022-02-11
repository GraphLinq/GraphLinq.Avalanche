using Nethereum.Web3;
using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Avalanche.Transaction
{
    [NodeDefinition("GetAvaxBlockTransactionParametersNode", "Get Block Parameters", NodeTypeEnum.Function, "Blockchain.Avalanche")]
    [NodeGraphDescription("Parse the last Avalanche block object received into string out parameters readable")]
    public class GetAvaxBlockTransactionParametersNode : Node
    {
        public GetAvaxBlockTransactionParametersNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetAvaxBlockTransactionParametersNode).Name)
        {
            this.InParameters.Add("block", new NodeParameter(this, "block", typeof(Nethereum.RPC.Eth.DTOs.Block), true));

            this.OutParameters.Add("gasUsed", new NodeParameter(this, "gasUsed", typeof(decimal), false));
            this.OutParameters.Add("gasLimit", new NodeParameter(this, "gasLimit", typeof(decimal), false));
            this.OutParameters.Add("blockHash", new NodeParameter(this, "blockHash", typeof(decimal), false));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "gasUsed")
            {
                return Web3.Convert.FromWei((this.InParameters["block"].GetValue() as Nethereum.RPC.Eth.DTOs.Block).GasUsed);
            }
            if (parameter.Name == "gasLimit")
            {
                return Web3.Convert.FromWei((this.InParameters["block"].GetValue() as Nethereum.RPC.Eth.DTOs.Block).GasLimit);
            }
            if (parameter.Name == "blockHash")
            {
                return (this.InParameters["block"].GetValue() as Nethereum.RPC.Eth.DTOs.Block).BlockHash;
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
