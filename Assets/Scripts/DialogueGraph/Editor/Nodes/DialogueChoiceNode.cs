using System;
using Unity.GraphToolkit.Editor;

[Serializable]
internal class DialogueChoiceNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort<string>("Choice Text").Build();
        context.AddInputPort<Affinities>("Affinity").Build();

        // Output port for DialogueChoice
        context
            .AddOutputPort<DialogueChoice>("Dialogue Choice")
            .WithDisplayName(string.Empty)
            .Build();
    }
}
