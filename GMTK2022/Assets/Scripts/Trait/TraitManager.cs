using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class TraitManager : Singleton<TraitManager>
{
    // 用于配置所有的Trait
    public List<TraitCardObject> AllTraitCards;
    // 用于根据ID检索TraitCardObject
    private Dictionary<int, TraitCardObject> TraitDict;
    public int CardsInShopEveryTime = 4;
    public GameObject ShoppingUI;
    public MyButton ShoppingEndButton;
    public GameObject CardContent;
    public GameObject CardButtonPrefab;

    public Dictionary<int, List<GameObject>> AllOwnedTraits;

    private void Start()
    {
        EndShopping();
        TraitDict = new Dictionary<int, TraitCardObject>();
        AllOwnedTraits = new Dictionary<int, List<GameObject>>();
        foreach (var CardObject in AllTraitCards)
        {
            if (!TraitDict.TryAdd(CardObject.UniqueID, CardObject))
            {
                Debug.LogError("有卡牌使用了相同的UniqueID，或者卡牌在TraitManager上配了两次。");
                Debug.Break();
            }
            if (!AllOwnedTraits.TryAdd(CardObject.UniqueID, new List<GameObject>()))
            {
                Debug.LogError("Initialize AllOwnedTraits Failed.");
                Debug.Break();
            }

        }
        //AddTrait(1017);
    }

    public TraitCardObject GetTraitCardObjectByID(int ID)
    {
        if (!TraitDict.TryGetValue(ID, out TraitCardObject CardObject))
            return null;
        else return CardObject;
    }

    public void AddTrait(int ID)
    {
        //TODO 这个不优雅
        if (!IsShopping) return;

        var CardObject = GetTraitCardObjectByID(ID);
        GameObject TmpObject = new GameObject(CardObject.Name);
        var TraitObject = Instantiate(TmpObject, gameObject.transform);
        if (AllOwnedTraits.TryGetValue(ID, out var ListOfIDObjects))
        {
            ListOfIDObjects.Add(TraitObject);
        }
        else
        {
            var TmpList = new List<GameObject>();
            TmpList.Add(TraitObject);
            AllOwnedTraits.Add(ID, TmpList);
        }
        foreach (var ActivateEffect in CardObject.ActivateEffects)
        {
            if (ActivateEffect.ActivateEffectScript == null) continue;
            var CustomScript = TmpObject.AddComponent(ActivateEffect.ActivateEffectScript.Type);
            (CustomScript as TraitCustomScript).Activate(ActivateEffect.EffectStr, CardObject);
        }
    }


    // 商店UI部分。
    [HideInInspector]
    public bool IsShopping = false;
    public void StartShopping()
    {
        StartCoroutine(StartShoppingCorotinue());
    }

    bool GetIsCardAvailable(TraitCardObject Card)
    {
        int CurrentNum = AllOwnedTraits[Card.UniqueID].Count;
        if (CurrentNum >= Card.MaxNum) return false;
        foreach (var RequestID in Card.PrerequisitesID)
            if (AllOwnedTraits[RequestID].Count <= 0) return false;
        foreach (var RequestNotId in Card.PrerequisitesNotID)
            if (AllOwnedTraits[RequestNotId].Count > 0) return false;
        return true;
    }

    int GetIndexRandomly(List<TraitCardObject> CardList)
    {
        float SumOfProbablity = 0;
        List<float> TmpList = new List<float>(CardList.Count);
        for (int i = 0; i < CardList.Count; i++)
        {
            SumOfProbablity += CardList[i].Probability;
            TmpList.Add(SumOfProbablity);
        }
        float RandomValue = Random.Range(0, SumOfProbablity);
        //TODO 二分查找
        for (int i = 0; i < CardList.Count; i++)
            if (RandomValue <= TmpList[i]) return i;
        Debug.Assert(true, "WTF");
        return 0;

    }

    List<TraitCardObject> GenerateAvailableCardList(int NumOfCard)
    {
        List<TraitCardObject> AllPossibleCards = new List<TraitCardObject>();
        List<TraitCardObject> ResultCard = new List<TraitCardObject>(NumOfCard);
        foreach (TraitCardObject Card in AllTraitCards)
        {
            if (GetIsCardAvailable(Card))
            {
                AllPossibleCards.Add(Card);
            }
        }
        // 注意每次刷出的卡池里不会有重复的卡。
        if (AllPossibleCards.Count <= NumOfCard) return AllPossibleCards;

        for (int i = 1; i <= NumOfCard; i++)
        {
            int Index = GetIndexRandomly(AllPossibleCards);
            ResultCard.Add(AllPossibleCards[Index]);
            // 这里的Remove是
            AllPossibleCards[Index] = AllPossibleCards[AllPossibleCards.Count - 1];
            AllPossibleCards.RemoveAt(AllPossibleCards.Count - 1);
        }
        return ResultCard;
    }

    IEnumerator StartShoppingCorotinue()
    {
        int ChildCount = CardContent.transform.childCount;
        for (int i = ChildCount - 1; i >= 0; i--)
        {
            Destroy(CardContent.transform.GetChild(i).gameObject);
        }
        var CardList = GenerateAvailableCardList(CardsInShopEveryTime);
        foreach (var Card in CardList)
        {
            var CardButton = Instantiate(CardButtonPrefab, CardContent.transform);
            ConfigureCardUIObject(CardButton, Card);
        }

        ShoppingUI.transform.DOScale(Vector3.one, 1).SetEase(Ease.InOutElastic);
        yield return new WaitForSeconds(1);
        IsShopping = true;

    }

    public void EndShopping()
    {
        StartCoroutine(EndShoppingCorotinue());
    }
    IEnumerator EndShoppingCorotinue()
    {
        IsShopping = false;
        ShoppingUI.transform.DOScale(Vector3.zero, 1).SetEase(Ease.InOutElastic);
        yield return new WaitForSeconds(1);
    }

    public bool GetIsEndShopping()
    {
        return ShoppingEndButton.GetIsClicked();
    }

    public void ConfigureCardUIObject(GameObject CardUI, TraitCardObject Card)
    {
        CardUI.transform.GetChild(0).Find("Name").GetComponent<TMPro.TMP_Text>().text = Card.Name + "\n$" + Card.Price.ToString();
        CardUI.transform.GetChild(0).Find("Desc").GetComponent<TMPro.TMP_Text>().text = Card.Description;
        var Button = CardUI.GetComponentInChildren<Button>();
        Button.onClick.AddListener(delegate
        {
            if (ScoreManager.Instance.Score < Card.Price) return;
            //TODO：金钱不足的表现。
            ScoreManager.Instance.ChangeScore(-Card.Price);
            TraitManager.Instance.AddTrait(Card.UniqueID);
            Button.enabled = false; Destroy(CardUI);
        });
        //ShoppingEndButton.onClick.AddListener()
        //TODO 生成UI，绑定
        //ButtonPre.onClick.AddListener(delegate { SwitchButtonHandler(0); });
        //ButtonNext.onClick.AddListener(delegate { SwitchButtonHandler(1); });
        // ADD TRAIT()
    }

}
