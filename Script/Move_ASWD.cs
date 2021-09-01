
using UnityEngine;

public class Move_ASWD : MonoBehaviour
{
    public MotorCtrl motor;


    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            motor.Rotate(-Time.deltaTime*40);
        }
        if (Input.GetKey(KeyCode.W))
        {
            motor.Walk();
        }
        if (Input.GetKey(KeyCode.S))
        {
            motor.BackOff();
        }
        if (Input.GetKey(KeyCode.D))
        {
            motor.Rotate(Time.deltaTime*40);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            TriggerManager.Instance.DoTrigger();
        }
        if (!Input.anyKey)
        {
            motor.StopSlow();
        }
    }
}
