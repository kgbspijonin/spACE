using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTextToPlayer : MonoBehaviour {
	void Start () {
        GetComponent<Text>().text = StaticInfo.player;
    }
}
