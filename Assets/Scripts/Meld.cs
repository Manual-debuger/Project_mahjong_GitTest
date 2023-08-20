using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public void SetByTileSuitsList(List<TileSuits> tileSuits)
    {
        try
        {
            MeldTypes meldTypes = MeldTypes.Sequence;
            if (tileSuits.Count == 3)
            {
                if (tileSuits[0] == tileSuits[1] && tileSuits[1] == tileSuits[2])
                {
                    meldTypes = MeldTypes.Triplet;
                }
                else
                {
                    meldTypes = MeldTypes.Sequence;
                }
            }
            else if (tileSuits.Count == 1)
            {
                meldTypes = MeldTypes.ExposedQuadplet;
            }
            else if (tileSuits.Count == 2)
            {
                meldTypes = MeldTypes.ConcealedQuadplet;
            }
            else
            {
                Debug.LogError("Error:Meld.SetByTileSuitsList() tileSuits.Count!=1,2,3");
                throw new System.Exception("Error:Meld.SetByTileSuitsList() tileSuits.Count!=1,2,3");
            }
            _meldType = meldTypes;
            switch (meldTypes)
            {
                case MeldTypes.Sequence:
                case MeldTypes.Triplet:
                    for(int i = 0; i < tileSuits.Count; i++)
                    {
                        _meldTileComponents[i].Appear();
                        _meldTileComponents[i].TileSuit = (tileSuits[i]);
                        _meldTileComponents[i].ShowTileFrontSide();
                    }
                    break;
                case MeldTypes.ExposedQuadplet:
                    for(int i=0; i<4;i++)
                    {
                        _meldTileComponents[i].Appear();
                        _meldTileComponents[i].TileSuit = (tileSuits[0]);
                        _meldTileComponents[i].ShowTileFrontSide();
                    }
                    break;
                case MeldTypes.ConcealedQuadplet:
                    if (tileSuits[1]==TileSuits.NULL)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            _meldTileComponents[i].Appear();                        
                            _meldTileComponents[i].ShowTileBackSide();
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            _meldTileComponents[i].Appear();
                            _meldTileComponents[i].ShowTileBackSide();
                        }
                        _meldTileComponents[3].Appear();
                        _meldTileComponents[3].TileSuit = (tileSuits[1]);
                        _meldTileComponents[3].ShowTileFrontSide();
                    }
                    break;
            }
            
        }
        catch (System.Exception)
        {

            throw;
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
