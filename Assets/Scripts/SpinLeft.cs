using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinLeft : MonoBehaviour
{
    [SerializeField] float velocity = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 1f * Time.deltaTime * velocity, 0f, Space.Self);
    }
}
