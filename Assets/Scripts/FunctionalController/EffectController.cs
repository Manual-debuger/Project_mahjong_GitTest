using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Duty: �B�z�S��/�ʵe����������
public class EffectController : MonoBehaviour
{

    public EffectController Instance { get { return _instance; } }

    private EffectController _instance;
    private Dictionary<EffectID, GameObject> _effectsdict;

    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else if (_instance == null)
        {
            _instance = this;
            if (_effectsdict == null)
                _effectsdict = AssetsPoolController.Instance.EffectsDict;
        }
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
