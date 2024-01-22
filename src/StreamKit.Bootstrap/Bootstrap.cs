﻿// MIT License
//
// Copyright (c) 2023 SirRandoo
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Serialization;
using RimWorld;
using SirRandoo.CommonLib.Entities;
using SirRandoo.CommonLib.Interfaces;
using UnityEngine;
using Verse;

namespace StreamKit.Bootstrap;

[StaticConstructorOnStartup]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
internal static class Bootstrap
{
    private static readonly string? NativeExtension = GetNativeExtension();
    private static readonly XmlSerializer Serializer = new(typeof(Corpus));
    private static readonly IRimLogger Logger = new RimThreadedLogger("StreamKit.BootLoader");
    private static readonly List<string> SpecialFiles = new();

    static Bootstrap()
    {
        if (string.IsNullOrEmpty(NativeExtension))
        {
            Logger.Error($"Bootstrap is running on an unsupported platform '{UnityData.platform.ToStringSafe()}'. Aborting...");

            return;
        }

        Application.quitting += CleanNativeFiles;

        Logger.Info($"StreamKit is running on the platform '{UnityData.platform.ToStringSafe()}'.");

        foreach (ModContentPack mod in LoadedModManager.RunningMods)
        {
            string corpusPath = Path.Combine(mod.RootDir, "Corpus.xml");

            if (!File.Exists(corpusPath))
            {
                continue;
            }

            LoadContent(mod, corpusPath);
        }
    }

    private static void CleanNativeFiles()
    {
        for (var index = 0; index < SpecialFiles.Count; index++)
        {
            string file = SpecialFiles[index];

            try
            {
                File.Delete(file);
            }
            catch (Exception e)
            {
                Logger.Error($"Could not clean file @ {file}. Any mod updates pending to this file will not go through.", e);
            }
        }
    }

    private static void LoadContent(ModContentPack mod, string corpusPath)
    {
        if (!File.Exists(corpusPath))
        {
            Logger.Error($"{mod.Name} requested that content be loaded, but doesn't have a corpus file in their root directory. Aborting...");

            return;
        }

        Corpus? corpus;

        using (FileStream stream = File.Open(corpusPath, FileMode.Open, FileAccess.Read))
        {
            corpus = Serializer.Deserialize(stream) as Corpus;
        }

        if (corpus is null)
        {
            Logger.Error($"Object within corpus file for {mod.Name} was malformed. Aborting...");

            return;
        }

        LoadResources(mod, corpus);
    }

    private static void LoadResources(ModContentPack mod, Corpus corpus)
    {
        foreach (ResourceBundle bundle in corpus.Resources)
        {
            LoadResourceBundle(mod, bundle);
        }
    }

    private static void LoadResourceBundle(ModContentPack mod, ResourceBundle bundle)
    {
        string path = GetPathFor(mod, bundle);

        if (!Directory.Exists(path))
        {
            Logger.Error($"The directory {path} doesn't exist, but was specified in {mod.Name}'s corpus. Aborting...");

            return;
        }

        foreach (Resource resource in bundle.Resources)
        {
            string resourceDir = Path.GetFullPath(Path.Combine(path, resource.Root));

            switch (resource.Type)
            {
                case ResourceType.Dll:
                    CopyNativeFile(resource, resourceDir);

                    break;
                case ResourceType.Assembly:
                    BootModLoader.LoadAssembly(mod, Path.Combine(path, resource.Root, $"{resource.Name}.dll"));

                    break;
                case ResourceType.NetStandardAssembly:
                    CopyStandardManagedFile(resource, resourceDir);

                    break;
            }
        }
    }

    private static void CopyNativeFile(Resource resource, string resourceDir)
    {
        var fileName = $"{resource.Name}.{NativeExtension}";
        string resourcePath = Path.Combine(resourceDir, $"{resource.Name}.{NativeExtension}");
        string destinationPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

        if (File.Exists(destinationPath))
        {
            return;
        }

        try
        {
            File.Copy(resourcePath, destinationPath);

            SpecialFiles.Add(destinationPath);
        }
        catch (Exception e)
        {
            Logger.Error($"Could not copy {fileName} to {destinationPath} (from {resourcePath}). Things will not work correctly.", e);
        }
    }


    private static void CopyStandardManagedFile(Resource resource, string resourceDir)
    {
        var fileName = $"{resource.Name}.dll";
        string resourcePath = Path.Combine(resourceDir, $"{resource.Name}.dll");
        string destinationPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

        if (File.Exists(destinationPath))
        {
            return;
        }

        try
        {
            File.Copy(resourcePath, destinationPath);

            SpecialFiles.Add(destinationPath);
        }
        catch (Exception e)
        {
            Logger.Error($"Could not copy {fileName} to {destinationPath} (from {resourcePath}). Things will not work correctly.", e);
        }
    }

    private static string GetPathFor(ModContentPack mod, ResourceBundle bundle)
    {
        string root = mod.RootDir;

        if (!string.IsNullOrEmpty(bundle.Root))
        {
            root = Path.Combine(root, bundle.Root);
        }

        if (!bundle.Versioned)
        {
            return root;
        }

        string withoutBuild = Path.Combine(root, VersionControl.CurrentVersionString);

        return Directory.Exists(withoutBuild) ? withoutBuild : Path.Combine(root, VersionControl.CurrentVersionStringWithoutBuild);
    }

    private static string? GetNativeExtension()
    {
        switch (UnityData.platform)
        {
            case RuntimePlatform.WindowsPlayer:
                return ".dll";
            case RuntimePlatform.LinuxPlayer:
                return ".so";
            case RuntimePlatform.OSXPlayer:
                return ".dylib";
            default:
                return null;
        }
    }
}
