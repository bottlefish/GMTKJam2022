using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class SceneArtManager : Singleton<SceneArtManager>
{
    public Material GroundMat;
    public Light DirectionalLight;
    public MeshRenderer GroundMesh;
    [HideInInspector]
    public Material GroundMatInst;
    [HideInInspector]
    public Color InitGroundColor;

    private void Start()
    {
        GroundMatInst = new Material(GroundMat);
        Material[] MaterialList = GroundMesh.materials;
        MaterialList[0] = GroundMatInst;
        GroundMesh.materials = MaterialList;
        InitGroundColor = GroundMatInst.GetColor("LitColor");

        // ²âÊÔ¶¯»­
        //int Level = 1;
        //Material GroundMat2 = SceneArtManager.Instance.GroundMatInst;
        //var MyTween = DOTween.To(() => { return GroundMat2.GetColor("LitColor"); },
        //    (_color) => { GroundMat2.SetColor("LitColor", _color); },
        //    SceneArtManager.Instance.InitGroundColor * (Level + 1),
        //    3f);
        //MyTween.Play();
    }
}
