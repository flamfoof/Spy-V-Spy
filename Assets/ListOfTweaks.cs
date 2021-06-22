using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfTweaks : MonoBehaviour {

	/*Player
	PlayerController script
		Everything on there is tweakable. Already defined in the inspector.
		EmittorWaitTime - the time to wait until you can place another emittor.
	*/
	 
	/* Sentry
	SentryBotAI script
		Sentry Speed, acceleration, angular speed, auto brake (recommended on) are all tweakable in the inspector.
		xRadius is the entire circle in which it detects the player.
		EmittorRemovalTime - how long the sentry stays still to get rid of emittor (once an emittor is placed, it will walk towards it immediately)
		TimeToNoticeEmittor - It doesn't notice it right away, but will chase it after the x amount of seconds.
		Debug parts help find out how big the area affected is. Will remove before build.
		
	FieldOfView script
		viewRadius should be the same as xRadius. Also helps to find the area where a player is.
		//I need to make it smaller
		viewAngle how wide the view of the sentry is.
		
	*/ 
	
	/* Bomb
	BombUI script
		Can adjust the timeRemaining. No need for the others.
	*/

	/* Scene Changes
	BombUI script
		UpdateBombTimer() - lose screen for when the bomb timer reaches 0 from the time it starts.
	MainMenu script
		Go to the Canvas in the Pause Menu for Pause Button and toMainMenu button.
	Main Menu Scene
		All the buttons.
	Collision Detector script
		Under the other.tag == "Player". This is where the player gets caught.

	*/
	
	//have a red light
}
