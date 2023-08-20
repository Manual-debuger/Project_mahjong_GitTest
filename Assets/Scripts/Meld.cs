using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//一刻牌(一組 吃/碰/槓)
public class Meld : MonoBehaviour,IInitiable
{
    private MeldTypes _meldType;
    [SerializeField]
    private List<TileComponent> _meldTileComponents = new List<TileComponent>();
    public MeldTypes MeldType { get { return _meldType; }  }
    public List<TileComponent> MeldTiles
    {
        get { return _meldTileComponents; }
    }
    public void Init()
    {
        foreach (var tile in _meldTileComponents)
        {
            tile.Disappear();
            tile.ShowTileBackSide();
        }
    }

    public void SetByMeldTypeAndTileSuits(MeldTypes meldTypes, List<TileSuits> tileSuits)
    {
        _meldType = meldTypes;
        for (int i = 0; i < tileSuits.Count; i++)
        {
            _meldTileComponents[i].Appear();
            _meldTileComponents[i].TileSuit=(tileSuits[i]);
            switch(meldTypes)
            {
                case MeldTypes.Sequence:
                case MeldTypes.Triplet:
                case MeldTypes.ExposedQuadplet:
                    _meldTileComponents[i].ShowTileFrontSide();
                    break;                
                case MeldTypes.ConcealedQuadplet:
                    _meldTileComponents[i].ShowTileBackSide();
                    break;
            }
        }        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
}
