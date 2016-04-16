using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    private int _initialBlood;

    [SerializeField]
    private string _location;

    [SerializeField]
    private string _nextScene;

    [SerializeField]
    private string _prevScene;

    public void Awake()
    {
        var ui = FindObjectOfType<UI>();
        ui.SetLocation(_location);

        var vampire = FindObjectOfType<Vampire>();
        vampire.OnBloodChange += ui.SetBlood;
        vampire.Blood = 5;
    }
}