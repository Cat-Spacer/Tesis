using UnityEngine;

public class Battery : MonoBehaviour
{
    [SerializeField] private SoundManager.Types _soundName = SoundManager.Types.BatteryCollected;
    private void Start()
    {
        EventManager.Instance.Subscribe("PlayerDeath", OnReset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Hamster>())
        {
            var hamster = collision.gameObject.GetComponent<Hamster>();
            if (hamster == null) return;
            if (hamster.Energy < hamster.MaxEnergy)
            {
                SoundManager.instance.Play(_soundName, false);
                hamster.AddEnergy(1);
                gameObject.SetActive(false);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Hamster>())
        {
            var hamster = collision.gameObject.GetComponent<Hamster>();
            if (hamster == null) return;
            if (hamster.Energy < hamster.MaxEnergy)
            {
                SoundManager.instance.Play(_soundName, false);
                hamster.AddEnergy(1);
                gameObject.SetActive(false);
            }
        }
    }

    void OnReset(params object[] param)
    {
        gameObject.SetActive(true);
    }
}