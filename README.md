# Simple Third-Person Player Control

This package will give you everything you need for a basic third-person character controller that allows for independent camera rotation. It has built-in support for both keyboard/mouse control and gamepad control. It can also be used for splitscreen multiplayer if you add a [PlayerInputManager](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.PlayerInputManager.html) in your scene somewhere.

This third person controller is a simplified version of the one used in [Polyathlon](https://github.com/mvorsteg/Polyathlon).

## Initial Set-Up
1. This player controller uses the [new Unity Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/). Before importing the Third Person Player Control package, you need to import the new Unity Input System package.
    1. Go to Window->Package Manager.
    2. In the Package Manager, set the "Packages: " tab at the top to "Packages: Unity Registry."
    3. Locate the package called "Input System" and click "Install."
    4. A warning will appear asking you if you would like to enable the new backends. Click "No," as we will do this ourselves without disabling the old backends in the next step.
    5. We are going to tell Unity to use both input backends. To do this, go to Edit->Project Settings->Player, then under "Other Settings" find "Active Input Handling" beneath "Configuration" and set its value to "Both." This will allow you to use the traditional input system (for example, "Input.GetKey()") in addition to the new input system that this third person player controller uses.
3. Import [the package](https://github.com/grabermtw/Simple-Third-Person-Player-Control/releases/tag/v1.0.0) (Assets->Import Package->Custom Package).
4. The package will give you a prefab called ThirdPersonPlayer. You should be able to drag this directly into your scene and have a fully functioning third person player.

## Replacing the Default Character
It is easy to replace the default character in the ThirdPersonPlayer prefab assuming you have a character to replace it with that is structured similarly (the default character was created with Adobe Fuse, rigged with Mixamo, then imported into Unity with [this script](https://forum.unity.com/threads/script-for-importing-adobe-fuse-character-model-into-unity-fixes-materials.482093/)).

_If your character was originally created for [Second Floor Smash](https://github.com/grabermtw/Second-Floor-Smash), then this will be easy._

1. Open the ThirdPersonPlayer prefab.
2. Delete the character prefab that's currently a child of the ThirdPersonPlayer prefab (if you just imported the package, then it will be called "anders").
3. Drag in your new character's prefab and make it a child of the ThirdPersonPlayer prefab in place of Anders.
4. Make sure that the character's prefab (as in the gameObject with your character's name that is a child of ThirdPersonPlayer) has an Animator component attached.
    1. Set the Animator component's "Controller" field to be the "StandardCharacterAnim" that came with the package.
5. Attach the "PlayerAnimationEvents.cs" script to the character's prefab (as in the gameObject with your character's name that is a child of ThirdPersonPlayer).
6. On the ThirdPersonPlayer gameObject, find the ThirdPersonPlayerController.cs script that is attached to it. You will see a field for "Character Hips" which should be missing its value. You need to expand your character's gameObject in the hierarchy and find the character's hip bone in their rig. If your character was rigged with Mixamo, then the hip bone gameObject will be called "mixamorig:Hips". Drag this gameObject (mixamorig:Hips) into the "Character Hips" field of the ThirdPersonCharacterComponent.cs script on the ThirdPersonPlayer gameObject.
7. Adjust the capsule collider to fit your new character's stature.
8. Please note that the sample animations this package comes with are set to "Generic." If your character's animation type is not set to "Generic," you will need to change this either in your character's model's import settings, or you could change the animations to be whatever your character's animation type is in their import settings.

_Congratulations! You have a third-person character!_
