using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public Transform spawnPoint; // 적 AI를 소환할 위치들

    private List<Enemy> enemies = new List<Enemy>(); // 생성된 적들을 담는 리스트
    private int currunetWave; // 현재 웨이브
    private float lastSpawnTime;
    private int enemiesSpawned;

    private void Start()
    {
        currunetWave = GameManager.instance.currentlevel - 1;
    }

    private void Update() {
        if (!UIMGR.instance.loadingend)
        {
            lastSpawnTime = Time.time;
        }
        else
        {
            if (currunetWave < GameManager.instance.waves.Length)
            {
                // 현재 웨이브에 맞춰 적을 생성
                if (UIMGR.instance.loadingend && ((enemiesSpawned == 0 && Time.time > lastSpawnTime + GameManager.instance.waves[currunetWave].waveInterval) || (enemiesSpawned != 0 && enemiesSpawned < GameManager.instance.waves[currunetWave].maxEnemies && Time.time >= lastSpawnTime + GameManager.instance.waves[currunetWave].spawnInterval)))
                {
                    int id = GameManager.instance.waves[currunetWave].zombieList[Random.Range(0, GameManager.instance.waves[currunetWave].zombieList.Length)];
                    CreateEnemy(id);

                    lastSpawnTime = Time.time;
                    enemiesSpawned++;
                }
                if (enemiesSpawned == GameManager.instance.waves[currunetWave].maxEnemies && GameObject.FindGameObjectWithTag("Enemy") == null)
                {
                    currunetWave++;
                    enemiesSpawned = 0;
                    lastSpawnTime = Time.time;
                    GameManager.instance.AddZen(currunetWave);
                }
            }
        }
    }

    // 적을 생성
    private void CreateEnemy(int id) {
        Enemy enemy = Instantiate(GameManager.instance.zombies[id].enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        float leveleffect = 1f+(GameManager.instance.currentlevel-1)*0.1f;

        enemy.Setup(GameManager.instance.zombies[id].health * leveleffect, GameManager.instance.zombies[id].damage * leveleffect, -GameManager.instance.zombies[id].speed, GameManager.instance.zombies[id].timeBetAttack);

        enemies.Add(enemy);
        Debug.Log(enemy.startingHealth);
        enemy.onDeath += () => enemies.Remove(enemy);
        enemy.onDeath += () => Destroy(enemy.gameObject, 3f);
        enemy.onDeath += () => GameManager.instance.AddCoin((int) enemy.startingHealth);
        float zenSpawn = Random.Range(0.0f, 100.0f);
        if (zenSpawn < GameManager.instance.zombies[id].zenSpawnRate)
        {
            enemy.onDeath += () => GameManager.instance.AddZen(1);
        }
    }
}