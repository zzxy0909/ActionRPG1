using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BotController))]
[CanEditMultipleObjects]
public class edBot : Editor {

    
	// Use this for initialization
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI ();
        BotController l_bot = target as BotController;

        GUILayout.Label("< OP >");
       
            
        if (GUILayout.Button("test ani"))
        {
            l_bot.SetAvoid();
        }
    }

	
}
