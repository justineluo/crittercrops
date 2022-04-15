using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public float spawnTime = 7; 
    public float xMin = -6;
    public float xMax = 6;    
    public float zMin = -6;
    public float zMax = 6;
    public int maxEnemies = 10; // max number of enemies in region at a time
    public float spawnRate = 5f;
    float elapsedTime = 0f;

    GameObject[] enemiesInScene;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("enemyspawner at " + transform.position);

    }

    void Update()
    {

        enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemiesInScene.Length < maxEnemies)
        {
            SpawnEnemies();
        }
        elapsedTime += Time.deltaTime;


    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow cube at the transform position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(5, 5, 5));
    }

    void SpawnEnemies()
    {  
        if(elapsedTime >= spawnRate)
            {
                elapsedTime = 0.0f;
                Vector3 enemyPosition = transform.position;
                enemyPosition.x += Random.Range(xMin, xMax);

                enemyPosition.y = 0.5f;
                enemyPosition.z += transform.position.x + Random.Range(zMin, zMax);
                Debug.Log("enemyPosition at " + enemyPosition);

                int enemyIndex = Random.Range(0, 2);
                GameObject enemyPrefab;
                if (enemyIndex == 0) {
                    enemyPrefab = enemyPrefab1;
                } else {
                    enemyPrefab = enemyPrefab2;
                } 
            
                GameObject spawnedEnemy = Instantiate(enemyPrefab, enemyPosition, transform.rotation)
                    as GameObject;
                
                spawnedEnemy.transform.parent = gameObject.transform;
        
        } 
       
    }
    
}
