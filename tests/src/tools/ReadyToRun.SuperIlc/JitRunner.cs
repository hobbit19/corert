﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// No-op runner keeping the original IL assemblies to be directly run with full jitting.
/// </summary>
class JitRunner : CompilerRunner
{
    public override CompilerIndex Index => CompilerIndex.Jit;

    protected override string CompilerFileName => "clrjit.dll";

    public JitRunner(IEnumerable<string> referenceFolders) : base(null, referenceFolders) { }

    /// <summary>
    /// JIT runner has no compilation process as it doesn't transform the source IL code in any manner.
    /// </summary>
    /// <returns></returns>
    public override ProcessInfo CompilationProcess(string outputRoot, string assemblyFileName)
    {
        File.Copy(assemblyFileName, GetOutputFileName(outputRoot, assemblyFileName), overwrite: true);
        return null;
    }

    public override ProcessInfo ExecutionProcess(string outputRoot, string appPath, IEnumerable<string> modules, IEnumerable<string> folders, string coreRunPath, bool noEtw)
    {
        ProcessInfo processInfo = base.ExecutionProcess(outputRoot, appPath, modules, folders, coreRunPath, noEtw);
        processInfo.EnvironmentOverrides["COMPLUS_ReadyToRun"] = "0";
        return processInfo;
    }

    protected override IEnumerable<string> BuildCommandLineArguments(string assemblyFileName, string outputFileName)
    {
        // This should never get called as the overridden CompilationProcess returns null
        throw new NotImplementedException();
    }

}
