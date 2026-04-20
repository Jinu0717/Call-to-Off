using System;
using UnityEngine;

[Serializable]
public class ARSChoice
{
    [Header("입력 번호")]
    public string inputKey;

    [Header("선택지 설명")]
    public string choiceText;

    [Header("이동할 다음 노드 ID")]
    public int nextNodeId;
}