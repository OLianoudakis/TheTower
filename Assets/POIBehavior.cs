using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class POIBehavior : MonoBehaviour
{
    public Transform POIPosition;
    public CanvasGroup canvasGroup;
    public Text text;

    public string myText;

    private POIBehavior myPOIBehavior;

    private void Start()
    {
        myPOIBehavior = GetComponent<POIBehavior>();
    }

    public void OnMouseDown()
    {
        Debug.Log("poi pressed");
        MovementController player = FindObjectOfType<MovementController>();
        player.SetDestination(POIPosition.position, true, myPOIBehavior);
    }

    public void ActivatePOIBehavior()
    {
        text.text = myText;
        canvasGroup.alpha = 1.0f;
    }

    public void DeactivatePOIBehavior()
    {
        canvasGroup.alpha = 0.0f;
    }
}
