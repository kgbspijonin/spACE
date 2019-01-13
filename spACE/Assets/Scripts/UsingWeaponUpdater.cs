using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsingWeaponUpdater : MonoBehaviour {
	void Update () {
        GetComponent<Text>().text = CameraController.weaponAlias;
	}
}
