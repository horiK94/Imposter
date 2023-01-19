using UnityEngine;
using UnityEngine.Assertions;

public class ImposterSpriteBillboard : MonoBehaviour
{
   [SerializeField] private int widthCount = 0;
    [SerializeField] private int heightCount = 0;

    [SerializeField] private Vector2 cameraWidthRange = new Vector2(0, 360);
    [SerializeField] private Vector2 cameraHeightRange = new Vector2(0, 180);

    [SerializeField] private Renderer spriteRenderer = null;

    private const string WIDTH_COUNT = "_WidthCount";
    private const string HEIGHT_COUNT = "_HeightCount";
    private const string USE_POINT_NAME = "_UsePoint";

    private Material material = null;
    private Transform cameraTransform = null;

    private int nameId = 0;
    private Vector2 divideDuration = Vector2.zero;
    private Vector2Int useSpritePoint = -Vector2Int.one;

    private void Start()
    {
        Assert.IsTrue(widthCount >= 2, "widthCount は2以上を指定して下さい");
        Assert.IsTrue(heightCount >= 2, "heightCount は2以上を指定して下さい");
        Assert.IsNotNull(spriteRenderer, "materialが指定されていません");
        Assert.IsTrue(cameraWidthRange.x < cameraWidthRange.y, "cameraWidthRangeのxはyより小さい値を指定する必要があります");
        Assert.IsTrue(cameraHeightRange.x < cameraHeightRange.y, "cameraHeightRangeのxはyより小さい値を指定する必要があります");
        Assert.IsTrue(cameraWidthRange.x >= 0 && cameraWidthRange.x <= 360, "cameraWidthRange.xは0 ~ 360で指定して下さい");
        Assert.IsTrue(cameraWidthRange.y >= 0 && cameraWidthRange.y <= 360, "cameraWidthRange.yは0 ~ 360で指定して下さい");
        Assert.IsTrue(cameraHeightRange.x >= 0 && cameraHeightRange.x <= 180, "cameraHeightRange.xは0 ~ 180で指定して下さい");
        Assert.IsTrue(cameraHeightRange.y >= 0 && cameraHeightRange.y <= 180, "cameraHeightRange.yは0 ~ 180で指定して下さい");

        material = spriteRenderer.material;
        cameraTransform = Camera.main.transform;

        int divideAreaX = cameraWidthRange.x == 0 && cameraWidthRange.y == 360
            ? widthCount
            : widthCount - 1;
        divideDuration = new Vector2((cameraWidthRange.y - cameraWidthRange.x) / divideAreaX,
            (cameraHeightRange.y - cameraHeightRange.x) / (heightCount - 1));

        nameId = Shader.PropertyToID(USE_POINT_NAME);
        material.SetInt(WIDTH_COUNT, widthCount);
        material.SetInt(HEIGHT_COUNT, heightCount);

        Vector2Int point = getUseSprite(calcAngle());
        setUseSprite(point);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posFromCamera = transform.position - cameraTransform.position;

        if (Mathf.Approximately(posFromCamera.x, 0) && Mathf.Approximately(posFromCamera.z, 0))
        {
            //真上か真下にカメラがあるとき画像が反転するのでひっくり返す
            transform.LookAt(posFromCamera, Vector3.up);
            Quaternion rotateY = Quaternion.AngleAxis(180, Vector3.forward);

            transform.rotation *= rotateY;
        }
        else
        {
            transform.LookAt(posFromCamera, Vector3.up);
        }

        Vector2Int point = getUseSprite(calcAngle());
        setUseSprite(point);
    }

    private Vector2 calcAngle()
    {
        Vector3 cameraVecForUp = Vector3.ProjectOnPlane(-transform.forward, Vector3.up);
        Vector3 rightForUp = Vector3.ProjectOnPlane(Vector3.right, Vector3.up);

        float xAngle = (Vector3.SignedAngle(cameraVecForUp, rightForUp, Vector3.up) + 360) % 360;

        Vector3 cameraVecForForward = Vector3.ProjectOnPlane(transform.forward, transform.right);
        Vector3 forwardForRight = Vector3.ProjectOnPlane(Vector3.down, transform.right);

        float yAngle = Vector3.Angle(cameraVecForForward, forwardForRight);

        return new Vector2(xAngle, yAngle);
    }

    private Vector2Int getUseSprite(Vector2 angle)
    {
        Assert.IsTrue(angle.x >= cameraWidthRange.x && angle.x <= cameraWidthRange.y,
            "cameraWidthRange がサポートしていない角度が指定されています: " + angle.x);
        Assert.IsTrue(angle.y >= cameraHeightRange.x && angle.y <= cameraHeightRange.y,
            "サポートされていない角度が指定されています" + angle.y);

        if (angle.x < cameraWidthRange.x || angle.x > cameraWidthRange.y)
        {
            return Vector2Int.zero;
        }

        if (angle.y < cameraHeightRange.x || angle.y > cameraHeightRange.y)
        {
            return Vector2Int.zero;
        }

        if (cameraWidthRange.x == 0 && cameraWidthRange.y == 360)
        {
            //円周
            return new Vector2Int((int) ((angle.x + divideDuration.x * 0.5f + 360) % 360 / divideDuration.x),
                (int) ((angle.y - cameraHeightRange.x + divideDuration.y * 0.5f) / divideDuration.y));
        }
        else
        {
            return new Vector2Int((int) ((angle.x - cameraWidthRange.x + divideDuration.x * 0.5f) / divideDuration.x),
                (int) ((angle.y - cameraHeightRange.x + divideDuration.y * 0.5f) / divideDuration.y));
        }
    }

    private void setUseSprite(Vector2Int _usePoint)
    {
        if (useSpritePoint == _usePoint)
        {
            return;
        }

        useSpritePoint = _usePoint;

        material.SetVector(nameId, new Vector4(_usePoint.x, _usePoint.y));
    }
}
