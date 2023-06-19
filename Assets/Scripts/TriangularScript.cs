using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangularScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            if(transform.parent.gameObject.GetComponent<TriangularParentScript>().triggered)
                GetComponent<Rigidbody>().isKinematic = false;
    }
}
