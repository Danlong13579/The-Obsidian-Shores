using System;
using Unity.GraphToolkit.Editor;
using UnityEditor;

[Serializable]
[Graph(AssetExtension)]
public class DialogueGraph : Graph
{
    public const string AssetExtension = "dialoguegraph";

    [MenuItem("Assets/Create/Graph Toolkit/Dialogue Graph", false)]
    static void CreateAssetFile()
    {
        GraphDatabase.PromptInProjectBrowserToCreateNewAsset<DialogueGraph>();
    }
}
