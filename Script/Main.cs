using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public AliceInput role;
    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TriggerManager.Instance.Update(role.transform.position);
    }

    
}
