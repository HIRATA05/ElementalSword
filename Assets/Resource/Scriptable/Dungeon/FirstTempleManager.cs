using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Temple/FirstTemple")]
public class FirstTempleManager : ScriptableObject
{
    //�t���O��ݒ肷�邱�ƂŃ_���W��������蒼���Ă����Z�b�g����Ȃ��悤�ɂ���

    //���̂��������h�A�̃t���O�Ǘ� true���Ƃ��łɊJ���ꂽ�h�A�Ń_���W�������o�Ă����̏�Ԃ����Z�b�g����Ȃ�
    public bool LockDoor_1 = false;
    public bool Puting_Key_1 = false;

    //�{�X��|�����t���O
    public bool Boss_flg = false;

    //�_���W�������N���A�����t���O
    public bool clear_flg = false;
}
