using System.Collections;
using UnityEngine;
using MixedReality.Toolkit.UX;
using TMPro;

public class RocketPropulsion : MonoBehaviour
{
    public float GRAVITY = 9.81f;
    public float DRAG = 0.1f;
    public float MASS = 10f;
    public float thrust;
    public float thrustDuration = 5f;
    public float targetPositionY = 50f;

    public Slider massSlider; // Slider to control mass
    public Slider thrustSlider; // Slider to control thrust
    public Slider thrustDurationSlider; // Slider to control thrust duration

    public TextMeshProUGUI massText;
    public TextMeshProUGUI thrustText;
    public TextMeshProUGUI thrustDurationText;

    private Rigidbody rb;
    public float thrustTime = 0f;

    public float cumulativeMomentum = 0f; // Track the cumulative momentum over time
    public float maxMomentum = 6500f; // Set a threshold for the maximum allowed cumulative momentum

    public ParticleSystem explosion;
    public GameObject rocketShipModel;

    public GameManager gameManager;
    public bool rocketIsLaunched;

    public bool isVictory;
    public bool isGameOver;

    public bool thrustIsFinished = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            gameObject.AddComponent<Rigidbody>();
            rb = GetComponent<Rigidbody>();
        }

        massText.text = "Mass: " + MASS.ToString("0");
        thrustText.text = "Thrust: " + thrust.ToString("0");
        thrustDurationText.text = "Thrust Duration: " + thrustDuration.ToString("0.0");

        rb.mass = MASS;
        rb.drag = DRAG;
        rb.useGravity = false;

        // Set up sliders with initial values and add listeners
        massSlider.Value = MASS;
        massSlider.OnValueUpdated.AddListener(UpdateMass);

        thrustSlider.Value = thrust;
        thrustSlider.OnValueUpdated.AddListener(UpdateThrust);

        thrustDurationSlider.Value = thrustDuration;
        thrustDurationSlider.OnValueUpdated.AddListener(UpdateThrustDuration);
    }

    void Update()
    {
        rb.AddForce(new Vector3(0, -GRAVITY * MASS, 0));

        // Implement logic when the rocket reaches the target position
        if (transform.position.y >= targetPositionY && cumulativeMomentum < maxMomentum && !isGameOver && !isVictory)
        {
            // Logic for when the rocket reaches the target position
            isVictory = true;
            gameManager.Victory();
        }

        if (rb.velocity.y < 0 && rocketIsLaunched && !isVictory && !isGameOver)
        {
            isGameOver = true;
            gameManager.GameOverFalling();
        }
    }

    public void LaunchRocket()
    {
        StartCoroutine(CountdownAndLaunch());
    }

    IEnumerator CountdownAndLaunch()
    {
        AudioManager.Instance.sfxSource.clip = AudioManager.Instance.sfxSounds[0].audioClip;
        AudioManager.Instance.sfxSource.Play();
        yield return new WaitForSeconds(AudioManager.Instance.sfxSounds[0].audioClip.length);
        StartCoroutine(RocketMove());
        rocketIsLaunched = true;
    }

    IEnumerator RocketMove()
    {
        float initialTime = Time.time;

        while (Time.time - initialTime < thrustDuration)
        {
            float netForce = thrust;
            Vector3 forceVector = new Vector3(0, netForce * 3f, 0);
            rb.AddForce(forceVector);

            // Calculate the change in momentum and add it to the cumulative total
            float changeInMomentum = netForce * Time.deltaTime;
            cumulativeMomentum += changeInMomentum;

            // Check if the cumulative momentum exceeds the maximum allowed value
            if (cumulativeMomentum > maxMomentum && !isVictory && !isGameOver)
            {
                explosion.Play();
                rocketShipModel.SetActive(false);
                isGameOver = true;
                gameManager.GameOverMomentum();
                AudioManager.Instance.PlaySFX("Rocket Explodes");
            }

            yield return null;
        }

        thrust = 0;
        thrustIsFinished = true;
    }

    public void UpdateMass(SliderEventData eventData)
    {
        MASS = eventData.NewValue;

        float minMass = CalculateMinMass(thrust);

        if (MASS < minMass)
        {
            MASS = minMass;
            massSlider.Value = minMass; // Reset the mass slider to the minimum required mass
            rb.mass = minMass;
        }
        else
        {
            rb.mass = MASS;
        }

        massText.text = "Mass: " + MASS.ToString("0");
    }

    public void UpdateThrust(SliderEventData eventData)
    {
        float minMass = CalculateMinMass(thrustSlider.Value);

        if (MASS < minMass)
        {
            MASS = minMass;
            massSlider.Value = minMass; // Reset the mass slider to the minimum required mass
            rb.mass = minMass;
        }

        thrust = eventData.NewValue;
        thrustText.text = "Thrust: " + thrust.ToString("0");
    }

    private float CalculateMinMass(float thrust)
    {
        float a, b, c;

        // Assuming the function passes through (0, 0) and (350, 3000)
        // We can solve the system of equations to find the coefficients a, b, and c
        // 0 = a * (0)^2 + b * (0) + c
        // 3000 = a * (350)^2 + b * (350) + c

        c = 0; // From the first equation
        a = (3000 - c) / (350 * 350); // From the second equation
        b = 0; // Assuming a simple quadratic relationship with no linear term

        float mass = (-b + Mathf.Sqrt(b * b - 4 * a * (c - thrust))) / (2 * a);

        return mass;
    }

    public void UpdateThrustDuration(SliderEventData eventData)
    {
        thrustDuration = eventData.NewValue;
        thrustDurationText.text = "Thrust Duration: " + thrustDuration.ToString("0.0");
    }
}
