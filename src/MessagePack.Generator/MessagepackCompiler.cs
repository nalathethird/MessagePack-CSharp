// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using ConsoleAppFramework;
using Microsoft.Build.Locator;
using Microsoft.Build.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MessagePack.Generator
{
    public class MessagepackCompiler
    {
        private static async Task Main(string[] args)
        {
            var instance = MSBuildLocator.RegisterDefaults();
            AssemblyLoadContext.Default.Resolving += (assemblyLoadContext, assemblyName) =>
            {
                var path = Path.Combine(instance.MSBuildPath, assemblyName.Name + ".dll");
                if (File.Exists(path))
                {
                    return assemblyLoadContext.LoadFromAssemblyPath(path);
                }

                return null;
            };

            var app = ConsoleApp.Create();
            app.Add("", RunAsync);
            await app.RunAsync(args);
        }

        /// <summary>
        /// MessagePack Code Generator.
        /// </summary>
        /// <param name="input">-i, Input path to MSBuild project file or the directory containing Unity source files.</param>
        /// <param name="output">-o, Output file path(.cs) or directory (multiple generate file).</param>
        /// <param name="conditionalSymbol">-c, Conditional compiler symbols, split with ','. Ignored if a project file is specified for input.</param>
        /// <param name="resolverName">-r, Set resolver name.</param>
        /// <param name="namespace">-n, Set namespace root name.</param>
        /// <param name="useMapMode">-m, Force use map mode serialization.</param>
        /// <param name="multipleIfDirectiveOutputSymbols">-ms, Generate #if-- files by symbols, split with ','.</param>
        /// <param name="externalIgnoreTypeNames">-ei, Ignore type names.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task RunAsync(
            string input,
            string output,
            string? conditionalSymbol = null,
            string resolverName = "GeneratedResolver",
            string @namespace = "MessagePack",
            bool useMapMode = false,
            string? multipleIfDirectiveOutputSymbols = null,
            string[]? externalIgnoreTypeNames = null,
            CancellationToken cancellationToken = default)
        {
            Workspace? workspace = null;
            try
            {
                Compilation compilation;
                if (Directory.Exists(input))
                {
                    string[]? conditionalSymbols = conditionalSymbol?.Split(',');
                    compilation = await PseudoCompilation.CreateFromDirectoryAsync(input, conditionalSymbols, cancellationToken);
                }
                else
                {
                    (workspace, compilation) = await OpenMSBuildProjectAsync(input, cancellationToken);
                }

                await new MessagePackCompiler.CodeGenerator(x => Console.WriteLine(x), cancellationToken)
                    .GenerateFileAsync(
                        compilation,
                        output,
                        resolverName,
                        @namespace,
                        useMapMode,
                        multipleIfDirectiveOutputSymbols,
                        externalIgnoreTypeNames).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                await Console.Error.WriteLineAsync("Canceled");
                throw;
            }
            finally
            {
                workspace?.Dispose();
            }
        }

        private static async Task<(Workspace Workspace, Compilation Compilation)> OpenMSBuildProjectAsync(string projectPath, CancellationToken cancellationToken)
        {
            var workspace = MSBuildWorkspace.Create();
            try
            {
                var logger = new ConsoleLogger(Microsoft.Build.Framework.LoggerVerbosity.Quiet);
                var project = await workspace.OpenProjectAsync(projectPath, logger, null, cancellationToken);
                var compilation = await project.GetCompilationAsync(cancellationToken);
                if (compilation is null)
                {
                    throw new NotSupportedException("The project does not support creating Compilation.");
                }

                return (workspace, compilation);
            }
            catch
            {
                workspace.Dispose();
                throw;
            }
        }
    }
}
