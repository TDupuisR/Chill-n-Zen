using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagerSpace;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public Sprite asset2D;
    public GMStatic.tagRoom room;
    public GMStatic.tagType type;
    public GMStatic.tagStyle style;
    public List<GMStatic.tagUsage> listUsage;
    [Range(0, 10000)]
    public int price;
    public Vector3Int size; // X = Width // Y = Depth // Z = Height // Front side at spawn is always x = -1 //

    public int orientation; // 0 - 90 - 180 - 270 //
    //public 
}


