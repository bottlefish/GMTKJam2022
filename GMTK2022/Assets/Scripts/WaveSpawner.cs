using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public enum SpawnState { SPAWNING, WAITING, COUNTING};

    [System.Serializable]
    public class Wave
    {
        //public string name;
        public Transform[] enemy;
        public int count;
        public float rate;
    }
    public Wave[] waves;
    int nextWave = 0;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    public float spawnPointDistanceToPlayer=2f;
    float waveCountDown;

    float searchCountDown = 1f;

    private List<Transform> spawnPoint=new List<Transform>();

    SpawnState state = SpawnState.COUNTING;



    void Start()
    {
        waveCountDown = timeBetweenWaves;

        spawnPoint.Clear();
        if (spawnPoints.Length == 0)
        {
           
        }
    }

    void Update ()
    {

        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountDown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine( SpawnWave ( waves[nextWave] ) );
            }
        }
        else 
        {
            waveCountDown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed");

        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            // Implemnt some sorta multiplier here to make it harder over time
            nextWave = 0;
        }
        else
        {
            nextWave++;
        }

        

    }

    bool EnemyIsAlive()
    {
        searchCountDown -= Time.deltaTime;
        if (searchCountDown <= 0)
        {
            searchCountDown = 1f;   
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
             return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave (Wave _wave)
    {
        //Debug.Log("Spawn Wave:" + _wave.name);
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy[i]); 
            yield return new WaitForSeconds( 1f/_wave.rate); 
        }

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy (Transform _enemy)
    {
        Debug.Log("Spawning Enemy: " + _enemy.name);


       

        foreach(Transform point in spawnPoints)
        {
            if(CheckIfPlayerNearby(point))
            {
                spawnPoint.Add(point);
            }

        }
        Transform _sp= spawnPoints[ Random.Range (0, spawnPoint.Count) ];;
        if(spawnPoint.Count!=0)
        {
            _sp = spawnPoint[ Random.Range (0, spawnPoint.Count) ];
        }
         
        Instantiate (_enemy, _sp.position, _sp.rotation);
        spawnPoint.Clear();
    }
    bool CheckIfPlayerNearby(Transform checkPoint)
    {
        if ((Vector3.Distance(transform.position, checkPoint.transform.position) >=spawnPointDistanceToPlayer))
         {
            Debug.Log("玩家距离出生点的位置"+Vector3.Distance(transform.position, checkPoint.transform.position));
            return true;
         }
         else
         {
            return false;
         }

    }
}
