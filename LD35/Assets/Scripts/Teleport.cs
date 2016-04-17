using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport: MonoBehaviour
{
    [SerializeField]
    private string _sceneName;

    private const float LEVEL_LOAD_DELAY = 3f;

    public delegate void CurtainEvent(bool state, float countdown);
    public event CurtainEvent OnCurtain = delegate { };

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
        OnCurtain(true, LEVEL_LOAD_DELAY - 1f);
        StartCoroutine(TeleportCoroutine(_sceneName, LEVEL_LOAD_DELAY));
    }

    private IEnumerator TeleportCoroutine(string sceneName, float duration)
    {
        yield return new WaitForSeconds(duration);

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}