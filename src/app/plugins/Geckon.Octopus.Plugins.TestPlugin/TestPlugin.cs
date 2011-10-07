using System;
using Geckon.Octopus.Plugin.Interface;
using Geckon.Octopus.Plugins.BasePlugins;
using Geckon.Serialization.Xml;

namespace Geckon.Octopus.Plugins.TestPlugin
{
    public class TestPlugin : APluginExtended, IPlugin //TODO: Find out why IPlugin must be implemented here, when it already is in base class.
    {
        #region Fields

    	private const int UPDATE_PROGRESS_INTERVAL = 30;

    	private bool _EndCurrentOperation = false;

		private readonly PluginPropertyValueType<uint> _ExecuteDuration;
		private readonly PluginPropertyValueType<uint> _CommitDuration;
		private readonly PluginPropertyValueType<uint> _RollbackDuration;

		private readonly PluginPropertyValueType<bool> _ShouldExecuteFail;
		private readonly PluginPropertyValueType<bool> _ShouldCommitFail;
		private readonly PluginPropertyValueType<bool> _ShouldRollbackFail;

        #endregion
		#region Properties

		[Element("ExecuteDuration")]
		public uint ExecuteDuration
		{
			get { return _ExecuteDuration.Value; }
			set { _ExecuteDuration.SetValueIfPropertiesAreEditable(value); }
		}

		[Element("CommitDuration")]
		public uint CommitDuration
		{
			get { return _CommitDuration.Value; }
			set { _CommitDuration.SetValueIfPropertiesAreEditable(value); }
		}

		[Element("RollbackDuration")]
		public uint RollbackDuration
		{
			get { return _RollbackDuration.Value; }
			set { _RollbackDuration.SetValueIfPropertiesAreEditable(value); }
		}

		[Element("ShouldExecuteFail")]
		public bool ShouldExecuteFail
		{
			get { return _ShouldExecuteFail.Value; }
			set { _ShouldExecuteFail.SetValueIfPropertiesAreEditable(value); }
		}

		[Element("ShouldCommitFail")]
		public bool ShouldCommitFail
		{
			get { return _ShouldCommitFail.Value; }
			set { _ShouldCommitFail.SetValueIfPropertiesAreEditable(value); }
		}

		[Element("ShouldRollbackFail")]
		public bool ShouldRollbackFail
		{
			get { return _ShouldRollbackFail.Value; }
			set { _ShouldRollbackFail.SetValueIfPropertiesAreEditable(value); }
		}

		#endregion
		#region Constructors

		public TestPlugin() :
			this( 0, 0, 0, false, false, false)
		{
		}

		public TestPlugin(uint executeDuration, uint commitDuration, uint rollbackDuration) :
			this(executeDuration, commitDuration, rollbackDuration, false, false, false)
		{
		}

		public TestPlugin(bool shouldExecuteFail, bool shouldCommitFail, bool shouldRollbackFail) :
			this(0, 0, 0, shouldExecuteFail, shouldCommitFail, shouldRollbackFail)
		{
		}

		public TestPlugin(uint operationsDelay) :
			this( operationsDelay, operationsDelay, operationsDelay, false, false, false)
		{
		}

		public TestPlugin(bool shouldOperationsFail) :
			this(0, 0, 0, shouldOperationsFail, shouldOperationsFail, shouldOperationsFail)
		{
		}

		public TestPlugin(uint operationsDelay, bool shouldOperationsFail) :
			this(operationsDelay, operationsDelay, operationsDelay, shouldOperationsFail, shouldOperationsFail, shouldOperationsFail)
		{
		}

		public TestPlugin(uint executeDuration, uint commitDuration, uint rollbackDuration, bool shouldExecuteFail, bool shouldCommitFail, bool shouldRollbackFail)
		{
			_ExecuteDuration = new PluginPropertyValueType<uint>("ExecuteDuration", this, executeDuration);
			_CommitDuration = new PluginPropertyValueType<uint>("CommitDuration", this, commitDuration);
			_RollbackDuration = new PluginPropertyValueType<uint>("RollbackDuration", this, rollbackDuration);

			_ShouldExecuteFail = new PluginPropertyValueType<bool>("ShouldExecuteFail", this, shouldExecuteFail);
			_ShouldCommitFail = new PluginPropertyValueType<bool>("ShouldCommitFail", this, shouldCommitFail);
			_ShouldRollbackFail = new PluginPropertyValueType<bool>("ShouldRollbackFail", this, shouldRollbackFail);
		}

    	#endregion
        #region Business Logic

        protected override void Execute()
        {
        	DelayAndUpdateProgress(ExecuteDuration, ShouldExecuteFail);
			
            base.Execute();
        }

    	protected override void Commit()
        {
			DelayAndUpdateProgress(CommitDuration, ShouldCommitFail);

			base.Execute();
        }

        protected override void Rollback()
        {
			DelayAndUpdateProgress(RollbackDuration, ShouldRollbackFail);

			base.Execute();
        }

		public void EndCurrentOperation()
		{
			_EndCurrentOperation = true;
		}

		private void DelayAndUpdateProgress(uint duration, bool shouldFail)
		{
			if(duration == 0)
			{
				OperationProgress = 0.5;
			}
			else
			{
				var startTime = DateTime.Now.AddMilliseconds(duration);

				_EndCurrentOperation = false;

				while (startTime.CompareTo(DateTime.Now) > 0 && !_EndCurrentOperation)
				{
					System.Threading.Thread.Sleep(Math.Min(UPDATE_PROGRESS_INTERVAL, (int)startTime.Subtract(DateTime.Now).TotalMilliseconds));

					OperationProgress = (duration - startTime.Subtract(DateTime.Now).TotalMilliseconds) / duration;
				}
			}
			
			if(shouldFail)
				throw new Exception("Test plugin set to fail");
		}

        #endregion
		#region Misc

		protected string GetState()
		{
			return "Status: " + Status +
				   " ExecuteDuration: " + ExecuteDuration + " CommitDuration: " + CommitDuration +
				   " RollbackDuration: " + RollbackDuration + " ShouldExecuteFail: " + ShouldExecuteFail +
				   " ShouldCommitFail: " + ShouldCommitFail + " ShouldRollbackFail: " + ShouldRollbackFail;
		}

		public override string ToString()
		{
			return "TestPlugin " + GetState();
		}

		#endregion
	}
}