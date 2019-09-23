// Auto Generated script. Use "Berry Panel > Item Colors" to edit it.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ItemColor {
    Red = 0,
    Green = 1,
    Blue = 2,
    Yellow = 3,
    Purple = 4,
    Orange = 5,
    Unknown = 100,
    Uncolored = 101,
    Universal = 102
}

public static class RealColors {

    static Dictionary<ItemColor, Color> colors = new Dictionary<ItemColor, Color>() {
        {ItemColor.Red, new Color(1.00f, 0.50f, 0.50f, 1.00f)},
        {ItemColor.Green, new Color(0.50f, 1.00f, 0.60f, 1.00f)},
        {ItemColor.Blue, new Color(0.40f, 0.80f, 1.00f, 1.00f)},
        {ItemColor.Yellow, new Color(1.00f, 0.90f, 0.30f, 1.00f)},
        {ItemColor.Purple, new Color(0.80f, 0.40f, 1.00f, 1.00f)},
        {ItemColor.Orange, new Color(1.00f, 0.70f, 0.00f, 1.00f)}
    };

    public static Color Get(ItemColor color) {
        try {
            if (color.IsPhysicalColor())
                return colors[color];
        } catch (System.Exception) {
        }
        return Color.white;
    }
}