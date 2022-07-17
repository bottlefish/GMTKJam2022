using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public bool isFiring;
    public float ForceAmount;
    public DiceController dice;

    public float timeBetweenShots;
    public float shotCounter;
    public int diceBoxSize = 6;

    private Queue<int> diceQueue;

    public Transform firePoint;

  
    // Start is called before the first frame update
    void Start()
    {
        InitDiceQueue();
    }

    // 初始化骰子队列，随机
    private void InitDiceQueue()
    {
        diceQueue = new Queue<int>();
        for (int i = 0; i < diceBoxSize; i++)
        {
            diceQueue.Enqueue(Random.Range(1, 7));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (diceQueue.Count == 0)
            {
                // 手上没有骰子了
                Debug.Log("手上没有骰子！");
                return;
            }
            else
            {
                // 手上有骰子，发射并从骰子弹匣出栈
                ShootDice(diceQueue.Dequeue());
                //添加没有骰子的反馈
            }
        }
    }

    private void ShootDice(int diceState)
    {
        Debug.Log("点数：" + diceState);
        DiceController newDice = Instantiate(dice, firePoint.position, Quaternion.identity) as DiceController;
        // 为新骰子设置点数
        newDice.InitDiceState(diceState);
        newDice.GetComponent<Rigidbody>().AddForce(transform.forward * ForceAmount, ForceMode.Impulse);
        newDice.GetComponent<Rigidbody>().freezeRotation = true;
    }

    private void OnTriggerEnter(Collider other) {
        DiceController[] colliderDiceArray = other.GetComponents<DiceController>();
        
        if (colliderDiceArray.Length != 0)
        {
            foreach (var colliderDice in colliderDiceArray)
            {
                RecycleDice(colliderDice);
            }
        }
    }

    // 回收骰子装填弹匣
    public void RecycleDice(DiceController colliderDice)
    {
        if (diceQueue.Count == diceBoxSize)
        {
            Debug.Log("骰子已满");
            return;
        }
        else
        {
            // 骰子进入弹匣并销毁骰子
            diceQueue.Enqueue(colliderDice.State);
            colliderDice.haveDiced=false;
            colliderDice.canDestoryEnemy=true;
            colliderDice.DestroySelf();
        }
    }
}
