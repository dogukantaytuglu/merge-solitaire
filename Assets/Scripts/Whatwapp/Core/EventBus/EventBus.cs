using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus<T> where T : IEvent 
{
    private static readonly HashSet<IEventBinding<T>> Bindings = new();
    private static readonly HashSet<IEventBinding<T>> BindingsToAdd = new();
    private static readonly HashSet<IEventBinding<T>> BindingsToClean = new();

    public static void Register(Action<T> action)
    {
        foreach (var eventBinding in BindingsToClean)
        {
            if (eventBinding.OnEvent == action)
            {
                BindingsToClean.Remove(eventBinding);
                return;
            }
        }
        var binding = new EventBinding<T>(action);
        BindingsToAdd.Add(binding);
    }

    public static void Register(Action action)
    {
        foreach (var eventBinding in BindingsToClean)
        {
            if (eventBinding.OnEventNoArgs == action)
            {
                BindingsToClean.Remove(eventBinding);
                return;
            }
        }
        
        var binding = new EventBinding<T>(action);
        BindingsToAdd.Add(binding);
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public static void Deregister(Action<T> action)
    {
        if (TryFindEventBinding(action, out var eventBinding))
        {
            BindingsToClean.Add(eventBinding);
        }
        
        else
        {
            Debug.LogError($"{action.Method}/{action.Target} is trying to deregister but it was not registered!");
        }
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public static void Deregister(Action action)
    {
        if (TryFindEventBinding(action, out var eventBinding))
        {
            BindingsToClean.Add(eventBinding);
        }

        else
        {
            Debug.LogError($"{action} is trying to deregister but it was not registered!");
        }
    }

    private static bool TryFindEventBinding(Action<T> action, out IEventBinding<T> eventBinding)
    {
        eventBinding = null;
        foreach (var binding in Bindings)
        {
            if (binding.OnEvent == action)
            {
                eventBinding = binding;
                return true;
            }
        }

        foreach (var binding in BindingsToAdd)
        {
            if (binding.OnEvent == action)
            {
                eventBinding = binding;
                return true;
            }
        }

        return false;
    }
    
    private static bool TryFindEventBinding(Action action, out IEventBinding<T> eventBinding)
    {
        eventBinding = null;
        foreach (var binding in Bindings)
        {
            if (binding.OnEventNoArgs == action)
            {
                eventBinding = binding;
                return true;
            }
        }
        
        foreach (var binding in BindingsToAdd)
        {
            if (binding.OnEventNoArgs == action)
            {
                eventBinding = binding;
                return true;
            }
        }

        return false;
    }

    public static void Raise(T @event)
    {
        CleanBindings();
        AddBindings();

        foreach (var binding in Bindings) 
        {
            binding.OnEvent.Invoke(@event);
            binding.OnEventNoArgs.Invoke();
        }
    }

    private static void CleanBindings()
    {
        foreach (var binding in BindingsToClean)
        {
            if (! Bindings.Remove(binding))
            {
                BindingsToAdd.Remove(binding);
            }
        }

        BindingsToClean.Clear();
    }

    private static void AddBindings()
    {
        foreach (var binding in BindingsToAdd)
        {
            Bindings.Add(binding);
        }
        
        BindingsToAdd.Clear();
    }
}