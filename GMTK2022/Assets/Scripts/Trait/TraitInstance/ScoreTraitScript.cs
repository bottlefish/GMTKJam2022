using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateFlushDrawRule : TraitCustomScript
{
    public override void Activate(string EffectStr, TraitCardObject CardObject)
    {
        int NewVal = int.Parse(EffectStr);
        ScoreManager.Instance.UpdateFlushDrawRule(NewVal);
    }

    public override void Deactivate(string EffectStr)
    {
        throw new System.NotImplementedException();
    }
}

public class AddBeastRule : TraitCustomScript
{
    public override void Activate(string EffectStr, TraitCardObject CardObject)
    {
        ScoreManager.Instance.AddRuleToChain(new BeastRule(), true);
    }

    public override void Deactivate(string EffectStr)
    {
        throw new System.NotImplementedException();
    }
}

public class AddDrawbackRule : TraitCustomScript
{
    public override void Activate(string EffectStr, TraitCardObject CardObject)
    {
        ScoreManager.Instance.AddRuleToChain(new DrawbackRule(), true);
    }

    public override void Deactivate(string EffectStr)
    {
        throw new System.NotImplementedException();
    }
}