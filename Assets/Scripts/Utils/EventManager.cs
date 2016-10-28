using System.Linq;


public class EventManager
{
	public interface IEvent
	{
	}

	public delegate void EventDelegate<T>(T e) where T : IEvent;
	
	private class EventMethodListener<T> where T : IEvent
	{
		public static EventDelegate<T> method;
	}

	public static void Attach<T>(EventDelegate<T> method) where T : IEvent
	{
		EventMethodListener<T>.method += method;
	}

	public static void Detach<T>(EventDelegate<T> method) where T : IEvent
	{
		EventMethodListener<T>.method -= method;
	}

	public static bool IsAttached<T>(EventDelegate<T> method) where T : IEvent
	{
		return EventMethodListener<T>.method.GetInvocationList ().Contains (method);
	}

	public static void Send<T>(T evnt) where T : IEvent
	{
		if (EventMethodListener<T>.method != null)
			EventMethodListener<T>.method(evnt);
	}
}

