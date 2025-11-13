// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using Nerdbank.Streams;
using Xunit.Abstractions;

namespace MessagePack.Tests
{
    public partial class MessagePackReaderTests
    {
        private readonly ITestOutputHelper logger;

        public MessagePackReaderTests(ITestOutputHelper logger)
        {
            this.logger = logger;
        }

        private static readonly ReadOnlySequence<byte> StringEncodedAsFixStr = Encode((ref MessagePackWriter w) => w.Write("hi"));

        private delegate void WriterEncoder(ref MessagePackWriter writer);

        private static ReadOnlySequence<byte> Encode(WriterEncoder cb)
        {
            var sequence = new Sequence<byte>();
            var writer = new MessagePackWriter(sequence);
            cb(ref writer);
            writer.Flush();
            return sequence.AsReadOnlySequence;
        }
    }

    internal static class MessagePackWriterExtensions
    {
        internal static void WriteByte(ref this MessagePackWriter writer, byte value) => writer.WriteUInt8(value);

        internal static void WriteSByte(ref this MessagePackWriter writer, sbyte value) => writer.WriteInt8(value);
    }
}
