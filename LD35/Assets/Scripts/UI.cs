using System.Collections;
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

    [SerializeField]
    private GameObject _voiceCaption;

    [SerializeField]
    private Text _voiceText;

    private Coroutine _displayText;

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

    public void SetVoiceText(string value)
    {
        if (_displayText != null)
        {
            StopCoroutine(_displayText);
        }

        _displayText = StartCoroutine(DisplayVoiceTextCoroutine(value));
    }

    private IEnumerator DisplayVoiceTextCoroutine(string value)
    {
        _voiceCaption.SetActive(true);

        int charIndex = 1;
        while (charIndex < value.Length + 1)
        {
            _voiceText.text = value.Substring(0, charIndex);
            yield return new WaitForSeconds(0.025f);
            charIndex++;
        }

        yield return new WaitForSeconds(4f);

        _voiceCaption.SetActive(false);
    }
}