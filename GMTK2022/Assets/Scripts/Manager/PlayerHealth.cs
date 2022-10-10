using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    private int healthMax=3;

	public int health;

    public Image damagedEffect;

     public Image[] healthUI1;
     public AudioClip[] behited;


	void Start () {
		health = healthMax;
        damagedEffect.enabled=false;
	}

	public void UpdateHealth(){
        //@Snoww£∫≥Â¥Ã÷–≤ª ‹…À∫¶
        if (GetComponent<PlayerMovement>().isDashing) return;
		health -= 1;
        AudioManager.Instance.playsound(behited[0]);
        AudioManager.Instance.playsound2(behited[1]);
        damagedEffect.enabled=true;
        CameraShake.Shake(0.2f,0.5f);
        damagedEffect.DOFade(0.65f,0.1f).SetEase(Ease.Flash).SetLoops(2,LoopType.Yoyo).OnComplete(()=>{damagedEffect.enabled=false ;});
		if (health <= 0){
			health = 0;
			GameManager.Instance.TriggerGameOver();
		}
        for(int i=0;i<healthUI1.Length;i++)
        {
            if(i<health)
            {
                healthUI1[i].enabled=true;
            }
            else
            {
                healthUI1[i].enabled=false;

            }
        }
    
		
	}

    
}
