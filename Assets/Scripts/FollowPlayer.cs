using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] float offsetX = 0f;
    [SerializeField] float offsetY = 0f;
    [SerializeField] float offsetZ = -10f;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    //Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(offsetX, offsetY, offsetZ);
    }

    private void OnEnable()
    {
        if (player != null)
            transform.position = player.transform.position + new Vector3(offsetX, offsetY, offsetZ);
    }
}
