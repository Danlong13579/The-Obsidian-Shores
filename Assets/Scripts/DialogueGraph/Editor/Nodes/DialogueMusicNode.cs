using System;
using UnityEngine;

[Serializable]
internal class DialogueMusicNode : BaseNode
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        AddExecutionPorts(context, true);

        context.AddInputPort<AudioClip>("Audio Clip").Build();
    }

    protected override void OnDefineOptions(IOptionDefinitionContext context)
    {
        context.AddOption<float>("Execution Delay");
        context.AddOption<bool>("is Playing");
    }
}
