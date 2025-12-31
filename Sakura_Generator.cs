using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sakura_Generator : MonoBehaviour
{
    public GameObject petal;    // 花びらの設計図

    // 定数宣言
    const float SPAN = 3.0f; // 生成間隔

    // 変数宣言
    float timer;     // 経過時間

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;  // 経過時間を0に初期化
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;    // 経過時間取得
        // 花びら生成
        if (timer > SPAN)    // 経過時間が生成間隔を超えたら
        {
            Instantiate(petal, new Vector3(10.0f, 0.0f, 0.0f), Quaternion.identity);    // 花びら生成

            timer = 0.0f;   // 経過時間を0にリセット
        }
    }
}
