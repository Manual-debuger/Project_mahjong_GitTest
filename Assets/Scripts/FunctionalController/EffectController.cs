using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Duty: 處理特效/動畫相關的控制
public class EffectController : Singleton<EffectController>
{    
    
    private Dictionary<EffectID, GameObject> _effectsdict;
    [SerializeField] private List<Transform> _effectTransforms;
    
    // Start is called before the first frame update
    void Start()
    {
        if (_effectsdict == null)
            Instance._effectsdict = AssetsPoolController.Instance.EffectsDict;
        StopAllEffects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void PlayEffect(EffectID effectID,int PlayerIndex)
    {        
        var effectobject = Instance._effectsdict[EffectID.Chow];
        var effectTransform= effectobject.GetComponent<Transform>();
        effectTransform.position = Instance._effectTransforms[PlayerIndex].position;
        effectobject.SetActive(true);
    }
    public void StopAllEffects()
    {
        foreach (var effect in _effectsdict)
        {
            effect.Value.SetActive(false);
        }
    }
}
