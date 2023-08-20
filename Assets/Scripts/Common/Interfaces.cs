using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

interface IInitiable
{
    public void Init();
}

interface IPopTileAble
{
    public void PopLastTile();
}
interface IReturnTileSuitsAble
{
    public TileSuits[] GetTileSuits();
    
}
