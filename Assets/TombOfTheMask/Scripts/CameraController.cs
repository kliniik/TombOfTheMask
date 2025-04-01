using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // reference to the player
    public Vector2 offset; // offset so that the camera does not "touch" the player
    public float smoothSpeed = 0.125f; // speed of the smoothing

    // Start is called before the first frame update
    void Start()
    {
        if (player != null)
        {
            offset = transform.position - player.position;
            StartCoroutine(FollowPlayer()); // Start the coroutine
        }
        else
        {
            Debug.Log("ERROR: player not found!");
        }
    }

    // Coroutine to update the camera position smoothly
    IEnumerator FollowPlayer()
    {
        while (true) // Run this coroutine indefinitely
        {
            if (player != null)
            {
                Vector3 targetPosition = new Vector3(
                    transform.position.x,
                    player.position.y + offset.y,
                    transform.position.z
                );

                // Smoothly interpolate the position
                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            }
            else
            {
                Debug.LogWarning("Player is null, stopping FollowPlayer coroutine.");
                yield break; // Exit the coroutine if the player is null
            }

            yield return null; // Wait for the next frame
        }
    }
}
