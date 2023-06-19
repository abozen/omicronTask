using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public static bool isTapped = false;
    public GameObject startPanel;
    private bool  salda = true;
    // Start is called before the first frame update
    void Start()
    {
        Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Fire1"))
            isTapped = true;
        
        if(isTapped && salda)
        {
            Contuniue();
        }
    }

    private void Contuniue()
    {
        startPanel.SetActive(false);
        Time.timeScale = 1f;
        salda = false;
    }
    private void Pause()
    {
        startPanel.SetActive(true);
        Time.timeScale = 0f;
        
    }
}
