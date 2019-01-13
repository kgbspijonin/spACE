using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipPacifier : MonoBehaviour {
    public void Pacify() {
        Spaceship spaceship = new Spaceship();
        string name = "";
        string path = "Assets/Resources/";
        List<Component> children = new List<Component>();
        for (int i = 1; i < transform.childCount; i++) {
            children.Add(transform.GetChild(i));
        }
        foreach (Component component in children) {
            InputField input = component.GetComponent<InputField>();
            switch (component.transform.name) {
                case "Name": name = input.text; break;
            }
        }
        spaceship = Spaceship.DeSerialize("Assets/Resources/" + name + ".xml");
        spaceship.weapons = new List<WeaponRef>();
        spaceship.Serialize(path + spaceship.name + ".xml");
    }
}
