using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class  AssetsPoolController:MonoBehaviour
{
    [SerializeField] private Mesh[] _tileMeshs;
    public Mesh[] TileMeshs {
        get { return _instance._tileMeshs; }
    }

    [SerializeField] private Sprite[] _tileSprites;
    public Sprite[] TileSprites
    {
        get { return _instance._tileSprites; }
    }

    [SerializeField] private Material _defaultTileMaterial;
    [SerializeField] private Material _highLightedTileMaterial;
    [SerializeField] private GameObject[] _effectsPrefabs;
    [SerializeField] private Dictionary<EffectID, GameObject> _effectsDict;
    
    public Material DefaultTileMaterial
    {
        get { return _instance._defaultTileMaterial; }
    }
    public Material HighLightedTileMaterial
    {
        get { return _instance._highLightedTileMaterial; }
    }
    public Dictionary<EffectID, GameObject> EffectsDict
    {
        get { return _instance._effectsDict; }
    }
    private static AssetsPoolController _instance;
    public static AssetsPoolController Instance
    {
        get
        {
            if (_instance == null)
                _instance = new AssetsPoolController();
            return _instance;
        }
    }
    private AssetsPoolController()
    {
        if (_instance == null)
        {
            _instance = this;            
        }       
    }
    // Start is called before the first frame update
    private void Awake()
    {
        if(_instance._effectsDict==null)
            _instance._effectsDict = new Dictionary<EffectID, GameObject>
            {
                { EffectID.Chow,Instantiate(_effectsPrefabs[0]) },
                { EffectID.Pong,Instantiate(_effectsPrefabs[1])},
                { EffectID.Kong, Instantiate(_effectsPrefabs[2]) }
            };

            
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
