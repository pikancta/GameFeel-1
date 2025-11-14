using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Color DefaultColor {  get; private set; }

    [SerializeField] private SpriteRenderer _fillSpriteRenderer;
    [SerializeField] private Color[] _colors;

    public void SetColor(Color color)
    {
        _fillSpriteRenderer.color = color;
    }

}
