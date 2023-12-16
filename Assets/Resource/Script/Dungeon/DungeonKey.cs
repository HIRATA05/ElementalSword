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
            //��������
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�v���C���[���ڐG�������X�N���v�^�u���̌��𑝂₵����
    void OnTriggerEnter(Collider other)
    {

        Debug.Log("���I�I");
        if (other.CompareTag("Player") || other.CompareTag("PlayerWeapon"))
        {
            //�X�N���v�^�u�����Q�Ƃ��Č������
            PlayerData.key_FirstTemple++;
            TempleManager.Puting_Key_1 = true;

            //�������SE��炷


            //��������
            Destroy(this.gameObject);
        }
        
    }
}
