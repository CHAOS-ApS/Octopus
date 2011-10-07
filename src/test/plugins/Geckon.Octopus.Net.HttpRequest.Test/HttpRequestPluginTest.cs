using System;
using System.Net;
using Geckon.Events;
using Geckon.Octopus.Plugin.Interface;
using Geckon.Octopus.Plugins.Net.HttpRequest;
using NUnit.Framework;

namespace Geckon.Octopus.Net.HttpRequest.Test
{
    [TestFixture]
    public class HttpRequestPluginTest
    {
        [Test]
        public void Should_Send_Http_GET_Request()
        {
            HttpRequestPlugin plugin = new HttpRequestPlugin();

            plugin.Execute_URL  = "http://web.server00.geckon.com/portal/api/portalservice.svc/Session_Start";
            plugin.ExecuteData = "?clientSettingID=1&repositoryID=1";

            plugin.ExecuteFailed += delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

            plugin.BeginExecute();
        }

        [Test]
        public void Should_Send_Http_GET_Request_Rollback()
        {
            HttpRequestPlugin plugin = new HttpRequestPlugin();

            plugin.Rollback_URL = "http://web.server00.geckon.com/portal/api/portalservice.svc/Session_Start";
            plugin.RollbackData = "?clientSettingID=1&repositoryID=1";

            plugin.RollbackFailed += delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

            plugin.BeginRollback();
        }

        [Test, ExpectedException(typeof(WebException))]
        public void Should_Fail_On_Invalid_Request()
        {
            HttpRequestPlugin plugin = new HttpRequestPlugin();

            plugin.Execute_URL = "http://web.server00.geckon.com/portal/api/portalservice.svc/INVALID";
            plugin.ExecuteData = "";

            plugin.ExecuteFailed += delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

            plugin.BeginExecute();
        }

        [Test]
        public void Should_Combine_URL_And_URLEncode_Data()
        {
            HttpRequestPlugin plugin = new HttpRequestPlugin();

            plugin.Execute_URL = "http://web.server00.geckon.com/portal/api/portalservice.svc/Session_Start";
            plugin.ExecuteData = @"clientSettingID=1&repositoryID=\\gwrg\\\:";

            Assert.AreEqual("clientSettingID=1&repositoryID=%5c%5cgwrg%5c%5c%5c%3a&", plugin.ExecuteData);
        }

        [Test]
        public void Should_Combine_URL_And_Data1()
        {
            HttpRequestPlugin plugin = new HttpRequestPlugin();

            plugin.Execute_URL = "http://web.server00.geckon.com/portal/api/portalservice.svc/Session_Start";
            plugin.ExecuteData = "clientSettingID=1&repositoryID=1";

            plugin.ExecuteFailed += delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

            plugin.BeginExecute();
        }

        [Test]
        public void Should_Combine_URL_And_Data2()
        {
            HttpRequestPlugin plugin = new HttpRequestPlugin();

            plugin.Execute_URL = "http://web.server00.geckon.com/portal/api/portalservice.svc/Session_Start?";
            plugin.ExecuteData = "?clientSettingID=1&repositoryID=1";

            plugin.ExecuteFailed += delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

            plugin.BeginExecute();
        }

        [Test,ExpectedException(typeof(NullReferenceException))]
        public void Should_Fail_When_URL_Is_NULL()
        {
            HttpRequestPlugin plugin = new HttpRequestPlugin();

            plugin.Execute_URL = null;
            plugin.ExecuteData = "?clientSettingID=1&repositoryID=1";

            plugin.ExecuteFailed += delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

            plugin.BeginExecute();
        }

        [Test, ExpectedException(typeof(WebException))]
        public void Should_Send_If_Data_Is_NULL()
        {
            HttpRequestPlugin plugin = new HttpRequestPlugin();

            plugin.Execute_URL = "http://web.server00.geckon.com/portal/api/portalservice.svc/Session_Start";
            plugin.ExecuteData = null;

            plugin.ExecuteFailed += delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

            plugin.BeginExecute();
        }
    }
}
