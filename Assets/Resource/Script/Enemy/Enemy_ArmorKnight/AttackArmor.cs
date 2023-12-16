using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArmor : MonoBehaviour
{
	//敵の攻撃時のプレイヤーに与える動作

	[SerializeField] private GameObject UseWeaponEnemy;
	private BoxCollider WeaponCol;
	//攻撃ごとに設定された攻撃力をメソッドに渡す
	//スクリプタブルに攻撃力を設定
	int atk;

	private void Start()
    {
        WeaponCol = GetComponent<BoxCollider>();
		atk = UseWeaponEnemy.GetComponent<EnemyAI>().str;

	}

    void OnTriggerEnter(Collider col)
	{
		//当たったものがプレイヤー
		if (col.tag == "Player")
		{
			Debug.Log("当たり");
			col.GetComponent<SideViewController>().TakeDamage(UseWeaponEnemy.transform, atk);
		}

		//当たったものが盾
		//盾に当たった時攻撃を無効化するため相手の武器コライダーを無効化
		if (col.tag == "Shield")
		{
			WeaponCol.enabled = false;
			Debug.Log("盾に当たった");
			col.transform.root.GetComponent<SideViewController>().ShildGuard_success(WeaponCol);
		}
	}
}
