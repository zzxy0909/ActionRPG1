using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(eff_size_controller))]
[CanEditMultipleObjects]
public class edEff_size_controller : Editor {
     
	// Use this for initialization
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI ();
        eff_size_controller l_cont = target as eff_size_controller;

        GUILayout.Label("< OP >");
       
            
        if (GUILayout.Button("test level"))
        {
            l_cont.Set_EffectLevel(l_cont.currentIx);
        }
    }

	
}
