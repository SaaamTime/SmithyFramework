
using UnityEngine;
using System;

public abstract class AbstractInpuut_ASWD  : MonoBehaviour
{
    public float speed_W;
    public float speed_A;
    public float speed_S;
    public float speed_D;
    public abstract void DoEvent_W(float _speed);
    public abstract void DoEvent_A(float _speed);
    public abstract void DoEvent_S(float _speed);
    public abstract void DoEvent_D(float _speed);
    public abstract void DoEvent_E();
    public abstract void DoEvent_Empty();

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            DoEvent_A(-Time.deltaTime * speed_A);
        }
        if (Input.GetKey(KeyCode.W))
        {
            DoEvent_W(Time.deltaTime * speed_W);
        }
        if (Input.GetKey(KeyCode.S))
        {
            DoEvent_S(-Time.deltaTime * speed_S);
        }
        if (Input.GetKey(KeyCode.D))
        {
            DoEvent_D(Time.deltaTime * speed_D);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            DoEvent_E();
        }
        if (!Input.anyKey)
        {
            DoEvent_Empty();
        }
    }
}
