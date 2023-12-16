using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //�R���C�_�[�ɃA�^�b�`����
    //�R���C�_�[�Őݒ肵���G���A�ɓ��������ʊO����G���o��
    //EnemyAssault��t�����G���o��������A���S�����݂̏o���l�������炷

    //�o��������G��ݒ�
    [SerializeField] GameObject[] enemys;
    //���ɓG���o������܂ł̎���
    [SerializeField] float appearNextTime;
    //���̏ꏊ����o������G�̐�
    [SerializeField] int maxNumOfEnemys;
    //���݂̏o���l��
    public int numberOfEnemys;
    //�҂����Ԍv��
    private float elapsedTime;

    //�G�̏o���ʒu
    public Vector3 enemyPosition;
    //�G�̏o���ʒu��x�ʒu
    float enemyPos_x = 15;

    //�X�N���v�^�u����NPC�f�[�^ �X�P���g���̃f�[�^������
    [SerializeField] private Scriptable_CharaData EnemyData;
    //EnemyAssault�X�N���v�g�̎擾
    private EnemyAssault enemyAssault;

    // Start is called before the first frame update
    void Start()
    {
        //�ϐ��̏�����
        numberOfEnemys = 0;
        elapsedTime = 0f;

        enemyAssault = GetComponent<EnemyAssault>();
    }

    // Update is called once per frame
    void Update()
    {
        //�o�ߎ��Ԃ��v��
        elapsedTime += Time.deltaTime;
        //�G�o���ʒu���J�����O�ɍX�V
        enemyPosition = Camera.main.ViewportToWorldPoint(new Vector3(enemyPos_x, -1, Camera.main.nearClipPlane));
        enemyPosition.z = 0;

        //�X�P���g���̎��S���m�F�����猻�݂̓G�o���������炵��die��false�ɂ���
        if(EnemyData.die == true)
        {
            numberOfEnemys--;
            EnemyData.die = false;
        }
        
    }

    //�N�������̂��v���C���[�̎��G�o��
    void OnTriggerStay(Collider col)
    {
        //���݂̐������̏ꏊ����o������ő吔�𒴂��Ă��牽�����Ȃ�
        if (numberOfEnemys >= maxNumOfEnemys)
        {
            return;
        }
        if (col.gameObject.tag == "Player")
        {
            //���Ԃ��v�����Ĉ�莞�Ԃ��Ƃɏo��������
            if (elapsedTime > appearNextTime)
            {
                elapsedTime = 0f;
                AppearEnemy();
            }
        }
    }

    //�G�o�����\�b�h
    void AppearEnemy()
    {
        //�G�𕡐��o��������ꍇ�����_���ɑI��
        var randomValue = Random.Range(0, enemys.Length);
        //�v���C���[�̌����ɂ���ďo����������A������ς���
        //var randomRotationY = Random.value * 360f;

        //��ʊO���w�肵�ēG���o��
        GameObject.Instantiate(enemys[randomValue], enemyPosition, Quaternion.Euler(0f, 180f, 0f));

        //�G�̏o�����𑝂₵�Ď��Ԍv�������Z�b�g
        numberOfEnemys++;
        elapsedTime = 0f;
    }
}
