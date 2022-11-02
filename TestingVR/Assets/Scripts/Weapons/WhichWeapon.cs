using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhichWeapon : MonoBehaviour
{
    [SerializeField]
    public int BulletDamage;

    void Start()
    {
        //FindObjectOfType<ListVariables>().GetComponent<ListVariables>().CurrentBulletDamage = BulletDamage;
    }
    void OnEnable()
    {
        FindObjectOfType<ListVariables>().GetComponent<ListVariables>().CurrentBulletDamage = BulletDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
