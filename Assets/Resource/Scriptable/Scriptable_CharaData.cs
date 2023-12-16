using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create CharaData")]
public class Scriptable_CharaData : ScriptableObject
{
    //味方敵NPCキャラクターのデータ
    //名前
    public string charaname = "";

    //ステータス
    public int
        max_hp = 20, hp = 20, max_ep = 20, ep = 20, str = 10,
        mgc = 10, tec = 10, spd = 10,
        luc = 10, def = 10, res = 10;

    //撃破時に獲得する要素
    public int GetExp = 20;
    public int GetGold = 10;

    //攻撃範囲
    public int AttackRange = 1;
    //攻撃間隔速度
    public float EnemyTime = 0.5f;

    //生死
    public bool die = false;
}
