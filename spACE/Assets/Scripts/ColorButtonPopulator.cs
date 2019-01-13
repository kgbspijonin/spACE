using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ColorButtonPopulator : MonoBehaviour {
    public Button button;

	void Start () {
        PlayerController.PopulateLasers();
        string[] bullets = StaticInfo.lasers.Keys.ToArray();

        foreach(string str in bullets) {
            Button created = Instantiate(button);
            created.transform.parent = transform;
            created.transform.name = str;
            created.transform.GetComponent<Image>().color = str.ToColor();
            created.transform.localScale = new Vector3(1, 1, 1);
        }
	}
}
