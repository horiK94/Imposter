#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

[CustomEditor(typeof(ImposterAtlasGenerator))]
public class ImposterAtlasGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ImposterAtlasGenerator atlasGenerator = target as ImposterAtlasGenerator;

        if (GUILayout.Button("Camera Put For Check"))
        {
            atlasGenerator.PutTemporaryCamera();
        }

        if (GUILayout.Button("Create Atlas"))
        {
            onClickCreateSpriteButton(atlasGenerator);
        }
    }

    private void onClickCreateSpriteButton(ImposterAtlasGenerator atlasGenerator)
    {
        if (!AssetDatabase.IsValidFolder("Assets/ImposterGenerator"))
        {
            AssetDatabase.CreateFolder("Assets", "ImposterGenerator");
        }

        if (!AssetDatabase.IsValidFolder($"Assets/ImposterGenerator/{atlasGenerator.OutputName}"))
        {
            AssetDatabase.CreateFolder("Assets/ImposterGenerator", $"{atlasGenerator.OutputName}");
        }

        atlasGenerator.CreateAtlas();
        AssetDatabase.Refresh();

        Object[] spriteAtlasObj = new Object[atlasGenerator.CameraHeightCount * atlasGenerator.CameraWidthCount];
        for (int heightIndex = 0; heightIndex < atlasGenerator.CameraHeightCount; heightIndex++)
        {
            for (int widthIndex = 0; widthIndex < atlasGenerator.CameraWidthCount; widthIndex++)
            {
                Object spriteObj = AssetDatabase.LoadAssetAtPath<Object>(
                    $"Assets/ImposterGenerator/{atlasGenerator.OutputName}/{heightIndex}_{widthIndex}.png");

                spriteAtlasObj[heightIndex * atlasGenerator.CameraWidthCount + widthIndex] = spriteObj;
            }
        }

        SpriteAtlas spriteAtlas = new SpriteAtlas();
        spriteAtlas.Add(spriteAtlasObj);

        SpriteAtlasUtility.PackAtlases(new[] {spriteAtlas}, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.CreateAsset(spriteAtlas, $"Assets/{atlasGenerator.OutputName}.spriteatlas");
    }
}
#endif