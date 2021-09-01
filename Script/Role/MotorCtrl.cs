
using UnityEngine;

public class MotorCtrl : MonoBehaviour
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

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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

    public virtual void Walk() {
        MoveForward(Time.deltaTime);
    }

    public virtual void BackOff()
    {
        MoveBack(Time.deltaTime);
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
        velocity -= _velocity;
        velocity = Mathf.Max(velocity, velocity_max);
        rig.position = Vector3.Lerp(rig.position, rig.position + transform.forward * velocity * 0.3f, 5 * Time.deltaTime);
        SetAnimParam_Speed(this.velocity);
    }

    public void StopMove()
    {
        //if (input.sqrMagnitude < 0.1 || !isGrounded) return;

        //RaycastHit hitinfo;
        //Ray ray = new Ray(transform.position + new Vector3(0, stopMoveHeight, 0), targetDirection.normalized);

        //if (Physics.Raycast(ray, out hitinfo, _capsuleCollider.radius + stopMoveDistance, stopMoveLayer))
        //{
        //    var hitAngle = Vector3.Angle(Vector3.up, hitinfo.normal);

        //    if (hitinfo.distance <= stopMoveDistance && hitAngle > 85)
        //        stopMove = true;
        //    else if (hitAngle >= slopeLimit + 1f && hitAngle <= 85)
        //        stopMove = true;
        //}
        //else if (Physics.Raycast(ray, out hitinfo, 1f, groundLayer))
        //{
        //    var hitAngle = Vector3.Angle(Vector3.up, hitinfo.normal);
        //    if (hitAngle >= slopeLimit + 1f && hitAngle <= 85)
        //        stopMove = true;
        //}
        //else
        //    stopMove = false;
    }

    public void StopSlow() {
        MoveForward(-Time.deltaTime);
    }

}
