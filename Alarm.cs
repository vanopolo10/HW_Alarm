using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Collider))]
public class Alarm : MonoBehaviour
{
    [SerializeField] private float _volumeChangeDelay = 0.5f;

    [SerializeField] [Min(0.1f)] private float _increaseBy = 0.1f;
    [SerializeField] [Min(0.1f)] private float _decreaseBy = 0.1f;
    [SerializeField] [Min(0)] private float _baseVolume;

    private AudioSource _alarmAudio;
    private Coroutine _coroutine;
    private float _minVolume = 0;
    private float _maxVolume = 1;

    private void Awake()
    {
        _alarmAudio = GetComponent<AudioSource>();
        _alarmAudio.volume = _minVolume;
    }

    private void Start()
    {
        _alarmAudio.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Thief>(out Thief thief))
        {
            _alarmAudio.volume = _baseVolume;
            StartVolumeChange(_maxVolume, _increaseBy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Thief>(out Thief thief))
            StartVolumeChange(_minVolume, 0 - _decreaseBy);
    }

    private void StartVolumeChange(float targetVolume, float changeCoefficient)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ChangeVolume(_volumeChangeDelay, targetVolume, changeCoefficient));
    }

    private IEnumerator ChangeVolume(float delay, float targetVolume, float changeCoefficient)
    {
        WaitForSeconds volumeChangeDelay = new WaitForSeconds(delay);

        while (Mathf.Approximately(_alarmAudio.volume, targetVolume) == false)
        {
            _alarmAudio.volume = Mathf.Clamp(_alarmAudio.volume + changeCoefficient, 0, 1);
            yield return volumeChangeDelay;
        }
    }
}
