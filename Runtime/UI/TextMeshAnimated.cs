using TMPro;
using UnityEngine;

public class TextMeshAnimated : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Animation _animation;

    public void PlayAnimatedText(string text)
    {
        _text.text = text;
        _animation.Play();
    }
}
