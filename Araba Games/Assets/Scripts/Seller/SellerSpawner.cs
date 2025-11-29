using System.Collections.Generic;
using UnityEngine;

public class SellerSpawner : MonoBehaviour
{
    [Header("Spawn settings")]
    public GameObject SellerPrefab;       // Префаб продавца
    public Transform SpawnParent;         // Panel/контейнер для UI
    public int StartSellersCount = 3;     // Стартовое количество продавцов
    public float SpawnInterval = 10f;     // Интервал появления новых продавцов

    [Header("Manual seller list")]
    public List<SellerData> SellerList = new List<SellerData>();

    [Header("Archive Names List")]
    public ArchiveNameListSO ArchiveNameList; // Список названий архивов

    private float timer = 0f;

    private void Start()
    {
        // Спавн стартовых продавцов
        for (int i = 0; i < Mathf.Min(StartSellersCount, SellerList.Count); i++)
        {
            SpawnSeller(SellerList[i]);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= SpawnInterval)
        {
            timer = 0f;
            if (SellerList.Count > 0)
            {
                int idx = Random.Range(0, SellerList.Count);
                SpawnSeller(SellerList[idx]);
            }
        }
    }

    private void SpawnSeller(SellerData data)
    {
        if (SellerPrefab == null || SpawnParent == null) return;

        GameObject go = Instantiate(SellerPrefab, SpawnParent);
        var sb = go.GetComponent<SellerBehaviour>();

        // Генерация архива
        int fileCount = Random.Range(data.MinFiles, data.MaxFiles + 1);
        ArchiveData archive = new ArchiveData();

        // Выбираем случайное название из ArchiveNameList
        if (ArchiveNameList != null && ArchiveNameList.Names.Count > 0)
        {
            int randIndex = Random.Range(0, ArchiveNameList.Names.Count);
            archive.ArchiveName = ArchiveNameList.Names[randIndex];
        }
        else
        {
            archive.ArchiveName = $"{data.SellerName}'s Archive";
        }

        // Генерация файлов архива
        for (int i = 0; i < fileCount; i++)
        {
            if (data.PossibleFiles.Count == 0) continue;

            int fIdx = Random.Range(0, data.PossibleFiles.Count);
            var fileSO = data.PossibleFiles[fIdx];

            var file = new FileData
            {
                Index = fileSO.Index,
                FileName = fileSO.FileName,
                Description = fileSO.Description,
                BasePrice = fileSO.BasePrice,
                DropChance = fileSO.DropChance,
                MemoryPoints = fileSO.MemoryPoints
            };

            archive.Files.Add(file);
        }

        // Цена архива: (Rating / 100) * Кол-во файлов
        int price = Mathf.RoundToInt((data.Rating / 100f) * archive.Files.Count);

        // Передаём имя продавца, архив и цену в SellerBehaviour
        sb.Setup(data.SellerName, archive, price);
    }
}
