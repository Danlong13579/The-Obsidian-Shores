using System;
using Unity.GraphToolkit.Editor;

struct DialogueChoicePorts
{
    public const string InPort = "in";
    public const string SpeakerPort = "Speaker";
    public const string TextPort = "Text";
    public const string ChoiceText = "Choice text";
    public const string ChoiceOut = "Choice ";
}

[Serializable]
internal class DialogueChoiceNode : Node
{
    public const string portCount = "Choice Count";

    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context
            .AddInputPort(DialogueChoicePorts.InPort)
            .WithDisplayName(String.Empty)
            .WithConnectorUI(PortConnectorUI.Arrowhead)
            .Build();

        context.AddInputPort<string>(DialogueChoicePorts.SpeakerPort).Build();
        context.AddInputPort<string>(DialogueChoicePorts.TextPort).Build();

        var portCountOption = GetNodeOptionByName(portCount);

        portCountOption.TryGetValue(out int choiceCount);
        for (int i = 0; i < choiceCount; i++)
        {
            context.AddInputPort<string>($"{DialogueChoicePorts.ChoiceText} {i}").Build();
            context
                .AddOutputPort($"{DialogueChoicePorts.ChoiceOut} {i}")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }

    protected override void OnDefineOptions(IOptionDefinitionContext context)
    {
        context.AddOption<int>(portCount);
    }
}
