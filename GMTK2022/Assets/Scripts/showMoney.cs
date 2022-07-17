using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class showMoney : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text text;
    void Start()
    {
        text.text=Scorekeeper.Instance.FetchScore().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void reTry()
    {

    }
}
