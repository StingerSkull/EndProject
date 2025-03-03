using UnityEngine;
using UnityEngine.Events;
namespace Edgar.Unity
{
    public class TeleportInteract : InteractableBase
    {
        public Teleport teleport;
        public override void BeginInteract()
        {
            ShowText("Press interact to teleport back");
        }

        public override void Interact()
        {
            //Teleport and activate
            teleport.InteractTp();
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
