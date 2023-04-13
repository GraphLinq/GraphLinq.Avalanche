using Nethereum.JsonRpc.Client.Streaming;
using Nethereum.RPC.Eth.Subscriptions;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using NodeBlock.Plugin.Avalanche;

namespace NodeBlock.Plugin.Avalanche.Nodes
{
    [NodeDefinition("OnNewAaxTransactionEventNode", "On Avalanche Transaction", NodeTypeEnum.Event, "Blockchain.Avalanche")]
    [NodeGraphDescription("Event that occurs everytime a new avalanche transaction appears in the last network block")]
    public class OnNewAvaxTransactionEventNode : Node, IEventEthereumNode
    {
        private EthNewPendingTransactionSubscription ethNewPendingTransactionSubscription;

        public OnNewAvaxTransactionEventNode(string id, BlockGraph graph)
            : base(id, graph, typeof(OnNewAvaxTransactionEventNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(string), true));

            this.OutParameters.Add("transactionHash", new NodeParameter(this, "transactionHash", typeof(string), true));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            AvaxConnection avaxConnection = this.InParameters["connection"].GetValue() as AvaxConnection;
            if (avaxConnection.UseManaged)
            {
                ethNewPendingTransactionSubscription = Plugin.EventsManagerAvalanche.NewEventTypePendingTxs(this);
            }
            else
            {
                this.ethNewPendingTransactionSubscription = new EthNewPendingTransactionSubscription(avaxConnection.SocketClient);
                ethNewPendingTransactionSubscription.SubscriptionDataResponse += OnEventNode;
                ethNewPendingTransactionSubscription.SubscribeAsync().Wait();
            }
        }

        public override void OnStop()
        {
            AvaxConnection avaxConnection = this.InParameters["connection"].GetValue() as AvaxConnection;
            if (avaxConnection.UseManaged)
            {
                string eventType = ethNewPendingTransactionSubscription.GetType().ToString();
                Plugin.EventsManagerAvalanche.RemoveEventNode(eventType, this);
                return;
            }
            this.ethNewPendingTransactionSubscription.UnsubscribeAsync().Wait();
        }

        public void OnEventNode(object sender, dynamic e)
        {
            StreamingEventArgs<string> eventData = e;
            if (eventData.Response == null) return;
            var instanciatedParameters = this.InstanciatedParametersForCycle();
            instanciatedParameters["transactionHash"].SetValue(eventData.Response);

            this.Graph.AddCycle(this, instanciatedParameters);
        }
        public override void BeginCycle()
        {
            this.Next();
        }

    }
}
