using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public Action entry { get; set; }
    public Action exit { get; set; }
    public Func<object> during { get; set; }
    LinkedList<Transition> transitions { get; set; }

    public Func<bool> CreateBoundTimeoutFunction(float duration)
    {
        float start = 0;
        entry += () => start = Time.time;
        return () => Time.time - start > duration;
    }

    public State(Action entry, Func<object> during, Action exit)
    {
        this.entry = entry;
        this.exit = exit;
        this.during = during;
        this.transitions = new();
    }

    private bool TryGetTransition(out Transition transition)
    {
        transition = null;
        foreach (var x in transitions)
        {
            if (x.condition())
            {
                transition = x;
                return true;
            }
        }
        return false;
    }

    public void After(float duration, State state) => After(duration, state, (_) => { });
    public void After(float duration, State state, Action<Machine> action)
    {
        When(
            CreateBoundTimeoutFunction(duration),
            state,
            action
        );
    }

    public void AfterOrWhen(float duration, Func<bool> condition, State state) => AfterOrWhen(duration, condition, state, (_) => { });
    public void AfterOrWhen(float duration, Func<bool> condition, State state, Action<Machine> action)
    {
        var timeoutfunc = CreateBoundTimeoutFunction(duration);
        When(
            () => condition() || timeoutfunc(),
            state,
            action
        );
    }

    public void AfterAndWhen(float duration, Func<bool> condition, State state) => AfterAndWhen(duration, condition, state, (_) => { });
    public void AfterAndWhen(float duration, Func<bool> condition, State state, Action<Machine> action)
    {
        var timeoutfunc = CreateBoundTimeoutFunction(duration);
        When(
            () => condition() && timeoutfunc(),
            state,
            action
        );
    }

    // When {condition} is fulfilled execute {action} and goto {state}
    public void When(Func<bool> condition, State state) => When(condition, state, (_) => { });
    public void When(Func<bool> condition, State state, Action<Machine> action = default!)
    {
        transitions.AddLast(new Transition(action, condition, state));
    }

    public void ExitWhen(Func<bool> condition) => When(condition, null);
    public void ExitWhen(Func<bool> condition, Action<Machine> action) => When(condition, null, action);

#nullable enable
    // #pragma warning disable CS8618
    public class Transition
    {
        public Action<Machine> action { get; private set; }
        public Func<bool> condition { get; private set; }
        public State? to { get; private set; }

        public Transition(Action<Machine> action, Func<bool> condition, State? state)
        {
            this.condition = condition;
            this.to = state;
            this.action = action;
        }
    }
#nullable disable

    public abstract class Machine
    {
        private Coroutine _routine;
        private State _current;

        public abstract MonoBehaviour runner { get; }
        public State current => _current;
        public bool isInProgress => current != null;

        protected void Run(State state)
        {
            Abort();
            _current = state;
            _routine = runner.StartCoroutine(AsRoutine());
        }

        protected void Abort()
        {
            if (current != null) current.exit();
            if (_routine != null)
            {
                var r = _routine;
                _current = null;
                _routine = null;
                runner.StopCoroutine(r);
            }
        }

        private IEnumerator AsRoutine()
        {
            Transition next = null;

            for (; ; )
            {
                // Current should never be null at this point
                // Either this function has just been called from {Run} in which case current is set by the caller
                // Otherwise this is a iteration of the for loop, in which case null was checked at the end.
                current.entry();
                while (!current.TryGetTransition(out next))
                {
                    yield return current.during();
                }

                next.action(this);

                if (next.to == null)
                {
                    Abort();
                    yield break;
                }
                else
                {
                    current.exit();
                    _current = next.to;
                }
            }
        }
    }
}

