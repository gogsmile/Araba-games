using UnityEngine;
using UnityEngine.UI;

public class ArchiveIcon : MonoBehaviour
{
    public TMPro.TMP_Text ArchiveNameText;
    private ArchiveData archiveData;
    private ArchiveWindow archiveWindow;

    public void Setup(ArchiveData data, ArchiveWindow window)
    {
        archiveData = data;
        archiveWindow = window;
        ArchiveNameText.text = data.ArchiveName;

        // Назначаем кнопку на открытие окна архива
        GetComponent<Button>().onClick.AddListener(OpenArchive);
    }

    private void OpenArchive()
    {
        if (archiveWindow != null && archiveData != null)
        {
            archiveWindow.OpenArchive(archiveData);
        }
    }
}
