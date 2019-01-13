using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("Weapon")]
public class Weapon {
    public string name;
    public string barrel;
    public string bullet;
    public float fireRate;

    public void Serialize(string path) {
        XmlSerializer serializer = new XmlSerializer(typeof(Weapon));
        using (var stream = new FileStream(path, FileMode.Create)) {
            serializer.Serialize(stream, this);
        }
    }

    public static Weapon DeSerialize(string path) {
        XmlSerializer serializer = new XmlSerializer(typeof(Weapon));
        using (var stream = new FileStream(path, FileMode.Open)) {
            return serializer.Deserialize(stream) as Weapon;
        }
    }
}
