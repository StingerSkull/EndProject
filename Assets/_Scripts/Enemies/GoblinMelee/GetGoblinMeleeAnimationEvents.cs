using UnityEngine;
using UnityEngine.Events;

public class GetGoblinMeleeAnimationEvents : MonoBehaviour
{
    public GoblinMelee goblinMelee;
    public UnityEvent death;

    public void Death()
    {
        death.Invoke();
    }

}
