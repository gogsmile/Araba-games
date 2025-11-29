using System.Collections.Generic;
using UnityEngine;

public class DiskStorage : MonoBehaviour
{
    public static DiskStorage Instance;

    public List<FileData> StoredFiles = new List<FileData>();
    public int Capacity = 100; // F1, максимальное количество файлов

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
    /// Добавление файла на диск
    /// </summary>
    public void AddFile(FileData file)
    {
        if (StoredFiles.Count >= Capacity)
        {
            Debug.Log("Disk is full!");
            return;
        }
        StoredFiles.Add(file);
    }

    /// <summary>
    /// Удаление файла с диска
    /// </summary>
    public void RemoveFile(FileData file)
    {
        StoredFiles.Remove(file);
    }
}
