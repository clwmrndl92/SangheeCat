using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorController : MonoBehaviour
{   
    public static float speed = 2f;
    [SerializeField] private GameObject _bigWheel;
    [SerializeField] private GameObject[] _smallWheel;

    private const float WHEELspeed = 1f;

    private Animator _animator;
    
    Player player;

    private enum CharactorState
    {
        Wait,
        Idle,
        Run,
        Hurt,
        Die,
        DieIdle,

    }
    CharactorState _currentState = CharactorState.Wait;

    private void Awake() 
    {
        _animator = GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Start()
    {
        ChangeState(CharactorState.Run);
    }

    void Update()
    {
        if(_currentState == CharactorState.Run) {
            RunUpdate();
        }
    }

    void RunUpdate()
    {
        speed = player.velocity.x / 7.5f;
        _animator.speed = 2f;
        _bigWheel.transform.Rotate(speed * WHEELspeed * Vector3.back);
        foreach (var sw in _smallWheel)
        {
            sw.transform.Rotate(speed * WHEELspeed * 2 * Vector3.back);
        }
    }

    void ChangeState(CharactorState state) {
        if(_currentState == state) return;
        _animator.Play("Idle");
        _animator.SetBool(_currentState.ToString(), false);
        _animator.SetBool(state.ToString(), true);
        _currentState = state;
    }
    public void Hurt(){
        _animator.SetTrigger("Hurt");
    }
    bool isDie = false;
    public void Die(){
        isDie = !isDie;
        if(isDie) speed = 0;
        else speed = 2f;
        ChangeState(isDie? CharactorState.Die : CharactorState.Run);
    }
}
