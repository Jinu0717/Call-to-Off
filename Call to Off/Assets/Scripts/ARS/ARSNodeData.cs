using System;
using System.Collections.Generic;
using UnityEngine;

public enum ARSNodeType
{
    NormalMenu,
    NumberInput,
    EndingSuccess,
    EndingFail,
    Loop
}

[Serializable]
public class ARSInputRule
{
    [Header("정답 입력값")]
    public string correctValue;

    [Header("정답일 때 이동할 노드 (O)")]
    public int successNodeId = -1;

    [Header("오답일 때 이동할 노드 (X)")]
    public int failNodeId = -1;

    [TextArea(2, 5)]
    public string failMessage;
}

[Serializable]
public class ARSNodeData
{
    [Header("노드 ID")]
    public int nodeId;

    [Header("노드 이름")]
    public string nodeName;

    [Header("노드 타입")]
    public ARSNodeType nodeType;

    [Header("출력 대사")]
    [TextArea(3, 20)]
    public string dialogue;

    [Header("일반 메뉴 선택지")]
    public List<ARSChoice> choices = new List<ARSChoice>();

    [Header("숫자 입력 규칙")]
    public ARSInputRule inputRule;

    [Header("비용 증가량")]
    public float costIncrease = 1f;

    [Header("루프 노드 여부")]
    public bool autoLoop = false;

    [Header("루프 시 돌아갈 노드")]
    public int loopTargetNodeId = -1;
}