using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //�v���C���[���U����^���鎞�̔���

    [SerializeField]
    private Scriptable_Player PlayerData;

    //�����Q�[���I�u�W�F�N�g�ɐN�������u�ԂɌĂяo��
    void OnTriggerEnter(Collider other)
    {
        //other�̃Q�[���I�u�W�F�N�g�̃C���^�[�t�F�[�X���Ăяo��
        IDamageable damageable = other.GetComponent<IDamageable>();

        //damageable��null�l�������Ă��Ȃ����`�F�b�N
        if (damageable != null)
        {
            if (other.CompareTag("Enemy"))
            {
                //damageable�̃_���[�W�������\�b�h���Ăяo���B�����Ƃ��ăX�N���v�^�u��Playerdata��atk_tes���w��
                damageable.Damage(PlayerData.str);
            }
        }
    }
}
