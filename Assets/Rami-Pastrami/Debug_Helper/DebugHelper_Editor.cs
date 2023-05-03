// Used to add custom buttons to the Line tools, cause Unity doesnt allow proper fields for properties

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Rami.DebugHelper;
using VRC.Udon.Editor.ProgramSources.UdonGraphProgram.UI.GraphView.UdonNodes;

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
            
        }
        if (GUILayout.Button("Toggle Segment Text"))
        {
            
        }


    }
}
#endif