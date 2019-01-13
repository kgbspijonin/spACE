using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("Player")]
public class Player {
    public string name;
    public float score;
    public List<string> unlockedColors;
    public int availableAbilities;

    public void Serialize(string path) {
        XmlSerializer serializer = new XmlSerializer(typeof(Player));
        using (var stream = new FileStream(path, FileMode.Create)) {
            serializer.Serialize(stream, this);
        }
    }

    public static Player DeSerialize(string path) {
        XmlSerializer serializer = new XmlSerializer(typeof(Player));
        using (var stream = new FileStream(path, FileMode.Open)) {
            return serializer.Deserialize(stream) as Player;
        }
    }
}
