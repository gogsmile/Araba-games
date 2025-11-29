using System.Collections.Generic;
using UnityEngine;

public enum FactionType { Corporates, Rebels, Hackers }

[System.Serializable]
public class Faction
{
    public FactionType Type;
    public int TrustLevel; // уровень доверия (0–100)
    public List<int> WantedFileIndexes = new List<int>(); // какие файлы они покупают
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

    /// <summary>
    /// Изменение доверия фракции
    /// </summary>
    public void ChangeTrust(FactionType type, int amount)
    {
        var faction = Factions.Find(f => f.Type == type);
        if (faction != null)
        {
            faction.TrustLevel += amount;
            faction.TrustLevel = Mathf.Clamp(faction.TrustLevel, 0, 100);
        }
    }

    /// <summary>
    /// Получение текущего доверия фракции
    /// </summary>
    public int GetTrust(FactionType type)
    {
        var faction = Factions.Find(f => f.Type == type);
        return faction != null ? faction.TrustLevel : 0;
    }

    /// <summary>
    /// Проверка, нужен ли фракции файл
    /// </summary>
    public bool IsFileWanted(FactionType type, int fileIndex)
    {
        var faction = Factions.Find(f => f.Type == type);
        if (faction == null) return false;
        return faction.WantedFileIndexes.Contains(fileIndex);
    }
}
