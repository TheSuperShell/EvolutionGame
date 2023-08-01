using UnityEngine;
using UnityEditor;
using TheSuperShell.Microbes;


namespace TheSuperShell
{
    [CustomEditor(typeof(FieldOfView))]
    public class FOVEditor : Editor
    {
        private void OnSceneGUI()
        {
            FieldOfView fov = (FieldOfView)target;
            Handles.color = Color.white;
            Handles.DrawWireArc(fov.transform.position, Vector3.back, Vector3.right, 360, fov.Radius);
            Vector3 vieweAngleA = fov.DirFromAngle(-fov.Angle / 2, false);
            Vector3 vieweAngleB = fov.DirFromAngle(fov.Angle / 2, false);

            Handles.DrawLine(fov.transform.position, fov.transform.position + vieweAngleA * fov.Radius);
            Handles.DrawLine(fov.transform.position, fov.transform.position + vieweAngleB * fov.Radius);
        }
    }
}
