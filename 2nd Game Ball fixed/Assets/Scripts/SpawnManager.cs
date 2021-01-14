﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameManager gameManager;


    public GameObject[] enemyPrefab;
    [SerializeField] GameObject[] powerUpPrefab;

    private float spawnRangeX = 73f;
    private float spawnRangeZ = 73f;

    // Start is called before the first frame update
    void Start()
    {
        float spawnTimeEnemy1 = 3f;
        float repeatTimeEnemy1 = Random.Range(5f, 10f);
        float spawnTimeEnemy2 = 30f;
        float repeatTimeEnemy2 = Random.Range(7f, 13f);

        float spawnTimePowerUp = Random.Range(3f, 5f);
        float repeatTimePowerUp = Random.Range(5f, 7f);

        InvokeRepeating("SpawnEnemy", spawnTimeEnemy1, repeatTimeEnemy1);
        InvokeRepeating("SpawnPowerUp", spawnTimePowerUp, repeatTimePowerUp);
        InvokeRepeating("SpawnEnemyShooter", spawnTimeEnemy2, repeatTimeEnemy2);


        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRangeX, spawnRangeX);
        float spawnPosZ = Random.Range(-spawnRangeZ, spawnRangeZ);

        Vector3 randomPos = new Vector3(spawnPosX, 1.2f, spawnPosZ);
        return randomPos;
    }

    private void SpawnEnemy()
    {
        if (gameManager.isGameOver == false)
        {
            Instantiate(enemyPrefab[0], GenerateSpawnPosition(), enemyPrefab[0].transform.rotation);
        }
    }

    private void SpawnPowerUp()
    {
        if (gameManager.isGameOver == false)
        {
            int powerUpRange = Random.Range(0, powerUpPrefab.Length);
            Instantiate(powerUpPrefab[powerUpRange], GenerateSpawnPosition(), powerUpPrefab[powerUpRange].transform.rotation);
        }
    }

    private void SpawnEnemyShooter()
    {
        if (gameManager.isGameOver == false)
        {
            Instantiate(enemyPrefab[1], GenerateSpawnPosition(), enemyPrefab[1].transform.rotation);
        }
    }
}
