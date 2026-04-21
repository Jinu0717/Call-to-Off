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

        if (Application.isPlaying)
        {
            Debug.LogWarning("플레이 모드에서는 생성 데이터를 영구 저장하지 않는 것이 좋습니다. 플레이 모드를 종료한 뒤 다시 실행하세요.");
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