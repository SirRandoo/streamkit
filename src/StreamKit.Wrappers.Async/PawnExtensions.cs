﻿// MIT License
// 
// Copyright (c) 2022 SirRandoo
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
using System.Threading.Tasks;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace StreamKit.Wrappers.Async
{
    /// <summary>
    ///     A set of wrappers around the synchronous, unsafe methods within
    ///     the <see cref="Pawn"/> class.
    /// </summary>
    /// <remarks>
    ///     Usage of the extensions provided by this class are to be used
    ///     with care as RimWorld was not designed with asynchronous code in
    ///     mind.
    /// </remarks>
    public static class PawnExtensions
    {
        /// <inheritdoc cref="ParentRelationUtility.SetFather"/>
        public static async Task SetFatherAsync([NotNull] this Pawn pawn, Pawn father)
        {
            await TaskExtensions.OnMainAsync(ParentRelationUtility.SetFather, pawn, father);
        }

        /// <inheritdoc cref="ParentRelationUtility.SetMother"/>
        public static async Task SetMotherAsync([NotNull] this Pawn pawn, Pawn mother)
        {
            await TaskExtensions.OnMainAsync(ParentRelationUtility.SetMother, pawn, mother);
        }

        /// <inheritdoc cref="Pawn.SetFactionDirect"/>
        public static async Task SetFactionDirectAsync([NotNull] this Pawn pawn, Faction faction)
        {
            await TaskExtensions.OnMainAsync(pawn.SetFactionDirect, faction);
        }

        /// <inheritdoc cref="Pawn.SetFaction"/>
        public static async Task SetFactionAsync([NotNull] this Pawn pawn, Faction faction, [CanBeNull] Pawn recruiter = null)
        {
            await TaskExtensions.OnMainAsync(pawn.SetFaction, faction, recruiter);
        }

        /// <inheritdoc cref="Pawn.Kill"/>
        public static async Task KillAsync([NotNull] this Pawn pawn, DamageInfo? info, [CanBeNull] Hediff culprit = null)
        {
            await TaskExtensions.OnMainAsync(pawn.Kill, info, culprit);
        }

        /// <inheritdoc cref="PawnRelationUtility.GetRelations"/>
        public static async Task<List<PawnRelationDef>> GetRelationsAsync([NotNull] this Pawn pawn, Pawn other)
        {
            List<PawnRelationDef> GetRelations(Pawn subject, Pawn target)
            {
                var container = new List<PawnRelationDef>();

                foreach (PawnRelationDef relation in subject.GetRelations(target))
                {
                    container.Add(relation);
                }

                return container;
            }

            return await TaskExtensions.OnMainAsync(GetRelations, pawn, other);
        }
    }
}
