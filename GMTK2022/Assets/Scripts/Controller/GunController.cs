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

    public Transform firePoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            DiceController newDice=Instantiate(dice,firePoint.position,Quaternion.identity) as DiceController;
                newDice.GetComponent<Rigidbody>().AddForce(transform.forward*ForceAmount,ForceMode.Impulse);
                newDice.GetComponent<Rigidbody>().freezeRotation=true;
        }



    }
}
