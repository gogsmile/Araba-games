using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text MoneyText;
    public TMP_Text MemoryText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateMoney(int amount)
    {
        if (MoneyText != null)
            MoneyText.text = $"$ {amount}";
    }

    public void UpdateMemory(int points)
    {
        if (MemoryText != null)
            MemoryText.text = $"Memory: {points}";
    }
}
