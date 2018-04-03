using System;
using ProtoBuf;

namespace IFX.Gluneon.MessageWriter
{

    [ProtoContract]
    public class Message
    {
        public Message() {
            
        }
        public Guid Id { get; set; }
        [ProtoMember(1)]
        public string Channel { get; set; }
        [ProtoMember(2)]
        public string Publisher { get; set; }
        [ProtoMember(3)]
        public string Protocol { get; set; }
        [ProtoMember(4)]
        public string Name { get; set; }
        [ProtoMember(5)]
        public string Unit { get; set; }
        [ProtoMember(6)]
        public double Value { get; set; }
        [ProtoMember(7)]
        public string StringValue { get; set; }
        [ProtoMember(8)]
        public bool BoolValue { get; set; }
        [ProtoMember(9)]
        public string DataValue { get; set; }
        [ProtoMember(10)]
        public double ValueSum { get; set; }
        [ProtoMember(11)]
        public double Time { get; set; }
        [ProtoMember(12)]
        public double UpdateTime { get; set; }
        [ProtoMember(13)]
        public string Link { get; set; }
    }
}
