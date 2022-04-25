using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordBehavior : MonoBehaviour
{
    float timeCounter = 0;
    float speed = .05f;
    float radius = 70;

    public Transform target;

    private void Update()
    {

        timeCounter += Time.deltaTime * speed;
        float x = Mathf.Cos(timeCounter) * radius + target.position.x;
        float z = Mathf.Sin(timeCounter) * radius + target.position.z;

        transform.LookAt(target);
        transform.position = new Vector3(x, transform.position.y, z);
    }
}
