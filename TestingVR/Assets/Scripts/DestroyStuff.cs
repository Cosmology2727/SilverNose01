using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyStuff : MonoBehaviour
{
    [SerializeField]
    public int TimeToDestroy = 300;

    [SerializeField]
    public GameObject ToDestroyObj;


    void Start()
    {
        if (ToDestroyObj = null)
        {
            ToDestroyObj = this.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        TimeToDestroy -= 1;
        if (TimeToDestroy < 0)
        {
            //Debug.Log("destroy ");
            Destroy(this);
        }
    }
}
