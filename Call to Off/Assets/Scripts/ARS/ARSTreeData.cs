using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ARSTreeData", menuName = "CallToOff/ARS Tree Data")]
public class ARSTreeData : ScriptableObject
{
    public int startNodeId = 0;
    public List<ARSNodeData> nodes = new List<ARSNodeData>();

    private Dictionary<int, ARSNodeData> nodeDict;

    public void BuildDictionary()
    {
        nodeDict = new Dictionary<int, ARSNodeData>();

        for (int i = 0; i < nodes.Count; i++)
        {
            ARSNodeData node = nodes[i];
            if (node == null) continue;

            if (!nodeDict.ContainsKey(node.nodeId))
                nodeDict.Add(node.nodeId, node);
            else
                Debug.LogWarning($"중복된 nodeId 발견: {node.nodeId}");
        }
    }

    public ARSNodeData GetNode(int nodeId)
    {
        if (nodeDict == null || nodeDict.Count != nodes.Count)
            BuildDictionary();

        if (nodeDict.TryGetValue(nodeId, out ARSNodeData node))
            return node;

        Debug.LogWarning($"노드를 찾지 못했습니다. nodeId = {nodeId}");
        return null;
    }
}