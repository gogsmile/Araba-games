using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArchiveIcon : MonoBehaviour
{
    /// <summary>
    /// Данные архива, которые представляет эта иконка.
    /// </summary>
    public ArchiveData ArchiveData;

    /// <summary>
    /// Текст на иконке (например, название архива).
    /// Прикрепи сюда Text (TMP) из дочернего объекта иконки.
    /// </summary>
    public TMP_Text ArchiveNameText;

    /// <summary>
    /// Окно архива. Может быть передано через Setup из SellerBehaviour.
    /// </summary>
    private ArchiveWindow archiveWindow;

    private void Start()
    {
        // Навешиваем клик по кнопке
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(OnClick);
        }

        // Если окно не передали через Setup — пробуем взять из DesktopManager
        if (archiveWindow == null && DesktopManager.Instance != null)
        {
            archiveWindow = DesktopManager.Instance.ArchiveWindow;
        }

        // Если ArchiveData уже задан (например, через инспектор),
        // обновим текст на иконке.
        UpdateText();
    }

    /// <summary>
    /// Вызывается из SellerBehaviour после покупки архива.
    /// </summary>
    public void Setup(ArchiveData data, ArchiveWindow window)
    {
        ArchiveData = data;
        archiveWindow = window;

        UpdateText();
    }

    /// <summary>
    /// Обновляет текст на иконке согласно ArchiveData.
    /// </summary>
    private void UpdateText()
    {
        if (ArchiveNameText != null && ArchiveData != null)
        {
            // предполагаем, что в ArchiveData есть поле ArchiveName
            ArchiveNameText.text = ArchiveData.ArchiveName;
        }
    }

    private void OnClick()
    {
        if (ArchiveData == null)
        {
            Debug.LogWarning("ArchiveIcon: ArchiveData is null");
            return;
        }

        if (archiveWindow == null && DesktopManager.Instance != null)
        {
            archiveWindow = DesktopManager.Instance.ArchiveWindow;
        }

        if (archiveWindow == null)
        {
            Debug.LogWarning("ArchiveIcon: ArchiveWindow is missing");
            return;
        }

        // Передаём ссылку на САМУ иконку, чтобы потом её удалить,
        // когда архив станет пустым.
        archiveWindow.OpenArchive(ArchiveData, gameObject);
    }
}
