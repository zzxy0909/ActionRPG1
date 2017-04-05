using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetChildMainCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(Camera.main != null)
        {
            Transform tmp = Camera.main.transform;
            tmp.SetParent(this.transform);
            tmp.localPosition = Vector3.zero;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
