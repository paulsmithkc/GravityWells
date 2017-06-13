using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject player;
    public PlayerMovement playerMovement;
    public PlayerHealth playerHealth;
    public MeteorShower meteorShower;
    public TextTyping textTyping;

    public new Camera camera;
    public FadeImageEffect cameraFadeEffect;
    public PixelateImageEffect pixelateImageEffect;
    
	void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        playerHealth = player.GetComponent<PlayerHealth>();
        meteorShower = GameObject.FindObjectOfType<MeteorShower>();
        textTyping = GameObject.FindObjectOfType<TextTyping>();

        camera = Camera.main;
        cameraFadeEffect = camera.GetComponent<FadeImageEffect>();
        pixelateImageEffect = camera.GetComponent<PixelateImageEffect>();

        StartCoroutine(PlayStory());
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    public Vector3 playerPosition
    {
        get { return player.transform.position; }
    }

    public void DamagePlayer(float damage)
    {
        playerHealth.DamagePlayer(damage);
    }

    void Update()
    {
        pixelateImageEffect.downsample = Mathf.FloorToInt(
            (playerHealth.maxHealth - playerHealth.currentHealth) / playerHealth.maxHealth * 60
        );
    }

    IEnumerator PlayStory()
    {
        // Initialization
        cameraFadeEffect.Fade(Color.black, Color.black, 0);
        textTyping.Clear();
        playerMovement.inputs = new PlayerMovement.Inputs
        {
            allowPlayerControl = false
        };

        // Fade in from black
        yield return StartCoroutine(DisplayMessage("Activating visual array"));
        float fadeTime = 4;
        cameraFadeEffect.Fade(Color.black, new Color(0, 0, 0, 0), fadeTime);
        yield return new WaitForSeconds(fadeTime);

        // Eject from dropship
        yield return StartCoroutine(DisplayMessage("Deploying asset"));
        playerMovement.inputs = new PlayerMovement.Inputs
        {
            allowPlayerControl = false,
            Vertical = 1
        };
        yield return new WaitForSeconds(1);
        playerMovement.inputs = new PlayerMovement.Inputs
        {
            allowPlayerControl = true
        };
    }

    IEnumerator DisplayMessage(string message, bool keepOnScreen = false)
    {
        bool messageDisplayed = false;
        textTyping.Enqueue(message, keepOnScreen, m => { messageDisplayed = true; });
        while (!messageDisplayed)
        {
            yield return new WaitForEndOfFrame();
        }
    }
}
