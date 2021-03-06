//------------------------------------------------------------------------------
// <auto-generated>                                                             
//     This code was generated by a tool.                                       
//     Changes to this file may cause incorrect behavior and will be lost if    
//     the code is regenerated.                                                 
// </auto-generated>                                                            
//------------------------------------------------------------------------------
namespace QuantumRNG.__QsEntryPoint__
{
    using System;
    using Microsoft.Quantum.Core;
    using Microsoft.Quantum.Intrinsic;
    using Microsoft.Quantum.Simulation.Core;

    internal static class Constants
    {
        public static System.Collections.Generic.IEnumerable<string> SimulatorOptions => new [] { "--simulator", "-s" };
        public const string QuantumSimulator = "QuantumSimulator";
        public const string ToffoliSimulator = "ToffoliSimulator";
        public const string ResourcesEstimator = "ResourcesEstimator";
    }

    internal class EntryPoint
    {
        public static string Summary => "";
        public static string DefaultSimulator => "QuantumSimulator";
        public static System.Collections.Generic.IEnumerable<System.CommandLine.Option> Options => new System.CommandLine.Option[] { };
        public static IOperationFactory CreateDefaultCustomSimulator() => throw new InvalidOperationException();
        public async System.Threading.Tasks.Task<Int64> Run(IOperationFactory __factory__) => await QuantumRNG.SampleRandomNumber.Run(__factory__);
    }
}
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace QuantumRNG.__QsEntryPoint__
{
    using Microsoft.Quantum.Simulation.Core;
    using Microsoft.Quantum.Simulation.Simulators;
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.CommandLine;
    using System.CommandLine.Builder;
    using System.CommandLine.Help;
    using System.CommandLine.Invocation;
    using System.CommandLine.Parsing;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The entry point driver is the entry point for the C# application that executes the Q# entry point.
    /// </summary>
    internal static class Driver
    {
        /// <summary>
        /// A modification of the command line <see cref="HelpBuilder"/> class.
        /// </summary>
        private sealed class QsHelpBuilder : HelpBuilder
        {
            public QsHelpBuilder(IConsole console) : base(console) { }

            protected override string ArgumentDescriptor(IArgument argument)
            {
                // Hide long argument descriptors.
                var descriptor = base.ArgumentDescriptor(argument);
                return descriptor.Length > 30 ? argument.Name : descriptor;
            }
        }

        /// <summary>
        /// Runs the entry point.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        /// <returns>The exit code.</returns>
        private static async Task<int> Main(string[] args)
        {
            var simulate = new Command("simulate", "(default) Run the program using a local simulator.");
            TryCreateOption(
                Constants.SimulatorOptions,
                () => EntryPoint.DefaultSimulator,
                "The name of the simulator to use.").Then(option =>
                {
                    option.Argument.AddSuggestions(ImmutableHashSet<string>.Empty
                        .Add(Constants.QuantumSimulator)
                        .Add(Constants.ToffoliSimulator)
                        .Add(Constants.ResourcesEstimator)
                        .Add(EntryPoint.DefaultSimulator));
                    simulate.AddOption(option);
                });
            simulate.Handler = CommandHandler.Create<EntryPoint, string>(Simulate);

            var root = new RootCommand(EntryPoint.Summary) { simulate };
            foreach (var option in EntryPoint.Options) { root.AddGlobalOption(option); }

            // Set the simulate command as the default.
            foreach (var option in simulate.Options) { root.AddOption(option); }
            root.Handler = simulate.Handler;

            return await new CommandLineBuilder(root)
                .UseDefaults()
                .UseHelpBuilder(context => new QsHelpBuilder(context.Console))
                .Build()
                .InvokeAsync(args);
        }

        /// <summary>
        /// Simulates the entry point.
        /// </summary>
        /// <param name="entryPoint">The entry point.</param>
        /// <param name="simulator">The simulator to use.</param>
        /// <returns>The exit code.</returns>
        private static async Task<int> Simulate(EntryPoint entryPoint, string simulator)
        {
            simulator = DefaultIfShadowed(Constants.SimulatorOptions.First(), simulator, EntryPoint.DefaultSimulator);
            switch (simulator)
            {
                case Constants.ResourcesEstimator:
                    var resourcesEstimator = new ResourcesEstimator();
                    await entryPoint.Run(resourcesEstimator);
                    Console.WriteLine(resourcesEstimator.ToTSV());
                    break;
                default:
                    var (isCustom, createSimulator) = simulator switch
                    {
                        Constants.QuantumSimulator =>
                            (false, new Func<IOperationFactory>(() => new QuantumSimulator())),
                        Constants.ToffoliSimulator =>
                            (false, new Func<IOperationFactory>(() => new ToffoliSimulator())),
                        _ => (true, EntryPoint.CreateDefaultCustomSimulator)
                    };
                    if (isCustom && simulator != EntryPoint.DefaultSimulator)
                    {
                        DisplayCustomSimulatorError(simulator);
                        return 1;
                    }
                    await DisplayEntryPointResult(entryPoint, createSimulator);
                    break;
            }
            return 0;
        }

        /// <summary>
        /// Runs the entry point on a simulator and displays its return value.
        /// </summary>
        /// <param name="entryPoint">The entry point.</param>
        /// <param name="createSimulator">A function that creates an instance of the simulator to use.</param>
        private static async Task DisplayEntryPointResult(
            EntryPoint entryPoint, Func<IOperationFactory> createSimulator)
        {
            var simulator = createSimulator();
            try
            {
                var value = await entryPoint.Run(simulator);
#pragma warning disable CS0184
                if (!(value is QVoid))
#pragma warning restore CS0184
                {
                    Console.WriteLine(value);
                }
            }
            finally
            {
                if (simulator is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        /// <summary>
        /// Returns true if the alias is available for use by the driver (that is, the alias is not already used by an
        /// entry point option).
        /// </summary>
        /// <param name="alias">The alias to check.</param>
        /// <returns>True if the alias is available for use by the driver.</returns>
        private static bool IsAliasAvailable(string alias) =>
            !EntryPoint.Options.SelectMany(option => option.RawAliases).Contains(alias);

        /// <summary>
        /// Returns the default value and displays a warning if the alias is shadowed by an entry point option, and
        /// returns the original value otherwise.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="alias">The primary option alias corresponding to the value.</param>
        /// <param name="value">The value of the option given on the command line.</param>
        /// <param name="defaultValue">The default value for the option.</param>
        /// <returns></returns>
        private static T DefaultIfShadowed<T>(string alias, T value, T defaultValue)
        {
            if (IsAliasAvailable(alias))
            {
                return value;
            }
            else
            {
                var originalForeground = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Error.WriteLine($"Warning: Option {alias} is overriden by an entry point parameter name.");
                Console.Error.WriteLine($"         Using default value {defaultValue}.");
                Console.ForegroundColor = originalForeground;
                return defaultValue;
            }
        }

        /// <summary>
        /// Tries to create an option by removing aliases that are already in use by the entry point. If the first
        /// alias, which is considered the primary alias, is in use, then the option is not created.
        /// </summary>
        /// <typeparam name="T">The type of the option's argument.</typeparam>
        /// <param name="aliases">The option's aliases.</param>
        /// <param name="getDefaultValue">A function that returns the option's default value.</param>
        /// <param name="description">The option's description.</param>
        /// <returns>A validation of the option.</returns>
        private static Validation<Option<T>> TryCreateOption<T>(
                IEnumerable<string> aliases, Func<T> getDefaultValue, string description = null) =>
            IsAliasAvailable(aliases.First())
            ? Validation<Option<T>>.Success(
                new Option<T>(aliases.Where(IsAliasAvailable).ToArray(), getDefaultValue, description))
            : Validation<Option<T>>.Failure();

        /// <summary>
        /// Displays an error message for using a non-default custom simulator.
        /// </summary>
        /// <param name="name">The name of the custom simulator.</param>
        private static void DisplayCustomSimulatorError(string name)
        {
            var originalForeground = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"The simulator '{name}' could not be found.");
            Console.ForegroundColor = originalForeground;
            Console.Error.WriteLine();
            Console.Error.WriteLine(
                $"If '{name}' is a custom simulator, it must be set in the DefaultSimulator project property:");
            Console.Error.WriteLine();
            Console.Error.WriteLine("<PropertyGroup>");
            Console.Error.WriteLine($"  <DefaultSimulator>{name}</DefaultSimulator>");
            Console.Error.WriteLine("</PropertyGroup>");
        }
    }
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace QuantumRNG.__QsEntryPoint__
{
    using Microsoft.Quantum.Simulation.Core;
    using System;
    using System.Collections.Generic;
    using System.CommandLine.Parsing;
    using System.Linq;
    using System.Numerics;

    /// <summary>
    /// A delegate that parses the value and returns a validation.
    /// </summary>
    /// <typeparam name="T">The type parsed value.</typeparam>
    /// <param name="value">The string to parse.</param>
    /// <param name="optionName">The name of the option that the value was used with.</param>
    /// <returns>A validation of the parsed value.</returns>
    internal delegate Validation<T> TryParseValue<T>(string value, string optionName = null);

    /// <summary>
    /// Parsers for command-line arguments.
    /// </summary>
    internal static class Parsers
    {
        /// <summary>
        /// Creates an argument parser for a many-valued argument using a parser that operates on each string value.
        /// </summary>
        /// <typeparam name="T">The type of the parsed value.</typeparam>
        /// <param name="parse">The string parser.</param>
        /// <returns>The argument parser.</returns>
        internal static ParseArgument<IEnumerable<T>> ParseArgumentsWith<T>(TryParseValue<T> parse) => argument =>
        {
            var optionName = ((OptionResult)argument.Parent).Token.Value;
            var validation = argument.Tokens.Select(token => parse(token.Value, optionName)).Sequence();
            if (validation.IsFailure)
            {
                argument.ErrorMessage = validation.ErrorMessage;
            }
            return validation.ValueOrDefault;
        };

        /// <summary>
        /// Creates an argument parser for a single-valued argument using a parser that operates on the string value.
        /// </summary>
        /// <typeparam name="T">The type of the parsed value.</typeparam>
        /// <param name="parse">The string parser.</param>
        /// <returns>The argument parser.</returns>
        internal static ParseArgument<T> ParseArgumentWith<T>(TryParseValue<T> parse) => argument =>
        {
            var values = ParseArgumentsWith(parse)(argument);
            return values == null ? default : values.Single();
        };

        /// <summary>
        /// Parses a <see cref="BigInteger"/>.
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <param name="optionName">The name of the option that the value was used with.</param>
        /// <returns>A validation of the parsed <see cref="BigInteger"/>.</returns>
        internal static Validation<BigInteger> TryParseBigInteger(string value, string optionName = null) =>
            BigInteger.TryParse(value, out var result)
            ? Validation<BigInteger>.Success(result)
            : Validation<BigInteger>.Failure(GetArgumentErrorMessage(value, optionName, typeof(BigInteger)));

        /// <summary>
        /// Parses a <see cref="QRange"/>.
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <param name="optionName">The name of the option that the value was used with.</param>
        /// <returns>A validation of the parsed <see cref="QRange"/>.</returns>
        internal static Validation<QRange> TryParseQRange(string value, string optionName = null)
        {
            Validation<long> tryParseLong(string longValue) =>
                long.TryParse(longValue, out var result)
                ? Validation<long>.Success(result)
                : Validation<long>.Failure(GetArgumentErrorMessage(longValue, optionName, typeof(long)));

            return value.Split("..").Select(tryParseLong).Sequence().Bind(values =>
                values.Count() == 2
                ? Validation<QRange>.Success(new QRange(values.ElementAt(0), values.ElementAt(1)))
                : values.Count() == 3
                ? Validation<QRange>.Success(new QRange(values.ElementAt(0), values.ElementAt(1), values.ElementAt(2)))
                : Validation<QRange>.Failure(GetArgumentErrorMessage(value, optionName, typeof(QRange))));
        }

        /// <summary>
        /// Parses a <see cref="QVoid"/>.
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <param name="optionName">The name of the option that the value was used with.</param>
        /// <returns>A validation of the parsed <see cref="QVoid"/>.</returns>
        internal static Validation<QVoid> TryParseQVoid(string value, string optionName = null) =>
            value.Trim() == QVoid.Instance.ToString()
            ? Validation<QVoid>.Success(QVoid.Instance)
            : Validation<QVoid>.Failure(GetArgumentErrorMessage(value, optionName, typeof(QVoid)));

        /// <summary>
        /// Parses a <see cref="Result"/>.
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <param name="optionName">The name of the option that the value was used with.</param>
        /// <returns>A validation of the parsed <see cref="Result"/>.</returns>
        internal static Validation<Result> TryParseResult(string value, string optionName = null) =>
            Enum.TryParse(value, ignoreCase: true, out ResultValue result)
            ? Validation<Result>.Success(result switch
            {
                ResultValue.Zero => Result.Zero,
                ResultValue.One => Result.One,
                var invalid => throw new Exception($"Invalid result value '{invalid}'.")
            })
            : Validation<Result>.Failure(GetArgumentErrorMessage(value, optionName, typeof(Result)));

        /// <summary>
        /// Returns an error message string for an argument parser.
        /// </summary>
        /// <param name="arg">The value of the argument being parsed.</param>
        /// <param name="optionName">The name of the option.</param>
        /// <param name="type">The expected type of the argument.</param>
        /// <returns>An error message string for an argument parser.</returns>
        private static string GetArgumentErrorMessage(string arg, string optionName, Type type) =>
            $"Cannot parse argument '{arg}' for option '{optionName}' as expected type {type}.";
    }
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace QuantumRNG.__QsEntryPoint__
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents either a success or a failure of a process.
    /// </summary>
    /// <typeparam name="T">The type of the success value.</typeparam>
    internal struct Validation<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure { get => !IsSuccess; }
        public T Value { get => IsSuccess ? ValueOrDefault : throw new InvalidOperationException(); }
        public T ValueOrDefault { get; }
        public string ErrorMessage { get; }

        private Validation(bool isSuccess, T value, string errorMessage)
        {
            IsSuccess = isSuccess;
            ValueOrDefault = value;
            ErrorMessage = errorMessage;
        }

        public static Validation<T> Success(T value) =>
            new Validation<T>(true, value, default);

        public static Validation<T> Failure(string errorMessage = null) =>
            new Validation<T>(false, default, errorMessage);
    }

    /// <summary>
    /// Extension methods for <see cref="Validation{T}"/>.
    /// </summary>
    internal static class ValidationExtensions
    {
        /// <summary>
        /// Sequentially composes two validations, passing the value of the first validation to another
        /// validation-producing function if the first validation is a success.
        /// </summary>
        /// <typeparam name="T">The type of the first validation's success value.</typeparam>
        /// <typeparam name="U">The type of the second validation's success value.</typeparam>
        /// <param name="validation">The first validation.</param>
        /// <param name="bind">
        /// A function that takes the value of the first validation and returns a second validation.
        /// </param>
        /// <returns>
        /// The first validation if the first validation is a failure; otherwise, the return value of calling the bind
        /// function on the first validation's success value.
        /// </returns>
        internal static Validation<U> Bind<T, U>(this Validation<T> validation, Func<T, Validation<U>> bind) =>
            validation.IsFailure ? Validation<U>.Failure(validation.ErrorMessage) : bind(validation.Value);

        /// <summary>
        /// Converts an enumerable of validations into a validation of an enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the validation success values.</typeparam>
        /// <param name="validations">The validations to sequence.</param>
        /// <returns>
        /// A validation that contains an enumerable of the validation values if all of the validations are a success,
        /// or the first error message if one of the validations is a failure.
        /// </returns>
        internal static Validation<IEnumerable<T>> Sequence<T>(this IEnumerable<Validation<T>> validations) =>
            validations.All(validation => validation.IsSuccess)
            ? Validation<IEnumerable<T>>.Success(validations.Select(validation => validation.Value))
            : Validation<IEnumerable<T>>.Failure(validations.First(validation => validation.IsFailure).ErrorMessage);

        /// <summary>
        /// Calls the action on the validation value if the validation is a success.
        /// </summary>
        /// <typeparam name="T">The type of the validation's success value.</typeparam>
        /// <param name="validation">The validation.</param>
        /// <param name="onSuccess">The action to call if the validation is a success.</param>
        internal static void Then<T>(this Validation<T> validation, Action<T> onSuccess)
        {
            if (validation.IsSuccess)
            {
                onSuccess(validation.Value);
            }
        }
    }
}
