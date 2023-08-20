using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileComponent : MonoBehaviour,IInitiable
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Transform _transform;
    private Material _defaultTileMaterial;
    private Material _highLightedTileMaterial;
    private TileSuits _tileSuits;
    
    public TileSuits TileSuit { 
        get { return this._tileSuits; }
        set {
                this._tileSuits = value;
                //Debug.Log($"TileCompoent Changing mesh into {value}:{(int)value}");            
                _meshFilter.mesh= AssetsPoolController.Instance.TileMeshs[(int)value];
            }
    }
    public void Init()
    {
        this.Disappear();
        this.ShowTileBackSide();
    }
    private void Awake()
    {
        if (_meshFilter == null)
            _meshFilter = this.GetComponent<MeshFilter>();
        if (_meshRenderer == null)
            _meshRenderer = this.GetComponent<MeshRenderer>();
        if(_transform==null)
            _transform = this.GetComponent<Transform>();
        if (_defaultTileMaterial == null)
            _defaultTileMaterial = AssetsPoolController.Instance.DefaultTileMaterial;
        if (_highLightedTileMaterial == null)
            _highLightedTileMaterial = AssetsPoolController.Instance.HighLightedTileMaterial;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTileFrontSide()
    {
        //Debug.Log($"_transform.eulerAngles:{_transform.eulerAngles.x}");
        //Debug.Log($"_transform.localEulerAngles:{_transform.localEulerAngles.x}");
        if (_transform.eulerAngles.x==270)//牌是直立的
        {
            
        }            
        else
        {           
            _transform.rotation = Quaternion.Euler(_transform.eulerAngles.x, _transform.eulerAngles.y, 0);
        }            
    }
    
    public void ShowTileBackSide()
    {
        if(_transform.eulerAngles.x== 270)//牌是直立的
        {
           
        }            
        else
        {
            _transform.rotation = Quaternion.Euler(_transform.eulerAngles.x, _transform.eulerAngles.y, 180);
        }            
    }
    public void Disappear()
    {
        this._meshRenderer.enabled = false;
    }
    public void Appear()
    {
        this._meshRenderer.enabled = true;
    }
    public void HighLight()
    {
        if(this._meshRenderer.material!=_highLightedTileMaterial)
            this._meshRenderer.material = _highLightedTileMaterial;

    }
    public void UnHighLight()
    {
        if(this._meshRenderer.material!=_defaultTileMaterial)
            this._meshRenderer.material = _defaultTileMaterial;
    }
}
