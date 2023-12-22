using UnityEngine;
using UnityEngine.SceneManagement;

public class BotTriggerDeath : MonoBehaviour
{
    private CircleCollider2D _collider;
    private AiStateMachine _asm;

    private void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
        _asm = transform.GetComponentInParent<AiStateMachine>();
        _collider.enabled = false;
    }

    private void Update()
    {
        if (_asm.state.ToString() == "chase")
        {
            if (_asm.canResream == true)
            {
                _asm.canResream = false;
                _asm._monsterSource.clip = _asm.monsterScream;
                _asm._monsterSource.Play();
            }
            if (_collider.enabled == false)
            {
                _collider.enabled = true;
            }
        }
        else
        {
            if (_collider.enabled == true)
            {
                _collider.enabled = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SceneManager.LoadScene("AlienGameOver");
        }
    }
}
