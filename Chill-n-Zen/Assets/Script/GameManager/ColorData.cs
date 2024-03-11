using UnityEngine;

public class ColorData : MonoBehaviour
{
    [SerializeField] Color color1;
    [SerializeField] Color color2;
    [SerializeField] Color color3;
    [SerializeField] Color color4;
    [SerializeField] Color color5;

    public Color Color1 {  get { return color1; } }
    public Color Color2 { get { return color2; } }
    public Color Color3 { get { return color3; } }
    public Color Color4 { get { return color4; } }
    public Color Color5 { get { return color5; } }
}
