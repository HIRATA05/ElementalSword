using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Field_Data : ScriptableObject
{
    //エンカウントからフィールドマップに戻ることを示すフラグ
    public bool encTOfei_flg = false;

    //ワールドフィールドのプレイヤー座標
    public Vector3 PlayerPosition_World;

    //エンカウントフィールドのプレイヤー座標
    public Vector3 PlayerPosition_Field;
}
