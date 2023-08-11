using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Color _darkColor, _lightColor, _highlightColor, _checkColor, _attackedColor;
    [SerializeField] private SpriteRenderer _renderer;

    public void Init(bool isDarkColor)
    {
        _renderer.color = isDarkColor ? _darkColor : _lightColor;
    }

    public void Highlight()
    {
        _renderer.color = _highlightColor;
    }

    public void Check()
    {
        _renderer.color = _checkColor;
    }

    public void Attacked()
    {
        _renderer.color = _attackedColor;
    }
}
