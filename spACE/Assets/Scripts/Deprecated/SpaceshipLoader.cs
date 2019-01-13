using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SpaceshipLoader : MonoBehaviour {
    public void Load() {
        Spaceship spaceship = new Spaceship();
        string shipname = "";
        List<Component> children = new List<Component>();
        for (int i = 1; i < transform.childCount; i++) {
            children.Add(transform.GetChild(i));
        }
        foreach (Component component in children) {
            InputField input = component.GetComponent<InputField>();
            switch (component.transform.name) {
                case "Name": shipname = input.text; break;
            }
        }
        GameObject[] currentPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in currentPlayers) {
            Destroy(player.gameObject);
        }
        spaceship = Spaceship.DeSerialize("Assets/Resources/" + shipname + ".xml");
        GameObject created = new GameObject();
        GameObject weapons = new GameObject();
        created.name = "Spaceship";
        created.AddComponent<Rigidbody>();
        created.AddComponent<SphereCollider>();
        created.AddComponent<PlayerMover2>();
        created.GetComponent<PlayerMover2>().speedHorizontal = spaceship.speed;
        created.GetComponent<PlayerMover2>().speedVertical = spaceship.speed;
        created.GetComponent<PlayerMover2>().tilt = spaceship.tilt;
        weapons.transform.name = "Weapons";
        weapons.transform.parent = created.transform;
        /*
        GameObject mesh = Instantiate(Resources.Load<GameObject>(spaceship.prefabLocation));
        mesh.transform.parent = created.transform;
        mesh.transform.name = "Renderer";
        */
        for (int i = 0; i < spaceship.weapons.Count; i++) {
            
        }
    }
}
