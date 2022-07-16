using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CustomizableButton : MonoBehaviour
{
    [SerializeField]
    private Image _image;
    [SerializeField]
    private TMP_Text _text;
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Image _highlight;
    [SerializeField]
    private Color _fadeColor = Color.grey;

    public void SetIcon(Sprite sprite)
    {
        _image.sprite = sprite;
    }
    public void SetText(string text)
    {
        _text.text = text;
    }
    public void AddClickEvent(Action clickAction)
    {
        _button.onClick.AddListener(() => clickAction());
    }
    public void SetFade(bool state)
    {
        _image.color = state ? _fadeColor : Color.white;
    }
    public void SetHighlight(bool state)
    {
        var color = _highlight.color;
        color.a = state ? 1.0f : 0.0f; ;
        _highlight.color = color;
    }
}