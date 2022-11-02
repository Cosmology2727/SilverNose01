using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject EnemyToSpawn;

    [SerializeField]
    public float TimeBetweenSpawns;
    [System.NonSerialized]
    public float HowMuchTime;

    [SerializeField]
    public bool IsInfinite;

    [SerializeField]
    public int HowManyToSpawn;


    private void Start()
    {
        TimeBetweenSpawns *= 50; //Makes whatever it is into seconds
        HowMuchTime = TimeBetweenSpawns;
    }

    void FixedUpdate()
    {
        HowMuchTime -= 1;
        if (HowMuchTime <= 0)
        {
            if (IsInfinite)
            {
                SpawnEnemy();
            }
            else if (HowManyToSpawn > 0)
            {
                HowManyToSpawn -= 1;
                SpawnEnemy();
            }
        }
    }

    public void SpawnEnemy()
    {
        HowMuchTime = TimeBetweenSpawns;
        Instantiate(EnemyToSpawn, transform.position, Quaternion.identity);
    }
}
