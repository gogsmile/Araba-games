using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SellerBehaviour : MonoBehaviour
{
    public TMP_Text SellerNameText;
    public TMP_Text ArchiveNameText;
    public TMP_Text PriceText;
    public Button BuyButton;

    private string sellerName;
    private ArchiveData archive;
    private int archivePrice;

    private void Start()
    {
        if (BuyButton != null)
        {
            BuyButton.onClick.RemoveAllListeners();
            BuyButton.onClick.AddListener(TryBuyArchive);
        }

        UpdateUI();
    }

    public void Setup(string sellerName, ArchiveData archive, int price)
    {
        this.sellerName = sellerName;
        this.archive = archive;
        this.archivePrice = price;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (SellerNameText != null)
            SellerNameText.text = sellerName;

        if (ArchiveNameText != null)
            ArchiveNameText.text = archive != null ? archive.ArchiveName : "Archive";

        if (PriceText != null)
            PriceText.text = "$" + archivePrice;
    }

    private void TryBuyArchive()
    {
        if (PlayerInventory.Instance.Money >= archivePrice)
        {
            PlayerInventory.Instance.SpendMoney(archivePrice);
            PlayerInventory.Instance.AddArchive(archive);

            // Создаём иконку архива на рабочем столе
            if (DesktopManager.Instance != null &&
                DesktopManager.Instance.ArchiveIconPrefab != null &&
                DesktopManager.Instance.DesktopPanel != null)
            {
                GameObject iconGO = Instantiate(
                    DesktopManager.Instance.ArchiveIconPrefab,
                    DesktopManager.Instance.DesktopPanel
                );

                iconGO.transform.SetParent(DesktopManager.Instance.DesktopPanel, false);
                iconGO.transform.localScale = Vector3.one;

                var archiveIcon = iconGO.GetComponent<ArchiveIcon>();
                if (archiveIcon != null)
                    archiveIcon.Setup(archive, DesktopManager.Instance.ArchiveWindow);
            }

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Not enough money to buy this archive!");
        }
    }
}
