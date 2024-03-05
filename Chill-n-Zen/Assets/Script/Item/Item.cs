using System.Collections.Generic;
using UnityEngine;
using GameManagerSpace;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class Item : ScriptableObject
{
    [Header("Sprites")]
    public Sprite spriteOneFixed;
    public Sprite spriteOneColored;
    public Sprite spriteTwoFixed;
    public Sprite spriteTwoColored;

    [Header("Tags")]
    public GMStatic.tagRoom room;
    public GMStatic.tagType type;
    public GMStatic.tagMaterial style;
    public List<GMStatic.tagUsage> listUsage;

    [Header("Constraint")]
    public GMStatic.constraint constraint;
    public bool doorAccess;

    [Header("Combo")]
    public int frontBonus;
    public List<GMStatic.tagUsage> frontUsageBonus;
    public List<Item> frontItemBonus;
    [Space(7)]
    public int proxiBonus;
    public List<GMStatic.tagUsage> proxyUsageBonus;
    public List<Item> proxyItemBonus;
    [Space(7)]
    public int onBonus;
    public List<GMStatic.tagUsage> onUsageBonus;
    public List<Item> onItemBonus;

    [Header("Specification")]
    public int score;
    [Range(0, 10000)]
    public int price;
    public Vector3Int size; // X = Depth // Y = Width // Z = Height // Front side at spawn is always in direction x = -1 // Mustn't have negative nor 0 as values (exept: z = 0) //
    public bool fullRotation; // false = 0° - 90° // true = 0° - 90° - 180° - 270° //
}