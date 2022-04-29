using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawberryWalkAimlessly : MonoBehaviour
{
    Rigidbody rb;
    GameObject[] wanderPoints;
    Vector3 nextDestination;
    float critterSpeeed = 3f;
    int currentDestinationIndex = 0;

    // Use this for initialization
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        wanderPoints = GameObject.FindGameObjectsWithTag("StrawberryWanderPoint");
        nextDestination = wanderPoints[0].transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        FaceTarget(nextDestination);
        transform.position = Vector3.MoveTowards(transform.position, nextDestination, critterSpeeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, nextDestination) <= 2)
        {
            nextDestination = wanderPoints[currentDestinationIndex].transform.position;
            currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;
        }

    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }
}

