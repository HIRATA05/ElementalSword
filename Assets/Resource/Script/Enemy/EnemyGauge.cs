using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGauge : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    public void Update()
    {
        //EnemyGauge���J�����֌�������
        canvas.transform.rotation = Camera.main.transform.rotation;
    }
}
