using UnityEngine;

public class GrappleGun : MonoBehaviour
{
    private Vector3 grapplePoint;
    public LayerMask canGrapple;
    public Transform gunTip, cam, player;
    private float maxDistance = 100f;
    private SpringJoint joint;
    private bool isGrappling = false;


    private void Update()
    {


        if (Input.GetMouseButton(1) && !isGrappling)
        {
            StartGrapple();
        }

        //Suppress warning message as empty statement is intentional.
        #pragma warning disable CS0642 // Possible mistaken empty statement
        else if (Input.GetMouseButton(1)) ;
        #pragma warning restore CS0642 // Possible mistaken empty statement

        else
        {
            StopGrapple();
        }
    }



    private void OnDisable()
    {
        StopGrapple();
    }


    void StartGrapple()
    {
        isGrappling = true;
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxDistance, canGrapple))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            // The distance grapple will try to keep from grapple point
            joint.maxDistance = distanceFromPoint * 0.6f;
            joint.minDistance = distanceFromPoint * 0.25f;

            // Change these values to preference
            joint.spring = 10f; //Spring: Takeoff acceleration
            joint.damper = 7f; //Damper: how quickly you lose acceleration
            joint.massScale = 4.5f; //MassScale: applies weight to grapple (if 0, its like walking a dog on a leash)
        }
    }





    void StopGrapple()
    {
        isGrappling = false;
        Destroy(joint);
    }


    public bool IsGrappling()
    {
        return joint != null;
    }


    public Vector3 GetGrapplingPoint()
    {
        return grapplePoint;
    }

}

