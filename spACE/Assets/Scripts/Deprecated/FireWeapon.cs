using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : MonoBehaviour {
    public GameObject bulletPrefab;
    public AudioSource sound;
    public float fireRate;
    public float bulletSpeed;
    private float time;

    void Start() {
        time = Time.time;
    }

    public void Fire() {
        if (Time.time > time + 1 / fireRate) {
            sound.Play();
            time = Time.time;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
        }
    }
}
