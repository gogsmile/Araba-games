using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ArchiveWindow : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text ArchiveNameText;
    public Transform FileListParent;
    public GameObject FileEntryPrefab;
    public Button CloseButton;

    // Текущий архив и его иконка на рабочем столе
    private ArchiveData currentArchive;
    private GameObject archiveGO;

    private void Awake()
    {
        if (CloseButton != null)
        {
            CloseButton.onClick.RemoveAllListeners();
            CloseButton.onClick.AddListener(CloseArchive);
        }

        gameObject.SetActive(false);
    }

    /// <summary>
    /// Открыть архив и запомнить иконку, которую надо удалить, когда архив опустеет.
    /// </summary>
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

    /// <summary>
    /// Построение списка файлов.
    /// </summary>
    public void RefreshFileList()
    {
        if (FileListParent == null)
            return;

        // Удаляем старые элементы
        for (int i = FileListParent.childCount - 1; i >= 0; i--)
        {
            Destroy(FileListParent.GetChild(i).gameObject);
        }

        if (currentArchive == null || currentArchive.Files == null)
            return;

        // Копия списка на случай изменений во время цикла
        List<FileData> copy = new List<FileData>(currentArchive.Files);

        foreach (var file in copy)
        {
            if (file == null) continue;

            GameObject entryGO = Instantiate(FileEntryPrefab, FileListParent);
            var entry = entryGO.GetComponent<FileEntry>();
            if (entry != null)
            {
                entry.Setup(file, currentArchive, this);
            }
        }
    }

    /// <summary>
    /// Вызывается из FileEntry, когда файл отправлен фракции/на диск/удалён.
    /// </summary>
    public void RemoveFileFromArchive(FileData file)
    {
        if (currentArchive == null || file == null)
            return;

        if (currentArchive.Files == null)
            return;

        if (!currentArchive.Files.Contains(file))
            return;

        currentArchive.Files.Remove(file);

        // Обновляем список на следующем кадре (чтобы избежать конфликтов)
        StartCoroutine(RefreshNextFrame());

        // Если архив опустел — закрываем окно и уничтожаем иконку
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
        // ждём один кадр, чтобы закончились все UI-операции
        yield return null;

        CloseArchive();

        // Удаляем иконку с рабочего стола
        if (archiveGO != null)
        {
            Destroy(archiveGO);
            archiveGO = null;
        }

        // Удаляем архив из инвентаря игрока, чтобы он не висел "мертвым"
        if (PlayerInventory.Instance != null &&
            PlayerInventory.Instance.Archives != null &&
            currentArchive != null &&
            PlayerInventory.Instance.Archives.Contains(currentArchive))
        {
            PlayerInventory.Instance.Archives.Remove(currentArchive);
        }

        currentArchive = null;
    }

    public void CloseArchive()
    {
        gameObject.SetActive(false);
    }
}
