using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellSpawner : MonoBehaviour
{
    public GameObject wellPrefab;
    public float spawnRate = 20f;
    public float spawnX = 0;
    public float spawnY = 0;
    public float spawnZ = 25.8f;
    float elapsedTime = 0f;

    GameObject wellInScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        wellInScene = GameObject.FindGameObjectWithTag("Well");
        if(wellInScene == null)
        {
            elapsedTime += Time.deltaTime;
            SpawnWell();
        }
    }

    void SpawnWell()
    {  
        if(elapsedTime >= spawnRate) {
            elapsedTime = 0.0f;
        
            GameObject spawnedWell = Instantiate(wellPrefab, new Vector3(spawnX, spawnY, spawnZ), transform.rotation)
                as GameObject;
            
            spawnedWell.transform.parent = gameObject.transform;
        } 
       
    }
}
