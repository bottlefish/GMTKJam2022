using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TypeReferences;

[CreateAssetMenu(fileName = "Trait", menuName = "Trait/TraitCardObject", order = 1)]
public class TraitCardObject : ScriptableObject
{
    [System.Serializable]
    public struct ActivateEffectStruct
    {
        [Inherits(typeof(TraitCustomScript))]
        public TypeReference ActivateEffectScript;
        public string EffectStr;
    }
    
    public int UniqueID;
    public string Name;
    public string Description;
    public int Price = 35;
    public Texture2D UITexture;
    public List<int> PrerequisitesID;
    public List<int> PrerequisitesNotID;
    public int MaxNum = 1;
    public float Probability = 1;
    public ActivateEffectStruct[] ActivateEffects;
}
