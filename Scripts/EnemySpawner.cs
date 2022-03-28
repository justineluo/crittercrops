using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies;
    public float spawnTime = 7; 
    public float xMin = -35;
    public float xMax = 35;    
    public float zMin = -35;
    public float zMax = 35;
    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        InvokeRepeating("SpawnEnemies", spawnTime, spawnTime);
 
    }

    void SpawnEnemies()
    {   
        Vector3 enemyPosition;
        enemyPosition.x = Random.Range(xMin, xMax);
        enemyPosition.y = 0.5f;
        enemyPosition.z = Random.Range(zMin, zMax);
        int enemyIndex = Random.Range(0, enemies.Length);
        GameObject enemyPrefab = enemies[enemyIndex]; 
      
        GameObject spawnedEnemy = Instantiate(enemyPrefab, enemyPosition, transform.rotation)
            as GameObject;
        
        spawnedEnemy.transform.parent = gameObject.transform;
    }
    
}