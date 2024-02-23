using UnityEngine;

public class ItemPlacement : MonoBehaviour
{
    [SerializeField] SpriteRenderer _uncoloredSprite;
    [SerializeField] SpriteRenderer _coloredSprite;

    Item _item;

    public void Initialize(Item item)
    {
        _item = item;
    }

    private void ResizeLineRenderer()
    {

    }
}
