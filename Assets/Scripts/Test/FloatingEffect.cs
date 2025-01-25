using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    public bool mAutoStart = false;

    public float bounceHeight = 0.2f;
    public float bounceSpeed = 2f;
    public float smoothStopTime = 0.5f;

    private float bounceFactor = 0f;
    private bool isBouncing = false;

    private Vector3 originalPosition;

    public void StartFloating()
    {
        isBouncing = true; // Start smooth stop
        bounceFactor = 1f; // Fully bounce
    }

    public void StopFloating()
    {
        isBouncing = false;
    }

    private void Start()
    {
        // Store the original position of the sprite
        originalPosition = transform.localPosition;

        if (mAutoStart)
            StartFloating();
    }

    private void Update()
    {
        if (isBouncing || bounceFactor > 0f)
        {
            float bounce = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight * bounceFactor;
            transform.localPosition = new Vector3(originalPosition.x, originalPosition.y + bounce, originalPosition.z);

            if (!isBouncing)
            {
                // Gradually reduce bounceFactor when not bouncing
                bounceFactor = Mathf.Max(0f, bounceFactor - Time.deltaTime / smoothStopTime);
            }
        }
    }
}
