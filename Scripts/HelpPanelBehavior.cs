using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpPanelBehavior : MonoBehaviour
{
    public GameObject helpPanel;
    static public bool isGamePaused;
    public GameObject recticle;

    // Start is called before the first frame update
    void Start()
    {
        if (helpPanel == null)
        {
            helpPanel = GameObject.FindGameObjectWithTag("HelpPanel");
        }

        if (recticle == null)
        {
            recticle = GameObject.FindGameObjectWithTag("Crosshair");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (isGamePaused)
            {
                CloseHelpPanel();
            }
            else
            {
                OpenHelpPanel();
            }

        }
    }
    void OpenHelpPanel()
    {
        isGamePaused = true;
        Time.timeScale = 0.0f;
        helpPanel.SetActive(true);
        recticle.SetActive(false);
        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void CloseHelpPanel()
    {
        Time.timeScale = 1.0f;
        helpPanel.SetActive(false);
        recticle.SetActive(true);
        isGamePaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
    }
}
