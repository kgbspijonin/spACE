using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour {
    public float speed;
    public float damage;
    public bool followsPlayer;
    public GameObject DestructionFX;

    void Start() {
        transform.parent = GameObject.Find("Enemy Bullets").transform;
        if (followsPlayer) {
            StartCoroutine("AimAtPlayer");
        }
    }

	void Update () {
        transform.position += transform.forward * Time.deltaTime * speed;
	}

    void OnTriggerExit(Collider other) {
        if(other.transform.tag == "Border") {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.transform.tag == "Bullet") {
            GameObject fx = Instantiate(DestructionFX, transform.position, transform.rotation);
            Destroy(fx, 2.0f);
            Destroy(gameObject);
        }
    }

    IEnumerator AimAtPlayer() {
        Destroy(gameObject, 10f);
        while(true) {
            transform.LookAt(GameObject.Find("Player").transform);
            yield return new WaitForSeconds(1);
        }
    }
}
