﻿// Storm.NET - Simple Topologically Ordered Reactive Model
// Copyright © 2019 Storm.NET. All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace StormDotNet.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    internal class StormInput<T> : StormBase<T>, IStormInput<T>
    {
        public StormInput(IEqualityComparer<T>? comparer) : base(comparer)
        {
        }

        public void SetError(IStormToken token, StormError error)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));
            if (token.IsDisposed) throw new ArgumentException("Object is disposed.", nameof(token));
            if (error == null) throw new ArgumentNullException(nameof(error));

            if (IsError(error))
                return;

            token.Disposing += () =>
            {
                SetError(error);
                RaiseUpdateLeave(token, true);
            };

            RaiseUpdateEnter(token);
        }

        public void SetValue(IStormToken token, [AllowNull] [MaybeNull] T value)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));
            if (token.IsDisposed) throw new ArgumentException("Object is disposed.", nameof(token));

            if (IsValue(value))
                return;

            token.Disposing += () =>
            {
                SetValue(value);
                RaiseUpdateLeave(token, true);
            };

            RaiseUpdateEnter(token);
        }
    }
}