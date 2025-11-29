using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SellerBehaviour : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text NameText;      // Имя продавца
    public TMP_Text PriceText;     // Цена архива
    public Button BuyButton;       // Кнопка покупки

    [Header("Data")]
    public SellerData Data;        // Данные продавца

    private void Start()
    {
        if (BuyButton != null)
        {
            // Назначаем метод покупки на кнопку
            BuyButton.onClick.AddListener(TryBuyArchive);
        }

        UpdateUI();
    }

    /// <summary>
    /// Устанавливаем данные продавца и обновляем UI
    /// </summary>
    public void Setup(SellerData data)
    {
        Data = data;
        UpdateUI();
    }

    /// <summary>
    /// Обновление UI элементов
    /// </summary>
    private void UpdateUI()
    {
        if (Data == null) return;

        if (NameText != null)
            NameText.text = Data.SellerName;

        if (PriceText != null)
            PriceText.text = "$" + Data.ArchivePrice;
    }

    /// <summary>
    /// Попытка купить архив
    /// Проверяет зажатие ПКМ + ЛКМ
    /// </summary>
    private void TryBuyArchive()
    {
        if (!Input.GetMouseButton(1) || !Input.GetMouseButton(0))
            return; // ПКМ + ЛКМ должны быть зажаты

        if (PlayerInventory.Instance.Money >= Data.ArchivePrice)
        {
            // Списываем деньги
            PlayerInventory.Instance.SpendMoney(Data.ArchivePrice);

            // Добавляем архив в инвентарь
            PlayerInventory.Instance.AddArchive(Data.Archive);

            Debug.Log("Archive purchased: " + Data.Archive.ArchiveName);

            // Создаём иконку на рабочем столе
            if (DesktopManager.Instance != null && DesktopManager.Instance.ArchiveIconPrefab != null)
            {
                GameObject iconGO = Instantiate(
                    DesktopManager.Instance.ArchiveIconPrefab,
                    DesktopManager.Instance.DesktopPanel
                );
                iconGO.GetComponent<ArchiveIcon>().Setup(Data.Archive, DesktopManager.Instance.ArchiveWindow);
            }

            // Можно удалить продавца после покупки, если нужно
            // Destroy(gameObject);
        }
        else
        {
            Debug.Log("Not enough money to buy this archive!");
        }
    }
}
