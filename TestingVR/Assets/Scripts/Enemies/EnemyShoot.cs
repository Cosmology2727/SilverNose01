using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField]
    public GameObject EnemyAIRef;

    public AudioSource AudSource;
    public AudioClip AudClip;

    public void Shoot()
    {
        AudSource.PlayOneShot(AudClip);
        EnemyAIRef.GetComponent<EnemyAI>().EnemyShootFunction();
    }
}
