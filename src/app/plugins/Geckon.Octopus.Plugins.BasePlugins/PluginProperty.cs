using System;

namespace Geckon.Octopus.Plugins.BasePlugins
{
	public class PluginProperty<T>
	{
		#region Fields

		private readonly string _Name;
		private T _Value;
		private readonly IPluginExtended _Plugin;

		private Predicate<T> _IsValidTest;
		private Predicate<T> _IsInRangeTest;

		#endregion

		#region Properties

		protected string Name
		{
			get { return _Name; }
		}

		public virtual T Value
		{
			get { return _Value; }
			set
			{
				ValidateValue(value);

				_Value = value;
			}
		}

		protected IPluginExtended Plugin
		{
			get
			{
				return _Plugin;
			}
		}

		public virtual bool IsSet
		{
			get
			{
				return true;
			}
		}

		#endregion

		#region Constructors

		public PluginProperty(string name, IPluginExtended plugin)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (plugin == null)
				throw new ArgumentNullException("plugin");

			_Name = name;
			_Plugin = plugin;

			AddIsValidTest(IsValid);
			AddIsInRangeTest(IsInRange);
		}

		public PluginProperty(string name, IPluginExtended plugin, T value)
			: this(name, plugin)
		{
			_Value = value;
		}

		#endregion

		#region Property Tests

		public void AddIsValidTest(Predicate<T> test)
		{
			_IsValidTest += test;
		}

		public void RemoveIsValidTest(Predicate<T> test)
		{
			_IsValidTest -= test;
		}

		public void AddIsInRangeTest(Predicate<T> test)
		{
			_IsInRangeTest += test;
		}

		public void RemoveIsInRangeTest(Predicate<T> test)
		{
			_IsInRangeTest -= test;
		}

		public void ValidatePropertyIsSet()
		{
			if (!IsSet)
				throw new PropertyNotSetBeforeOperationException(Name);
		}

		private void ValidatePropertyIsValid(T value)
		{
			RunTest<ArgumentException>(_IsValidTest, value, "Invalid value attempted set for " + Name);
		}

		private void ValidatePropertyIsInRange(T value)
		{
			RunTest<ArgumentOutOfRangeException>(_IsInRangeTest, value);
		}

		protected virtual void ValidateValue(T value)
		{
			ValidatePropertyIsValid(value);
			ValidatePropertyIsInRange(value);
		}

		protected virtual bool IsValid(T value)
		{
			return true;
		}

		protected virtual bool IsInRange(T value)
		{
			return true;
		}

		protected void RunTest<U>(Predicate<T> test, T value) where U : Exception
		{
			RunTest<U>(test, value, null);
		}

		//Run through a multicast delegate, if any method returns false, throw specified exception with specified parameter.
		protected void RunTest<U>(Predicate<T> test, T value, string exceptionParameter) where U : Exception
		{
			if (test == null)
				return;

			foreach (Predicate<T> singleTest in test.GetInvocationList())
			{
				if (!singleTest(value))
				{
					if(exceptionParameter == null)
						throw (Exception)Activator.CreateInstance(typeof(U), new object[] { Name });

					throw (Exception)Activator.CreateInstance(typeof(U), new object[] { exceptionParameter });	
				}
					
			}
		}

		#endregion

		#region Business Logic

		public void SetValueIfPropertiesAreEditable(T value)
		{
			if (!Plugin.ArePropertiesEditable)
				throw new PropertySetAfterOperationStartedException(Name);

			Value = value;
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		#endregion
	}
}