using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpPanelBehavior : MonoBehaviour
{
    public GameObject helpPanel;
    static public bool isGamePaused;

    // Start is called before the first frame update
    void Start()
    {
        if (helpPanel == null)
        {
            helpPanel = GameObject.FindGameObjectWithTag("HelpPanel");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {

        }
    }
    void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0.0f;
        helpPanel.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        helpPanel.SetActive(false);
        isGamePaused = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
