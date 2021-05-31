using System;
using System.Collections.Generic;

namespace Robot.Utility
{
	/// <summary>
	/// The ServiceLocator is the only allowed Singleton in the game.
	/// It manages all other globally accessible objects by making them register themself by object type.
	/// </summary>
	public class ServiceLocator
	{
		// Implementation of singleton pattern.
		public static ServiceLocator Instance
		{
			get;
			private set;
		}

		// Dictionary of all registered objects.
		private Dictionary<Type, Object> objects = new Dictionary<Type, Object>();

		public static void Initialize(ServiceLocator instance)
		{
			// Implementation of singleton pattern.
			if (Instance != null)
				throw new Exception("ServiceLocator was already initialized");

			Instance = instance;
		}

		/// <summary>
		/// Register an object on the ServiceLocator.
		/// </summary>
		/// <param name="obj">Object to register under.</param>
		public static void Register<T>(T obj) where T : class
		{
			Instance.RegisterInternal<T>(obj);
		}

		private void RegisterInternal<T>(T obj) where T : class
		{
			Type type = typeof(T);

			if (objects.ContainsKey(type))
				throw new Exception("Object was already registered");

			objects[type] = obj;
		}

		/// <summary>
		/// Unregister an object from the ServiceLocator.
		/// </summary>
		/// <param name="obj">Object to unregister under.</param>
		public static void Unregister<T>(T obj) where T : class
		{
			Instance.UnregisterInternal<T>(obj);
		}

		private void UnregisterInternal<T>(T obj) where T : class
		{
			Type type = typeof(T);

			if (!objects.ContainsKey(type))
				throw new Exception("Object was already unregistered");

			objects[type] = null;
		}

		/// <summary>
		/// Gets an object of type T.
		/// </summary>
		/// <typeparam name="T">Type of object to get.</typeparam>
		/// <returns>A registered object with type T.</returns>
		public static T Get<T>() where T : class
		{
			return Instance.GetInternal<T>();
		}

		private T GetInternal<T>() where T : class
		{
			objects.TryGetValue(typeof(T), out Object obj);
			return obj as T;
		}

		/// <summary>
		/// Tries to get an object of type T.
		/// </summary>
		/// <typeparam name="T">Type of object to get.</typeparam>
		/// <param name="obj">Reference to object to populate.</param>
		/// <returns>True if object was found, false otherwise.</returns>
		public static bool TryGet<T>(T obj) where T : class
		{
			return Instance.TryGetInternal<T>(obj);		
		}

		private bool TryGetInternal<T>(Object obj) where T : class
		{
			return objects.TryGetValue(typeof(T), out obj);
		}
	}
}