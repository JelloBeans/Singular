namespace Singular.Core.Dynamic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using global::Singular.Champion;
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
        public static BehaviorType CurrentBehaviorType { get; set; }

        /// <summary>
        /// A list of types for the assembly inheriting from <see cref="ChampionComposite"/>.
        /// </summary>
        private static readonly List<Type> Types = new List<Type>();

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

            if (!Types.Any())
            {
                var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ChampionComposite)));
                Types.AddRange(types);
            }

            var matches = new Dictionary<BehaviorAttribute, Type>();
            foreach (var type in Types)
            {
                foreach (var attribute in type.GetCustomAttributes(typeof(BehaviorAttribute), false))
                {
                    var behavior = attribute as BehaviorAttribute;
                    if (behavior == null || !IsMatchingMethod(behavior, champion, BehaviorType.Initialize))
                    {
                        continue;
                    }

                    matches.Add(behavior, type);
                }
            }

            foreach (var match in matches.OrderByDescending(m => m.Key.Priority))
            {
                CurrentBehaviorType = BehaviorType.Initialize;
                InvokeComposite(match.Value);
            }
        }

        /// <summary>
        /// Gets the composite.
        /// </summary>
        /// <param name="champion">The champion.</param>
        /// <param name="behavior">The behavior type.</param>
        /// <param name="count">The count of composites.</param>
        /// <returns>
        /// The sequential selector composite or null if no matches
        /// </returns>
        public static Composite GetComposite(Champion champion, BehaviorType behavior, out int count)
        {
            count = 0;

            CurrentBehaviorType = behavior;

            var matches = new Dictionary<BehaviorAttribute, Composite>();

            foreach (var type in Types)
            {
                foreach (var attribute in type.GetCustomAttributes(typeof(BehaviorAttribute), false))
                {
                    var behaviorAttribute = attribute as BehaviorAttribute;
                    if (behaviorAttribute == null || !IsMatchingMethod(behaviorAttribute, champion, behavior))
                    {
                        continue;
                    }

                    CurrentBehaviorType = behavior;
                    var composite = InvokeComposite(type);
                    matches.Add(behaviorAttribute, composite);
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

        /// <summary>
        /// Invokes the composite using dynamic methods as we are in a sandbox.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// The composite.
        /// </returns>
        private static Composite InvokeComposite(Type type)
        {
            var target = type.GetConstructor(Type.EmptyTypes);
            if (target == null || target.DeclaringType == null)
            {
                return null;
            }

            var dynamic = new DynamicMethod(string.Empty, type, new Type[0], target.DeclaringType);
            var il = dynamic.GetILGenerator();
            il.DeclareLocal(target.DeclaringType);
            il.Emit(OpCodes.Newobj, target);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);

            var method = (Func<ChampionComposite>)dynamic.CreateDelegate(typeof(Func<ChampionComposite>));
            return method().Composite;
        }
    }
}
