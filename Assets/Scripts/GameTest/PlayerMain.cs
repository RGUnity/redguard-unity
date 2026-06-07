using System;
using System.Collections.Generic;
using UnityEngine;
using RGFileImport;

public partial class PlayerMain: MonoBehaviour
{
    public PlayerMain()
    {
        currentState = new State_init(this);
        transitions = new Dictionary<ST, TD>
        {
// keep this table up-to-date with anim_state_machine.txt in /docs so we get a nice plantuml diagram

//                     from state                              event                                          to state                         internal transition?
// init
            {new ST(new State_init(this),                      Event.init),                        new TD(new State_panic(this),                    false)},

// walking forward
            {new ST(new State_panic(this),                     Event.input_walkfwd),               new TD(new State_walk_forward(this),             false)},
            {new ST(new State_walk_forward(this),              Event.input_walkfwd_released),      new TD(new State_panic(this),                    false)},
            {new ST(new State_walk_forward(this),              Event.input_walkbckw),              new TD(new State_walk_backward(this),            false)},
            {new ST(new State_walk_forward(this),              Event.input_runfwd),                new TD(new State_run_forward(this),              false)},

            // walking forward turns 
            {new ST(new State_walk_forward(this),              Event.input_turnleft),              new TD(new State_walk_forward_turn_left(this),   true)},
            {new ST(new State_walk_forward_turn_left(this),    Event.input_turnleft_released),     new TD(new State_walk_forward(this),             true)},
            {new ST(new State_walk_forward_turn_left(this),    Event.input_walkfwd_released),      new TD(new State_turn_left(this),                false)},

            {new ST(new State_walk_forward(this),              Event.input_turnright),             new TD(new State_walk_forward_turn_right(this),  true)},
            {new ST(new State_walk_forward_turn_right(this),   Event.input_turnright_released),    new TD(new State_walk_forward(this),             true)},
            {new ST(new State_walk_forward_turn_right(this),   Event.input_walkfwd_released),      new TD(new State_turn_right(this),               false)},

// walking backwards
            {new ST(new State_panic(this),                     Event.input_walkbckw),              new TD(new State_walk_backward(this),            false)},
            {new ST(new State_walk_backward(this),             Event.input_walkbckw_released),     new TD(new State_panic(this),                    false)},
            {new ST(new State_walk_backward(this),             Event.input_runfwd),                new TD(new State_run_forward(this),              false)},
            {new ST(new State_walk_backward(this),             Event.input_walkfwd),               new TD(new State_walk_forward(this),             false)},

            // walking backwards turns
            {new ST(new State_walk_backward(this),              Event.input_turnleft),              new TD(new State_walk_backward_turn_left(this), true)},
            {new ST(new State_walk_backward_turn_left(this),    Event.input_turnleft_released),     new TD(new State_walk_backward(this),           true)},
            {new ST(new State_walk_backward_turn_left(this),    Event.input_walkbckw_released),      new TD(new State_turn_left(this),              false)},

            {new ST(new State_walk_backward(this),              Event.input_turnright),             new TD(new State_walk_backward_turn_right(this),true)},
            {new ST(new State_walk_backward_turn_right(this),   Event.input_turnright_released),    new TD(new State_walk_backward(this),           true)},
            {new ST(new State_walk_backward_turn_right(this),   Event.input_walkbckw_released),     new TD(new State_turn_right(this),              false)},


// running forward
            {new ST(new State_panic(this),                     Event.input_runfwd),                 new TD(new State_run_forward(this),             false)},
            {new ST(new State_run_forward(this),               Event.input_runfwd_released),        new TD(new State_panic(this),                   false)},
            {new ST(new State_run_forward(this),               Event.input_walkfwd),                new TD(new State_walk_forward(this),            false)},
            {new ST(new State_run_forward(this),               Event.input_walkbckw),               new TD(new State_walk_backward(this),           false)},

            // running forward turns
            {new ST(new State_run_forward(this),              Event.input_turnleft),                new TD(new State_run_forward_turn_left(this),   true)},
            {new ST(new State_run_forward_turn_left(this),    Event.input_turnleft_released),       new TD(new State_run_forward(this),             true)},
            {new ST(new State_run_forward_turn_left(this),    Event.input_runfwd_released),         new TD(new State_turn_left(this),               false)},

            {new ST(new State_run_forward(this),              Event.input_turnright),               new TD(new State_run_forward_turn_right(this),  true)},
            {new ST(new State_run_forward_turn_right(this),   Event.input_turnright_released),      new TD(new State_run_forward(this),             true)},
            {new ST(new State_run_forward_turn_right(this),   Event.input_runfwd_released),         new TD(new State_turn_right(this),              false)},


// turns
            {new ST(new State_panic(this),                     Event.input_turnleft),              new TD(new State_turn_left(this),                false)},
            {new ST(new State_turn_left(this),                 Event.input_turnleft_released),     new TD(new State_panic(this),                    false)},
            {new ST(new State_turn_left(this),                 Event.input_walkfwd),               new TD(new State_walk_forward_turn_left(this),   false)},
            {new ST(new State_turn_left(this),                 Event.input_walkbckw),              new TD(new State_walk_backward_turn_left(this),  false)},
            {new ST(new State_turn_left(this),                 Event.input_runfwd),                new TD(new State_run_forward_turn_left(this),    false)},

            {new ST(new State_panic(this),                     Event.input_turnright),             new TD(new State_turn_right(this),               false)},
            {new ST(new State_turn_right(this),                Event.input_turnright_released),    new TD(new State_panic(this),                    false)},
            {new ST(new State_turn_right(this),                Event.input_walkfwd),               new TD(new State_walk_forward_turn_right(this),  false)},
            {new ST(new State_turn_right(this),                Event.input_walkbckw),              new TD(new State_walk_backward_turn_right(this), false)},
            {new ST(new State_turn_right(this),                Event.input_runfwd),                new TD(new State_run_forward_turn_right(this),   false)},

// jumps
            // standing jump
            {new ST(new State_panic(this),                     Event.input_jump),                  new TD(new State_jump_start(this),               false)},
            {new ST(new State_jump_start(this),                Event.anim_done),                   new TD(new State_jump_up(this),                  false)},
            {new ST(new State_jump_up(this),                   Event.anim_done),                   new TD(new State_fall(this),                     false)},
            {new ST(new State_fall(this),                      Event.player_landed),               new TD(new State_land(this),                     false)},
            {new ST(new State_land(this),                      Event.anim_done),                   new TD(new State_panic(this),                    false)},

            // forward jump
            {new ST(new State_walk_forward(this),              Event.input_jump),                  new TD(new State_jump_start_forward(this),       false)},
            {new ST(new State_jump_start_forward(this),        Event.anim_done),                   new TD(new State_jump_forward(this),             false)},
            {new ST(new State_jump_forward(this),              Event.anim_done),                   new TD(new State_fall(this),                     false)},

            // backward jump
            {new ST(new State_walk_backward(this),             Event.input_jump),                  new TD(new State_jump_start_backward(this),      false)},
            {new ST(new State_jump_start_backward(this),       Event.anim_done),                   new TD(new State_jump_backward(this),            false)},
            {new ST(new State_jump_backward(this),             Event.anim_done),                   new TD(new State_fall_backward(this),            false)},
            {new ST(new State_fall_backward(this),             Event.player_landed),               new TD(new State_land_backward(this),            false)},
            {new ST(new State_land_backward(this),             Event.anim_done),                  new TD(new State_panic(this),                     false)},
        };
    }

    public void setLocked(bool l)
    {
    }



    // Unity stuff
    private RGScriptedObject player;
    [SerializeField] private CharacterController cc;
    [SerializeField] private PlayerMovementConfig config;

    private bool _isGrounded;
    public RGScriptedObject _currentScriptedGround = default;

    private Vector3 _velocity;
    [SerializeField] private float _yRotation;

    public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
    {   
        // disable CC, otherwise it will overwrite transform
        cc.enabled = false;
        cc.transform.SetPositionAndRotation(pos, rot);
        cc.enabled = true;
    }   

    private bool IsGrounded()
    {
        float castRadius = 0.34f;
        float castDistance = cc.height / 2 - (castRadius - 0.14f);

        if (Physics.SphereCast(transform.position, castRadius, Vector3.down, out RaycastHit sphereHit, castDistance, config.groundLayers))
        {

            if (sphereHit.transform.TryGetComponent(out RGScriptedObject platform))
            {
                _currentScriptedGround = platform;
                _currentScriptedGround.playerStanding = true;
//                this.transform.SetParent(_currentScriptedGround.transform);
            }
            else
            {
                if(_currentScriptedGround != null)
                    _currentScriptedGround.playerStanding = false;
//                this.transform.SetParent(null);
                _currentScriptedGround = null;
            }
            return true;
        }
        else
        {
            if(_currentScriptedGround != null)
                _currentScriptedGround.playerStanding = false;
//            this.transform.SetParent(null);
            _currentScriptedGround = null;
            return false;
        }
    }

    
    public void FixedUpdate()
    {
        if(currentState.id == -1)
            MoveNext(Event.init);
        if(Game.Input.moveForward)
        {
            if(Game.Input.moveModifier)
                MoveNext(Event.input_runfwd);
            else
                MoveNext(Event.input_walkfwd);
        }
        else
        {
            MoveNext(Event.input_walkfwd_released);
            MoveNext(Event.input_runfwd_released);
        }

        if(Game.Input.moveBackward)
            MoveNext(Event.input_walkbckw);
        else
            MoveNext(Event.input_walkbckw_released);

        if(Game.Input.jump)
            MoveNext(Event.input_jump);

        if(Game.Input.stepLeft)
            MoveNext(Event.input_stepleft);
        else
            MoveNext(Event.input_stepleft_released);
        if(Game.Input.stepRight)
            MoveNext(Event.input_stepright);
        else
            MoveNext(Event.input_stepright_released);

        if(Game.Input.turnLeft)
            MoveNext(Event.input_turnleft);
        else
            MoveNext(Event.input_turnleft_released);
        if(Game.Input.turnRight)
            MoveNext(Event.input_turnright);
        else
            MoveNext(Event.input_turnright_released);
 
        if(animShouldExit) // this should be set to true in state entry if this is a valid event
            MoveNext(Event.anim_done);

        if(IsGrounded())
        {
            Debug.Log("GROUNDED");
            MoveNext(Event.player_landed);
        }

        float rot = _yRotation * config.turnSpeed * Time.deltaTime * 60;
        transform.Rotate(0, _yRotation, 0);

        _velocity.y += config.groundMagnet;
        Vector3 localvel = transform.TransformDirection(_velocity);
        cc.Move(localvel);
    }
}
