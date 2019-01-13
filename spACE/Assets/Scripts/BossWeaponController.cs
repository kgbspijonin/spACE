using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeaponController : MonoBehaviour {
    public GameObject bulletPrefab;
    public float fireRate;

    IEnumerator Shoot() {
        while(true) {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.GetComponent<EnemyBulletController>().damage = 4f;
            yield return new WaitForSeconds(Random.Range(0.75f / fireRate, 1f / fireRate));
        }
    }

    IEnumerator LookAtPlayer() {
        while (true) {
            GameObject player = GameObject.Find("Player");
            transform.LookAt(player.transform);
            yield return new WaitForSeconds(.1f);
        }
    }
}
