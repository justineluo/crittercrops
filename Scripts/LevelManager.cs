using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

public class LevelManager : MonoBehaviour
{
    public static bool isGameOver = false;
    private int currentMoneyCount;

    public int moneyGoal = 100;

    public Text moneyCountText;
    public Text statusText;
    public AudioClip winSFX;
    public AudioClip loseSFX;

    private Scene scene;
    bool hasPlayedEndSFX;

    // Start is called before the first frame update
    void Start()
    {
        setAndDisplayCurrentMoney(0);
        scene = SceneManager.GetActiveScene();
        // update text

    }

    // Update is called once per frame
    void Update()
    {

        if (currentMoneyCount == moneyGoal)
        {
            LevelWin();
        }
        else if (isGameOver)
        {
            LevelLost();
        }
    }

    public void addToCurrentMoney(int amount)
    {
        setAndDisplayCurrentMoney(currentMoneyCount + amount);

    }

    private void setAndDisplayCurrentMoney(int amount)
    {
        currentMoneyCount = amount;
        moneyCountText.text = currentMoneyCount.ToString() + "/" + moneyGoal.ToString();

    }

    public void LevelLost()
    {
        // call SetGameOverStatus with "GAME OVER!"
        SetGameOverStatus("YOU LOSER!", loseSFX);
        setAndDisplayCurrentMoney(0);
        Invoke("LoadCurrentLevel", 2); // delays it by 2 seconds

    }

    public void LevelWin()
    {
        /// <summary>method to specify what happens when the level is won</summary>
        ///

        // call SetGameOverStatus with "YOU WIN!"
        SetGameOverStatus("YOU WIN!", winSFX);
        Invoke("LoadNextLevel", 2); // delays it by 2 seconds

    }

    public void LoadCurrentLevel()
    {
        int currIndex = scene.buildIndex;
        SceneManager.LoadScene(currIndex, LoadSceneMode.Single);
    }

    public void LoadNextLevel()
    {
        // TODO
        Debug.Log("Loading next level...");
    }

    void SetGameOverStatus(string gameTextMessage, AudioClip sfx)
    {

        // set isGameOver 
        isGameOver = true;

        // update gameText UI component with appropriate message and activate it
        // message is received as an argument
        statusText.text = gameTextMessage;
        statusText.enabled = true; // can't use SetActive here

        if (!hasPlayedEndSFX) {
            AudioSource.PlayClipAtPoint(sfx, Camera.main.transform.position);
            hasPlayedEndSFX = true;
        }
    }
}