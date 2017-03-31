#pragma strict
var maxHpBar : Texture2D;
var hpBar : Texture2D;
var mpBar : Texture2D;
var expBar : Texture2D;
var maxHpBarPosition : Vector2 = new Vector2(20 , 20);
var hpBarPosition : Vector2 = new Vector2(152 , 48);
var mpBarPosition : Vector2 = new Vector2(152 , 71);
var expBarPosition : Vector2 = new Vector2(152 , 94);
var levelPosition : Vector2 = new Vector2(24 , 86);
var maxHpBarWidth : int = 310;
var maxHpBarHeigh : int = 115;
var barHeight : int = 19;
var expBarHeight : int = 8;
var textStyle : GUIStyle;
var hpTextStyle : GUIStyle;

var barMultiply : float = 1.6;

var player : GameObject;
private var hptext : int = 100;

private var maxHp : int;
private var hp : int;
private var maxMp : int;
private var mp : int;
private var maxExp : int;
private var exp : int;
private var lv : int;
private var currentHp : int;
private var currentMp : int;

function Awake(){
	 if(!player){
		player = this.gameObject;
	}
	hptext = 100 * barMultiply;
}

function Update(){
	maxHp = player.GetComponent(Status).maxHealth;
    hp = player.GetComponent(Status).health * 100 / maxHp *barMultiply;
    maxMp = player.GetComponent(Status).maxMana;
    mp = player.GetComponent(Status).mana * 100 / maxMp *barMultiply;
    maxExp = player.GetComponent(Status).maxExp;
    exp = player.GetComponent(Status).exp * 100 / maxExp *barMultiply;
    lv = player.GetComponent(Status).level;
    
    currentHp = player.GetComponent(Status).health;
    currentMp = player.GetComponent(Status).mana;

}

function OnGUI() {
    if(!player){
        return;
    }
    GUI.DrawTexture(Rect(maxHpBarPosition.x ,maxHpBarPosition.y ,maxHpBarWidth,maxHpBarHeigh), maxHpBar);
    GUI.DrawTexture(Rect(hpBarPosition.x ,hpBarPosition.y ,hp,barHeight), hpBar);
    GUI.DrawTexture(Rect(mpBarPosition.x ,mpBarPosition.y ,mp,barHeight), mpBar);
    GUI.DrawTexture(Rect(expBarPosition.x ,expBarPosition.y ,exp,expBarHeight), expBar);
    
    GUI.Label (Rect (levelPosition.x, levelPosition.y, 50, 50), lv.ToString() , textStyle);
    GUI.Label (Rect (hpBarPosition.x, hpBarPosition.y, hptext, barHeight), currentHp.ToString() + "/" + maxHp.ToString() , hpTextStyle);
    GUI.Label (Rect (mpBarPosition.x, mpBarPosition.y, hptext, barHeight), currentMp.ToString() + "/" + maxMp.ToString() , hpTextStyle);
 }
