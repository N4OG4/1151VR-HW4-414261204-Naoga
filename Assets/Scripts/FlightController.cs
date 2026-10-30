using UnityEngine;

public class FlightController : MonoBehaviour
{
    [Header("移動設定")]
    public float forwardSpeed = 10f;
    public float controlSpeed = 5f;

    [Header("音效設定")]
    public AudioSource flightAudioSource; // 飛行中音效 (Loop)
    public AudioClip crashClip;           // 撞擊音效
    public AudioClip successClip;         // 通過音效
    private AudioSource sfxAudioSource;   // 用來播特效音的 AudioSource

    private bool isDead = false;

    void Start()
    {
        // 自動檢查並配置特效音 AudioSource
        sfxAudioSource = gameObject.AddComponent<AudioSource>();

        if (flightAudioSource != null && !flightAudioSource.isPlaying)
        {
            flightAudioSource.Play();
        }
    }

    void Update()
    {
        if (isDead) return;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // 1. 持續向前飛行
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // 2. 簡單的玩家方向控制 (WASD 或方向鍵)
        float moveX = Input.GetAxis("Horizontal") * controlSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * controlSpeed * Time.deltaTime;
        Debug.Log("Horizontal: " + h + ", Vertical: " + v);
        transform.Translate(new Vector3(moveX, moveY, 0));
    }

    // 碰撞偵測 (撞到障礙物)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !isDead)
        {
            isDead = true;

            // 停止飛行音效
            if (flightAudioSource != null) flightAudioSource.Stop();

            // 播放撞擊音效
            sfxAudioSource.PlayOneShot(crashClip);

            Debug.Log("撞到障礙物！遊戲結束");
            // TODO: 可以在這裡加入重新載入場景或顯示失敗畫面的邏輯
        }
    }

    // 觸發偵測 (順利通過障礙物)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ScoreGate") && !isDead)
        {
            // 播放順利通過音效
            sfxAudioSource.PlayOneShot(successClip);

            Debug.Log("順利通過障礙物！");

            // 通過後將該觸發器關閉，避免重複觸發
            other.gameObject.SetActive(false);
        }
    }
}