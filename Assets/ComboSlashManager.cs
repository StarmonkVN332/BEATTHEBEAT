using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class ComboSlashManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private ParticleSystem[] slashEffects; // size 3
    [SerializeField] private AudioClip[] slashSounds;       // size 3
    [SerializeField] private string[] attackAnimations = { "Attack_01", "Attack_02", "Attack_03" };

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        int randomIndex = Random.Range(0, attackAnimations.Length);

        // Chạy animation tương ứng
        animator.SetTrigger(attackAnimations[randomIndex]);

        // Chạy effect tương ứng
        if (slashEffects[randomIndex] != null)
            slashEffects[randomIndex].Play();

        // Âm thanh tương ứng
        if (audioSource != null && slashSounds.Length > randomIndex && slashSounds[randomIndex] != null)
            audioSource.PlayOneShot(slashSounds[randomIndex]);

        Debug.Log($"🎲 Random Slash: {attackAnimations[randomIndex]}");

        //Destroy(other.gameObject);
    }
}
