using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHp = 100;
    private int _hp;

    public AudioClip playerHurtSound;
    public AudioClip playerDeathSound;

    private AudioSource playerAudioSource;

    public int MaxHp => _maxHp;
    public TextMeshProUGUI healthText;

    public UnityEvent Died; // Triggered when object dies

    public int Hp
    {
        get => _hp;
        private set
        {
            bool isDamage = value < _hp;
            _hp = Mathf.Clamp(value, 0, _maxHp);

            if (_hp <= 0)
            {
                if (CompareTag("Player") && playerDeathSound != null)
                {
                    playerAudioSource?.PlayOneShot(playerDeathSound);
                }

                Died?.Invoke();
                Debug.Log("Item is dead");
            }
        }
    }

    private void Awake()
    {
        _hp = _maxHp;
        Debug.Log("Item health set to " + _hp);

        if (CompareTag("Player"))
        {
            playerAudioSource = GetComponent<AudioSource>();
            if (playerAudioSource == null)
                playerAudioSource = gameObject.AddComponent<AudioSource>();

            playerAudioSource.spatialBlend = 0f; // 2D
            playerAudioSource.playOnAwake = false;
        }
    }

    private void Update()
    {
        if (healthText != null)
            healthText.text = "HEALTH: " + Mathf.RoundToInt(_hp).ToString();
    }

    public void Damage(int amount)
    {
        if (_hp <= 0) return; // Already dead

        Hp -= amount; // triggers setter
        Debug.Log("Item health lowered to " + _hp);

        if (CompareTag("Player") && playerHurtSound != null && _hp > 0)
        {
            playerAudioSource?.PlayOneShot(playerHurtSound);
        }

        if (TryGetComponent(out ZombieAudio zombieAudio) && amount > 0 && _hp > 0)
        {
            zombieAudio.PlayHurtSound();
        }
    }

    public void Heal(int amount)
    {
        Hp += amount;
        Debug.Log("Item health healed to " + _hp);
    }

    public void HealFull()
    {
        Hp = _maxHp;
        Debug.Log("Full health");
    }

    public void Kill()
    {
        Hp = 0;
        Debug.Log("Kill triggered");
    }

    public void Adjust(int value)
    {
        Hp = value;
    }
}
