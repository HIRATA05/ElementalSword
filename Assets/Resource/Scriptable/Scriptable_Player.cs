using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Scriptable_Player : ScriptableObject
{
    
    //プレイヤーに関するデータをここで記述する
    //プレイヤーの名前
    public string player_name = "プレイヤー";
    
    //PersonalDataは来歴　ステータスの初期値を決める
    //GrowPatternは特徴　ステータスの成長率を決める
    public string PersonalData, GrowPattern;

    /*
     * HPプレイヤーの生命　なくなると入り口に戻される
     * EPエレメンタルポイントの略称 魔法攻撃に必要
     * STR腕っぷしの強さ 物理攻撃の強さを表す
     * MGC魔道の強さ 魔法攻撃の強さを表す
     * TEC技量の高さ クリティカルや弱点攻撃の威力に関わる
     * SPD足の軽やかさ 攻撃を受けた時の回避判定に関わる
     * LUC運の良さ 状態異常の確率に関わる
     * DEF防御の硬さ 高いほど物理の受けるダメージが減る
     * RES魔法の抵抗力 高いほど魔法の受けるダメージが減る
    */
    //ステータスの値
    public int 
        max_hp = 20, max_ep = 18, str = 10, 
        mgc = 8, tec = 8, spd = 7, 
        luc = 9, def = 5, res = 4;

    //現在のマップ


    //現在所持しているダンジョンごとの鍵の数
    //最初の神殿
    public int key_FirstTemple = 0;


    //ステータスの基本成長率　成長率%　これに特徴分の成長率を加算
    public int 
        max_hp_grow = 50, max_ep_grow = 50, str_grow = 50, 
        mgc_grow = 50, tec_grow = 50, spd_grow = 50, 
        luc_grow = 50, def_grow = 50, res_grow = 50;

    /* 特徴一覧
     * 頑強　HPやDEFの上がりやすい特徴
     * 力自慢　HPやSTR、TECの上がりやすい特徴
     * 器用　TECやSPDの上がりやすい特徴
     * 幸運　TECやSPD、LUC、RESが満遍なく上がる特徴
     * 異端者　EPやRESの上がりやすい特徴
    */
    public string tough = "ガンキョウ";
    public string Power = "チカラジマン";
    public string dexterous = "キヨウ";
    public string goodluck = "コウウン";
    public string Heretics = "イタンシャ";
}
