
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class EnemyAiTutorial : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    //Patroling
    public Vector3 walkPoint;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    private float timeUpdatePlayerPos=0.02f;

    private Vector3 target;

    

    public enum AItype
    {
        follow,
        predict
    }

    public AItype aiType;


   


    //States
    private float attackRange=1f;
    //public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("PlayerObj").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start() {
        float distance= Vector3.Distance(transform.position,player.transform.position);
        float time=distance/agent.speed;
        target=player.GetComponent<Rigidbody>().velocity*player.GetComponent<PlayerMovement>().PlayerSpeed;
    }

    private void Update()
    {   if(!isInAttackRange())
        {
            ChasePlayer();
        }
        else
        {
            AttackPlayer();

        }
 
        //if (!playerInAttackRange) ChasePlayer();
        ChasePlayer();
        //if (playerInAttackRange) AttackPlayer();
    }

    bool isInAttackRange()
    {
        float distance= Vector3.Distance(transform.position,player.transform.position);
        if(distance<=attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }

        
    }
    private void ChasePlayer()
    {
        if(aiType==AItype.follow)
        {
            agent.SetDestination(player.position);
        }
        if(aiType==AItype.predict)
        {
            timeUpdatePlayerPos-=Time.deltaTime;
            if(timeUpdatePlayerPos<=0)
            {
            
            timeUpdatePlayerPos=1;

            Debug.Log(target+""+transform.position+""+player.transform.position);

            } 
            float distance= Vector3.Distance(transform.position,player.transform.position);
            float time=distance/agent.speed;
            target= new Vector3( player.GetComponent<PlayerMovement>().movement.x,0, player.GetComponent<PlayerMovement>().movement.y)*5f +player.transform.position;          
            agent.SetDestination(new Vector3(target.x,transform.position.y,target.z));
        }
        
    }

    private void AttackPlayer()
    {
        transform.DOMove(player.transform.position,0.3f);
        transform.DOLocalMoveY(3, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                    {                       
                        transform.DOMove(player.transform.position,0.1f);
                        
                    });
                 
        //Make sure enemy doesn't move
        /*agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }*/
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
       // Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.transform.tag=="Player")
        {
            Destroy(this.gameObject);
            other.transform.GetComponent<PlayerHealth>().UpdateHealth();

        }
    }
}
