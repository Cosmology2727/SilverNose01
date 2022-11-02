using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    [SerializeField]
    public GameObject[] Rigid;

    [SerializeField]
    public bool RagdollNow;

    public void Start()
    {
        if (RagdollNow)
        {
            RagdollOn();
        }
    }

    public void RagdollOn()
    {
        for (int i = 0; i < 40; i++)
        {
            if (i == 39 & Rigid[i].GetComponent<Rigidbody>() != null)
            {
                Rigid[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
            else 
            {
                Rigid[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
        }
    }
}
