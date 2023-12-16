using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideViewCamera : MonoBehaviour
{
    GameObject player;

    // Use this for initialization
    void Start()
    {
        // Player�̕����̓J�������ǂ����������I�u�W�F�N�g�̖��O�������
        this.player = GameObject.Find("MainPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = this.player.transform.position;
        transform.position = new Vector3(
            playerPos.x, playerPos.y + 2.0f, transform.position.z);

        //�W�����v���ɃJ�����������Ȃ肷���Ȃ��悤�ɂ���
        if (Input.GetKeyDown(KeyCode.Space)) transform.position = new Vector3(
            playerPos.x, playerPos.y + 0.3f, transform.position.z);
    }
}
