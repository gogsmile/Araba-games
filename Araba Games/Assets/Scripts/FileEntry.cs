using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FileEntry : MonoBehaviour
{
    public TMP_Text FileNameText;
    public TMP_Text DescriptionText;
    public TMP_Text BasePriceText;
    public TMP_Dropdown DestinationDropdown;
    public Button DeleteButton;

    private FileData fileData;
    private ArchiveData parentArchive;
    private ArchiveWindow archiveWindow;

    // Флаг подавления обработки dropdown при программных изменениях
    private bool suppressDropdown = false;

    public void Setup(FileData data, ArchiveData archive, ArchiveWindow window)
    {
        fileData = data;
        parentArchive = archive;
        archiveWindow = window;

        if (FileNameText != null) FileNameText.text = data.FileName;
        if (DescriptionText != null) DescriptionText.text = data.Description;
        if (BasePriceText != null) BasePriceText.text = "$" + data.BasePrice;

        if (DestinationDropdown != null)
        {
            DestinationDropdown.onValueChanged.RemoveAllListeners();
            DestinationDropdown.onValueChanged.AddListener(OnDestinationSelected);

            // Устанавливаем "Диск" по умолчанию без вызова callback'а
            suppressDropdown = true;
            DestinationDropdown.SetValueWithoutNotify(3); // 0:Corp,1:Rebels,2:Hackers,3:Disk
            suppressDropdown = false;
        }

        if (DeleteButton != null)
        {
            DeleteButton.onClick.RemoveAllListeners();
            DeleteButton.onClick.AddListener(DeleteFile);
        }
    }

    private void OnDestinationSelected(int index)
    {
        if (suppressDropdown) return;

        switch (index)
        {
            case 0: SendToFaction(FactionType.Corporates); break;
            case 1: SendToFaction(FactionType.Rebels); break;
            case 2: SendToFaction(FactionType.Hackers); break;
            case 3: SendToDisk(); break;
            default: break;
        }

        // Сброс выборa в UI обратно на "Диск" без триггера
        suppressDropdown = true;
        DestinationDropdown.SetValueWithoutNotify(3);
        suppressDropdown = false;
    }

    private void SendToFaction(FactionType faction)
    {
        if (fileData == null) return;

        bool isWanted = FactionManager.Instance.IsFileWanted(faction, fileData.Index);

        if (isWanted)
        {
            float multiplier = FactionManager.Instance.GetTrustMultiplier(faction);
            int reward = Mathf.RoundToInt(fileData.BasePrice * multiplier);
            PlayerInventory.Instance.AddMoney(reward);
            FactionManager.Instance.ChangeTrust(faction, +1);
        }
        else
        {
            FactionManager.Instance.ChangeTrust(faction, -1);
        }

        RemoveFromArchive();
    }

    private void SendToDisk()
    {
        if (fileData == null) return;

        DiskStorage.Instance.AddFile(fileData);

        if (fileData.IsMemoryFragment)
        {
            // Если у тебя разные значения очков памяти в SO — можно использовать их вместо 10
            PlayerInventory.Instance.AddMemory(10);
        }

        RemoveFromArchive();
    }

    private void DeleteFile()
    {
        RemoveFromArchive();
    }

    private void RemoveFromArchive()
    {
        // Убираем из данных архива и просим окно обновиться (оно делает это отложенно)
        if (archiveWindow != null)
            archiveWindow.RemoveFileFromArchive(fileData);

        // Удаляем визуальный элемент
        Destroy(gameObject);
    }
}
