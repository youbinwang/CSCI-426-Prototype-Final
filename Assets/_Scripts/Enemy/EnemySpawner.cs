using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class EnemySpawner : MonoBehaviour
{
    [Header("Beat Track")]
    [SerializeField] public KoreographyTrack trackName;

    [Header("Spawn Points")]
    [SerializeField] public List<GameObject> spawnPoints;
    
    [Header("Enemy List")]
    [SerializeField] public List<GameObject> meleeEnemies;
    [SerializeField] public List<GameObject> rangedEnemies;
    
    [Header("Ranged Enemy Probability")]
    [SerializeField] public float rangedProbability = 0.5f; 

    [Header("Enemy Random Size")]
    [SerializeField] public Vector2 scaleRange = new Vector2(0.5f, 4f);

    [Header("Enemy Spawn Time")]
    [SerializeField] public int spawnBeat = 4;
    
    private int timer;
    
    
    void Start()
    {
        Koreographer.Instance.RegisterForEvents(trackName.EventID, EnemyAction);

        spawnBeat--;
        timer = spawnBeat;
    }
    
    void Update()
    {
        /*timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnEnemy();
        }*/
    }

    void EnemyAction(KoreographyEvent koreoEvent) //Loop When On the Beat
    {
        if (timer == spawnBeat)
        {
            timer = 0;
            SpawnEnemy();
        }
        else
        {
            timer++;
        }
    }

    private void SpawnEnemy()
    {
        if (meleeEnemies.Count == 0 && rangedEnemies.Count == 0)
        {
            Debug.LogWarning("No enemy types specified.");
            return;
        }

        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points specified.");
            return;
        }

        //Spawn Points
        int spawnIndex = Random.Range(0, spawnPoints.Count);
        GameObject spawnPoint = spawnPoints[spawnIndex];

        //Ranged Enemy Spawn Rate
        GameObject enemyPrefab = Random.Range(0f, 1f) < rangedProbability ? GetRandomEnemy(rangedEnemies) : GetRandomEnemy(meleeEnemies);

        float randomScale = Random.Range(scaleRange.x, scaleRange.y);
        Vector3 spawnScale = new(randomScale, 1, randomScale);

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);
        enemy.transform.localScale = spawnScale;

        enemy.transform.localScale = spawnScale;
    }

    private GameObject GetRandomEnemy(List<GameObject> enemyList)
        {
            if (enemyList.Count == 0)
                return null;
        
            int index = Random.Range(0, enemyList.Count);
            return enemyList[index];
        }

    }
