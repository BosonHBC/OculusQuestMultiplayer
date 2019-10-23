using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarManager : MonoBehaviour
{
    //AVATAR IS CHARACTER MODEL
    //RIG IS OVR CONTROLLER



    public GameObject avatarhand_L;
    public GameObject avatarhand_R;
    public GameObject avatarhead;
    public GameObject avatarcenter;
    public GameObject righand_L;
    public GameObject righand_R;
    public GameObject righead;
    public GameObject rigcenter;
    public GameObject mirror;

    //wrist joint controls pos while hand joint controlls rot
    public GameObject avatarhand_dir_L;
    public GameObject avatarhand_dir_R;

    //position
    private Vector3 avatarpos_L;
    private Vector3 avatarpos_R;
    private Vector3 rigpos_L;
    private Vector3 rigpos_R;
    private Vector3 rigorigin_L;
    private Vector3 rigorigin_R;
    private Vector3 rigcenterpos;
    private Vector3 avatarcenterpos;

    //rotation
    private Quaternion avatarheadrot;
    private Quaternion righeadrot;
    private Quaternion rigHoriginrot;
    private Quaternion avatarhand_L_rot;
    private Quaternion avatarhand_R_rot;
    private Quaternion righand_L_origin;
    private Quaternion righand_R_origin;
    private Quaternion righand_Lrot;
    private Quaternion righand_Rrot;


    //animation
    private Animator animcont;


    void Start()
    {
        
        animcont = gameObject.GetComponent<Animator>();
        //Debug.Log(rigpos_L);
    }

    // Update is called once per frame
    void Update()
    {
        Getposition();
        GetOrigin();
        CenterPlayer();
        Getdelta();
        AnimHandler();
    }

    void Getposition()
    {
        avatarpos_L = avatarhand_L.transform.localPosition;
        avatarpos_R = avatarhand_R.transform.localPosition;
        avatarheadrot = avatarhead.transform.localRotation;
        avatarhand_L_rot = avatarhand_dir_L.transform.localRotation;
        avatarhand_R_rot = avatarhand_dir_R.transform.localRotation;
        avatarcenterpos = avatarcenter.transform.position;
      
        rigpos_L = righand_L.transform.position;
        rigpos_R = righand_R.transform.position;
        righeadrot = righead.transform.rotation;
        righand_Rrot = righand_R.transform.rotation;
        righand_Lrot = righand_L.transform.rotation;
        rigcenterpos = rigcenter.transform.position;

    }


     void GetOrigin()
    {
        if(Input.GetButtonDown("Fire1") || OVRInput.GetDown( OVRInput.RawButton.A, OVRInput.Controller.RTouch))
        {
            rigorigin_R = rigpos_R;
            rigorigin_L = rigpos_L;
            rigHoriginrot = righeadrot;
            righand_L_origin = righand_L.transform.rotation;

        }
    }


    void Getdelta()
    {
        //positionCalc_hands
        Vector3 Rdelta;
        Rdelta = rigpos_R - rigorigin_R;
       // avatarhand_R.transform.localPosition = Rdelta;
        Vector3 Ldelta;
        Ldelta = rigpos_L - rigorigin_L;
       // avatarhand_L.transform.localPosition = Ldelta;

        avatarhand_L.transform.position = righand_L.transform.position;
        avatarhand_R.transform.position = righand_R.transform.position;


        //rotationCalc_head
        float Hdeltax;
        float Hdeltay;
        float Hdeltaz;
        Hdeltax = righeadrot.x - rigHoriginrot.x;
        Hdeltay = righeadrot.y - rigHoriginrot.y;
        Hdeltaz = righeadrot.z - rigHoriginrot.z;
        // Hdeltaz = Mathf.Lerp(-5.1f, 5.1f, Mathf.InverseLerp(-30f, 30f, Hdeltay));
        avatarhead.transform.rotation = new Quaternion(Hdeltax, Hdeltay, Hdeltaz, 1);
        
        
        //rotationCalc_hands
        float Ldeltax;
        float Ldeltay;
        float Ldeltaz;
        Ldeltax = righand_Lrot.x - righand_L_origin.x;
        Ldeltay = righand_Lrot.y - righand_L_origin.y;
        Ldeltaz = righand_Lrot.z - righand_L_origin.z;    
        avatarhand_dir_L.transform.rotation = new Quaternion(Ldeltax, Ldeltay, -Ldeltaz, 1);

        float Rdeltax;
        float Rdeltay;
        float Rdeltaz;
        Rdeltax = righand_Rrot.x - righand_R_origin.x;
        Rdeltay = righand_Rrot.y - righand_R_origin.y;
        Rdeltaz = righand_Rrot.z - righand_R_origin.z;
        avatarhand_dir_R.transform.rotation = new Quaternion(Rdeltax, Rdeltay, -Rdeltaz, 1);

    }

    void CenterPlayer()
    {

       // avatarcenterpos.x = rigcenterpos.x;
       // avatarcenterpos.z = rigcenterpos.z;
        avatarcenter.transform.position = new Vector3(rigcenterpos.x, avatarcenter.transform.position.y, rigcenterpos.z);
    }
    
    void AnimHandler()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger, OVRInput.Controller.RTouch))
        {
            animcont.SetBool("Rgrip", true);
            
        }
        if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger, OVRInput.Controller.RTouch))
        {
            animcont.SetBool("Rgrip", false);

        }
        if (OVRInput.GetDown(OVRInput.RawButton.LHandTrigger))
        {   
                animcont.SetBool("Lgrip", true);

        }
        if (OVRInput.GetUp(OVRInput.RawButton.LHandTrigger))
        {
            animcont.SetBool("Lgrip", false);

        }

    }

   
}
