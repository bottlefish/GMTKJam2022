using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DiceController : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    public float ForceAmount = 10000f;
    public float TorqueAmount = 20f;

    public float randomRadius = 6f;

    public Transform xMin;
    public Transform xMax;
    public Transform zMin;

    public Transform zMax;


    public bool canDestoryEnemy = true;

    private GunController gun;

    private Rigidbody rb;

    /*public enum State{
        one,
        two,
        three,
        four,
        five,
        six,
    }*/

    //public State state;

    private int state = 5;

    public int State
    {
        get
        {
            return state;
        }
    }
    public bool haveDiced = false;

    //
    void Start()
    {
        xMin = GameObject.Find("xMin").transform;
        xMax = GameObject.Find("xMax").transform;
        zMax = GameObject.Find("zMax").transform;
        zMin = GameObject.Find("zMin").transform;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        gun = player.GetComponent<GunController>();

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

    }

    /*void RandomDice()
    {
         int num=Random.Range(1,6);
        if(num==1)
        {
            state=State.one;
        }
        if(num==2)
        {
            state=State.two;
        }
        if(num==3)
        {
            state=State.three;
        }
        if(num==4)
        {
            state=State.four;
        }
        if(num==5)
        {
            state=State.five;
        }
        if(num==6)
        {
            state=State.six;
        }
        else
        {
            state=State.one;
        }

    }*/

    void SetDiceFace()
    {
        if (state == 1)
        {
            transform.eulerAngles = new Vector3(0f, 0, 0f);
        }
        if (state == 2)
        {
            // transform.DORotate(new Vector3(0,0,90),0.1f);
            transform.eulerAngles = new Vector3(0f, 0, 90f);
        }
        if (state == 3)
        {
            //transform.DORotate(new Vector3(180,0,0),0.1f);
            transform.eulerAngles = new Vector3(270f, 0, 0f);
        }
        if (state == 4)
        {
            //transform.DORotate(new Vector3(90,0,0),0.1f);
            transform.eulerAngles = new Vector3(90f, 0, 0f);
        }
        if (state == 5)
        {
            //transform.DORotate(new Vector3(0,0,270),0.1f);
            transform.eulerAngles = new Vector3(0, 0, 270f);
        }
        if (state == 6)
        {
            //transform.DORotate(new Vector3(180,0,0),0.1f);
            transform.eulerAngles = new Vector3(180, 0, 0);
        }
        else
        {
            //transform.DORotate(Vector3.zero,0.1f);
        }
        Debug.Log(state);
    }

    private void RandomDiceFace()
    {
        state = Random.Range(1, 7);
        SetDiceFace();
    }
    private Vector3 RandomPointOnCircleEdge(float radius)
    {
        var vector2 = Random.insideUnitCircle.normalized * radius;
        return new Vector3(vector2.x, 0, vector2.y);
    }

    void DoDiceRoll()
    {
        haveDiced = true;
        transform.GetComponent<Rigidbody>().freezeRotation = false;
        //transform.DOJump(new Vector3(transform.position.x,transform.position.y+5,transform.position.z),2,1,0.5f,true);
        Vector3 tempPos = transform.position;

        //transform.DOMove(new Vector3(Random.Range(-2f, 2f)+tempPos.x,tempPos.y,tempPos.z+Random.Range(-2f, 2f)),0.6f);

        //transform.DOLocalMoveX(tempPos.x + Random.Range(-2f, 2f), 0.6f).SetEase(Ease.OutBack);
        //transform.DOLocalMoveZ(tempPos.z + Random.Range(-2f, 2f), 0.6f).SetEase(Ease.OutBack);

        Vector3 target = RandomPointOnCircleEdge(10) + transform.position;

        if (target.x < xMin.transform.position.x)
        {
            target.x = xMin.transform.position.x;
        }
        if (target.x > xMax.transform.position.x)
        {
            target.x = xMax.transform.position.x;
        }
        if (target.z < zMin.transform.position.z)
        {
            target.z = zMin.transform.position.z;
        }
        if (target.z > zMax.transform.position.z)
        {
            target.z = xMax.transform.position.z;
        }


        transform.DOMove(target, 0.4f);
        transform.DORotate(new Vector3(30, 30, 30), 0.02f).SetLoops(24, LoopType.Incremental).OnComplete(() =>
        {
            RandomDiceFace();
            transform.GetComponent<Rigidbody>().freezeRotation = true;
        });
        //transform.GetComponent<Rigidbody>().AddTorque(transform.up*TorqueAmount,ForceMode.Impulse);
        //transform.GetComponent<Rigidbody>().AddForce(new Vector3(0,1,0)*ForceAmount);

        transform.DOLocalMoveY(5, 0.6f).SetEase(Ease.OutBack).OnComplete(() =>
            {

                transform.DOLocalMoveY(0, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                {

                    transform.GetComponent<Rigidbody>().isKinematic = true;
                    canDestoryEnemy = !canDestoryEnemy;
                    ScoreManager.Instance.CheckDiceInScene();
                });
            }
        );

    }

    private void OnCollisionEnter(Collision other)
    {

        if (other.transform.tag == "Enemy")
        {
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (canDestoryEnemy)
            {
                Destroy(other.gameObject);

            }


            if (!haveDiced)
            {
                DoDiceRoll();
            }
        }
        if (other.transform.tag == "Wall")
        {

            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (!haveDiced)
            {
                haveDiced = true;
                Vector3 tempPos = transform.position;
                transform.GetComponent<Rigidbody>().freezeRotation = false;
                transform.DOLocalMove(other.transform.forward * 2 + tempPos, 0.5f);
                transform.DORotate(new Vector3(30, 30, 30), 0.02f).SetLoops(24, LoopType.Incremental).OnComplete(() =>
                {
                    RandomDiceFace();
                    transform.GetComponent<Rigidbody>().freezeRotation = true;
                });
                transform.DOLocalMoveY(5, 0.6f).SetEase(Ease.OutBack).OnComplete(() =>
                    {

                        transform.DOLocalMoveY(0, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                        {

                            transform.GetComponent<Rigidbody>().isKinematic = true;
                            canDestoryEnemy = !canDestoryEnemy;
                        });
                    }
                );
            }
        }
        if (other.transform.tag == "Dice")
        {

            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (!haveDiced)
            {
                DoDiceRoll();
            }
            other.transform.GetComponent<Rigidbody>().isKinematic = false;
            other.transform.GetComponent<Rigidbody>().AddForce((other.transform.position - transform.position).normalized * 8f, ForceMode.Impulse);
            //transform.DOMove(transform.position,1f).OnComplete(() => { other.transform.GetComponent<Rigidbody>().isKinematic=true;});       
        }
    }

    /// <summary>
    /// 初始化骰子状态并旋转
    /// </summary>
    /// <param name="diceState">骰子状态</param>
    public void InitDiceState(int diceState)
    {
        state = diceState;
        SetDiceFace();
    }

    /// <summary>
    /// 销毁自身
    /// </summary>
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 移到展示槽中
    /// </summary>
    /// <param name="position">展示槽位置</param>
    public void MoveToShowSlot(Vector3 position)
    {
        transform.DOMove(position, 0.4f).OnComplete(() =>
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            StartCoroutine(DelayRecycle(2f));
        });
    }
    private IEnumerator DelayRecycle(float duration)
    {
        //transition.SetTrigger("FadeOut");
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        gun.RecycleDice(this);
    }
}
