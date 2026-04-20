using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ARSTreeInstaller : MonoBehaviour
{
    public ARSTreeData treeData;

    [ContextMenu("전체 ARS 데이터 채우기")]
    public void FillData()
    {
        if (treeData == null)
        {
            Debug.LogError("ARSTreeData가 연결되지 않았습니다.");
            return;
        }

        treeData.startNodeId = 0;
        treeData.nodes = ARSDataFactory.CreateAllNodes();
        treeData.BuildDictionary();

#if UNITY_EDITOR
        EditorUtility.SetDirty(treeData);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif

        Debug.Log("전체 ARS 데이터 입력 완료 및 저장 완료");
    }
}