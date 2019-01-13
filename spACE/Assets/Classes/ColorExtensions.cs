using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorExtensions {
    public static Color ToColor(this string color) {
        return (Color)typeof(Color).GetProperty(color.ToLowerInvariant()).GetValue(null, null);
    }
}