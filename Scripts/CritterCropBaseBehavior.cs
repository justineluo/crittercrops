using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public AudioSource audioSource;
    public AudioClip critterDieSFX;

    public GameObject critterDieVFX;
    public AudioClip noticePlayerSFX;
    public float attackRate = 3f;
    float elapsedTime = 0f;

    bool isNewSighting = true;
    public Slider healthSlider;
    // Start is called before the first frame update
    void Awake()
    {
        healthSlider = GetComponentInChildren<Slider>();
    }
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        rb = gameObject.GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float step = moveSpeed * Time.deltaTime;

        Vector3 groundedPlayerPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        float groundedDistance = Vector3.Distance(transform.position, groundedPlayerPosition);
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < critterSightRadius && groundedDistance > 1 * transform.localScale.x)
        {
            if (isNewSighting)
            {
                AudioSource.PlayClipAtPoint(noticePlayerSFX, Camera.main.transform.position);
                isNewSighting = false;
            }

            var height = transform.lossyScale.y;
            transform.LookAt(player);

            transform.position = Vector3.MoveTowards(transform.position, groundedPlayerPosition, step);
        }
        else
        {
            // rb.velocity = Vector3.zero;
            // rb.angularVelocity = Vector3.zero;
            isNewSighting = true;
        }

        if (currentHealth <= 0)
        {
            CritterDies();
        }
        elapsedTime += Time.deltaTime;

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // TODO: do something to damage the player

            // only attacks every 5 seconds
            if (elapsedTime >= attackRate)
            {
                var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
                playerHealth.TakeDamage(damageAmount);
                elapsedTime = 0.0f;
            }

            // ApplyKnockBack(other.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile") && WeaponChangeBehavior.selectedWeaponIndex == 0)
        {
            TakeDamage();
        }
    }

    //call this when crittercrop is hit by bug spray
    public void TakeDamage()
    {
        if (currentHealth > 0)
        {
            currentHealth -= bugSprayDamage;
            healthSlider.value = currentHealth;
        }
        if (currentHealth <= 0)
        {
            CritterDies();
        }
    }

    //call this when the crittercrop runs out of health
    private void CritterDies()
    {
        audioSource.PlayOneShot(critterDieSFX, 0.2f);
        // Maybe also add a dying animation 

        transform.Rotate(-90, 0, 0, Space.Self);
        Destroy(gameObject, .5f);
    }

    private void OnDestroy()
    {
        if (!LevelManager.isGameOver)
        {
            Vector3 effectPosition = transform.position;
            effectPosition.y = transform.position.y + 1;
            Instantiate(critterDieVFX, effectPosition, transform.rotation);
            Instantiate(seedPrefab, new Vector3(transform.position.x, .75f, transform.position.z), transform.rotation);

        }
    }

    private void ApplyKnockBack(Transform other)
    {
        Vector3 dir = (other.position - transform.position).normalized;
        dir = new Vector3(dir.x, 0, dir.z);
        StartCoroutine(KnockBackCoroutine(dir));
    }
    IEnumerator KnockBackCoroutine(Vector3 direction)
    {
        float timeleft = .5f;
        while (timeleft > 0)
        {

            transform.Translate(direction * Time.deltaTime * 5);
            timeleft -= Time.deltaTime;

            yield return null;
        }
    }
}
