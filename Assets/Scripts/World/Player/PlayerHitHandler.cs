using UnityEngine;

public class PlayerHitHandler : MonoBehaviour
{
    [SerializeField] private Health _health;

    private void Start()
    {
        if (_health == null)
        {
            _health = GetComponent<Health>();
        }

        _health.Died.AddListener(OnPlayerDeath); //ajoute un listener au event du health called Died, quand Died est trigger, call OnPlayerDeath
    }

    private void OnPlayerDeath() //quoi faire quand player est dead
    {
        //respawn to coordinates
        

    }
}
