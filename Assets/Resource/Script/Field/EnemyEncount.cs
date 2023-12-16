using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyEncount : MonoBehaviour
{
    //�v���C���[����莞�ԕ����ƈ��͈͓��ɏo�����ăv���C���[��ǂ�������A������ƃG���J�E���g
    //�v���C���[��ǂ������鎞��NavMeshAgent���g�p
    //��莞�Ԓǂ�������Ə���

    public Transform target;
    private NavMeshAgent agent;
    //�v������
    float elapsedTime;
    //�G�̏��Ŏ���
    public float eraseTime = 8f;

    // Start is called before the first frame update
    void Start()
    {
        //������
        elapsedTime = 0;

        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[��ǐ�
        agent.destination = target.position;

        //���Ԃ̌v��
        elapsedTime += Time.deltaTime;
        if (elapsedTime > eraseTime) Destroy(this.gameObject);
    }
    
}
