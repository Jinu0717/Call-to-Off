using System.Collections.Generic;
using UnityEngine;

public enum SmartHomeDeviceType
{
    None,
    Light,
    Aircon,
    RobotVacuum,
    AirPurifier
}

[System.Serializable]
public class ARSDeviceRuntimeInfo
{
    public SmartHomeDeviceType deviceType;
    public string deviceDisplayName;
    public int deviceNumber;
    public int detailCode;
    public string deviceCode;
}

[CreateAssetMenu(fileName = "ARSTreeData", menuName = "CallToOff/ARS Tree Data")]
public class ARSTreeData : ScriptableObject
{
    [Header("기본 트리")]
    public int startNodeId = 0;
    public List<ARSNodeData> nodes = new List<ARSNodeData>();

    [Header("현재 플레이에서 사용할 랜덤 값 (디버그 표시용)")]
    [SerializeField] private string phoneLast4 = "0000";

    [SerializeField]
    private ARSDeviceRuntimeInfo lightInfo = new ARSDeviceRuntimeInfo
    {
        deviceType = SmartHomeDeviceType.Light,
        deviceDisplayName = "거실 조명"
    };

    [SerializeField]
    private ARSDeviceRuntimeInfo airconInfo = new ARSDeviceRuntimeInfo
    {
        deviceType = SmartHomeDeviceType.Aircon,
        deviceDisplayName = "거실 스탠드형 에어컨"
    };

    [SerializeField]
    private ARSDeviceRuntimeInfo robotInfo = new ARSDeviceRuntimeInfo
    {
        deviceType = SmartHomeDeviceType.RobotVacuum,
        deviceDisplayName = "로봇 청소기"
    };

    [SerializeField]
    private ARSDeviceRuntimeInfo purifierInfo = new ARSDeviceRuntimeInfo
    {
        deviceType = SmartHomeDeviceType.AirPurifier,
        deviceDisplayName = "공기청정기"
    };

    private Dictionary<int, ARSNodeData> nodeDict;

    // 런타임 전용. 저장되지 않음.
    [System.NonSerialized] private bool sessionInitialized = false;

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

    public void InitializeRuntimeSession(bool forceRegenerate = false)
    {
        if (!Application.isPlaying) { return; }

        if (sessionInitialized && !forceRegenerate) { return; }

        GenerateRandomSessionValues();
        sessionInitialized = true;

        Debug.Log("[ARSTreeData] 이번 플레이용 랜덤 값 생성 완료");
    }

    [ContextMenu("현재 세션 랜덤 값 생성")]
    public void GenerateRandomSessionValues()
    {
        phoneLast4 = Random.Range(0, 10000).ToString("0000");

        List<int> deviceNumbers = new List<int> { 1, 2, 3, 4 };
        Shuffle(deviceNumbers);

        List<int> detailCodes = new List<int> { 4, 5, 7, 8 };
        Shuffle(detailCodes);

        lightInfo.deviceNumber = deviceNumbers[0];
        airconInfo.deviceNumber = deviceNumbers[1];
        robotInfo.deviceNumber = deviceNumbers[2];
        purifierInfo.deviceNumber = deviceNumbers[3];

        lightInfo.detailCode = detailCodes[0];
        airconInfo.detailCode = detailCodes[1];
        robotInfo.detailCode = detailCodes[2];
        purifierInfo.detailCode = detailCodes[3];

        HashSet<string> usedCodes = new HashSet<string>();
        lightInfo.deviceCode = CreateUnique4DigitCode(usedCodes);
        airconInfo.deviceCode = CreateUnique4DigitCode(usedCodes);
        robotInfo.deviceCode = CreateUnique4DigitCode(usedCodes);
        purifierInfo.deviceCode = CreateUnique4DigitCode(usedCodes);
    }

    private string CreateUnique4DigitCode(HashSet<string> usedCodes)
    {
        while (true)
        {
            string code = Random.Range(0, 10000).ToString("0000");
            if (usedCodes.Add(code)) { return code; }
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    public string GetPhoneLast4() => phoneLast4;

    public ARSDeviceRuntimeInfo GetDeviceInfo(SmartHomeDeviceType type)
    {
        switch (type)
        {
            case SmartHomeDeviceType.Light: return lightInfo;
            case SmartHomeDeviceType.Aircon: return airconInfo;
            case SmartHomeDeviceType.RobotVacuum: return robotInfo;
            case SmartHomeDeviceType.AirPurifier: return purifierInfo;
            default: return null;
        }
    }

    public bool IsValidDeviceNumber(int number)
    {
        return lightInfo.deviceNumber == number ||
               airconInfo.deviceNumber == number ||
               robotInfo.deviceNumber == number ||
               purifierInfo.deviceNumber == number;
    }

    public bool IsValidDetailCode(int code)
    {
        return lightInfo.detailCode == code ||
               airconInfo.detailCode == code ||
               robotInfo.detailCode == code ||
               purifierInfo.detailCode == code;
    }

    public bool IsValidDeviceCode(string code)
    {
        return lightInfo.deviceCode == code ||
               airconInfo.deviceCode == code ||
               robotInfo.deviceCode == code ||
               purifierInfo.deviceCode == code;
    }

    public SmartHomeDeviceType GetDeviceTypeByNumber(int number)
    {
        if (lightInfo.deviceNumber == number) return SmartHomeDeviceType.Light;
        if (airconInfo.deviceNumber == number) return SmartHomeDeviceType.Aircon;
        if (robotInfo.deviceNumber == number) return SmartHomeDeviceType.RobotVacuum;
        if (purifierInfo.deviceNumber == number) return SmartHomeDeviceType.AirPurifier;
        return SmartHomeDeviceType.None;
    }

    public SmartHomeDeviceType GetDeviceTypeByDetailCode(int code)
    {
        if (lightInfo.detailCode == code) return SmartHomeDeviceType.Light;
        if (airconInfo.detailCode == code) return SmartHomeDeviceType.Aircon;
        if (robotInfo.detailCode == code) return SmartHomeDeviceType.RobotVacuum;
        if (purifierInfo.detailCode == code) return SmartHomeDeviceType.AirPurifier;
        return SmartHomeDeviceType.None;
    }

    public SmartHomeDeviceType GetDeviceTypeByCode(string code)
    {
        if (lightInfo.deviceCode == code) return SmartHomeDeviceType.Light;
        if (airconInfo.deviceCode == code) return SmartHomeDeviceType.Aircon;
        if (robotInfo.deviceCode == code) return SmartHomeDeviceType.RobotVacuum;
        if (purifierInfo.deviceCode == code) return SmartHomeDeviceType.AirPurifier;
        return SmartHomeDeviceType.None;
    }

    public string FormatDialogue(string raw)
    {
        if (string.IsNullOrEmpty(raw))
            return "";

        return raw
            .Replace("{PHONE_LAST4}", GetPhoneLast4())

            .Replace("{LIGHT_DEVICE_NO}", lightInfo.deviceNumber.ToString())
            .Replace("{AIRCON_DEVICE_NO}", airconInfo.deviceNumber.ToString())
            .Replace("{ROBOT_DEVICE_NO}", robotInfo.deviceNumber.ToString())
            .Replace("{PURIFIER_DEVICE_NO}", purifierInfo.deviceNumber.ToString())

            .Replace("{LIGHT_DETAIL_CODE}", lightInfo.detailCode.ToString())
            .Replace("{AIRCON_DETAIL_CODE}", airconInfo.detailCode.ToString())
            .Replace("{ROBOT_DETAIL_CODE}", robotInfo.detailCode.ToString())
            .Replace("{PURIFIER_DETAIL_CODE}", purifierInfo.detailCode.ToString())

            .Replace("{LIGHT_DEVICE_CODE}", lightInfo.deviceCode)
            .Replace("{AIRCON_DEVICE_CODE}", airconInfo.deviceCode)
            .Replace("{ROBOT_DEVICE_CODE}", robotInfo.deviceCode)
            .Replace("{PURIFIER_DEVICE_CODE}", purifierInfo.deviceCode);
    }
}