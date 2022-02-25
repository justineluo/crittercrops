using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterCropBaseBehavior : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform player;
    public int critterSightRadius = 10;
    public int damageAmount = 5;
    Rigidbody rb;
    public GameObject seedPrefab;

    public int startingHealth = 30;
    int currentHealth;
    public AudioClip critterDieSFX;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        rb = gameObject.GetComponent<Rigidbody>();
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float step = moveSpeed * Time.deltaTime;
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < critterSightRadius)
        {
            transform.LookAt(player);
            Vector3 groundPlayerPosition = Vector3.Scale(player.position, new Vector3(1, 0, 1));
            transform.position = Vector3.MoveTowards(transform.position, groundPlayerPosition, step);
        }

        if (currentHealth <= 0)
        {
            CritterDies();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // TODO: do something to damage the player
            // var playerHealth = other.GetComponent<PlayerHealth>();
            // playerHealth.TakeDamage(damageAmount);

            // TODO: apply some kind of knockback ApplyKnockBack()
        }
    }

    //call this when the crittercrop runs out of health
    private void CritterDies()
    {
        // TODO: Idk add a Critter Die sounds??
        // AudioSource.PlayClipAtPoint(critterDieSFX, transform.position);
        // Maybe also add a dying animation 

        transform.Rotate(-90, 0, 0, Space.Self);
        Instantiate(seedPrefab, transform.position, transform.rotation);
        Destroy(gameObject, .5f);
    }

    private void ApplyKnockBack(Collision other)
    {
        Vector3 dir = (other.transform.position - transform.position).normalized;
        dir = Vector3.Scale(dir, new Vector3(-1, 0, -1));
        rb.AddForce(dir * 3, ForceMode.VelocityChange);
    }
}
