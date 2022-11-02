using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Components;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Core.Utils;
using HurricaneVR.Framework.Shared;
using HurricaneVR.Framework.Weapons.Guns;

public class BodyPartId : MonoBehaviour
{
    [SerializeField]
    public bool Is01VeryWeak;
    [SerializeField]
    public bool Is02Weak;
    [SerializeField]
    public bool Is03Mild;
    [SerializeField]
    public bool Is04Normal;
    [SerializeField]
    public bool Is05Rough;
    [SerializeField]
    public bool Is06Strong;
    [SerializeField]
    public bool Is07VeryStrong;

    [SerializeField]
    public GameObject EnemyStatsObj;
}
