using UnityEngine;
using UnityEngine.Events;

public class Win : MonoBehaviour
{
    public UnityEvent onWin;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onWin.Invoke();
        }
    }
}
