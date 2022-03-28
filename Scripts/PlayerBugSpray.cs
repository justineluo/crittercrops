using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBugSpray : MonoBehaviour
{
    public ParticleSystem bugSprayVFX;
    public GameObject projectilePrefab;
    public float projectileSpeed = 9f;
    public AudioClip spraySFX;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            audioSource.PlayOneShot(spraySFX);
        }

        if (Input.GetButtonUp("Fire1")) {
            audioSource.Stop();
        }
        
        if (Input.GetButton("Fire1"))
        {
            bugSprayVFX.Play();
            Shoot();
        }
        else
        {
            bugSprayVFX.Stop();
        }
    }

    void Shoot()
    {
        float straightX = transform.forward.x;
        float straightZ = transform.forward.z;

        //forward
        ShootHelper(transform.forward);

        // 45 degree from forward
        Vector3 projectileAngle2 = new Vector3(straightX * .707f - straightZ * .707f, 0, straightX * .707f + straightZ * .707f);
        ShootHelper(projectileAngle2);

        // 22.5 degree from forward
        Vector3 projectileAngle3 = new Vector3(straightX * .92f - straightZ * .38f, 0, straightX * .38f + straightZ * .92f);
        ShootHelper(projectileAngle3);

        // -45 degree from forward
        Vector3 projectileAngle4 = new Vector3(straightX * .707f + straightZ * .707f, 0, straightX * -.707f + straightZ * .707f);
        ShootHelper(projectileAngle4);

        // -22.5 degree from forward
        Vector3 projectileAngle5 = new Vector3(straightX * .92f + straightZ * .38f, 0, straightX * -.38f + straightZ * .92f);
        ShootHelper(projectileAngle5);
    }

    private void ShootHelper(Vector3 projectileAngle)
    {
        GameObject particles = GameObject.FindGameObjectWithTag("ParticleSystem");
        GameObject projectile = Instantiate(projectilePrefab, particles.transform.position + transform.forward, transform.rotation);
        Rigidbody _rb = projectile.GetComponent<Rigidbody>();
        _rb.AddForce(projectileAngle * projectileSpeed, ForceMode.VelocityChange);
    }
}