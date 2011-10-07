namespace Geckon.Octopus.Plugins.TestPlugin
{
	public class TestPlugin2 : TestPlugin
	{
		#region Constructors

		public TestPlugin2() :
			base( 0, 0, 0, false, false, false)
		{
		}

		public TestPlugin2(uint executeDuration, uint commitDuration, uint rollbackDuration) :
			base(executeDuration, commitDuration, rollbackDuration, false, false, false)
		{
		}

		public TestPlugin2(bool shouldExecuteFail, bool shouldCommitFail, bool shouldRollbackFail) :
			base(0, 0, 0, shouldExecuteFail, shouldCommitFail, shouldRollbackFail)
		{
		}

		public TestPlugin2(uint operationsDelay) :
			base( operationsDelay, operationsDelay, operationsDelay, false, false, false)
		{
		}

		public TestPlugin2(bool shouldOperationsFail) :
			base(0, 0, 0, shouldOperationsFail, shouldOperationsFail, shouldOperationsFail)
		{
		}

    	#endregion
		#region Misc

		public override string ToString()
		{
			return "TestPlugin2 " + GetState();
		}

		#endregion
	}
}