using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionAttack : MonoBehaviour
{
    //�v���C���[�Ȃǂ̐N����T�m���čU��

    [SerializeField] private TriggerEvent onTriggerStay = new TriggerEvent();


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) onTriggerStay.Invoke(other);
    }

    [Serializable]
    public class TriggerEvent : UnityEvent<Collider>
    {

    }

}
