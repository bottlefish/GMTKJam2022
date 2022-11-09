using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootOnDiceDropped : TraitCustomScript
{
    public override void Activate(string EffectStr, TraitCardObject CardObject)
    {



    }

    public override void Deactivate(string EffectStr)
    {
        throw new System.NotImplementedException();
    }
}

public class BiggerDice : TraitCustomScript
{
    public override void Activate(string EffectStr, TraitCardObject CardObject)
    {
        int DiceToExpand = int.Parse(EffectStr);
        var gunController = FindObjectOfType<GunController>();
        gunController.onDiceShootDelList.Add(delegate (DiceController diceController)
        {
            if (diceController.State == DiceToExpand)
                diceController.transform.localScale = diceController.transform.localScale * 1.5f;
        });
    }

    public override void Deactivate(string EffectStr)
    {
        throw new System.NotImplementedException();
    }
}

public class EveryShootCheck : TraitCustomScript
{
    int CurrentShootTime = 0;
    public override void Activate(string EffectStr, TraitCardObject CardObject)
    {
        int EveryShootTimes = int.Parse(EffectStr);
        var gunController = FindObjectOfType<GunController>();
        gunController.onDiceShootDelList.Add(delegate (DiceController diceController)
        {
            CurrentShootTime = (CurrentShootTime + 1) % 9;
            if (CurrentShootTime == 0)
            {
                var Enemies = FindObjectsOfType<EnemyAiTutorial>();
                var Player = FindObjectOfType<PlayerMovement>();
                foreach (var Enemy in Enemies)
                {
                    Vector3 v1 = (Enemy.transform.position - Player.transform.position).normalized;
                    Vector3 v2 = Player.transform.forward;
                    if (Vector3.Angle(v2, v1) < 45)
                        Destroy(Enemy.gameObject);
                }
            }

        });
    }

    public override void Deactivate(string EffectStr)
    {
        throw new System.NotImplementedException();
    }
}

public class ShootOneWhenOneLeft : TraitCustomScript
{
    public override void Activate(string EffectStr, TraitCardObject CardObject)
    {
        int EveryShootTimes = int.Parse(EffectStr);
        var gunController = FindObjectOfType<GunController>();
        gunController.onDiceShootDelList.Add(delegate (DiceController diceController)
        {
            if (gunController.diceQueue.Count == 0)
            {
                diceController.FinalState = Random.value > 0.5f ? 1 : 6;
            }

        });
    }

    public override void Deactivate(string EffectStr)
    {
        throw new System.NotImplementedException();
    }
}
