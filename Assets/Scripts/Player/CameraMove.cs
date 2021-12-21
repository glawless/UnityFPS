using UnityEngine;
using Photon.Pun;
using UnityEngine.Rendering.PostProcessing;

public class CameraMove : MonoBehaviour
{
    PhotonView PV;
    public GameObject playerMesh;

    private void Start()
    {
        PV = transform.root.gameObject.GetComponent<PhotonView>();
        if (!PV.IsMine)
        {
            playerMesh.layer = 0;
            //gameObject.SetActive(false);
            Behaviour[] comps = GetComponents<Behaviour>();
            foreach (var comp in comps)
            {
                if (!comp.GetType().Name.Equals("PhotonTransformView"))
                    comp.enabled = false;
            }
        }
    }

    public void MoveCam(Transform moveTo)
    {
        if (moveTo != null && transform.position.y != moveTo.position.y)
        {
            transform.position = moveTo.position;
        }
    }
}
