# VRHideNSeek2020
 VR Hide & Seek

 Kyle VanSteelandt
 109677339
 Virtual Reality
 CSCI 4800
 12/09/2020

Engine Used:
Unity 2020.1.6f1

Target Device:
Oculus Quest

Controls:
Left Thumbstick to move the player.
Player can physically crouch to get into small areas.
Triggers on either hand are used to grab boxes, releasing the trigger will drop the box
Hold the Oculus button to recenter the player on the collision model

Seeker Modes:
Red = Has visual of Player
Green = Wandering
Yellow = Stunned/Deactivated/Moving to Start Location
Blue = Lost Sight of Player, Searching area of the last know location
White = Heard another Seeker get stunned by a Box
Black = GAME OVER

Game Rules:
Player has 30 seconds to find a spot before the Seekers leave the Start Room. Once the Seekers leave the room they will respond to Visual and Audio checks. If the Seekers get close to the player the game is over and a new round will start.

Known Issues:
Sometimes the Seekers hit a box that is laying on the floor- This causes the nearby Seekers to investigate, I left it in because it can lead to some fun stratgies that can allow the player to get away.

Player camera is not anchored to the character collider- There is code in the ColliderFollowPlayer.cs script that will anchor the camera but currently if the player turns their head that is also seen as movement which causes the player to zip through the level.

Sometimes Seekers seem to be able to see through walls - I am not sure about this one, it could be because the player camera is actually centered around the center of the playercontroller. This gives the player some extra height which may be the issue. It could also be that some walls have "holes" in them.

The player timer doesnt stop at the end of the game - Stopping the timer froze the game and I haven't gotten back to it after commenting it out.

Seekers don't play the Game Over laughter-  This is because the WaitForSeconds in the IEnumerator is not working correctly. Low priority because the audio isn't really crucial.