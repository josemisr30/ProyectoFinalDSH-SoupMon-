using UnityEngine;

public class SelectMusica : MonoBehaviour
{
    private AudioSource aud;
    public AudioClip[] clips = new AudioClip[2];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aud = GetComponent<AudioSource>();
        aud.clip = clips[Random.Range(0, 2)];
        aud.Play();
    }
}
