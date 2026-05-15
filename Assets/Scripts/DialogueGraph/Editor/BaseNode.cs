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
    protected void AddExecutionPorts(IPortDefinitionContext context)
    {
        context
            .AddInputPort(ExecutionPorts.InPort)
            .WithDisplayName(String.Empty)
            .WithConnectorUI(PortConnectorUI.Arrowhead)
            .Build();
        context
            .AddOutputPort(ExecutionPorts.OutPort)
            .WithDisplayName(String.Empty)
            .WithConnectorUI(PortConnectorUI.Arrowhead)
            .Build();
    }
}
