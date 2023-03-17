using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WW.Waiters
{
	public class TimeWaiter : Waiter
	{
		private float _timeToWait;

		private float _remainingTime;

		public TimeWaiter(float inTimeToWait, Action inOnCompleteAction)
		{
			_timeToWait = _remainingTime = inTimeToWait;

			_completionCheck = () =>
			{
				return _remainingTime <= 0;
			};

			_action = inOnCompleteAction;
		}

		protected override void RestartForLoop()
		{
			_remainingTime = _timeToWait;

			base.RestartForLoop();
		}

		protected override bool CheckForCompletion(float inDeltaTime)
		{
			_remainingTime -= inDeltaTime;
			return base.CheckForCompletion(inDeltaTime);
		}
	}
}
