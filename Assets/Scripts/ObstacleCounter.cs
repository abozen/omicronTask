using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class ObstacleCounter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject waterParticle;
    [SerializeField] TMP_Text percentText;
    [SerializeField] TMP_Text fpsText;
    [SerializeField] TMP_Text menLeftText;
    [SerializeField] TMP_Text percentTextEnd;
    [SerializeField] GameObject sling;

    public Slider slider;
    public Image image;
    public Color color;
    public float obstacleCounter = 0;
    float deltaTime;
    float fps;
    public static float number;

    SlingController slingController;
    

    void Start()
    {
        slingController = sling.GetComponent<SlingController>();
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;
        StartCoroutine(WaitAndDo(null, 2));
        number = Mathf.Floor((obstacleCounter / 88) * 100);
        
        image.color = number == 100 ? color : image.color;
        slider.value = number;
        percentText.text = "%" + number;
        menLeftText.text = slingController.menLeft + " left";

        percentTextEnd.text = "%" + number;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "obstacle" || other.tag == "man")
        {
            GameObject waterP = Instantiate(waterParticle, other.gameObject.transform.position, Quaternion.Euler(90,0,0));
            //waterP.GetComponent<ParticleSystem>().Play(false);  
        }   
        if(other.tag == "obstacle")
        {
            obstacleCounter++;      
        }
        
        
        
    }
    public IEnumerator WaitAndDo(System.Action action, float waitingTime)
    {
        float time = 0;
        
        while(time < waitingTime)
        {
            time += Time.deltaTime;
            yield return null;
        }

        fpsText.text = Mathf.Ceil (fps).ToString ();
    }
}
