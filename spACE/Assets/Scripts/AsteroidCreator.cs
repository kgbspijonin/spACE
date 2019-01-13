using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCreator : MonoBehaviour {
    public GameObject asteroid1;
    public GameObject asteroid2;
    public GameObject asteroid3;
    public List<GameObject> asteroids = new List<GameObject>();

    public float offset;
    public float fireRate;
    public float asteroidSpeed;
    public float speedMultiplier;
    private float time;
    private float nextShot;
    void Start () {
        asteroids.Add(asteroid1);
        asteroids.Add(asteroid2);
        asteroids.Add(asteroid3);
        time = Time.time;
	}

	void Update () {
		if(Time.time > time + 1 / fireRate) {
            time = Time.time;
            Vector3 spawnPoint = transform.position;
            spawnPoint.z = transform.localScale.y;
            spawnPoint.x = Random.Range(-offset, offset);
            spawnPoint.y = Random.Range(-offset, offset);
            int obj = Random.Range(0, asteroids.Count);
            GameObject created = Instantiate(asteroids[obj], spawnPoint, Quaternion.identity);
            created.transform.LookAt(new Vector3(
                Random.Range(-offset, offset), 
                Random.Range(-offset, offset), 
                -100));
            created.transform.localScale = new Vector3(100, 100, 100);
            created.GetComponent<AsteroidController>().speedMultiplier = speedMultiplier;
            speedMultiplier += 0.005f;
            fireRate += 0.005f;
            created.transform.parent = GameObject.Find("Asteroids").transform;
        }
	}
}
