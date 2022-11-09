using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class TraitCustomScript : MonoBehaviour
{

    public abstract void Activate(string EffectStr, TraitCardObject CardObject);
    public abstract void Deactivate(string EffectStr);


}
