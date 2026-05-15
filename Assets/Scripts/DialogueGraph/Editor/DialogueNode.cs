using System;
using UnityEngine;

struct DialoguePorts
{
    public const string SpeakerPort = "Speaker";
    public const string TextPort = "Text";
    public const string SpritePort = "Sprite";
}

[Serializable]
internal class DialogueNode : BaseNode
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        AddExecutionPorts(context);

        context.AddInputPort<string>(DialoguePorts.SpeakerPort).Build();
        context.AddInputPort<Sprite>(DialoguePorts.SpritePort).Build();
        context.AddInputPort<string>(DialoguePorts.TextPort).Build();
    }
}
