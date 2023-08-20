using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportCaculator : MonoBehaviour
{
    private static SupportCaculator _instance;
    public static SupportCaculator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SupportCaculator();
            }
            return _instance;
        }
    }
    [SerializeField] private AbandonedTilesAreaController _abandonedTilesAreaController;

    public void HighLightDiscardTiles(TileSuits tileSuit)
    {
        _abandonedTilesAreaController.HighLightDiscardTiles(tileSuit);
    }
    public void UnHighLightDiscardTiles()
    {
        _abandonedTilesAreaController.UnHighLightDiscardTiles();
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else if (_instance == null)
            _instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
