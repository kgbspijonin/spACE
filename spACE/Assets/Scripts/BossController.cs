using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {
    BossWeaponController[] weapons;
    static public float maxHealth = 1000000;
    static public float currentHealth;
    public GameObject DestructionFX;

    void OnTriggerEnter(Collider other) {
        if(other.transform.tag == "Bullet") {
            currentHealth -= other.GetComponent<BulletController>().info.damage;
            GameObject particles = Instantiate(DestructionFX, other.transform.position, other.transform.rotation);
            PlayerController.currentScore += other.GetComponent<BulletController>().info.damage * 0.01f;
            Destroy(particles, 5.0f);
            if (currentHealth <= 0) {
                currentHealth = 0;
                PlayerController.hasDefeatedBoss = true;
                for (int i = 0; i < 10; i++) {
                    particles = Instantiate(DestructionFX, new Vector3(
                        Random.Range(-100, 100),
                        Random.Range(-100, 100),
                        transform.position.z - 200
                        ), other.transform.rotation);
                    Destroy(particles, 5.0f);
                }
            }
        }
    }

    void Start() {
        currentHealth = maxHealth;
        weapons = transform.GetComponentsInChildren<BossWeaponController>();
        foreach(BossWeaponController weapon in weapons) {
            weapon.StartCoroutine("Shoot");
            weapon.StartCoroutine("LookAtPlayer");
        }
    }

    void Update() {
        transform.parent.GetComponent<Rigidbody>().velocity = transform.forward * Time.deltaTime * 500;
    }
}
