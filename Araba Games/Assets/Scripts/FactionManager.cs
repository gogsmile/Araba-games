using System.Collections.Generic;
using UnityEngine;

public enum FactionType { Corporates, Rebels, Hackers }

[System.Serializable]
public class Faction
{
    public FactionType Type;
    public int TrustLevel = 50; // 0–100, стартовое доверие
    public List<int> WantedFileIndexes = new List<int>();
}

public class FactionManager : MonoBehaviour
{
    public static FactionManager Instance;

    public List<Faction> Factions = new List<Faction>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public Faction GetFactionInfo(FactionType type)
    {
        return Factions.Find(f => f.Type == type);
    }

    public void ChangeTrust(FactionType type, int amount)
    {
        var faction = GetFactionInfo(type);
        if (faction != null)
        {
            faction.TrustLevel += amount;
            faction.TrustLevel = Mathf.Clamp(faction.TrustLevel, 0, 100);
        }
    }

    public int GetTrust(FactionType type)
    {
        var faction = GetFactionInfo(type);
        return faction != null ? faction.TrustLevel : 0;
    }

    public bool IsFileWanted(FactionType type, int fileIndex)
    {
        var faction = GetFactionInfo(type);
        if (faction == null) return false;

        return faction.WantedFileIndexes.Contains(fileIndex);
    }

    /// <summary>
    /// Получаем множитель прибыли 0.0 → 1.0
    /// </summary>
    public float GetTrustMultiplier(FactionType type)
    {
        return GetTrust(type) / 100f;
    }
}
