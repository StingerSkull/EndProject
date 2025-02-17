using UnityEngine;
namespace Edgar.Unity
{
    /// <summary>
    /// Example implementation of an exit is activated by pressing E and loads the next level.
    /// </summary>
    public class Exit : InteractableBase
    {

        public override void BeginInteract()
        {
            ShowText("Press E to exit the level");
        }

        public override void Interact()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GenManager.Instance.LoadNextLevel();
            }
        }

        public override void EndInteract()
        {
            HideText();
        }
    }
}