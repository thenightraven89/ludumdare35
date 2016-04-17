using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [SerializeField]
    private GameObject _gameOverText;

    [SerializeField]
    private RectTransform _keyBar;

    private Dictionary<string, Image> _items;

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

    public void AddCollected(string name)
    {
        _items[name].gameObject.SetActive(true);
    }

    public void RemoveCollected(string name)
    {
        _items[name].gameObject.SetActive(false);
    }

    public void InitInventory()
    {
        _items = new Dictionary<string, Image>();

        var allItems = _keyBar.GetComponentsInChildren<Image>(true);

        foreach (var item in allItems)
        {
            _items.Add(item.gameObject.name, item);
        }
    }

    public void GameOver(string intruder)
    {
        _gameOverText.SetActive(true);
        SetCurtain(true, 2f);
        SetVoiceText("INTRUDER ALERT!!!");
        StartCoroutine(ReloadCoroutine(4.5f));
    }

    private IEnumerator ReloadCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}