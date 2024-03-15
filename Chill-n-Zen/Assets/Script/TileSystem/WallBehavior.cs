using GameManagerSpace;
using System.Collections.Generic;
using UnityEngine;

public class WallBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _leftWall;
    [SerializeField] private GameObject _rightWall;
    [SerializeField] private Transform _wallParent;
    [Range(0f, 1f)]
    [SerializeField] private float _opacityWall;

    private List<GameObject> _wallList = new List<GameObject>();
    private Vector2 _posToInstanciateWall;
    private Vector2 _noTile;
    private Vector3 _posWall;
    private Vector3 _vectorRight;
    private Vector3 _vectorLeft;
    private bool _wallBehind = false;
    private int _posGridX;
    private int _posGridY;



    public void InstantiateWall()
    {

        ClearWallList();
        DistanceBetweenTile();
        for (int i = 0; i < TileSystem.Instance.TilesList.Count; i++)
        {
            _posToInstanciateWall = CanInstantiateWall(TileSystem.Instance.WorldToGrid(TileSystem.Instance.TilesList[i].transform.position).x, TileSystem.Instance.WorldToGrid(TileSystem.Instance.TilesList[i].transform.position).y);
            if (_posToInstanciateWall.x == 1)
            {
                _posWall = TileSystem.Instance.TilesList[i].transform.position + _vectorRight;
                GameObject wall = Instantiate(_rightWall, _posWall, Quaternion.identity);
                SpriteRenderer spriteRenderer = wall.transform.GetChild(0).GetComponent<SpriteRenderer>();

                spriteRenderer.color = GameManager.colorData.WallColor;
                wall.transform.parent = _wallParent;
                wall.transform.GetChild(0);
                if (_wallBehind)
                {
                    spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, _opacityWall);
                }
                _wallList.Add(wall);
                
            }
            if (_posToInstanciateWall.y == 1)
            {
                _posWall = TileSystem.Instance.TilesList[i].transform.position + _vectorLeft;
                GameObject wall = Instantiate(_leftWall, _posWall, Quaternion.identity);
                SpriteRenderer spriteRenderer = wall.transform.GetChild(0).GetComponent<SpriteRenderer>();

                spriteRenderer.color = GameManager.colorData.WallColor;
                wall.transform.parent = _wallParent;
                if (_wallBehind)
                {
                    spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, _opacityWall);
                }
                _wallList.Add(wall);
            }
        }
    }

    void DistanceBetweenTile()
    {
        for (int i = 0; i < TileSystem.Instance.TilesList.Count; i++)
        {
            if (CanInstantiateWall(TileSystem.Instance.WorldToGrid(TileSystem.Instance.TilesList[i].transform.position).x, TileSystem.Instance.WorldToGrid(TileSystem.Instance.TilesList[i].transform.position).y) == new Vector2(0, 0))
            {
                _posGridX = TileSystem.Instance.WorldToGrid(TileSystem.Instance.TilesList[i].transform.position).x;
                _posGridY = TileSystem.Instance.WorldToGrid(TileSystem.Instance.TilesList[i].transform.position).y;
                _vectorLeft = (TileSystem.Instance.TilesList[TileSystem.Instance.CheckTileExist(_posGridX, _posGridY + 1)].transform.position - TileSystem.Instance.TilesList[i].transform.position) / 2;
                _vectorRight = (TileSystem.Instance.TilesList[TileSystem.Instance.CheckTileExist(_posGridX + 1, _posGridY)].transform.position - TileSystem.Instance.TilesList[i].transform.position) / 2;
                return;
            }
        }
    }

    private Vector2 CanInstantiateWall(int posGridX,int posGridY)
    {
        _wallBehind = false;
        _noTile = new Vector2(0,0);
        if (TileSystem.Instance.CheckTileExist(posGridX + 1, posGridY) == -1)
        {
            _noTile.x = 1;
        }
        if (TileSystem.Instance.CheckTileExist(posGridX, posGridY + 1) == -1)
        {
            _noTile.y = 1;
        }
        if (TileSystem.Instance.CheckTileExist(posGridX+ 1, posGridY + 1) != -1 || TileSystem.Instance.CheckTileExist(posGridX + 2, posGridY + 2) != -1|| TileSystem.Instance.CheckTileExist(posGridX + 3, posGridY + 3) != -1)
        {
            _wallBehind = true;
        }
        return _noTile;
    }

    private void ClearWallList()
    {
        for (int i = 0; i< _wallList.Count; i++)
        {
            Destroy(_wallList[i]);
        }
        _wallList.Clear();
    }
}
