using UnityEngine;
using UnityEngine.Events;
namespace Edgar.Unity
{
    public class TeleportInteract : InteractableBase
    {
        public Teleport teleport;
        public override void BeginInteract()
        {
            ShowText("Press E to teleport back");
        }

        public override void Interact()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Teleport and activate
                teleport.InteractTp();
            }
        }

        public override void EndInteract()
        {
            HideText();
        }

        public override bool IsInteractionAllowed()
        {
            return teleport.activated;
        }
    }
}
