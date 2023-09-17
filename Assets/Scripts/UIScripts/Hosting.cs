using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using APIDataNamespace;

public class Hosting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AutoPlay()
    {
        APIData.instance.SentAutoPlay();
    }

    public void Appear()
    {
        this.gameObject.SetActive(true);
    }
    public void Disappear()
    {
        this.gameObject.SetActive(false);
    }
}
