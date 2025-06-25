using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField]
    public float burnDamage = 3f;
    [SerializeField]
    public float burnDuration = 3f;

    private Animator anim;
    private bool hasActivated = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!hasActivated && collision.CompareTag("Player"))
        {
            hasActivated = true;
            anim.SetTrigger("Activate");

            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.ApplyBurn(burnDamage, burnDuration);
            }
        }
    }

    //Lap lai Fire
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hasActivated = false;
        }
    }
}
