using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
namespace Edgar.Unity
{
    public class Teleport : MonoBehaviour
    {
        public string otherTpName;
        public Teleport tp;
        public bool activated;
        public GameObject tpActivated;
        public GameObject tpInactivated;
        public Transform playerTransform;
        public CinemachineCamera virtualCamera;

        private void Start()
        {
            tp = GameObject.Find(otherTpName).GetComponent<Teleport>();
            tpActivated.SetActive(activated);
            tpInactivated.SetActive(!activated);
            virtualCamera = GameObject.Find("CinemachineCamera").GetComponent<CinemachineCamera>(); 
        }

        public void InteractTp()
        {
            if (!tp.activated)
            {
                tp.activated = true;
                tp.tpActivated.SetActive(tp.activated);
                tp.tpInactivated.SetActive(!tp.activated);
            }

            playerTransform.position = tp.transform.position;
            virtualCamera.PreviousStateIsValid = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (playerTransform == null && collision.CompareTag("Player"))
            {
                playerTransform = collision.transform.parent;
            }
        }
    }
}
