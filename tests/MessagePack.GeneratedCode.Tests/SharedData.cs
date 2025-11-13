// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MessagePack;

namespace SharedData
{
    [MessagePackObject(true)]
    public class DefaultValueStringKeyClassWithoutExplicitConstructor
    {
        public const int Prop1Constant = 11;
        public const int Prop2Constant = 45;

        public int Prop1 { get; set; } = Prop1Constant;

        public int Prop2 { get; set; } = Prop2Constant;
    }

    [MessagePackObject(true)]
    public class DefaultValueStringKeyClassWithExplicitConstructor
    {
        public const int Prop2Constant = 1419;

        public int Prop1 { get; set; }

        public int Prop2 { get; set; }

        public DefaultValueStringKeyClassWithExplicitConstructor(int prop1)
        {
            Prop1 = prop1;
            Prop2 = Prop2Constant;
        }
    }

    [MessagePackObject(true)]
    public struct DefaultValueStringKeyStructWithExplicitConstructor
    {
        public const int Prop2Constant = 198;

        public int Prop1 { get; set; }

        public int Prop2 { get; set; }

        public DefaultValueStringKeyStructWithExplicitConstructor(int prop1)
        {
            Prop1 = prop1;
            Prop2 = Prop2Constant;
        }
    }

    [MessagePackObject]
    public class DefaultValueIntKeyClassWithoutExplicitConstructor
    {
        public const int Prop1Constant = 33;
        public const int Prop2Constant = -4;

        [Key(0)]
        public int Prop1 { get; set; } = Prop1Constant;

        [Key(1)]
        public int Prop2 { get; set; } = Prop2Constant;
    }

    [MessagePackObject]
    public class DefaultValueIntKeyClassWithExplicitConstructor
    {
        public const int Prop2Constant = -109;
        public const string Prop3Constant = "????????????????????????";
        public const string Prop4Constant = "Hello, world! To you, From me.";

        [Key(0)]
        public int Prop1 { get; set; }

        [Key(1)]
        public int Prop2 { get; set; }

        [Key(2)]
        public string Prop3 { get; set; }

        [Key(3)]
        public string Prop4 { get; set; }

        public DefaultValueIntKeyClassWithExplicitConstructor(int prop1)
        {
            Prop1 = prop1;
            Prop2 = Prop2Constant;
            Prop3 = Prop3Constant;
            Prop4 = Prop4Constant;
        }
    }

    [MessagePackObject]
    public struct DefaultValueIntKeyStructWithExplicitConstructor
    {
        public const int Prop2Constant = 31;

        [Key(0)]
        public int Prop1 { get; set; }

        [Key(1)]
        public int Prop2 { get; set; }

        public DefaultValueIntKeyStructWithExplicitConstructor(int prop1)
        {
            Prop1 = prop1;
            Prop2 = Prop2Constant;
        }
    }
}
