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
                ProcessDialogueNode(dialogueNode, runtimeNode, nodeIDMap);
            }

            runtimeGraphSO.AllNodes.Add(runtimeNode);
        }

        ctx.AddObjectToAsset("RuntimeData", runtimeGraphSO);
        ctx.SetMainObject(runtimeGraphSO);
    }

    private void ProcessDialogueNode(
        DialogueNode node,
        RuntimeDialogueNode runtimeNode,
        Dictionary<INode, string> nodeIDMap
    )
    {
        runtimeNode.SpeakerName = GetPortValue<string>(
            node.GetInputPortByName(DialoguePorts.SpeakerPort)
        );
        runtimeNode.DialogueText = GetPortValue<string>(
            node.GetInputPortByName(DialoguePorts.TextPort)
        );

        var nextNodePort = node.GetOutputPortByName(ExecutionPorts.OutPort)?.FirstConnectedPort;

        if (nextNodePort != null)
            runtimeNode.NextNodeID = nodeIDMap[nextNodePort.GetNode()];
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
        }

        // No port conection data is hard coded
        port.TryGetValue(out T hardcodedValue);
        return hardcodedValue;
    }
}
