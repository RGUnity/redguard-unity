using System;
using System.Collections.Generic;
using UnityEngine;
using RGFileImport;

public partial class PlayerMain: MonoBehaviour
{
    // statemachine stuff
    public abstract class State
    {
        public int id;
        public bool panic;
        public bool s_internal;
        public PlayerMain main;
        public virtual void Entry() {}
        public virtual void InternalEntry() {}
        public virtual void Exit() {}
        public virtual void InternalExit() {}

        public State(PlayerMain m)
        {
            main = m;
            panic = false;
        }

    }

    public enum AnimState
    {
        state_init              = -1,
        state_panic             = RGRGMAnimStore.AnimGroup.anim_panic,
        state_walk_forward      = RGRGMAnimStore.AnimGroup.anim_walk_forward,
        state_walk_backward     = RGRGMAnimStore.AnimGroup.anim_walk_backward,
        state_run_forward       = RGRGMAnimStore.AnimGroup.anim_run_forward,
        state_turn_left         = RGRGMAnimStore.AnimGroup.anim_turn_left,
        state_turn_right        = RGRGMAnimStore.AnimGroup.anim_turn_right,

        state_jump_start        = RGRGMAnimStore.AnimGroup.anim_jump_start,
        state_jump_up           = RGRGMAnimStore.AnimGroup.anim_jump_up,
        state_fall              = RGRGMAnimStore.AnimGroup.anim_fall,
        state_land              = RGRGMAnimStore.AnimGroup.anim_land,

        state_locked            = 1337, //  bit of a hack but better than before :)
    }

    public enum Event
    {
        init,
        anim_done,
        input_jump,
        input_runfwd,
        input_runfwd_released,
        input_stepleft,
        input_stepleft_released,
        input_stepright,
        input_stepright_released,
        input_turnleft,
        input_turnleft_released,
        input_turnright,
        input_turnright_released,
        input_walkbckw,
        input_walkbckw_released,
        input_walkfwd,
        input_walkfwd_released,
        //close to wall(?),
        player_landed,

    }


    Dictionary<ST, TD> transitions;
    State currentState;
    [SerializeField] int state_int;

    State nextState;
    [SerializeField] bool animShouldExit;

    class ST // state transition
    {
        State currentState;
        Event trigger;
        public ST(State state, Event ev)
        {
            currentState = state;
            trigger = ev;
        }

        public override int GetHashCode()
        {
            return 17 + 31 * currentState.id.GetHashCode() + 31 * trigger.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            ST other = obj as ST;
            return other != null && this.currentState.id == other.currentState.id && this.trigger == other.trigger;
        }
    }
    class TD //transition data
    {
        public State state;
        public bool internalTransition;
        public TD(State s, bool i)
        {
            state = s;
            internalTransition = i;
        }
    }
    public PlayerMain()
    {
        currentState = new State_init(this);
        transitions = new Dictionary<ST, TD>
        {
            {new ST(new State_init(this),                      Event.init),                        new TD(new State_panic(this), false)},

// walking forward tree
            {new ST(new State_panic(this),                     Event.input_walkfwd),               new TD(new State_walk_forward(this), false)},
            {new ST(new State_walk_forward(this),              Event.input_walkfwd_released),      new TD(new State_panic(this), false)},
            {new ST(new State_walk_forward(this),              Event.input_walkbckw),              new TD(new State_walk_backward(this), false)},
            {new ST(new State_walk_forward(this),              Event.input_runfwd),                new TD(new State_run_forward(this), false)},

            // walking forward turns 
            {new ST(new State_walk_forward(this),              Event.input_turnleft),              new TD(new State_walk_forward_turn_left(this), true)},
            {new ST(new State_walk_forward_turn_left(this),    Event.input_turnleft_released),     new TD(new State_walk_forward(this), true)},
            {new ST(new State_walk_forward_turn_left(this),    Event.input_walkfwd_released),      new TD(new State_turn_left(this), false)},

            {new ST(new State_walk_forward(this),              Event.input_turnright),             new TD(new State_walk_forward_turn_right(this), true)},
            {new ST(new State_walk_forward_turn_right(this),   Event.input_turnright_released),    new TD(new State_walk_forward(this), true)},
            {new ST(new State_walk_forward_turn_right(this),   Event.input_walkfwd_released),      new TD(new State_turn_right(this), false)},

// walking backwards tree
            {new ST(new State_panic(this),                     Event.input_walkbckw),              new TD(new State_walk_backward(this), false)},
            {new ST(new State_walk_backward(this),             Event.input_walkbckw_released),     new TD(new State_panic(this), false)},
            {new ST(new State_walk_backward(this),             Event.input_runfwd),                new TD(new State_run_forward(this), false)},
            {new ST(new State_walk_backward(this),             Event.input_walkfwd),               new TD(new State_walk_forward(this), false)},

            // walking backwards turns
            {new ST(new State_walk_backward(this),              Event.input_turnleft),              new TD(new State_walk_backward_turn_left(this), true)},
            {new ST(new State_walk_backward_turn_left(this),    Event.input_turnleft_released),     new TD(new State_walk_backward(this), true)},
            {new ST(new State_walk_backward_turn_left(this),    Event.input_walkbckw_released),      new TD(new State_turn_left(this), false)},

            {new ST(new State_walk_backward(this),              Event.input_turnright),             new TD(new State_walk_backward_turn_right(this), true)},
            {new ST(new State_walk_backward_turn_right(this),   Event.input_turnright_released),    new TD(new State_walk_backward(this), true)},
            {new ST(new State_walk_backward_turn_right(this),   Event.input_walkbckw_released),     new TD(new State_turn_right(this), false)},


// running forward tree tree
            {new ST(new State_panic(this),                     Event.input_runfwd),                 new TD(new State_run_forward(this), false)},
            {new ST(new State_run_forward(this),               Event.input_runfwd_released),        new TD(new State_panic(this), false)},
            {new ST(new State_run_forward(this),               Event.input_walkfwd),                new TD(new State_walk_forward(this), false)},
            {new ST(new State_run_forward(this),               Event.input_walkbckw),               new TD(new State_walk_backward(this), false)},

            // running forward turns
            {new ST(new State_run_forward(this),              Event.input_turnleft),                new TD(new State_run_forward_turn_left(this), true)},
            {new ST(new State_run_forward_turn_left(this),    Event.input_turnleft_released),       new TD(new State_run_forward(this), true)},
            {new ST(new State_run_forward_turn_left(this),    Event.input_runfwd_released),         new TD(new State_turn_left(this), false)},

            {new ST(new State_run_forward(this),              Event.input_turnright),               new TD(new State_run_forward_turn_right(this), true)},
            {new ST(new State_run_forward_turn_right(this),   Event.input_turnright_released),      new TD(new State_run_forward(this), true)},
            {new ST(new State_run_forward_turn_right(this),   Event.input_runfwd_released),         new TD(new State_turn_right(this), false)},


            // turns
            {new ST(new State_panic(this),                     Event.input_turnleft),              new TD(new State_turn_left(this), false)},
            {new ST(new State_turn_left(this),                 Event.input_turnleft_released),     new TD(new State_panic(this), false)},
            {new ST(new State_turn_left(this),                 Event.input_walkfwd),               new TD(new State_walk_forward_turn_left(this), false)},
            {new ST(new State_turn_left(this),                 Event.input_walkbckw),              new TD(new State_walk_backward_turn_left(this), false)},
            {new ST(new State_turn_left(this),                 Event.input_runfwd),                new TD(new State_run_forward_turn_left(this), false)},

            {new ST(new State_panic(this),                     Event.input_turnright),             new TD(new State_turn_right(this), false)},
            {new ST(new State_turn_right(this),                Event.input_turnright_released),    new TD(new State_panic(this), false)},
            {new ST(new State_turn_right(this),                Event.input_walkfwd),               new TD(new State_walk_forward_turn_right(this), false)},
            {new ST(new State_turn_right(this),                Event.input_walkbckw),              new TD(new State_walk_backward_turn_right(this), false)},
            {new ST(new State_turn_right(this),                Event.input_runfwd),                new TD(new State_run_forward_turn_right(this), false)},


        };
    }
    bool GetNext(Event ev, out TD next)
    {
        ST transition = new ST(currentState, ev);
        if(!transitions.TryGetValue(transition, out next))
            return false;
        return true;
    }

    public void MoveNext(Event ev)
    {
        TD next;
        bool transitioned = GetNext(ev, out next);
        if(transitioned)
        {
            nextState = next.state;
            if(next.internalTransition== true)
            {
                currentState.InternalExit();
                nextState.InternalEntry();

                currentState = nextState;
                state_int = currentState.id;
            }
            else if(currentState.panic == true)
                animDoExitFcn();
            else
                animShouldExit = true;
        }
    }
    bool animShouldExitFcn()
    {
        return animShouldExit;
    }
    void animDoExitFcn()
    {
        currentState.Exit();
        nextState.Entry();
        currentState = nextState;
        animShouldExit = false;
        state_int = currentState.id;
    }
    public void setLocked(bool l)
    {
    }



    // Unity stuff
    private RGScriptedObject player;
    [SerializeField] private CharacterController cc;
    [SerializeField] private PlayerMovementConfig config;

    private Vector3 _velocity;
    [SerializeField] private float _yRotation;

    public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
    {   
        // disable CC, otherwise it will overwrite transform
        cc.enabled = false;
        cc.transform.SetPositionAndRotation(pos, rot);
        cc.enabled = true;
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
 

        float rot = _yRotation * config.turnSpeed * Time.deltaTime * 60;
        transform.Rotate(0, _yRotation, 0);

        _velocity.y += config.groundMagnet;
        Vector3 localvel = transform.TransformDirection(_velocity);
        cc.Move(localvel);
    }




    public class State_init: State
    {
        public State_init(PlayerMain m):base(m)
        {
            id = -1;
            panic = true;
        }
        public override void Exit()
        {
            main.player = RGObjectStore.GetPlayer();
            if (main.player != null && main.player.animations != null)
            {
                main.player.animations.shouldExitFcn = main.animShouldExitFcn;
                main.player.animations.doExitFcn = main.animDoExitFcn;
            }
            main.player.SetAnim((int)RGRGMAnimStore.AnimGroup.anim_land, 0);
        }
    }
    public class State_panic: State
    {
        public State_panic(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_panic;
            panic = true;
        }
        public override void Entry()
        {
            main.player.SetAnim((int)RGRGMAnimStore.AnimGroup.anim_panic, 0);
            main._velocity = Vector3.zero;
            main._yRotation = 0.0f;
        }
    }
    public class State_turn_left: State
    {
        public State_turn_left(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_turn_left;
        }
        public override void Entry()
        {
            main.player.SetAnim((int)RGRGMAnimStore.AnimGroup.anim_turn_left, 0);
            main._yRotation = -1.0f;
        }
    }
    public class State_turn_right: State
    {
        public State_turn_right(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_turn_right;
        }
        public override void Entry()
        {
            main.player.SetAnim((int)RGRGMAnimStore.AnimGroup.anim_turn_right, 0);
            main._yRotation = 1.0f;
        }
    }







}
