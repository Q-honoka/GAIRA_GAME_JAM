using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sakura_Move : MonoBehaviour
{
    // 定数宣言
    const float DEBUFF = -80.0f; // プレイヤーと衝突したときの降下割合(数字が大きいほど大きく落ちる)
    const float SPAN = 2.0f;    // 花びらの移動速度を変える間隔

    // 変数宣言
    float x, y, z;      // 座標
    float FallSpeed;    // 落下速度(y座標) 値が大きいほど速く落下する
    float MoveSpeed;    // 移動速度(x座標) マイナス値だと左、プラス値だと右に移動
    float DestroyX;     // 花びらを消去するx座標
    float timer;    // 経過時間

    // テスト
    GameObject Player;  // プレイヤーのgameObject
    Rigidbody2D PlayerRigid;    // プレイヤーのRigidbody2D

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        x = Random.Range(1.0f, 5.0f) + 10.0f;    // x座標 11.0 ～ 15.0 最小と最大の差は4.0
        y = 5.0f;
        z = 0.0f;
        FallSpeed = Random.Range(0.01f, 0.03f);             // 落下速度 最小と最大の差は0.02
        MoveSpeed = Random.Range(0.05f, 0.07f) * -1.0f;     // 移動速度 最小と最大の差は0.02
        DestroyX = -10.0f;  // マイナスの値だと左に消去のライン、プラスの値だと右に消去のラインがあるイメージ
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // 以下の文を桜の花びらの挙動にする？
        x += MoveSpeed; // x座標を移動速度の分減らす(左に移動)
        y -= FallSpeed; // FallSpeed分減算して下に落とす
        transform.position = new Vector3(x, y, z);

        // 桜の移動速度を変える場合は以下のコメントを解除してください
        /*
        timer += Time.deltaTime;
        if(timer > SPAN)    // 経過時間がSPANを超えた場合
        {
            FallSpeed = Random.Range(0.01f, 0.03f); // 落下速度0.01 ～ 0.03 
            MoveSpeed = Random.Range(0.05f, 0.08f) * -1.0f;  // 移動速度 -0.05 ～ -0.08 

            timer = 0.0f;
        }
        */

        // 桜の花びらの消去
        if (DestroyX > x)    // x座標が DestroyX よりも小さくなったら
        {
            Destroy(this.gameObject);   // 花びら(自身)を消去
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("デバフ");   // コンソールにログを出す
        Player = collision.gameObject;  // プレイヤーのgameObjectの取得
        PlayerRigid = Player.GetComponent<Rigidbody2D>();   // プレイヤーのrigidbody2Dの取得

        PlayerRigid.AddForce(transform.up * DEBUFF);    // 下に力を加える
    }
}
