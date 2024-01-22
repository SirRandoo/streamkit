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

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using FluentResults;

namespace StreamKit.Api;

public record JsonDataSerializer<T>(JsonSerializerOptions? SerializerOptions) : IDataSerializer<T>
{
    /// <inheritdoc />
    public Result<T> Deserialize(Stream stream)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(stream)!;
        }
        catch (Exception e)
        {
            return new ExceptionalError("Could not deserialize json object from stream", e);
        }
    }

    /// <inheritdoc />
    public Result Serialize(Stream stream, [DisallowNull] T data)
    {
        try
        {
            JsonSerializer.Serialize(stream, data, SerializerOptions);

            return Result.Ok();
        }
        catch (Exception e)
        {
            return new ExceptionalError("Could not serialize json object into stream.", e);
        }
    }

    /// <inheritdoc />
    public async Task<Result<T>> DeserializeAsync(Stream stream)
    {
        try
        {
            return (await JsonSerializer.DeserializeAsync<T>(stream))!;
        }
        catch (Exception e)
        {
            return new ExceptionalError("Could not deserialize json object from stream", e);
        }
    }

    /// <inheritdoc />
    public async Task<Result> SerializeAsync(Stream stream, [DisallowNull] T data)
    {
        try
        {
            await JsonSerializer.SerializeAsync(stream, data);

            return Result.Ok();
        }
        catch (Exception e)
        {
            return new ExceptionalError("Could not serialize json object into stream", e);
        }
    }
}
