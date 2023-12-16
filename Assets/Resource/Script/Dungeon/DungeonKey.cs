using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonKey : MonoBehaviour
{

    [SerializeField] private Scriptable_Player PlayerData;
    [SerializeField] private FirstTempleManager TempleManager;

    // Start is called before the first frame update
    void Start()
    {
        if (TempleManager.Puting_Key_1)
        {
            //鍵を消滅
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //プレイヤーが接触した時スクリプタブルの鍵を増やし消滅
    void OnTriggerEnter(Collider other)
    {

        Debug.Log("鍵！！");
        if (other.CompareTag("Player") || other.CompareTag("PlayerWeapon"))
        {
            //スクリプタブルを参照して鍵を入手
            PlayerData.key_FirstTemple++;
            TempleManager.Puting_Key_1 = true;

            //鍵入手のSEを鳴らす


            //鍵を消滅
            Destroy(this.gameObject);
        }
        
    }
}
