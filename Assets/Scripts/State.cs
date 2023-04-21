using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

public class State
{
    public Action entry { get; set; }
    public Action exit { get; set; }
    public Func<object> during { get; set; }
    LinkedList<Transition> transitions { get; set; }

    public State(Action entry, Action exit, Func<object> during)
    {
        this.entry = entry;
        this.exit = exit;
        this.during = during;
        this.transitions = new();
    }

    private Transition? TryGetTransition()
    {
        foreach (var transition in transitions)
        {
            if (transition.condition())
            {
                return transition;
            }
        }
        return null;
    }

    public void After(float duration, State state)
    {
        float start = 0;
        entry += () => start = Time.time;
        AddTransition(() => {
            
            return Time.time - start < duration;
        },
        state);
    }

    public void AddTransition(Func<bool> condition, State state)
    {
        AddTransition(condition, (state) => { }, state);
    }

    public void AddTransition(Func<bool> condition, Action<State> action, State state)
    {
        transitions.AddLast(new Transition(action, condition, state));
    }


    class Transition
    {
        public Action<State> action { get; set; }
        public Func<bool> condition { get; set; }
        public State? to { get; set; }

        public Transition(Action<State> action, Func<bool> condition, State state)
        {
            this.action = action;
            this.condition = condition;
            this.to = state;
        }
    }

    public class Machine
    {
        private MonoBehaviour runner;
        private Coroutine? routine;

        public State? current { get; private set; }
        public bool running => current != null;

        public Machine(MonoBehaviour runner)
        {
            this.runner = runner;
            this.routine = null;
            this.current = null;
        }

        public void Run(State state)
        {
            Abort();
            current = state;
            routine = runner.StartCoroutine(AsRoutine());
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
            Transition? next = null;

            for (; ; )
            {
                // Current should never be null at this point
                // Either this function has just been called from {Run} in which case current is set by the caller
                // Otherwise this is a iteration of the for loop, in which case null was checked at the end.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                current.entry();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                for (; ; )
                {
                    next = current.TryGetTransition();
                    if (next != null) break;

                    yield return current.during();
                }

                next.action(current);

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
#nullable disable

