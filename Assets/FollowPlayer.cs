using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform playerPos;

    private Vector3 offset = new Vector3(-6.0f, 11.0f, -6.0f);

    // Update is called once per frame
    void Update()
    {
        transform.position = playerPos.position + offset;
    }
}
