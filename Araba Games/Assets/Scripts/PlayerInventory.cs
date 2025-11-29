using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    public int Money = 1000;
    public int MemoryPoints = 0;

    // Список купленных архивов
    public List<ArchiveData> Archives = new List<ArchiveData>();

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

    // Деньги
    public void AddMoney(int amount)
    {
        Money += amount;
        UIManager.Instance.UpdateMoney(Money);
    }

    public void SpendMoney(int amount)
    {
        Money -= amount;
        UIManager.Instance.UpdateMoney(Money);
    }

    // Очки памяти
    public void AddMemory(int points)
    {
        MemoryPoints += points;
        UIManager.Instance.UpdateMemory(MemoryPoints);
    }

    // Добавление купленного архива
    public void AddArchive(ArchiveData archive)
    {
        if (archive != null)
        {
            Archives.Add(archive);
            Debug.Log($"Archive added: {archive.ArchiveName}");
        }
    }
}
