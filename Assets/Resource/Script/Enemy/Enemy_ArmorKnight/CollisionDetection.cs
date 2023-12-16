using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionDetection : MonoBehaviour
{
    //プレイヤーなどの侵入を探知して移動

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
