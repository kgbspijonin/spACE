using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbleAsteroid : MonoBehaviour {
    public float tumble;
    public float speed = 10;
    void Start() {
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
        transform.LookAt(new Vector3(-30, 0, transform.position.z));
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed);
    }
}
