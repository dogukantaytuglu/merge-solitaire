using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;
        
    public static CameraManager Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = FindObjectOfType<CameraManager>();
            if (_instance != null) return _instance;
            var go = new GameObject("CameraManager");
            _instance = go.AddComponent<CameraManager>();
            DontDestroyOnLoad(go);
            return _instance;
        }
    }

    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    private Tween _cameraShakeTween;
    private CinemachineBasicMultiChannelPerlin _multiChannelPerlin;

    private void Awake()
    {
        _multiChannelPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float amplitude, float frequency, float duration)
    {
        _cameraShakeTween?.Kill(true);
        _multiChannelPerlin.m_AmplitudeGain = amplitude;
        _multiChannelPerlin.m_FrequencyGain = frequency;
        _cameraShakeTween = DOVirtual.DelayedCall(duration, StopCameraShake);
    }

    private void StopCameraShake()
    {
        _multiChannelPerlin.m_AmplitudeGain = 0;
        _multiChannelPerlin.m_FrequencyGain = 0;
    }
}
