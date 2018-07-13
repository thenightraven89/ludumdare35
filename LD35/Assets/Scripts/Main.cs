using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    private int _initialBlood;

    [SerializeField]
    private string _location;

    [SerializeField]
    private string _vampireThoughts;

    public void Awake()
    {
        var ui = FindObjectOfType<UI>();
        ui.InitInventory();
        ui.SetLocation(_location);
        ui.SetVoiceText(_vampireThoughts);
        ui.UnveilCurtain();

        var vampire = FindObjectOfType<Vampire>();
        vampire.OnBloodChange += ui.SetBlood;
        vampire.OnSpeak += ui.SetVoiceText;
        vampire.OnCollected += ui.AddCollected;
        vampire.OnConsumed += ui.RemoveCollected;
        vampire.Blood = _initialBlood;

        var teleports = FindObjectsOfType<Teleport>();
        foreach (var t in teleports)
        {
            t.OnCheckpoint += ui.LevelComplete;
        }

        var seers = FindObjectsOfType<SightBehaviour>();
        foreach (var s in seers)
        {
            s.SetTarget(vampire.transform);
            s.OnDetected += ui.GameOver;
        }
    }
}