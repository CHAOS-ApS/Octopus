using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Geckon.Octopus.Controller.Interface;
using Geckon.Octopus.Plugin.Core;
using Geckon.Octopus.Plugin.Interface;
using Geckon.Serialization.Xml;

namespace Geckon.Octopus.Controller.Core
{
    [Document("Step")]
    public class Step : APluginContainer, IStep
    {
		#region Properties

    	public IStepContent this[int index]
    	{
			get { return PluginTrackables[index] as IStepContent; }
			set
			{
				PluginTrackables[index] = value;
			}
    	}
			
        #endregion
        #region Construction

        public Step( )
        {
           
        }

		public Step(XmlNode jobXML) : this()
		{
			AddPluginTrackables(jobXML);
		}

    	private void AddPluginTrackables(XmlNode jobXML)
		{
			foreach (XmlNode subNode in jobXML.ChildNodes)
			{
				switch (subNode.Name)
				{
					case "Flow":
						AddFlow(subNode);
						break;
					case "Plugin":
						AddPlugin(subNode);
						break;
					default:
						throw new NotImplementedException("Not implemented for " + subNode.Name);
				}
			}
		}

		private void AddFlow(XmlNode flowXml)
		{
			if (flowXml.ChildNodes.Count == 0)
				return;

			PluginTrackables.Add(new Flow(flowXml));
		}

		private void AddPlugin(XmlNode pluginXML)
		{
			IPlugin plugin = PluginLoader.GetPlugin<APlugin>(pluginXML.Attributes["Version"].InnerText, pluginXML.Attributes["Class"].InnerText);

			ApplyPreset(pluginXML, plugin);

			XmlSerialize.FromXML(pluginXML, plugin);

			PluginTrackables.Add(plugin);

			// TODO: Implement better error handling here
		}

		#region Preset

		private static void ApplyPreset(XmlNode node, IPlugin plugin)
		{
			if (node.Attributes["PresetID"] == null)
				return;

			var stringID = node.Attributes["PresetID"].InnerText;

			if (string.IsNullOrEmpty(stringID))
				return;

			int id;

			if( !int.TryParse( stringID, out id ) )
				throw new Exception( "Could not parse Preset ID on: " + node );

			var presetXML = GetPreset(id);

			XmlSerialize.FromXML(presetXML, plugin);
		}

		private static XmlNode GetPreset(int id)
		{
			throw new NotImplementedException(); //TODO: Write code here.
		}

		#endregion
		#endregion
		#region Business Logic

    	public override IEnumerable<IPlugin> GetRunablePlugins(JobCommand command)
    	{
    		if (command != JobCommand.Execute && command != JobCommand.Commit && command != JobCommand.Rollback)
    			yield break;

			foreach (IStepContent pluginTrackable in PluginTrackables)
			{
				if (pluginTrackable is IPlugin)
				{
					if (IsPluginRunnable(command, (IPlugin) pluginTrackable))
						yield return (IPlugin) pluginTrackable;
				}
				else if (pluginTrackable is IFlow)
				{
					foreach (IPlugin plugin in ((IFlow) pluginTrackable).GetRunablePlugins(command))
					{
						yield return plugin;
					}
				}
				else
					throw new NotImplementedException("Not implemented for " + pluginTrackable.GetType().FullName);
			}
    	}

    	private static bool IsPluginRunnable(JobCommand command, IPlugin plugin)
		{
			switch (command)
			{
				case JobCommand.Execute:
					return plugin.Status == PluginStatus.Initialized;
				case JobCommand.Commit:
					return plugin.Status == PluginStatus.Executed;
				case JobCommand.Rollback:
					return plugin.Status == PluginStatus.Executed || plugin.Status == PluginStatus.ExecuteFailed;
				default:
					return false;
			}
		}

        #endregion
		#region Implementation of IList<IStepContent>

		public IEnumerator<IStepContent> GetEnumerator()
		{
			foreach (IStepContent stepContent in PluginTrackables)
				yield return stepContent;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public bool Contains(IStepContent item)
		{
			return PluginTrackables.Contains(item);
		}

		public int IndexOf(IStepContent item)
		{
			return PluginTrackables.IndexOf(item);
		}

		public void CopyTo(IStepContent[] array, int arrayIndex)
		{
			PluginTrackables.CopyTo(array, arrayIndex);
		}

		public void Add(IStepContent item)
		{
			PluginTrackables.Add(item);
		}

		public void Insert(int index, IStepContent item)
		{
			PluginTrackables.Insert(index, item);
		}

		public bool Remove(IStepContent item)
		{
			return PluginTrackables.Remove(item);
		}

		public void RemoveAt(int index)
		{
			PluginTrackables.RemoveAt(index);
		}

		public void Clear()
		{
			PluginTrackables.Clear();
		}

    	#endregion
    }
}