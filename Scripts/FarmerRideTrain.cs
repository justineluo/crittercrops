using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerRideTrain : MonoBehaviour
{
    public GameObject trainEntrance;
    public GameObject topOfStairs;
    bool gettingOnTrain;
    bool goUpStairs;
    bool trainMove;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("GetOnTrain", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (gettingOnTrain) {
            FaceTarget(trainEntrance.transform.position);
            float step = 2f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, trainEntrance.transform.position, step);
            if (Vector3.Distance(transform.position, trainEntrance.transform.position) < 0.2) {
                gettingOnTrain = false;
                goUpStairs = true;
            }
        } else if (goUpStairs) {
            FaceTarget(topOfStairs.transform.position);
            float step = 2f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, topOfStairs.transform.position, step);
            if (Vector3.Distance(transform.position, topOfStairs.transform.position) < 0.2) {
                goUpStairs = false;
                Invoke("TrainMove", 1);
            }
        } else if (trainMove) {
            TrainChooChooMove.startMove = true;
            float step = 10f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, topOfStairs.transform.position, step);
        } else {
            FaceTarget(Camera.main.gameObject.transform.position);
        }
    }

    void GetOnTrain() {
        gettingOnTrain = true;
    }

    void TrainMove() {
        trainMove = true;
    }
    
    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0; // we dont change the y rotation
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }
}
