using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WW.Waiters
{
	public class FrameWaiter : Waiter
	{
		private int _framesToWait;

		private int _remainingFrames;

		public FrameWaiter(int inFramesToWait, Action inOnCompleteAction)
		{
			_framesToWait = _remainingFrames = inFramesToWait;

			_completionCheck = () =>
			{
				return _remainingFrames <= 0;
			};

			_action = inOnCompleteAction;
		}

		protected override void RestartForLoop()
		{
			_remainingFrames = _framesToWait;
			base.RestartForLoop();
		}

		protected override bool CheckForCompletion(float inDeltaTime)
		{
			_remainingFrames--;
			return base.CheckForCompletion(inDeltaTime);
		}
	}
}
