using UnityEngine;
using UnityEngine.UI;

public class SellerTabController : MonoBehaviour
{
    public GameObject sellerPanel; // Панель со скроллом

    public Button openButton;
    public Button closeButton;

    private void Start()
    {
        // Убедимся, что панель закрыта в начале
        sellerPanel.SetActive(false);

        // Кнопки
        openButton.onClick.AddListener(OpenPanel);
        closeButton.onClick.AddListener(ClosePanel);
    }

    public void OpenPanel()
    {
        sellerPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        sellerPanel.SetActive(false);
    }
}
