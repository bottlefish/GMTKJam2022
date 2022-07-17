using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]public GameObject enemy;
    public float timer=3f;

    private float clock=1f;

    public float numInWave=4f;

    public float spawnRange=6f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        clock-=Time.deltaTime;
        if(clock<=0)
        {
            SpawnEnemy();
            clock=timer;
            

        }
    }
    void SpawnEnemy()
    {
        for(int i=0;i<numInWave;i++)
        {
            Vector3 temp=Random.insideUnitCircle*spawnRange;
            Instantiate(enemy,temp+transform.position,Quaternion.identity);
        }

    }
}
