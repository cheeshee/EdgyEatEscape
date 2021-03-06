﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueProximity : MonoBehaviour
{
    Vector3 dialoguePos = new Vector3(0.0f, -6.0f, 5.0f);

    //resizing from Free Aspect
    private float refWidth = 898.0f;
    private float ratio;
    //5:4 ref
    private float width5 = 454.0f;
    //4:3 ref
    private float width4 = 484.0f;
    //3:2 ref
    private float width3 = 544.0f;
    //16:10 ref
    private float width10 = 581.0f;
    //16:9 ref
    private float width9 = 645.0f;

    //inputs for dialogue
    public string Name;
    public string Dialogue;

    //portraits
    public GameObject Portrait;
    private GameObject PortraitInst;

    //unique values for each character
    public float offX, offY;

    //exclamation point above head
    public GameObject exclam; //Public reference
    private GameObject exclamInst; //private clone

    //dialogue Box
    public DialogueBox dialogueBox; //public reference
    private DialogueBox dialogueBoxInst; //private clone

    //Empty
    public GameObject empty;
    private GameObject hanger;

    //text
    public Text texter;
    private Text nameTag;

    //control
    private bool talking;
    //private bool done;
    private float rangeSquare;
    private bool inRange;
    private GameObject player;
    private GameObject camera;

    //set for all objects that use this script
    private float range = 5.0f;
    private float offset = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        //set private variables
        float aspect = (float)Screen.width / (float)Screen.height;
        Debug.Log("width = " + Screen.width);
        Debug.Log("height = " + Screen.height);
        Debug.Log("aspect = " + aspect);
        //check aspect ratio
        if (aspect >= 16.0 / 9.0 - 0.01)
        {
            Debug.Log("16:9");
            ratio = width9 / refWidth;
        }
        else if (aspect >= 16.0 / 10.0 - 0.01)
        {
            Debug.Log("16:10");
            ratio = width10 / refWidth;
        }
        else if (aspect >= 3.0 / 2.0 - 0.01)
        {
            Debug.Log("3:2");
            ratio = width3 / refWidth;
        }
        else if (aspect >= 4.0 / 3.0 - 0.01)
        {
            Debug.Log("4:3");
            ratio = width4 / refWidth;
        }
        else if (aspect >= 5.0 / 4.0 - 0.01)
        {
            Debug.Log("5:4");
            ratio = width5 / refWidth;
        }
        else
        {
            Debug.Log("Free Aspect");
            ratio = 1.0f;
        }
        ratio *= 12.0f / 7.0f;

        rangeSquare = Mathf.Pow(range, 2);
        player = GameObject.FindGameObjectWithTag("Player");
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        inRange = false;
        //done = false;
        talking = false;
    }

    // Update is called once per frame
    void Update()
    {
        //check if the object is currently set to out of range, but the player object is now in the circular radius
        if (!inRange && ((Mathf.Pow(player.transform.position.x - gameObject.transform.position.x, 2) + Mathf.Pow(player.transform.position.y - gameObject.transform.position.y, 2)) - offset <= rangeSquare))
        {
            //Debug.Log("In range");
            exclamInst = Instantiate(exclam, new Vector3(gameObject.transform.position.x + offX, gameObject.transform.position.y + offY, gameObject.transform.position.z), Quaternion.identity);
            exclamInst.transform.parent = gameObject.transform;
            inRange = true;
        }
        //else if the object is set to in range, but the player has now exited the circular radius
        else if(inRange && ((Mathf.Pow(player.transform.position.x - gameObject.transform.position.x, 2) + Mathf.Pow(player.transform.position.y - gameObject.transform.position.y, 2)) - offset > rangeSquare))
        {
            //Debug.Log("Out of range");
            Object.Destroy(exclamInst);
            inRange = false;
        }

        if (inRange) {
            if (Input.GetButtonDown("Submit") && !talking)
            {
                //Debug.Log(Dialogue);
                dialogueBoxInst = Instantiate(dialogueBox, dialoguePos + camera.transform.position, Quaternion.identity);
                dialogueBoxInst.transform.parent = camera.transform;
                nameTag = Instantiate(texter, camera.transform.position, Quaternion.identity);
                nameTag.transform.parent = dialogueBoxInst.transform;
                nameTag.printer(Name, -13.0f, -4.2f, 4.0f);
                //nameTag.transform.localScale = new Vector3(nameTag.transform.localScale.x, nameTag.transform.localScale.y, nameTag.transform.localScale.z);
                PortraitInst = Instantiate(Portrait, camera.transform.position + new Vector3(-11.8f, -6.8f, 4.0f), Quaternion.identity);
                //PortraitInst.transform.localScale = new Vector3(PortraitInst.transform.localScale.x * ratio, PortraitInst.transform.localScale.y * ratio, PortraitInst.transform.localScale.z);
                PortraitInst.transform.parent = dialogueBoxInst.transform;
                dialogueBoxInst.transform.localScale = new Vector3(dialogueBoxInst.transform.localScale.x * ratio, dialogueBoxInst.transform.localScale.y * ratio, dialogueBoxInst.transform.localScale.z);
                dialogueBoxInst.setDialogue(Dialogue);
                talking = true;
                //done = dialogue play function
            }
        }

        if (dialogueBoxInst != null)
        {
            if (dialogueBoxInst.dialogueFin && talking)
            {
                hanger = Instantiate(empty, new Vector3(), Quaternion.identity);
                PortraitInst.transform.parent = hanger.transform;
                dialogueBoxInst.transform.parent = hanger.transform;
                nameTag.transform.parent = hanger.transform;
                Destroy(hanger);
                talking = false;
            }
            //Object.Destroy(dialogueBoxInst);
        }
        
    }
}
