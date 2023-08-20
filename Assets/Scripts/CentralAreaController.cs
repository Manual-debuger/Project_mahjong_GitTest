using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

//Duty: �B�z������Ĺ���A����A���a����
public class CentralAreaController : MonoBehaviour,IInitiable
{
    [SerializeField] private TextMeshPro _WallCountTextMeshPro;    
    [SerializeField] private List<TextMeshPro> _scoresTextMeshProList; // Default ESWN
    [SerializeField] private List<TextMeshPro> _dealersTextMeshProList;// Default ESWN
    void Awake()
    {
        Init();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Init()
    {
        foreach(var scoreTextMeshPro in _scoresTextMeshProList)
        {
            scoreTextMeshPro.text = "0";
        }
        foreach(var dealerTextMeshPro in _dealersTextMeshProList)
        {
            dealerTextMeshPro.text = "";
        }
        _WallCountTextMeshPro.text = "88";        
    }
    public void SetScore(int plyaerIndex, int score)
    {
        _scoresTextMeshProList[plyaerIndex].text = score.ToString();
        //throw new System.NotImplementedException();
    }
    public void SetBanker(int targetIndex,int remainingBankerCount)
    {
        _dealersTextMeshProList[targetIndex].text =$"��{remainingBankerCount.ToString()}";
    }
    public void SetWallCount(int number)
    {
        _WallCountTextMeshPro.text = number.ToString();
    }
    public void ReduceNumberOfRemainedTilesByOne()
    {
        throw new System.NotImplementedException();
    }
}
