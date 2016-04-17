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
        ui.SetLocation(_location);
        ui.SetVoiceText(_vampireThoughts);
        ui.SetCurtain(false, 1f);

        var vampire = FindObjectOfType<Vampire>();
        vampire.OnBloodChange += ui.SetBlood;
        vampire.OnSpeak += ui.SetVoiceText;
        vampire.Blood = _initialBlood;

        var teleports = FindObjectsOfType<Teleport>();
        foreach (var t in teleports)
        {
            t.OnCurtain += ui.SetCurtain;
        }
    }
}