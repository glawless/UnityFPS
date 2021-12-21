using UnityEngine;
using Photon.Pun;

public class CharacterAnimation : MonoBehaviour
{

    private Animator anim;
    private PlayerController pcon;
    private PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        pcon = GetComponentInParent<PlayerController>();
        PV = GetComponentInParent<PhotonView>();
    }

    // Update is called once per frame

    private void Update()
    {
        if (!PV.IsMine && PhotonNetwork.IsConnected)
            return;

        //MOVEMENT ANIMATIONS
       anim.SetFloat("InputX", pcon.xMove);
       anim.SetFloat("InputY", pcon.zMove);

        //JUMP ANIMATIONS
        if (pcon.isGrounded)
            anim.SetInteger("JumpCondition", 0);
        else 
            anim.SetInteger("JumpCondition", 1);

        //CROUCHING/SLIDING ANIMATIONS
        if (pcon.isSliding)
            anim.SetInteger("SlideCondition", 1);
        else if (pcon.isCrouching)
        {
            anim.SetInteger("CrouchCondition", 2);
            anim.SetInteger("SlideCondition", 0);

            anim.SetFloat("InputXC", pcon.xMove);
            anim.SetFloat("InputYC", pcon.zMove);
        }
        else
        {
            anim.SetInteger("SlideCondition", 0);
            anim.SetInteger("CrouchCondition", 0);
        }

        
    }
}
