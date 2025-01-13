
This package offers two straightforward methods for integrating the 2D Platformer Controller Script into your Unity project:

1-Using the Prefab:

-I've provided a ready-to-use prefab for quick implementation. Simply drag and drop the prefab into your level scene.

-Customize the controller's settings by adjusting the values in the inspector panel to suit your game's requirements.

2-Starting from Scratch:

-If you prefer to start with your own setup or integrate the controller into an existing character, follow these steps:
-Import the package into your Unity project.
-Create an empty game object to serve as the parent for your character.
-Attach the platformer controller script to this parent object.
-Inside this parent object, add a child game object with a Sprite Renderer component to represent your character visually.
-If you want animations, add an Animator component to the child object and set up the animations accordingly.
-Customize the controller's settings via the inspector to tailor it to your game's needs.