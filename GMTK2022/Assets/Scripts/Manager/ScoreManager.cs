using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理全场的骰子
/// </summary>
public class ScoreManager : Singleton<ScoreManager>
{
    private int totalScore = 0;

    private AbstractRule rulesChain = BuildRulesChain();

    public int Score
    {
        get { return totalScore; }
    }

    /// <summary>
    /// 检查场上骰子阵型并算分，骰子落地和销毁都要触发
    /// </summary>
    public void CheckDiceInScene()
    {
        GameObject[] objArrayInScene = GameObject.FindGameObjectsWithTag("Dice");

        // 规则校验
        List<DiceController>[] diceStateArray = InitDiceCollection();
        foreach (var obj in objArrayInScene)
        {
            DiceController dice = obj.GetComponent<DiceController>();
            if (dice != null)
            {
                diceStateArray[dice.State].Add(dice);
            }
        }
        AbstractRule checkResult = rulesChain.CheckRule(diceStateArray);

        // 计算总分
        totalScore += checkResult.GetScore();
        // 回收骰子
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GunController gun = player.GetComponent<GunController>();
        gun.RecycleDiceBatch(checkResult.diceIDList);

        // 责任链结果清空
        rulesChain.Clear();
    }

    private List<DiceController>[] InitDiceCollection()
    {
        List<DiceController>[] diceCollection = new List<DiceController>[6];
        for (int i = 0; i < 6; i++)
        {
            diceCollection[i] = new List<DiceController>();
        }
        return diceCollection;
    }

    // 构建校验责任链
    private static AbstractRule BuildRulesChain()
    {
        AbstractRule straightDrawRule = new StraightDrawRule();
        AbstractRule flushDrawRule = new FlushDrawRule();
        AbstractRule doublePairsRule = new DoublePairsRule();

        straightDrawRule.SetNextRule(flushDrawRule);
        flushDrawRule.SetNextRule(doublePairsRule);

        return straightDrawRule;
    }
}

/// <summary>
/// 规则校验责任链父类
/// </summary>
public abstract class AbstractRule
{
    protected AbstractRule nextRule;

    public List<DiceController> diceIDList;

    public void SetNextRule(AbstractRule nextRule)
    {
        this.nextRule = nextRule;
    }

    /// <summary>
    /// 责任链调用入口
    /// </summary>
    /// <param name="diceStateArray">当前场上骰子状况</param>
    /// <returns>规则结果</returns>
    public AbstractRule CheckRule(List<DiceController>[] diceStateArray) {
        diceIDList = CheckSelfRule(diceStateArray);
        if (diceIDList.Count != 0 || nextRule == null)
        {
            return this;
        }
        else
        {
            return nextRule.CheckRule(diceStateArray);
        }
    }

    /// <summary>
    /// 责任链结果清空，避免内存泄漏
    /// </summary>
    public void Clear()
    {
        this.diceIDList = null;
        this.nextRule.Clear();
    }

    /// <summary>
    /// 检查具体规则
    /// </summary>
    /// <param name="diceStateArray">当前场上骰子状况</param>
    /// <returns>符合规则的骰子ID</returns>
    public abstract List<DiceController> CheckSelfRule(List<DiceController>[] diceStateArray);

    // 返回分数
    public abstract int GetScore();
}

/// <summary>
/// 校验顺子规则
/// </summary>
public class StraightDrawRule : AbstractRule
{
    public override int GetScore()
    {
        if (this.diceIDList.Count < 4)
        {
            return 0;
        }
        else if (this.diceIDList.Count == 4)
        {
            return 30;
        }
        else if (this.diceIDList.Count == 5)
        {
            return 40;
        }
        else
        {
            return 50;
        }
    }

    public override List<DiceController> CheckSelfRule(List<DiceController>[] diceStateArray)
    {
        List<DiceController> res = new List<DiceController>();

        foreach (var diceState in diceStateArray)
        {
            if (diceState.Count != 0)
            {
                res.Add(diceState[0]);
            }
            else if (res.Count < 4)
            {
                res.Clear();
            }
            else
            {
                break;
            }
        }

        return res;
    }
}

/// <summary>
/// 校验同花规则
/// </summary>
public class FlushDrawRule : AbstractRule
{
    public override int GetScore()
    {
        if (this.diceIDList.Count < 3)
        {
            return 0;
        }
        else if (this.diceIDList.Count == 3)
        {
            return 30;
        }
        else if (this.diceIDList.Count == 4)
        {
            return 40;
        }
        else if (this.diceIDList.Count == 5)
        {
            return 50;
        }
        else
        {
            return 60;
        }
    }

    public override List<DiceController> CheckSelfRule(List<DiceController>[] diceStateArray)
    {
        int maxLen = 0;
        int maxLenIdx = -1;
        for (int i = 0; i < diceStateArray.Length; i++)
        {
            if (diceStateArray[i].Count > maxLen)
            {
                maxLen = diceStateArray[i].Count;
                maxLenIdx = i;
            }
        }

        if (maxLen >= 3)
        {
            return diceStateArray[maxLenIdx];
        }

        return new List<DiceController>();
    }
}

/// <summary>
/// 校验两对规则
/// </summary>
public class DoublePairsRule : AbstractRule
{
    public override int GetScore()
    {
        if (this.diceIDList.Count == 0)
        {
            return 0;
        }
        else
        {
            return 20;
        }
    }

    public override List<DiceController> CheckSelfRule(List<DiceController>[] diceStateArray)
    {
        List<DiceController> res = new List<DiceController>();
        foreach (var diceState in diceStateArray)
        {
            if (diceState.Count == 2)
            {
                res.AddRange(diceState);
            }

            if (res.Count >= 4)
            {
                break;
            }
        }

        return res;
    }
}
