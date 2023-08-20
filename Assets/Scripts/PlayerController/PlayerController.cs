using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���ѥu�����a���a�~�����ާ@
public class PlayerController : PlayerControllerBase
{
    // Start is called before the first frame update
    private static PlayerController _instance;
    private PlayerController() { }
    public static PlayerController Instance { get { return _instance; } }

    private void Awake()
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
