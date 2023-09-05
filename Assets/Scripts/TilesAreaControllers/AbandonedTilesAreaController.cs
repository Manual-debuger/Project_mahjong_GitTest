using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
//Server as a Controller for the abandoned tiles area
public class AbandonedTilesAreaController : MonoBehaviour,IInitiable
{
    [SerializeField]private List<SeaTilesAreaController> _abandonedTilesAreas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddTile(int targetIndex, TileSuits tileSuit)
    {
        _abandonedTilesAreas[targetIndex].AddTile(tileSuit);
        //throw new System.NotImplementedException();
    }

    public void HighLightDiscardTiles(TileSuits tileSuit)
    {
        foreach (var abandonedTilesArea in _abandonedTilesAreas)
        {
            abandonedTilesArea.HighLightDiscardTiles(tileSuit);
        }
    }
    public void UnHighLightDiscardTiles()
    {
        foreach (var abandonedTilesArea in _abandonedTilesAreas)
        {
            abandonedTilesArea.UnHighLightDiscardTiles();
        }
    }
    public void SetTiles(int targetIndex, List<TileSuits> tileSuits)
    {
        _abandonedTilesAreas[targetIndex].SetTiles(tileSuits);
    }

    public void Init()
    {
        foreach(SeaTilesAreaController seaTilesAreaController in _abandonedTilesAreas)
        {
            seaTilesAreaController.Init();
        }
    }
}