#pragma strict
//////////////////////////////////////////////////////////////
// Joystick.js
// Modify from Penelope iPhone Tutorial
//
// Joystick creates a movable joystick (via GUITexture) that
// handles touch input, taps, and phases. Dead zones can control
// where the joystick input gets picked up and can be normalized.
//
// Optionally, you can enable the touchPad property from the editor
// to treat this Joystick as a TouchPad. A TouchPad allows the finger
// to touch down at any point and it tracks the movement relatively
// without moving the graphic
//////////////////////////////////////////////////////////////

@script RequireComponent( GUITexture )

var player : GameObject;
// A simple class for bounding how far the GUITexture will move
class Boundary{
		var min : Vector2 = Vector2.zero;
		var max : Vector2 = Vector2.zero;
}

static private var joysticks : MobileJoyStick[]; // A static collection of all joysticks
static private var enumeratedJoysticks : boolean = false;
static private var tapTimeDelta : float = 0.3; // Time allowed between taps

var touchPad : boolean; // Is this a TouchPad?
var touchZone : Rect;
var deadZone : Vector2 = Vector2.zero; // Control when position is output
var normalize : boolean = false; // Normalize output after the dead-zone?
var position : Vector2; // [-1, 1] in x,y
var tapCount : int; // Current tap count

private var lastFingerId : int = -1; // Finger last used for this joystick
private var tapTimeWindow : float; // How much time there is left for a tap to occur
private var fingerDownPos : Vector2;
private var fingerDownTime : float;
private var firstDeltaTime : float = 0.5;

private var gui : GUITexture; // Joystick graphic
private var defaultRect : Rect; // Default position / extents of the joystick graphic
private var guiBoundary : Boundary = Boundary(); // Boundary for joystick graphic
private var guiTouchOffset : Vector2; // Offset to apply to touch input
private var guiCenter : Vector2; // Center of joystick
var joyBackground : Transform;

function Start(){
		if(!player){
			player = GameObject.FindWithTag("Player");
		}
		gameObject.tag = "JoyStick";
		//DontDestroyOnLoad (transform.gameObject);
		var originalInset : Vector2;
		originalInset.x = GetComponent.<GUITexture>().pixelInset.x;
		originalInset.y = GetComponent.<GUITexture>().pixelInset.y;
		
		//Reset GUI Pixel Inset to original position
		GetComponent.<GUITexture>().pixelInset.x = originalInset.x;
		GetComponent.<GUITexture>().pixelInset.y = originalInset.y;
		
		transform.parent = null;
		transform.position.x = 0.0;
		transform.position.y = 0.0;
		if(joyBackground){
			joyBackground.parent = null;
			joyBackground.position.x = 0.0;
			joyBackground.position.y = 0.0;
			joyBackground.position.z = transform.position.z - 2;
		}
		// Cache this component at startup instead of looking up every frame
		gui = GetComponent( GUITexture );
		// Store the default rect for the gui, so we can snap back to it
		defaultRect = gui.pixelInset;
		defaultRect.x += transform.position.x * Screen.width;// + gui.pixelInset.x; // - Screen.width * 0.5;
		defaultRect.y += transform.position.y * Screen.height;// - Screen.height * 0.5;
		
		if ( touchPad ){
				// If a texture has been assigned, then use the rect ferom the gui as our touchZone
				if ( gui.texture )
				touchZone = defaultRect;
		}else{
				// This is an offset for touch input to match with the top left
				// corner of the GUI
				guiTouchOffset.x = defaultRect.width * 0.5;
				guiTouchOffset.y = defaultRect.height * 0.5;
				
				// Cache the center of the GUI, since it doesn't change
				guiCenter.x = defaultRect.x + guiTouchOffset.x;
				guiCenter.y = defaultRect.y + guiTouchOffset.y;
				
				// Let's build the GUI boundary, so we can clamp joystick movement
				guiBoundary.min.x = defaultRect.x - guiTouchOffset.x;
				guiBoundary.max.x = defaultRect.x + guiTouchOffset.x;
				guiBoundary.min.y = defaultRect.y - guiTouchOffset.y;
				guiBoundary.max.y = defaultRect.y + guiTouchOffset.y;
		}
}

function Disable(){
		//gameObject.active = false;
		gameObject.SetActive(false);
		enumeratedJoysticks = false;
}

function ResetJoystick(){
		// Release the finger control and set the joystick back to the default position
		gui.pixelInset = defaultRect;
		lastFingerId = -1;
		position = Vector2.zero;
		fingerDownPos = Vector2.zero;
		
		if ( touchPad )
			gui.color.a = 0.025;
}

function IsFingerDown() : boolean{
	return (lastFingerId != -1);
}

function LatchedFinger( fingerId : int ){
		// If another joystick has latched this finger, then we must release it
		if ( lastFingerId == fingerId )
		ResetJoystick();
}

function Update(){
		if(!player){
			if(joyBackground){
				Destroy(joyBackground.gameObject);
			}
			Destroy(gameObject);
		}
		
		//-----------------------------------------------
		for (var k = 0; k < Input.touchCount; ++k) {
			var toucha : Touch = Input.GetTouch(k);
			if (toucha.position.x < (Screen.width/2) && toucha.phase == TouchPhase.Ended || toucha.position.x < (Screen.width/2) && toucha.phase == TouchPhase.Canceled ){
				ResetJoystick();
			}
			//--------------
			if (toucha.phase == TouchPhase.Began) {
				if (toucha.position.x > (Screen.width/2) && toucha.phase == TouchPhase.Began) {
					return;
				}
			}
		}
		//-----------------------------------------------
		
		if (!enumeratedJoysticks ){
				// Collect all joysticks in the game, so we can relay finger latching messages
				joysticks = FindObjectsOfType( MobileJoyStick );
				enumeratedJoysticks = true;
		}
		var count : int = 0;

		if (Application.platform != RuntimePlatform.IPhonePlayer){
				if(Input.GetMouseButton(0)){
						count = 1;
						//Debug.Log("Mouse button: " + count);
				}
		}else{
				count = Input.touchCount;
		}
		//var count = Input.touchCount;
		// Adjust the tap time window while it still available
		if ( tapTimeWindow > 0 )
			tapTimeWindow -= Time.deltaTime;
		else
			tapCount = 0;
		
		if ( count == 0 )
			ResetJoystick();
		else{
			for(var i : int = 0;i < count; i++){
				var touch : Touch;
				var guiTouchPos : Vector2;
				var fingerID : int;
				var touchPosition : Vector2;

				if(Application.platform == RuntimePlatform.Android){
						touch = Input.GetTouch(i);
						guiTouchPos = touch.position - guiTouchOffset;
						fingerID = touch.fingerId;
						touchPosition = touch.position;
				}else if (Application.platform != RuntimePlatform.IPhonePlayer){
						guiTouchPos = Input.mousePosition - guiTouchOffset;
						fingerID = 1;
						touchPosition = Input.mousePosition;
				}else{
						touch = Input.GetTouch(i);
						guiTouchPos = touch.position - guiTouchOffset;
						fingerID = touch.fingerId;
						touchPosition = touch.position;
				}
				var shouldLatchFinger = false;
				if ( touchPad ){
						if ( touchZone.Contains( touchPosition ) )
						shouldLatchFinger = true;
				}else if ( gui.HitTest( touchPosition ) ){
						shouldLatchFinger = true;
				}
				
				// Latch the finger if this is a new touch
				if ( shouldLatchFinger && (lastFingerId == -1 || lastFingerId != fingerID)){
					if ( touchPad ){
							gui.color.a = 0.15;
							
							lastFingerId = fingerID;
							fingerDownPos = touchPosition;
							fingerDownTime = Time.time;
					}
				
					lastFingerId = fingerID;
				
				// Accumulate taps if it is within the time window
				if ( tapTimeWindow > 0 )
						tapCount++;
				else{
						tapCount = 1;
						tapTimeWindow = tapTimeDelta;
				}		
					// Tell other joysticks we've latched this finger
					for ( var j : MobileJoyStick in joysticks ){
							if ( j != this )
							j.LatchedFinger( fingerID );
					}
				}
				
				if ( lastFingerId == fingerID ){
				// Override the tap count with what the iPhone SDK reports if it is greater
				// This is a workaround, since the iPhone SDK does not currently track taps
				// for multiple touches
				if (Application.platform == RuntimePlatform.IPhonePlayer){
						if ( touch.tapCount > tapCount )
						tapCount = touch.tapCount;
				}
				
				if ( touchPad ){
						// For a touchpad, let's just set the position directly based on distance from initial touchdown
						position.x = Mathf.Clamp( ( touchPosition.x - fingerDownPos.x ) / ( touchZone.width / 2 ), -1, 1 );
						position.y = Mathf.Clamp( ( touchPosition.y - fingerDownPos.y ) / ( touchZone.height / 2 ), -1, 1 );
				}else{
						// Change the location of the joystick graphic to match where the touch is
						gui.pixelInset.x = Mathf.Clamp( guiTouchPos.x, guiBoundary.min.x, guiBoundary.max.x );
						gui.pixelInset.y = Mathf.Clamp( guiTouchPos.y, guiBoundary.min.y, guiBoundary.max.y );
				}
				
				if (Application.platform != RuntimePlatform.IPhonePlayer){
						if (!Input.GetMouseButton(0)){
							ResetJoystick();
							Debug.Log("Joystick Reset.");
						}
				}else{
						if ( touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled ){
							ResetJoystick();
							Debug.Log("Joystick Reset.");
						}
					}
				}
			}
		}
		
		if ( !touchPad ){
				// Get a value between -1 and 1 based on the joystick graphic location
				position.x = ( gui.pixelInset.x + guiTouchOffset.x - guiCenter.x ) / guiTouchOffset.x;
				position.y = ( gui.pixelInset.y + guiTouchOffset.y - guiCenter.y ) / guiTouchOffset.y;
		}
		
		// Adjust for dead zone
		var absoluteX = Mathf.Abs( position.x );
		var absoluteY = Mathf.Abs( position.y );
		
		if ( absoluteX < deadZone.x ){
			// Report the joystick as being at the center if it is within the dead zone
			position.x = 0;
		}else if ( normalize ){
			// Rescale the output after taking the dead zone into account
			position.x = Mathf.Sign( position.x ) * ( absoluteX - deadZone.x ) / ( 1 - deadZone.x );
		}
		
		if ( absoluteY < deadZone.y ){
			// Report the joystick as being at the center if it is within the dead zone
			position.y = 0;
		}else if ( normalize ){
			// Rescale the output after taking the dead zone into account
			position.y = Mathf.Sign( position.y ) * ( absoluteY - deadZone.y ) / ( 1 - deadZone.y );
		}
		/*if(touchPosition.x > Screen.width /2){
			ResetJoystick();
		}*/
		//print(touchPosition);
}