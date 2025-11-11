using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected AudioSource deathAudioSource;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        deathAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void Death()
    {
       
        Destroy(gameObject);
    }

    public void JumpOn()
    {
        deathAudioSource.Play();
        anim.SetTrigger("death");
    }
}
