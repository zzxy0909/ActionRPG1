using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiStartObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GuiMgr.Instance.Show<Gui_PlayUI>(GuiMgr.ELayerType.Back, true, null);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
