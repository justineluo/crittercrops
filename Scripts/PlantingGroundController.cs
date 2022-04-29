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
    public static ParticleSystem currentVFX;
    public static AudioClip currentSFX;
    bool isWatering;
    public GameObject projectilePrefab;
    public float projectileSpeed = 9f;
    public Material dryGroundMaterial;
    public Material wetGroundMaterial;


    public AudioSource audioSource;
    public AudioClip plantSFX;
    public AudioClip harvestSFX;

    public GameObject promptCanvas;

    bool doIHaveSeeds;
    bool doIHaveWater;

    Coroutine cooldownCoroutine;
    public Slider cooldownSlider;
    public float cooldownTime = 2;

    void Awake()
    {
        promptCanvas = GameObject.FindGameObjectWithTag("Prompt");
        promptCanvas.SetActive(false);
    }
    void Start()
    {
        originalReticleColor = reticleImage.color;
        audioSource = GetComponent<AudioSource>();
        if (cooldownSlider != null)
        {
            cooldownSlider.maxValue = cooldownTime;
        }
    }
    // Update is called once per frame
    void Update()
    {
        doIHaveSeeds = inventory.GetSeedCount() > 0;
        doIHaveWater = inventory.GetWaterCount() > 0;

        if (!HelpPanelBehavior.isGamePaused)
        {
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        if ((Input.GetButtonDown("Fire1") && WeaponChangeBehavior.selectedWeaponIndex == 0)
            || (Input.GetButtonDown("Fire1") && WeaponChangeBehavior.selectedWeaponIndex == 1 && doIHaveWater))
        {
            audioSource.PlayOneShot(currentSFX);
            currentVFX.Play();
            cooldownCoroutine = StartCoroutine(WeaponCooldown(cooldownTime));
        }
        else if (Input.GetButtonDown("Fire1") && WeaponChangeBehavior.selectedWeaponIndex == 1 && !doIHaveWater)
        {
            promptCanvas.SetActive(true);
            Invoke("DeactivatePromptWithDelay", 2);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            audioSource.Stop();
            currentVFX.Stop();
            if (cooldownCoroutine != null)
            {
                StopCoroutine(cooldownCoroutine);
            }
            if (cooldownSlider != null)
            {
                cooldownSlider.value = cooldownTime;
            }
        }
    }

    void DeactivatePromptWithDelay()
    {
        promptCanvas.SetActive(false);
    }

    IEnumerator WeaponCooldown(float countdown)
    {
        while (countdown > 0)
        {
            countdown -= Time.deltaTime;
            if (cooldownSlider != null)
            {
                cooldownSlider.value = countdown;
            }
            yield return null;
        }
        audioSource.Stop();
        currentVFX.Stop();
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
            else if (hit.collider.CompareTag("FullPlantingGround") && doIHaveWater && WeaponChangeBehavior.selectedWeaponIndex == 1)
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
        plantingGround.collider.gameObject.GetComponent<MeshRenderer>().material = dryGroundMaterial;
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
        plantingGround.collider.gameObject.GetComponent<MeshRenderer>().material = wetGroundMaterial;
    }
}