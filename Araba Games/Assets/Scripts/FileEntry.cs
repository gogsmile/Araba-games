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

        if (FileNameText != null)
            FileNameText.text = fileData.FileName;

        if (DescriptionText != null)
            DescriptionText.text = fileData.Description;

        if (BasePriceText != null)
            BasePriceText.text = "$" + fileData.BasePrice;

        if (DestinationDropdown != null)
        {
            DestinationDropdown.onValueChanged.RemoveAllListeners();
            DestinationDropdown.onValueChanged.AddListener(OnDestinationSelected);
            DestinationDropdown.value = 3; // по умолчанию Диск
        }
    }

    private void OnDestinationSelected(int index)
    {
        switch (index)
        {
            case 0: SendToFaction(FactionType.Corporates); break;
            case 1: SendToFaction(FactionType.Rebels); break;
            case 2: SendToFaction(FactionType.Hackers); break;
            case 3: SendToDisk(); break;
        }

        DestinationDropdown.value = 3;
    }

    private void SendToFaction(FactionType faction)
    {
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

        RemoveFileFromArchive();
    }

    private void SendToDisk()
    {
        DiskStorage.Instance.AddFile(fileData);

        if (fileData.IsMemoryFragment)
        {
            PlayerInventory.Instance.AddMemory(10); // L очков памяти
        }

        RemoveFileFromArchive();
    }

    private void RemoveFileFromArchive()
    {
        parentArchive.Files.Remove(fileData);

        if (parentArchive.IsEmpty())
        {
            var window = GetComponentInParent<ArchiveWindow>();
            if (window != null)
                window.CloseArchive();
        }

        Destroy(gameObject);
    }
}
