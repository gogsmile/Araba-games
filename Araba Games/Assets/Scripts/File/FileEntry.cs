using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FileEntry : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text FileNameText;
    public TMP_Text DescriptionText;
    public TMP_Text BasePriceText;
    public TMP_Dropdown DestinationDropdown; // 0 = Корпораты, 1 = Повстанцы, 2 = Хакеры, 3 = Диск
    public Button ConfirmButton;
    public Button DeleteButton;

    private FileData fileData;
    private ArchiveData parentArchive;
    private ArchiveWindow archiveWindow;

    private int selectedIndex = 3; // По умолчанию "Диск"

    public void Setup(FileData data, ArchiveData archive, ArchiveWindow window)
    {
        fileData = data;
        parentArchive = archive;
        archiveWindow = window;

        if (FileNameText != null)
            FileNameText.text = fileData.FileName;

        if (DescriptionText != null)
            DescriptionText.text = fileData.Description;

        if (BasePriceText != null)
            BasePriceText.text = fileData.BasePrice.ToString();

        if (DestinationDropdown != null)
        {
            DestinationDropdown.onValueChanged.RemoveAllListeners();
            DestinationDropdown.onValueChanged.AddListener(OnDropdownChanged);
            DestinationDropdown.SetValueWithoutNotify(selectedIndex);
        }

        if (ConfirmButton != null)
        {
            ConfirmButton.onClick.RemoveAllListeners();
            ConfirmButton.onClick.AddListener(ConfirmAction);
        }

        if (DeleteButton != null)
        {
            DeleteButton.onClick.RemoveAllListeners();
            DeleteButton.onClick.AddListener(DeleteFile);
        }
    }

    private void OnDropdownChanged(int index)
    {
        selectedIndex = index;
    }

    private void ConfirmAction()
    {
        switch (selectedIndex)
        {
            case 0: SendToFaction(FactionType.Corporates); break;
            case 1: SendToFaction(FactionType.Rebels); break;
            case 2: SendToFaction(FactionType.Hackers); break;
            case 3: SendToDisk(); break;
        }
    }

    private void SendToFaction(FactionType faction)
    {
        if (FactionManager.Instance == null)
        {
            Debug.LogWarning("SendToFaction: FactionManager.Instance is null");
            return;
        }

        bool isWanted = FactionManager.Instance.IsFileWanted(faction, fileData.Index);

        if (isWanted)
        {
            float multiplier = FactionManager.Instance.GetTrustMultiplier(faction);
            int reward = Mathf.RoundToInt(fileData.BasePrice * multiplier);
            PlayerInventory.Instance.AddMoney(reward);

            float trustDelta = FactionManager.Instance.TrustGainOnSuccessfulFile;
            FactionManager.Instance.ChangeTrust(faction, trustDelta);
        }
        else
        {
            float trustDelta = FactionManager.Instance.TrustLossOnUnsuccessfulFile;
            FactionManager.Instance.ChangeTrust(faction, -trustDelta);
        }

        RemoveFromArchive();
    }

    private void SendToDisk()
    {
        if (DiskStorage.Instance == null)
        {
            Debug.LogWarning("SendToDisk: DiskStorage.Instance is null");
            return;
        }

        bool added = DiskStorage.Instance.AddFile(fileData);
        if (!added)
        {
            Debug.Log("Disk is full, file not added.");
            return;
        }

        if (fileData.MemoryPoints > 0)
        {
            PlayerInventory.Instance.AddMemory(fileData.MemoryPoints);
        }

        if (DiskWindow.Instance != null)
        {
            DiskWindow.Instance.RefreshDiskFiles();
        }

        RemoveFromArchive();
    }

    private void DeleteFile()
    {
        RemoveFromArchive();
    }

    private void RemoveFromArchive()
    {
        // ВАЖНО: всю логику удаления из архива делаем через окно,
        // чтобы оно смогло заметить, что архив стал пустым.
        if (archiveWindow != null)
        {
            archiveWindow.RemoveFileFromArchive(fileData);
        }

        Destroy(gameObject);
    }
}
