using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationMove : EnvironmentMove
{
    public override void OnGetEndPoint()
    {
        gameObject.SetActive(false);
    }
}
