using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private EffectController _effectController;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayEffect(string effectID)
    {
         _effectController.PlayEffect((EffectID)int.Parse(effectID), 0);
    }
    
}
