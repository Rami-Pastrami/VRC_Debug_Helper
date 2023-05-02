// Used to add custom buttons to the Line tools, cause Unity doesnt allow proper fields for properties

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Rami.DebugHelper;

#if UNITY_EDITOR
[CustomEditor(typeof(DistanceLine))]
public class DebugHelper_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        //base.OnInspectorGUI();

        DistanceLine DL = (DistanceLine)target;

        if(GUILayout.Button("Toggle Center Text"))
        {
            DL.usingCenterText = !DL.usingCenterText;
        }
        if (GUILayout.Button("Toggle Segment Text"))
        {
            DL.usingSegmentText = !DL.usingSegmentText;
        }

        int _numPoints = 0;
        EditorGUILayout.IntField("Number of Vertices", _numPoints);

        if (GUILayout.Button("Apply Vertices count"))
        {
            Debug.Log(_numPoints);
        }
    }
}
#endif