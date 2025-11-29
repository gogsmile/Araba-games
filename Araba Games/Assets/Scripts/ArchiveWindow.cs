using UnityEngine;
using TMPro;

public class ArchiveWindow : MonoBehaviour
{
    public TMP_Text ArchiveNameText;
    public Transform FileListParent;
    public GameObject FileEntryPrefab;

    private ArchiveData currentArchive;

    public void OpenArchive(ArchiveData archive)
    {
        currentArchive = archive;
        gameObject.SetActive(true);

        if (ArchiveNameText != null)
            ArchiveNameText.text = archive.ArchiveName;

        // Очистка старых элементов
        foreach (Transform t in FileListParent)
            Destroy(t.gameObject);

        // Создание элементов файлов
        foreach (var file in archive.Files)
        {
            GameObject entryGO = Instantiate(FileEntryPrefab, FileListParent);
            var entry = entryGO.GetComponent<FileEntry>();
            if (entry != null)
                entry.Setup(file, currentArchive);
        }
    }

    public void CloseArchive()
    {
        gameObject.SetActive(false);
    }
}
