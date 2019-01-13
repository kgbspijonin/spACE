using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("Projectile")]
public class Projectile {
    public string prefab;
    public int pierce;
    public float speed;
    public float fireRate;
    public float damage;

    public void Serialize(string path) {
        XmlSerializer serializer = new XmlSerializer(typeof(Projectile));
        using (var stream = new FileStream(path, FileMode.Create)) {
            serializer.Serialize(stream, this);
        }
    }

    public static Projectile DeSerialize(string path) {
        XmlSerializer serializer = new XmlSerializer(typeof(Projectile));
        using (var stream = new FileStream(path, FileMode.Open)) {
            return serializer.Deserialize(stream) as Projectile;
        }
    }
}
