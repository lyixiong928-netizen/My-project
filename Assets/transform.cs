using UnityEngine;
using System.IO;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject target;
    //變數設定target將運動見好就收
    
    // 快取 Transform 以提升效能
    private Transform targetTransform;
    
    // 預先定義移動和旋轉速度
    private const float moveSpeed = 2f;
    private const float rotateSpeed = 30f;
    
    // 限制單幀最大時間，避免卡頓時跳太多
    private const float maxDeltaTime = 0.0417f; // 最多 24 FPS 的時間步長 (電影標準)
    
    // 錄影功能 - 在 Inspector 中勾選開始錄影
    [Header("錄影設定")]
    public bool startRecording = false;
    public int captureFrameRate = 24; // 每秒截圖幾幀
    
    private bool wasRecording = false;
    private int frameCount = 0;
    private float recordTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // 快取 target 的 Transform，避免每幀都取得
        if (target != null)
        {
            targetTransform = target.transform;
        }
    }

    // Update is called once per frame - 折衷方案：限制 deltaTime 上限
    void Update()
    {
        // 檢查 targetTransform 是否存在
        if (targetTransform == null) return;
        
        // 限制 deltaTime，避免卡頓時物體瞬移過遠
        float dt = Mathf.Min(Time.deltaTime, maxDeltaTime);
        
        // 優化：只在 X 軸移動（移除不必要的 0 計算）
        targetTransform.Translate(moveSpeed * dt, 0, 0);
        
        // 優化：只在 Y 軸旋轉（移除不必要的 0 計算）
        targetTransform.Rotate(0, rotateSpeed * dt, 0);
        
        // 錄影控制
        HandleRecording();
    }
    
    void HandleRecording()
    {
        // 偵測錄影狀態改變
        if (startRecording && !wasRecording)
        {
            // 開始錄影 - 先建立資料夾
            string folderPath = Path.Combine(Application.dataPath, "..", "Screenshots");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                Debug.Log($"📁 建立資料夾: {folderPath}");
            }
            
            frameCount = 0;
            recordTimer = 0f;
            Debug.Log("🔴 開始錄影！存到 Screenshots/ 資料夾");
        }
        else if (!startRecording && wasRecording)
        {
            // 停止錄影
            Debug.Log($"⏹️ 停止錄影！共錄製 {frameCount} 幀");
        }
        
        wasRecording = startRecording;
        
        // 如果正在錄影，按照設定的幀率截圖
        if (startRecording)
        {
            recordTimer += Time.unscaledDeltaTime;
            float interval = 1f / captureFrameRate;
            
            if (recordTimer >= interval)
            {
                recordTimer -= interval;
                string filename = $"Screenshots/frame_{frameCount:D4}.png";
                ScreenCapture.CaptureScreenshot(filename);
                frameCount++;
            }
        }
    }
}
