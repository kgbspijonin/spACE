using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsController : MonoBehaviour {
    public void Fire(int weapon) {
        transform.GetChild(weapon).GetComponent<WeaponController>().Shoot();
    }
}
