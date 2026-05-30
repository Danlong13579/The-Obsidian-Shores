using System;
using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

[ScriptedImporter(1, DialogueGraph.AssetExtension)]
public class DialogueGraphImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        DialogueGraph editorGraph = GraphDatabase.LoadGraphForImporter<DialogueGraph>(
            ctx.assetPath
        );
        RuntimeDialogueGraph runtimeGraphSO =
            ScriptableObject.CreateInstance<RuntimeDialogueGraph>();

        var nodeIDMap = new Dictionary<INode, string>();

        foreach (var node in editorGraph.GetNodes())
        {
            nodeIDMap[node] = Guid.NewGuid().ToString();
        }

        var startNode = editorGraph.GetNodes().OfType<StartNode>().FirstOrDefault();

        if (startNode != null)
        {
            var entryPort = startNode.GetOutputPorts().FirstOrDefault()?.FirstConnectedPort;
            if (entryPort != null)
            {
                runtimeGraphSO.EntryNodeID = nodeIDMap[entryPort.GetNode()];
            }
        }

        foreach (var iNode in editorGraph.GetNodes())
        {
            if (iNode is StartNode || iNode is EndNode)
                continue;

            // Test to remove value nodes that are processed
            if (iNode is not DialogueNode)
                continue;

            var runtimeNode = new RuntimeDialogueNode { NodeID = nodeIDMap[iNode] };

            if (iNode is DialogueNode dialogueNode)
            {
                dialogueNode.GetNodeOptionByName("Choice Count").TryGetValue(out int choiceCount);
                bool hasChoices = choiceCount > 0;

                if (!hasChoices)
                {
                    ProcessBasicDialogueNode(dialogueNode, runtimeNode, nodeIDMap);
                }
                else if (hasChoices)
                {
                    ProcessDialogueNodeWithChoices(dialogueNode, runtimeNode, nodeIDMap);
                }
            }

            runtimeGraphSO.AllNodes.Add(runtimeNode);
        }

        ctx.AddObjectToAsset("RuntimeData", runtimeGraphSO);
        ctx.SetMainObject(runtimeGraphSO);
    }

    private void ProcessBasicDialogueNode(
        DialogueNode node,
        RuntimeDialogueNode runtimeNode,
        Dictionary<INode, string> nodeIDMap
    )
    {
        SetBaseDialogueData(node, runtimeNode);

        var nextNodePort = node.GetOutputPortByName(ExecutionPorts.OutPort)?.FirstConnectedPort;

        if (nextNodePort != null)
        {
            var nextNode = nextNodePort.GetNode();

            if (nextNode is not EndNode)
                runtimeNode.NextNodeID = nodeIDMap[nextNode];

            if (nextNode is EndNode)
                runtimeNode.isLastNode = true;
        }

        var hasDelayOption = GetOptionValue<bool>(
            node.GetNodeOptionByName(DialogueNodeOptions.HasDelay)
        );
        runtimeNode.HasDelay = hasDelayOption;

        if (hasDelayOption)
            runtimeNode.DelayTime = GetPortValue<float>(node.GetInputPortByName("Delay Time"));

        var choicePort = GetPortValue<DialogueChoice>(node.GetInputPortByName("Choice text 0"));
    }

    private void ProcessDialogueNodeWithChoices(
        DialogueNode node,
        RuntimeDialogueNode runtimeNode,
        Dictionary<INode, string> nodeIDMap
    )
    {
        // Handle processing choices and there next node uuid
        SetBaseDialogueData(node, runtimeNode);
    }

    private void SetBaseDialogueData(DialogueNode node, RuntimeDialogueNode runtimeNode)
    {
        runtimeNode.SpeakerName = GetPortValue<string>(
            node.GetInputPortByName(DialogueNodePorts.SpeakerPort)
        );
        runtimeNode.DialogueText = GetPortValue<string>(
            node.GetInputPortByName(DialogueNodePorts.TextPort)
        );
    }

    private T GetPortValue<T>(IPort port)
    {
        if (port == null)
            return default;

        // If port is connected grab the connected data
        if (port.IsConnected)
        {
            if (port.FirstConnectedPort.GetNode() is IVariableNode variableNode)
            {
                variableNode.Variable.TryGetDefaultValue(out T connectedValue);
                return connectedValue;
            }

            if (port.FirstConnectedPort.GetNode() is DialogueChoiceNode choice)
            {
                choice.GetInputPortByName("Affinity").TryGetValue(out Affinities affinity);
                Debug.Log(affinity);
            }
        }

        // No port connection data is hard coded
        port.TryGetValue(out T hardcodedValue);
        return hardcodedValue;
    }

    private T GetOptionValue<T>(INodeOption option)
    {
        if (option == null)
            return default;

        option.TryGetValue(out T optionValue);
        return optionValue;
    }

    private void LogPorts(INode node)
    {
        Debug.Log("=== All Input Ports on DialogueNode ===");
        foreach (var port in node.GetInputPorts())
        {
            Debug.Log($"Port name: '{port.DisplayName}' | '{port.Name}'");
        }
        Debug.Log("=====================================");
    }
}
