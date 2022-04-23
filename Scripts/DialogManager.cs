using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    private Queue<string> sentences = new Queue<string>();
    // Start is called before the first frame update
    public Text nameText;
    public Text dialogText;
    public GameObject dialogCanvas;

    void Awake()
    {
        AddDialogue();
    }

    public void AddDialogue()
    {
        sentences.Clear();
        sentences = new Queue<string>();
        sentences.Enqueue("Hello youngster! I see you finally woke up...");

        sentences.Enqueue("I was just going for my morning stroll when I saw you lying on the ground. ");
        sentences.Enqueue("I'll tell you this since you look like you aren't from around here: ");
        sentences.Enqueue("Beware of the Crittercrops! They will attack you and kill you if you get too close.");
        sentences.Enqueue("However, if you are looking to make some money...");
        sentences.Enqueue("To damage CritterCrops, aim your spray bottle to stray pesticide on them. That should give them damage.");
        sentences.Enqueue("If you manage to kill them, you can pick up the seeds they drop by making contact.");
        sentences.Enqueue("You can grow crops out of these seeds for money.");
        sentences.Enqueue("Then you can try planting in this farm land here by pressing q.");
        sentences.Enqueue("Once the plant's ready, press r to harvest it.");
        sentences.Enqueue("To open and close the inventory, press x.");
        sentences.Enqueue("To run, press Shift.");
        sentences.Enqueue("If you forget anything, press h for help or come talk to me again!");
        sentences.Enqueue("Good luck!");
    }

    public void StartDialog(string npcName)
    {
        AddDialogue();
        nameText.text = npcName;
        dialogCanvas.SetActive(true);
        InvokeRepeating("DisplayNextSentence", 0f, 4f);
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }
        else
        {
            string sentence = sentences.Dequeue();
            dialogText.text = sentence;

            // trying to skip through
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("skip");
                DisplayNextSentence();
            }
        }

    }

    void EndDialog()
    {
        CancelInvoke();
        dialogCanvas.SetActive(false);
    }

}
