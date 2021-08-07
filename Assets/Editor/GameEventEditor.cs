using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEvent))]
public class GameEventEditor : Editor
{
    public void OnEnable()
    {
        if (target == null)
        {
            target = target as GameEvent;
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (GameEvent)target;

        if(GUILayout.Button("Raise"))
        {
            script.Raise();
        }
    }
}
