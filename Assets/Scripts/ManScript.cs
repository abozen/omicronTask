using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManScript : MonoBehaviour
{
    public bool onSling = false;
    [SerializeField] GameObject rubber;
    // Start is called before the first frame update
    void Start()
    {
        SetRigidbodyState(true);
        SetColliderState(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(onSling)
        {
            transform.position = rubber.transform.position + new Vector3(0, -0.4f, 0.2f);
        }
    }

    public void SetRigidbodyState(bool state)
    {

        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            //if(rigidbody.transform != transform)
                rigidbody.isKinematic = state;
        }

        //GetComponent<Rigidbody>().isKinematic = !state;

    }
    public void SetColliderState(bool state)
    {

        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            //if(collider.transform != transform)
                collider.enabled = state;
        }

        GetComponent<Collider>().enabled = !state;

    }

    public void SetAnimatorState(bool state)
    {
        gameObject.GetComponent<Animator>().enabled = state;
    }
}
