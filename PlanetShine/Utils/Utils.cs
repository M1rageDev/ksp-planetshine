/*
* (C) Copyright 2014, Valerian Gaudeau
* 
* Kerbal Space Program is Copyright (C) 2013 Squad. See http://kerbalspaceprogram.com/. This
* project is in no way associated with nor endorsed by Squad.
* 
* This code is licensed under the Apache License Version 2.0. See the LICENSE.txt
* file for more information.
*/

using System.Collections.Generic;
using System.Collections;
using System;
using System.IO;
using UnityEngine;

namespace PlanetShine
{
    public class Utils
    {
        public static LineRenderer CreateDebugLine(Color startColor, Color endColor)
        {
            GameObject obj = new GameObject ("Line");
            LineRenderer line = obj.AddComponent< LineRenderer > ();
            line.material = new Material (Shader.Find ("Particles/Additive"));
            line.SetColors (startColor, endColor);
            line.SetWidth (0.05f, 0.05f); 
            line.SetVertexCount (2);
            return line;
        }

        public static Color GetTextureAverageColor(Texture2D texture)
        {
            try
            {
                Color[] texColors = texture.GetPixels();

                int total = texColors.Length;

                float r = 0;
                float g = 0;
                float b = 0;

                for (int i = 0; i < total; i++)
                {

                    r += texColors[i].r;

                    g += texColors[i].g;

                    b += texColors[i].b;

                }
                return new Color((byte)(r / total), (byte)(g / total), (byte)(b / total), 0);
            }
            catch (Exception e)
            {
                Debug.LogError(String.Format(
                    "[PlanetShine] An exception occured while extracting color from a Texture2D:\n{0}\nThe exception was:\n{1}",
                    texture,
                    e
                ));
            }
            return new Color();
        }
    }

    public class DisplaySettingOption<T>
    {
        public string label { get; private set; }
        public T value { get; private set; }

        public DisplaySettingOption(string label, T value)
        {
            this.label = label;
            this.value = value;
        }
    }


}
