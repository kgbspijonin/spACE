using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("Cube")]
public class Cube {
    public string name;
    public Color color;
    public Vector3 position;
    public Vector3 rotation;
    public string w; //weapon

    public void Serialize(string path) {
        XmlSerializer serializer = new XmlSerializer(typeof(Cube));
        using (var stream = new FileStream(path, FileMode.Create)) {
            serializer.Serialize(stream, this);
        }
    }

    public static Cube DeSerialize(string path) {
        XmlSerializer serializer = new XmlSerializer(typeof(Cube));
        using (var stream = new FileStream(path, FileMode.Open)) {
            return serializer.Deserialize(stream) as Cube;
        }
    }
}
