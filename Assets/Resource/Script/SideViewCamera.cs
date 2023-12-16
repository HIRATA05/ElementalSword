using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideViewCamera : MonoBehaviour
{
    GameObject player;

    // Use this for initialization
    void Start()
    {
        // Playerの部分はカメラが追いかけたいオブジェクトの名前をいれる
        this.player = GameObject.Find("MainPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = this.player.transform.position;
        transform.position = new Vector3(
            playerPos.x, playerPos.y + 2.0f, transform.position.z);

        //ジャンプ時にカメラが高くなりすぎないようにする
        if (Input.GetKeyDown(KeyCode.Space)) transform.position = new Vector3(
            playerPos.x, playerPos.y + 0.3f, transform.position.z);
    }
}
