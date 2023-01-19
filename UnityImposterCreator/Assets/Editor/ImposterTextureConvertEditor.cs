using System;
using UnityEngine;
using UnityEditor;

public class ImposterTextureConvertEditor : AssetPostprocessor
{
    private const string TARGET_FOLDER = "Assets/ImposterGenerator/";

    private void OnPreprocessTexture()
    {
        if (!assetPath.Contains(TARGET_FOLDER))
        {
            return;
        }

        var importer = (TextureImporter) assetImporter;
        importer.textureType = TextureImporterType.Sprite;
    }
}