
using UnityEngine;

public class AliceInput : AbstractInpuut_ASWD
{
    public Rigidbody rig;
    public float velocity = 0f;
    public Animator animator;
    public float velocity_max = 1f;

    #region 生命周期
    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        base.speed_W = 1;
        base.speed_A = 160;
        base.speed_S = 0.5f;
        base.speed_D = base.speed_A;
    }
    #endregion

    public void SetAnimParam_Speed(float _speed) {
        animator.SetFloat("Speed", Mathf.Abs(_speed));
    }

    public virtual void Rotate(float _velocity) {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, 
            Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y + _velocity, transform.localEulerAngles.z), 
            Time.deltaTime*5);
        SetAnimParam_Speed(velocity == 0 ? 0.2f : velocity);
    }

    public virtual void RunSlow()
    {
        MoveForward(Time.deltaTime,0.6f);
    }

    public virtual void Run() {
        MoveForward(Time.deltaTime,1f);
    }

    public virtual void MoveForward(float _velocity,float _maxVelocity=0.3f)
    {
        velocity_max = Mathf.Abs(_maxVelocity);
        velocity += _velocity;
        velocity = Mathf.Min(velocity, velocity_max);
        velocity = Mathf.Max(velocity, 0);
        rig.position = Vector3.Lerp(rig.position, rig.position + transform.forward * velocity * 0.3f, 5 * Time.deltaTime);
        SetAnimParam_Speed(this.velocity);
    }

    public virtual void MoveBack(float _velocity,float _maxVelocity=0.3f) {
        velocity_max = Mathf.Abs(_maxVelocity) * -1;
        velocity += _velocity;
        velocity = Mathf.Max(velocity, velocity_max);
        rig.position = Vector3.Lerp(rig.position, rig.position + transform.forward * velocity * 0.3f, 5 * Time.deltaTime);
        SetAnimParam_Speed(this.velocity);
    }

    public override void DoEvent_W(float _speed)
    {
        MoveForward(_speed);
    }

    public override void DoEvent_A(float _speed)
    {
        Rotate(_speed);
    }

    public override void DoEvent_S(float _speed)
    {
        MoveBack(_speed);
    }

    public override void DoEvent_D(float _speed)
    {
        Rotate(_speed);
    }

    public override void DoEvent_E()
    {
        TriggerManager.Instance.DoTrigger();
    }

    public override void DoEvent_Empty()
    {
        MoveForward(-Time.deltaTime);
    }
}
