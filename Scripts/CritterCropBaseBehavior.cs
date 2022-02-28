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

    public int bugSprayDamage = 1;
    public int startingHealth = 300;
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
            Vector3 groundedPlayerPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
            rb.MovePosition(Vector3.MoveTowards(transform.position, groundedPlayerPosition, step));
        }
        else
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
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

            ApplyKnockBack(other.transform);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Projectile")) {
            TakeDamage();
        }
    }
    
    //call this when crittercrop is hit by bug spray
    public void TakeDamage() {
        if (currentHealth > 0) {
            currentHealth -= bugSprayDamage;
        } 
        if (currentHealth <= 0) {
            CritterDies();
        }
        Debug.Log("Current health: " + currentHealth);
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

    private void ApplyKnockBack(Transform other)
    {
        Vector3 dir = (other.position - transform.position).normalized;
        dir = new Vector3(dir.x, 0, dir.z);
        StartCoroutine(KnockBackCoroutine(dir));
    }
    IEnumerator KnockBackCoroutine(Vector3 direction)
    {
        float timeleft = .3f;
        Debug.Log(direction * 5);
        while (timeleft > 0)
        {

            transform.Translate(direction * Time.deltaTime * 5);
            timeleft -= Time.deltaTime;

            yield return null;
        }
    }
}
