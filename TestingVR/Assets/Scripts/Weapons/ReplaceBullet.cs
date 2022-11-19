using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceBullet : MonoBehaviour
{
    [SerializeField]
    public GameObject NewBullet;

    [SerializeField]
    public Rigidbody NewBulletRigid;



    // Start is called before the first frame update
    void Start()
    {
        GameObject CreatedBullet = Instantiate(NewBullet, this.transform);
        CreatedBullet.transform.forward = this.transform.forward * 10;
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
