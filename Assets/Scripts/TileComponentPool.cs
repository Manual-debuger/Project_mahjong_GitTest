using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
//Duty: To create and manage a pool of tile components 要拿牌都從這裡拿
//singleton
public class TileComponentPool : MonoBehaviour
{
    private static TileComponentPool _instance;
    private TileComponentPool() { }
    private TileComponentFactory _tileComponentFactory;
    
    public static TileComponentPool Instance { get { return _instance; } }
    public TileComponent GetTileComponent()
    {
        throw new NotImplementedException();
        return new TileComponent();
    }
    public void Start()
    {
        
    }

    public TileComponent TileComponent
    {
        get => default;
        set
        {
        }
    }

    public TileComponentFactory TileComponentFactory
    {
        get => default;
        set
        {
        }
    }
}


