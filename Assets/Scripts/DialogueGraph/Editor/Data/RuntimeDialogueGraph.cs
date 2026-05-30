using System;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueNodeTypeNames
{
    DialogueNode,
}

public class RuntimeDialogueGraph : ScriptableObject
{
    public string EntryNodeID;
    public List<RuntimeDialogueNode> AllNodes = new();
}

[Serializable]
public class RuntimeDialogueNode
{
    [Header("Node Data")]
    public string NodeID;
    public string NextNodeID;
    public bool isLastNode;

    [Header("Basic Dialogue Data")]
    public string SpeakerName;
    public string DialogueText;

    [Header("Timed Node Info")]
    public bool HasDelay;
    public float DelayTime;
}
