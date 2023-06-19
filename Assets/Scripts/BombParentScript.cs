using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombParentScript : MonoBehaviour
{
    // Start is called before the first frame update
   private bool isExplode = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetExplode(bool bo)
    {
        isExplode = bo;
    }
    public bool GetExplode()
    {
        return isExplode;
    }


}
