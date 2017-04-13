using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eff_size_controller : MonoBehaviour {
    public ParticleSystem m_ParticleSystem;
    public List<Vector2> m_lstEffectLevel;
    public int currentIx = 0;

    public void Set_EffectLevel(int ix)
    {
        if( ix < 0 || m_lstEffectLevel.Count <= ix)
        {
            Debug.LogError(" error ==> ix < 0 || m_lstEffectLevel.Count <= ix");
            return;
        }
        currentIx = ix;
        Vector2 tmp = m_lstEffectLevel[ix];

        var pmain = m_ParticleSystem.main;
        pmain.startSize = new ParticleSystem.MinMaxCurve(tmp.x, tmp.y);

    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
