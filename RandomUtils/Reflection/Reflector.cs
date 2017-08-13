using RandomUtils.Attributes;
using RandomUtils.Attributes.Collections;
using RandomUtils.Enums;
using RandomUtils.Parameterization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Reflection
{
    internal class Reflector<TInput> : BaseReflector<TInput>
    {        
        internal Reflector(Assembly assembly) : base(assembly)
        {

        }

        internal override void Analyze(string resourceName = "")
        {
            Type randomizedType = typeof(TInput);
            List<AspectModel> readAspects = new List<AspectModel>();
            if (!string.IsNullOrEmpty(resourceName))
            {
                _resourceName = resourceName;
                Parser parser = new Parser(_callingAssembly, _resourceName);
                readAspects = parser.ParseAspects();
            }
            Aspects = Parse(randomizedType, readAspects).ToList();
            List<MethodInfo> validator = randomizedType.GetMethods()
                .Where(method => IsValidator(method)).ToList();
            if (validator.Count == 1)
            {
                RandomizationValidator = validator.First();
            }
        }

        private IEnumerable<Aspect> Parse(Type randomizedType, List<AspectModel> readAspects)
        {
            foreach (PropertyInfo property in randomizedType.GetProperties())
            {
                bool ignoreProperty = ShouldIgnore(property);

                if (property.CanWrite && !ignoreProperty)
                {
                    List<AspectModel> currentAspectModels = readAspects
                        .Where(aspect => aspect.PropertyName == property.Name).ToList();
                    AspectModel currentAspectModel;
                    if (currentAspectModels.Count > 1)
                    {
                        currentAspectModel = currentAspectModels.Where(aspect => aspect.ClassName == property.DeclaringType.Name).FirstOrDefault();
                    }
                    else
                    {
                        currentAspectModel = currentAspectModels.FirstOrDefault();
                    }
                    if (currentAspectModel != null && currentAspectModel.Maximum < currentAspectModel.Minimum)
                    {
                        throw new ArgumentException("The specified maximum must not be less than the specified minimum.");
                    }
                    yield return DefineAspect(property, currentAspectModel);
                }
            }
        }

        private bool ShouldIgnore(PropertyInfo property)
        {
            if (property.GetCustomAttribute<RandomizerIgnoreAttribute>() != null)
            {
                return true;
            }
            return false;
        }

        private bool IsValidator(MethodInfo method)
        {
            if (method.GetCustomAttribute<ValidatorAttribute>() != null)
            {
                return method.GetCustomAttribute<ValidatorAttribute>().ClassScope;
            }
            return false;
        }

        private bool IsPropertyValidator(PropertyInfo property, MethodInfo method)
        {
            if (method.GetCustomAttribute<ValidatorAttribute>() != null)
            {
                ValidatorAttribute validator = method.GetCustomAttribute<ValidatorAttribute>();
                return !validator.ClassScope && validator.Validate.ToLower() == property.Name.ToLower();
            }
            return false;
        }

        private bool IsCollectionItemValidator(PropertyInfo property, MethodInfo method)
        {
            if (method.GetCustomAttribute<CollectionItemValidatorAttribute>() != null)
            {
                CollectionItemValidatorAttribute validator = method.GetCustomAttribute<CollectionItemValidatorAttribute>();
                return validator.Validate.ToLower() == property.Name.ToLower();
            }
            return false;
        }

        private Aspect DefineAspect(PropertyInfo property, AspectModel aspectModel)
        {
            Aspect currentAspect = new Aspect();
            currentAspect = DefineBaseAspects(property, aspectModel, currentAspect);

            bool isCollection = (property.PropertyType.GetInterfaces()
                            .Where(type => type.Namespace.Contains("Collection")).Count() > 0 ||
                            property.PropertyType.IsArray);
            if (property.PropertyType == typeof(string))
            {
                currentAspect = DefineStringAspects(property, aspectModel, currentAspect);
            }
            else if (isCollection)
            {
                currentAspect = DefineCollectionAspects(property, aspectModel, currentAspect);
                if (currentAspect.ConcreteCollectionItemType == typeof(DateTime))
                {
                    currentAspect = DefineDateTimeAspects(property, aspectModel, currentAspect);
                }
            }
            else
            {
                if (property.PropertyType == typeof(DateTime))
                {
                    currentAspect = DefineDateTimeAspects(property, aspectModel, currentAspect);
                }
            }

            return currentAspect;
        }

        private Aspect DefineBaseAspects(PropertyInfo property, AspectModel aspectModel, Aspect currentAspect)
        {
            currentAspect.Property = property;
            if (property.GetCustomAttribute<RequiredAttribute>() != null)
            {
                currentAspect.Required = property.GetCustomAttribute<RequiredAttribute>().IsRequired;
            }
            else if (aspectModel != null)
            {
                currentAspect.Required = aspectModel.Required;
            }
            else
            {
                currentAspect.Required = false;
            }

            if (property.GetCustomAttribute<MinimumAttribute>() != null)
            {
                currentAspect.Minimum = property.GetCustomAttribute<MinimumAttribute>().MinimumSize;
            }
            else if (aspectModel != null)
            {
                currentAspect.Minimum = aspectModel.Minimum;
            }
            else
            {
                currentAspect.Minimum = DefaultValues.DEFAULTMINIMUM;
            }

            if (property.GetCustomAttribute<MaximumAttribute>() != null)
            {
                currentAspect.Maximum = property.GetCustomAttribute<MaximumAttribute>().MaximumSize;
            }
            else if (aspectModel != null)
            {
                currentAspect.Maximum = aspectModel.Maximum;
            }
            else
            {
                currentAspect.Maximum = DefaultValues.DEFAULTMAXIMUM;
            }

            if (property.GetCustomAttribute<ConcreteTypeAttribute>() != null)
            {
                currentAspect.ConcreteType = property.GetCustomAttribute<ConcreteTypeAttribute>().ConcreteType;
            }
            else
            {
                currentAspect.ConcreteType = property.PropertyType;
            }

            if (property.GetCustomAttribute<ValueAttribute>() != null)
            {
                currentAspect.ValueType = property.GetCustomAttribute<ValueAttribute>().ValueType;
            }
            else
            {
                currentAspect.ValueType = Enums.ValueType.DefaultValue;
            }

            List<MethodInfo> validator = property.DeclaringType.GetMethods()
                .Where(method => IsPropertyValidator(property, method)).ToList();
            if (validator.Count == 1)
            {
                currentAspect.Validator = validator.First();
            }
            return currentAspect;
        }

        private Aspect DefineCollectionAspects(PropertyInfo property, AspectModel aspectModel, Aspect currentAspect)
        {
            Aspect newCurrentAspect = currentAspect;
            if (property.GetCustomAttribute<MinimumLengthAttribute>() != null)
            {
                currentAspect.MinimumLength = property.GetCustomAttribute<MinimumLengthAttribute>().MinimumLength;
            }
            else if (aspectModel != null)
            {
                currentAspect.MinimumLength = aspectModel.MinimumLength;
            }
            else
            {
                currentAspect.MinimumLength = DefaultValues.DEFAULTMINIMUMLENGTH;
            }

            if (property.GetCustomAttribute<MaximumLengthAttribute>() != null)
            {
                currentAspect.MaximumLength = property.GetCustomAttribute<MaximumLengthAttribute>().MaximumLength;
            }
            else if (aspectModel != null)
            {
                currentAspect.MaximumLength = aspectModel.MaximumLength;
            }
            else
            {
                currentAspect.MaximumLength = DefaultValues.DEFAULTMAXIMUMLENGTH;
            }

            if (property.GetCustomAttribute<CollectionItemConcreteTypeAttribute>() != null)
            {
                currentAspect.ConcreteCollectionItemType = property.GetCustomAttribute<CollectionItemConcreteTypeAttribute>().ConcreteType;
            }
            else
            {
                if (property.PropertyType.BaseType == typeof(Array))
                {
                    throw new ArgumentException($"The specified type in property {property.DeclaringType.Name}.{property.Name} is an array. Please switch to a List<T> instead.");
                }
                else
                {
                    currentAspect.ConcreteCollectionItemType = property.PropertyType.GenericTypeArguments.First();
                    if (!currentAspect.ConcreteCollectionItemType.HasDefaultConstructor())
                    {
                        throw new ArgumentException($"The specified Concrete type of the items in the collection specified in property {property.DeclaringType.Name}.{property.Name} needs to be concrete and have a parameterless constructor.");
                    }
                }
            }

            List<MethodInfo> validator = property.DeclaringType.GetMethods()
                .Where(method => IsCollectionItemValidator(property, method)).ToList();
            if (validator.Count == 1)
            {
                currentAspect.CollectionItemValidator = validator.First();
            }
            return newCurrentAspect;
        }

        private Aspect DefineDateTimeAspects(PropertyInfo property, AspectModel aspectModel, Aspect currentAspect)
        {
            if (property.GetCustomAttribute<EarliestDateAttribute>() != null)
            {
                currentAspect.EarliestDateTime = property.GetCustomAttribute<EarliestDateAttribute>().EarliestDate;
            }
            else if (aspectModel != null)
            {
                currentAspect.EarliestDateTime = aspectModel.EarliestDateTime;
            }
            else
            {
                currentAspect.EarliestDateTime = DefaultValues.DEFAULTEARLIESTDATETIME;
            }

            if (property.GetCustomAttribute<LatestDateAttribute>() != null)
            {
                currentAspect.LatestDateTime = property.GetCustomAttribute<LatestDateAttribute>().LatestDateTime;
            }
            else if (aspectModel != null)
            {
                currentAspect.LatestDateTime = aspectModel.LatestDateTime;
            }
            else
            {
                currentAspect.LatestDateTime = DefaultValues.DEFAULTLATESTDATETIME;
            }
            return currentAspect;
        }

        private Aspect DefineStringAspects(PropertyInfo property, AspectModel aspectModel, Aspect currentAspect)
        {
            if (property.GetCustomAttribute<StringTypeAttribute>() != null)
            {
                currentAspect.StringType = property.GetCustomAttribute<StringTypeAttribute>().StringType;
                if (currentAspect.StringType == StringType.Regex)
                {
                    currentAspect.RegexPattern = property.GetCustomAttribute<StringTypeAttribute>().RegexPattern;
                }
            }
            else if (aspectModel != null)
            {
                currentAspect.StringType = aspectModel.StringType;
                if (currentAspect.StringType == StringType.Regex)
                {
                    currentAspect.RegexPattern = aspectModel.RegexPattern;
                }
            }
            else
            {
                currentAspect.StringType = DefaultValues.DEFAULTSTRINGTYPE;
            }
            return currentAspect;
        }
    }
}
