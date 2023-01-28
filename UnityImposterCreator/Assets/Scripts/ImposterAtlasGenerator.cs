using UnityEngine;
using UnityEngine.Assertions;
using System.IO;

public class ImposterAtlasGenerator : MonoBehaviour
{
    [SerializeField] private float cameraDistance = 5f;
    [SerializeField] private float cameraHeight = 0;

    [SerializeField] private int cameraWidthCount = 0;
    [SerializeField] private int cameraHeightCount = 0;

    [SerializeField] private GameObject targetPrefab = null;
    [SerializeField] private Vector2 cameraWidthRange = new Vector2(0, 360);
    [SerializeField] private Vector2 cameraHeightRange = new Vector2(0, 180);

    [SerializeField] private Vector2Int cameraSize = Vector2Int.zero;
    [SerializeField] private string outputName = null;

    private GameObject createdPrefab = null;
    private Camera[,] captureCameras = new Camera[0, 0];

    public void CreateSprite()
    {
        Assert.IsTrue(cameraDistance > 0, "カメラの距離は0より大きい値にして下さい");
        Assert.IsTrue(cameraWidthCount >= 2, "カメラの横の個数は2以上を指定して下さい");
        Assert.IsTrue(cameraHeightCount >= 2, "カメラの縦の個数は2以上を指定して下さい");
        Assert.IsTrue(cameraSize.x > 0 && cameraSize.y > 0, "カメラサイズは1以上を指定して下さい");
        Assert.IsFalse(string.IsNullOrEmpty(outputName), "出力パスを指定して下さい");
        Assert.IsTrue(cameraWidthRange.x < cameraWidthRange.y, "cameraWidthRangeのxはyより小さい値を指定する必要があります");
        Assert.IsTrue(cameraHeightRange.x < cameraHeightRange.y, "cameraHeightRangeのxはyより小さい値を指定する必要があります");
        Assert.IsTrue(cameraWidthRange.x >= 0 && cameraWidthRange.x <= 360, "cameraWidthRange.xは0 ~ 360で指定して下さい");
        Assert.IsTrue(cameraWidthRange.y >= 0 && cameraWidthRange.y <= 360, "cameraWidthRange.yは0 ~ 360で指定して下さい");
        Assert.IsTrue(cameraHeightRange.x >= 0 && cameraHeightRange.x <= 180, "cameraHeightRange.xは0 ~ 180で指定して下さい");
        Assert.IsTrue(cameraHeightRange.y >= 0 && cameraHeightRange.y <= 180, "cameraHeightRange.yは0 ~ 180で指定して下さい");

        Debug.Log("Make");

        createTargetPrefab();
        RenderTexture renderTexture = createCamera();
        createSprite(renderTexture);
    }

    public void PutTemporaryCamera()
    {
        Assert.IsTrue(cameraDistance > 0, "カメラの距離は0より大きい値にして下さい");
        Assert.IsTrue(cameraWidthCount >= 2, "カメラの横の個数は2以上を指定して下さい");
        Assert.IsTrue(cameraHeightCount >= 2, "カメラの縦の個数は2以上を指定して下さい");
        Assert.IsTrue(cameraSize.x > 0 && cameraSize.y > 0, "カメラサイズは1以上を指定して下さい");
        Assert.IsTrue(cameraWidthRange.x < cameraWidthRange.y, "cameraWidthRangeのxはyより小さい値を指定する必要があります");
        Assert.IsTrue(cameraHeightRange.x < cameraHeightRange.y, "cameraHeightRangeのxはyより小さい値を指定する必要があります");
        Assert.IsTrue(cameraWidthRange.x >= 0 && cameraWidthRange.x <= 360, "cameraWidthRange.xは0 ~ 360で指定して下さい");
        Assert.IsTrue(cameraWidthRange.y >= 0 && cameraWidthRange.y <= 360, "cameraWidthRange.yは0 ~ 360で指定して下さい");
        Assert.IsTrue(cameraHeightRange.x >= 0 && cameraHeightRange.x <= 180, "cameraHeightRange.xは0 ~ 180で指定して下さい");
        Assert.IsTrue(cameraHeightRange.y >= 0 && cameraHeightRange.y <= 180, "cameraHeightRange.yは0 ~ 180で指定して下さい");

        createTargetPrefab();
        createCamera();
    }

    private RenderTexture createCamera()
    {
        //再配置前のものは全部捨てる
        for (int heightInd = 0; heightInd < captureCameras.GetLength(0); heightInd++)
        {
            for (int widthInd = 0; widthInd < captureCameras.GetLength(1); widthInd++)
            {
                if (captureCameras[heightInd, widthInd] != null)
                {
                    DestroyImmediate(captureCameras[heightInd, widthInd].gameObject);
                }
            }
        }

        int divideAreaX = cameraWidthRange.x == 0 && cameraWidthRange.y == 360
            ? cameraWidthCount
            : cameraWidthCount - 1;
        float durationWidthAngle = (cameraWidthRange.y - cameraWidthRange.x) * Mathf.Deg2Rad / divideAreaX;
        float durationHeightAngle =
            (cameraHeightRange.y - cameraHeightRange.x) * Mathf.Deg2Rad / (cameraHeightCount - 1);

        captureCameras = new Camera[cameraHeightCount, cameraWidthCount];

        var renderTexture = new RenderTexture(cameraSize.x * cameraWidthCount, cameraSize.y * cameraHeightCount, 24);
        RenderTexture.active = renderTexture;

        Vector3 targetPos = new Vector3(0, cameraHeight, 0);

        float heightAngle = cameraHeightRange.x;
        for (int heightInd = 0; heightInd < cameraHeightCount; heightInd++)
        {
            float widthAngle = cameraWidthRange.x;
            for (int widthInd = 0; widthInd < cameraWidthCount; widthInd++)
            {
                GameObject obj = new GameObject("Camera");
                obj.transform.parent = transform;

                Transform cameraTrans = obj.transform;
                Camera camera = obj.AddComponent<Camera>();
                captureCameras[heightInd, widthInd] = camera;

                Vector3 cameraPos = new Vector3(Mathf.Sin(heightAngle) * Mathf.Cos(widthAngle),
                    Mathf.Cos(heightAngle) + cameraHeight,
                    Mathf.Sin(heightAngle) * Mathf.Sin(widthAngle)
                );
                cameraTrans.position = cameraPos * cameraDistance;

                //カメラへの向きベクトル
                Vector3 cameraYZeroOnSphere = targetPos -
                                              new Vector3(Mathf.Cos(widthAngle), cameraHeight, Mathf.Sin(widthAngle));
                cameraYZeroOnSphere.Normalize();
                Vector3 widthNormalVec = Vector3.Cross(cameraYZeroOnSphere, Vector3.up);

                //上方向のベクトルの算出
                Vector3 upVec = Vector3.Cross(widthNormalVec, targetPos - cameraPos);

                cameraTrans.LookAt(new Vector3(0, cameraHeight, 0), upVec);

                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = Color.clear;

                camera.rect = new Rect((float) widthInd / cameraWidthCount, (float) heightInd / cameraHeightCount,
                    1f / cameraWidthCount, 1f / cameraHeightCount);

                camera.targetTexture = renderTexture;

                widthAngle += durationWidthAngle;
            }

            heightAngle += durationHeightAngle;
        }

        return renderTexture;
    }

    private void createTargetPrefab()
    {
        if (createdPrefab != null)
        {
            DestroyImmediate(createdPrefab);
        }

        createdPrefab = Instantiate(targetPrefab, Vector3.zero, Quaternion.identity);
        createdPrefab.transform.parent = transform;
    }

    private void createSprite(RenderTexture _renderTexture)
    {
        for (int heightInd = 0; heightInd < cameraHeightCount; heightInd++)
        {
            for (int widthInd = 0; widthInd < cameraWidthCount; widthInd++)
            {
                captureCameras[heightInd, widthInd].Render();
            }
        }

        var texture = new Texture2D(cameraSize.x * cameraWidthCount, cameraSize.y * cameraHeightCount);
        texture.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);

        File.WriteAllBytes(
            $"{Application.dataPath}/{outputName}.png",
            texture.EncodeToPNG());
    }
}
