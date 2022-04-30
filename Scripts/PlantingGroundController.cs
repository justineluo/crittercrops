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
    // public GameObject plantPrefab;
    public static int moneyCount = 0;
    public InventoryObject inventory;
    public InventoryObject waterInventory;
    public static ParticleSystem currentVFX;
    public static AudioClip currentSFX;
    bool isWatering;
    public GameObject projectilePrefab;
    public float projectileSpeed = 9f;
    public Material dryGroundMaterial;
    public Material wetGroundMaterial;
    public Animator shovelAnimator;


    public AudioSource audioSource;
    public AudioClip plantSFX;
    public AudioClip harvestSFX;

    public GameObject promptCanvas;

    bool doIHaveCurrentSeeds;
    bool doIHaveWater;
    bool isCooledDown = true;
    public static SeedObject currentSeed;

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
        doIHaveCurrentSeeds = currentSeed == null ? false : inventory.GetSeedCount(currentSeed.description) > 0;
        doIHaveWater = waterInventory.GetWaterCount() > 0;

        if (!HelpPanelBehavior.isGamePaused)
        {
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        if ((Input.GetButtonDown("Fire1") && WeaponChangeBehavior.selectedWeaponIndex == 0)
            || (Input.GetButtonDown("Fire1") && WeaponChangeBehavior.selectedWeaponIndex == 1 && doIHaveWater)
            || Input.GetButtonDown("Fire1") && WeaponChangeBehavior.selectedWeaponIndex == 2)
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
                isCooledDown = true;
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
            isCooledDown = true;
            yield return null;
        }
        isCooledDown = false;
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
            if (CheckTag(hit, "EmptyPlantingGround") && doIHaveCurrentSeeds)
            {
                UpdateReticle(reticlePlantingColor, reducedReticleSize, false);
                if (Input.GetKeyDown(KeyCode.Q))
                {

                    inventory.RemoveOneDescpItem(currentSeed.description);
                    PlantCrop(hit);
                    audioSource.PlayOneShot(plantSFX);
                }
            }
            else if (CheckTag(hit, "FullGrownPlantingGround") && WeaponChangeBehavior.selectedWeaponIndex == 2 && isCooledDown)
            {
                UpdateReticle(reticleHarvestingColor, reducedReticleSize, true);
                if (Input.GetButton("Fire1"))
                {
                    HarvestPlant(hit);
                    audioSource.PlayOneShot(harvestSFX);
                }
            }
            else if (CheckTag(hit, "FullPlantingGround") && doIHaveWater && WeaponChangeBehavior.selectedWeaponIndex == 1 && isCooledDown)
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

    bool CheckTag(RaycastHit hit, string tag)
    {

        return hit.collider.CompareTag(tag) || (hit.collider.gameObject.transform.parent && hit.collider.gameObject.transform.parent.CompareTag(tag));
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
        GameObject plant = plantingGround.transform.CompareTag("Plant") ? plantingGround.collider.gameObject : plantingGround.transform.GetChild(0).gameObject;
        int plantValue = plant.GetComponent<PlantGrowthBehavior>().moniesAmount;
        FindObjectOfType<LevelManager>().addToCurrentMoney(plantValue);

        Destroy(plant.gameObject);
        GameObject dirt = plantingGround.transform.CompareTag("Plant") ? plantingGround.collider.transform.parent.gameObject : plantingGround.collider.gameObject;
        dirt.GetComponent<MeshRenderer>().material = dryGroundMaterial;
        dirt.tag = "EmptyPlantingGround";
        // maybe add some cute effect and sound when you harvest
    }
    // Handles the action of planting a crop and any dependencies or effects that come with it

    void PlantCrop(RaycastHit plantingGround)
    {
        var plantObject = Instantiate(currentSeed.plantPrefab, plantingGround.transform.position, plantingGround.transform.rotation);
        plantObject.transform.parent = plantingGround.transform;
        plantingGround.collider.tag = "FullPlantingGround";
        // maybe add some cute effect and sound when you plant something
    }
    void WaterCrop(RaycastHit plantingGround)
    {
        GameObject plant = plantingGround.transform.CompareTag("Plant") ? plantingGround.collider.gameObject : plantingGround.transform.GetChild(0).gameObject;

        plant.GetComponent<PlantGrowthBehavior>().WaterPlantOnce(waterInventory);
        GameObject dirt = plantingGround.transform.CompareTag("Plant") ? plantingGround.collider.transform.parent.gameObject : plantingGround.collider.gameObject;
        dirt.GetComponent<MeshRenderer>().material = wetGroundMaterial;
    }
}