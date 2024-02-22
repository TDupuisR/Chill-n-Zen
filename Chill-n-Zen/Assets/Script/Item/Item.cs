using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public Sprite asset2D;
    public GameManagerSpace.GMStatic.tagRoom room;
    public GameManagerSpace.GMStatic.tagType type;
    public GameManagerSpace.GMStatic.tagStyle style;
    public List<GameManagerSpace.GMStatic.tagUsage> listUsage;
    [Range(0, 10000)]
    public int price;
}


