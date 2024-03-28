using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState
    {
        SPAWNING, WAITING, COUNTING
    }
    public Wave[] waves;
    public int nextWave = 0;
    Wave wave;

    public List<Transform> enemies = new List<Transform>();

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    public float waveCountDown;

    private float searchCountDown = 1f;


    public SpawnState state = SpawnState.COUNTING;

    private void Start()
    {
        waveCountDown = timeBetweenWaves;
    }

    private void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();

                return;
            }
            else
            {
                return;
            }
        }

        if (waveCountDown <= 0 && state != SpawnState.SPAWNING)
        {
            StartCoroutine(SpawnWave(waves[nextWave]));

        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }

    }


    IEnumerator SpawnWave(Wave _wave)
    {

        state = SpawnState.SPAWNING;


        for (int i = 0; i < _wave.count; i++)
        {


            SpawnEnemy(_wave);

            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;

        yield break;
    }



    bool EnemyIsAlive()
    {
        searchCountDown -= Time.deltaTime;
        if (searchCountDown <= 0f)
        {
            searchCountDown = 1f;

            // Find all active enemy objects in the scene by tag
            GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy"); // Ensure "Enemy" is the correct tag

            if (allEnemies.Length == 0 && enemies.Count == 0)
            {
                return false; // No enemies are alive if both the specific list and scene-wide search are empty
            }
        }
        return true; // Enemies are still alive
    }

    void WaveCompleted()
    {
        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;

        // Increase enemy stats for the new loop and reset wave if needed
        if (nextWave + 1 > waves.Length - 1)
        {
            Debug.Log("All waves completed - Looping");
            //Increase each wave + 5 enemies
            for (int i = 0; i < waves.Length; i++)
            {
                waves[i].count += 10;
            }
            nextWave = 0;
        }
        else
        {
            nextWave++;
        }

    }


    public void SpawnEnemy(Wave _wave)
    {
        int randEnemy = Random.Range(0,_wave.enemies.Length);
        int randSpawnPoint = Random.Range(0,spawnPoints.Length);
        Instantiate(_wave.enemies[randEnemy],spawnPoints[randSpawnPoint].transform.position,Quaternion.identity);
    }


    [System.Serializable]
    public class Wave
    {

        public string name;
        public Transform[] enemies;
        public int count;
        public float rate;


    }
}