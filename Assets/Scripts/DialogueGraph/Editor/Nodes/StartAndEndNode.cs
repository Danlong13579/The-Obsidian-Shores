using System;
using Unity.GraphToolkit.Editor;

[Serializable]
public class StartNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context
            .AddOutputPort(String.Empty)
            .WithDisplayName(String.Empty)
            .WithConnectorUI(PortConnectorUI.Arrowhead)
            .Build();
    }
}

[Serializable]
public class EndNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context
            .AddInputPort(String.Empty)
            .WithDisplayName(String.Empty)
            .WithConnectorUI(PortConnectorUI.Arrowhead)
            .Build();
    }
}
