using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform playerPos;

    private Vector3 offset = new Vector3(-3.0f, 7.0f, -3.0f);

    // Update is called once per frame
    void Update()
    {
        transform.position = playerPos.position + offset;
    }
}
