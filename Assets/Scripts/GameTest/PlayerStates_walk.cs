using RGFileImport;
using UnityEngine;

public partial class PlayerMain: MonoBehaviour
{
    public class State_walk_forward: State
    {
        public State_walk_forward(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_walk_forward;
        }
        public override void Entry()
        {
            main.player.SetAnim((int)RGRGMAnimStore.AnimGroup.anim_walk_forward, 0);
            main._velocity = Vector3.forward* (1.0f * main.config.walkSpeed / 60);
        }
        public override void Exit()
        {
            main._velocity = Vector3.zero;
        }
    }
    public class State_walk_forward_turn_left: State_walk_forward
    {
        public State_walk_forward_turn_left(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_walk_forward + 1000;
        }
        public override void InternalEntry()
        {
            main._yRotation = -1.0f;
        }
        public override void InternalExit()
        {
            main._yRotation = 0.0f;
        }
    }
    public class State_walk_forward_turn_right: State_walk_forward
    {
        public State_walk_forward_turn_right(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_walk_forward + 1001;
        }
        public override void InternalEntry()
        {
            main._yRotation = 1.0f;
        }
        public override void InternalExit()
        {
            main._yRotation = 0.0f;
        }
    }


    public class State_run_forward: State
    {
        public State_run_forward(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_run_forward;
        }
        public override void Entry()
        {
            main.player.SetAnim((int)RGRGMAnimStore.AnimGroup.anim_run_forward, 0);
            main._velocity = Vector3.forward* (1.0f * main.config.walkSpeed / 60);
        }
        public override void Exit()
        {
            main._velocity = Vector3.zero;
        }
    }
    public class State_run_forward_turn_left: State_run_forward
    {
        public State_run_forward_turn_left(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_run_forward + 1000;
        }
        public override void InternalEntry()
        {
            main._yRotation = -1.0f;
        }
        public override void InternalExit()
        {
            main._yRotation = 0.0f;
        }
    }
    public class State_run_forward_turn_right: State_run_forward
    {
        public State_run_forward_turn_right(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_run_forward + 1001;
        }
        public override void InternalEntry()
        {
            main._yRotation = 1.0f;
        }
        public override void InternalExit()
        {
            main._yRotation = 0.0f;
        }
    }



    public class State_walk_backward: State
    {
        public State_walk_backward(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_walk_backward;
        }
        public override void Entry()
        {
            main.player.SetAnim((int)RGRGMAnimStore.AnimGroup.anim_walk_backward, 0);
            main._velocity = Vector3.forward* (-1.0f * main.config.walkSpeed / 60);
        }
        public override void Exit()
        {
            main._velocity = Vector3.zero;
        }
    }
    public class State_walk_backward_turn_left: State_walk_backward
    {
        public State_walk_backward_turn_left(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_walk_backward + 1000;
        }
        public override void InternalEntry()
        {
            main._yRotation = -1.0f;
        }
        public override void InternalExit()
        {
            main._yRotation = 0.0f;
        }
    }
    public class State_walk_backward_turn_right: State_walk_backward
    {
        public State_walk_backward_turn_right(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_walk_backward + 1001;
        }
        public override void InternalEntry()
        {
            main._yRotation = 1.0f;
        }
        public override void InternalExit()
        {
            main._yRotation = 0.0f;
        }
    }


}
