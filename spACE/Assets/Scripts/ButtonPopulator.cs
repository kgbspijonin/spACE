using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPopulator : MonoBehaviour {
    public Button button;
    public string folderName;

    public void OnEnable() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        List<string> spaceships = BasicUI.GetFilesInFolder("Assets/Resources/" + folderName + "/");
        for(int i = 0; i < spaceships.Count; i++) {
            if(spaceships[i].Contains(".meta") || !spaceships[i].Contains(".xml")) {
                spaceships.RemoveAt(i);
            }
        }
        for(int i = 0; i < spaceships.Count; i++) {
            spaceships[i] = spaceships[i].Substring(0, spaceships[i].LastIndexOf('.'));
        }
        spaceships = BasicUI.SliceFolderNames(spaceships);
        foreach(string name in spaceships) {
            Button created = Instantiate(button);
            created.transform.parent = transform;
            created.transform.name = name;
            created.transform.GetChild(0).GetComponent<Text>().text = name;
            created.transform.localScale = new Vector3(1, 1, 1);    
        }
    }
}
