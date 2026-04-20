using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ARSGameState
{
    public float currentCost = 0f;

    [SerializeField]
    private List<string> unlockedFlags = new List<string>();

    private HashSet<string> flagSet = new HashSet<string>();

    public void Init() => flagSet = new HashSet<string>(unlockedFlags);

    public void AddFlag(string flagKey)
    {
        if (string.IsNullOrWhiteSpace(flagKey)) { return; }

        if (flagSet == null) { Init(); }

        if (flagSet.Add(flagKey))
        {
            unlockedFlags.Add(flagKey);
            Debug.Log($"ÇÃ·¡±× È¹µæ: {flagKey}");
        }
    }

    public bool HasFlag(string flagKey)
    {
        if (flagSet == null) { Init(); }

        return flagSet.Contains(flagKey);
    }

    public string GetAllFlagsText()
    {
        if (unlockedFlags == null || unlockedFlags.Count == 0) { return "¾øÀ½"; }

        return string.Join(", ", unlockedFlags);
    }

    public void ResetState()
    {
        currentCost = 0f;
        unlockedFlags.Clear();
        flagSet.Clear();
    }
}