using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Weapons.Guns;

public class EnemyStats : MonoBehaviour
{
    [SerializeField]
    public GameObject AiRef;

    [SerializeField]
    public Animator AnimatorObj;

    [SerializeField]
    public GameObject ThisObj;

    [SerializeField]
    public int EnemyHealth = 100;
    [SerializeField]
    public int EnemyArmor = 0;

    public int ToDelete = 800;

    [System.NonSerialized] public bool HasGrenade = false;


    void Start()
    {
        EnemyHealth *= FindObjectOfType<ListVariables>().GetComponent<ListVariables>().Difficulty;
    }

    // Update is called once per frame
    void Update()
    {
        if (AiRef != null)
        {
            if (EnemyHealth <= 0 & AiRef.GetComponent<EnemyAI>().CurrentMood != 0)
            {
                GetComponent<EnemyRagdoll>().RagdollOn();
                AiRef.GetComponent<EnemyAI>().CurrentMood = 0;
                AnimatorObj.enabled = false;
                ToDelete -= 1;
                if (ToDelete <= 0)
                {
                    Destroy(ThisObj);
                }
            }
        }
    }
}
