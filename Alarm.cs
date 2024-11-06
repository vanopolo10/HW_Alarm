using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource), typeof(Collider))]
public class Alarm : MonoBehaviour
{
    [FormerlySerializedAs("_volumeIncreaseDelay")] [SerializeField]
    private float _volumeChangeDelay = 0.5f;

    [SerializeField] [Min(0.1f)] private float _increaseBy = 0.1f;
    [SerializeField] [Min(0.1f)] private float _decreaseBy = 0.1f;
    [SerializeField] [Min(0)] private float _baseVolume;

    private AudioSource _alarmAudio;
    private Coroutine _coroutine;

    private void Awake()
    {
        _alarmAudio = GetComponent<AudioSource>();
        _alarmAudio.volume = 0;
    }

    private void Start()
    {
        _alarmAudio.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        float maxVolume = 1;
        float minVolume = 0;

        if (other.TryGetComponent<Thief>(out Thief thief))
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            if (_alarmAudio.isPlaying == false || _alarmAudio.volume <= minVolume )
            {
                _alarmAudio.volume = _baseVolume;
                
                if (_coroutine != null)
                    StopCoroutine(_coroutine);
                
                _coroutine = StartCoroutine(ChangeVolume(_volumeChangeDelay, maxVolume, _increaseBy));
            }
            else
            {
                if (_coroutine != null)
                    StopCoroutine(_coroutine);
                
                _coroutine = StartCoroutine(ChangeVolume(_volumeChangeDelay, minVolume, -_decreaseBy));
            }
        }
    }

    private IEnumerator ChangeVolume(float delay, float targetVolume, float changeCoefficient)
    {
        WaitForSeconds volumeChangeDelay = new WaitForSeconds(delay);

        while (!Mathf.Approximately(_alarmAudio.volume, targetVolume))
        {
            _alarmAudio.volume = Mathf.Clamp(_alarmAudio.volume + changeCoefficient, 0, 1);

            if ((changeCoefficient > 0 && _alarmAudio.volume >= targetVolume) ||
                (changeCoefficient < 0 && _alarmAudio.volume <= targetVolume))
            {
                _alarmAudio.volume = targetVolume;
                yield break;
            }

            yield return volumeChangeDelay;
        }
    }
}
