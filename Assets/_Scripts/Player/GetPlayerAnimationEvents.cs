using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class GetPlayerAnimationEvents : MonoBehaviour
{
    public UnityEvent animCast;

    public void CastAnimation()
    {
        animCast.Invoke();
    }

}
