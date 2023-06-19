using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubberScript : MonoBehaviour
{
    [SerializeField] GameObject slingRubber;
    [SerializeField] GameObject man1;
    [SerializeField] GameObject man2;
    [SerializeField] Animator animator;
    Vector3 mousePos0;
    Vector3 mousePosS;
    Vector3 rubberPos0;
    Vector3 man1Pos0;
    
    [SerializeField] float time = 100;
    float diffPosX;
    float diffPosY;
    float eff = 0.005f;
    float diffX;
    float diffY;
    float diffZ;

    bool tween = false;
    // Start is called before the first frame update
    void Start()
    {
        rubberPos0 = slingRubber.transform.position;
        animator = man2.GetComponent<Animator>();
        man1Pos0 = man1.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            mousePos0 =  Input.mousePosition;
            
        }
        if (Input.GetButton("Fire1"))
        {
            mousePosS =  Input.mousePosition;
            
            diffPosX = (mousePos0.x - mousePosS.x);
            diffPosY = (mousePos0.y - mousePosS.y);
            
        }
        if (Input.GetButtonUp("Fire1"))
        {
            slingRubber.transform.position = rubberPos0;
            diffPosX = 0;
            diffPosY = 0;
            diffX = man1Pos0.x + 0.2f - man2.transform.position.x;
            diffY = man2.transform.position.y - man2.transform.position.y;
            diffZ = man1Pos0.z - man2.transform.position.z;
            man1.GetComponent<ManScript>().onSling = false;
            //man2.transform.position = new Vector3(man1Pos0.x + 0.2f, man2.transform.position.y, man1Pos0.z);
            tween = true;
            animator.SetBool("isClimbing", true);
        }

        if(man2.transform.position.y > 3.8)
        {
            animator.SetBool("idle", true);
            man2.transform.rotation = Quaternion.Euler(0,0,0);
        }

 

        slingRubber.transform.position = new Vector3( Mathf.Clamp(-diffPosX * eff + rubberPos0.x, -0.5f, 0.5f), slingRubber.transform.position.y, Mathf.Clamp(-diffPosY * eff + rubberPos0.z, -9.7f, rubberPos0.z));
        
    }

    void Tween(Vector3 v0, Vector3 vS, float time)
    {
        float diffX = vS.x - v0.x;
        float diffY = vS.y - v0.y;
        float diffZ = vS.z - v0.z;

        v0 = new Vector3(v0.x - (diffX / time) * Time.deltaTime, v0.y - (diffY / time) * Time.deltaTime, v0.z - (diffZ / time) * Time.deltaTime);
    }
}
