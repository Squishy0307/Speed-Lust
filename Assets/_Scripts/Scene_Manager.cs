using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    public static Scene_Manager Instance;
    private bool isLoadingScene = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void LoadScene(int buildIndex)
    {      
        if (!isLoadingScene)
        {
            StartCoroutine(fadeToScene(buildIndex));
        }
    }

    IEnumerator fadeToScene(int buildIndex)
    {
        Fader.Instance.FadeOut();
        MusicController.instance.ChangeVolume(0, 0, 0.5f);
        isLoadingScene = true;

        yield return new WaitForSeconds(0.5f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(buildIndex);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }

        isLoadingScene = false;
        Fader.Instance.FadeIn();
    }
}
