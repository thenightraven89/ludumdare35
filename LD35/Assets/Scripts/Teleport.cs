using UnityEngine;

public class Teleport: MonoBehaviour
{
    [SerializeField]
    private string _sceneName;

    private const float LEVEL_LOAD_DELAY = 3f;

    public delegate void CheckpointEvent(string nextLevel);
    public event CheckpointEvent OnCheckpoint = delegate { };

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
        OnCheckpoint(_sceneName);
    }
}