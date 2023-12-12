using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSleepController : NPCBaseController
{
    protected override void OnAcceptedButtonClicked()
    {
        GameMode.Instance.Sleep();
    }
}
