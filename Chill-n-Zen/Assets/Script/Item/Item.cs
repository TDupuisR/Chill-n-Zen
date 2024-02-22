using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagerSpace;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public Texture2D asset2D;
    public GMStatic.tagRoom room;
    public GMStatic.tagType type;
    public GMStatic.tagStyle style;
    public List<GMStatic.tagUsage> listUsage;
    [Range(0, 10000)]
    public int price;
}


