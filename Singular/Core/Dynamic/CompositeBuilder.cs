namespace Singular.Core.Dynamic
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using global::Singular.Core.Composite;
    using global::Singular.Core.Enum;

    /// <summary>
    /// Builds the composites using the Behavior attribute
    /// </summary>
    public static class CompositeBuilder
    {
        /// <summary>
        /// Gets or sets the type of the behavior.
        /// </summary>
        /// <value>
        /// The type of the behavior.
        /// </value>
        public static BehaviorType BehaviorType { get; set; }

        /// <summary>
        /// A list of methods for the assembly with a composite return type
        /// </summary>
        private static readonly List<MethodInfo> Methods = new List<MethodInfo>();

        /// <summary>
        /// Invokes the initializers.
        /// </summary>
        /// <param name="champion">The champion.</param>
        public static void InvokeInitializers(Champion champion)
        {
            if (champion == Champion.None)
            {
                return;
            }

            if (!Methods.Any())
            {
                foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
                {
                    Methods.AddRange(type.GetMethods(BindingFlags.Static | BindingFlags.Public).Where(m => !m.IsGenericMethod && !m.GetParameters().Any()).Where(m => m.ReturnType.IsAssignableFrom(typeof(Composite))));
                }
            }

            var matches = new Dictionary<BehaviorAttribute, MethodInfo>();
            foreach (var method in Methods)
            {
                foreach (var attribute in method.GetCustomAttributes(typeof(BehaviorAttribute), false))
                {
                    var behavior = attribute as BehaviorAttribute;
                    if (behavior == null || !IsMatchingMethod(behavior, champion, BehaviorType.Initialize))
                    {
                        continue;
                    }

                    matches.Add(behavior, method);
                }
            }

            foreach (var match in matches.OrderByDescending(m => m.Key.Priority))
            {
                match.Value.Invoke(null, null);
            }
        }

        /// <summary>
        /// Gets the composite.
        /// </summary>
        /// <param name="champion">The champion.</param>
        /// <param name="type">The type.</param>
        /// <param name="count">The count of composites.</param>
        /// <returns>
        /// The sequential selector composite or null if no matches
        /// </returns>
        public static Composite GetComposite(Champion champion, BehaviorType type, out int count)
        {
            count = 0;

            var matches = new Dictionary<BehaviorAttribute, Composite>();

            foreach (var method in Methods)
            {
                foreach (var attribute in method.GetCustomAttributes(typeof(BehaviorAttribute), false))
                {
                    var behavior = attribute as BehaviorAttribute;
                    if (behavior == null || !IsMatchingMethod(behavior, champion, type))
                    {
                        continue;
                    }

                    var composite = method.Invoke(null, null) as Composite;
                    matches.Add(behavior, composite);
                }
            }

            if (!matches.Any())
            {
                return null;
            }

            var selector = new SequentialSelector();
            foreach (var composite in matches.OrderByDescending(m => m.Key.Priority).Select(m => m.Value))
            {
                selector.AddChild(composite);
                count++;
            }

            return selector;
        }

        /// <summary>
        /// Determines whether [is matching method] [the specified attribute].
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="champion">The champion.</param>
        /// <param name="type">The type.</param>
        /// <returns>True if matches else false</returns>
        private static bool IsMatchingMethod(BehaviorAttribute attribute, Champion champion, BehaviorType type)
        {
            if (attribute.Champion != champion && attribute.Champion != Champion.None)
            {
                return false;
            }

            if ((attribute.Type & type) == 0)
            {
                return false;
            }

            return true;
        }
    }
}
