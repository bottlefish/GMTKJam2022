using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSpecialDice : TraitCustomScript
{
    public override void Activate(string EffectStr, TraitCardObject CardObject)
    {
        int Extra = 0;
        if (EffectStr.Equals("Odd")) Extra = 1;
        if (EffectStr.Equals("Even")) Extra = 2;
        var gunController = FindObjectOfType<GunController>();
        gunController.OnMouseRightClickDel +=
            delegate
            {
                int RandValue = 2 * Random.Range(0, 2) + Extra;
                gunController.TryShootDice(RandValue);
            };


    }

    public override void Deactivate(string EffectStr)
    {
        throw new System.NotImplementedException();
    }
}


public class DashToMouse : TraitCustomScript
{

    Vector3 GetMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            return point;
        }
        return Vector3.zero;
    }

    public override void Activate(string EffectStr, TraitCardObject CardObject)
    {
        var playerMovement = FindObjectOfType<PlayerMovement>();
        var gunController = FindObjectOfType<GunController>();
        gunController.OnMouseRightClickDel +=
            delegate
            {
                playerMovement.TryMoveTo(GetMousePos());
            };


    }

    public override void Deactivate(string EffectStr)
    {
        throw new System.NotImplementedException();
    }
}

public class KillAllEnemy : TraitCustomScript
{
    public override void Activate(string EffectStr, TraitCardObject CardObject)
    {
        var Enemies = FindObjectsOfType<EnemyAiTutorial>();
        foreach(var Enemy in Enemies)
        {
            Destroy(Enemy.gameObject);
        }
    }

    public override void Deactivate(string EffectStr)
    {
        throw new System.NotImplementedException();
    }
}