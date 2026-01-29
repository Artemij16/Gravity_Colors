using UnityEngine;

public class CameraBoundsAnchor : MonoBehaviour
{
    public enum BorderSide { Left, Right, Top, Bottom }
    public BorderSide side;
    public float offset = 0f;

    void Update()
    {
        AnchorToCamera();
    }

    void AnchorToCamera()
    {
        Camera cam = Camera.main;
        Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.nearClipPlane));

        Vector3 pos = transform.position;

        switch (side)
        {
            case BorderSide.Left:
                pos.x = bottomLeft.x + offset;
                transform.localScale = new Vector3(transform.localScale.x, (topRight.y - bottomLeft.y), 1);
                break;
            case BorderSide.Right:
                pos.x = topRight.x - offset;
                transform.localScale = new Vector3(transform.localScale.x, (topRight.y - bottomLeft.y), 1);
                break;
            case BorderSide.Top:
                pos.y = topRight.y - offset;
                transform.localScale = new Vector3((topRight.x - bottomLeft.x), transform.localScale.y, 1);
                break;
            case BorderSide.Bottom:
                pos.y = bottomLeft.y + offset;
                transform.localScale = new Vector3((topRight.x - bottomLeft.x), transform.localScale.y, 1);
                break;
        }

        transform.position = pos;
    }
}