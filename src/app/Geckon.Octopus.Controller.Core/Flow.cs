using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Geckon.Octopus.Controller.Interface;
using Geckon.Octopus.Plugin.Interface;
using Geckon.Serialization.Xml;

namespace Geckon.Octopus.Controller.Core
{
    [Document("Flow")]
    public class Flow : APluginContainer, IFlow
    {
        #region Fields

        #endregion
		#region Properties

		public IStep this[int index]
		{
			get { return PluginTrackables[index] as IStep; }
			set { PluginTrackables[index] = value; }
		}

    	#endregion
        #region Construction

        public Flow( )
        {
            
        }

		public Flow(XmlNode jobXml) : this()
		{
			AddPluginTrackables(jobXml);
		}

    	protected void AddPluginTrackables(XmlNode jobXml)
    	{
    		foreach (XmlNode subNode in jobXml.ChildNodes)
    		{
    			switch (subNode.Name)
    			{
    				case "Step":
    					AddStep(subNode);
    					break;
    				default:
    					throw new NotImplementedException("Not implemented for " + subNode.Name);
    			}
    		}
    	}

		private void AddStep(XmlNode stepXml)
    	{
			if (stepXml.ChildNodes.Count == 0)
				return;

			PluginTrackables.Add(new Step(stepXml));
    	}

        #endregion
		#region Business Logic

    	public override IEnumerable<IPlugin> GetRunablePlugins( JobCommand command )
        {
			if (command != JobCommand.Execute && command != JobCommand.Commit && command != JobCommand.Rollback)
    			yield break;

			IEnumerable<IPluginTrackable> steps = command == JobCommand.Rollback ? PluginTrackables.GetReverseEnumerator : PluginTrackables; //Reverse order for Rollback.

			foreach (IStep step in steps)
			{
				bool didEnter = false;

				foreach (IPlugin plugin in step.GetRunablePlugins(command))
				{
					didEnter = true;

					yield return plugin;
				}

				if (didEnter || step.HasRunningPlugins)
					yield break;
			}
        }

    	#endregion
		#region Implementation of IList<IStep>

		public IEnumerator<IStep> GetEnumerator()
		{
			foreach (IStep step in PluginTrackables)
				yield return step;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public bool Contains(IStep item)
		{
			return PluginTrackables.Contains(item);
		}

		public int IndexOf(IStep item)
		{
			return PluginTrackables.IndexOf(item);
		}

		public void CopyTo(IStep[] array, int arrayIndex)
		{
			PluginTrackables.CopyTo(array, arrayIndex);
		}

		public void Add(IStep item)
		{
			PluginTrackables.Add(item);
		}

		public void Insert(int index, IStep item)
		{
			PluginTrackables.Insert(index, item);
		}

		public bool Remove(IStep item)
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