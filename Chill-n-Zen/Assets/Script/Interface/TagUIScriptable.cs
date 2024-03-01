using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TagUI", menuName = "TagUI")]
public class TagUIScriptable : ScriptableObject
{
    public List<string> tagNames;
    public List<Sprite> associatedSprite;
}
