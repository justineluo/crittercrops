
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BeaverBoihavior : MonoBehaviour
{
    // Start is called before the first frame update
    public string NPCname = "Beaver Boi";
    public GameObject player;
    public GameObject dialogCanvas;
    public GameObject reticle;

    public static bool startedConvo = false;
    GameObject[] wanderPoints;
    NavMeshAgent agent;

    enum FSMStates
    {
        Converse,
        Stroll
    }
    FSMStates currentState;
    float distanceToPlayer;
    Vector3 nextDestination;
    int currentDestinationIndex = 0;
    Animator anim;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        wanderPoints = GameObject.FindGameObjectsWithTag("Wanderpoint");
        agent = GetComponent<NavMeshAgent>();
        currentState = FSMStates.Converse;
        nextDestination = wanderPoints[0].transform.position;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        switch (currentState)
        {
            case FSMStates.Stroll:
                UpdateStrollState();
                break;
            case FSMStates.Converse:
                UpdateConverseState();
                break;

        }
        if (distanceToPlayer <= 5)
        {
            agent.speed = 0f;
            currentState = FSMStates.Converse;
        } else
        {
            currentState = FSMStates.Stroll;
        }
    }


    void UpdateStrollState()
    {
        anim.SetInteger("animState", 1);
        reticle.SetActive(true);

        agent.speed = 3f;

        if (Vector3.Distance(transform.position, nextDestination) <= 2)
        {
            nextDestination = wanderPoints[currentDestinationIndex].transform.position;
            currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;
        }

        FaceTarget(nextDestination);

        agent.SetDestination(nextDestination);

    }

    void UpdateConverseState()
    {
        anim.SetInteger("animState", 0);
        reticle.SetActive(false);

        if (!startedConvo)
        {
            FindObjectOfType<DialogManager>().StartDialog(NPCname);
            startedConvo = true;
        }


        FaceTarget(player.transform.position);

        if (distanceToPlayer > 5)
        {
            dialogCanvas.SetActive(false);
            startedConvo = false;
            currentState = FSMStates.Stroll;
        }
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0; // we dont change the y rotation
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

}