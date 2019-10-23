using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Head_Chest_rotation_solver : MonoBehaviour
{
    public GameObject hud;
    public GameObject hud2;
    public GameObject hud3;
    private Text disp;

    public GameObject controller;

    public GameObject headcontrol;
    public GameObject chestcontrol;
    private Transform headtrans;
    private Transform chesttrans;
    private Transform chestorigin;
    private Transform controllerT;
    private Quaternion headQ;
    private float headY;
    private float deltaR;
    private bool negativedir;
    private bool middledir;
    private bool posativedir;
    private float displ;
    private float deltay;
    private float deltaz;
    private float deltax;
    // private bool positivedir;

    private Vector3 headorigin;
    private Quaternion chestdirp;
    private Quaternion chestdirn;



    void Start()
    {
        headtrans = headcontrol.GetComponent<Transform>();
        chesttrans = chestcontrol.GetComponent<Transform>();
        headorigin = headtrans.localEulerAngles;
        if (hud)
            disp = hud.GetComponent<Text>();
        if (controllerT)
            controllerT = controller.GetComponent<Transform>();
        chestorigin = chesttrans;

        chestdirp = Quaternion.Euler(new Vector3(25, 0, 0));
        chestdirn = Quaternion.Euler(new Vector3(-25, 0, 0));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        headQ = headtrans.rotation;
        headY = headQ.eulerAngles.y;
        float t = 0f;
        t += .5f * Time.deltaTime;

        float d = chesttrans.localRotation.x;
        float v = chesttrans.localRotation.y;
        float c = chesttrans.localRotation.z;
        float b = chesttrans.localRotation.w;

        //disp.text = d.ToString();



        if ((headY < 40) && (headY > 15))
        {
            // print("yes");
            // deltaR = headY - 18;
            //if(middledir == true) headorigin = headtrans.localEulerAngles;
            middledir = false;
            negativedir = true;

        }

        if ((headY < 350) && (headY > 320))
        {

            middledir = false;
            posativedir = true;

        }

        if (((headY > 0) && (headY < 15)) || ((headY < 360) && (headY > 350)))
        {
            middledir = true;
            negativedir = false;
            posativedir = false;
        }

        if (negativedir == true)
        {



            //chesttrans.localEulerAngles = new Vector3(Mathf.Lerp(chesttrans.localEulerAngles.x, -5f, t), Mathf.Lerp(chesttrans.localEulerAngles.y, chesttrans.localEulerAngles.y + 0, t), Mathf.Lerp(chesttrans.localEulerAngles.z, chesttrans.localEulerAngles.z + 0, t));

            chesttrans.localRotation = new Quaternion(Mathf.Lerp(chesttrans.localRotation.x, -.2629f, t), Mathf.Lerp(chesttrans.localRotation.y, 0f, t), Mathf.Lerp(chesttrans.localRotation.z, 0f, t), Mathf.Lerp(chesttrans.localRotation.w, .977f, t));

            // print("go");
        }

        if (posativedir == true)
        {
            chesttrans.localRotation = new Quaternion(Mathf.Lerp(chesttrans.localRotation.x, .2114f, t), Mathf.Lerp(chesttrans.localRotation.y, 0f, t), Mathf.Lerp(chesttrans.localRotation.z, 0f, t), Mathf.Lerp(chesttrans.localRotation.w, .983f, t));

        }

        if (middledir == true)
        {
            chesttrans.localRotation = new Quaternion(Mathf.Lerp(chesttrans.localRotation.x, 0, t), Mathf.Lerp(chesttrans.localRotation.y, 0, t), Mathf.Lerp(chesttrans.localRotation.z, 0, t), 1);
            // print("stop");
        }


    }
}
