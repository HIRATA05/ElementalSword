using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Field_Data : ScriptableObject
{
    //�G���J�E���g����t�B�[���h�}�b�v�ɖ߂邱�Ƃ������t���O
    public bool encTOfei_flg = false;

    //���[���h�t�B�[���h�̃v���C���[���W
    public Vector3 PlayerPosition_World;

    //�G���J�E���g�t�B�[���h�̃v���C���[���W
    public Vector3 PlayerPosition_Field;
}
