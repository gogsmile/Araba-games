using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArchiveIcon : MonoBehaviour
{
    public TMP_Text ArchiveNameText;
    private ArchiveData archiveData;
    private ArchiveWindow archiveWindow;

    public void Setup(ArchiveData data, ArchiveWindow window)
    {
        archiveData = data;
        archiveWindow = window;

        if (ArchiveNameText != null)
            ArchiveNameText.text = data.ArchiveName;

        var button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OpenArchive);
        }
    }

    private void OpenArchive()
    {
        if (archiveWindow != null && archiveData != null)
            archiveWindow.OpenArchive(archiveData, gameObject);
    }
}
