using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArmor : MonoBehaviour
{
	//�G�̍U�����̃v���C���[�ɗ^���铮��

	[SerializeField] private GameObject UseWeaponEnemy;
	private BoxCollider WeaponCol;
	//�U�����Ƃɐݒ肳�ꂽ�U���͂����\�b�h�ɓn��
	//�X�N���v�^�u���ɍU���͂�ݒ�
	int atk;

	private void Start()
    {
        WeaponCol = GetComponent<BoxCollider>();
		atk = UseWeaponEnemy.GetComponent<EnemyAI>().str;

	}

    void OnTriggerEnter(Collider col)
	{
		//�����������̂��v���C���[
		if (col.tag == "Player")
		{
			Debug.Log("������");
			col.GetComponent<SideViewController>().TakeDamage(UseWeaponEnemy.transform, atk);
		}

		//�����������̂���
		//���ɓ����������U���𖳌������邽�ߑ���̕���R���C�_�[�𖳌���
		if (col.tag == "Shield")
		{
			WeaponCol.enabled = false;
			Debug.Log("���ɓ�������");
			col.transform.root.GetComponent<SideViewController>().ShildGuard_success(WeaponCol);
		}
	}
}
