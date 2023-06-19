using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicControl : MonoBehaviour
{
    
    public bool triggered = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "man")
        {
            // other.transform.root.gameObject.GetComponent<ManScript>().SetRigidbodyState(false);
            // other.transform.root.gameObject.GetComponent<ManScript>().SetColliderState(true);
            // other.transform.root.gameObject.GetComponent<Animator>().enabled = false;
            other.transform.root.gameObject.transform.Find("Hit Particle").GetComponent<ParticleSystem>().Play(true);
            other.transform.root.gameObject.transform.Find("Hit Particle 2").GetComponent<ParticleSystem>().Play(true);
            
            triggered = true;
        }
    }
}
