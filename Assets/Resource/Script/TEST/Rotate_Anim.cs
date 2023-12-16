using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_Anim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AnimatedRotate());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator AnimatedRotate()
    {

        for (int i = 0; i < 2;)
        {

            transform.Rotate(new Vector3(0, 18, 0));
            yield return null;

        }
    }
}
