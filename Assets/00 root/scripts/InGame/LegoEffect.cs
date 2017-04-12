using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegoEffect : MonoBehaviour {

    public List<SetEffect> m_setEffectList;

    [System.Serializable]
    public class SetEffect
    {
        public bool attach = false;
        public GameObject effectPrefab;
        public GameObject boneLink;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
