using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{

    bool debug;

    public override void OnInspectorGUI()
    {
        debug = EditorGUILayout.Toggle("Debug Mode", debug);

        if(debug)
        {
            GameManager.DebugMode = true;
        }
        else
        {
            GameManager.DebugMode = false;
        }
    }
}
