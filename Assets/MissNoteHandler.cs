using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissNoteHandler : MonoBehaviour
{
    [Header("Miss Zone Settings")]
    [SerializeField] private AudioSource missSoundPlayer;  // Optional
    //[SerializeField] private ParticleSystem missEffect;    // Optional
    [SerializeField] private PlayerInfo missPlayerInfo;    // Your player/game manager

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("MissZone: Missed a note!");

            // Play miss sound if assigned
            if (missSoundPlayer != null)
                missSoundPlayer.Play();

            // Play miss effect if assigned
            //if (missEffect != null)
            //    missEffect.Play();

            // Call your miss logic
            if (missPlayerInfo != null)
                missPlayerInfo.HandleNoteMiss();

            // Destroy the missed note
        }
    }
}
