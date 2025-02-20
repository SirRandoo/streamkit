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

using NetEscapades.EnumGenerators;

namespace StreamKit.Shared;

/// <summary>
///     An enum representing the various weapons that exist in RimWorld.
/// </summary>
[EnumExtensions]
public enum WeaponType
{
    /// <summary>
    ///     Represents an item that wasn't a weapon, but was classified as a
    ///     weapon by a different subsystem. You should generally report
    ///     this.
    /// </summary>
    None,

    /// <summary>
    ///     Represents a melee weapon available for purchase.
    /// </summary>
    Melee,

    /// <summary>
    ///     Represents a ranged weapon available for purchase.
    /// </summary>
    Ranged
}
