using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = System.Object;

namespace WW.Waiters
{
	public class Waiter
	{
		protected Action _action;

		protected Func<bool> _completionCheck;

		private RoutineState _routineState;
		public RoutineState state { get { return _routineState; } }

		private UpdateType _updateType;
		public UpdateType updateType { get { return _updateType; } }

		private int _remainingLoops = 0;

		public bool IsRunning { get { return _routineState == RoutineState.Running; } }

		private Object _id = null;
		public Object ID { get { return _id; } }

		public Waiter()
		{
			Init();
		}

		public Waiter(Func<bool> inCompletionCheck, Action inOnCompleteAction)
		{
			_completionCheck = inCompletionCheck;
			_action = inOnCompleteAction;

			Init();
		}

		protected void Init()
		{
			_routineState = RoutineState.Running;
			_remainingLoops = 0;
		}

		/// <summary>
		/// sets the number of times you want the waiter to loop
		/// </summary>
		/// <param name="inLoopCount">-1 for infinite loops</param>
		/// <returns></returns>
		public Waiter SetLoops(int inLoopCount)
		{
			if(inLoopCount > 0)
				inLoopCount--;

			_remainingLoops = inLoopCount;
			return this;
		}

		/// <summary>
		/// sets an altenrate update type for the waiter
		/// </summary>
		/// <param name="inUpdateType">Update type you want to use for this waiter</param>
		/// <returns></returns>
		public Waiter SetUpdateType(UpdateType inUpdateType)
		{
			_updateType = inUpdateType;
			return this;
		}

		/// <summary>
		/// sets the identifier for this waiter. Waiters can be found and cancelled by identifier. Identifer does not need to be unique.
		/// </summary>
		/// <param name="inID">object to use as an identifier. Can be a string or a gameobject etc.</param>
		/// <returns></returns>
		public Waiter SetID(Object inID)
		{
			_id = inID;
			return this;
		}

		/// <summary>
		/// Stops the waiter from waiting and will not execute it's Completion Action
		/// </summary>
		public void Cancel()
		{
			_routineState = RoutineState.Cancelled;
		}

		/// <summary>
		/// Update the waiter. Should be called by the WaitController unless the waiter was created independently.
		/// </summary>
		/// <param name="inDeltaTime">time elapsed since last update</param>
		public void Update(float inDeltaTime)
		{
			if(_completionCheck == null || _action == null)
			{
				Debug.LogError("Simple Routine has null delegates and cannot Update!!!");
				return;
			}

			if(_routineState != RoutineState.Running)
			{
				return;
			}

			try
			{
				if(CheckForCompletion(inDeltaTime))
				{
					_action.Invoke();
					_routineState = RoutineState.Completed;

					if(_remainingLoops != 0)
					{
						RestartForLoop();

						if(_remainingLoops > 0)
							_remainingLoops--;
					}
				}
			}
			catch(Exception e)
			{
				Debug.Log(string.Format("Encountered error in Waiter! stopping waiter {0}", e.ToString()));
				_routineState = RoutineState.Failed;
			}
		}

		protected virtual void RestartForLoop()
		{
			_routineState = RoutineState.Running;
		}

		protected virtual bool CheckForCompletion(float inDeltaTime)
		{
			return _completionCheck.Invoke();
		}
	}
}
