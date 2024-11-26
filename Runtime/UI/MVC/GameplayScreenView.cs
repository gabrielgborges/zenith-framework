using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayScreenView : ScreenViewBase
{
    [SerializeField] private Image _wonRoundMark;
    [SerializeField] private TextMeshAnimated _textMeshAnimated;
    [SerializeField] private TextMeshProUGUI _roundTimer;
    [SerializeField] private TextMeshProUGUI _roundWinnerAnnounce;

    public void PlayStartGameAnimation(string text)
    {
        _textMeshAnimated.PlayAnimatedText(text);
    }

    public void SetMiddleText(string text)
    {
        _roundWinnerAnnounce.text = text;
    }
    
    protected override void Initialize()
    {
        
    }

    protected override void InitializeController()
    {
        _controller.Initialize(this);
    }
}
