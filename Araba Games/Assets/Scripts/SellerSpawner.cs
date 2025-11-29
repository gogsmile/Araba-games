using System.Collections.Generic;
using UnityEngine;

public class SellerSpawner : MonoBehaviour
{
    public GameObject SellerPrefab;
    public Transform SpawnParent;
    public int SellersCount = 5;

    public int MinFilesPerArchive = 3;
    public int MaxFilesPerArchive = 7;

    public List<SellerData> SellerList = new List<SellerData>();

    void Start()
    {
        SpawnSellers();
    }

    void SpawnSellers()
    {
        for (int i = 0; i < SellersCount; i++)
        {
            GameObject sellerGO = Instantiate(SellerPrefab, SpawnParent);
            SellerData data = GenerateSellerData(i);
            sellerGO.GetComponent<SellerBehaviour>().Setup(data);
            SellerList.Add(data);
        }
    }

    SellerData GenerateSellerData(int id)
    {
        SellerData data = new SellerData();
        data.SellerName = "Seller " + id;
        data.Rating = Random.Range(50, 101);

        // Генерация архива
        ArchiveData archive = new ArchiveData();
        int fileCount = Random.Range(MinFilesPerArchive, MaxFilesPerArchive + 1);
        for (int i = 0; i < fileCount; i++)
        {
            FileData file = new FileData();
            file.Index = i;
            file.FileName = "File_" + i;
            file.Description = "Description of file " + i;
            file.BasePrice = Random.Range(10, 51);
            file.IsMemoryFragment = (Random.value < 0.2f); // 20% шанс быть фрагментом памяти
            archive.Files.Add(file);
        }

        data.Archive = archive;
        data.ArchivePrice = Mathf.RoundToInt((data.Rating / 100f) * fileCount);

        return data;
    }
}
