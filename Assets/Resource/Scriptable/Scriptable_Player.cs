using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Scriptable_Player : ScriptableObject
{
    
    //�v���C���[�Ɋւ���f�[�^�������ŋL�q����
    //�v���C���[�̖��O
    public string player_name = "�v���C���[";
    
    //PersonalData�͗����@�X�e�[�^�X�̏����l�����߂�
    //GrowPattern�͓����@�X�e�[�^�X�̐����������߂�
    public string PersonalData, GrowPattern;

    /*
     * HP�v���C���[�̐����@�Ȃ��Ȃ�Ɠ�����ɖ߂����
     * EP�G�������^���|�C���g�̗��� ���@�U���ɕK�v
     * STR�r���Ղ��̋��� �����U���̋�����\��
     * MGC�����̋��� ���@�U���̋�����\��
     * TEC�Z�ʂ̍��� �N���e�B�J�����_�U���̈З͂Ɋւ��
     * SPD���̌y�₩�� �U�����󂯂����̉�𔻒�Ɋւ��
     * LUC�^�̗ǂ� ��Ԉُ�̊m���Ɋւ��
     * DEF�h��̍d�� �����قǕ����̎󂯂�_���[�W������
     * RES���@�̒�R�� �����قǖ��@�̎󂯂�_���[�W������
    */
    //�X�e�[�^�X�̒l
    public int 
        max_hp = 20, max_ep = 18, str = 10, 
        mgc = 8, tec = 8, spd = 7, 
        luc = 9, def = 5, res = 4;

    //���݂̃}�b�v


    //���ݏ������Ă���_���W�������Ƃ̌��̐�
    //�ŏ��̐_�a
    public int key_FirstTemple = 0;


    //�X�e�[�^�X�̊�{�������@������%�@����ɓ������̐����������Z
    public int 
        max_hp_grow = 50, max_ep_grow = 50, str_grow = 50, 
        mgc_grow = 50, tec_grow = 50, spd_grow = 50, 
        luc_grow = 50, def_grow = 50, res_grow = 50;

    /* �����ꗗ
     * �拭�@HP��DEF�̏オ��₷������
     * �͎����@HP��STR�ATEC�̏オ��₷������
     * ��p�@TEC��SPD�̏オ��₷������
     * �K�^�@TEC��SPD�ALUC�ARES�����ՂȂ��オ�����
     * �ْ[�ҁ@EP��RES�̏オ��₷������
    */
    public string tough = "�K���L���E";
    public string Power = "�`�J���W�}��";
    public string dexterous = "�L���E";
    public string goodluck = "�R�E�E��";
    public string Heretics = "�C�^���V��";
}
