namespace Geckon.Octopus.Plugins.Transcoding.Carbon
{
	internal class CarbonJobStatus
	{
		#region Fields

		private readonly string _JobID;
		private readonly double _Progress;
		private readonly bool _HasFailed;

		#endregion

		#region Properties

		public string JobId
		{
			get { return _JobID; }
		}

		public double Progress
		{
			get { return _Progress; }
		}

		public bool HasFailed
		{
			get { return _HasFailed; }
		}

		public bool IsCompleted
		{
			get { return _Progress == 1; }
		}

		public bool HasEnded
		{
			get { return IsCompleted || HasFailed; }
		}

		#endregion

		#region Constructors

		public CarbonJobStatus(string jobID, double progress, bool hasFailed)
		{
			_JobID = jobID;
			_Progress = progress;
			_HasFailed = hasFailed;
		}

		#endregion
	}
}