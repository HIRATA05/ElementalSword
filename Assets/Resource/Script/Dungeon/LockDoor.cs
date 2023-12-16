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
        //���łɌ����J���Ă������������ł�����
        if (TempleManager.LockDoor_1)
        {
            //��������
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
            Debug.Log("������!");

            //�X�N���v�^�u�����Q�Ƃ��Č������
            PlayerData.key_FirstTemple--;
            //�����J�����t���O
            TempleManager.LockDoor_1 = true;

            //���g�p��SE��炷


            //��������
            Destroy(this.gameObject);
        }
    }
}
