using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangularParentScript : MonoBehaviour
{
    [SerializeField] GameObject obj;
    private KinematicControl kinematicControl;
    // Start is called before the first frame update
    public bool triggered = false;
    void Start()
    {
        kinematicControl = obj.GetComponent<KinematicControl>();
    }

    // Update is called once per frame
    void Update()
    {
        triggered = kinematicControl.triggered;
    }
}
