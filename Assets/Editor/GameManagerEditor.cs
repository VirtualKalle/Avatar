using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AvatarGameManager))]
public class GameManagerEditor : Editor
{

    bool debug;

    public override void OnInspectorGUI()
    {
        debug = EditorGUILayout.Toggle("Debug Mode", debug);

        if(debug)
        {
            AvatarGameManager.DebugMode = true;
        }
        else
        {
            AvatarGameManager.DebugMode = false;
        }
    }
}
