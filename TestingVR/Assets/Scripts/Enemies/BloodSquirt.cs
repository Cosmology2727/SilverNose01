using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Weapons.Guns;
using HurricaneVR.Framework.Components;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Core.Utils;
using HurricaneVR.Framework.Shared;


public class BloodSquirt : MonoBehaviour
{
    [SerializeField]
    public GameObject BloodObj;
    [SerializeField]
    public GameObject DustObj;
    [SerializeField]
    public GameObject SparksObj;

    public int BulletDamage;
    public int ThisDamage;
    public int TempDamage;

    public int FindingHit = 0;
    public int HitNumber = -1;

    void Start()
    {
        BulletDamage = FindObjectOfType<ListVariables>().GetComponent<ListVariables>().CurrentBulletDamage;

        if (Physics.CheckSphere(this.transform.position, 0.1f))
        {
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 0.1f, 1<<10 | 10);
            //Debug.Log(hitColliders[0].transform.name + " " + hitColliders[0].transform.tag);

            /*while (FindingHit < hitColliders.Length | HitNumber != -1)            //This whole thing crashes the game as it is
            {
                if (hitColliders[FindingHit] == null)
                {
                    FindingHit = hitColliders.Length - 1;
                }
                else if (hitColliders[FindingHit].transform == null)
                {
                    FindingHit = hitColliders.Length;
                }
                else if (hitColliders[FindingHit].transform.gameObject == null)
                {
                    FindingHit = hitColliders.Length;
                }
                else if (hitColliders[FindingHit].transform.gameObject.GetComponent<BodyPartId>() == null)
                {
                    FindingHit = hitColliders.Length - 1;
                }
                else if (hitColliders[FindingHit].transform.gameObject.GetComponent<BodyPartId>() != null)
                {
                    HitNumber = FindingHit;
                }

                FindingHit = 1;
            }*/


            Debug.Log(hitColliders[0].transform.tag);
            if (hitColliders[0] != null && hitColliders[0].transform.tag == "Enemy")
            {
                if (hitColliders[0].transform.gameObject.GetComponent<BodyPartId>() != null)
                {
                    if (hitColliders[0].transform.gameObject.GetComponent<BodyPartId>().Is01VeryWeak)
                    {
                        ThisDamage = BulletDamage * 1;
                        //Debug.Log("1");
                    }
                    else if (hitColliders[0].transform.gameObject.GetComponent<BodyPartId>().Is02Weak)
                    {
                        ThisDamage = BulletDamage * 2;
                        //Debug.Log("2");
                    }
                    else if (hitColliders[0].transform.gameObject.GetComponent<BodyPartId>().Is03Mild)
                    {
                        ThisDamage = BulletDamage * 3;
                        //Debug.Log("3");
                    }
                    else if (hitColliders[0].transform.gameObject.GetComponent<BodyPartId>().Is04Normal)
                    {
                        ThisDamage = BulletDamage * 4;
                        //Debug.Log("4");
                    }
                    else if (hitColliders[0].transform.gameObject.GetComponent<BodyPartId>().Is05Rough)
                    {
                        ThisDamage = BulletDamage * 5;
                        //Debug.Log("5");
                    }
                    else if (hitColliders[0].transform.gameObject.GetComponent<BodyPartId>().Is06Strong)
                    {
                        ThisDamage = BulletDamage * 6;
                        //Debug.Log("6");
                    }
                    else if (hitColliders[0].transform.gameObject.GetComponent<BodyPartId>().Is07VeryStrong)
                    {
                        ThisDamage = BulletDamage * 100;
                        //Debug.Log("7");
                    }

                    if (hitColliders[0].transform.gameObject.GetComponent<BodyPartId>().EnemyStatsObj.GetComponent<EnemyStats>().EnemyArmor > 0)
                    {
                        GameObject NewDust = Instantiate(DustObj, this.transform.position, this.transform.rotation);
                        TempDamage = ThisDamage;
                        ThisDamage -= hitColliders[0].transform.gameObject.GetComponent<BodyPartId>().EnemyStatsObj.GetComponent<EnemyStats>().EnemyArmor;
                        hitColliders[0].transform.gameObject.GetComponent<BodyPartId>().EnemyStatsObj.GetComponent<EnemyStats>().EnemyArmor -= TempDamage;
                    }
                    if (ThisDamage > 0)
                    {
                        //Debug.Log("blood");
                        GameObject NewBlood = Instantiate(BloodObj, this.transform.position, this.transform.rotation);
                        hitColliders[0].transform.gameObject.GetComponent<BodyPartId>().EnemyStatsObj.GetComponent<EnemyStats>().EnemyHealth -= ThisDamage;
                    }
                }
                
            }

            else if (hitColliders[0].transform.tag == "Gun")
            {
                GameObject NewSparks = Instantiate(SparksObj, this.transform.position, this.transform.rotation);
            }
            else
            {
                GameObject NewDust = Instantiate(DustObj, this.transform.position, this.transform.rotation);
            }
            
        }
    }
}
