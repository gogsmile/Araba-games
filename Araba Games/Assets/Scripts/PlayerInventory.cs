using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    public int Money;
    public List<ArchiveData> Archives = new List<ArchiveData>();
    public List<FileData> Files = new List<FileData>();
    public int MemoryPoints;

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

    public void AddMoney(int amount)
    {
        Money += amount;
    }

    public void SpendMoney(int amount)
    {
        Money -= amount;
        if (Money < 0) Money = 0;
    }

    public void AddArchive(ArchiveData archive)
    {
        Archives.Add(archive);
    }

    public void RemoveArchive(ArchiveData archive)
    {
        Archives.Remove(archive);
    }
}
