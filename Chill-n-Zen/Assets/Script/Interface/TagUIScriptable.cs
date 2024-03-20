using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TagUI", menuName = "TagUI")]
public class TagUIScriptable : ScriptableObject
{
    public List<string> materialName;
    public List<Sprite> materialSprite;
    public List<string> roomName;
    public List<Sprite> roomSprite;
    public List<string> TypeName;
    public List<Sprite> TypeSprite;
}
