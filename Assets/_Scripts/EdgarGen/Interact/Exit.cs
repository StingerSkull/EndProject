using UnityEngine;
using UnityEngine.InputSystem;
namespace Edgar.Unity
{
    /// <summary>
    /// Example implementation of an exit is activated by pressing E and loads the next level.
    /// </summary>
    public class Exit : InteractableBase
    {

        public override void BeginInteract()
        {
            ShowText("Press interact to exit the level");
        }

        public override void Interact()
        {
            GenManager.Instance.LoadNextLevel();
        }

        public override void EndInteract()
        {
            HideText();
        }
    }
}