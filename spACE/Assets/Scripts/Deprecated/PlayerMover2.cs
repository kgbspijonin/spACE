using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover2 : MonoBehaviour {
    public float speedHorizontal = 10;
    public float speedVertical = 10;
    public float minX = 20;
    public float minZ = 10;
    public float tilt = 2;

    void Start() {

    }

    void Update() {
        if (Input.GetButton("Fire1")) {
            Component [] scripts = transform.GetChild(0).gameObject.GetComponentsInChildren<WeaponController>();
            foreach(WeaponController weapon in scripts) {
                weapon.Shoot();
            }
        }
    }

    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis("Horizontal") * speedHorizontal;
        float moveVertical = Input.GetAxis("Vertical") * speedVertical;

        Vector3 movement = new Vector3(
            //Mathf.Clamp(
                moveHorizontal,
                // -minX, minX),
            0,
            //Mathf.Clamp(
                moveVertical
                //, -minZ, minZ)
        );
        GetComponent<Rigidbody>().velocity = movement;
        GetComponent<Rigidbody>().position = new Vector3(
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, -20, 20),
            0,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z, -10, 10)
        );
        float playerCameraOffset = Camera.main.transform.position.y - transform.position.y;
        Vector3 mousePositionScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, playerCameraOffset);
        Vector3 mousePositionWorldSpace = Camera.main.ScreenToWorldPoint(mousePositionScreenSpace);
        Quaternion newRotation = Quaternion.LookRotation(mousePositionWorldSpace - transform.position);
        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, newRotation.eulerAngles.y, movement.x * -tilt);
    }
}
