using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayerInfo : MonoBehaviour {
	void Start () {
        BasicUI.StaticPlayerLoader();
	}

    void OnEnable() {
        BasicUI.StaticPlayerLoader();
    }
}
