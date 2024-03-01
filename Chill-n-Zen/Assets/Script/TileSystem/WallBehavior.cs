using GameManagerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _leftWall;
    [SerializeField] private GameObject _rightWall;
    private List<GameObject> _wallList = new List<GameObject>();
    private Vector2 _posToInstanciateWall;
    private Vector2 _noTile;
    private Vector3 _posWall;
    private Vector3 _vectorRight;
    private Vector3 _vectorLeft;


    public Vector2 Rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

        public void InstantiateWall()
    {
        _vectorRight = (TileSystem.Instance.TilesList[1].transform.position- TileSystem.Instance.TilesList[0].transform.position)/2;
        _vectorLeft = Quaternion.Euler(0f, 0f, -90) * _vectorRight;
        for (int i  = 0; i<TileSystem.Instance.TilesList.Count; i++) 
        {
            Debug.Log("inin");
            _posToInstanciateWall = CanInstantiateWall(TileSystem.Instance.WorldToGrid(TileSystem.Instance.TilesList[i].transform.position).x, TileSystem.Instance.WorldToGrid(TileSystem.Instance.TilesList[i].transform.position).y);
            if (_posToInstanciateWall.x == 1)
            {
                Debug.Log("in");
                _posWall =  TileSystem.Instance.TilesList[i].transform.position + _vectorRight;
                Instantiate(_rightWall,_posWall,Quaternion.identity);
            }
            if(_posToInstanciateWall.y == 1)
            {
                Debug.Log("in");
                _posWall = TileSystem.Instance.TilesList[i].transform.position + _vectorLeft;
                Instantiate(_leftWall, _posWall, Quaternion.identity);
            }
        }
    }

    private void DeleteWall()
    {
        
    }

    private Vector2 CanInstantiateWall(int posGridX,int posGridY)
    {
        if(TileSystem.Instance.CheckTileExist(posGridX + 1, posGridY) == -1)
        {
            _noTile.x = 1;
        }
        if (TileSystem.Instance.CheckTileExist(posGridY, posGridY + 1) == -1)
        {
            _noTile.y = 1;
        }
        return _noTile;
    }
}
