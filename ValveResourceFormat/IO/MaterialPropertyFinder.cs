using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

/*
//Exceptions
textureProperty == "TextureColor" && !line.Contains("TextureColor1") && !line.Contains("TextureColor2") ||
textureProperty == "g_tColor" && !line.Contains("g_tColorA") && !line.Contains("g_tColorB") && !line.Contains("g_tColor1") && !line.Contains("g_tColor2") ||
textureProperty == "TextureNormal" && !line.Contains("TextureNormal1") && !line.Contains("TextureNorma2") ||
textureProperty == "TextureRoughness" && !line.Contains("TextureRoughness1") && !line.Contains("TextureRoughness2") ||
textureProperty == "g_tNormal" && !line.Contains("g_tNormal1") && !line.Contains("g_tNormal2") && !line.Contains("g_tNormalA") && !line.Contains("g_tNormalB"))
*/

namespace ValveResourceFormat.IO
{
    class MaterialPropertyFinder
    {
        public static List<string> ValveMaterialTexturesCompiled = new List<string>
        {
                //Singular instances
                "g_tBlendModulation",
                "g_tColor",
                "g_tMask",
                "g_tTintMask",
                "g_tSharedColorOverlay",
                "g_tNormal",
                //Glass
                "g_tGlassDust",
                "g_tGlassTintColor",

                //Albedo layer 1 -----------------------
                "g_tColor1",
                "g_tColorA",
                "g_tLayer1Color",
                //Normal-height layer 1    
                "g_tNormal1",
                "g_tNormalA",
                "g_tHeight1",
                "g_tLayer1NormalRoughness",

                "g_tLayer1AmbientOcclusion",
                "g_tLayer1Detail",

                //Albedo layer 2 ------------------------
                "g_tColor2",
                "g_tColorB",
                "g_tLayer2Color",
                //Normal-height layer 2
                "g_tNormal2",
                "g_tNormalB",
                "g_tHeight2",
                "g_tLayer2NormalRoughness",

                "g_tLayer2Detail",
                "g_tLayer2AmbientOcclusion",

                //Found in Foliage
                "g_tTransmissiveColor",
                "g_tNoiseMap",
                "g_tAmbientOcclusion",
        };

        public static List<string> ValveMaterialTextures = new List<string>
        {
            "GlassMaskColor",
            "GlassMaskTranslucency",
            "GlassMaskTransmission",
            "GlassTintColor",

            "TextureColor",
            "TextureColor1",
            "TextureColor2",

            "TextureAmbientOcclusion1",
            "TextureAmbientOcclusion2",
            "TextureHeight1",
            "TextureHeight2",
            "TextureTintMask1",
            "TextureTintMask2",

            "TextureNormal",
            "TextureNormal1",
            "TextureNormal2",

            "TextureRoughness",
            "TextureRoughness1",
            "TextureRoughness2",

            "TextureMetalness1",
            "TextureMetalness2",

            "TextureBlendModulation",
            "TextureLayer1Color",
            "TextureLayer1AmbientOcclusion",
            "TextureLayer1Normal",
            "TextureLayer1Roughness",
            "TextureLayer2AmbientOcclusion",
            "TextureLayer2Color",
            "TextureLayer2Normal",
            "TextureLayer2Roughness",

            "TextureColorA",
            "TextureColorB",
            "TextureMask",
            "TextureNormalA",
            "TextureNormalB",
            "TextureRoughnessA",
            "TextureRoughnessB",
        };

        public static List<string> ValveMaterialScalars = new List<string>
        {
            "F_BLEND_EFFECTS",
            "g_bFogEnabled",
            "g_bMetalness1",
            "g_bMetalness2",
            "g_nColorCorrectionMode1",
            "g_nColorCorrectionMode2",
            "g_nScaleTexCoordUByModelScaleAxis" ,
            "g_nScaleTexCoordVByModelScaleAxis",
            "g_nTextureAddressModeU",
            "g_nTextureAddressModeV",
            "g_nVertexColorMode1",
            "g_nVertexColorMode2",
            "g_flBevelCurve2",
            "g_flBevelSoftness2",
            "g_flBevelSpread2",
            "g_flBevelStrength2",
            "g_flBorderOffset2",
            "g_flBorderSoftness2",
            "g_flBorderSpread2",
            "g_flHeightMapScale2",
            "g_flModelTintAmount",
            "g_flTexCoordRotation1",
            "g_flTexCoordRotation2",
            "g_fTextureColorBrightness1",
            "g_fTextureColorBrightness2",
            "g_fTextureColorContrast1",
            "g_fTextureColorContrast2",
            "g_fTextureColorSaturation1",
            "g_fTextureColorSaturation2",
            "g_fTextureNormalContrast1",
            "g_fTextureRoughnessBrightness1",
            "g_fTextureRoughnessContrast2",
            "g_fTintMaskBrightness1",
            "g_fTintMaskBrightness2",
            "g_fTintMaskContrast1",
            "g_fTintMaskContrast2",
            "g_flHeightMapScale1",
            "g_flHeightMapZeroPoint1",
            "g_fTextureRoughnessContrast1",
            "g_bModelTint1", // g_b ? bool ?
        };

        public static List<string> ValveMaterialVectors = new List<string>
        {
            "g_vAmbientOcclusionLevels1",
            "g_vAmbientOcclusionLevels2",
            "g_vBorderTint2",
            "g_vColorTint",
            "g_vTexCoordCenter1",
            "g_vTexCoordCenter2",
            "g_vTexCoordOffset",
            "g_vTexCoordOffset1",
            "g_vTexCoordOffset2",
            "g_vTexCoordScale",
            "g_vTexCoordScale1",
            "g_vTexCoordScale2" ,
            "g_vTextureColorTint1",
            "g_vTextureColorTint2",
            "g_vNormalTexCoordCenter",
            "g_vNormalTexCoordOffset",
            "g_vNormalTexCoordScale",
            "g_vRoughnessAdjustBrightnessContrast",
            "g_vTexCoordScrollSpeed",
            "g_vLayer1RoughnessBrightnessContrast",
            "g_vLayer1Tint",
            "TextureMetalness", // Unique? TextureMetalness1 is a texture
        };

        //Both Materials and Models have to be decompiled in order to successfully retrieve paths
        public static string GetTexturePath(string fullPath, string textureProperty)
        {
            //Example: //"TextureLayer1Color"	"materials/asphalt/hr_c/hr_asphalt_001_color.png"
            string pathToTex = "";

            if (File.Exists(fullPath))
            {
                var matLines = File.ReadLines(fullPath);

                foreach (string line in matLines)
                {
                    if (!line.Contains("//") && line.Contains(textureProperty))
                    {
                        //if (line.Substring(line.IndexOf(textureProperty) + textureProperty.Length - 1) == "\"")
                        //{
                        string[] splited = line.Split("\"");
                        if (splited.Length > 0 && (splited.Length - 2) >= 0)
                        {
                            var output = splited[splited.Length - 2];
                            output = output.Replace(".vtex", ".webp"); // .webp / .png
                            return output;
                        }
                    }
                    //}
                }
            }
            else
            {
                pathToTex = "Not found: " + fullPath;
            }

            return pathToTex;
        }
        public static Vector4? TryGetVector4Value(string fullPath, string Vector4Key)
        {
            if (File.Exists(fullPath))
            {
                var matLines = File.ReadLines(fullPath);

                foreach (string line in matLines)
                {
                    if (line.Contains(Vector4Key) && !line.Contains("//"))
                    {
                        //string cons = "Hiiiiiiiiiiiiiiiiii " + line.Substring(line.IndexOf(Vector4Key)).ToString();
                        //Console.Write(cons);

                        //ProgressReporter?.Report("");
                        //if (line.Substring(line.IndexOf(Vector4Key) + Vector4Key.Length - 1) == "\"")
                        // {
                        string[] splited = line.Split("\"");
                        if (splited.Length > 0 && (splited.Length - 2) >= 0)
                        {
                            string clean = splited[splited.Length - 2].Replace("[", "");
                            clean = clean.Replace("]", "");

                            string[] outArray = clean.Split(" ");
                            if (outArray.Length == 4)
                            {
                                float x = -1, y = -1, z = -1, w = -1;
                                if (float.TryParse(outArray[0], out x) && float.TryParse(outArray[1], out y) && float.TryParse(outArray[2], out z) && float.TryParse(outArray[3], out w))
                                {
                                    return new Vector4(x, y, z, w);
                                }
                            }
                        }
                        //}
                    }
                }
            }
            return null;
        }

        public static float? TryGetScalarValue(string fullPath, string ScalarValue)
        {
            //Example: "g_flLayerBorderSoftness"	"0.5"
            float output = -1;

            if (File.Exists(fullPath))
            {
                var matLines = File.ReadLines(fullPath);

                foreach (string line in matLines)
                {
                    if (!line.Contains("//") && line.Contains(ScalarValue))
                    {
                        //if (line.Substring(line.IndexOf(ScalarValue) + ScalarValue.Length - 1) == "\"")
                        //{
                        string[] splited = line.Split("\"");
                        if (splited.Length > 0 && (splited.Length - 2) >=0)
                        {
                            var outputString = splited[splited.Length - 2];
                            if (float.TryParse(outputString, out output))
                            {
                                return output;
                            }
                        }
                        //}
                    }
                }
            }
            return null;
        }
    }
}