namespace nlcEngine;

/// <summary>
/// A color represented by RGBA components.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Color
{
    /// <summary>
    /// The R component.
    /// </summary>
    public byte R;
    /// <summary>
    /// The G component.
    /// </summary>
    public byte G;
    /// <summary>
    /// The B component.
    /// </summary>
    public byte B;
    /// <summary>
    /// The A component.
    /// </summary>
    public byte A;

    /// <summary>
    /// Gets the R component in float.
    /// </summary>
    public float Rf => R / 255f;
    /// <summary>
    /// Gets the G component in float.
    /// </summary>
    public float Gf => G / 255f;
    /// <summary>
    /// Gets the B component in float.
    /// </summary>
    public float Bf => B / 255f;
    /// <summary>
    /// Gets the A component in float.
    /// </summary>
    public float Af => A / 255f;

    /// <summary>
    /// Initializes a new structure with the R, G, B, and A component.
    /// </summary>
    /// <param name="r">R component</param>
    /// <param name="g">G component</param>
    /// <param name="b">B component</param>
    /// <param name="a">A component</param>
    public Color(byte r, byte g, byte b, byte a)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    /// <summary>
    /// Initializes a new structure with the R, G, and B component. The A component will be set to 255.
    /// </summary>
    /// <param name="r">R component</param>
    /// <param name="g">G component</param>
    /// <param name="b">B component</param>
    public Color(byte r, byte g, byte b) : this(r, g, b, 255)
    {
    }

    /// <summary>
    /// Returns a new structure with the R, G, and B component, specified with the float values.
    /// </summary>
    /// <param name="r">R component</param>
    /// <param name="g">G component</param>
    /// <param name="b">B component</param>
    /// <returns>a structure with the specified components</returns>
    public static Color FromFloats(float r, float g, float b)
    {
        byte rb = (byte)(r * 255);
        byte gb = (byte)(g * 255);
        byte bb = (byte)(b * 255);
        byte ab = 255;

        return new Color(rb, gb, bb, ab);
    }

    /// <summary>
    /// Returns a new structure with the R, G, B, and A component, specified with the float values.
    /// </summary>
    /// <param name="r">R component</param>
    /// <param name="g">G component</param>
    /// <param name="b">B component</param>
    /// <param name="a">A component</param>
    /// <returns>a structure with the specified components</returns>
    public static Color FromFloats(float r, float g, float b, float a)
    {
        byte rb = (byte)(r * 255);
        byte gb = (byte)(g * 255);
        byte bb = (byte)(b * 255);
        byte ab = (byte)(a * 255);

        return new Color(rb, gb, bb, ab);
    }

    /// <summary>
    /// Gets the inverted color from this structure.
    /// </summary>
    /// <returns>inverted color</returns>
    public Color Invert()
    {
        return new Color((byte)(255 - R), (byte)(255 - G), (byte)(255 - B), A);
    }



    #region Web Colors Declaration

    ///<summary>
    /// Gets the TRANSPARENT color.
    ///</summary>
    public static readonly Color Transparent = new Color(255, 255, 255);
    ///<summary>
    /// Gets the ALICEBLUE color.
    ///</summary>
    public static readonly Color AliceBlue = new Color(240, 248, 255);
    ///<summary>
    /// Gets the ANTIQUEWHITE color.
    ///</summary>
    public static readonly Color AntiqueWhite = new Color(250, 235, 215);
    ///<summary>
    /// Gets the AQUA color.
    ///</summary>
    public static readonly Color Aqua = new Color(0, 255, 255);
    ///<summary>
    /// Gets the AQUAMARINE color.
    ///</summary>
    public static readonly Color Aquamarine = new Color(127, 255, 212);
    ///<summary>
    /// Gets the AZURE color.
    ///</summary>
    public static readonly Color Azure = new Color(240, 255, 255);
    ///<summary>
    /// Gets the BEIGE color.
    ///</summary>
    public static readonly Color Beige = new Color(245, 245, 220);
    ///<summary>
    /// Gets the BISQUE color.
    ///</summary>
    public static readonly Color Bisque = new Color(255, 228, 196);
    ///<summary>
    /// Gets the BLACK color.
    ///</summary>
    public static readonly Color Black = new Color(0, 0, 0);
    ///<summary>
    /// Gets the BLANCHEDALMOND color.
    ///</summary>
    public static readonly Color BlanchedAlmond = new Color(255, 235, 205);
    ///<summary>
    /// Gets the BLUE color.
    ///</summary>
    public static readonly Color Blue = new Color(0, 0, 255);
    ///<summary>
    /// Gets the BLUEVIOLET color.
    ///</summary>
    public static readonly Color BlueViolet = new Color(138, 43, 226);
    ///<summary>
    /// Gets the BROWN color.
    ///</summary>
    public static readonly Color Brown = new Color(165, 42, 42);
    ///<summary>
    /// Gets the BURLYWOOD color.
    ///</summary>
    public static readonly Color BurlyWood = new Color(222, 184, 135);
    ///<summary>
    /// Gets the CADETBLUE color.
    ///</summary>
    public static readonly Color CadetBlue = new Color(95, 158, 160);
    ///<summary>
    /// Gets the CHARTREUSE color.
    ///</summary>
    public static readonly Color Chartreuse = new Color(127, 255, 0);
    ///<summary>
    /// Gets the CHOCOLATE color.
    ///</summary>
    public static readonly Color Chocolate = new Color(210, 105, 30);
    ///<summary>
    /// Gets the CORAL color.
    ///</summary>
    public static readonly Color Coral = new Color(255, 127, 80);
    ///<summary>
    /// Gets the CORNFLOWERBLUE color.
    ///</summary>
    public static readonly Color CornflowerBlue = new Color(100, 149, 237);
    ///<summary>
    /// Gets the CORNSILK color.
    ///</summary>
    public static readonly Color Cornsilk = new Color(255, 248, 220);
    ///<summary>
    /// Gets the CRIMSON color.
    ///</summary>
    public static readonly Color Crimson = new Color(220, 20, 60);
    ///<summary>
    /// Gets the CYAN color.
    ///</summary>
    public static readonly Color Cyan = new Color(0, 255, 255);
    ///<summary>
    /// Gets the DARKBLUE color.
    ///</summary>
    public static readonly Color DarkBlue = new Color(0, 0, 139);
    ///<summary>
    /// Gets the DARKCYAN color.
    ///</summary>
    public static readonly Color DarkCyan = new Color(0, 139, 139);
    ///<summary>
    /// Gets the DARKGOLDENROD color.
    ///</summary>
    public static readonly Color DarkGoldenrod = new Color(184, 134, 11);
    ///<summary>
    /// Gets the DARKGRAY color.
    ///</summary>
    public static readonly Color DarkGray = new Color(169, 169, 169);
    ///<summary>
    /// Gets the DARKGREEN color.
    ///</summary>
    public static readonly Color DarkGreen = new Color(0, 100, 0);
    ///<summary>
    /// Gets the DARKKHAKI color.
    ///</summary>
    public static readonly Color DarkKhaki = new Color(189, 183, 107);
    ///<summary>
    /// Gets the DARKMAGENTA color.
    ///</summary>
    public static readonly Color DarkMagenta = new Color(139, 0, 139);
    ///<summary>
    /// Gets the DARKOLIVEGREEN color.
    ///</summary>
    public static readonly Color DarkOliveGreen = new Color(85, 107, 47);
    ///<summary>
    /// Gets the DARKORANGE color.
    ///</summary>
    public static readonly Color DarkOrange = new Color(255, 140, 0);
    ///<summary>
    /// Gets the DARKORCHID color.
    ///</summary>
    public static readonly Color DarkOrchid = new Color(153, 50, 204);
    ///<summary>
    /// Gets the DARKRED color.
    ///</summary>
    public static readonly Color DarkRed = new Color(139, 0, 0);
    ///<summary>
    /// Gets the DARKSALMON color.
    ///</summary>
    public static readonly Color DarkSalmon = new Color(233, 150, 122);
    ///<summary>
    /// Gets the DARKSEAGREEN color.
    ///</summary>
    public static readonly Color DarkSeaGreen = new Color(143, 188, 143);
    ///<summary>
    /// Gets the DARKSLATEBLUE color.
    ///</summary>
    public static readonly Color DarkSlateBlue = new Color(72, 61, 139);
    ///<summary>
    /// Gets the DARKSLATEGRAY color.
    ///</summary>
    public static readonly Color DarkSlateGray = new Color(47, 79, 79);
    ///<summary>
    /// Gets the DARKTURQUOISE color.
    ///</summary>
    public static readonly Color DarkTurquoise = new Color(0, 206, 209);
    ///<summary>
    /// Gets the DARKVIOLET color.
    ///</summary>
    public static readonly Color DarkViolet = new Color(148, 0, 211);
    ///<summary>
    /// Gets the DEEPPINK color.
    ///</summary>
    public static readonly Color DeepPink = new Color(255, 20, 147);
    ///<summary>
    /// Gets the DEEPSKYBLUE color.
    ///</summary>
    public static readonly Color DeepSkyBlue = new Color(0, 191, 255);
    ///<summary>
    /// Gets the DIMGRAY color.
    ///</summary>
    public static readonly Color DimGray = new Color(105, 105, 105);
    ///<summary>
    /// Gets the DODGERBLUE color.
    ///</summary>
    public static readonly Color DodgerBlue = new Color(30, 144, 255);
    ///<summary>
    /// Gets the FIREBRICK color.
    ///</summary>
    public static readonly Color Firebrick = new Color(178, 34, 34);
    ///<summary>
    /// Gets the FLORALWHITE color.
    ///</summary>
    public static readonly Color FloralWhite = new Color(255, 250, 240);
    ///<summary>
    /// Gets the FORESTGREEN color.
    ///</summary>
    public static readonly Color ForestGreen = new Color(34, 139, 34);
    ///<summary>
    /// Gets the FUCHSIA color.
    ///</summary>
    public static readonly Color Fuchsia = new Color(255, 0, 255);
    ///<summary>
    /// Gets the GAINSBORO color.
    ///</summary>
    public static readonly Color Gainsboro = new Color(220, 220, 220);
    ///<summary>
    /// Gets the GHOSTWHITE color.
    ///</summary>
    public static readonly Color GhostWhite = new Color(248, 248, 255);
    ///<summary>
    /// Gets the GOLD color.
    ///</summary>
    public static readonly Color Gold = new Color(255, 215, 0);
    ///<summary>
    /// Gets the GOLDENROD color.
    ///</summary>
    public static readonly Color Goldenrod = new Color(218, 165, 32);
    ///<summary>
    /// Gets the GRAY color.
    ///</summary>
    public static readonly Color Gray = new Color(128, 128, 128);
    ///<summary>
    /// Gets the GREEN color.
    ///</summary>
    public static readonly Color Green = new Color(0, 128, 0);
    ///<summary>
    /// Gets the GREENYELLOW color.
    ///</summary>
    public static readonly Color GreenYellow = new Color(173, 255, 47);
    ///<summary>
    /// Gets the HONEYDEW color.
    ///</summary>
    public static readonly Color Honeydew = new Color(240, 255, 240);
    ///<summary>
    /// Gets the HOTPINK color.
    ///</summary>
    public static readonly Color HotPink = new Color(255, 105, 180);
    ///<summary>
    /// Gets the INDIANRED color.
    ///</summary>
    public static readonly Color IndianRed = new Color(205, 92, 92);
    ///<summary>
    /// Gets the INDIGO color.
    ///</summary>
    public static readonly Color Indigo = new Color(75, 0, 130);
    ///<summary>
    /// Gets the IVORY color.
    ///</summary>
    public static readonly Color Ivory = new Color(255, 255, 240);
    ///<summary>
    /// Gets the KHAKI color.
    ///</summary>
    public static readonly Color Khaki = new Color(240, 230, 140);
    ///<summary>
    /// Gets the LAVENDER color.
    ///</summary>
    public static readonly Color Lavender = new Color(230, 230, 250);
    ///<summary>
    /// Gets the LAVENDERBLUSH color.
    ///</summary>
    public static readonly Color LavenderBlush = new Color(255, 240, 245);
    ///<summary>
    /// Gets the LAWNGREEN color.
    ///</summary>
    public static readonly Color LawnGreen = new Color(124, 252, 0);
    ///<summary>
    /// Gets the LEMONCHIFFON color.
    ///</summary>
    public static readonly Color LemonChiffon = new Color(255, 250, 205);
    ///<summary>
    /// Gets the LIGHTBLUE color.
    ///</summary>
    public static readonly Color LightBlue = new Color(173, 216, 230);
    ///<summary>
    /// Gets the LIGHTCORAL color.
    ///</summary>
    public static readonly Color LightCoral = new Color(240, 128, 128);
    ///<summary>
    /// Gets the LIGHTCYAN color.
    ///</summary>
    public static readonly Color LightCyan = new Color(224, 255, 255);
    ///<summary>
    /// Gets the LIGHTGOLDENRODYELLOW color.
    ///</summary>
    public static readonly Color LightGoldenrodYellow = new Color(250, 250, 210);
    ///<summary>
    /// Gets the LIGHTGREEN color.
    ///</summary>
    public static readonly Color LightGreen = new Color(144, 238, 144);
    ///<summary>
    /// Gets the LIGHTGRAY color.
    ///</summary>
    public static readonly Color LightGray = new Color(211, 211, 211);
    ///<summary>
    /// Gets the LIGHTPINK color.
    ///</summary>
    public static readonly Color LightPink = new Color(255, 182, 193);
    ///<summary>
    /// Gets the LIGHTSALMON color.
    ///</summary>
    public static readonly Color LightSalmon = new Color(255, 160, 122);
    ///<summary>
    /// Gets the LIGHTSEAGREEN color.
    ///</summary>
    public static readonly Color LightSeaGreen = new Color(32, 178, 170);
    ///<summary>
    /// Gets the LIGHTSKYBLUE color.
    ///</summary>
    public static readonly Color LightSkyBlue = new Color(135, 206, 250);
    ///<summary>
    /// Gets the LIGHTSLATEGRAY color.
    ///</summary>
    public static readonly Color LightSlateGray = new Color(119, 136, 153);
    ///<summary>
    /// Gets the LIGHTSTEELBLUE color.
    ///</summary>
    public static readonly Color LightSteelBlue = new Color(176, 196, 222);
    ///<summary>
    /// Gets the LIGHTYELLOW color.
    ///</summary>
    public static readonly Color LightYellow = new Color(255, 255, 224);
    ///<summary>
    /// Gets the LIME color.
    ///</summary>
    public static readonly Color Lime = new Color(0, 255, 0);
    ///<summary>
    /// Gets the LIMEGREEN color.
    ///</summary>
    public static readonly Color LimeGreen = new Color(50, 205, 50);
    ///<summary>
    /// Gets the LINEN color.
    ///</summary>
    public static readonly Color Linen = new Color(250, 240, 230);
    ///<summary>
    /// Gets the MAGENTA color.
    ///</summary>
    public static readonly Color Magenta = new Color(255, 0, 255);
    ///<summary>
    /// Gets the MAROON color.
    ///</summary>
    public static readonly Color Maroon = new Color(128, 0, 0);
    ///<summary>
    /// Gets the MEDIUMAQUAMARINE color.
    ///</summary>
    public static readonly Color MediumAquamarine = new Color(102, 205, 170);
    ///<summary>
    /// Gets the MEDIUMBLUE color.
    ///</summary>
    public static readonly Color MediumBlue = new Color(0, 0, 205);
    ///<summary>
    /// Gets the MEDIUMORCHID color.
    ///</summary>
    public static readonly Color MediumOrchid = new Color(186, 85, 211);
    ///<summary>
    /// Gets the MEDIUMPURPLE color.
    ///</summary>
    public static readonly Color MediumPurple = new Color(147, 112, 219);
    ///<summary>
    /// Gets the MEDIUMSEAGREEN color.
    ///</summary>
    public static readonly Color MediumSeaGreen = new Color(60, 179, 113);
    ///<summary>
    /// Gets the MEDIUMSLATEBLUE color.
    ///</summary>
    public static readonly Color MediumSlateBlue = new Color(123, 104, 238);
    ///<summary>
    /// Gets the MEDIUMSPRINGGREEN color.
    ///</summary>
    public static readonly Color MediumSpringGreen = new Color(0, 250, 154);
    ///<summary>
    /// Gets the MEDIUMTURQUOISE color.
    ///</summary>
    public static readonly Color MediumTurquoise = new Color(72, 209, 204);
    ///<summary>
    /// Gets the MEDIUMVIOLETRED color.
    ///</summary>
    public static readonly Color MediumVioletRed = new Color(199, 21, 133);
    ///<summary>
    /// Gets the MIDNIGHTBLUE color.
    ///</summary>
    public static readonly Color MidnightBlue = new Color(25, 25, 112);
    ///<summary>
    /// Gets the MINTCREAM color.
    ///</summary>
    public static readonly Color MintCream = new Color(245, 255, 250);
    ///<summary>
    /// Gets the MISTYROSE color.
    ///</summary>
    public static readonly Color MistyRose = new Color(255, 228, 225);
    ///<summary>
    /// Gets the MOCCASIN color.
    ///</summary>
    public static readonly Color Moccasin = new Color(255, 228, 181);
    ///<summary>
    /// Gets the NAVAJOWHITE color.
    ///</summary>
    public static readonly Color NavajoWhite = new Color(255, 222, 173);
    ///<summary>
    /// Gets the NAVY color.
    ///</summary>
    public static readonly Color Navy = new Color(0, 0, 128);
    ///<summary>
    /// Gets the OLDLACE color.
    ///</summary>
    public static readonly Color OldLace = new Color(253, 245, 230);
    ///<summary>
    /// Gets the OLIVE color.
    ///</summary>
    public static readonly Color Olive = new Color(128, 128, 0);
    ///<summary>
    /// Gets the OLIVEDRAB color.
    ///</summary>
    public static readonly Color OliveDrab = new Color(107, 142, 35);
    ///<summary>
    /// Gets the ORANGE color.
    ///</summary>
    public static readonly Color Orange = new Color(255, 165, 0);
    ///<summary>
    /// Gets the ORANGERED color.
    ///</summary>
    public static readonly Color OrangeRed = new Color(255, 69, 0);
    ///<summary>
    /// Gets the ORCHID color.
    ///</summary>
    public static readonly Color Orchid = new Color(218, 112, 214);
    ///<summary>
    /// Gets the PALEGOLDENROD color.
    ///</summary>
    public static readonly Color PaleGoldenrod = new Color(238, 232, 170);
    ///<summary>
    /// Gets the PALEGREEN color.
    ///</summary>
    public static readonly Color PaleGreen = new Color(152, 251, 152);
    ///<summary>
    /// Gets the PALETURQUOISE color.
    ///</summary>
    public static readonly Color PaleTurquoise = new Color(175, 238, 238);
    ///<summary>
    /// Gets the PALEVIOLETRED color.
    ///</summary>
    public static readonly Color PaleVioletRed = new Color(219, 112, 147);
    ///<summary>
    /// Gets the PAPAYAWHIP color.
    ///</summary>
    public static readonly Color PapayaWhip = new Color(255, 239, 213);
    ///<summary>
    /// Gets the PEACHPUFF color.
    ///</summary>
    public static readonly Color PeachPuff = new Color(255, 218, 185);
    ///<summary>
    /// Gets the PERU color.
    ///</summary>
    public static readonly Color Peru = new Color(205, 133, 63);
    ///<summary>
    /// Gets the PINK color.
    ///</summary>
    public static readonly Color Pink = new Color(255, 192, 203);
    ///<summary>
    /// Gets the PLUM color.
    ///</summary>
    public static readonly Color Plum = new Color(221, 160, 221);
    ///<summary>
    /// Gets the POWDERBLUE color.
    ///</summary>
    public static readonly Color PowderBlue = new Color(176, 224, 230);
    ///<summary>
    /// Gets the PURPLE color.
    ///</summary>
    public static readonly Color Purple = new Color(128, 0, 128);
    ///<summary>
    /// Gets the REBECCAPURPLE color.
    ///</summary>
    public static readonly Color RebeccaPurple = new Color(102, 51, 153);
    ///<summary>
    /// Gets the RED color.
    ///</summary>
    public static readonly Color Red = new Color(255, 0, 0);
    ///<summary>
    /// Gets the ROSYBROWN color.
    ///</summary>
    public static readonly Color RosyBrown = new Color(188, 143, 143);
    ///<summary>
    /// Gets the ROYALBLUE color.
    ///</summary>
    public static readonly Color RoyalBlue = new Color(65, 105, 225);
    ///<summary>
    /// Gets the SADDLEBROWN color.
    ///</summary>
    public static readonly Color SaddleBrown = new Color(139, 69, 19);
    ///<summary>
    /// Gets the SALMON color.
    ///</summary>
    public static readonly Color Salmon = new Color(250, 128, 114);
    ///<summary>
    /// Gets the SANDYBROWN color.
    ///</summary>
    public static readonly Color SandyBrown = new Color(244, 164, 96);
    ///<summary>
    /// Gets the SEAGREEN color.
    ///</summary>
    public static readonly Color SeaGreen = new Color(46, 139, 87);
    ///<summary>
    /// Gets the SEASHELL color.
    ///</summary>
    public static readonly Color SeaShell = new Color(255, 245, 238);
    ///<summary>
    /// Gets the SIENNA color.
    ///</summary>
    public static readonly Color Sienna = new Color(160, 82, 45);
    ///<summary>
    /// Gets the SILVER color.
    ///</summary>
    public static readonly Color Silver = new Color(192, 192, 192);
    ///<summary>
    /// Gets the SKYBLUE color.
    ///</summary>
    public static readonly Color SkyBlue = new Color(135, 206, 235);
    ///<summary>
    /// Gets the SLATEBLUE color.
    ///</summary>
    public static readonly Color SlateBlue = new Color(106, 90, 205);
    ///<summary>
    /// Gets the SLATEGRAY color.
    ///</summary>
    public static readonly Color SlateGray = new Color(112, 128, 144);
    ///<summary>
    /// Gets the SNOW color.
    ///</summary>
    public static readonly Color Snow = new Color(255, 250, 250);
    ///<summary>
    /// Gets the SPRINGGREEN color.
    ///</summary>
    public static readonly Color SpringGreen = new Color(0, 255, 127);
    ///<summary>
    /// Gets the STEELBLUE color.
    ///</summary>
    public static readonly Color SteelBlue = new Color(70, 130, 180);
    ///<summary>
    /// Gets the TAN color.
    ///</summary>
    public static readonly Color Tan = new Color(210, 180, 140);
    ///<summary>
    /// Gets the TEAL color.
    ///</summary>
    public static readonly Color Teal = new Color(0, 128, 128);
    ///<summary>
    /// Gets the THISTLE color.
    ///</summary>
    public static readonly Color Thistle = new Color(216, 191, 216);
    ///<summary>
    /// Gets the TOMATO color.
    ///</summary>
    public static readonly Color Tomato = new Color(255, 99, 71);
    ///<summary>
    /// Gets the TURQUOISE color.
    ///</summary>
    public static readonly Color Turquoise = new Color(64, 224, 208);
    ///<summary>
    /// Gets the VIOLET color.
    ///</summary>
    public static readonly Color Violet = new Color(238, 130, 238);
    ///<summary>
    /// Gets the WHEAT color.
    ///</summary>
    public static readonly Color Wheat = new Color(245, 222, 179);
    ///<summary>
    /// Gets the WHITE color.
    ///</summary>
    public static readonly Color White = new Color(255, 255, 255);
    ///<summary>
    /// Gets the WHITESMOKE color.
    ///</summary>
    public static readonly Color WhiteSmoke = new Color(245, 245, 245);
    ///<summary>
    /// Gets the YELLOW color.
    ///</summary>
    public static readonly Color Yellow = new Color(255, 255, 0);
    ///<summary>
    /// Gets the YELLOWGREEN color.
    ///</summary>
    public static readonly Color YellowGreen = new Color(154, 205, 50);

    #endregion

}