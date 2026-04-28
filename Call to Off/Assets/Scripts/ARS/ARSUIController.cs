using System.Collections;
using TMPro;
using UnityEngine;

public class ARSUIController : MonoBehaviour
{
    [Header("ARS 트리 데이터")]
    public ARSTreeData treeData;

    [Header("시작 노드")]
    public int startNodeId = 0;

    [Header("출력")]
    [SerializeField] private TMP_TypeByAnimationLength typeWriter;
    [SerializeField] private TMPSmartWrappedLayout smartLayout;

    [Header("Outro")]
    [SerializeField] private Outro outro;

    [Header("입력 버퍼 표시용 UI")]
    [SerializeField] private TMP_Text inputPreviewText;

    [Header("디버그 출력")]
    [SerializeField] private bool useDebugLog = true;

    [Header("공통 안내문")]
    [TextArea(2, 3)]
    [SerializeField] private string upperMenuGuide = "상위로 올라가고 싶다면 0번을 눌러주세요.";

    private ARSNodeData currentNode;
    private string inputBuffer = "";
    private SmartHomeDeviceType selectedCodeGuideDevice = SmartHomeDeviceType.None;

    private Coroutine outroFinishRoutine;
    private bool outroFinishCalled = false;

    private void Awake()
    {
        if (treeData == null)
        {
            Debug.LogError("ARSTreeData가 연결되지 않았습니다.");
            return;
        }

        // 플레이 시작할 때 딱 한 번 이번 세션용 랜덤값 생성
        treeData.InitializeRuntimeSession(true);

        if (useDebugLog)
        {
            Debug.Log("[ARSUIController] Awake에서 이번 플레이용 ARS 랜덤값 초기화 완료");
        }
    }

    private void Start()
    {
        if (treeData == null)
            return;

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

        if (nodeId == startNodeId || nodeId == 34 || nodeId == 3401)
            selectedCodeGuideDevice = SmartHomeDeviceType.None;

        if (useDebugLog)
            Debug.Log($"[ARS 이동] nodeId={nodeId}, nodeName={node.nodeName}, selectedCodeGuideDevice={selectedCodeGuideDevice}");

        RefreshUI();

        if (IsSuccessEndingNode())
        {
            StartOutroAfterTyping();
        }
    }

    private void StartOutroAfterTyping()
    {
        if (outroFinishCalled)
            return;

        if (outroFinishRoutine != null)
            StopCoroutine(outroFinishRoutine);

        outroFinishRoutine = StartCoroutine(CoWaitTypingAndCallOutro());
    }

    private IEnumerator CoWaitTypingAndCallOutro()
    {
        while (typeWriter != null && typeWriter.IsTyping)
        {
            yield return null;
        }

        if (!outroFinishCalled && outro != null)
        {
            outroFinishCalled = true;
            outro.Finish();

            if (useDebugLog)
                Debug.Log("[ARS] 성공 엔딩 후 Outro.Finish() 호출 완료");
        }

        outroFinishRoutine = null;
    }

    public void Click(int num)
    {
        if (currentNode == null) return;
        if (IsSuccessEndingNode()) return;
        if (typeWriter != null && typeWriter.IsTyping) return;

        if (currentNode.nodeType == ARSNodeType.NumberInput)
        {
            inputBuffer += num.ToString();

            if (useDebugLog)
                Debug.Log($"[ARS 입력] 현재 입력값: {inputBuffer}");

            RefreshInputPreview();
            return;
        }

        string input = num.ToString();

        if (currentNode.choices != null)
        {
            for (int i = 0; i < currentNode.choices.Count; i++)
            {
                if (currentNode.choices[i].inputKey == input)
                {
                    ShowNode(currentNode.choices[i].nextNodeId);
                    return;
                }
            }
        }

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
        if (currentNode == null) return;
        if (IsSuccessEndingNode()) return;
        if (typeWriter != null && typeWriter.IsTyping) return;
        if (currentNode.nodeType != ARSNodeType.NumberInput) return;
        if (string.IsNullOrEmpty(inputBuffer)) return;

        switch (currentNode.nodeId)
        {
            case 321:
                ValidatePhoneForPowerOff();
                return;

            case 3210:
                ValidatePowerOffDeviceCode();
                return;

            case 34:
                ValidatePhoneForCodeGuide();
                return;

            case 3401:
                ValidateCodeGuideDeviceNumber();
                return;

            case 3402:
                ValidateCodeGuideDetailCode();
                return;
        }

        if (currentNode.inputRule == null)
        {
            Debug.LogWarning($"노드 {currentNode.nodeId} 에 inputRule이 없습니다.");
            RefreshUI();
            return;
        }

        if (inputBuffer == currentNode.inputRule.correctValue)
        {
            if (currentNode.inputRule.successNodeId >= 0)
                ShowNode(currentNode.inputRule.successNodeId);
        }
        else
        {
            if (currentNode.inputRule.failNodeId >= 0)
                ShowNode(currentNode.inputRule.failNodeId);
            else
            {
                inputBuffer = "";
                RefreshUI();
            }
        }
    }

    private void ValidatePhoneForPowerOff()
    {
        if (inputBuffer == treeData.GetPhoneLast4())
            ShowNode(3210);
        else
            ShowNode(3211);
    }

    private void ValidatePowerOffDeviceCode()
    {
        SmartHomeDeviceType type = treeData.GetDeviceTypeByCode(inputBuffer);

        if (type == SmartHomeDeviceType.Aircon)
        {
            ShowNode(32107);
            return;
        }

        if (type == SmartHomeDeviceType.None)
        {
            ShowNode(321012);
            return;
        }

        ShowNode(321013);
    }

    private void ValidatePhoneForCodeGuide()
    {
        if (inputBuffer == treeData.GetPhoneLast4())
            ShowNode(3401);
        else
            ShowNode(34011);
    }

    private void ValidateCodeGuideDeviceNumber()
    {
        if (!int.TryParse(inputBuffer, out int number))
        {
            ShowNode(34012);
            return;
        }

        SmartHomeDeviceType type = treeData.GetDeviceTypeByNumber(number);

        if (type == SmartHomeDeviceType.None)
        {
            ShowNode(34012);
            return;
        }

        selectedCodeGuideDevice = type;
        ShowNode(3402);
    }

    private void ValidateCodeGuideDetailCode()
    {
        if (!int.TryParse(inputBuffer, out int code))
        {
            ShowNode(34028);
            return;
        }

        SmartHomeDeviceType detailOwner = treeData.GetDeviceTypeByDetailCode(code);

        if (detailOwner == SmartHomeDeviceType.None)
        {
            ShowNode(34028);
            return;
        }

        if (detailOwner != selectedCodeGuideDevice)
        {
            ShowNode(34029);
            return;
        }

        switch (selectedCodeGuideDevice)
        {
            case SmartHomeDeviceType.Light:
                ShowNode(34021);
                return;
            case SmartHomeDeviceType.Aircon:
                ShowNode(34022);
                return;
            case SmartHomeDeviceType.RobotVacuum:
                ShowNode(34023);
                return;
            case SmartHomeDeviceType.AirPurifier:
                ShowNode(34024);
                return;
        }
    }

    public void Backspace()
    {
        if (IsSuccessEndingNode()) return;
        if (typeWriter != null && typeWriter.IsTyping) return;
        if (string.IsNullOrEmpty(inputBuffer)) return;

        inputBuffer = inputBuffer.Substring(0, inputBuffer.Length - 1);
        RefreshInputPreview();
    }

    public void ClearInput()
    {
        if (IsSuccessEndingNode()) return;
        if (typeWriter != null && typeWriter.IsTyping) return;

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

            if (smartLayout != null && smartLayout.TargetText == typeWriter.TargetText)
            {
                string wrapped = smartLayout.ApplyBestLayout(finalText);
                typeWriter.Play(wrapped);
            }
            else
            {
                typeWriter.Play(finalText);
            }
        }

        RefreshInputPreview();
    }

    private string BuildDisplayedDialogue(ARSNodeData node)
    {
        if (node == null) return "";

        string text = treeData.FormatDialogue(node.dialogue ?? "");

        if (node.nodeId == startNodeId)
            return text;

        if (node.nodeType == ARSNodeType.NumberInput)
            return text;

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
        if (inputPreviewText == null) return;

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