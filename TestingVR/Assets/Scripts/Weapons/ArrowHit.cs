using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHit : MonoBehaviour
{
    [SerializeField]
    GameObject ThisArrow;

    bool InAir = true;

    int TimeToDeath = 600;

    public Rigidbody ThisRigid;

    public Vector3 ArrowVector;

    [SerializeField]
    public GameObject BloodSpurtObj;

    [SerializeField]
    public int BulletDamage = 10;
    public int ThisDamage;
    public int TempDamage;
    



    private void Start()
    {
        TimeToDeath = 5000;
        ThisRigid = GetComponent<Rigidbody>();
        InAir = true;
    }

    void Update()
    {
        TimeToDeath -= 1;
        if (TimeToDeath < 1)
        {
            Destroy(ThisArrow);
        }
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision other)
    {
        if (InAir)
        {
            Debug.Log("collision of " + other.gameObject.tag);
        }

        if (other.gameObject.tag == "StickyObj" && InAir)
        {
            ThisRigid.constraints = RigidbodyConstraints.FreezeAll; 
            ArrowVector = ThisRigid.velocity;
            InAir = false;
            //ThisArrow.transform.parent = other.transform;                 //For some reason this was changing the rotation of the arrow
        }

        if (other.gameObject.tag == "Enemy" && InAir) //Checks collision object to see if it's an enemy
        {
            GameObject NewBloodSpurt = Instantiate(BloodSpurtObj, this.transform.position, this.transform.rotation);

            if (other.gameObject.GetComponent<BodyPartId>().Is01VeryWeak)
            {
                ThisDamage = BulletDamage * 1;
            }
            if (other.gameObject.GetComponent<BodyPartId>().Is02Weak)
            {
                ThisDamage = BulletDamage * 2;
            }
            if (other.gameObject.GetComponent<BodyPartId>().Is03Mild)
            {
                ThisDamage = BulletDamage * 3;
            }
            if (other.gameObject.GetComponent<BodyPartId>().Is04Normal)
            {
                ThisDamage = BulletDamage * 4;
            }
            if (other.gameObject.GetComponent<BodyPartId>().Is05Rough)
            {
                ThisDamage = BulletDamage * 5;
            }
            if (other.gameObject.GetComponent<BodyPartId>().Is06Strong)
            {
                ThisDamage = BulletDamage * 6;
            }
            if (other.gameObject.GetComponent<BodyPartId>().Is07VeryStrong)
            {
                ThisDamage = BulletDamage * 100;
            }

            if (other.gameObject.GetComponent<BodyPartId>().EnemyStatsObj.GetComponent<EnemyStats>().EnemyArmor > 0)
            {
                TempDamage = ThisDamage;
                ThisDamage -= other.gameObject.GetComponent<BodyPartId>().EnemyStatsObj.GetComponent<EnemyStats>().EnemyArmor;
                other.gameObject.GetComponent<BodyPartId>().EnemyStatsObj.GetComponent<EnemyStats>().EnemyArmor -= TempDamage;
            }
            other.gameObject.GetComponent<BodyPartId>().EnemyStatsObj.GetComponent<EnemyStats>().EnemyHealth -= ThisDamage;
        }

        if (other.gameObject.tag == "Hat")
        {
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            other.transform.parent = null;
            InAir = false;
            Destroy(other.gameObject);
        }

        else if (other.gameObject.tag != "Enemy" && InAir)
        {
            InAir = false;
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        Debug.Log("1");
        if (other.gameObject.tag == "Enemy") //Checks collision object to see if it's an enemy
        {
            Debug.Log("2");
            Destroy(other.gameObject);

        }
    }*/
    }
