using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StateLockableButton : Button
{
    public List<StateLock> stateLocks = new();
    private SelectionState buttonState = SelectionState.Normal;
    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        foreach (StateLock stateLock in stateLocks)
        {
            if (stateLock.lockType == StateLock.LockType.everything) { return; }
            if (stateLock.lockType == StateLock.LockType.cantEnter && stateLock.stateName == state.ToString()) { return; }
            if (stateLock.lockType == StateLock.LockType.cantExit && stateLock.stateName == buttonState.ToString()) { return; }
        }

        buttonState = state;

        base.DoStateTransition(state, instant);
    }

    public void TransitionToNormalState()
    {
        DoStateTransition(SelectionState.Normal, true);
    }

    public void TransitionToSelectedState()
    {
        DoStateTransition(SelectionState.Selected, true);
    }

    public class StateLock
    {
        public LockType lockType;
        public string stateName;
        public StateLock(LockType lockType, string stateName)
        {
            this.lockType = lockType;
            this.stateName = stateName;
        }

        public enum LockType
        {
            nothing, everything, cantEnter, cantExit
        }
    }
}
