using UnityEngine;
namespace PlayerMechanics
{

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(IPlayerController))]
    [RequireComponent(typeof(IPlayerAttacks))]
    public class PlayerMedia : MonoBehaviour
    {
        private Animator _anim;
        private AudioSource _source;
        private IPlayerController _playerCont;
        private IPlayerAttacks _playerAttack;

        [SerializeField] private AudioSource jumpSound;
        [SerializeField] private AudioSource caseSound;


        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _source = GetComponent<AudioSource>();
            _playerCont = GetComponent<IPlayerController>();
            _playerAttack = GetComponent<IPlayerAttacks>();

        }

        private void Update()
        {
            _anim.SetBool("isGrounded", _playerCont.grounded);
            _anim.SetBool("isMoving", Mathf.Abs(_playerCont.velocity.x) >= 0.01);
            _anim.SetBool("ranged", _playerAttack.ranged);
            _anim.SetBool("atack", _playerAttack.attacked);
            if (_playerAttack.attacked || _playerAttack.ranged)
            {
                caseSound.Play();
            }
            if (_playerCont.jumpingThisFrame)
            {
                _anim.SetTrigger("jumped");
                jumpSound.Play();
            }
           
            if (_playerCont.landingThisFrame) _anim.SetTrigger("landed");
            if (_playerCont.velocity.x != 0) transform.localScale = new Vector3(_playerCont.velocity.x > 0 ? 1 : -1, 1, 1);
            
            // Debug.Log("Ata " + _playerAttack.attacked + " " + _playerAttack.hit);
            // if (_player.jumpingThisFrame)
            // {

            // }
            // else if (_player.landingThisFrame)
            // {
            //     Debug.Log("land");
            // }

            // if (_player.rawMovement.x > 0)
            // {
            //     Debug.Log("right");
            // }
            // else if (_player.rawMovement.x < 0)
            // {
            //     Debug.Log("left");
            // }
        }
    }
}