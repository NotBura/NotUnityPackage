using System;
using Unity.Collections;

namespace NotBura.Packages
{
    public struct Context
        : IDisposable
    {
        public Frame Frame;
        public ByteCodeReader Reader;
        public Stack Stack;

        public void Dispose()
        {
            Stack.Dispose();
        }
    }

    public struct NotVM
        : IDisposable
    {
        private NativeArray<IntPtr> m_instructions;

        private NotVM(NativeArray<IntPtr> instructions)
        {
            m_instructions = instructions;
        }

        public void Dispose()
        {
            m_instructions.Dispose();
        }

        public static unsafe NotVM Create()
        {
            var instructions = new NativeArray<IntPtr>(16, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            instructions[0] = (IntPtr)(delegate*<ref Context, void>)&Operator.Return;
            instructions[1] = (IntPtr)(delegate*<ref Context, void>)&Operator.LoadConstant;
            instructions[2] = (IntPtr)(delegate*<ref Context, void>)&Operator.LoadArgument;
            instructions[3] = (IntPtr)(delegate*<ref Context, void>)&Operator.Equals;
            instructions[4] = (IntPtr)(delegate*<ref Context, void>)&Operator.AddI4;
            instructions[5] = (IntPtr)(delegate*<ref Context, void>)&Operator.SubstactI4;
            instructions[6] = (IntPtr)(delegate*<ref Context, void>)&Operator.MultiplyI4;
            instructions[7] = (IntPtr)(delegate*<ref Context, void>)&Operator.DivideI4;

            return new(instructions);
        }

        public unsafe int Execute(NativeArray<byte> instructions, NativeArray<byte> frame, NativeArray<byte> reader, ushort stack)
        {
            var table = m_instructions;
            var context = new Context
            {
                Frame = new(frame.ToArray()),
                Reader = new(reader.ToArray()),
                Stack = new(stack),
            };

            for (int i = 0; i < instructions.Length; ++i)
            {
                var pointer = (delegate*<ref Context, void>)table[instructions[i]];

                pointer(ref context);
            }

            var result = context.Stack.Pop<int>();

            context.Dispose();

            return result;
        }

        public (NativeArray<byte>, NativeArray<byte>, NativeArray<byte>) Compile(ReadOnlySpan<char> source)
        {
            var instructions = new NativeArray<byte>(6, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            instructions[0] = 1;
            instructions[1] = 2;
            instructions[2] = 2;
            instructions[3] = 5;
            instructions[4] = 7;
            instructions[5] = 0;

            var frame = new NativeArray<byte>(8, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            frame[0] = 0x01;
            frame[1] = 0x00;
            frame[2] = 0x00;
            frame[3] = 0x00;
            frame[4] = 0x03;
            frame[5] = 0x00;
            frame[6] = 0x00;
            frame[7] = 0x00;

            var reader = new NativeArray<byte>(6, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            reader[0] = 0x02;
            reader[1] = 0x00;
            reader[2] = 0x00;
            reader[3] = 0x00;
            reader[4] = 0x00;
            reader[5] = 0x01;

            return (instructions, frame, reader);
        }
    }
}
