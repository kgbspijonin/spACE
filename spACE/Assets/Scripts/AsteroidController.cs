using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour {
    public float speed;
    public float health = 5000;
    public float speedMultiplier;
    public AudioSource destroySound;
    public float DestructionFXDuration = 0.5f;
    public float offset = 90;
    public GameObject DestructionFX;
    
    private float currentHealth;
    private Vector3 currentScale;
    private float TargetX;
    private float TargetY;

    void Start() {
        health = 5000;
        currentHealth = 5000;
        TargetX = Random.Range(-offset, offset);
        TargetY = Random.Range(-offset, offset);
        GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere.normalized * 2;
    }

    void Update() {
        
        GetComponent<Rigidbody>().position = Vector3.MoveTowards(transform.position, 
            new Vector3(TargetX, TargetY, -500), 
            Time.deltaTime * (1+ speedMultiplier) * 200);
        if(transform.position.z < -400) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision) {
        if(collision.collider.transform.gameObject.layer != 10) {
            return;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.transform.gameObject.layer != 10) {
            return;
        }
        currentHealth -= other.GetComponent<BulletController>().info.damage;
        ReceiveHit(other.gameObject);
        other.gameObject.GetComponent<BulletController>().info.pierce--;
        if(other.gameObject.GetComponent<BulletController>().info.pierce <= 0) {
            Destroy(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other) {
        if(other.transform.name == "World Border") {
            Destroy(gameObject);
        }
    }

    public void ReceiveHit(GameObject damageDealer) {
        if (DestructionFX != null) {
            GameObject spawnedFX = Instantiate(DestructionFX, transform);
            Destroy(spawnedFX, DestructionFXDuration);
        }
        if (currentHealth <= 2000) {
            PlayerController.currentScore += Random.Range(1, 3) * 1;
            GameObject spawnedFX = Instantiate(DestructionFX, transform.position, transform.rotation);
            Destroy(spawnedFX, DestructionFXDuration);
            Destroy(gameObject, DestructionFXDuration);
        } 
        float v = Mathf.Sqrt(Mathf.Abs(10000 * (currentHealth / health)));
        if(!float.IsNaN(v)) {
            transform.localScale = new Vector3(v, v, v);
        }      
        if(v <= 30) {
            Destroy(gameObject);
        }
    }
}
