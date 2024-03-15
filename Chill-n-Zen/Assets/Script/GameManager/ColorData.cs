using UnityEngine;

public class ColorData : MonoBehaviour
{
    [SerializeField] Color _color1;
    [SerializeField] Color _color2;
    [SerializeField] Color _color3;
    [SerializeField] Color _color4;
    [SerializeField] Color _color5;
    [SerializeField] Color _color6;
    [Space(7)]
    [SerializeField] Color _groundColor1;
    [SerializeField] Color _groundColor2;
    [SerializeField] Color _groundColor3;
    [Space(7)]
    [SerializeField] Color _wallColor1;
    [SerializeField] Color _wallColor2;
    [SerializeField] Color _wallColor3;

    public Color Color1 {  get { return _color1; } }
    public Color Color2 { get { return _color2; } }
    public Color Color3 { get { return _color3; } }
    public Color Color4 { get { return _color4; } }
    public Color Color5 { get { return _color5; } }
    public Color Color6 { get { return _color6; } }

    public int GroundIndex { get; set; }
    public Color GroundColor
    {
        get
        {
            Color[] groundColors = { _groundColor1, _groundColor2, _groundColor3 };
            return groundColors[GroundIndex]; 
        }
    }
    public int WallIndex { get; set; }
    public Color WallColor
    {
        get
        {
            Color[] wallColors = { _wallColor1, _wallColor2, _wallColor3 };
            return wallColors[WallIndex];
        }
    }
}
