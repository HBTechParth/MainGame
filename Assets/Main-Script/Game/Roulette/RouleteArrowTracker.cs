using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RouleteArrowTracker : MonoBehaviour
{
    /* public GraphicRaycaster raycaster;
     public EventSystem eventSystem;
     public List<GameObject> uiObjects; // List of UI objects to check against

     void Update()
     {
         DetectUIElementUnderMouse();
     }

     void DetectUIElementUnderMouse()
     {
         PointerEventData pointerData = new PointerEventData(eventSystem)
         {
             position = Input.mousePosition
         };

         List<RaycastResult> results = new List<RaycastResult>();
         raycaster.Raycast(pointerData, results);

         foreach (RaycastResult result in results)
         {
             if (uiObjects.Contains(result.gameObject))
             {
                 Debug.Log("UI Element hit: " + result.gameObject.name);
                 // Add any additional logic here
                 return;
             }
         }
     }*/

    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    public List<GameObject> uiObjects; // List of UI objects to check against

    private GameObject currentHoverObject = null; // To track the current object under the mouse

    void Update()
    {
        if (RouletteManager.Instance.betStatus)
        {
            DetectUIElementUnderMouse();
        }
        else
        {
            DeactivateAllObjects();
        }
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Input.GetMouseButtonUp");
            DeactivateAllObjects();
        }
    }

    void DetectUIElementUnderMouse()
    {
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        bool objectHovered = false;

        foreach (RaycastResult result in results)
        {
            if (uiObjects.Contains(result.gameObject))
            {
                objectHovered = true;
                if (currentHoverObject != result.gameObject)
                {
                    // Mouse has just started hovering over a new object
                    if (currentHoverObject != null)
                    {
                        // Deactivate objects in the previously hovered UI element
                        RouletteButtonData previousScript = currentHoverObject.GetComponent<RouletteButtonData>();
                        if (previousScript != null)
                        {
                            previousScript.ActivateObjects(false);
                        }
                        //Debug.Log("UI Element deactivated: " + currentHoverObject.name);
                    }

                    // Activate objects in the new hovered UI element
                    currentHoverObject = result.gameObject;
                    RouletteButtonData newScript = currentHoverObject.GetComponent<RouletteButtonData>();
                    if (newScript != null)
                    {
                        newScript.ActivateObjects(true);
                    }
                 //   Debug.Log("UI Element activated: " + currentHoverObject.name);
                }
                break;
            }
        }

        if (!objectHovered && currentHoverObject != null)
        {
            // Mouse has exited the currently hovered object
            if (uiObjects.Contains(currentHoverObject))
            {
                RouletteButtonData currentScript = currentHoverObject.GetComponent<RouletteButtonData>();
                if (currentScript != null)
                {
                    currentScript.ActivateObjects(false);
                }
               // Debug.Log("UI Element deactivated: " + currentHoverObject.name);
            }
            currentHoverObject = null;
        }
    }

    void DeactivateAllObjects()
    {
        foreach (GameObject uiObject in uiObjects)
        {
            RouletteButtonData script = uiObject.GetComponent<RouletteButtonData>();
            if (script != null)
            {
                script.ActivateObjects(false);
            }
        }
        currentHoverObject = null;
    }

}
