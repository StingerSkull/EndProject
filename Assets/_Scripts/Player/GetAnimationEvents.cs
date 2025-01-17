using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class GetAnimationEvents : MonoBehaviour
{
    public UnityEvent animCast;

    public void CastAnimation(int value)
    {
        switch (value)
        {
            case 0:
                animCast.Invoke();
                break;

            default:
                break;
        }
    }
}
