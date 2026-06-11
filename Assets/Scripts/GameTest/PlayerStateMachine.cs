using System;
using System.Collections.Generic;
using UnityEngine;
using RGFileImport;

public partial class PlayerMain: MonoBehaviour
{

    Dictionary<ST, TD> transitions;
    State currentState;
    [SerializeField] int state_int;

    State nextState;
    [SerializeField] bool animShouldExit;

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
            Debug.Log($"movenext: {next.state}");
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
        animShouldExit = false; // do this before entry for the anim_done transitions
        currentState.Exit();
        nextState.Entry();
        currentState = nextState;
        state_int = currentState.id;
    }

    // init and panic state

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
    public class State_locked: State
    {
        public State_locked(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_panic+1000;
        }
        public override void Entry()
        {
            main.player.SetAnim((int)RGRGMAnimStore.AnimGroup.anim_panic, 0);
            main._velocity = Vector3.zero;
            main._yRotation = 0.0f;
        }
    }

}
