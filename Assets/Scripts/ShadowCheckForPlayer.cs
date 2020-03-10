using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class ShadowCheckForPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            other.gameObject.GetComponent<PlayerInvisibility>().SetInvisible();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            other.gameObject.GetComponent<PlayerInvisibility>().SetVisible();
        }
    }
}
