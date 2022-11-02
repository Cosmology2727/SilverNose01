using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Core.Player;
using HurricaneVR.Framework.Components;
using HurricaneVR.Framework.ControllerInput;
using HurricaneVR.Framework.Core.Grabbers;

public class ListVariables : MonoBehaviour
{
    public GameObject WhichHand;
    public GameObject CurrentlyHeld;
    public float LGValue = 0;

    public int Difficulty = 1;

    public int CurrentBulletDamage;

    public bool AlertIsOn;

    void Start()
    {
        WhichHand = FindObjectOfType<HandFinderLeft>().GetComponent<GameObject>();    
    }


    void Update()
    {
        
    }
}
