using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugSprayHitBehavior : MonoBehaviour
{
    public GameObject bugSpray;
    public int bugSprayDamage = 1;
    public int startingHealth = 300;
    int currentHealth;

    void Start()
    {
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Projectile")) {
            TakeDamage();
        }
    }
    public void TakeDamage() {
        if (currentHealth > 0) {
            currentHealth -= bugSprayDamage;
        } 
        if (currentHealth <= 0) {
            CritterCropDies();
        }
        Debug.Log("Current health: " + currentHealth);
    }

    void CritterCropDies() {
        //TODO: critter crop drops seeds
        Debug.Log("RIP mr. crittercrop");
        Destroy(gameObject);
    }
}
