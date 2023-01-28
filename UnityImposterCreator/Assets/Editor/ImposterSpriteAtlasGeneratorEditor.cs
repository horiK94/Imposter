#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

[CustomEditor(typeof(ImposterSpriteAtlasGenerator))]
public class ImposterSpriteAtlasGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ImposterSpriteAtlasGenerator spriteAtlasGenerator = target as ImposterSpriteAtlasGenerator;

        if (GUILayout.Button("Camera Put For Check"))
        {
            spriteAtlasGenerator.PutTemporaryCamera();
        }

        if (GUILayout.Button("Create Atlas"))
        {
            onClickCreateSpriteButton(spriteAtlasGenerator);
        }
    }

    private void onClickCreateSpriteButton(ImposterSpriteAtlasGenerator spriteAtlasGenerator)
    {
        if (!AssetDatabase.IsValidFolder("Assets/ImposterGenerator"))
        {
            AssetDatabase.CreateFolder("Assets", "ImposterGenerator");
        }

        if (!AssetDatabase.IsValidFolder($"Assets/ImposterGenerator/{spriteAtlasGenerator.OutputName}"))
        {
            AssetDatabase.CreateFolder("Assets/ImposterGenerator", $"{spriteAtlasGenerator.OutputName}");
        }

        spriteAtlasGenerator.CreateAtlas();
        AssetDatabase.Refresh();

        Object[] spriteAtlasObj = new Object[spriteAtlasGenerator.CameraHeightCount * spriteAtlasGenerator.CameraWidthCount];
        for (int heightIndex = 0; heightIndex < spriteAtlasGenerator.CameraHeightCount; heightIndex++)
        {
            for (int widthIndex = 0; widthIndex < spriteAtlasGenerator.CameraWidthCount; widthIndex++)
            {
                Object spriteObj = AssetDatabase.LoadAssetAtPath<Object>(
                    $"Assets/ImposterGenerator/{spriteAtlasGenerator.OutputName}/{heightIndex}_{widthIndex}.png");

                spriteAtlasObj[heightIndex * spriteAtlasGenerator.CameraWidthCount + widthIndex] = spriteObj;
            }
        }

        SpriteAtlas spriteAtlas = new SpriteAtlas();
        spriteAtlas.Add(spriteAtlasObj);

        SpriteAtlasUtility.PackAtlases(new[] {spriteAtlas}, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.CreateAsset(spriteAtlas, $"Assets/{spriteAtlasGenerator.OutputName}.spriteatlas");
    }
}
#endif