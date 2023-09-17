using System.Collections;
using UnityEngine;
using MixedReality.Toolkit.UX;
using TMPro;

[RequireComponent(typeof(LineRenderer))]
public class BallLauncher : MonoBehaviour
{
    public float initialVelocity = 10f;
    public float launchAngle = 45f;
    public float gravity = 9.81f;
    public GameObject ball;
    public float ballMass = 1f;

    public Slider velocitySlider;
    public Slider angleSlider;

    private LineRenderer lineRenderer;
    private int numTrajectoryPoints = 50;
    private bool ballLaunched = false;
    private Vector3 ballInitialPosition;
    private Vector3 lastBallPosition;
    public float totalDistanceTraveled = 0f;

    public TextMeshProUGUI highestPointText;
    public TextMeshProUGUI initialVelocityText;
    public TextMeshProUGUI initialAngleText;

    private bool showTrajectory = true;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numTrajectoryPoints;

        Material whiteMaterial = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material = whiteMaterial;
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
        lineRenderer.material.mainTexture = tex;
        lineRenderer.material.SetTextureScale("_MainTex", new Vector2(0.1f, 1f));

        ballInitialPosition = ball.transform.position;

        velocitySlider.OnValueUpdated.AddListener(UpdateInitialVelocity);
        angleSlider.OnValueUpdated.AddListener(UpdateLaunchAngle);

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Add this line to set interpolation
    }

    void Update()
    {
        if (showTrajectory)
        {
            DrawTrajectory();
        }

        if (ballLaunched)
        {
            totalDistanceTraveled += Vector3.Distance(lastBallPosition, ball.transform.position);
            lastBallPosition = ball.transform.position;
        }
    }

    void DrawTrajectory()
    {
        float launchAngleInRadians = launchAngle * Mathf.Deg2Rad;
        Vector3 initialVelocityVector = new Vector3(
            initialVelocity * Mathf.Cos(launchAngleInRadians),
            initialVelocity * Mathf.Sin(launchAngleInRadians),
            0
        );

        float t_peak = initialVelocity * Mathf.Sin(launchAngleInRadians) / gravity;
        Vector3 highestPointPosition = ballInitialPosition + t_peak * initialVelocityVector - 0.5f * gravity * t_peak * t_peak * Vector3.up;

        // Set the position and text of the highest point indicator
        highestPointText.text = totalDistanceTraveled.ToString("0") + " Units Traveled";
        highestPointText.transform.position = Camera.main.WorldToScreenPoint(highestPointPosition);
        highestPointText.text = "Highest Point";

        for (int i = 0; i < numTrajectoryPoints; i++)
        {
            float t = i / (float)(numTrajectoryPoints - 1);
            t *= (2f * initialVelocity * Mathf.Sin(launchAngleInRadians)) / gravity; // Time till peak
            Vector3 position = ballInitialPosition + t * initialVelocityVector - 0.5f * gravity * t * t * Vector3.up;
            lineRenderer.SetPosition(i, position);
        }
    }

    public void LaunchBall()
    {
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true; // Enable gravity when the ball is launched

        float launchAngleInRadians = launchAngle * Mathf.Deg2Rad;
        Vector3 initialVelocityVector = new Vector3(
            initialVelocity * Mathf.Cos(launchAngleInRadians),
            initialVelocity * Mathf.Sin(launchAngleInRadians),
            0
        );

        rb.velocity = initialVelocityVector;

        lastBallPosition = ball.transform.position;

        // Start a coroutine to reset ball position and launched flag after a certain time
        StartCoroutine(ResetBallPosition(5.0f));
    }

    IEnumerator ResetBallPosition(float delay)
    {
        yield return new WaitForSeconds(delay);

        ballLaunched = false;
        ball.transform.position = ballInitialPosition;
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false; // Disable gravity when reset
        lineRenderer.positionCount = numTrajectoryPoints; // Make the line visible again
        totalDistanceTraveled = 0f; // Reset the total distance traveled
    }

    void UpdateInitialVelocity(SliderEventData eventData)
    {
        initialVelocity = eventData.NewValue;
        initialVelocityText.text = "Initial Velocity: " + initialVelocity.ToString("0");
        showTrajectory = true;
    }

    void UpdateLaunchAngle(SliderEventData eventData)
    {
        launchAngle = eventData.NewValue;
        initialAngleText.text = "Launch Angle: " + launchAngle.ToString("0°");
        showTrajectory = true;
    }
}
