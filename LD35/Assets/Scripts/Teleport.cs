using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport: MonoBehaviour
{
    [SerializeField]
    private string _sceneName;

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
        StartCoroutine(TeleportCoroutine(_sceneName));
    }

    private IEnumerator TeleportCoroutine(string sceneName)
    {
        var initialColor = RenderSettings.ambientLight;
        float t = 0;
        float duration = 3f;
        while (t < duration)
        {
            t += Time.deltaTime;
            RenderSettings.ambientLight = Color.Lerp(initialColor, Color.black, t / duration);
            yield return null;
        }

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}