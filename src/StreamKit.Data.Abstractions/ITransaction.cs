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

namespace StreamKit.Data.Abstractions;

/// <summary>
///     Represents a transaction made by a viewer.
///     <br/>
///     <br/>
///     Transactions serve as a historical record of a viewer's purchases
///     for use in the reputation system. As the reputation system is
///     large and complex, you should refer to the documentation for it
///     for further information.
/// </summary>
public interface ITransaction : IIdentifiable
{
    /// <summary>
    ///     The date and time the product was purchased.
    /// </summary>
    DateTime PurchasedAt { get; }

    /// <summary>
    ///     The type of the product.
    /// </summary>
    ProductType Type { get; set; }

    /// <summary>
    ///     The amount of points a viewer had to pay in order to purchase
    ///     this product.
    /// </summary>
    long Price { get; set; }

    /// <summary>
    ///     The morality of purchasing the product.
    /// </summary>
    Morality Morality { get; set; }

    /// <summary>
    ///     Whether the transaction was refunded to the viewer.
    /// </summary>
    bool Refunded { get; set; }
}
