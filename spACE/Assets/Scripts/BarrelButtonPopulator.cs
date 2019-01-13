using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BarrelButtonPopulator : MonoBehaviour {
    public Button button;

    void Start() {
        PlayerController.PopulateBarrels();
        string[] barrels = StaticInfo.barrels.ToArray();

        foreach (string str in barrels) {
            Button created = Instantiate(button);
            created.transform.parent = transform;
            created.transform.name = str;
            created.transform.Find("Text").GetComponent<Text>().text = str;
            created.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
