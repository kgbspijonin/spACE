using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    public Bullet info = new Bullet();
    public bool followsEnemy = false;
    public GameObject enemy;

    void Start() {
        if (followsEnemy) {
            enemy = GameObject.FindGameObjectWithTag("Enemy");
            StartCoroutine("AimAtEnemy");
        }
        Bullet mainInfo = new Bullet();
        StaticInfo.lasers.TryGetValue(transform.name, out mainInfo);
        info.color = mainInfo.color;
        info.damage *= mainInfo.damage;
        info.pierce = mainInfo.pierce;
        info.speed *= mainInfo.speed;
    }

    void Update() {
        transform.position += transform.forward * info.speed * Time.deltaTime;
    }

    void OnTriggerExit(Collider other) {
        if(other.transform.name == "World Border")
        Destroy(gameObject, 1.0f);
    }

    IEnumerator AimAtEnemy() {
        while(true) {
            transform.LookAt(enemy.transform);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
