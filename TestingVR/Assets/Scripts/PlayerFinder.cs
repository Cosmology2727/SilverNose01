using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFinder : MonoBehaviour
{

    [System.NonSerialized]
    public Vector3 PlayerPos;

    [SerializeField]
    public GameObject PlayerObj;



    void Start()
    {
        //Debug.Log("From playerFinder obj " + this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPos = this.transform.position;
        //Debug.Log("PlayerFinder.PlayerPos = " + PlayerPos);
        //Debug.Log(this.transform.position);
    }
}
