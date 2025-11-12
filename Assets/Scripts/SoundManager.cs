using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource audioSource;
    [SerializeField]
    private AudioClip jumpAudioClip, hurtAudioClip, collectAudioClip, runningAudioClip;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayJumpSound()
    {
        // 使用PlayOneShot避免中断其他音效
        audioSource.clip = jumpAudioClip;
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void StopSound()
    {
        // 使用PlayOneShot避免中断其他音效
        audioSource.Stop();
    }

    public void PlayHurtSound()
    {
        
        // 使用PlayOneShot避免中断其他音效
        audioSource.clip = hurtAudioClip;
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void PlayCollectSound()
    {
        // 使用PlayOneShot避免中断其他音效
        audioSource.clip = collectAudioClip;
        audioSource.PlayOneShot(audioSource.clip);
    }
    
    public void PlayRunningSound()
    {
        // 使用PlayOneShot避免中断其他音效
        audioSource.clip = runningAudioClip;
        audioSource.PlayOneShot(audioSource.clip);
    }
}
