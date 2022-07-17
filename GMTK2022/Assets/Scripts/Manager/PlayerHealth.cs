using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    private int healthMax=3;

	public int health;

    

     public Image[] healthUI1;


	void Start () {
		health = healthMax;
	}

	public void UpdateHealth(){
		health -= 1;
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
