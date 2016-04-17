using UnityEngine;

public class InteractionText : MonoBehaviour
{
    [SerializeField]
    private string[] _lines;

    public string GetRandom()
    {
        if (_lines.Length > 0)
        {
            return _lines[Random.Range(0, _lines.Length)];
        }
        else
        {
            return "...";
        }
    }
}