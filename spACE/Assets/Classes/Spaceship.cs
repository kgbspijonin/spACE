using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("Spaceship")]
public class Spaceship {
    public string name;
    public string owner;
    public float speed;
    public float tilt;

    [XmlArray("Cubes")]
    [XmlArrayItem("Cube")]
    public List<Cube> cubes = new List<Cube>();

    [XmlArray("Weapons")]
    [XmlArrayItem("Weapon")]
    public List<WeaponRef> weapons = new List<WeaponRef>();

    public void Serialize(string path) {
        XmlSerializer serializer = new XmlSerializer(typeof(Spaceship));
        using (var stream = new FileStream(path, FileMode.Create)) {
            serializer.Serialize(stream, this);
        }
    }

    public static Spaceship DeSerialize(string path) {
        XmlSerializer serializer = new XmlSerializer(typeof(Spaceship));
        using (var stream = new FileStream(path, FileMode.Open)) {
            return serializer.Deserialize(stream) as Spaceship;
        }
    }
}
