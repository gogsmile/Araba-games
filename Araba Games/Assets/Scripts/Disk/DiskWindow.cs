using UnityEngine;

public class DiskWindow : MonoBehaviour
{
    public static DiskWindow Instance;

    public Transform FileListParent;
    public GameObject DiskFileEntryPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenDisk()
    {
        gameObject.SetActive(true);
        RefreshDiskFiles(); // отображаем все файлы, даже если добавлялись ранее
    }

    public void RefreshDiskFiles()
    {
        // Удаляем старые элементы UI
        foreach (Transform t in FileListParent)
            Destroy(t.gameObject);

        // Создаём элементы для всех файлов в хранилище
        foreach (var file in DiskStorage.Instance.StoredFiles)
        {
            GameObject entryGO = Instantiate(DiskFileEntryPrefab, FileListParent);
            entryGO.transform.SetParent(FileListParent, false);
            entryGO.transform.localScale = Vector3.one;

            var entry = entryGO.GetComponent<DiskFileEntry>();
            if (entry != null)
                entry.Setup(file, this);
        }
    }

    public void RemoveFileFromDisk(FileData file)
    {
        DiskStorage.Instance.RemoveFile(file);
        RefreshDiskFiles();
    }
}
