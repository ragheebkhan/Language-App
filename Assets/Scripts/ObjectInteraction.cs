using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    private GameObject heldObject;
    private Vector3 holdOffset = new Vector3(-0.5f, -.3f, 1f); // adjust the offset as needed
    private bool isHoldingObject = false;
    [SerializeField] List<GameObject> pickUpNames;

    void Update()
    {
       
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Here");
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, transform.forward, out hitInfo, Mathf.Infinity))
            {
                //Debug.DrawRay(transform.position, transform.forward * hitInfo.distance, Color.green, 2f);
                //Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.blue, 2f);
                //Debug.Log("Here1");
                //Debug.Log(hitInfo.collider.gameObject.tag);
                //Debug.Log(hitInfo.collider.name);
                //Debug.Log(hitInfo.collider.transform);
                //Debug.Log("isHolding" + isHoldingObject);
                if (pickUpNames.Contains(hitInfo.collider.gameObject))
                {
                    //Debug.Log("Here2");
                    if (!isHoldingObject) // Only pick up object if not already holding one
                    {
                        heldObject = hitInfo.collider.gameObject;
                        heldObject.transform.SetParent(transform);
                        heldObject.transform.localPosition = holdOffset;
                        isHoldingObject = true;
                    }
                    else if (heldObject == hitInfo.collider.gameObject) // Release object if already holding it
                    {
                        heldObject.transform.SetParent(null);
                        heldObject = null;
                        isHoldingObject = false;
                    }
                    else if (isHoldingObject) // Release object if holding one and clicking on something else
                    {
                        //Debug.Log("Here3");
                        heldObject.transform.SetParent(null);
                        heldObject.transform.position = hitInfo.point;
                        heldObject = null;
                        isHoldingObject = false;
                    }
                }
                else if (isHoldingObject) // Release object if holding one and not clicking on anything
                {
                    //Debug.Log("Here4");
                    heldObject.transform.SetParent(null);
                    heldObject.transform.position = hitInfo.point;
                    heldObject = null;
                    isHoldingObject = false;
                }
            }
            
        }
    }

    
}
