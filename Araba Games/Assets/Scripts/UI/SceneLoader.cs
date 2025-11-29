using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField, Min(0)]
    private int sceneIndex = 1; // тут в инспекторе выбираешь индекс сцены

    // вызывать из кнопки (OnClick)
    public void LoadScene()
    {
        int total = SceneManager.sceneCountInBuildSettings;
        if (sceneIndex < 0 || sceneIndex >= total)
        {
            Debug.LogError($"SceneLoader: индекс {sceneIndex} вне диапазона 0..{total - 1}. " +
                           "Добавь сцену в File → Build Settings → Scenes In Build.");
            return;
        }

        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }
}
