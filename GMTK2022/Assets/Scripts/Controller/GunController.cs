using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using TMPro;

public class GunController : MonoBehaviour
{
    public float ShootCD = 1.5f;
    private float LastShootTime = -100;
    public bool isFiring;
    public float ForceAmount;
    public DiceController dice;

    public float timeBetweenShots;
    public float shotCounter;
    public int diceBoxSize = 6;

    public Queue<int> diceQueue;

    public Transform firePoint;

    public AudioClip noAmmoSound;

    public AudioClip[] shootShound;
    public VisualEffect gundust;

    public AudioClip[] pickSound;

    public TMP_Text text;

    public delegate void OnMouseRightButtonDelegate();
    public OnMouseRightButtonDelegate OnMouseRightClickDel;

    public delegate void OnDiceShooted(DiceController diceController);
    public List<OnDiceShooted> onDiceShootDelList;

    // Start is called before the first frame update
    private void Awake()
    {
        onDiceShootDelList = new List<OnDiceShooted>();
    }

    void Start()
    {
        InitDiceQueue();
        text.text = diceQueue.Count.ToString();
    }

    public void ChangeDiceNum(int Delta)
    {
        diceBoxSize += Delta;
        if (Delta > 0)
        {
            for (int i = 0; i < Delta; i++)
                diceQueue.Enqueue(Random.Range(1, 7));
        }
        else if (Delta < 0)
        {
            for (int i = 0; i < Delta; i++)
            {
                if (diceQueue.Count <= 0) break;
                diceQueue.Dequeue();
            }
        }
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

    //TODO：仅用于表现，实际射击时未调用这个检查
    public bool CanShoot()
    {
        return diceQueue.Count > 0
                && Time.time - LastShootTime > ShootCD
                && !TraitManager.Instance.IsShopping;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryShootDice();
        }
        if (Input.GetMouseButtonDown(1))
        {
            OnMouseRightClickDel.Invoke();
        }
        text.text = diceQueue.Count.ToString();

    }

    public bool TryShootDice(int overrideState = -1)
    {
        if (Time.time - LastShootTime > ShootCD && !TraitManager.Instance.IsShopping)
        {
            if (diceQueue.Count == 0)
            {
                AudioManager.Instance.playsound(noAmmoSound);
                // 手上没有骰子了
                Debug.Log("手上没有骰子！");
                return false;
            }
            else
            {
                // 手上有骰子，发射并从骰子弹匣出栈
                // 但如果传入了override state，则使用 override state；
                LastShootTime = Time.time;
                int ShootState = diceQueue.Dequeue();
                var Dice = ShootDice(ShootState);
                if (overrideState > 0)
                    Dice.FinalState = overrideState;
                foreach (var Del in onDiceShootDelList)
                    Del.Invoke(Dice);
                return true;
            }
        }

        return false;
    }

    private DiceController ShootDice(int diceState)
    {
        CameraShake.Shake(0.1f, 0.4f);
        Instantiate(gundust, firePoint.position, Quaternion.identity);
        gundust.Play();
        AudioManager.Instance.playsound(shootShound[Random.Range(0, shootShound.Length)]);
        Debug.Log("点数：" + diceState);
        DiceController newDice = Instantiate(dice, firePoint.position, Quaternion.identity) as DiceController;
        // 为新骰子设置点数
        newDice.InitDiceState(diceState);
        newDice.GetComponent<Rigidbody>().AddForce(transform.forward * ForceAmount, ForceMode.Impulse);
        newDice.GetComponent<Rigidbody>().freezeRotation = true;
        return newDice;
    }

    private void OnTriggerEnter(Collider other)
    {
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
            AudioManager.Instance.Diceplaysound(pickSound[Random.Range(0, pickSound.Length)]);
            // 骰子进入弹匣并销毁骰子
            diceQueue.Enqueue(colliderDice.State);
            colliderDice.haveDiced = false;
            colliderDice.canDestoryEnemy = true;
            colliderDice.DestroySelf();
        }
    }
}
