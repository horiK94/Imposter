#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ImposterSpriteGenerator))]
public class ImposterSpriteGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ImposterSpriteGenerator generator = target as ImposterSpriteGenerator;

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
