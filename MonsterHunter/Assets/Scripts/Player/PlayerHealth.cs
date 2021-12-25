using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int originHealth = 100;
    public int currentHealth;
    //创建血条
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5;//单次伤害
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    private Animator anim;
    private AudioSource playerAudio;
    private PlayerMovement playerMovement;
    private PlayerShooting playerShooting;
    private bool isDead;
    private bool damaged;

    private void Awake()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        playerShooting = GetComponentInChildren<PlayerShooting>();
        currentHealth = originHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth = currentHealth - amount;
        playerAudio.Play();

        if (currentHealth < 0)
        {
            playerAudio.clip = deathClip;
            playerAudio.Play();
            anim.SetTrigger ("Die");
        }
    
    }
    
    void Update ()
    {
        if(damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    public void SetHealth(int health)
    {
        if(health < currentHealth)
        {
            damaged = true;
            currentHealth = health;
            healthSlider.value = currentHealth;
            playerAudio.Play();
        }
    }


    public void Death ()
    {
        isDead = true;

        playerShooting.DisableEffects();

        anim.SetTrigger ("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play ();

        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }
    
}