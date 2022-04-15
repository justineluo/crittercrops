using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VillagerBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public string NPCname = "Villager";
    public GameObject player;
    public GameObject dialogCanvas; 

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



    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        wanderPoints = GameObject.FindGameObjectsWithTag("Wanderpoint");
        agent = GetComponent<NavMeshAgent>();
        currentState = FSMStates.Stroll;
        nextDestination = wanderPoints[0].transform.position;
    
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        switch(currentState)
        {
            case FSMStates.Stroll:
                UpdateStrollState();
                break;
            case FSMStates.Converse:
                UpdateConverseState();
                break;
           
        }
       if (distanceToPlayer <= 10)
       {
            agent.speed = 0f;
            currentState = FSMStates.Converse;
       } else {
            currentState = FSMStates.Stroll;

       }
     
    }


    void UpdateStrollState()
    {

        agent.speed = 3f;

        if(Vector3.Distance(transform.position, nextDestination) <= 2)
        {
            nextDestination = wanderPoints[currentDestinationIndex].transform.position;
            currentDestinationIndex = (currentDestinationIndex + 1 ) % wanderPoints.Length;
            Debug.Log("Going to next wanderpoint");
        }

        FaceTarget(nextDestination);

        agent.SetDestination(nextDestination);
       
    }

    void UpdateConverseState()
    { 
        if (!startedConvo) 
        {
            FindObjectOfType<DialogManager>().StartDialog(NPCname);
            dialogCanvas.SetActive(true);
            startedConvo = true;
        }
     
    
        FaceTarget(player.transform.position);

        if(distanceToPlayer > 10) 
        {
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
