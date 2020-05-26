using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    #region 欄位與屬性
    /// <summary>
    /// 玩家變形元件
    /// </summary>
    private Transform player;

    [Header("追蹤速度"), Range(0.1f, 50.5f)]
    public float speed = 1.5f;
    #endregion

    #region 方法
    /// <summary>
    /// 追蹤玩家
    /// </summary>
    private void Track()
    {
        // 攝影機與小明 Y 軸距離 5.5 - 3 = 2.5
        // 攝影機與小明 Z 軸距離 -2.5 - 0 = -2.5
        Vector3 posTrack = player.position;
        posTrack.y += 4.5f;
        posTrack.z += -5.5f;

        // 攝影機座標 = 變形.座標
        Vector3 posCam = transform.position;
        // 攝影機座標 = 三維向量.插值(A點,B點,百分比)
        posCam = Vector3.Lerp(posCam, posTrack, 0.5f * Time.deltaTime * speed);
        // 變形.座標 = 攝影機座標
        transform.position = posCam;
    }
    #endregion

    #region 事件
    #region 實驗 Lerp 差值
    /*
    public float A = 0;
    public float B = 1000;

    public Vector2 V2A = Vector2.zero;
    public Vector2 V2B = Vector2.one*1000;
    */
    #endregion

    private void Start()
    {
        // 小明物件 = 遊戲物件.尋找("物件名稱").變形
        player = GameObject.Find("小明").transform;
    }

    private void LateUpdate()
    {
        Track();
    }
    #endregion
}
