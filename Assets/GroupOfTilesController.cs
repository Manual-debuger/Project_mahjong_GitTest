using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public enum GroupOfTilesType
{
    None = 0,
    Sequence,//顺子三張照順序
    Triplet,//刻子三張一樣
    Quadruplet,//杠子四張一樣
}
public class GroupOfTilesController : MonoBehaviour,IInitiable
{
    private GroupOfTilesType _groupOfTilesType;
    public GroupOfTilesType Type { get { return _groupOfTilesType; } }
    [SerializeField] private List<TileComponent> _tileComponents = new List<TileComponent>();
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetByTileSuits(List<TileSuits> tileSuits, GroupOfTilesType groupOfTilesType)
    {
        for (int i = 0; i < tileSuits.Count; i++)
        {
            _tileComponents[i].TileSuit = tileSuits[i];
            _tileComponents[i].Appear();
            _tileComponents[i].ShowTileFrontSide();
        }
        _groupOfTilesType = groupOfTilesType;
        
    }
    public void Init()
    {
        foreach (TileComponent tile in _tileComponents)
        {
            tile.Disappear();
            tile.ShowTileBackSide();
        }
        _groupOfTilesType = GroupOfTilesType.None;
    }
}
