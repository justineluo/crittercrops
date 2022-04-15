using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantingGroundController : MonoBehaviour
{
    public Image reticleImage;
    public Color reticlePlantingColor;
    public Color reticleHarvestingColor;
    public Color reticleWaterColor;
    Color originalReticleColor;
    public float reticleChangeSpeed = 2f;
    public GameObject plantPrefab;
    public static int moneyCount = 0;
    public InventoryObject inventory;


    public ParticleSystem bugSprayVFX;
    public ParticleSystem wateringVFX;
    bool isWatering;
    public GameObject projectilePrefab;
    public float projectileSpeed = 9f;


    public AudioSource audioSource;
    public AudioClip plantSFX;
    public AudioClip harvestSFX;
    public AudioClip spraySFX;

    void Start()
    {
        originalReticleColor = reticleImage.color;
        audioSource = GetComponent<AudioSource>();

        wateringVFX.Stop();
        bugSprayVFX.Stop();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            audioSource.PlayOneShot(spraySFX);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            audioSource.Stop();
        }

        if (isWatering)
        {
            fireWatering();
        }
        else
        {
            fireBugSpray();
        }
    }

    void fireBugSpray()
    {
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
    void fireWatering()
    {
        if (Input.GetButton("Fire1"))
        {
            wateringVFX.Play();
        }
        else
        {
            wateringVFX.Stop();
        }
    }

    void FixedUpdate()
    {
        ReticleEffect();
    }

    // Maintained planting ground invariant: 
    // state: EmptyPlantingGround, there is no plant present
    // state: FullPlantingGround, there is a plant that is not ready to harvest
    // state: FullGrownPlantingGround, there is a plant ready to harvest
    // Use Q and R to plant and harvest the plants respectively
    void ReticleEffect()
    {
        bool doIHaveSeeds = inventory.GetSeedCount() > 0;
        bool doIHaveWater = inventory.GetWaterCount() > 0;
        RaycastHit hit;
        Vector3 reducedReticleSize = new Vector3(.7f, .7f, 1);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5))
        {

            if (hit.collider.CompareTag("EmptyPlantingGround") && doIHaveSeeds)
            {
                UpdateReticle(reticlePlantingColor, reducedReticleSize, false);
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    inventory.RemoveAnyOneSeedItem();
                    PlantCrop(hit);
                    audioSource.PlayOneShot(plantSFX);
                }
            }
            else if (hit.collider.CompareTag("FullGrownPlantingGround"))
            {
                UpdateReticle(reticleHarvestingColor, reducedReticleSize, true);
                if (Input.GetKeyDown(KeyCode.R))
                {
                    HarvestPlant(hit);
                    audioSource.PlayOneShot(harvestSFX);
                }
            }
            else if (hit.collider.CompareTag("FullPlantingGround") && doIHaveWater)
            {
                UpdateReticle(reticleWaterColor, reducedReticleSize, false);
                if (Input.GetButton("Fire1"))
                {
                    WaterCrop(hit);
                }
            }
            else
            {
                UpdateReticle(originalReticleColor, Vector3.one, false);
            }
        }
    }

    // Sets the reticle color and size from what it currently is to the given color and size
    // the lerp speed is determined by `reticleChangeSpeed`
    void UpdateReticle(Color color, Vector3 size, bool isWateringState)
    {
        var lerpSpeed = Time.deltaTime * reticleChangeSpeed;
        reticleImage.color = Color.Lerp(reticleImage.color, color, lerpSpeed);
        reticleImage.transform.localScale = Vector3.Lerp(reticleImage.transform.localScale, size, lerpSpeed);
        isWatering = isWateringState;
    }

    // Handles the action of harvesting a plant and any dependencies or effetcs  that come with it
    void HarvestPlant(RaycastHit plantingGround)
    {
        int plantValue = plantingGround.transform.GetChild(0).gameObject.GetComponent<PlantGrowthBehavior>().moniesAmount;
        FindObjectOfType<LevelManager>().addToCurrentMoney(plantValue);
        Destroy(plantingGround.transform.GetChild(0).gameObject);
        plantingGround.collider.tag = "EmptyPlantingGround";
        // maybe add some cute effect and sound when you harvest
    }
    // Handles the action of planting a crop and any dependencies or effects that come with it

    void PlantCrop(RaycastHit plantingGround)
    {
        var plantObject = Instantiate(plantPrefab, plantingGround.transform.position, plantingGround.transform.rotation);
        plantObject.transform.parent = plantingGround.transform;
        plantingGround.collider.tag = "FullPlantingGround";
        // maybe add some cute effect and sound when you plant something
    }
    void WaterCrop(RaycastHit plantingGround)
    {
        plantingGround.transform.GetChild(0).gameObject.GetComponent<PlantGrowthBehavior>().WaterPlantOnce(inventory);
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