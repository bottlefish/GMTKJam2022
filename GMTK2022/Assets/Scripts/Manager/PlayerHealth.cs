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


    void Start()
    {
        health = healthMax;
        damagedEffect.enabled = false;
    }

    public void RestoreLife(int Delta)
    {
        health = Mathf.Clamp(health + Delta, health, healthMax);
        restoredEffect.DOFade(0.65f, 0.5f).SetEase(Ease.Flash).SetLoops(2, LoopType.Yoyo).OnComplete(() => { restoredEffect.enabled = false; });
        // TODO 回血屏幕特效？
        UpdateHealthUI();
    }

    public void TakeDamage()
    {
        //@Snoww：冲刺中不受伤害
        if (GetComponent<PlayerMovement>().isDashing) return;
        health -= 1;
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
                healthUI1[i].enabled = true;
            }
            else
            {
                healthUI1[i].enabled = false;

            }
        }
    }


}
