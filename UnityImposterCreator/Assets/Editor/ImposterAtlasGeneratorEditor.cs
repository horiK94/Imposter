#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ImposterAtlasGenerator))]
public class ImposterAtlasGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ImposterAtlasGenerator generator = target as ImposterAtlasGenerator;

        if (GUILayout.Button("Camera Put For Check"))
        {
            generator.PutTemporaryCamera();
        }
        if (GUILayout.Button("Create Sprite"))
        {
            generator.CreateSprite();
            AssetDatabase.Refresh();
        }
    }
}
#endif
