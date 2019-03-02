using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace TestProject
{
    public struct LayoutElement
    {
        public VertexAttribPointerType Type;
        public int Count;
        public bool Normalized;

        public static int GetSizeOfType(VertexAttribPointerType type)
        {
            switch (type)
            {
                case VertexAttribPointerType.Byte:
                    return sizeof(sbyte);
                case VertexAttribPointerType.UnsignedByte:
                    return sizeof(byte);
                case VertexAttribPointerType.Short:
                    return sizeof(short);
                case VertexAttribPointerType.UnsignedShort:
                    return sizeof(ushort);
                case VertexAttribPointerType.Int:
                    return sizeof(int);
                case VertexAttribPointerType.UnsignedInt:
                    return sizeof(uint);
                case VertexAttribPointerType.Float:
                    return sizeof(float);
                case VertexAttribPointerType.Double:
                    return sizeof(double);
                case VertexAttribPointerType.HalfFloat:
                case VertexAttribPointerType.Fixed:
                case VertexAttribPointerType.UnsignedInt2101010Rev:
                case VertexAttribPointerType.UnsignedInt10F11F11FRev:
                case VertexAttribPointerType.Int2101010Rev:
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
        
    public class BufferLayout
    {
        public List<LayoutElement> Elements { get; private set; } = new List<LayoutElement>();
        public int Stride { get; private set; }
            
        public void AddLayoutItem(VertexAttribPointerType type, int count, bool normalized = false)
        {
            Elements.Add(new LayoutElement(){Type = type, Count = count, Normalized = normalized});
            Stride += LayoutElement.GetSizeOfType(type) *count;
        }
    }
}