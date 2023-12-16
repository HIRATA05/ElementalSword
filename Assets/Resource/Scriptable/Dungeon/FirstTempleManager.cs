using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Temple/FirstTemple")]
public class FirstTempleManager : ScriptableObject
{
    //フラグを設定することでダンジョンを入り直してもリセットされないようにする

    //鍵のかかったドアのフラグ管理 trueだとすでに開かれたドアでダンジョンを出ても鍵の状態がリセットされない
    public bool LockDoor_1 = false;
    public bool Puting_Key_1 = false;

    //ボスを倒したフラグ
    public bool Boss_flg = false;

    //ダンジョンをクリアしたフラグ
    public bool clear_flg = false;
}
