using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CreateQuad))]
public class CreateQuadEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        CreateQuad myScript = (CreateQuad)target;
        if (GUILayout.Button("Regenerate")) 
        {
            myScript.GenerateCube();
            myScript.GenerateMesh();
        }
    }
}
