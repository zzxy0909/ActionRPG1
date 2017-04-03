using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Player_controller))]
[CanEditMultipleObjects]
public class edPlayer : Editor {

    
	// Use this for initialization
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI ();
        Player_controller l_Player = target as Player_controller;

        GUILayout.Label("< OP >");
        if (GUILayout.Button("setup inspecter"))
        {
            l_Player.Startup();
        }
    }

	
}
