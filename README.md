## This is an add-in for [Fody](https://github.com/Fody/Fody/) 

[Introduction to Fody](http://github.com/Fody/Fody/wiki/SampleUsage)

## Nuget 

Nuget package http://nuget.org/packages/NullGuard.Fody 

To Install from the Nuget Package Manager Console 
    
    PM> Install-Package NullGuard.Fody

### Your Code


    public class Sample
    {
        public void SomeMethod(string arg)
        {
            // throws ArgumentNullException if arg is null.
        }

        public void AnotherMethod([AllowNull] string arg)
        {
            // arg may be null here
        }

        public string MethodWithReturn()
        {
            return SomeOtherClass.SomeMethod();
        }

        [return: AllowNull]
        public string MethodAllowsNullReturnValue()
        {
            return null;
        }

        // Null checking works for automatic properties too.
        public string SomeProperty { get; set; }

        // can be applied to a whole property
        [AllowNull] 
        public string NullProperty { get; set; }

        // Or just the setter.
        public string NullPropertyOnSet { get; [param: AllowNull] set; }
    }

### What gets compiled 

    public class SampleOutput
    {

        public string NullProperty{get;set}
    
        string nullPropertyOnSet;
        public string NullPropertyOnSet
        {
            get
            {
                var returnValue = nullPropertyOnSet;
                if (returnValue == null)
                {
                    throw new InvalidOperationException("Return value of property 'NullPropertyOnSet' is null.");
                }
                return returnValue;
            }
            set
            {
                nullPropertyOnSet = value;
            }
        }
    
        public string MethodAllowsNullReturnValue()
        {
            return null;
        }

        string someProperty;
        public string SomeProperty
        {
            get
            {
                if (someProperty == null)
                {
                    throw new InvalidOperationException("Return value of property 'SomeProperty' is null.");
                }
                return someProperty;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "Cannot set the value of property 'SomeProperty' to null.");
                }
                someProperty = value;
            }
        }

        public void AnotherMethod(string arg)
        {
        }

        public string MethodWithReturn()
        {
            var returnValue = SomeOtherClass.SomeMethod();
            if (returnValue == null)
            {
                throw new InvalidOperationException("Return value of method 'MethodWithReturn' is null.");
            }
            return returnValue;
        }

        public void SomeMethod(string arg)
        {
            if (arg == null)
            {
                throw new ArgumentNullException("arg");
            }
        }
    }
    
## Attributes

Where and how injection occurs can be controlled via attributes. The NullGuard.Fody nuget ships with an assembly containing these attributes.

    namespace NullGuard
    {
        /// <summary>
        /// Prevents the injection of null checking.
        /// </summary>
        [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.Property)]
        public class AllowNullAttribute : Attribute
        {
        }
        
        /// <summary>
        /// Allow specific categories of members to be targeted for injection. <seealso cref="ValidationFlags"/>
        /// </summary>
        [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class)]
        public class NullGuardAttribute : Attribute
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="NullGuardAttribute"/> with a <see cref="ValidationFlags"/>.
            /// </summary>
            /// <param name="flags">The <see cref="ValidationFlags"/> to use for the target this attribute is being applied to.</param>
            public NullGuardAttribute(ValidationFlags flags)
            {
            }
        }
        
        /// <summary>
        /// Used by <see cref="NullGuardAttribute"/> to taget specific categories of members.
        /// </summary>
        [Flags]
        public enum ValidationFlags
        {
            None = 0,
            Properties = 1,
            Arguments = 2,
            OutValues = 4,
            ReturnValues = 8,
            NonPublic = 16,
            Methods = Arguments | OutValues | ReturnValues,
            AllPublicArguments = Properties | Arguments,
            AllPublic = Properties | Methods,
            All = AllPublic | NonPublic
        }
    }

## Configuration

For Release builds NullGuard will weave code that throws ArgumentNullException. For Debug builds NullGuard weaves Debug.Assert. 
If you want ArgumentNullException to be thrown for Debug builds then update FodyWeavers.xml to include:

    <NullGuard IncludeDebugAssert="false" />

An complete example of FodyWeavers.xml looks like this:

    <?xml version="1.0" encoding="utf-8" ?>
    <Weavers>
        <NullGuard IncludeDebugAssert="false" />
    </Weavers>

You can also use RegEx to specify the name of a class to exclude from NullGuard.

    <NullGuard ExcludeRegex="^ClassToExclude$" />

## Contributors

  * [Cameron MacFarland](https://github.com/distantcam)
  * [Simon Cropp](https://github.com/simoncropp)
  * [Tim Murphy](https://github.com/TimMurphy)

## Icon

Icon courtesy of [The Noun Project](http://thenounproject.com)
