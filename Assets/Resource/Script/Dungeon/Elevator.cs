using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    //���̃X�N���v�g��t�����I�u�W�F�N�g�̏�ŏ㉺�L�[�������ƃI�u�W�F�N�g���㉺�ɓ��� rigitbody�œ�����
    //�G���x�[�^�[�ɏ���Ă��鎞�̔���AInput.GetAxis("Vertical")�ŃG���x�[�^�[��Y����ω��̏��������

    private Rigidbody rigidBody;
    //�ő�ړ����x
    public float EV_top = 20f;
    //�G���x�[�^�[�̈ړ����x
    private float elevatorSpeed = 0.1f;
    //���݂̃G���x�[�^�[�̈ړ����x
    private float elevatorSpeed_current = 0f;
    //�ŏI�I�Ȉړ����x
    private Vector3 velocity = Vector3.zero;
    private Vector3 force;

    private bool EVflag = false;
    //���͂̎󂯎��
    private Vector3 input;
    private Vector3 player_pos;
    float top;

    float bottom;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        top = gameObject.transform.position.y + EV_top;
        bottom = gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        //�����L�[�������Ə㉺�ɓ���
        //�G���x�[�^�[���ŏ�L�[�������ƈړ�
        if (EVflag && Input.GetAxis("Vertical") > 0 && gameObject.transform.position.y < top)
        {
            force = new Vector3(0.0f, 15.0f, 0.0f);    // �͂�ݒ�
            //elevatorSpeed_current = elevatorSpeed;
            //elevatorSpeed = 0.02f;

        }
        else if (EVflag && Input.GetAxis("Vertical") < 0 && gameObject.transform.position.y > bottom)
        {
            force = new Vector3(0.0f, -15.0f, 0.0f);    // �͂�ݒ�
            //elevatorSpeed_current = elevatorSpeed * -1;
            //elevatorSpeed = -0.02f;
        }
        else
        {
            //elevatorSpeed_current = 0f;
            force = Vector3.zero;
        }

        rigidBody.MovePosition(transform.position + force * Time.deltaTime);
        //gameObject.transform.Translate(0, elevatorSpeed_current, 0);
    }

    //�G���x�[�^�[�̒��ɓ�������
    private void OnTriggerEnter(Collider other)
    {
        EVflag = true;
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    //�G���x�[�^�[����o����
    private void OnTriggerExit(Collider other)
    {
        EVflag = false;
    }
}
