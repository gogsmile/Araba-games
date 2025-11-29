using UnityEngine;

public class DesktopManager : MonoBehaviour
{
    public static DesktopManager Instance;

    public Transform DesktopPanel;          // Panel для иконок архивов
    public GameObject ArchiveIconPrefab;    // Префаб иконки
    public ArchiveWindow ArchiveWindow;     // Ссылка на окно архива

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (transform.parent != null)
            transform.SetParent(null);

        DontDestroyOnLoad(gameObject);
    }
}
