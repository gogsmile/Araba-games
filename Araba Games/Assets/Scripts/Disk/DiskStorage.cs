using System.Collections.Generic;
using UnityEngine;

public class DiskStorage : MonoBehaviour
{
    public static DiskStorage Instance;

    [Header("Disk Settings")]
    public int Capacity = 100;
    public List<FileData> StoredFiles = new List<FileData>();

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

    public bool HasSpace()
    {
        return StoredFiles.Count < Capacity;
    }

    public bool AddFile(FileData file)
    {
        if (!HasSpace())
        {
            Debug.Log("Disk is full, cannot add file.");
            return false;
        }

        StoredFiles.Add(file);
        return true;
    }

    public void RemoveFile(FileData file)
    {
        StoredFiles.Remove(file);
    }

    /// <summary>
    /// Продажа файла с диска фракции.
    /// Логика награды и доверия аналогична отправке из архива.
    /// </summary>
    public void SellFile(FileData file, FactionType faction)
    {
        if (FactionManager.Instance == null)
        {
            Debug.LogWarning("FactionManager.Instance is null");
            return;
        }

        bool isWanted = FactionManager.Instance.IsFileWanted(faction, file.Index);

        if (isWanted)
        {
            float multiplier = FactionManager.Instance.GetTrustMultiplier(faction);
            int reward = Mathf.RoundToInt(file.BasePrice * multiplier);
            PlayerInventory.Instance.AddMoney(reward);

            float trustDelta = FactionManager.Instance.TrustGainOnSuccessfulFile;
            FactionManager.Instance.ChangeTrust(faction, trustDelta);
        }
        else
        {
            float trustDelta = FactionManager.Instance.TrustLossOnUnsuccessfulFile;
            FactionManager.Instance.ChangeTrust(faction, -trustDelta);
        }

        RemoveFile(file);

        if (DiskWindow.Instance != null)
        {
            DiskWindow.Instance.RefreshDiskFiles();
        }
    }
}
