using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeldsAreaController : MonoBehaviour,IInitiable
{

    [SerializeField] private List<Meld> _meldControllers = new List<Meld>();
    private int _meldCount = 0;
    public void Init()
    {
        _meldCount = 0;
        foreach(var meld in _meldControllers)
        {
            meld.Init();
        }
    }
    public void AddMeld(List<TileSuits> tileSuits)
    {
        _meldControllers[_meldCount].SetByTileSuitsList(tileSuits);
        _meldCount++;
    }            
    public void SetDoors(List<List<TileSuits>> doors)
    {
        Init();
        foreach(var door in doors)
        {
            AddMeld(door);
        }
    }
    void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
