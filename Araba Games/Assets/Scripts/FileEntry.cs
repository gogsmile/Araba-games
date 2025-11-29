using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FileEntry : MonoBehaviour
{
    public TMP_Text FileNameText;
    public TMP_Text DescriptionText;
    public TMP_Text BasePriceText;
    public TMP_Dropdown DestinationDropdown;

    private FileData fileData;
    private ArchiveData parentArchive;

    public void Setup(FileData data, ArchiveData archive)
    {
        fileData = data;
        parentArchive = archive;

        FileNameText.text = data.FileName;
        DescriptionText.text = data.Description;
        BasePriceText.text = "$" + data.BasePrice;

        DestinationDropdown.onValueChanged.AddListener(OnDestinationSelected);
    }

    private void OnDestinationSelected(int index)
    {
        switch (index)
        {
            case 0: // Корпораты
                SendToFaction(FactionType.Corporates);
                break;
            case 1: // Повстанцы
                SendToFaction(FactionType.Rebels);
                break;
            case 2: // Хакеры
                SendToFaction(FactionType.Hackers);
                break;
            case 3: // Диск
                SendToDisk();
                break;
        }

        // После выбора сбрасываем dropdown
        DestinationDropdown.value = 3; // по умолчанию Диск или пустой пункт
    }

    private void SendToFaction(FactionType faction)
    {
        bool isWanted = FactionManager.Instance.IsFileWanted(faction, fileData.Index);

        if (isWanted)
        {
            int profit = Mathf.RoundToInt(fileData.BasePrice * FactionManager.Instance.GetTrust(faction) / 100f);
            PlayerInventory.Instance.AddMoney(profit);
            FactionManager.Instance.ChangeTrust(faction, 1); // +D1 доверия
        }
        else
        {
            FactionManager.Instance.ChangeTrust(faction, -1); // -D2 доверия
        }

        RemoveFileFromArchive();
    }

    private void SendToDisk()
    {
        DiskStorage.Instance.AddFile(fileData);
        if (fileData.IsMemoryFragment)
            PlayerInventory.Instance.MemoryPoints += 10; // L очков памяти

        RemoveFileFromArchive();
    }

    private void RemoveFileFromArchive()
    {
        parentArchive.Files.Remove(fileData);

        if (parentArchive.IsEmpty())
            GetComponentInParent<ArchiveWindow>().CloseArchive();

        Destroy(gameObject);
    }
}
