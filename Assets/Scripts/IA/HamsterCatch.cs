using UnityEngine;

public class HamsterCatch : MonoBehaviour
{
    [SerializeField] private bool _obstacle = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<Hamster>()) return;

        var hamster = collision.gameObject.GetComponent<Hamster>();
        if (hamster.InTube())
            if (!_obstacle)
                hamster.Die();
            else
                hamster.MoveToNextTube(hamster.LastTube);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<Hamster>()) return;

        var hamster = collision.gameObject.GetComponent<Hamster>();
        if (hamster.InTube())
            if (!_obstacle)
                hamster.Die();
            else
                hamster.MoveToNextTube(hamster.LastTube);
    }
}