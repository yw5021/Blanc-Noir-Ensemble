using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Vector3 target = new Vector3();
    public GameObject eff;
    public GameObject eff1;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, 0.7f);
    }

     void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag == "effbox")
        {
            Instantiate(eff, transform.position, transform.rotation);
            Instantiate(eff1, transform.position, transform.rotation);
        }
    }
}
