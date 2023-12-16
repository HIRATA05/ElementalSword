using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDoor : MonoBehaviour
{

    [SerializeField] private FirstTempleManager TempleManager;
    [SerializeField] private Scriptable_Player PlayerData;

    // Start is called before the first frame update
    void Start()
    {
        //すでに鍵を開けていた時扉を消滅させる
        if (TempleManager.LockDoor_1)
        {
            //扉を消滅
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Player" && PlayerData.key_FirstTemple > 0)
        {
            Debug.Log("鍵入手!");

            //スクリプタブルを参照して鍵を入手
            PlayerData.key_FirstTemple--;
            //扉を開けたフラグ
            TempleManager.LockDoor_1 = true;

            //鍵使用のSEを鳴らす


            //扉を消滅
            Destroy(this.gameObject);
        }
    }
}
