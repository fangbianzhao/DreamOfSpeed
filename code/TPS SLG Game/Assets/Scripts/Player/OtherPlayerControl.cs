using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerControl : MonoBehaviour {

    public float fire1CoolingTime = 0.15f;
    public AudioClip deathClip;
    public GameObject healthImage;

    [HideInInspector]
    public AudioSource otherPlayerHurtAduio;

    [HideInInspector]
    public int otherPlayerID;

    [HideInInspector]
    public int otherPlayerHealth;

    Animator anim;
    GameObject gunBarrelEnd;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    AudioSource playerAudio;
    Light gunLight;
    float effectsDisplayTime;
    float fire1Timer;

    void Awake()//Add component to game object
    {
        otherPlayerID = -1;
        otherPlayerHurtAduio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        effectsDisplayTime = 0.2f;

        foreach (Transform child in transform)//initialize every child
        {
            if(child.gameObject.name == "GunBarrelEnd") {
                gunBarrelEnd = child.gameObject;
                break;
            }
        }
        gunParticles = gunBarrelEnd.GetComponent<ParticleSystem>();
        gunLine = gunBarrelEnd.GetComponent<LineRenderer>();
        gunAudio = gunBarrelEnd.GetComponent<AudioSource>();
        gunLight = gunBarrelEnd.GetComponent<Light>();
        playerAudio = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        fire1Timer += Time.deltaTime;//control fire time

        if (fire1Timer >= fire1CoolingTime * effectsDisplayTime)
        {
            DisableEffects();
        }
        SetHealthImage();
        healthImage.transform.LookAt(Camera.main.transform);
    }

    public void SetHealthImage()//check player health
    {
        float health_value = GameSettings.HEALTH_INIT_WIDTH * otherPlayerHealth / GameSettings.PLAYER_INIT_HEALTH;
        healthImage.GetComponent<RectTransform>().sizeDelta = new Vector2(
            health_value,
            GameSettings.HEALTH_INIT_HEIGHT
        );
        healthImage.GetComponent<RectTransform>().anchoredPosition3D = new Vector2(
            (GameSettings.HEALTH_INIT_WIDTH - health_value) / 2,
            healthImage.GetComponent<RectTransform>().anchoredPosition3D.y);
    }

    public void DisableEffects()//disable player shooting
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    public void SetWalking(bool isWalking)//Set status
    {
        anim.SetBool("IsWalking", isWalking);
    }

    public void Shoot(Vector3 shootPoint)//control shooting point
    {
        fire1Timer = 0f;

        gunAudio.Play();

        gunLight.enabled = true;

        gunParticles.Stop();
        gunParticles.Play();

        gunLine.enabled = true;
        gunLine.SetPosition(0, gunBarrelEnd.transform.position);

        gunLine.SetPosition(1, shootPoint);
    }

    public void Die()//check if the player is dead
    { 

        anim.SetTrigger("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play();

        Destroy(gameObject, 5f);
    }

    public void RestartLevel()
    {
        // go to restart logical
        //SceneManager.LoadScene(0);
    }
}
