using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create CharaData")]
public class Scriptable_CharaData : ScriptableObject
{
    //�����GNPC�L�����N�^�[�̃f�[�^
    //���O
    public string charaname = "";

    //�X�e�[�^�X
    public int
        max_hp = 20, hp = 20, max_ep = 20, ep = 20, str = 10,
        mgc = 10, tec = 10, spd = 10,
        luc = 10, def = 10, res = 10;

    //���j���Ɋl������v�f
    public int GetExp = 20;
    public int GetGold = 10;

    //�U���͈�
    public int AttackRange = 1;
    //�U���Ԋu���x
    public float EnemyTime = 0.5f;

    //����
    public bool die = false;
}
