using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ArchiveWindow : MonoBehaviour
{
    public TMP_Text ArchiveNameText;
    public Transform FileListParent;
    public GameObject FileEntryPrefab;
    public Button CloseButton;    // <-- КНОПКА ЗАКРЫТЬ

    private ArchiveData currentArchive;
    private GameObject archiveGO; // ссылка на иконку архива

    private void Awake()
    {
        if (CloseButton != null)
        {
            CloseButton.onClick.RemoveAllListeners();
            CloseButton.onClick.AddListener(CloseArchive);
        }
    }

    public void OpenArchive(ArchiveData archive, GameObject icon)
    {
        if (archive == null)
        {
            Debug.LogWarning("ArchiveWindow.OpenArchive called with null archive");
            return;
        }

        currentArchive = archive;
        archiveGO = icon;

        gameObject.SetActive(true);

        if (ArchiveNameText != null)
            ArchiveNameText.text = currentArchive.ArchiveName;

        RefreshFileList();
    }

    // Построение списка файлов
    public void RefreshFileList()
    {
        // Удаляем старые элементы безопасно
        for (int i = FileListParent.childCount - 1; i >= 0; i--)
        {
            Destroy(FileListParent.GetChild(i).gameObject);
        }

        if (currentArchive == null) return;

        // Используем копию списка, чтобы не ловить ошибку изменения коллекции
        List<FileData> copy = new List<FileData>(currentArchive.Files);

        foreach (var file in copy)
        {
            GameObject entryGO = Instantiate(FileEntryPrefab, FileListParent);
            var entry = entryGO.GetComponent<FileEntry>();
            if (entry != null)
                entry.Setup(file, currentArchive, this);
        }
    }

    // Удаляем файл из архива
    public void RemoveFileFromArchive(FileData file)
    {
        if (currentArchive == null || file == null) return;

        if (!currentArchive.Files.Contains(file))
            return;

        currentArchive.Files.Remove(file);

        // Обновление UI на следующем кадре (устраняет reentrancy)
        StartCoroutine(RefreshNextFrame());

        // Если архив пуст — закрываем окно и удаляем иконку
        if (currentArchive.IsEmpty())
        {
            StartCoroutine(CloseAndDestroyIconNextFrame());
        }
    }

    private IEnumerator RefreshNextFrame()
    {
        yield return null;
        RefreshFileList();
    }

    private IEnumerator CloseAndDestroyIconNextFrame()
    {
        yield return null;

        CloseArchive();

        if (archiveGO != null)
        {
            Destroy(archiveGO);
            archiveGO = null;
        }

        currentArchive = null;
    }

    public void CloseArchive()
    {
        gameObject.SetActive(false);
    }
}
