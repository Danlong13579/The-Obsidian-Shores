using System;
using JetBrains.Annotations;
using Unity.GraphToolkit.Editor;
using UnityEngine;

struct DialogueNodePorts
{
    public const string SpeakerPort = "Speaker";
    public const string TextPort = "Text";
    public const string SpritePort = "Sprite";
    public const string ChoiceText = "Choice text";
    public const string ChoiceOut = "Choice ";
}

struct DialogueNodeOptions
{
    public const string HasDelay = "HasDelay";
}

[Serializable]
internal class DialogueNode : BaseNode
{
    public const string portCount = "Choice Count";

    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        var portCountOption = GetNodeOptionByName(portCount);

        portCountOption.TryGetValue(out int choiceCount);
        bool shouldBuildOutPort = choiceCount <= 0;

        AddExecutionPorts(context, shouldBuildOutPort);

        context.AddInputPort<string>(DialogueNodePorts.SpeakerPort).Build();
        context.AddInputPort<string>(DialogueNodePorts.TextPort).Build();

        AddDelayPort(context);

        AddChoicePorts(context);
    }

    protected override void OnDefineOptions(IOptionDefinitionContext context)
    {
        context
            .AddOption<bool>(DialogueNodeOptions.HasDelay)
            .WithDisplayName("Has Delay")
            .WithDefaultValue(false);
        // Add has seen option that shows extra text for returning players
        context.AddOption<int>(portCount).WithDefaultValue(0).Delayed();
    }

    void AddDelayPort(IPortDefinitionContext context)
    {
        var hasDelay = GetNodeOptionByName(DialogueNodeOptions.HasDelay);

        hasDelay.TryGetValue(out bool hasDelayValue);
        if (hasDelayValue == true)
        {
            context.AddInputPort<float>("Delay Time").WithDefaultValue(1.5f).Build();
        }
    }

    void AddChoicePorts(IPortDefinitionContext context)
    {
        var portCountOption = GetNodeOptionByName(portCount);

        portCountOption.TryGetValue(out int choiceCount);
        for (int i = 0; i < choiceCount; i++)
        {
            context.AddInputPort<DialogueChoice>($"{DialogueNodePorts.ChoiceText} {i}").Build();
            context
                .AddOutputPort($"{DialogueNodePorts.ChoiceOut} {i}")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }
}
