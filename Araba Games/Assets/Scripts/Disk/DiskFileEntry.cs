using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DiskFileEntry : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text FileNameText;
    public TMP_Text DescriptionText;
    public TMP_Text BasePriceText;
    public TMP_Dropdown DestinationDropdown; // Корпораты, Повстанцы, Хакеры, Удалить
    public Button ConfirmButton;
    public Button DeleteButton;

    private FileData fileData;
    private DiskWindow diskWindow;

    private int selectedIndex = 3; // по умолчанию "Удалить"

    public void Setup(FileData data, DiskWindow window)
    {
        fileData = data;
        diskWindow = window;

        if (FileNameText != null) FileNameText.text = fileData.FileName;
        if (DescriptionText != null) DescriptionText.text = fileData.Description;
        if (BasePriceText != null) BasePriceText.text = "$" + fileData.BasePrice;

        // Dropdown
        if (DestinationDropdown != null)
        {
            DestinationDropdown.onValueChanged.RemoveAllListeners();
            DestinationDropdown.onValueChanged.AddListener(OnDestinationSelected);
            DestinationDropdown.SetValueWithoutNotify(3); // по умолчанию "Удалить"
        }

        // ConfirmButton
        if (ConfirmButton != null)
        {
            ConfirmButton.onClick.RemoveAllListeners();
            ConfirmButton.onClick.AddListener(ConfirmAction);
        }

        // DeleteButton
        if (DeleteButton != null)
        {
            DeleteButton.onClick.RemoveAllListeners();
            DeleteButton.onClick.AddListener(DeleteFile);
        }
    }

    private void OnDestinationSelected(int index)
    {
        selectedIndex = index; // просто сохраняем выбор
    }

    private void ConfirmAction()
    {
        switch (selectedIndex)
        {
            case 0: SellToFaction(FactionType.Corporates); break;
            case 1: SellToFaction(FactionType.Rebels); break;
            case 2: SellToFaction(FactionType.Hackers); break;
            case 3: DeleteFile(); break;
        }

        // Сбрасываем Dropdown на "Удалить"
        DestinationDropdown.SetValueWithoutNotify(3);
        selectedIndex = 3;
    }

    private void SellToFaction(FactionType faction)
    {
        DiskStorage.Instance.SellFile(fileData, faction);
        RemoveFromDisk();
    }

    private void DeleteFile()
    {
        RemoveFromDisk();
    }

    private void RemoveFromDisk()
    {
        if (diskWindow != null)
            diskWindow.RemoveFileFromDisk(fileData);

        Destroy(gameObject);
    }
}
