using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem rightSlash;
    [SerializeField] private ParticleSystem leftSlash;
    [SerializeField] private ParticleSystem middleSlash;

    [Header("Check Box")]
    [SerializeField] private Collider leftZone;
    [SerializeField] private Collider midZone;
    [SerializeField] private Collider rightZone;

    [Header("Player Positions")]
    [SerializeField] private Transform leftPos;
    [SerializeField] private Transform midPos;
    [SerializeField] private Transform rightPos;

    [Header("Clone Settings")]
    [SerializeField] private GameObject clonePrefab;
    private bool hasClone = false;

    void Update()
    {
        if (Input.touchCount >= 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began && touch2.phase == TouchPhase.Began)
            {
                int zone1 = GetZone(touch1.position);
                int zone2 = GetZone(touch2.position);

                if (zone1 != zone2)
                {
                    MoveAndAttack(zone1);

                    if (!hasClone)
                    {
                        SpawnClone(zone2);
                    }
                }
            }
        }
        else if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                int zone = GetZone(touch.position);
                MoveAndAttack(zone);
            }
        }
    }

    int GetZone(Vector2 position)
    {
        float width = Screen.width;
        if (position.x < width / 3f) return 0;
        else if (position.x < 2f * width / 3f) return 1;
        else return 2;
    }

    void MoveAndAttack(int zone)
    {
        switch (zone)
        {
            case 0:
                transform.position = leftPos.position;
                animator.SetTrigger("isAttackLeft");
                leftSlash.Play();
                TryAttack(leftZone);
                break;
            case 1:
                transform.position = midPos.position;
                animator.SetTrigger("isAttackMiddle");
                middleSlash.Play();
                TryAttack(midZone);
                break;
            case 2:
                transform.position = rightPos.position;
                animator.SetTrigger("isAttackRight");
                rightSlash.Play();
                TryAttack(rightZone);
                break;
        }
    }

    void TryAttack(Collider zone)
    {
        Collider[] hits = Physics.OverlapBox(zone.bounds.center, zone.bounds.extents, zone.transform.rotation);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                hit.GetComponent<Enemy>().EnemyTakeDamage(1);
                return;
            }
        }
    }

    void SpawnClone(int zone)
    {
        Transform spawnPoint = zone == 0 ? leftPos : zone == 1 ? midPos : rightPos;
        GameObject clone = Instantiate(clonePrefab, spawnPoint.position, Quaternion.identity);

        Animator cloneAnimator = clone.GetComponent<Animator>();
        if (cloneAnimator != null)
        {
            switch (zone)
            {
                case 0: cloneAnimator.SetTrigger("isAttackLeft"); break;
                case 1: cloneAnimator.SetTrigger("isAttackMiddle"); break;
                case 2: cloneAnimator.SetTrigger("isAttackRight"); break;
            }
        }

        hasClone = true;
        Destroy(clone, 0.7f);
        StartCoroutine(ResetCloneFlag());
    }

    IEnumerator ResetCloneFlag()
    {
        yield return new WaitForSeconds(1f);
        hasClone = false;
    }
    
}
