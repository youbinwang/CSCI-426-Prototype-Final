using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class EnemySpawner : MonoBehaviour
{
    [Header("Beat Track")]
    [SerializeField] public KoreographyTrack trackName;

    [Header("Enemy List")]
    [SerializeField] public GameObject[] enemyList;

    [Header("Enemy Random Size")]
    [SerializeField] public Vector2 scaleRange = new Vector2(0.5f, 4f);

    [Header("Enemy Spawn Time")]
    [SerializeField] public int spawnBeat = 4;
    private int timer;
    private int enemyIndex;

    // Start is called before the first frame update
    void Start()
    {
        Koreographer.Instance.RegisterForEvents(trackName.EventID, EnemyAction);

        spawnBeat--;
        timer = spawnBeat;
        enemyIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        /*timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnEnemy();
        }*/
    }

    void EnemyAction(KoreographyEvent koreoEvent)//When On the Beat
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
        if (enemyList.Length == 0)
        {
            Debug.LogWarning("No enemy types specified.");
            return;
        }

        float randomScale = Random.Range(scaleRange.x, scaleRange.y);
        Vector3 spawnScale = new(randomScale, 1, randomScale);

        GameObject enemyPrefab = enemyList[enemyIndex];
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        enemy.transform.localScale = spawnScale;

        if (enemyIndex < enemyList.Length - 1)
        {
            enemyIndex++;
        }
        else
        {
            enemyIndex = 0;
        }

    }
}
