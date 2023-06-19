using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlingController : MonoBehaviour
{
    public float rotationSpeed = 1;
    public float BlastPower = 5;
    public int menLeft = 3;

    Vector3 mousePos0;
    Vector3 mousePosS;
    Vector3 rubberPos0;
    Vector3 man1Pos0;
    Quaternion man2Rot0;
    StartMenu startMenu;

    public float diffPosX;
    public float diffPosY;

    float cdiffPosX;
    float cdiffPosY;
    float eff = 0.005f;
    bool hunderedPercent;

    bool canFire = true;

    public GameObject currentMan;
    public GameObject[] men;
    public GameObject man1;
    public GameObject man2;
    public GameObject man3;
    public GameObject dotPref;
    public GameObject canvas;
    public GameObject endPanel;
    
    [SerializeField] GameObject slingRubber;
    
    GameObject[] dots = new GameObject[20];

    [SerializeField] Animator animator;

    ManScript curManScript;
    Rigidbody curManRigidbody;
    Animator curManAnimator;
    //[SerializeField] Animator currentManAnimator;

    public Transform ShotPoint;
    [SerializeField] Transform targetPos;

    private bool salda = false;

    void Start()
    {
        CreateDots();
        rubberPos0 = slingRubber.transform.position;
        man1Pos0 = man1.transform.position;
        currentMan = man1;
        man1.GetComponent<Animator>().SetBool("idle", true);
        animator = man2.GetComponent<Animator>();
        menLeft = men.Length;
        startMenu = canvas.GetComponent<StartMenu>();

        man2Rot0 = man2.transform.rotation;
        
        
    }

    private void Update()
    {
        if(Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        if (Input.GetButtonDown("Fire1") && canFire && StartMenu.isTapped)
        {
            mousePos0 =  Input.mousePosition;
            currentMan.GetComponent<ManScript>().onSling = true;
            salda = true;

            
        }
        if (Input.GetButton("Fire1") && canFire && salda && StartMenu.isTapped)
        {
            mousePosS =  Input.mousePosition;
            
            cdiffPosX = Mathf.Clamp((mousePos0.x - mousePosS.x) * 0.05f, -5f, 5f);
            cdiffPosY = Mathf.Clamp(-(mousePos0.y - mousePosS.y) * 0.05f, -35f, -5f);

            diffPosX = (mousePos0.x - mousePosS.x);
            diffPosY = (mousePos0.y - mousePosS.y);
            
            SetMarker();
            
        }
        if (Input.GetButtonUp("Fire1") && canFire &&  salda && StartMenu.isTapped)
        {
            salda = false;
            curManScript = currentMan.GetComponent<ManScript>();
            curManRigidbody = currentMan.GetComponent<Rigidbody>();
            curManAnimator = currentMan.gameObject.GetComponent<Animator>();

            curManScript.onSling = false;
            curManScript.SetRigidbodyState(false);
            curManScript.SetColliderState(true);
            curManAnimator.enabled = false;
            curManRigidbody.velocity = ShotPoint.transform.up * BlastPower + new Vector3(cdiffPosX, 0, 0);
            
            Rigidbody[] rigidbodies = currentMan.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody rigidbody in rigidbodies)
            { 
                    rigidbody.velocity = ShotPoint.transform.up * BlastPower + new Vector3(cdiffPosX, 0, 0);
            }
            curManRigidbody.useGravity = true;
            curManRigidbody.isKinematic = false;
            
            
            slingRubber.transform.position = rubberPos0;
            diffPosX = 0;
            diffPosY = 0;

            for (int t = 0; t < dots.Length; t++)
            {
                dots[t].transform.position = new Vector3(-10, -10, -10);   
            }

            menLeft--;
            if(menLeft != 0)
            {
                currentMan = men[men.Length - menLeft];
                canFire = false;
                currentMan.transform.Rotate(0, -95, 0);
                currentMan.GetComponent<Animator>().SetBool("isWalking", true);
                StartCoroutine(GoToTarget(targetPos.position, currentMan, Climb));
            }else
            {
                StartCoroutine(GameOver());
            }

        }

        hunderedPercent = ObstacleCounter.number == 100 ? true : false;
        if(hunderedPercent)
            StartCoroutine(GameOver());
        BlastPower = cdiffPosY;
        
        slingRubber.transform.position = new Vector3( Mathf.Clamp(-diffPosX * eff + rubberPos0.x, -0.5f, 0.5f), slingRubber.transform.position.y, Mathf.Clamp(-diffPosY * eff + rubberPos0.z, -9.7f, rubberPos0.z));
    }

    IEnumerator GameOver()
    {
        var time = 0f;
        while(time < 3 && !hunderedPercent)
        {
            time += Time.deltaTime;

            yield return null;
        }
        time = 0;
        while(time < 1)
        {
            time += Time.deltaTime;

            yield return null;
        }
        Time.timeScale = 0f;
        canFire = false;
        endPanel.SetActive(true);
    }

    void SetMarker()
    {
        float eff = 0.04f;
        Vector3 startingPosition = ShotPoint.position;
        Vector3 startingVelocity = ShotPoint.up * BlastPower + new Vector3(cdiffPosX, 0, 0);
        for (int t = 0; t < dots.Length; t++)
        {
            Vector3 newPoint = startingPosition + t * startingVelocity * eff;
            newPoint.y = startingPosition.y + startingVelocity.y * t * eff + Physics.gravity.y/2f * t * t * eff * eff;

            
            dots[t].transform.position = newPoint;
            
        }
    }

    void CreateDots()
    {
        for (int i = 0; i < dots.Length; i++)
        {
            GameObject createdDot = Instantiate(dotPref, new Vector3(-10, -10, -10), ShotPoint.rotation);
            dots[i] = createdDot;
        }
    }

    public IEnumerator GoToTarget(Vector3 targetPos, GameObject obj, System.Action action)
    {
        var time = 0f;
        var startPos = obj.transform.position;
        while(time < 1)
        {
            time += Time.deltaTime * 3 / 4;
            obj.transform.position = Vector3.Lerp(startPos, targetPos, time);

            yield return null;
        }

        action?.Invoke();
    }

    void Climb()
    {
        
        currentMan.transform.rotation = Quaternion.Euler(0,0,0);
        currentMan.GetComponent<Animator>().SetBool("isClimbing", true);
        StartCoroutine(GoToTarget(man1Pos0, currentMan, Idle));
        
    }

    void Idle()
    {
        canFire = true;
        
        currentMan.GetComponent<Animator>().SetBool("idle", true);
    }


}