using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum playerStats
{
    Stop,
    Playing,
}

public class player : MonoBehaviour
{
    // 定数宣言
    const float MAX_Y = 5.0f;   // プレイヤーが上がれる上限のy座標
    const float LOW_Y = -5.0f;  // プレイヤーが降りれる下限のy座標

    // 変数宣言
    float x, y, z;  // 各座標
    [SerializeField]float jump = 450.0f; // ジャンプ力
    Rigidbody2D playerRigid;  // プレイヤーのrigidbody2D
    AudioSource playerAudio;    // AudioSource
    
    Animator playerAnim;

    [Header("お団子を持った時の重力(default: 1)")]
    [SerializeField] float odango_white = 1.25f;
    [SerializeField] float odango_pink = 2.0f;
    [SerializeField] float odango_green = 5f;

    // 現在のプレイヤーの状態
    // 主に往復時のカメラ移動するときに操作不能状態にする
    playerStats playerStats = playerStats.Playing;

    // 状態を管理するplayerStats変数を変えるための変数
    public playerStats setStats { set { playerStats = value; } }

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        x = -3.0f;  // x座標の初期化
        y = -0.85f;   // y座標の初期化
        z = 0.0f;   // z座標の初期化
        playerRigid = GetComponent<Rigidbody2D>();    // rigidbody2Dの取得
        playerAnim = GetComponent<Animator>();
        transform.position = new Vector3( x, y, z); // 初期座標に移動
        playerAnim.SetInteger("hasDango", 0);       // プレイヤーのアニメーション 0 は何も持っていない
        playerAudio = this.GetComponent<AudioSource>(); // AudioSourceの取得
        //playerAudio.Play(); // プレイ開始の音を鳴らす
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStats == playerStats.Playing)
        {
            // 移動
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) // スペースキーが押されたまたは左クリックされたとき
            {
                playerRigid.velocity = Vector2.zero;  // 落下速度を0にリセット
                playerRigid.AddForce(transform.up * jump);    // ジャンプ
            }

            // プレイヤーが画面の外に出ないようにする
            if (transform.position.y > MAX_Y) // y座標が上限より大きくなったら
            {
                transform.position = new Vector3(x, MAX_Y, z); // y座標を上限の高さにする
            }
            if (LOW_Y > transform.position.y)    // y座標が下限より小さくなったら
            {
                SceneManager.LoadScene("GameOverScene"); // ゲームオーバー
            }
        }
    }

    // 衝突判定(タグを変更したらif文の条件式も変更する必要がある)
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 衝突したオブジェクトのタグによって分岐する
        if(collision.gameObject.tag == "gameover")  // 衝突したオブジェクトのタグが gameover だったら
        {
            // ゲームオーバー処理
            Debug.Log("ゲームオーバー");
        }
        else if(collision.gameObject.tag == "debuff") // 衝突したオブジェクトのタグが debuff だったら
        {
            // デバフ処理
            Debug.Log("デバフ");
            playerRigid.AddForce(transform.up * jump);    // 下向きに力を加える
        }
        else    // 一致するタグがない場合
        {
            Debug.Log("一致するタグがありません。"); // コンソールにメッセージを出す
        }

        // 衝突したオブジェクトの名前が Goal だったとき
        if (collision.gameObject.name == "Goal")
        {
            // player がお団子をすでに持っている状態なら終了
            if (playerAnim.GetInteger("hasDango") != 0)
            {
                return;
            }

            // Goal オブジェクトに付いている StageGoal スクリプトを取得
            StageGoal stageGoal = collision.gameObject.GetComponent<StageGoal>();
            GameObject getOdango = stageGoal.GetDango(); // ゲームシーンに出たお団子を取る

            // 見た目を指定するための変数
            int gettingDangoNum = 0;

            // 取ったお団子の名前によって処理を分ける
            switch (getOdango.name)
            {
                case "odango_white(Clone)": // 白の団子
                    gettingDangoNum = 1;
                    playerRigid.gravityScale = odango_white;
                    break;
                case "odango_pink(Clone)": // ピンクの団子
                    gettingDangoNum = 2;
                    playerRigid.gravityScale = odango_pink;
                    break;
                case "odango_green(Clone)": // 緑の団子
                    gettingDangoNum = 3;
                    playerRigid.gravityScale = odango_green;
                    break;
                default:
                    break;
            }
            // 置かれているお団子を消す
            Destroy(getOdango.gameObject);
            // プレイヤーの見た目を変える
            playerAnim.SetInteger("hasDango", gettingDangoNum);
        }
        
        // 衝突したオブジェクトの名前が Startだった時
        if(collision.gameObject.name =="Start")// スタート地点
        {
            // 何も持っていない状態にする
            playerRigid.gravityScale = 1f;
            playerAnim.SetInteger("hasDango", 0);
        }
        
    }
}