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
                .GetDerivedTypesFor(typeof(IStep<IEnumerable<IContext>, IEnumerable<IContext>>))
                .Where(type => type.IsClass)
                .ToDictionary(t=>t.Name, t => t);
        }

        public IEnumerable<IStep<IEnumerable<IContext>, IEnumerable<IContext>>> Get(Settings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (settings.Pipe == null) throw new ArgumentNullException(nameof(settings.Pipe));
            Settings.Step emptyStepName = settings.Pipe.FirstOrDefault(x => x.StepSettings == null);
            if (emptyStepName != null)
            {
                throw new InvalidOperationException($"Missing settings for step '{emptyStepName.StepName}'.");
            }
         
            IEnumerable<KeyValuePair<Type, IStep<IEnumerable<IContext>, IEnumerable<IContext>>>> product = GetCorrectTypesAndData(settings);

            foreach (KeyValuePair<Type, IStep<IEnumerable<IContext>, IEnumerable<IContext>>> pipeStep in product)
            {
                IStep<IEnumerable<IContext>, IEnumerable<IContext>> stepInstance =
                    Activator.CreateInstance(pipeStep.Key, pipeStep.Value) as
                        IStep<IEnumerable<IContext>, IEnumerable<IContext>>;

                yield return stepInstance;
            }
        }



        private IEnumerable<KeyValuePair<Type, IStep<IEnumerable<IContext>, IEnumerable<IContext>>>> GetCorrectTypesAndData(Settings settings)
        {
            foreach (Settings.Step step in settings.Pipe)
            {

                Type stepType = _stepTypes[step.StepName];

                ConstructorInfo stepConstructor = stepType.GetConstructors().FirstOrDefault();

                if (stepConstructor == null)
                {
                    throw new InvalidOperationException(
                        $"Pipe step '{step.StepName}' class should have a constructor.");
                }

                // Extract the relevant settings type from the step constructor and the convert Json settings to this type.
                ParameterInfo constructorSettingsParameter =
                    stepConstructor.GetParameters().FirstOrDefault(x => x.Position == 0);

                // ReSharper disable once PossibleNullReferenceException
                Type type = constructorSettingsParameter.ParameterType;
                IStep<IEnumerable<IContext>, IEnumerable<IContext>> instance = step.StepSettings.ToObject(type) as IStep<IEnumerable<IContext>, IEnumerable<IContext>>;

                if (instance == null)
                {
                    throw new NullReferenceException($"Error: {step.StepSettings.Type} is not derived from IStep<IEnumerable<IContext>, IEnumerable<IContext>>");
                }

                instance.Identifier = settings.Identifier;

                yield return KeyValuePair.Create(stepType, instance);
            }
        }

    }
}
