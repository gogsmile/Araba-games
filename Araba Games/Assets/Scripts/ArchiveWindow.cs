using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArchiveWindow : MonoBehaviour
{
    public TMP_Text ArchiveNameText;
    public Transform FileListParent;
    public GameObject FileEntryPrefab;

    private ArchiveData currentArchive;

    /// <summary>
    /// Открыть архив
    /// </summary>
    public void OpenArchive(ArchiveData archive)
    {
        currentArchive = archive;
        ArchiveNameText.text = archive.ArchiveName;

        // Очистка списка
        foreach (Transform t in FileListParent)
            Destroy(t.gameObject);

        // Создание элементов UI для каждого файла
        foreach (var file in archive.Files)
        {
            GameObject entry = Instantiate(FileEntryPrefab, FileListParent);
            var fileEntry = entry.GetComponent<FileEntry>();
            fileEntry.Setup(file, currentArchive);
        }

        gameObject.SetActive(true);
    }

    public void CloseArchive()
    {
        gameObject.SetActive(false);
    }
}
