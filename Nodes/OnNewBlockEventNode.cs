using Nethereum.JsonRpc.Client.Streaming;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Subscriptions;
using Nethereum.RPC.Reactive.Eth.Subscriptions;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Avalanche.Nodes
{
    [NodeDefinition("OnNewAvaxBlockEventNode", "On Avalanche Block", NodeTypeEnum.Event, "Blockchain.Avalanche")]
    [NodeGraphDescription("Event that occurs everytime a new avalanche block is minted")]
    public class OnNewAvaxBlockEventNode : Node, IEventEthereumNode
    {
        private EthNewBlockHeadersObservableSubscription blockHeadersSubscription;

        public OnNewAvaxBlockEventNode(string id, BlockGraph graph)
            : base(id, graph, typeof(OnNewAvaxBlockEventNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(string), true));

            this.OutParameters.Add("block", new NodeParameter(this, "block", typeof(Nethereum.RPC.Eth.DTOs.Block), true));

        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            AvaxConnection avaxConnection = this.InParameters["connection"].GetValue() as AvaxConnection;
            if (avaxConnection.UseManaged)
            {
                blockHeadersSubscription = Plugin.EventsManagerAvalanche.NewEventTypePendingBlocks(this);
            }
            else
            {
                this.blockHeadersSubscription = new EthNewBlockHeadersObservableSubscription(avaxConnection.SocketClient);
                blockHeadersSubscription.GetSubscriptionDataResponsesAsObservable().Subscribe(async Block =>
                {
                    var instanciatedParameters = this.InstanciateParametersForCycle();
                    instanciatedParameters["block"].SetValue(Block);

                    this.Graph.AddCycle(this, instanciatedParameters);
                });
                blockHeadersSubscription.SubscribeAsync();
            }

        }

        public override void OnStop()
        {
            AvaxConnection avaxConnection = this.InParameters["connection"].GetValue() as AvaxConnection;
            if (avaxConnection.UseManaged)
            {
                string eventType = blockHeadersSubscription.GetType().ToString();
                Plugin.EventsManagerAvalanche.RemoveEventNode(eventType, this);
                return;
            }
            this.blockHeadersSubscription.UnsubscribeAsync().Wait();
        }

        public override void BeginCycle()
        {
            this.Next();
        }

        public void OnEventNode(object sender, dynamic e)
        {
            var instanciatedParameters = this.InstanciateParametersForCycle();
            instanciatedParameters["block"].SetValue(e);

            this.Graph.AddCycle(this, instanciatedParameters);
        }
    }
}
