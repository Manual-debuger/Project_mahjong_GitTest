using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//提供只有本地玩家才有的操作
public class PlayerController : PlayerControllerBase
{
    // Start is called before the first frame update
    private static PlayerController _instance;
    private PlayerController() { }
    public static PlayerController Instance { get { return _instance; } }

    [SerializeField] private InGameUIController _inGameUIController;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else if (_instance == null)
        {
            _instance = this;
            if (_inGameUIController == null)
                _inGameUIController = this.GetComponent<InGameUIController>();
        }
    }
    void Start()
    {
        
    }
    public override void Init()
    {
        base.Init();
    }
    public override void SetSeatInfo(SeatInfo seatInfo)
    {
        base.SetSeatInfo(seatInfo);        
    }
    public override void SetHandTiles(List<TileSuits> tileSuits, bool IsDrawing = false)
    {
        try
        {
            Debug.LogWarning($"PlayerController.SetHandTiles(List<TileSuits> tileSuits, bool IsDrawing ={IsDrawing})");
            //for (int i = 0; i < tileSuits.Count; i++)
            //{
            //    Debug.Log(i + " : " + tileSuits[i]);
            //}

            Debug.LogWarning($"size ={tileSuits.Count}),IsDrawing ={IsDrawing})");
            _inGameUIController.SetHandTile(tileSuits, IsDrawing);
        }
        catch (System.Exception)
        {
            throw;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
