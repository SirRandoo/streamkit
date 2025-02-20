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

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace StreamKit.Shared.Serializers;

/// <summary>
///     An interface representing the minimum required class members for describing an object that
///     can save and load persistable data.
/// </summary>
public interface IDataSerializer
{
    /// <summary>Deserializes the data from the stream.</summary>
    /// <typeparam name="T">The type of the object that's being loaded from disk.</typeparam>
    /// <param name="stream">The stream to deserialize the data from.</param>
    /// <returns>
    ///     The data model deserialized from the stream, or the default value of the model if it
    ///     couldn't be deserialized.
    /// </returns>
    T? Deserialize<T>(Stream stream);

    /// <summary>Serializes the data to a stream.</summary>
    /// <typeparam name="T">The type of the object that's being saved to disk.</typeparam>
    /// <param name="data">The optional data model to serialize into the stream.</param>
    /// <param name="stream">The stream to serialize the data into.</param>
    void Serialize<T>(Stream stream, [DisallowNull] T data);

    /// <summary>Deserializes the data from the stream.</summary>
    /// <typeparam name="T">The type of the object that's being loaded from disk.</typeparam>
    /// <param name="stream">The stream to deserialize the data from.</param>
    /// <returns>
    ///     The data model deserialized from the stream, or the default value of the model if it
    ///     couldn't be deserialized.
    /// </returns>
    Task<T?> DeserializeAsync<T>(Stream stream);

    /// <summary>Serializes the data to a stream.</summary>
    /// <typeparam name="T">The type of the object that's being saved to disk.</typeparam>
    /// <param name="data">The data to serialize into the stream.</param>
    /// <param name="stream">The stream to serialize the data into.</param>
    Task SerializeAsync<T>(Stream stream, [DisallowNull] T data);
}
