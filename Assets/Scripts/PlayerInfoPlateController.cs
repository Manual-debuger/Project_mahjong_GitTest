using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfoPlateController : MonoBehaviour
{
    [SerializeField] private MeshRenderer _userAvatarMeshRenderer;
    [SerializeField] private TextMeshPro _userNameTextMeshPro;
    [SerializeField] private TextMeshPro _windPosisionTextMeshPro;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetUserName(string name)
    {
        _userNameTextMeshPro.text = name;
    }
    public void SetWindPosision(string wind)
    {
        _windPosisionTextMeshPro.text = wind;
    }
    public void SetUserAvaterPhoto(Material material)
    {
        _userAvatarMeshRenderer.material = material;
        _userAvatarMeshRenderer.materials[0] = material;
    }
}
