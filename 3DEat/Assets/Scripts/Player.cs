﻿using UnityEngine;

public class Player : MonoBehaviour
{
    #region 欄位與屬性
    [Header("移動速度"),Range(1,1000)]
    public float speed = 10;
    [Header("跳躍高度"), Range(1, 5000)]
    public float height;

    /// <summary>
    /// 是否在地板上
    /// </summary>
    private bool isGround
    {
        get
        {
            if (transform.position.y < 0.051f) return true; // 如果 Y 軸 小於0.051 傳回 true
            else return false;                              // 否則傳回 false
        }
    }

    /// <summary>
    /// 旋轉角度
    /// </summary>
    private Vector3 angle;

    private Animator ani;       // 動畫
    private Rigidbody rig;      // 剛體
    private AudioSource aud;    // 喇叭
    private GameManager gm;     // 遊戲管理器

    /// <summary>
    ///  跳躍力道 : 從 0 慢慢增加
    /// </summary>
    private float jump;

    [Header("熱狗音效")]
    public AudioClip soundHotDog;
    [Header("高粱音效")]
    public AudioClip soundBeer;
    #endregion

    #region 方法
    /// <summary>
    /// 移動 : 透過鍵盤
    /// </summary>
    private void Move()
    {
        #region 移動
        // 浮點數 前後值 = 輸入類別.取得軸向值("垂直") - 垂直 ws 上下
        float v = Input.GetAxisRaw("Vertical");
        // 水平 AD 左右
        float h = Input.GetAxisRaw("Horizontal");

        // 剛體.添加推力(x,y,z) - 世界座標
        // rig.AddForce(0, 0, speed * v );
        // 剛體.添加推力(三維向量)
        // 前方 transform.forward - Z
        // 前方 transform.right - X
        // 前方 transform.up - Y
        rig.AddForce(transform.forward * speed * Mathf.Abs(v));
        rig.AddForce(transform.forward * speed * Mathf.Abs(h));

        // 動畫.設定布林值("跑步參數，布林值") - 
        ani.SetBool("跑步開關", Mathf.Abs(v) > 0 || Mathf.Abs(h) > 0);
        #endregion

        #region 轉向
        if (v == 1) angle = new Vector3(0, 0, 0);               // 前 Y 0
        else if (v == -1) angle = new Vector3(0, 180, 0);       // 後 Y 180
        else if (h == 1) angle = new Vector3(0, 90, 0);         // 右 Y 90
        else if (h == -1) angle = new Vector3(0, 270, 0);       // 左 Y 270
        // 只要類別後面有 : MonoBehaviour
        // 就可以直接使用關鍵字 transform 取得此物件的 Transform 元件
        // eulerAngle 歐拉角度 0 - 360
        transform.eulerAngles = angle;

        // print("角度: " + angle);
        #endregion
    }

    /// <summary>
    /// 跳躍 : 判斷在地板並按下空白鍵跳躍
    /// </summary>
    private void Jump()
    {
        // 如果 在地板上 為 勾選 並且 按下空白鍵
        if(isGround && Input.GetButtonDown("Jump"))
        {
            // 每次跳躍 值都從 0 開始
            jump = 0;
            // 剛體.推力(0,跳躍高度,0)
            rig.AddForce(0, height, 0);
        }
        // 如果 不在地板上(在空中)
        if (!isGround)
        {
            // 跳躍 遞增 時間.一禎時間
            jump += Time.deltaTime;
        }
        // 動畫.設定浮點數("跳躍參數",跳躍時間)
        ani.SetFloat("跳躍力道", jump);
    }

    /// <summary>
    /// 碰到道具 : 碰到帶有標籤 [食物] 的物件
    /// </summary>
    private void HitProp(GameObject prop)
    {
        // print("碰到的物件標籤為 : " + prop.name);
        if (prop.tag == "熱狗")
        {
            aud.PlayOneShot(soundHotDog, 1.2f);    // 喇叭.播放一次音效(音效片段，音量)
            Destroy(prop);                      // 刪除物件
        }
        else if (prop.tag == "高粱")
        {
            aud.PlayOneShot(soundBeer, 1.2f);
            Destroy(prop);
        }

        gm.GetProp(prop.tag);                   // 告知 GM 取得道具(將道具標籤傳過去)
    }
    #endregion

    #region 事件
    private void Start()
    {
        // GetComponent<泛型>() 泛型方法 - 泛型 所有類型 Rigbody , Transform , Collider
        // 剛體 = 取得元件<剛體>();
        rig = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
        // FOOT 僅限於場景上只有一個類別存在時使用
        // 例如 : 場景上只有一個 GameManager 類別時可使用它來取得
        gm = FindObjectOfType<GameManager>();
    }

    // 固定更新頻率事件 : 1 秒 50 禎，使用物理必須在此事件內
    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        Jump();
    }

    // 碰撞事件 : 當物件碰撞開始時執行一次 ( 沒有勾選 is Trigger )
    // collision 碰到物件的碰裝資訊
    private void OnCollisionEnter(Collision collision)
    {

    }
    // 碰撞事件 : 當物件碰撞離開時執行一次 ( 沒有勾選 is Trigger )
    private void OnCollisionExit(Collision collision)
    {

    }
    // 碰撞事件 : 當物件碰撞開始時持續執行 ( 沒有勾選 is Trigger ) 60 FPS
    private void OnCollisionStay(Collision collision)
    {

    }

    /* --------- */
    // 觸發事件 : 當物件碰撞開始時執行一次 ( 有勾選 is Trigger )
    private void OnTriggerEnter(Collider other)
    {
        HitProp(other.gameObject);
    }
    // 觸發事件 : 當物件碰撞離開時執行一次 ( 有勾選 is Trigger )
    private void OnTriggerExit(Collider other)
    {

    }
    // 觸發事件 : 當物件碰撞開始時持續執行 ( 有勾選 is Trigger ) 60 FPS
    private void OnTriggerStay(Collider other)
    {

    }
    #endregion
}
