using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class AudioClick : MonoBehaviour
{

    public AudioClip sound;
    private Button button { get { return GetComponent<Button>(); } }
    private static AudioSource soundPlay;

    void Start()
    {
        button.onClick.AddListener(() => PlaySound());
    }

    void PlaySound()
    {
        soundPlay = Instantiate(gameObject.AddComponent<AudioSource>());
        soundPlay.clip = sound;
        soundPlay.playOnAwake = false;
        soundPlay.PlayOneShot(sound);
        DestroyObject(soundPlay, 1f);
    }
}
