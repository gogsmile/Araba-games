using UnityEngine;
using TMPro;

public class FactionUIManager : MonoBehaviour
{
    public static FactionUIManager Instance;

    public TMP_Text CorporateTrustText;
    public TMP_Text RebelsTrustText;
    public TMP_Text HackersTrustText;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        UpdateFactionUI();
    }

    public void UpdateFactionUI()
    {
        if (FactionManager.Instance == null)
            return;

        CorporateTrustText.text =
            FactionManager.Instance.GetTrustPercent(FactionType.Corporates).ToString("0") + "%";

        RebelsTrustText.text =
            FactionManager.Instance.GetTrustPercent(FactionType.Rebels).ToString("0") + "%";

        HackersTrustText.text =
            FactionManager.Instance.GetTrustPercent(FactionType.Hackers).ToString("0") + "%";
    }
}
