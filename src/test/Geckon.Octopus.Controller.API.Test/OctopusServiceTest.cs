using System;
using System.Linq;
using Geckon.Octopus.Controller.Interface;
using Geckon.Octopus.Data;
using Geckon.Octopus.TestUtilities;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace Geckon.Octopus.Controller.API.Test
{
    [TestFixture]
    public class OctopusServiceTest : OctopusService
    {
        private static readonly Uri SomeValidXslSheetUri = new Uri("http://www.w3schools.com/xsl/cdcatalog.xsl");

        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            using (DatabaseDataContext db = new DatabaseDataContext())
            {
                db.Test_CleanAndInsertDummyData(
                    Regex.Replace(
                        System.Environment.CurrentDirectory,
                        "(src)(\\\\)(test)(\\\\)[\\w.-]+(\\\\)(bin)(\\\\)(Debug|Release)$",
						"bin\\plugins\\"
                        )
                    );
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (DatabaseDataContext db = new DatabaseDataContext())
                db.Test_Clean();
        }

        #endregion
        #region Test methods

        [Test]
        public void Should_download_style_sheet()
        {
            Console.WriteLine(
                TryDownloadStyleSheet(SomeValidXslSheetUri.ToString())
                );
        }

        [Test, ExpectedException(ExpectedMessage = "Failed to download xslt style sheet from http://www.google.com/yahoo.com")]
        public void Should_not_download_style_sheet_from_invalid_url()
        {
            TryDownloadStyleSheet("http://www.google.com/yahoo.com");
        }

        [Test]
        public void Should_Add_Job_To_Database()
        {
            IJobData job = Job_Create(DTO.JobData.JobXML.ToString());

            using (DatabaseDataContext db = new DatabaseDataContext())
                Assert.AreEqual(
                    db.Job_GetBy(job.ID, null, null, null, true).ToList().Count, 1
                    );
        }

        [Test]
        public void Should_Get_All_Jobs()
        {
            Assert.Greater(Job_Get().Count, 0);
        }

        [Test]
        public void Should_create_job_template()
        {
            JobTemplate_Create(
                "Test_" + Guid.NewGuid(),
                new Uri("http://someuri.com/some.xsl")
                );
        }

        [Test]
        public void Should_create_job()
        {
            Job_Create("<job_xml/>");
        }

        [Test]
        public void Should_create_job_using_template_ID()
        {
            JobTemplate template = JobTemplate_Create(
                "test template " + Guid.NewGuid(),
                SomeValidXslSheetUri
                );

            IJobData job = Job_Create(
                template.ID,
                "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><!-- Edited by XMLSpy® --><catalog>	<cd>		<title>Empire Burlesque</title>		<artist>Bob Dylan</artist>		<country>USA</country>		<company>Columbia</company>		<price>10.90</price>		<year>1985</year>	</cd>	<cd>		<title>Hide your heart</title>		<artist>Bonnie Tyler</artist>		<country>UK</country>		<company>CBS Records</company>		<price>9.90</price>		<year>1988</year>	</cd>	<cd>		<title>Greatest Hits</title>		<artist>Dolly Parton</artist>		<country>USA</country>		<company>RCA</company>		<price>9.90</price>		<year>1982</year>	</cd>	<cd>		<title>Still got the blues</title>		<artist>Gary Moore</artist>		<country>UK</country>		<company>Virgin records</company>		<price>10.20</price>		<year>1990</year>	</cd>	<cd>		<title>Eros</title>		<artist>Eros Ramazzotti</artist>		<country>EU</country>		<company>BMG</company>		<price>9.90</price>		<year>1997</year>	</cd>	<cd>		<title>One night only</title>		<artist>Bee Gees</artist>		<country>UK</country>		<company>Polydor</company>		<price>10.90</price>		<year>1998</year>	</cd>	<cd>		<title>Sylvias Mother</title>		<artist>Dr.Hook</artist>		<country>UK</country>		<company>CBS</company>		<price>8.10</price>		<year>1973</year>	</cd>	<cd>		<title>Maggie May</title>		<artist>Rod Stewart</artist>		<country>UK</country>		<company>Pickwick</company>		<price>8.50</price>		<year>1990</year>	</cd>	<cd>		<title>Romanza</title>		<artist>Andrea Bocelli</artist>		<country>EU</country>		<company>Polydor</company>		<price>10.80</price>		<year>1996</year>	</cd>	<cd>		<title>When a man loves a woman</title>		<artist>Percy Sledge</artist>		<country>USA</country>		<company>Atlantic</company>		<price>8.70</price>		<year>1987</year>	</cd>	<cd>		<title>Black angel</title>		<artist>Savage Rose</artist>		<country>EU</country>		<company>Mega</company>		<price>10.90</price>		<year>1995</year>	</cd>	<cd>		<title>1999 Grammy Nominees</title>		<artist>Many</artist>		<country>USA</country>		<company>Grammy</company>		<price>10.20</price>		<year>1999</year>	</cd>	<cd>		<title>For the good times</title>		<artist>Kenny Rogers</artist>		<country>UK</country>		<company>Mucik Master</company>		<price>8.70</price>		<year>1995</year>	</cd>	<cd>		<title>Big Willie style</title>		<artist>Will Smith</artist>		<country>USA</country>		<company>Columbia</company>		<price>9.90</price>		<year>1997</year>	</cd>	<cd>		<title>Tupelo Honey</title>		<artist>Van Morrison</artist>		<country>UK</country>		<company>Polydor</company>		<price>8.20</price>		<year>1971</year>	</cd>	<cd>		<title>Soulsville</title>		<artist>Jorn Hoel</artist>		<country>Norway</country>		<company>WEA</company>		<price>7.90</price>		<year>1996</year>	</cd>	<cd>		<title>The very best of</title>		<artist>Cat Stevens</artist>		<country>UK</country>		<company>Island</company>		<price>8.90</price>		<year>1990</year>	</cd>	<cd>		<title>Stop</title>		<artist>Sam Brown</artist>		<country>UK</country>		<company>A and M</company>		<price>8.90</price>		<year>1988</year>	</cd>	<cd>		<title>Bridge of Spies</title>		<artist>T`Pau</artist>		<country>UK</country>		<company>Siren</company>		<price>7.90</price>		<year>1987</year>	</cd>	<cd>		<title>Private Dancer</title>		<artist>Tina Turner</artist>		<country>UK</country>		<company>Capitol</company>		<price>8.90</price>		<year>1983</year>	</cd>	<cd>		<title>Midt om natten</title>		<artist>Kim Larsen</artist>		<country>EU</country>		<company>Medley</company>		<price>7.80</price>		<year>1983</year>	</cd>	<cd>		<title>Pavarotti Gala Concert</title>		<artist>Luciano Pavarotti</artist>		<country>UK</country>		<company>DECCA</company>		<price>9.90</price>		<year>1991</year>	</cd>	<cd>		<title>The dock of the bay</title>		<artist>Otis Redding</artist>		<country>USA</country>		<company>Atlantic</company>		<price>7.90</price>		<year>1987</year>	</cd>	<cd>		<title>Picture book</title>		<artist>Simply Red</artist>		<country>EU</country>		<company>Elektra</company>		<price>7.20</price>		<year>1985</year>	</cd>	<cd>		<title>Red</title>		<artist>The Communards</artist>		<country>UK</country>		<company>London</company>		<price>7.80</price>		<year>1987</year>	</cd>	<cd>		<title>Unchain my heart</title>		<artist>Joe Cocker</artist>		<country>USA</country>		<company>EMI</company>		<price>8.20</price>		<year>1987</year>	</cd></catalog>"
                );

            Assert.AreEqual(
                job.JobXML, 
                "<?xml version=\"1.0\" encoding=\"utf-16\"?><html><body><h2>My CD Collection</h2><table border=\"1\"><tr bgcolor=\"#9acd32\"><th>Title</th><th>Artist</th></tr><tr><td>Empire Burlesque</td><td>Bob Dylan</td></tr><tr><td>Hide your heart</td><td>Bonnie Tyler</td></tr><tr><td>Greatest Hits</td><td>Dolly Parton</td></tr><tr><td>Still got the blues</td><td>Gary Moore</td></tr><tr><td>Eros</td><td>Eros Ramazzotti</td></tr><tr><td>One night only</td><td>Bee Gees</td></tr><tr><td>Sylvias Mother</td><td>Dr.Hook</td></tr><tr><td>Maggie May</td><td>Rod Stewart</td></tr><tr><td>Romanza</td><td>Andrea Bocelli</td></tr><tr><td>When a man loves a woman</td><td>Percy Sledge</td></tr><tr><td>Black angel</td><td>Savage Rose</td></tr><tr><td>1999 Grammy Nominees</td><td>Many</td></tr><tr><td>For the good times</td><td>Kenny Rogers</td></tr><tr><td>Big Willie style</td><td>Will Smith</td></tr><tr><td>Tupelo Honey</td><td>Van Morrison</td></tr><tr><td>Soulsville</td><td>Jorn Hoel</td></tr><tr><td>The very best of</td><td>Cat Stevens</td></tr><tr><td>Stop</td><td>Sam Brown</td></tr><tr><td>Bridge of Spies</td><td>T`Pau</td></tr><tr><td>Private Dancer</td><td>Tina Turner</td></tr><tr><td>Midt om natten</td><td>Kim Larsen</td></tr><tr><td>Pavarotti Gala Concert</td><td>Luciano Pavarotti</td></tr><tr><td>The dock of the bay</td><td>Otis Redding</td></tr><tr><td>Picture book</td><td>Simply Red</td></tr><tr><td>Red</td><td>The Communards</td></tr><tr><td>Unchain my heart</td><td>Joe Cocker</td></tr></table></body></html>",
                "Resulting xml not as expected."
                );
        }

        [Test]
        public void Should_get_one_or_more_job_templates()
        {
            Should_create_job_template();

            Assert.Greater(
                JobTemplate_Get(null, null).Count,
                0
                );
        }

        [Test]
        public void Should_add_job_using_template_uri_as_template_xml()
        {
            string templateXml = TryDownloadStyleSheet(SomeValidXslSheetUri.ToString());

            JobTemplate template = JobTemplate_Create(
                "test template " + Guid.NewGuid(),
                templateXml
                );

            IJobData job = Job_Create(
                template.ID,
                "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><!-- Edited by XMLSpy® --><catalog>	<cd>		<title>Empire Burlesque</title>		<artist>Bob Dylan</artist>		<country>USA</country>		<company>Columbia</company>		<price>10.90</price>		<year>1985</year>	</cd>	<cd>		<title>Hide your heart</title>		<artist>Bonnie Tyler</artist>		<country>UK</country>		<company>CBS Records</company>		<price>9.90</price>		<year>1988</year>	</cd>	<cd>		<title>Greatest Hits</title>		<artist>Dolly Parton</artist>		<country>USA</country>		<company>RCA</company>		<price>9.90</price>		<year>1982</year>	</cd>	<cd>		<title>Still got the blues</title>		<artist>Gary Moore</artist>		<country>UK</country>		<company>Virgin records</company>		<price>10.20</price>		<year>1990</year>	</cd>	<cd>		<title>Eros</title>		<artist>Eros Ramazzotti</artist>		<country>EU</country>		<company>BMG</company>		<price>9.90</price>		<year>1997</year>	</cd>	<cd>		<title>One night only</title>		<artist>Bee Gees</artist>		<country>UK</country>		<company>Polydor</company>		<price>10.90</price>		<year>1998</year>	</cd>	<cd>		<title>Sylvias Mother</title>		<artist>Dr.Hook</artist>		<country>UK</country>		<company>CBS</company>		<price>8.10</price>		<year>1973</year>	</cd>	<cd>		<title>Maggie May</title>		<artist>Rod Stewart</artist>		<country>UK</country>		<company>Pickwick</company>		<price>8.50</price>		<year>1990</year>	</cd>	<cd>		<title>Romanza</title>		<artist>Andrea Bocelli</artist>		<country>EU</country>		<company>Polydor</company>		<price>10.80</price>		<year>1996</year>	</cd>	<cd>		<title>When a man loves a woman</title>		<artist>Percy Sledge</artist>		<country>USA</country>		<company>Atlantic</company>		<price>8.70</price>		<year>1987</year>	</cd>	<cd>		<title>Black angel</title>		<artist>Savage Rose</artist>		<country>EU</country>		<company>Mega</company>		<price>10.90</price>		<year>1995</year>	</cd>	<cd>		<title>1999 Grammy Nominees</title>		<artist>Many</artist>		<country>USA</country>		<company>Grammy</company>		<price>10.20</price>		<year>1999</year>	</cd>	<cd>		<title>For the good times</title>		<artist>Kenny Rogers</artist>		<country>UK</country>		<company>Mucik Master</company>		<price>8.70</price>		<year>1995</year>	</cd>	<cd>		<title>Big Willie style</title>		<artist>Will Smith</artist>		<country>USA</country>		<company>Columbia</company>		<price>9.90</price>		<year>1997</year>	</cd>	<cd>		<title>Tupelo Honey</title>		<artist>Van Morrison</artist>		<country>UK</country>		<company>Polydor</company>		<price>8.20</price>		<year>1971</year>	</cd>	<cd>		<title>Soulsville</title>		<artist>Jorn Hoel</artist>		<country>Norway</country>		<company>WEA</company>		<price>7.90</price>		<year>1996</year>	</cd>	<cd>		<title>The very best of</title>		<artist>Cat Stevens</artist>		<country>UK</country>		<company>Island</company>		<price>8.90</price>		<year>1990</year>	</cd>	<cd>		<title>Stop</title>		<artist>Sam Brown</artist>		<country>UK</country>		<company>A and M</company>		<price>8.90</price>		<year>1988</year>	</cd>	<cd>		<title>Bridge of Spies</title>		<artist>T`Pau</artist>		<country>UK</country>		<company>Siren</company>		<price>7.90</price>		<year>1987</year>	</cd>	<cd>		<title>Private Dancer</title>		<artist>Tina Turner</artist>		<country>UK</country>		<company>Capitol</company>		<price>8.90</price>		<year>1983</year>	</cd>	<cd>		<title>Midt om natten</title>		<artist>Kim Larsen</artist>		<country>EU</country>		<company>Medley</company>		<price>7.80</price>		<year>1983</year>	</cd>	<cd>		<title>Pavarotti Gala Concert</title>		<artist>Luciano Pavarotti</artist>		<country>UK</country>		<company>DECCA</company>		<price>9.90</price>		<year>1991</year>	</cd>	<cd>		<title>The dock of the bay</title>		<artist>Otis Redding</artist>		<country>USA</country>		<company>Atlantic</company>		<price>7.90</price>		<year>1987</year>	</cd>	<cd>		<title>Picture book</title>		<artist>Simply Red</artist>		<country>EU</country>		<company>Elektra</company>		<price>7.20</price>		<year>1985</year>	</cd>	<cd>		<title>Red</title>		<artist>The Communards</artist>		<country>UK</country>		<company>London</company>		<price>7.80</price>		<year>1987</year>	</cd>	<cd>		<title>Unchain my heart</title>		<artist>Joe Cocker</artist>		<country>USA</country>		<company>EMI</company>		<price>8.20</price>		<year>1987</year>	</cd></catalog>"
                );

            Assert.AreEqual(
                job.JobXML,
                "<?xml version=\"1.0\" encoding=\"utf-16\"?><html><body><h2>My CD Collection</h2><table border=\"1\"><tr bgcolor=\"#9acd32\"><th>Title</th><th>Artist</th></tr><tr><td>Empire Burlesque</td><td>Bob Dylan</td></tr><tr><td>Hide your heart</td><td>Bonnie Tyler</td></tr><tr><td>Greatest Hits</td><td>Dolly Parton</td></tr><tr><td>Still got the blues</td><td>Gary Moore</td></tr><tr><td>Eros</td><td>Eros Ramazzotti</td></tr><tr><td>One night only</td><td>Bee Gees</td></tr><tr><td>Sylvias Mother</td><td>Dr.Hook</td></tr><tr><td>Maggie May</td><td>Rod Stewart</td></tr><tr><td>Romanza</td><td>Andrea Bocelli</td></tr><tr><td>When a man loves a woman</td><td>Percy Sledge</td></tr><tr><td>Black angel</td><td>Savage Rose</td></tr><tr><td>1999 Grammy Nominees</td><td>Many</td></tr><tr><td>For the good times</td><td>Kenny Rogers</td></tr><tr><td>Big Willie style</td><td>Will Smith</td></tr><tr><td>Tupelo Honey</td><td>Van Morrison</td></tr><tr><td>Soulsville</td><td>Jorn Hoel</td></tr><tr><td>The very best of</td><td>Cat Stevens</td></tr><tr><td>Stop</td><td>Sam Brown</td></tr><tr><td>Bridge of Spies</td><td>T`Pau</td></tr><tr><td>Private Dancer</td><td>Tina Turner</td></tr><tr><td>Midt om natten</td><td>Kim Larsen</td></tr><tr><td>Pavarotti Gala Concert</td><td>Luciano Pavarotti</td></tr><tr><td>The dock of the bay</td><td>Otis Redding</td></tr><tr><td>Picture book</td><td>Simply Red</td></tr><tr><td>Red</td><td>The Communards</td></tr><tr><td>Unchain my heart</td><td>Joe Cocker</td></tr></table></body></html>",
                "Resulting xml not as expected."
                );
        }

        #endregion
    }
}