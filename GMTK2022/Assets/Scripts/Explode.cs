using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Explode : MonoBehaviour
{
    VisualEffect enemyDie;
    static DiceController AnyDice;
    public float Duration = 0.7f;
    // Start is called before the first frame update
    void Start()
    {
        GetEnemyDieVFX();
        Destroy(gameObject, Duration);
    }

    void GetEnemyDieVFX()
    {
        if (enemyDie) return;
        if (!AnyDice) AnyDice = FindObjectOfType<DiceController>();
        if (AnyDice) enemyDie = AnyDice.enemyDie;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //@Snoww @TODO: 从DiceController复制来的，应该抽到Enemy里
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            GetEnemyDieVFX();
            Destroy(other.gameObject);
            Instantiate(enemyDie, other.gameObject.transform.position, Quaternion.identity);
            enemyDie.Play();

        }
    }
}
