using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AddMaxHP : TraitCustomScript
{
    public override void Activate(string EffectStr, TraitCardObject CardObject)
    {
        int Delta = int.Parse(EffectStr);
        FindObjectOfType<PlayerHealth>().ChangeMaxHealth(Delta);
    }

    public override void Deactivate(string EffectStr)
    {
        throw new System.NotImplementedException();
    }
}

public class InvalidDamage : TraitCustomScript
{
    public override void Activate(string EffectStr, TraitCardObject CardObject)
    {
        int CurrentNum = TraitManager.Instance.AllOwnedTraits[CardObject.UniqueID].Count;
        float InvalidProbablity = 0.2f + CurrentNum * 0.15f;
        FindObjectOfType<PlayerHealth>().OnPreDamageDelList.Add(
            delegate (int Delta) { return Random.value > 0.2 ? Delta : Delta - 1; });
    }

    public override void Deactivate(string EffectStr)
    {
        throw new System.NotImplementedException();
    }
}


public class ChangeHP : TraitCustomScript
{
    public override void Activate(string EffectStr, TraitCardObject CardObject)
    {
        int Delta = int.Parse(EffectStr);
        if (Delta > 0)
            FindObjectOfType<PlayerHealth>().RestoreLife(Delta);
        else
            FindObjectOfType<PlayerHealth>().TakeDamage(-Delta);
    }

    public override void Deactivate(string EffectStr)
    {
        throw new System.NotImplementedException();
    }
}

public class ReduceDashCD : TraitCustomScript
{
    public override void Activate(string EffectStr, TraitCardObject CardObject)
    {
        int CurrentNum = TraitManager.Instance.AllOwnedTraits[CardObject.UniqueID].Count;
        float CDToReduce = CurrentNum == 0 ? 0.2f : 0.1f;
        FindObjectOfType<PlayerMovement>().DashCD *= (1 - CDToReduce);

    }

    public override void Deactivate(string EffectStr)
    {

    }
}

public class ReduceShootCDWhenHurt : TraitCustomScript
{
    Coroutine EndEffectCoroutine;
    float InitCD;
    private void Start()
    {
        InitCD = FindObjectOfType<GunController>().ShootCD;
    }

    public override void Activate(string EffectStr, TraitCardObject CardObject)
    {
        var playerHealth = FindObjectOfType<PlayerHealth>();
        playerHealth.OnPreDamageDelList.Add(delegate (int Delta)
        {
            if (EndEffectCoroutine != null)
            {
                StopCoroutine(EndEffectCoroutine);
            }
            FindObjectOfType<GunController>().ShootCD = 0.7f * InitCD;
            EndEffectCoroutine = StartCoroutine(IEndEffect());
            return Delta;
        });

    }
    IEnumerator IEndEffect()
    {
        yield return new WaitForSeconds(5);
        FindObjectOfType<GunController>().ShootCD = InitCD;
    }

    public override void Deactivate(string EffectStr)
    {

    }
}

public class AddDiceNum : TraitCustomScript
{
    public override void Activate(string EffectStr, TraitCardObject CardObject)
    {
        int Delta = int.Parse(EffectStr);
        FindObjectOfType<GunController>().ChangeDiceNum(Delta);
    }

    public override void Deactivate(string EffectStr)
    {
        throw new System.NotImplementedException();
    }
}

public class AddShopCards : TraitCustomScript
{
    public override void Activate(string EffectStr, TraitCardObject CardObject)
    {
        int Delta = int.Parse(EffectStr);

        TraitManager.Instance.CardsInShopEveryTime += Delta;
    }

    public override void Deactivate(string EffectStr)
    {
        throw new System.NotImplementedException();
    }
}

public class ChangeGroundMaterialIntensity : TraitCustomScript
{
    public override void Activate(string EffectStr, TraitCardObject CardObject)
    {
        int Level = int.Parse(EffectStr);
        Material GroundMat = SceneArtManager.Instance.GroundMatInst;
        var MyTween = DOTween.To(() => { return GroundMat.GetColor("LitColor"); },
            (_color) => { GroundMat.SetColor("LitColor", _color); },
            SceneArtManager.Instance.InitGroundColor * (Level + 1),
            2.2f);
        MyTween.Play();

    }

    public override void Deactivate(string EffectStr)
    {
        throw new System.NotImplementedException();
    }
}


public class ChangeVelocity : TraitCustomScript
{
    public override void Activate(string EffectStr, TraitCardObject CardObject)
    {
        float Ratio = float.Parse(EffectStr);
        FindObjectOfType<PlayerMovement>().PlayerSpeed *= Ratio;
    }

    public override void Deactivate(string EffectStr)
    {
        throw new System.NotImplementedException();
    }
}

public class ChangeMoneyToAdd : TraitCustomScript
{
    public override void Activate(string EffectStr, TraitCardObject CardObject)
    {
        int Delta = int.Parse(EffectStr);
        FindObjectOfType<WaveSpawner>().ScoreToAddWhenWaveFinished += Delta;
    }

    public override void Deactivate(string EffectStr)
    {
        throw new System.NotImplementedException();
    }
}
