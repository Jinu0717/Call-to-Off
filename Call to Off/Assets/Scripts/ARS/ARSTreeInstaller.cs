using UnityEngine;

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

        Debug.Log("전체 ARS 데이터 입력 완료");
    }
}