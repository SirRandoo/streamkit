// MIT License
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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using StreamKit.Data.Abstractions;

namespace StreamKit.Api;

public record MutableRegistry<T>(IList<T>? AllRegistrants = default) : IRegistry<T> where T : class, IIdentifiable
{
    private readonly Dictionary<string, T> _allRegistrantsKeyed = new();

    /// <inheritdoc/>
    public IList<T> AllRegistrants { get; } = AllRegistrants ?? [];

    /// <inheritdoc/>
    public bool Register([NotNull] T obj)
    {
        if (_allRegistrantsKeyed.ContainsKey(obj.Id))
        {
            return false;
        }

        AllRegistrants.Add(obj);
        _allRegistrantsKeyed[obj.Id] = obj;

        return true;
    }

    /// <inheritdoc/>
    public bool Unregister(T obj)
    {
        if (!_allRegistrantsKeyed.ContainsKey(obj.Id))
        {
            return false;
        }

        bool listRemoved = AllRegistrants.Remove(obj);
        bool dictRemoved = _allRegistrantsKeyed.Remove(obj.Id);

        return listRemoved && dictRemoved;
    }

    /// <inheritdoc/>
    public T? Get(string id) => _allRegistrantsKeyed.GetValueOrDefault(id);
}
