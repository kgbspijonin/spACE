using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsingSpaceshipUpdater : MonoBehaviour {
    public string data = "";

	void Update () {
        GetComponent<Text>().text = (string) typeof(StaticInfo).GetField(data).GetValue(null);
    }
}
