using System.Collections.Generic;
using UnityEngine;

public enum FactionType
{
    Corporates,
    Rebels,
    Hackers
}

[System.Serializable]
public class Faction
{
    public FactionType Type;

    /// <summary>
    /// Уровень доверия (float). 
    /// Диапазон 0.0–2.0. 
    /// 0   = 0%
    /// 1.0 = 50%
    /// 2.0 = 100%
    /// </summary>
    [Range(0f, 2f)]
    public float TrustLevel = 1f;

    /// <summary>
    /// Индексы файлов, которые фракция хочет.
    /// </summary>
    public List<int> WantedFileIndexes = new List<int>();
}

public class FactionManager : MonoBehaviour
{
    public static FactionManager Instance;

    [Header("Фракции")]
    public List<Faction> Factions = new List<Faction>();

    [Header("Настройки изменения доверия")]
    [Tooltip("Сколько добавляем дробного доверия за удачный файл.")]
    public float TrustGainOnSuccessfulFile = 0.1f;

    [Tooltip("Сколько отнимаем дробного доверия за неудачный файл.")]
    public float TrustLossOnUnsuccessfulFile = 0.1f;

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
        foreach (var f in Factions)
        {
            if (f != null && f.Type == type)
                return f;
        }
        return null;
    }

    /// <summary>
    /// Изменяет доверие фракции, поддерживает дробные значения.
    /// </summary>
    public void ChangeTrust(FactionType type, float amount)
    {
        Faction faction = GetFactionInfo(type);
        if (faction == null)
            return;

        faction.TrustLevel += amount;

        if (faction.TrustLevel < 0f)
            faction.TrustLevel = 0f;
        else if (faction.TrustLevel > 2f)
            faction.TrustLevel = 2f;

        if (FactionUIManager.Instance != null)
            FactionUIManager.Instance.UpdateFactionUI();
    }

    public float GetTrust(FactionType type)
    {
        Faction faction = GetFactionInfo(type);
        return faction != null ? faction.TrustLevel : 0f;
    }

    /// <summary>
    /// Trust → Percent.
    /// 1.0 trust = 50%
    /// </summary>
    public float GetTrustPercent(FactionType type)
    {
        return GetTrust(type) * 50f;
    }

    public bool IsFileWanted(FactionType type, int fileIndex)
    {
        Faction faction = GetFactionInfo(type);
        return faction != null &&
               faction.WantedFileIndexes != null &&
               faction.WantedFileIndexes.Contains(fileIndex);
    }

    /// <summary>
    /// Множитель награды:
    /// 0.0 trust → 0.0 multiplier
    /// 1.0 trust → 0.5 multiplier
    /// 2.0 trust → 1.0 multiplier
    /// </summary>
    public float GetTrustMultiplier(FactionType type)
    {
        return GetTrust(type) * 0.5f;
    }
}
