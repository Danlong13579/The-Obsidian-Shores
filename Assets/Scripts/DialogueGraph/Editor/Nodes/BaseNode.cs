using System;
using Unity.GraphToolkit.Editor;

struct ExecutionPorts
{
    public const string InPort = "in";
    public const string OutPort = "out";
}

[Serializable]
abstract class BaseNode : Node
{
    protected void AddExecutionPorts(IPortDefinitionContext context, bool shouldBuildOutPort)
    {
        context
            .AddInputPort(ExecutionPorts.InPort)
            .WithDisplayName(string.Empty)
            .WithConnectorUI(PortConnectorUI.Arrowhead)
            .Build();

        if (shouldBuildOutPort)
            context
                .AddOutputPort(ExecutionPorts.OutPort)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
    }
}
