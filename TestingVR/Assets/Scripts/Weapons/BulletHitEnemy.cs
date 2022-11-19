using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitEnemy : MonoBehaviour
{
    [SerializeField]
    public int BulletDamage;
    public int ThisDamage;
    public int TempDamage;

    [SerializeField]
    public GameObject BloodSpurtObj;

    [SerializeField]
    public GameObject BloodObj;
    [SerializeField]
    public GameObject DustObj;
    [SerializeField]
    public GameObject SparksObj;

    [SerializeField]
    public GameObject ThisBullet;

    [SerializeField]
    public float range = 1000f;

    public Vector3 BulletDir;
    public float BulletOffX;
    public float BulletOffY;
    public float BulletOffZ;
    public float BulletTravelDistance;
    public float BulletEqualizer;
    public Vector3 BulletDirDifference;
    public Vector3 BulletStartPos;
    public Vector3 BulletSecondPos;
    public Vector3 BulletRayStart;
    public int BulletTime = 0;

    public GameObject GunBack;
    public GameObject GunFront;

    public GameObject BulletOrigin;
    [SerializeField]
    public GameObject DebugSphere;

    public void Start()
    {
        //Debug.Log(ThisBullet.transform.position);
        transform.parent = null;

        GunBack = BulletOrigin;      //FindObjectOfType<GunDirection>().GetComponent<GunDirection>().GunBack;    Used to be this, not sure why, but it was probably referencing the gun way of to the side?
        GunFront = FindObjectOfType<PlayerFinder>().GetComponent<PlayerFinder>().PlayerObj;
        BulletDir = GunFront.transform.position - GunBack.transform.position;
        BulletOffX = Random.Range(-1f, 1f);
        BulletOffY = Random.Range(-1f, 1f);
        BulletOffZ = Random.Range(-1f, 1f);
        BulletTravelDistance = Vector3.Distance(GunBack.transform.position, GunFront.transform.position);
        //15m is good for the 2x2
        BulletEqualizer = BulletTravelDistance / 15;  //This should be modifiable for difficulty
        BulletDir.x += BulletEqualizer * BulletOffX;
        BulletDir.y += (BulletEqualizer * BulletOffY) +1;
        //BulletDir.y += 1.2f;
        BulletDir.z += BulletEqualizer * BulletOffZ;

        //Debug.Log(BulletTravelDistance);
        //GameObject DebugSphere1 = Instantiate(DebugSphere, BulletOrigin.transform);
        //GameObject DebugSphere2 = Instantiate(DebugSphere, GunFront.transform);
        ThisBullet.transform.position = BulletOrigin.transform.position;
        ThisBullet.GetComponent<Rigidbody>().velocity = BulletDir * 3;
    }

    /*public void Update()
    {
        BulletTime += 1;
        if (BulletTime == 1)
        {
            BulletStartPos = ThisBullet.GetComponent<Rigidbody>().transform.position;
        }
        else if (BulletTime == 2)
        {
            BulletSecondPos = ThisBullet.GetComponent<Rigidbody>().transform.position;
        }

        if (BulletTime > 1)
        {
            BulletDir = BulletSecondPos - BulletStartPos;
            //BulletDirDifference = BulletDir / 2;
            RaycastHit hit;
            BulletRayStart = ThisBullet.GetComponent<Rigidbody>().position;
            
            if (Physics.Raycast(BulletRayStart, BulletDir, out hit, range))
            {
                Debug.Log(hit.transform.name + " " + hit.transform.tag);
                GameObject NewBloodSpurt = Instantiate(BloodSpurtObj, hit.point, this.transform.rotation);
            }
        }
        if (BulletTime > 30)
        {
            //Destroy(ThisBullet);
        }


    }
    //*/
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject NewBlood = Instantiate(BloodObj, this.transform.position, this.transform.rotation);
            Destroy(other);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        //Debug.Log("collision with " + other.gameObject.tag + " named " + other.transform.name);
        if (other.gameObject.tag == "Gun")
        {
            GameObject NewSparks = Instantiate(SparksObj, this.transform.position, this.transform.rotation);
        }
        else if (other.gameObject.tag == "Player")
        {
            GameObject NewBlood = Instantiate(BloodObj, this.transform.position, this.transform.rotation);
        }
        else
        {
            GameObject NewDust = Instantiate(DustObj, this.transform.position, this.transform.rotation);
        }
        Destroy(ThisBullet);
        /*else if (other.gameObject.tag == "Enemy") //Checks collision object to see if it's an enemy
        {
            GameObject NewBloodSpurt = Instantiate(BloodSpurtObj, this.transform.position, this.transform.rotation);

            if (other.gameObject.GetComponent<BodyPartId>() != null)
            {
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
                    GameObject NewDust = Instantiate(DustObj, this.transform.position, this.transform.rotation);
                }
                if (ThisDamage > 0)
                {
                    other.gameObject.GetComponent<BodyPartId>().EnemyStatsObj.GetComponent<EnemyStats>().EnemyHealth -= ThisDamage;
                    GameObject NewBlood = Instantiate(BloodObj, this.transform.position, this.transform.rotation);
                }
            }
            
        }
        else
        {
            GameObject NewDust = Instantiate(DustObj, this.transform.position, this.transform.rotation);
        }

        if (other.gameObject.tag == "Hat")
        {
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            other.transform.parent = null;
        }

        
        Destroy(ThisBullet);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        Debug.Log("1");       
        if (other.gameObject.tag == "Enemy") //Checks collision object to see if it's an enemy
        {
            Debug.Log("2");
            Destroy(other.gameObject);
        }
        //Destroy(this);*/
    }
}
