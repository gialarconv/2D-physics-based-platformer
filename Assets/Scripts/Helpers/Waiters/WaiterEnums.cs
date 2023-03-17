namespace WW.Waiters
{
	public enum RoutineState
	{
		Running,
		Completed,
		Cancelled,
		Failed
	}

	public enum UpdateType
	{
		Normal,
		Fixed,
		Late,
		TimescaleIndependent
	}
}
