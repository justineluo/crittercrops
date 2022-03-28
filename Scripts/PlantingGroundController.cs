using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantingGroundController : MonoBehaviour
{
    public Image reticleImage;
    public Color reticlePlantingColor;
    public Color reticleHarvestingColor;
    Color originalReticleColor;
    public GameObject plantPrefab;
    public float reticleChangeSpeed = 2f;

    public static int moneyCount = 0;
    public InventoryObject inventory;
    public AudioSource audioSource;
    public AudioClip plantSFX;
    public AudioClip harvestSFX;
    void Start()
    {
        originalReticleColor = reticleImage.color;
        audioSource = GetComponent<AudioSource>();
    }
    void FixedUpdate()
    {
        ReticleEffect();
    }

    // Maintained planting ground invariant: 
    // state: EmptyPlantingGround, there is no plant present
    // state: FullPlantingGround, there is a plant that is not ready to harvest
    // state: FullGrownPlantingGround, there is a plant ready to harvest
    // Use P and H to plant and harvest the plants respectively
    void ReticleEffect()
    {
        bool doIHaveSeeds = inventory.GetSeedCount() > 0;
        RaycastHit hit;
        Vector3 reducedReticleSize = new Vector3(.7f, .7f, 1);
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("EmptyPlantingGround"))
            {
                if (doIHaveSeeds)
                {
                    SetReticleSizeAndColor(reticlePlantingColor, reducedReticleSize);
                    if (Input.GetKeyDown(KeyCode.P))
                    {
                        inventory.RemoveAnyOneSeedItem();
                        PlantCrop(hit);
                        audioSource.PlayOneShot(plantSFX);
                    }
                }
            }
            else if (hit.collider.CompareTag("FullGrownPlantingGround"))
            {
                SetReticleSizeAndColor(reticleHarvestingColor, reducedReticleSize);
                if (Input.GetKeyDown(KeyCode.H))
                {
                    HarvestPlant(hit);
                    audioSource.PlayOneShot(harvestSFX);
                }
            }
            else
            {
                SetReticleSizeAndColor(originalReticleColor, Vector3.one);
            }
        }
    }

    // Sets the reticle color and size from what it currently is to the given color and size
    // the lerp speed is determined by `reticleChangeSpeed`
    void SetReticleSizeAndColor(Color color, Vector3 size)
    {
        var lerpSpeed = Time.deltaTime * reticleChangeSpeed;
        reticleImage.color = Color.Lerp(reticleImage.color, color, lerpSpeed);
        reticleImage.transform.localScale = Vector3.Lerp(reticleImage.transform.localScale, size, lerpSpeed);

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

}
