using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    PlayerMovement Player;
    public Text InteractionText;
    public Transform Hand;

    public Dictionary<Interactible.ObjectType, string> TypeToString = new Dictionary<Interactible.ObjectType, string>();

    Interactible GrabbedObject;

    void Start()
    {
        TypeToString[Interactible.ObjectType.Cake] = "the cake";
        TypeToString[Interactible.ObjectType.Apple] = "an apple";
        TypeToString[Interactible.ObjectType.Bowl] = "the bowl";
        TypeToString[Interactible.ObjectType.Sugar] = "the sugar";

        Player = GetComponent<PlayerMovement>();
    }

    void Update()
    {

        if (CheckObjectForward() && CheckObjectForward() != GrabbedObject)
        {
            InteractionText.text = "Press [F] to grab " + TypeToString[CheckObjectForward().Type];

            if (Input.GetKeyDown(KeyCode.F))
            {
                Interactible newGrabbedObject = CheckObjectForward();

                if (GrabbedObject)
                {
                    GrabbedObject.transform.parent = null;
                    GrabbedObject.IsGrabbed = false;
                }

                GrabbedObject = newGrabbedObject;
                newGrabbedObject.IsGrabbed = true;

                GrabbedObject.transform.parent = Hand;
            }
        }

        else if (Input.GetKeyDown(KeyCode.F) && GrabbedObject)
        {
            GrabbedObject.transform.parent = null;
            GrabbedObject.IsGrabbed = false;
            GrabbedObject = null;
        }

        else if (CheckObjectForward() == null)
            InteractionText.text = "";
    }

    Interactible CheckObjectForward()
    {
        RaycastHit hit;

        if (Physics.Raycast(Player.CameraPivot.transform.GetChild(0).transform.position, Player.CameraPivot.transform.GetChild(0).transform.forward, out hit, 10))
        {
            if (hit.collider.gameObject.GetComponent<Interactible>())
                return hit.collider.gameObject.GetComponent<Interactible>();
            else
                return null;
        }
        else
            return null;
    }
}