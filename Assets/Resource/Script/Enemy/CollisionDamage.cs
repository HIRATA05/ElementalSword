using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
    //�G�Ƃ̐ڐG���̃_���[�W����
    public int damage = 1;

    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    // �Փ˂������������ɌĂ΂��
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //�����������̂��v���C���[�ł���ꍇ
        if (hit.gameObject.tag == "Player")
        {
            Debug.Log("������");
            hit.gameObject.GetComponent<SideViewController>().TakeDamage(transform, damage);
        }
    }

}
