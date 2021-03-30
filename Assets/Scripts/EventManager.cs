using System;

public static class EventManager
{
    public static Action<string> OnImageAdded;
    public static Action<string> OnImageRemoved;
}