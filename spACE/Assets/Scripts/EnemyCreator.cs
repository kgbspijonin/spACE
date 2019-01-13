using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour {
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public List<GameObject> enemies = new List<GameObject>();
    public float offset;
    public float fireRate;
    public float asteroidSpeed;
    public GameObject bossPrefab;
    public static GameObject boss;
    public float bossSpawnCoordinates = 1700;
    private float time;
    private float nextShot;

    void Start() {
        if(enemy1 != null)
            enemies.Add(enemy1);
        if (enemy2 != null)
            enemies.Add(enemy2);
        if (enemy3 != null)
            enemies.Add(enemy3);
        time = Time.time;
        StartCoroutine("CreateEnemy");
        Invoke("CreateBoss", 60.0f);
    }

    IEnumerator CreateEnemy() {
        while(true) {
            Vector3 spawnPoint = transform.position;
            spawnPoint.x = Random.Range(-offset, offset);
            spawnPoint.x = Random.Range(-offset, offset);
            int obj = Random.Range(0, enemies.Count);
            GameObject enemy = Instantiate(enemies[obj], spawnPoint * 2, Quaternion.identity);
            enemy.transform.name = enemies[obj].transform.name;
            enemy.transform.parent = GameObject.Find("Enemies").transform;
            yield return new WaitForSeconds(1 / fireRate);
        }
    }

    public void CreateBoss() {
        boss = Instantiate(bossPrefab, new Vector3(0, 0, bossSpawnCoordinates), Quaternion.identity);
    }
}
