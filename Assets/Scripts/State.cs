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

    public State(Action entry, Action exit, Func<object> during)
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
            () =>
{
    bool cond = condition();
    bool timeout = timeoutfunc();
    Debug.LogFormat(
        "cond: {0}\n" +
        "timeout: {1}"
        , cond
        , timeout
    );

    return cond && timeout;
},
            state,
            action
        );
    }

    // When {condition} is fufilled execure {action} and goto {state}
    public void When(Func<bool> condition, State state) => When(condition, state, (_) => { });
    public void When(Func<bool> condition, State state, Action<Machine> action = default!)
    {
        transitions.AddLast(new Transition(action, condition, state));
    }

    private void TransitionTo(State state, Action<Machine> action)
    {
        Transition transition = null;
        transition = new Transition((machine) =>
        {
            Debug.LogFormat("Transition: {0}\nWas Null: {1}", transition, transition == null);
            machine.current.transitions.RemoveFirst();
            action(machine);
        },
            () => true,
            state
        );
        transitions.AddFirst(transition);
    }

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

    public class Machine
    {
        private MonoBehaviour runner;
        private Coroutine routine;

        public State current { get; private set; }
        public bool running => current != null;

        public Machine(MonoBehaviour runner)
        {
            this.runner = runner;
            this.routine = null;
        }

        public void Run(State state)
        {
            Abort();
            current = state;
            routine = runner.StartCoroutine(AsRoutine());
        }

        public void TransitionTo(State state) => TransitionTo(state, (_) => { });
        public void TransitionTo(State state, Action<Machine> action)
        {
            if (running)
            {
                current.TransitionTo(state, action);
            }
        }

        public void Abort()
        {
            if (current != null) current.exit();
            if (routine != null)
            {
                var r = routine;
                current = null;
                routine = null;
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
                    current = next.to;
                }
            }
        }
    }
}

