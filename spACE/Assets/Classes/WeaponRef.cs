using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("WeaponRef")]
public class WeaponRef {
    public string weapon = "";
    public Vector3 position = new Vector3();

    public void Serialize(string path) {
        XmlSerializer serializer = new XmlSerializer(typeof(WeaponRef));
        using (var stream = new FileStream(path, FileMode.Create)) {
            serializer.Serialize(stream, this);
        }
    }

    public static WeaponRef DeSerialize(string path) {
        XmlSerializer serializer = new XmlSerializer(typeof(WeaponRef));
        using (var stream = new FileStream(path, FileMode.Open)) {
            return serializer.Deserialize(stream) as WeaponRef;
        }
    }
}
