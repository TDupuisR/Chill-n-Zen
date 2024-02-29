using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagerSpace;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public Sprite spriteOneFixed;
    public Sprite spriteOneColored;
    public Sprite spriteTwoFixed;
    public Sprite spriteTwoColored;

    public GMStatic.tagRoom room;
    public GMStatic.tagType type;
    public GMStatic.tagStyle style;
    public List<GMStatic.tagUsage> listUsage;
    [Range(0, 10000)]
    public int price;
    public Vector3Int size; // X = Depth // Y = Width // Z = Height // Front side at spawn is always in direction x = -1 // Mustn't have negative nor 0 as values (exept: z = 0) //
}


