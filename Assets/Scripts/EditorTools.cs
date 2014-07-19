#if UNITY_EDITOR // Editor tool, so we only want this in our editor builds
using UnityEditor;
using UnityEngine;

public class EditorTools : ScriptableObject
{
    private const float Sqrt3 = 1.732050807568877293527f;
    public const float PieceRadius = 0.5f;

    [MenuItem("GameObject/Snap To Hexagonal Coordinates")]
    public static void SnapToHex()
    {
        foreach (Transform t in Selection.transforms)
        {
            Undo.RecordObject(t, "Snap to Hexagonal Coordinates");
            float x = t.position.x, y = t.position.y;

            int r = Mathf.RoundToInt((Sqrt3*x - y)/(3f*PieceRadius));
            int g = Mathf.RoundToInt((-Sqrt3*x - y)/(3f*PieceRadius));
            int b = -(r + g);

            t.position = new Vector3(
                Sqrt3*PieceRadius*(b/2f + r),
                3f/2f*PieceRadius*b,
                t.position.z
                );
        }
    }
}
#endif
