using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    public float maxHealth = 20000;
    public float currentHealth;
    public float randomMovement;
    public GameObject bulletPrefab;
    public float speed = 0.5f;
    public GameObject DestructionFX;
    public float FireRate;

    private GameObject border;
    private bool inPosition = false;

	void Start () {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        transform.Find("Renderer").transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        border = GameObject.Find("Worldwide Objects").transform.Find("Enemy Border").gameObject;
        currentHealth = maxHealth;
	}

	void Update () {
        if (transform.position.z > border.transform.position.z * 1) {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -100) * Time.deltaTime * 100;
        }
        else if (inPosition == false){
            StartCoroutine("LookAtPlayer");
            StartCoroutine("MoveRandomXY");
            StartCoroutine("Shoot");
            inPosition = true;
        }
	}

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 10) {
            GameObject particles = Instantiate(DestructionFX, transform.position, transform.rotation);
            Destroy(particles, 3f);
            Bullet data = other.GetComponent<BulletController>().info;
            currentHealth -= data.damage;
            if (currentHealth <= 0) {
                PlayerController.currentScore += Random.Range(5, 10) * 2;
                Destroy(gameObject);
            }
        }
    }

    IEnumerator LookAtPlayer() {
        while(true) {
            GameObject player = GameObject.Find("Player");
            transform.LookAt(player.transform);
            yield return new WaitForSeconds(.1f);
        }
    }

    IEnumerator Shoot() {
        while(true) {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.GetComponent<EnemyBulletController>().damage = 10;
            if(bullet.GetComponent<EnemyBulletController>().followsPlayer == false)
                Destroy(bullet, 5.0f);
            yield return new WaitForSeconds(Random.Range(0.75f / FireRate, 1f / FireRate));
        }
    }

    IEnumerator MoveRandomXY() {
        while(true) {
            float x = Random.Range(-randomMovement, randomMovement);
            float y = Random.Range(-randomMovement, randomMovement);
            float z;
            if(transform.position.z > border.transform.position.z * 0.5) {
                z = Random.Range(-randomMovement, randomMovement);
            }
            else {
                z = randomMovement;
            }
            GetComponent<Rigidbody>().velocity = new Vector3(x, y, z) * speed;
            yield return new WaitForSeconds(Random.Range(2.0f, 3.0f));
        }
    }
}
