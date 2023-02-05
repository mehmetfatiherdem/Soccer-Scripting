using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    Rigidbody playerRb;
    float speed = 500;
    GameObject focalPoint;

    [SerializeField] bool hasPowerup;
    [SerializeField] GameObject powerupIndicator;
    [SerializeField] int powerUpDuration = 5;
    [SerializeField] ParticleSystem particleFX;
    

    float normalStrength = 10; // how hard to hit enemy without powerup
    float powerupStrength = 25; // how hard to hit enemy with powerup
    
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    void Update()
    {
        // Add force to player in direction of the focal point (and camera)
        float verticalInput = Input.GetAxis("Vertical");

        playerRb.AddForce(verticalInput * speed * Time.deltaTime * focalPoint.transform.forward); 

        // Set powerup indicator position to beneath player
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);

        if (Input.GetKey(KeyCode.Space))
        {
            playerRb.AddForce(verticalInput * 50 * Time.deltaTime * focalPoint.transform.forward, ForceMode.Impulse);
            particleFX.Play();

        }

    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCooldown());
        }
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position;

            float strength = hasPowerup ? powerupStrength : normalStrength;
          
            enemyRigidbody.AddForce(awayFromPlayer * strength, ForceMode.Impulse);

        }
    }



}
