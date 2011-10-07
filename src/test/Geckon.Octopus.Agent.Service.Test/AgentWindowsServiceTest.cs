using NUnit.Framework;

namespace Geckon.Octopus.Agent.Service.Test
{
    [TestFixture]
    public class AgentWindowsServiceTest : AgentWindowsService
    {
        [Test]
        public void Should_Start_Service()
        {
            OnStart( null );

            Assert.IsNotNull( AgentEngine );
            Assert.IsNotNull( ServiceHost );
        }

    }
}
