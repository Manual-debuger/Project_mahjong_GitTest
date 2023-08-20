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
    public void AddMeld(MeldTypes meldTypes,List<TileSuits> tileSuits)
    {
        _meldControllers[_meldCount].SetByMeldTypeAndTileSuits(meldTypes, tileSuits);
        _meldCount++;
    }            
    
    void Awake()
    {
        Init();
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
