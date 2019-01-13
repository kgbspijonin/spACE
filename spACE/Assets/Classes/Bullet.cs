using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("Bullet")]
public class Bullet {
    public int pierce;
    public float speed;
    public Color color;
    public float damage;

    public Bullet(Color color_, int pierce_ = 1, int speed_ = 500, float damage_ = 100) {
        this.color = color_;
        this.pierce = pierce_;
        this.speed = speed_;
        this.damage = damage_;
    }

    public Bullet() {

    }

    public void Serialize(string path) {
        XmlSerializer serializer = new XmlSerializer(typeof(Bullet));
        using (var stream = new FileStream(path, FileMode.Create)) {
            serializer.Serialize(stream, this);
        }
    }

    public static Bullet DeSerialize(string path) {
        XmlSerializer serializer = new XmlSerializer(typeof(Bullet));
        using (var stream = new FileStream(path, FileMode.Open)) {
            return serializer.Deserialize(stream) as Bullet;
        }
    }
}
