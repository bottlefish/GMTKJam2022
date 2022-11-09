using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    private int healthMax = 3;

    public int health;

    public Image damagedEffect;
    public Image restoredEffect;

    public Image[] healthUI1;
    public AudioClip[] behited;

    public delegate int OnPreDamageDelegate(int f);
    public List<OnPreDamageDelegate> OnPreDamageDelList;


    void Start()
    {
        OnPreDamageDelList = new List<OnPreDamageDelegate>();
        health = healthMax;
        damagedEffect.enabled = false;
        UpdateHealthUI();
    }

    public void ChangeMaxHealth(int Delta)
    {
        healthMax += Delta;
        UpdateHealthUI();
    }

    public void RestoreLife(int Delta)
    {
        health = Mathf.Clamp(health + Delta, health, healthMax);
        restoredEffect.enabled = true;
        restoredEffect.color = new Color(restoredEffect.color.r, restoredEffect.color.g, restoredEffect.color.b, 0);
        restoredEffect.DOFade(0.65f, 0.5f).SetEase(Ease.Flash).SetLoops(2, LoopType.Yoyo).OnComplete(() => { restoredEffect.enabled = false; });
        // TODO ��Ѫ��Ļ��Ч��
        UpdateHealthUI();
    }

    public void TakeDamage(int Damage = 1)
    {
        //@Snoww������в����˺�
        if (GetComponent<PlayerMovement>().isDashing) return;

        // OnPreDamage
        foreach (var Del in OnPreDamageDelList)
        {
            Damage = Del(Damage);
        }
        //TODO�����ֲ㣬��һ��Miss��
        if (Damage == 0) return;

        health -= Damage;
        AudioManager.Instance.playsound(behited[0]);
        AudioManager.Instance.playsound2(behited[1]);
        damagedEffect.enabled = true;
        CameraShake.Shake(0.2f, 0.5f);
        damagedEffect.DOFade(0.65f, 0.2f).SetEase(Ease.InOutCubic).SetLoops(2, LoopType.Yoyo).OnComplete(() => { damagedEffect.enabled = false; });
        if (health <= 0)
        {
            health = 0;
            GameManager.Instance.TriggerGameOver();
        }
        UpdateHealthUI();

    }

    public void UpdateHealthUI()
    {
        for (int i = 0; i < healthUI1.Length; i++)
        {
            if (i < health)
            {
                healthUI1[i].color = Color.white;
            }
            else
            {
                healthUI1[i].color = new Color(0.3f, 0.3f, 0.3f, 1);
            }
            if (i < healthMax)
            {
                healthUI1[i].enabled = true;
            }
            else
            {
                healthUI1[i].enabled = false;
            }
        }
    }


}
