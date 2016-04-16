using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField]
    private Text _location;

    [SerializeField]
    private Image[] _blood;

    [SerializeField]
    private Sprite _bloodFull;

    [SerializeField]
    private Sprite _bloodEmpty;

    public void SetBlood(int value)
    {
        for (int i = 0; i < _blood.Length; i++)
        {
            _blood[i].sprite = (i < value) ? _bloodFull : _bloodEmpty;
        }
    }

    public void SetLocation(string value)
    {
        _location.text = value;
    }
}