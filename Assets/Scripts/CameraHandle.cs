using Cinemachine;
using UnityEngine;

public class CameraHandle : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    private CinemachineBasicMultiChannelPerlin cbmcp;
    
    public GeneralInput input;
    public Player player => G.main.player;
    
    public CinemachineVirtualCamera[] virtualCameras;
    private int currentCameraIndex = 0;

    public float initGain;
    public float initFreq;
    
    [Header("Tilt")]
    public float tiltAmount;
    public float tiltSpeed;
    
    private float targetTilt;
    private float currentTilt;

    private float timer;

    private void Start()
    {
        CinemachineBasicMultiChannelPerlin cbmcp = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        initGain = cbmcp.m_AmplitudeGain;
        initFreq = cbmcp.m_FrequencyGain;
        
        for (int i = 0; i < virtualCameras.Length; i++)
        {
            virtualCameras[i].gameObject.SetActive(i == currentCameraIndex);
        }
    }

    private void Update()
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                StopShake();
            }
        }
        
        CameraTilt();
    }

    public void DoShake(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cbmcp = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_AmplitudeGain = intensity;
        cbmcp.m_FrequencyGain = 1;

        timer = time;
    }

    public void StopShake()
    {
        CinemachineBasicMultiChannelPerlin cbmcp = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_AmplitudeGain = initGain;
        cbmcp.m_FrequencyGain = initFreq;

        timer = 0;
    }
    
    public void CameraTilt()
    {
        targetTilt = Mathf.Clamp(input.direction.x, -1f, 1f) * tiltAmount;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);
        vcam.transform.localRotation = Quaternion.Euler(0, 0, currentTilt);
    }

    public void SwitchToCamera(int newCameraIndex)
    {
        if (newCameraIndex < 0 || newCameraIndex >= virtualCameras.Length) return;

        virtualCameras[currentCameraIndex].gameObject.SetActive(false);
        
        currentCameraIndex = newCameraIndex;
        virtualCameras[currentCameraIndex].gameObject.SetActive(true);
    }
}
