using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusNbuff : MonoBehaviour {

    public string characterName = "";
    public int level = 1;
    public int atk = 0;
    public int def = 0;
    public int matk = 0;
    public int mdef = 0;
    public int exp = 0;
    public int maxExp = 100;
    public int maxHealth = 100;
    public int health = 100;
    public int maxMana = 100;
    public int mana = 100;
    public int statusPoint = 0;
    public bool dead = false;

    public class resist
    {
        public int poisonResist = 0;
		public int silenceResist = 0;
		public int webResist = 0;
		public int stunResist = 0;
    }
    resist statusResist ;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
