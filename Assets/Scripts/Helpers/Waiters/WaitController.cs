using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = System.Object;

namespace WW.Waiters
{
    /// <summary>
    /// In-Scene object responsible for controlling all created watiers and calling their update functions
    /// Can be created with Init() or will be auto-created when needed
    /// 
    /// Documentation at https://docs.google.com/document/d/14BHws_M4ZWoAYKmtnQKQ9PiT37dq2eP94CWMQsLTJtw/edit?usp=sharing
    /// </summary>
    public class WaitController : MonoBehaviour
    {
        private static WaitController _instance;

        private static WaitController Instance
        {
            get
            {
                if (_instance == null)
                {
                    Init();
                }
                return _instance;
            }
        }

        [SerializeField] private List<Waiter> _pendingWaiters = new List<Waiter>();

        private float _timeLastFrame;
        private List<Waiter> _trackedWaiters = new List<Waiter>();

        public List<Waiter> PendingWaiters { get { return _pendingWaiters; } }
        public List<Waiter> TrackedWaiters { get { return _trackedWaiters; } }
        private void Start()
        {
            if (_instance == null)
                _instance = this;

            else if (_instance != this)
                Destroy(gameObject);
        }

        private void Update()
        {
            //update tracked routines
            float dTime = TimeUtils.deltaTime;
            for (int i = 0; i < _trackedWaiters.Count; i++)
            {
                Waiter cWaiter = _trackedWaiters[i];
                if (cWaiter == null)
                    continue;

                if (cWaiter.updateType == UpdateType.Normal || cWaiter.updateType == UpdateType.TimescaleIndependent)
                {
                    cWaiter.Update(cWaiter.updateType == UpdateType.Normal ? dTime : TimeUtils.realtimeSinceStartup - _timeLastFrame);
                }
            }

            CleanupTrackedRoutines();
            _timeLastFrame = TimeUtils.realtimeSinceStartup;
        }

        private void FixedUpdate()
        {
            //update tracked routines
            for (int i = 0; i < _trackedWaiters.Count; i++)
            {
                Waiter cWaiter = _trackedWaiters[i];
                if (cWaiter == null)
                    continue;

                if (cWaiter.updateType == UpdateType.Fixed)
                {
                    cWaiter.Update(Time.fixedDeltaTime);
                }
            }

            CleanupTrackedRoutines();
        }

        private void LateUpdate()
        {
            //update tracked routines
            for (int i = 0; i < _trackedWaiters.Count; i++)
            {
                Waiter cWaiter = _trackedWaiters[i];
                if (cWaiter == null)
                    continue;

                if (cWaiter.updateType == UpdateType.Late)
                {
                    cWaiter.Update(Time.fixedDeltaTime);
                }
            }

            CleanupTrackedRoutines();

            for (int i = 0; i < _pendingWaiters.Count; i++)
            {
                Waiter cWaiter = _pendingWaiters[i];
                if (cWaiter == null)
                    continue;

                _trackedWaiters.Add(cWaiter);
            }
            _pendingWaiters.Clear();
        }
        /// <summary>
        /// Creates WaitController gameobject
        /// </summary>
        public static void Init()
        {
            _instance = new GameObject("[WaitController]", typeof(WaitController)).GetComponent<WaitController>();
            _instance._timeLastFrame = TimeUtils.realtimeSinceStartup;
            DontDestroyOnLoad(_instance);
        }

        /// <summary>
        /// Executes an action after the given condition is met
        /// </summary>
        /// <param name="inCompletionCheck">function that returns true or false depending on if the "condition" is met</param>
        /// <param name="inOnCompleteAction">Action to be invoked once the condition is met</param>
        /// <returns></returns>
        public static Waiter DoAfter(Func<bool> inCompletionCheck, Action inOnCompleteAction)
        {
            Waiter sr = new Waiter(inCompletionCheck, inOnCompleteAction);
            Instance._pendingWaiters.Add(sr);
            return sr;
        }

        /// <summary>
        /// Executes an action after the given time has elapsed
        /// </summary>
        /// <param name="inTimeToWait">amount of time before executing the action</param>
        /// <param name="inOnCompleteAction">Action to be invoked once the time has elapsed</param>
        /// <returns></returns>
        public static TimeWaiter DoAfterWait(float inTimeToWait, Action inOnCompleteAction)
        {
            TimeWaiter tr = new TimeWaiter(inTimeToWait, inOnCompleteAction);
            Instance._pendingWaiters.Add(tr);
            return tr;
        }

        /// <summary>
        /// Executes an action after the given number of frames have elapsed
        /// </summary>
        /// <param name="inFramesToWait">amount of frames before executing the action (should be greater than 0)</param>
        /// <param name="inOnCompleteAction">Action to be invoked once the frames have elapsed</param>
        /// <returns></returns>
        public static FrameWaiter DoAfterFrames(int inFramesToWait, Action inOnCompleteAction)
        {
            FrameWaiter fr = new FrameWaiter(inFramesToWait, inOnCompleteAction);
            Instance._pendingWaiters.Add(fr);
            return fr;
        }

        /// <summary>
        /// finds all tracked and pending Waiters with the given ID
        /// </summary>
        /// <param name="inID"></param>
        /// <returns>List of Waiters</returns>
        public static List<Waiter> GetWaitersWithID(Object inID)
        {
            List<Waiter> foundWaiters = new List<Waiter>();

            for (int i = 0; i < Instance._trackedWaiters.Count; i++)
            {
                Waiter cWaiter = Instance._trackedWaiters[i];
                if (cWaiter == null)
                    continue;

                if ((cWaiter.ID != null && inID != null && cWaiter.ID == inID)
                   || (cWaiter.ID == null && inID == null))
                    foundWaiters.Add(cWaiter);
            }

            for (int i = 0; i < Instance._pendingWaiters.Count; i++)
            {
                Waiter cWaiter = Instance._pendingWaiters[i];
                if (cWaiter == null)
                    continue;

                if ((cWaiter.ID != null && inID != null && cWaiter.ID == inID)
                   || (cWaiter.ID == null && inID == null))
                    foundWaiters.Add(cWaiter);
            }

            return foundWaiters;
        }
        /// <summary>
        /// remove routines from tracking if they are completed
        /// </summary>
        private void CleanupTrackedRoutines()
        {
            for (int i = 0; i < _trackedWaiters.Count; i++)
            {
                Waiter cWaiter = _trackedWaiters[i];
                if (cWaiter == null)
                    continue;

                if (!cWaiter.IsRunning)
                {
                    _trackedWaiters.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}