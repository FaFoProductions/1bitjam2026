using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] public AudioClip defaultAmbience;
    [SerializeField] public float fadeTime = 1f;
    private bool isPlayingTrack1;
    private AudioSource track1 = null, track2 = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    private void Start() {
        track1 = gameObject.AddComponent<AudioSource>();
        track2 = gameObject.AddComponent<AudioSource>();
        track1.loop = true;
        track2.loop = true;

        isPlayingTrack1 = true;

        SwapTrack(defaultAmbience);
    }

    public void SwapTrack(AudioClip newClip) {
        StopAllCoroutines();
        StartCoroutine(FadeTrack(newClip));
    }

    public void SwapToDefault() {
        SwapTrack(defaultAmbience);
    }

    private IEnumerator FadeTrack(AudioClip newClip) {
        float timeToFade = fadeTime;
        float timeElapsed = 0;

        if (isPlayingTrack1) {
            track2.clip = newClip;
            track2.Play();

            while(timeElapsed < timeToFade) {
                track2.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                track1.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            track1.Stop();
        }
        else {
            track1.clip = newClip;
            track1.Play();


            while(timeElapsed < timeToFade) {
                track1.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                track2.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            track2.Stop();
        }

        isPlayingTrack1 = !isPlayingTrack1;
    }
}
