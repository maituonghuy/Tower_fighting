using UnityEngine;

public class BuffPickup : MonoBehaviour
{
    [SerializeField] private Buff buff;

    public Buff GetBuff()
    {
        return buff;
    }
}
