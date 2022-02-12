using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Avalanche.Nodes.SnowTrace
{
    [NodeDefinition("SnowTraceConnectorNode", "SnowTrace Connector", NodeTypeEnum.Connector, "Avalanche.SnowTrace")]
    [NodeSpecialActionAttribute("Go to SnowTrade API Portal", "open_url", "https://snowtrace.io/apis")]
    [NodeGraphDescription("Connection to the SnowTrace API")]
    public class SnowTraceConnectorNode : Node
    {
        public SnowTraceConnectorNode(string id, BlockGraph graph)
          : base(id, graph, typeof(SnowTraceConnectorNode).Name)
        {
            this.InParameters.Add("apiKey", new NodeParameter(this, "apiKey", typeof(string), true));

            this.OutParameters.Add("snowtraceConnection", new NodeParameter(this, "snowtraceConnection", typeof(SnowTraceConnectorNode), true));
        }


        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            this.Next();
        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "snowtraceConnection")
            {
                return this;
            }

            return base.ComputeParameterValue(parameter, value);
        }
    }
}
