using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGrowthBehavior : MonoBehaviour
{

    public float startScale = 1;
    public float numOfStages = 4;
    float timeElapsed = 0;
    public float lerpDuration = 100;
    public int moniesAmount = 10;
    bool isWatered;
    void Start()
    {
        transform.localScale = Vector3.one * startScale;
    }

    void FixedUpdate()
    {
        // while the lerp hasn't finished, continue to lerp the growth value and increase the size of the
        // plant in staggered stages until it is fully grown
        if (timeElapsed < lerpDuration)
        {
            float growth = Mathf.Lerp(.0001f, 1f, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            float stage = Mathf.Ceil(growth * numOfStages);
            transform.localScale = (Vector3.one * stage * startScale);


            if (stage == numOfStages)
            {
                transform.parent.gameObject.tag = "FullGrownPlantingGround";
            }
        }

    }

    public void WaterPlantOnce(InventoryObject inventory)
    {
        if (!isWatered)
        {
            isWatered = true;
            inventory.RemoveOneWaterItem();
            lerpDuration = timeElapsed + (lerpDuration - timeElapsed) / 2;
        }
    }
}
