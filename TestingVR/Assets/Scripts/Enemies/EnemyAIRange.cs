using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIRange : MonoBehaviour
{
    [SerializeField]
    public GameObject SightViewObj;

    public GameObject OtherFromTrigger;

    public void OnTriggerEnter(Collider other)
    {
        OtherFromTrigger = other.gameObject;
        SightViewObj.GetComponent<EnemyAI>().InRangeTrigger(OtherFromTrigger);
    }

    public void OnTriggerExit(Collider other)
    {
        OtherFromTrigger = other.gameObject;
        SightViewObj.GetComponent<EnemyAI>().OutRangeTrigger(OtherFromTrigger);
    }
}
