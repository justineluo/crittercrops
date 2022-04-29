using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGrowthBehavior : MonoBehaviour
{

    public Vector3 startVectorScale;
    float scale = .25f;
    public float numOfStages = 4;
    float timeElapsed = 0;
    public float lerpDuration = 100;
    public int moniesAmount = 10;
    bool isWatered;
    void Start()
    {
        startVectorScale = transform.localScale;
        transform.localScale = startVectorScale * scale;
    }

    void FixedUpdate()
    {
        // while the lerp hasn't finished, continue to lerp the growth value and increase the size of the
        // plant in staggered stages until it is fully grown
        if (timeElapsed < lerpDuration && isWatered)
        {
            float growth = Mathf.Lerp(.0001f, 1f, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            float stage = Mathf.Ceil(growth * numOfStages);
            transform.localScale = (startVectorScale * stage * scale);

            print(stage);

            if (stage == numOfStages)
            {
                transform.parent.gameObject.tag = "FullGrownPlantingGround";

                print(stage);
            }
        }

    }

    public void WaterPlantOnce(InventoryObject inventory)
    {
        if (!isWatered)
        {
            isWatered = true;
            StartCoroutine(RemoveWater(inventory));
        }
    }

    // removes water from inventory after 2 second watering period
    IEnumerator RemoveWater(InventoryObject inventory)
    {
        yield return new WaitForSeconds(2f);
        inventory.RemoveOneWaterItem();
    }
}
