using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private EffectController _effectController;

    async void Start()
    {
        var message = (await ChatGPTRequester.CallChatGPT("introduce youself")).choices[0].message.content;
        Debug.Log(message);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayEffect(string effectID)
    {
         //_effectController.PlayEffect((EffectID)int.Parse(effectID), 0);
    }
}