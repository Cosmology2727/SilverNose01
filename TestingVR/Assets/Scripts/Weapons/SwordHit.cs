using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : MonoBehaviour
{



    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
    }
}
