using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

//Duty: 處理中間的贏分，風位，莊家等等
public class CentralAreaController : MonoBehaviour,IInitiable
{
    [SerializeField] private TextMeshPro _WallCountTextMeshPro;    
    [SerializeField] private List<TextMeshPro> _scoresTextMeshProList; // Default SWNE
    [SerializeField] private List<TextMeshPro> _dealersTextMeshProList;// Default SWNE
    [SerializeField] private List<MeshRenderer> _highlightBarMeshRenderList;   // Default SWNE
    private List<Material> _highlightBarMaterialList;
    void Awake()
    {
        _highlightBarMaterialList = new List<Material>()
        {
            AssetsPoolController.Instance.HighlightBarmaterialList[(int)HighlightBarState.Default],
            AssetsPoolController.Instance.HighlightBarmaterialList[(int)HighlightBarState.Highlight]
        };
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
        foreach(var highlightBarMeshRender in _highlightBarMeshRenderList)
        {
            highlightBarMeshRender.material = _highlightBarMaterialList[(int)HighlightBarState.Default];
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
        _dealersTextMeshProList[targetIndex].text =$"莊{remainingBankerCount.ToString()}";
    }
    public void SetWallCount(int number)
    {
        _WallCountTextMeshPro.text = number.ToString();
    }
    public void SetHighLightBar(int localPlayerIndex)
    {
        for(int i = 0; i < _highlightBarMeshRenderList.Count; i++)
        {
            if (i == localPlayerIndex)
                _highlightBarMeshRenderList[i].material = _highlightBarMaterialList[(int)HighlightBarState.Highlight];//
            else
                _highlightBarMeshRenderList[i].material = _highlightBarMaterialList[(int)HighlightBarState.Default];//
        }
    }
}
