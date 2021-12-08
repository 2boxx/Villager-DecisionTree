using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : Node
{
    public delegate void ActionFunction();

    private ActionFunction _action;

    public ActionNode(ActionFunction action)
    {
        _action = action;
    }

    public override void Action()
    {
        _action();
    }
}
