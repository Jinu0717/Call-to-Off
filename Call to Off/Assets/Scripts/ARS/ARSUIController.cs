using TMPro;
using UnityEngine;

public class ARSUIController : MonoBehaviour
{
    private enum ResidenceType
    {
        None,
        Apartment,
        Officetel,
        DetachedHouse
    }

    [Header("ARS 트리 데이터")]
    public ARSTreeData treeData;

    [Header("시작 노드")]
    public int startNodeId = 0;

    [Header("출력")]
    [SerializeField] private TMP_TypeByAnimationLength typeWriter;

    [Header("입력 버퍼 표시용 UI")]
    [SerializeField] private TMP_Text inputPreviewText;

    [Header("디버그 출력")]
    [SerializeField] private bool useDebugLog = true;

    [Header("공통 안내문")]
    [TextArea(2, 3)]
    [SerializeField] private string upperMenuGuide = "다시 듣고 싶다면 9번, 상위로 올라가고 싶다면 0번을 눌러주세요.";

    private ARSNodeData currentNode;
    private string inputBuffer = "";
    private ResidenceType selectedResidenceType = ResidenceType.None;

    private void Start()
    {
        if (treeData == null)
        {
            Debug.LogError("ARSTreeData가 연결되지 않았습니다.");
            return;
        }

        treeData.BuildDictionary();
        ShowNode(startNodeId);
    }

    private bool IsSuccessEndingNode()
    {
        return currentNode != null && currentNode.nodeType == ARSNodeType.EndingSuccess;
    }

    public void ShowNode(int nodeId)
    {
        ARSNodeData node = treeData.GetNode(nodeId);

        if (node == null)
        {
            Debug.LogWarning($"노드 ID {nodeId} 를 찾지 못했습니다.");
            return;
        }

        currentNode = node;
        inputBuffer = "";

        UpdateResidenceStateByNode(nodeId);

        if (useDebugLog)
            Debug.Log($"[ARS 이동] nodeId={nodeId}, nodeName={node.nodeName}, residence={selectedResidenceType}");

        RefreshUI();
    }

    public void Click(int num)
    {
        if (currentNode == null)
        {
            Debug.LogWarning("현재 노드가 없습니다.");
            return;
        }

        // 성공 엔딩이면 클릭 입력 완전 차단
        if (IsSuccessEndingNode())
        {
            if (useDebugLog)
                Debug.Log("[ARS] 성공 엔딩 노드에서는 입력이 비활성화됩니다.");
            return;
        }

        // 텍스트 출력 중이면 입력 무시
        if (typeWriter != null && typeWriter.IsTyping)
            return;

        if (currentNode.nodeType == ARSNodeType.NumberInput)
        {
            inputBuffer += num.ToString();

            if (useDebugLog)
                Debug.Log($"[ARS 입력] 현재 입력값: {inputBuffer}");

            RefreshInputPreview();
            return;
        }

        string input = num.ToString();

        // 먼저 선택지 우선 처리
        if (currentNode.choices != null)
        {
            for (int i = 0; i < currentNode.choices.Count; i++)
            {
                ARSChoice choice = currentNode.choices[i];

                if (choice.inputKey == input)
                {
                    ShowNode(choice.nextNodeId);
                    return;
                }
            }
        }

        // 공통 기능
        if (num == 0)
        {
            ShowNode(startNodeId);
            return;
        }

        if (num == 9)
        {
            RefreshUI();
            return;
        }

        RefreshUI();
    }

    public void PressSharp()
    {
        if (currentNode == null)
            return;

        // 성공 엔딩이면 입력 차단
        if (IsSuccessEndingNode())
            return;

        if (typeWriter != null && typeWriter.IsTyping)
            return;

        if (currentNode.nodeType != ARSNodeType.NumberInput)
        {
            RefreshUI();
            return;
        }

        if (currentNode.inputRule == null)
        {
            Debug.LogWarning($"노드 {currentNode.nodeId} 에 inputRule이 없습니다.");
            RefreshUI();
            return;
        }

        if (string.IsNullOrEmpty(inputBuffer))
        {
            if (useDebugLog)
                Debug.Log("[ARS 확인] 입력값이 비어 있습니다.");

            RefreshUI();
            return;
        }

        if (useDebugLog)
            Debug.Log($"[ARS 확인] node={currentNode.nodeId}, 입력값={inputBuffer}, 정답={currentNode.inputRule.correctValue}, residence={selectedResidenceType}");

        if (currentNode.nodeId == 32102)
        {
            ValidateAirconDeviceNumber();
            return;
        }

        if (currentNode.nodeId == 321027)
        {
            ValidateDetailCode();
            return;
        }

        if (inputBuffer == currentNode.inputRule.correctValue)
        {
            if (useDebugLog)
                Debug.Log("[ARS 결과] 정답");

            if (currentNode.inputRule.successNodeId >= 0)
                ShowNode(currentNode.inputRule.successNodeId);
            else
                RefreshUI();
        }
        else
        {
            if (useDebugLog)
                Debug.Log("[ARS 결과] 오답");

            if (currentNode.inputRule.failNodeId >= 0)
                ShowNode(currentNode.inputRule.failNodeId);
            else
            {
                inputBuffer = "";
                RefreshUI();
            }
        }
    }

    private void ValidateAirconDeviceNumber()
    {
        if (selectedResidenceType == ResidenceType.None)
        {
            ShowNode(32104);
            return;
        }

        if (selectedResidenceType != ResidenceType.Apartment)
        {
            ShowNode(32104);
            return;
        }

        if (inputBuffer == "2")
            ShowNode(321027);
        else
            ShowNode(321021);
    }

    private void ValidateDetailCode()
    {
        if (selectedResidenceType == ResidenceType.None)
        {
            ShowNode(32105);
            return;
        }

        if (selectedResidenceType != ResidenceType.Apartment)
        {
            ShowNode(32105);
            return;
        }

        if (inputBuffer == "7")
            ShowNode(32107);
        else
            ShowNode(32103);
    }

    private void UpdateResidenceStateByNode(int nodeId)
    {
        switch (nodeId)
        {
            case 331:
                selectedResidenceType = ResidenceType.Apartment;
                break;
            case 332:
                selectedResidenceType = ResidenceType.Officetel;
                break;
            case 333:
                selectedResidenceType = ResidenceType.DetachedHouse;
                break;
        }
    }

    public void Backspace()
    {
        if (IsSuccessEndingNode())
            return;

        if (typeWriter != null && typeWriter.IsTyping)
            return;

        if (string.IsNullOrEmpty(inputBuffer))
            return;

        inputBuffer = inputBuffer.Substring(0, inputBuffer.Length - 1);
        RefreshInputPreview();
    }

    public void ClearInput()
    {
        if (IsSuccessEndingNode())
            return;

        if (typeWriter != null && typeWriter.IsTyping)
            return;

        inputBuffer = "";
        RefreshInputPreview();
    }

    private void RefreshUI()
    {
        if (currentNode == null)
            return;

        string finalText = BuildDisplayedDialogue(currentNode);

        if (typeWriter != null)
        {
            typeWriter.StopTypingRoutineOnly();
            typeWriter.Play(finalText);
        }

        RefreshInputPreview();
    }

    private string BuildDisplayedDialogue(ARSNodeData node)
    {
        if (node == null)
            return "";

        string text = node.dialogue ?? "";

        if (node.nodeId == startNodeId)
            return text;

        if (node.nodeType == ARSNodeType.NumberInput)
            return text;

        // 성공 엔딩은 안내문 붙이지 않음
        if (node.nodeType == ARSNodeType.EndingSuccess)
            return text;

        if (!string.IsNullOrWhiteSpace(upperMenuGuide))
        {
            if (!string.IsNullOrWhiteSpace(text))
                text += "\n";

            text += upperMenuGuide;
        }

        return text;
    }

    private void RefreshInputPreview()
    {
        if (inputPreviewText == null)
            return;

        if (currentNode != null && currentNode.nodeType == ARSNodeType.NumberInput)
            inputPreviewText.text = inputBuffer;
        else
            inputPreviewText.text = "";
    }

    public void RefreshCurrentNode()
    {
        RefreshUI();
    }
}