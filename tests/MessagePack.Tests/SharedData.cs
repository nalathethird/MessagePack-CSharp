// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MessagePack;
using System;

namespace SharedData
{
    [MessagePackObject]
    public class FirstSimpleData : IEquatable<FirstSimpleData>
    {
        [System.Runtime.Serialization.DataMember(Order = 0)]
        public int Prop1 { get; set; }

        [System.Runtime.Serialization.DataMember(Order = 1)]
        public string Prop2 { get; set; }

        [System.Runtime.Serialization.DataMember(Order = 2)]
        public int Prop3 { get; set; }

        public bool Equals(FirstSimpleData other)
        {
            return other != null
                && this.Prop1 == other.Prop1
                && this.Prop2 == other.Prop2
                && this.Prop3 == other.Prop3;
        }
    }
}
