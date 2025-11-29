using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FileData
{
    public int Index;                  // индекс файла
    public string FileName;            // имя файла
    public string Description;         // описание
    public int BasePrice;              // базовая цена
    public int MemoryPoints; // добавляем поле
    public float DropChance;           // шанс выпадения (0–100)
}

[System.Serializable]
public class ArchiveData
{
    public string ArchiveName;                 // имя архива
    public List<FileData> Files = new List<FileData>();

    // Получить случайный файл по шансам выпадения
    public FileData GetRandomFile()
    {
        if (Files.Count == 0) return null;
        float roll = Random.Range(0f, 100f);
        float cumulative = 0f;

        foreach (var file in Files)
        {
            cumulative += file.DropChance;
            if (roll <= cumulative)
                return file;
        }

        return Files[Files.Count - 1];
    }

    // Проверка, пуст ли архив
    public bool IsEmpty()
    {
        return Files == null || Files.Count == 0;
    }
}
