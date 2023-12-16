using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchCharacter : MonoBehaviour
{
    //�G���v���C���[�𔭌����邽�߂̃R���C�_�[�ɕt����

    private EnemyAction enemyAction;
    private EnemyAI enemy_ai;

    void Start()
    {
        enemyAction = GetComponentInParent<EnemyAction>();
        enemy_ai = GetComponentInParent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider col)
    {
        //�v���C���[�𔭌�
        if (col.tag == "Player")
        {
            //�G�L�����N�^�[�̏�Ԃ��擾
            EnemyAI.EnemyState state = enemy_ai.GetState();
            //�G�L�����N�^�[���ǂ��������ԂłȂ���Βǂ�������ݒ�ɕύX
            if (state == EnemyAI.EnemyState.Wait || state == EnemyAI.EnemyState.Walk)
            {
                Debug.Log("����");
                enemy_ai.SetState(EnemyAI.EnemyState.Chase, col.transform);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            Debug.Log("������");
            enemy_ai.SetState(EnemyAI.EnemyState.Wait);
        }
    }
}
