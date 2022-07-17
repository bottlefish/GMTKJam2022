using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayEffect : MonoBehaviour
{
    public VisualEffect ve;
    public ParticleSystem ps1;
    public ParticleSystem ps2;
    public GameObject obj;
    public Material flash;
    private Material old_material;



    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if(ps1!=null){
               ps1.Play(); 
            }
            if(ps2!=null){
               ps2.Play(); 
            }
            if(ve!=null){
               ve.Play(); 
            }
            if(obj!=null&&flash!=null){
                old_material = obj.GetComponent<MeshRenderer>().sharedMaterial;
                obj.GetComponent<MeshRenderer>().sharedMaterial = flash;
                Invoke("changeback",0.02f);
            }
        }
    }

    void changeback(){
        obj.GetComponent<MeshRenderer>().sharedMaterial = old_material;
    }
}
