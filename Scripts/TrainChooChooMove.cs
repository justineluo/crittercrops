using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainChooChooMove : MonoBehaviour
{
    public static bool startMove;
    public AudioClip chooChoo;
    Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
        target.z -= 50f;
    }

    // Update is called once per frame
    void Update()
    {
        if (startMove) {
            AudioSource.PlayClipAtPoint(chooChoo, transform.position);

            float step = 10f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step); 
            if (Vector3.Distance(transform.position, target) < 0.2f) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
