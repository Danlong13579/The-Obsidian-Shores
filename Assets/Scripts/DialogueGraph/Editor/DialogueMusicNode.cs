using System;
using UnityEngine;

[Serializable]
internal class DialogueMusicNode : BaseNode
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        AddExecutionPorts(context);

        context.AddInputPort<AudioClip>("Audio Clip").Build();
    }

    protected override void OnDefineOptions(IOptionDefinitionContext context)
    {
        context.AddOption<float>("delay");
        context.AddOption<bool>("is Playing");
    }
}
