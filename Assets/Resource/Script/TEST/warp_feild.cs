using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class warp_feild : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    private Vector3 Player_pos;
    [SerializeField] private Field_Data FieldData;
    // Start is called before the first frame update
    void Start()
    {
        Player.transform.localPosition = FieldData.PlayerPosition_World;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Player_pos = Player.transform.position;
            FieldData.PlayerPosition_World = Player_pos;
            SceneManager.LoadScene("BattleField");
        }
    }
}
