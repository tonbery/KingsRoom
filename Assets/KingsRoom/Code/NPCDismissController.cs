using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDismissController : NPCBaseController
{
    protected override void OnAcceptedButtonClicked()
    {
        GameMode.Instance.Dismiss();
    }
}
