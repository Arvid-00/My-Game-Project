using UnityEngine;

public class MousePosition2D : MonoBehaviour
{
    public static Vector2 GetMouseWorldPos2D()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        return mouseWorldPos;
    }
}
