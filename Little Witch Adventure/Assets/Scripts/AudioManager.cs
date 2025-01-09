using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip background;
    public AudioClip gem;
    public AudioClip mushroom;
    public AudioClip bush;
    public AudioClip spit;
    public AudioClip wolf;
    public AudioClip shoot;
    public AudioClip button;
    public AudioClip death;
    public AudioClip healthPickUp;
    public AudioClip iceExplosion;
    public AudioClip stomp;
    public AudioClip portal;
    public AudioClip enemyDeath;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
