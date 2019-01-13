using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestColorer : MonoBehaviour {
    public ColorPicker picker;

    // Use this for initialization
    void Start() {
        picker = transform.GetComponent<ColorPicker>();
        picker.CurrentColor = Color.white;
        picker.onValueChanged.AddListener(color => {
            CameraController.color = color;
        });
        CameraController.color = picker.CurrentColor;
    }
}
