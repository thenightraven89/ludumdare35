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

    [SerializeField]
    private Image _curtain;

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

    public void SetCurtain(bool value, float time)
    {
        StartCoroutine(SetCurtainCoroutine(value, time));
    }

    private IEnumerator SetCurtainCoroutine(bool value, float time)
    {
        _curtain.enabled = true;

        var fromColor = value ? new Color(0f, 0f, 0f, 0f) : Color.black;
        var toColor = value ? Color.black : new Color(0f, 0f, 0f, 0f);

        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            _curtain.color = Color.Lerp(fromColor, toColor, t / time);
            yield return null;
        }

        _curtain.enabled = value;
    }
}