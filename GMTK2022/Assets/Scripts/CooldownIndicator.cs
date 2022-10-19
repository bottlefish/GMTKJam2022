using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// @Snoww：临时代码，用于做射击CD 和 冲刺CD 的表现层。
public class CooldownIndicator : MonoBehaviour
{
    public GunController gunController;
    public PlayerMovement playerMovement;
    public SkinnedMeshRenderer PlayerMesh;
    public LineRenderer ShootingLine;

    public Material HatMaterial;
    public Color CanNotDashColor, CanDashColor;
    private Material HatMatInstance;
    // Start is called before the first frame update
    void Start()
    {
        HatMatInstance = new Material(HatMaterial);
        Material[] MaterialList = PlayerMesh.materials;
        MaterialList[2] = HatMatInstance;
        PlayerMesh.materials = MaterialList;
    }

    void UpdateHatMat()
    {
        HatMatInstance.color = playerMovement.GetCanDash() ? CanDashColor : CanNotDashColor;
        //PlayerMesh.materials[2] = playerMovement.GetCanDash() ? CanDashMat : CanNotDashMat;
    }

    void UpdateLineRender()
    {
        ShootingLine.SetPosition(0, ShootingLine.transform.position);
        ShootingLine.SetPosition(1, ShootingLine.transform.position + playerMovement.transform.forward * 10
            );
        ShootingLine.enabled = gunController.CanShoot();
    }

    private void LateUpdate()
    {
        UpdateHatMat();
        UpdateLineRender();
    }
}
