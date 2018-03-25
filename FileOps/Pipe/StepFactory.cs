using System;
using System.Collections.Generic;
using FileOps.Configuration.Entities;
using FileOps.Common;
using System.Linq;
using System.Reflection;

namespace FileOps.Pipe
{
    
    internal class StepFactory : IStepFactory
    {

        private readonly IDictionary<string, Type> _stepTypes;

        public StepFactory()
        {
            _stepTypes = AssemblyHelper
                .GetDerivedTypesFor(typeof(IStep<IAggregate, IAggregate>))
                .Where(type => type.IsClass)
                .ToDictionary(t=>t.Name, t => t);
        }
        
        public IEnumerable<IStep<IAggregate, IAggregate>> Get(Settings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            if (settings.Pipe == null) {

                string message = settings.Common.IsNullOrEmpty() ? nameof(settings.Pipe) : $"{nameof(settings.Common)} configuration is filled but { nameof(settings.Pipe) } is empty";
                throw new ArgumentNullException(message);
            }

            Settings.Step emptyStepName = settings.Pipe.FirstOrDefault(x => x.StepSettings == null);
            if (emptyStepName != null)
            {
                throw new InvalidOperationException($"Missing settings for step '{emptyStepName.StepName}'.");
            }

            return GetCorrectTypesAndData(settings);
        }



        private IEnumerable<IStep<IAggregate, IAggregate>> GetCorrectTypesAndData(Settings settings)
        {
            foreach (Settings.Step step in settings.Pipe)
            {
                Type stepType;
                object constructorParameterInstance;
                try
                {
                    stepType = _stepTypes[step.StepName];

                    ConstructorInfo stepConstructor = stepType.GetConstructors().FirstOrDefault();

                    if (stepConstructor == null)
                    {
                        throw new InvalidOperationException(
                            $"Pipe step '{step.StepName}' class should have a constructor.");
                    }

                    // Extract the relevant settings type from the step constructor and the convert Json settings to this type.
                    ParameterInfo constructorSettingsParameter =
                        stepConstructor.GetParameters().FirstOrDefault();

                    Type type = constructorSettingsParameter.ParameterType;

                    constructorParameterInstance = step.StepSettings.ToObject(type);

                    if (constructorParameterInstance == null)
                    {
                        throw new NullReferenceException($"Error: {step.StepSettings.Type} is not derived from IStep<IEnumerable<IContext>, IEnumerable<IContext>>");
                    }

                    if(constructorParameterInstance as ISettings == null)
                    {
                        throw new InvalidCastException($"Error: {step.StepSettings.Type} doesn't implement {typeof(ISettings).FullName}");
                    }

                    ((ISettings)constructorParameterInstance).GroupIdentifier = settings.GroupIdentifier;

                    ((ISettings)constructorParameterInstance).Identifier = settings.Identifier;
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error at building pipe for step {settings.Identifier} - {step.StepName}",ex);
                }

                yield return Activator.CreateInstance(stepType, constructorParameterInstance) as IStep<IAggregate, IAggregate>;
            }
        }

    }
}
