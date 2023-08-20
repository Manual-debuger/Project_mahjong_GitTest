using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//singleton
public class TileComponentFactory: MonoBehaviour
{
    private static TileComponentFactory _instance=new TileComponentFactory();
    private TileComponentFactory() { }
    public static TileComponentFactory Instance { get { return _instance; } }

    public TileComponent CreateTileComponent()
    {
        throw new NotImplementedException();
        return new TileComponent();
    }
}
    
