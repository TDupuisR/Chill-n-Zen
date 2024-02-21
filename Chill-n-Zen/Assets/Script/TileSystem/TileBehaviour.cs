using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRender;
    [SerializeField] LineRenderer _lineRender;

    Vector3[] _patternPosition = new Vector3[5];

    private void OnEnable()
    {
        TileSystem.OnShowGrid += ShowGrid;
        TileSystem.OnShowGrid += GridReset;

        ChangeAesthetic();
        GridInitialise();
    }
    private void OnDisable()
    {
        TileSystem.OnShowGrid -= ShowGrid;
        TileSystem.OnShowGrid -= GridReset;
    }

    private void ChangeAesthetic()
    {
        Debug.LogError(" (error : 3x0) Missing Floor texture or color ", gameObject);
        // _renderer.sprite = // GameManager get Floor Texture //
        _spriteRender.color = new Color(255, 0, 132); // GameManager get Floor Color //
    }

    private void GridReset()
    {
        _lineRender.startColor = Color.white;
        _lineRender.endColor = Color.white;

        for (int i = 0; i < 5; i++)
        {
            _lineRender.SetPosition(i, _patternPosition[i] + transform.position);
        }
    }
    private void GridInitialise()
    {
        for (int i = 0; i < 5; i++)
        {
            _patternPosition[i] = _lineRender.GetPosition(i);
        }
        _lineRender.enabled = false;
    }

    private void ShowGrid()
    {
        _lineRender.enabled = !_lineRender.enabled;
    }
}
