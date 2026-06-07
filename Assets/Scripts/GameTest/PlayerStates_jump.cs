using RGFileImport;
using UnityEngine;

public partial class PlayerMain: MonoBehaviour
{
// standing jump
    public class State_jump_start: State
    {
        public State_jump_start(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_jump_start;
        }
        public override void Entry()
        {
            main.animShouldExit = true;
            main.player.SetAnim((int)RGRGMAnimStore.AnimGroup.anim_jump_start, 0);
        }
    }
    public class State_jump_up: State
    {
        public State_jump_up(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_jump_up;
        }
        public override void Entry()
        {
            main.animShouldExit = true;
            main.player.SetAnim((int)RGRGMAnimStore.AnimGroup.anim_jump_up, 0);
            main._velocity = Vector3.up* (1.0f * main.config.jumpHeight);
            main._isGrounded = false;
        }
    }
    public class State_fall: State
    {
        public State_fall(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_fall;
        }
        public override void Entry()
        {
            main.player.SetAnim((int)RGRGMAnimStore.AnimGroup.anim_fall, 0);
        }
    }
    public class State_land: State
    {
        public State_land(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_land;
        }
        public override void Entry()
        {
            main.animShouldExit = true;
            main.player.SetAnim((int)RGRGMAnimStore.AnimGroup.anim_land, 0);
            main._isGrounded = true;
        }
    }

// forward jump
    public class State_jump_start_forward: State_jump_start
    {
        public State_jump_start_forward(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_jump_start+1000;
        }
    }

    public class State_jump_forward: State
    {
        public State_jump_forward(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_jump_forward;
        }
        public override void Entry()
        {
            main.animShouldExit = true;
            // TODO: which animation here?
            main.player.SetAnim((int)RGRGMAnimStore.AnimGroup.anim_jump_forward, 0);

            main._velocity = Vector3.up* (1.0f * main.config.jumpHeight);
            main._velocity += Vector3.forward* (1.0f * main.config.walkForwardJumpSpeed/ 60);
            main._isGrounded = false;
        }
    }

// backward jump
    public class State_jump_start_backward: State_jump_start
    {
        public State_jump_start_backward(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_jump_start+1001;
        }
    }
    public class State_jump_backward: State
    {
        public State_jump_backward(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_jump_backward;
        }
        public override void Entry()
        {
            main.animShouldExit = true;
            main.player.SetAnim((int)RGRGMAnimStore.AnimGroup.anim_jump_backward, 0);
            main._velocity = Vector3.up* (1.0f * main.config.jumpHeight);
            main._velocity += Vector3.forward* -(1.0f * main.config.walkBackwardJumpSpeed/ 60);
            main._isGrounded = false;
        }
    }
    public class State_fall_backward: State
    {
        public State_fall_backward(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_fall_backward;
        }
        public override void Entry()
        {
            main.player.SetAnim((int)RGRGMAnimStore.AnimGroup.anim_fall_backward, 0);
        }
    }
    public class State_land_backward: State
    {
        public State_land_backward(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_land_backward;
        }
        public override void Entry()
        {
            main.animShouldExit = true;
            main.player.SetAnim((int)RGRGMAnimStore.AnimGroup.anim_land_backward, 0);
            main._isGrounded = true;
        }
    }


// run jump
    public class State_jump_run_arms: State
    {
        public State_jump_run_arms(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_jump_run_arms;
        }
        public override void InternalEntry()
        {
            main.animShouldExit = true;
            // TODO: which animation here?
            main.player.SetAnim((int)RGRGMAnimStore.AnimGroup.anim_jump_run_arms, 0);
            main._velocity = Vector3.up* (1.0f * main.config.jumpHeight);
            main._velocity += Vector3.forward* (1.0f * main.config.runForwardJumpSpeed/ 60);
            main._isGrounded = false;
        }
    }
    public class State_run_fall_up_arms: State_fall
    {
        public State_run_fall_up_arms(PlayerMain m):base(m)
        {
            id = (int)RGRGMAnimStore.AnimGroup.anim_fall_up_arms+1000;
        }
        public override void Entry()
        {
            main.player.SetAnim((int)RGRGMAnimStore.AnimGroup.anim_fall_up_arms, 0);
            base.Entry();
        }
    }

}
