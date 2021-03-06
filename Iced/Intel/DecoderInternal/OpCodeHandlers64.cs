﻿/*
    Copyright (C) 2018 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
*/

#if !NO_DECODER64 && !NO_DECODER
using System.Diagnostics;

namespace Iced.Intel.DecoderInternal.OpCodeHandlers64 {
	sealed class OpCodeHandler_Ev_Iz : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly HandlerFlags flags;

		public OpCodeHandler_Ev_Iz(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public OpCodeHandler_Ev_Iz(Code code16, Code code32, Code code64, HandlerFlags flags) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.flags = flags;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.Op0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.Op0Register = (int)index + Register.RAX;
				else
					instruction.Op0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalMemorySize = MemorySize.UInt32;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalMemorySize = MemorySize.UInt64;
				else
					instruction.InternalMemorySize = MemorySize.UInt16;
				decoder.ReadOpMem_m64(ref instruction);
				if ((flags & HandlerFlags.XacquireRelease) != 0)
					decoder.SetXacquireRelease(ref instruction, flags);
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				instruction.InternalOp2Kind = OpKind.Immediate32;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				instruction.InternalOp2Kind = OpKind.Immediate32to64;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else {
				instruction.InternalCode = code16;
				instruction.InternalOp2Kind = OpKind.Immediate16;
				instruction.InternalImmediate16 = decoder.ReadUInt16();
			}
		}
	}

	sealed class OpCodeHandler_Ev_Ib : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly HandlerFlags flags;

		public OpCodeHandler_Ev_Ib(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public OpCodeHandler_Ev_Ib(Code code16, Code code32, Code code64, HandlerFlags flags) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.flags = flags;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.Op0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.Op0Register = (int)index + Register.RAX;
				else
					instruction.Op0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalMemorySize = MemorySize.UInt32;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalMemorySize = MemorySize.UInt64;
				else
					instruction.InternalMemorySize = MemorySize.UInt16;
				decoder.ReadOpMem_m64(ref instruction);
				if ((flags & HandlerFlags.XacquireRelease) != 0)
					decoder.SetXacquireRelease(ref instruction, flags);
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				instruction.InternalOp2Kind = OpKind.Immediate8to32;
				instruction.InternalImmediate8 = decoder.ReadIb();
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				instruction.InternalOp2Kind = OpKind.Immediate8to64;
				instruction.InternalImmediate8 = decoder.ReadIb();
			}
			else {
				instruction.InternalCode = code16;
				instruction.InternalOp2Kind = OpKind.Immediate8to16;
				instruction.InternalImmediate8 = decoder.ReadIb();
			}
		}
	}

	sealed class OpCodeHandler_Ev_Ib2 : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly bool isSigned;
		readonly HandlerFlags flags;

		public OpCodeHandler_Ev_Ib2(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			isSigned = false;
		}

		public OpCodeHandler_Ev_Ib2(Code code16, Code code32, Code code64, bool isSigned) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.isSigned = isSigned;
		}

		public OpCodeHandler_Ev_Ib2(Code code16, Code code32, Code code64, HandlerFlags flags) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			isSigned = false;
			this.flags = flags;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.Op0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.Op0Register = (int)index + Register.RAX;
				else
					instruction.Op0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if (isSigned) {
					if (state.operandSize == OpSize.Size32)
						instruction.InternalMemorySize = MemorySize.Int32;
					else if (state.operandSize == OpSize.Size64)
						instruction.InternalMemorySize = MemorySize.Int64;
					else
						instruction.InternalMemorySize = MemorySize.Int16;
				}
				else {
					if (state.operandSize == OpSize.Size32)
						instruction.InternalMemorySize = MemorySize.UInt32;
					else if (state.operandSize == OpSize.Size64)
						instruction.InternalMemorySize = MemorySize.UInt64;
					else
						instruction.InternalMemorySize = MemorySize.UInt16;
				}
				decoder.ReadOpMem_m64(ref instruction);
				if ((flags & HandlerFlags.XacquireRelease) != 0)
					decoder.SetXacquireRelease(ref instruction, flags);
			}
			instruction.InternalOp2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_Ev_1 : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly bool isSigned;

		public OpCodeHandler_Ev_1(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			isSigned = false;
		}

		public OpCodeHandler_Ev_1(Code code16, Code code32, Code code64, bool isSigned) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.isSigned = isSigned;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.Op0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.Op0Register = (int)index + Register.RAX;
				else
					instruction.Op0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if (isSigned) {
					if (state.operandSize == OpSize.Size32)
						instruction.InternalMemorySize = MemorySize.Int32;
					else if (state.operandSize == OpSize.Size64)
						instruction.InternalMemorySize = MemorySize.Int64;
					else
						instruction.InternalMemorySize = MemorySize.Int16;
				}
				else {
					if (state.operandSize == OpSize.Size32)
						instruction.InternalMemorySize = MemorySize.UInt32;
					else if (state.operandSize == OpSize.Size64)
						instruction.InternalMemorySize = MemorySize.UInt64;
					else
						instruction.InternalMemorySize = MemorySize.UInt16;
				}
				decoder.ReadOpMem_m64(ref instruction);
			}
			instruction.InternalOp2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = 1;
			state.flags |= StateFlags.NoImm;
		}
	}

	sealed class OpCodeHandler_Ev_CL : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly bool isSigned;

		public OpCodeHandler_Ev_CL(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			isSigned = false;
		}

		public OpCodeHandler_Ev_CL(Code code16, Code code32, Code code64, bool isSigned) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.isSigned = isSigned;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.Op0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.Op0Register = (int)index + Register.RAX;
				else
					instruction.Op0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if (isSigned) {
					if (state.operandSize == OpSize.Size32)
						instruction.InternalMemorySize = MemorySize.Int32;
					else if (state.operandSize == OpSize.Size64)
						instruction.InternalMemorySize = MemorySize.Int64;
					else
						instruction.InternalMemorySize = MemorySize.Int16;
				}
				else {
					if (state.operandSize == OpSize.Size32)
						instruction.InternalMemorySize = MemorySize.UInt32;
					else if (state.operandSize == OpSize.Size64)
						instruction.InternalMemorySize = MemorySize.UInt64;
					else
						instruction.InternalMemorySize = MemorySize.UInt16;
				}
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = Register.CL;
		}
	}

	sealed class OpCodeHandler_Ev : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly bool isSigned;
		readonly HandlerFlags flags;

		public OpCodeHandler_Ev(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			isSigned = false;
		}

		public OpCodeHandler_Ev(Code code16, Code code32, Code code64, bool isSigned) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.isSigned = isSigned;
		}

		public OpCodeHandler_Ev(Code code16, Code code32, Code code64, HandlerFlags flags) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			isSigned = false;
			this.flags = flags;
		}

		public OpCodeHandler_Ev(Code code16, Code code32, Code code64, bool isSigned, HandlerFlags flags) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.isSigned = isSigned;
			this.flags = flags;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			instruction.InternalOpCount = 1;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.Op0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.Op0Register = (int)index + Register.RAX;
				else
					instruction.Op0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if (isSigned) {
					if (state.operandSize == OpSize.Size32)
						instruction.InternalMemorySize = MemorySize.Int32;
					else if (state.operandSize == OpSize.Size64)
						instruction.InternalMemorySize = MemorySize.Int64;
					else
						instruction.InternalMemorySize = MemorySize.Int16;
				}
				else {
					if (state.operandSize == OpSize.Size32)
						instruction.InternalMemorySize = MemorySize.UInt32;
					else if (state.operandSize == OpSize.Size64)
						instruction.InternalMemorySize = MemorySize.UInt64;
					else
						instruction.InternalMemorySize = MemorySize.UInt16;
				}
				decoder.ReadOpMem_m64(ref instruction);
				if ((flags & HandlerFlags.XacquireRelease) != 0)
					decoder.SetXacquireRelease(ref instruction, flags);
			}
		}
	}

	sealed class OpCodeHandler_Rv : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Rv(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.AX;
			}
			instruction.InternalOpCount = 1;
			if (state.mod != 3)
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_Rv_32_64 : OpCodeHandlerModRM {
		readonly Code code64;

		public OpCodeHandler_Rv_32_64(Code code32, Code code64) => this.code64 = code64;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code64;
			instruction.InternalOpCount = 1;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_Ev_REXW : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;
		readonly MemorySize memSize32;
		readonly MemorySize memSize64;

		public OpCodeHandler_Ev_REXW(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
			memSize32 = MemorySize.Unknown;
			memSize64 = MemorySize.Unknown;
		}

		public OpCodeHandler_Ev_REXW(Code code32, Code code64, MemorySize memSize32, MemorySize memSize64) {
			this.code32 = code32;
			this.code64 = code64;
			this.memSize32 = memSize32;
			this.memSize64 = memSize64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code32;
			instruction.InternalOpCount = 1;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.rm + state.extraBaseRegisterBase) + ((state.flags & StateFlags.W) != 0 ? Register.RAX : Register.EAX);
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if ((state.flags & StateFlags.W) != 0)
					instruction.InternalMemorySize = memSize64;
				else
					instruction.InternalMemorySize = memSize32;
				decoder.ReadOpMem_m64(ref instruction);
				if (memSize32 == MemorySize.Unknown)
					decoder.SetInvalidInstruction();
			}
		}
	}

	sealed class OpCodeHandler_Evj : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Evj(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 1;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = MemorySize.QwordOffset;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Ep : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Ep(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			instruction.InternalOpCount = 1;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if (state.operandSize == OpSize.Size64)
					instruction.InternalMemorySize = MemorySize.SegPtr64;
				else if (state.operandSize == OpSize.Size32)
					instruction.InternalMemorySize = MemorySize.SegPtr32;
				else
					instruction.InternalMemorySize = MemorySize.SegPtr16;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Evw : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Evw(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			instruction.InternalOpCount = 1;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.Op0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.Op0Register = (int)index + Register.RAX;
				else
					instruction.Op0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = MemorySize.UInt16;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Ew : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Ew(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			instruction.InternalOpCount = 1;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.Op0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.Op0Register = (int)index + Register.RAX;
				else
					instruction.Op0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = MemorySize.UInt16;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Ms : OpCodeHandlerModRM {
		readonly Code code64;

		public OpCodeHandler_Ms(Code code16, Code code32, Code code64) => this.code64 = code64;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code64;
			instruction.InternalOpCount = 1;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = MemorySize.Fword10;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ev : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly bool isSigned;

		public OpCodeHandler_Gv_Ev(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			isSigned = false;
		}

		public OpCodeHandler_Gv_Ev(Code code16, Code code32, Code code64, bool isSigned) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.isSigned = isSigned;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.Op1Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.Op1Register = (int)index + Register.RAX;
				else
					instruction.Op1Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if (isSigned) {
					if (state.operandSize == OpSize.Size32)
						instruction.InternalMemorySize = MemorySize.Int32;
					else if (state.operandSize == OpSize.Size64)
						instruction.InternalMemorySize = MemorySize.Int64;
					else
						instruction.InternalMemorySize = MemorySize.Int16;
				}
				else {
					if (state.operandSize == OpSize.Size32)
						instruction.InternalMemorySize = MemorySize.UInt32;
					else if (state.operandSize == OpSize.Size64)
						instruction.InternalMemorySize = MemorySize.UInt64;
					else
						instruction.InternalMemorySize = MemorySize.UInt16;
				}
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ev3 : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_Ev3(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.Op1Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.Op1Register = (int)index + Register.RAX;
				else
					instruction.Op1Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = MemorySize.UInt16;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ev2 : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_Ev2(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size16)
					instruction.Op1Register = (int)index + Register.AX;
				else
					instruction.Op1Register = (int)index + Register.EAX;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if (state.operandSize == OpSize.Size16)
					instruction.InternalMemorySize = MemorySize.Int16;
				else
					instruction.InternalMemorySize = MemorySize.Int32;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Rq_Cq : OpCodeHandlerModRM {
		readonly Code code;
		readonly Register baseReg;

		public OpCodeHandler_Rq_Cq(Code code, Register baseReg) {
			this.code = code;
			this.baseReg = baseReg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
		}
	}

	sealed class OpCodeHandler_Cq_Rq : OpCodeHandlerModRM {
		readonly Code code;
		readonly Register baseReg;

		public OpCodeHandler_Cq_Rq(Code code, Register baseReg) {
			this.code = code;
			this.baseReg = baseReg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
		}
	}

	sealed class OpCodeHandler_Jb : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_Jb(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 1;
			instruction.InternalOp1Kind = OpKind.NearBranch64;
			instruction.NearBranch64Target = (ulong)(sbyte)decoder.ReadByte() + decoder.GetCurrentInstructionPointer64();
		}
	}

	sealed class OpCodeHandler_Jx : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Jx(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 1;
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				instruction.InternalOp1Kind = OpKind.NearBranch64;
				instruction.NearBranch64Target = (ulong)(int)decoder.ReadUInt32() + decoder.GetCurrentInstructionPointer64();
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				instruction.InternalOp1Kind = OpKind.NearBranch64;
				instruction.NearBranch64Target = (ulong)(int)decoder.ReadUInt32() + decoder.GetCurrentInstructionPointer64();
			}
			else {
				Debug.Assert(state.operandSize == OpSize.Size16);
				instruction.InternalCode = code16;
				instruction.InternalOp1Kind = OpKind.NearBranch64;
				instruction.NearBranch64Target = (ulong)(short)decoder.ReadUInt16() + decoder.GetCurrentInstructionPointer64();
			}
		}
	}

	sealed class OpCodeHandler_Jz : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_Jz(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 1;
			instruction.InternalOp1Kind = OpKind.NearBranch64;
			instruction.NearBranch64Target = (ulong)(int)decoder.ReadUInt32() + decoder.GetCurrentInstructionPointer64();
		}
	}

	sealed class OpCodeHandler_Jb2 : OpCodeHandler {
		readonly Code code64_32;
		readonly Code code64_64;

		public OpCodeHandler_Jb2(Code code64_32, Code code64_64) {
			this.code64_32 = code64_32;
			this.code64_64 = code64_64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.addressSize == OpSize.Size64)
				instruction.InternalCode = code64_64;
			else
				instruction.InternalCode = code64_32;
			instruction.InternalOpCount = 1;
			instruction.InternalOp1Kind = OpKind.NearBranch64;
			instruction.NearBranch64Target = (ulong)(sbyte)decoder.ReadByte() + decoder.GetCurrentInstructionPointer64();
		}
	}

	sealed class OpCodeHandler_PushOpSizeReg : OpCodeHandler {
		readonly Code code16;
		readonly Code code64;
		readonly Register reg;

		public OpCodeHandler_PushOpSizeReg(Code code16, Code code64, Register reg) {
			this.code16 = code16;
			this.code64 = code64;
			this.reg = reg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize != OpSize.Size16)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			instruction.InternalOpCount = 1;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = reg;
		}
	}

	sealed class OpCodeHandler_PushEv : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code64;

		public OpCodeHandler_PushEv(Code code16, Code code64) {
			this.code16 = code16;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize != OpSize.Size16)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			instruction.InternalOpCount = 1;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize != OpSize.Size16)
					instruction.Op0Register = (int)index + Register.RAX;
				else
					instruction.Op0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if (state.operandSize != OpSize.Size16)
					instruction.InternalMemorySize = MemorySize.UInt64;
				else
					instruction.InternalMemorySize = MemorySize.UInt16;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Ev_Gv : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly HandlerFlags flags;

		public OpCodeHandler_Ev_Gv(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public OpCodeHandler_Ev_Gv(Code code16, Code code32, Code code64, HandlerFlags flags) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.flags = flags;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.Op0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.Op0Register = (int)index + Register.RAX;
				else
					instruction.Op0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalMemorySize = MemorySize.UInt32;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalMemorySize = MemorySize.UInt64;
				else
					instruction.InternalMemorySize = MemorySize.UInt16;
				decoder.ReadOpMem_m64(ref instruction);
				if ((flags & HandlerFlags.XacquireRelease) != 0)
					decoder.SetXacquireRelease(ref instruction, flags);
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
		}
	}

	sealed class OpCodeHandler_Ev_Gv_32_64 : OpCodeHandlerModRM {
		readonly Code code64;

		public OpCodeHandler_Ev_Gv_32_64(Code code32, Code code64) => this.code64 = code64;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = MemorySize.UInt64;
				decoder.ReadOpMem_m64(ref instruction);
			}
			instruction.InternalCode = code64;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
		}
	}

	sealed class OpCodeHandler_Ev_Gv_Ib : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Ev_Gv_Ib(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 3;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.Op0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.Op0Register = (int)index + Register.RAX;
				else
					instruction.Op0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalMemorySize = MemorySize.UInt32;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalMemorySize = MemorySize.UInt64;
				else
					instruction.InternalMemorySize = MemorySize.UInt16;
				decoder.ReadOpMem_m64(ref instruction);
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_Ev_Gv_CL : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Ev_Gv_CL(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 3;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.Op0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.Op0Register = (int)index + Register.RAX;
				else
					instruction.Op0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalMemorySize = MemorySize.UInt32;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalMemorySize = MemorySize.UInt64;
				else
					instruction.InternalMemorySize = MemorySize.UInt16;
				decoder.ReadOpMem_m64(ref instruction);
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.Op2Register = Register.CL;
		}
	}

	sealed class OpCodeHandler_Gv_Mp : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_Mp(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalMemorySize = MemorySize.SegPtr32;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalMemorySize = MemorySize.SegPtr64;
				else
					instruction.InternalMemorySize = MemorySize.SegPtr16;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gv_Eb : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly MemorySize memSize;

		public OpCodeHandler_Gv_Eb(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			memSize = MemorySize.UInt8;
		}

		public OpCodeHandler_Gv_Eb(Code code16, Code code32, Code code64, MemorySize memSize) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3) {
				uint index = state.rm + state.extraBaseRegisterBase;
				if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ew : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly MemorySize memSize;

		public OpCodeHandler_Gv_Ew(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			memSize = MemorySize.UInt16;
		}

		public OpCodeHandler_Gv_Ew(Code code16, Code code32, Code code64, MemorySize memSize) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.AX;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_PushSimple2 : OpCodeHandler {
		readonly Code code16;
		readonly Code code64;

		public OpCodeHandler_PushSimple2(Code code16, Code code64) {
			this.code16 = code16;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize != OpSize.Size16)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
		}
	}

	sealed class OpCodeHandler_Simple2 : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Simple2(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
		}
	}

	sealed class OpCodeHandler_Simple2Iw : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Simple2Iw(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			instruction.InternalOpCount = 1;
			instruction.InternalOp1Kind = OpKind.Immediate16;
			instruction.InternalImmediate16 = decoder.ReadUInt16();
		}
	}

	sealed class OpCodeHandler_Simple3 : OpCodeHandler {
		readonly Code code16;
		readonly Code code64;

		public OpCodeHandler_Simple3(Code code16, Code code64) {
			this.code16 = code16;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize != OpSize.Size16)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
		}
	}

	sealed class OpCodeHandler_Simple5 : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Simple5(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.addressSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else if (state.addressSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else
				instruction.InternalCode = code16;
		}
	}

	sealed class OpCodeHandler_Simple4 : OpCodeHandler {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Simple4(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code32;
		}
	}

	sealed class OpCodeHandler_PushSimpleReg : OpCodeHandler {
		readonly Code code16_lo;
		readonly Code code16_hi;
		readonly Code code64_lo;
		readonly Code code64_hi;
		readonly Register reg16_lo;
		readonly Register reg16_hi;
		readonly Register reg64_lo;
		readonly Register reg64_hi;

		public OpCodeHandler_PushSimpleReg(Code code16_lo, Code code16_hi, Code code64_lo, Code code64_hi, Register reg16_lo, Register reg16_hi, Register reg64_lo, Register reg64_hi) {
			this.code16_lo = code16_lo;
			this.code16_hi = code16_hi;
			this.code64_lo = code64_lo;
			this.code64_hi = code64_hi;
			this.reg16_lo = reg16_lo;
			this.reg16_hi = reg16_hi;
			this.reg64_lo = reg64_lo;
			this.reg64_hi = reg64_hi;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 1;
			if (state.operandSize != OpSize.Size16) {
				if (state.extraBaseRegisterBase == 0) {
					instruction.InternalCode = code64_lo;
					Debug.Assert(OpKind.Register == 0);
					//instruction.InternalOp0Kind = OpKind.Register;
					instruction.Op0Register = reg64_lo;
				}
				else {
					instruction.InternalCode = code64_hi;
					Debug.Assert(OpKind.Register == 0);
					//instruction.InternalOp0Kind = OpKind.Register;
					instruction.Op0Register = reg64_hi;
				}
			}
			else {
				if (state.extraBaseRegisterBase == 0) {
					instruction.InternalCode = code16_lo;
					Debug.Assert(OpKind.Register == 0);
					//instruction.InternalOp0Kind = OpKind.Register;
					instruction.Op0Register = reg16_lo;
				}
				else {
					instruction.InternalCode = code16_hi;
					Debug.Assert(OpKind.Register == 0);
					//instruction.InternalOp0Kind = OpKind.Register;
					instruction.Op0Register = reg16_hi;
				}
			}
		}
	}

	sealed class OpCodeHandler_SimpleReg : OpCodeHandler {
		readonly int index;

		public OpCodeHandler_SimpleReg(int index) {
			Debug.Assert(0 <= index && index <= 7);
			this.index = index;
		}

		static readonly Code[,] codes = new Code[3, 16] {
			{
				Code.Bswap_AX,
				Code.Bswap_CX,
				Code.Bswap_DX,
				Code.Bswap_BX,
				Code.Bswap_SP,
				Code.Bswap_BP,
				Code.Bswap_SI,
				Code.Bswap_DI,
				Code.Bswap_R8W,
				Code.Bswap_R9W,
				Code.Bswap_R10W,
				Code.Bswap_R11W,
				Code.Bswap_R12W,
				Code.Bswap_R13W,
				Code.Bswap_R14W,
				Code.Bswap_R15W,
			},
			{
				Code.Bswap_EAX,
				Code.Bswap_ECX,
				Code.Bswap_EDX,
				Code.Bswap_EBX,
				Code.Bswap_ESP,
				Code.Bswap_EBP,
				Code.Bswap_ESI,
				Code.Bswap_EDI,
				Code.Bswap_R8D,
				Code.Bswap_R9D,
				Code.Bswap_R10D,
				Code.Bswap_R11D,
				Code.Bswap_R12D,
				Code.Bswap_R13D,
				Code.Bswap_R14D,
				Code.Bswap_R15D,
			},
			{
				Code.Bswap_RAX,
				Code.Bswap_RCX,
				Code.Bswap_RDX,
				Code.Bswap_RBX,
				Code.Bswap_RSP,
				Code.Bswap_RBP,
				Code.Bswap_RSI,
				Code.Bswap_RDI,
				Code.Bswap_R8,
				Code.Bswap_R9,
				Code.Bswap_R10,
				Code.Bswap_R11,
				Code.Bswap_R12,
				Code.Bswap_R13,
				Code.Bswap_R14,
				Code.Bswap_R15,
			},
		};

		static readonly Register[,] registers = new Register[3, 16] {
			{
				Register.AX,
				Register.CX,
				Register.DX,
				Register.BX,
				Register.SP,
				Register.BP,
				Register.SI,
				Register.DI,
				Register.R8W,
				Register.R9W,
				Register.R10W,
				Register.R11W,
				Register.R12W,
				Register.R13W,
				Register.R14W,
				Register.R15W,
			},
			{
				Register.EAX,
				Register.ECX,
				Register.EDX,
				Register.EBX,
				Register.ESP,
				Register.EBP,
				Register.ESI,
				Register.EDI,
				Register.R8D,
				Register.R9D,
				Register.R10D,
				Register.R11D,
				Register.R12D,
				Register.R13D,
				Register.R14D,
				Register.R15D,
			},
			{
				Register.RAX,
				Register.RCX,
				Register.RDX,
				Register.RBX,
				Register.RSP,
				Register.RBP,
				Register.RSI,
				Register.RDI,
				Register.R8,
				Register.R9,
				Register.R10,
				Register.R11,
				Register.R12,
				Register.R13,
				Register.R14,
				Register.R15,
			},
		};

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Debug.Assert((int)OpSize.Size16 == 0);
			Debug.Assert((int)OpSize.Size32 == 1);
			Debug.Assert((int)OpSize.Size64 == 2);
			int sizeIndex = (int)state.operandSize;
			int codeIndex = index + (int)state.extraBaseRegisterBase;

			instruction.InternalCode = codes[sizeIndex, codeIndex];
			instruction.InternalOpCount = 1;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = registers[sizeIndex, codeIndex];
		}
	}

	sealed class OpCodeHandler_Xchg_Reg_rAX : OpCodeHandler {
		readonly int index;

		public OpCodeHandler_Xchg_Reg_rAX(int index) {
			Debug.Assert(0 <= index && index <= 7);
			this.index = index;
		}

		static readonly Code[,] codes = new Code[3, 16] {
			{
				Code.Nopw,
				Code.Xchg_CX_AX,
				Code.Xchg_DX_AX,
				Code.Xchg_BX_AX,
				Code.Xchg_SP_AX,
				Code.Xchg_BP_AX,
				Code.Xchg_SI_AX,
				Code.Xchg_DI_AX,
				Code.Xchg_R8W_AX,
				Code.Xchg_R9W_AX,
				Code.Xchg_R10W_AX,
				Code.Xchg_R11W_AX,
				Code.Xchg_R12W_AX,
				Code.Xchg_R13W_AX,
				Code.Xchg_R14W_AX,
				Code.Xchg_R15W_AX,
			},
			{
				Code.Nopd,
				Code.Xchg_ECX_EAX,
				Code.Xchg_EDX_EAX,
				Code.Xchg_EBX_EAX,
				Code.Xchg_ESP_EAX,
				Code.Xchg_EBP_EAX,
				Code.Xchg_ESI_EAX,
				Code.Xchg_EDI_EAX,
				Code.Xchg_R8D_EAX,
				Code.Xchg_R9D_EAX,
				Code.Xchg_R10D_EAX,
				Code.Xchg_R11D_EAX,
				Code.Xchg_R12D_EAX,
				Code.Xchg_R13D_EAX,
				Code.Xchg_R14D_EAX,
				Code.Xchg_R15D_EAX,
			},
			{
				Code.Nopq,
				Code.Xchg_RCX_RAX,
				Code.Xchg_RDX_RAX,
				Code.Xchg_RBX_RAX,
				Code.Xchg_RSP_RAX,
				Code.Xchg_RBP_RAX,
				Code.Xchg_RSI_RAX,
				Code.Xchg_RDI_RAX,
				Code.Xchg_R8_RAX,
				Code.Xchg_R9_RAX,
				Code.Xchg_R10_RAX,
				Code.Xchg_R11_RAX,
				Code.Xchg_R12_RAX,
				Code.Xchg_R13_RAX,
				Code.Xchg_R14_RAX,
				Code.Xchg_R15_RAX,
			},
		};

		static readonly Register[,] registers = new Register[3, 16] {
			{
				Register.None,
				Register.CX,
				Register.DX,
				Register.BX,
				Register.SP,
				Register.BP,
				Register.SI,
				Register.DI,
				Register.R8W,
				Register.R9W,
				Register.R10W,
				Register.R11W,
				Register.R12W,
				Register.R13W,
				Register.R14W,
				Register.R15W,
			},
			{
				Register.None,
				Register.ECX,
				Register.EDX,
				Register.EBX,
				Register.ESP,
				Register.EBP,
				Register.ESI,
				Register.EDI,
				Register.R8D,
				Register.R9D,
				Register.R10D,
				Register.R11D,
				Register.R12D,
				Register.R13D,
				Register.R14D,
				Register.R15D,
			},
			{
				Register.None,
				Register.RCX,
				Register.RDX,
				Register.RBX,
				Register.RSP,
				Register.RBP,
				Register.RSI,
				Register.RDI,
				Register.R8,
				Register.R9,
				Register.R10,
				Register.R11,
				Register.R12,
				Register.R13,
				Register.R14,
				Register.R15,
			},
		};

		static readonly Register[] accumulatorRegister = new Register[3] {
			Register.AX,
			Register.EAX,
			Register.RAX,
		};

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);

			if (index == 0 && state.mandatoryPrefix == MandatoryPrefix.PF3) {
				decoder.ClearMandatoryPrefixF3(ref instruction);
				instruction.InternalCode = Code.Pause;
			}
			else {
				Debug.Assert((int)OpSize.Size16 == 0);
				Debug.Assert((int)OpSize.Size32 == 1);
				Debug.Assert((int)OpSize.Size64 == 2);
				int sizeIndex = (int)state.operandSize;
				int codeIndex = index + (int)state.extraBaseRegisterBase;

				instruction.InternalCode = codes[sizeIndex, codeIndex];
				var reg = registers[sizeIndex, codeIndex];
				if (reg != Register.None) {
					instruction.InternalOpCount = 2;
					Debug.Assert(OpKind.Register == 0);
					//instruction.InternalOp0Kind = OpKind.Register;
					instruction.Op0Register = reg;
					Debug.Assert(OpKind.Register == 0);
					//instruction.InternalOp1Kind = OpKind.Register;
					instruction.Op1Register = accumulatorRegister[sizeIndex];
				}
			}
		}
	}

	sealed class OpCodeHandler_Reg_Iz : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly Register reg16;
		readonly Register reg32;
		readonly Register reg64;

		public OpCodeHandler_Reg_Iz(Code code16, Code code32, Code code64, Register reg16, Register reg32, Register reg64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.reg16 = reg16;
			this.reg32 = reg32;
			this.reg64 = reg64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = reg32;
				instruction.InternalOp2Kind = OpKind.Immediate32;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = reg64;
				instruction.InternalOp2Kind = OpKind.Immediate32to64;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = reg16;
				instruction.InternalOp2Kind = OpKind.Immediate16;
				instruction.InternalImmediate16 = decoder.ReadUInt16();
			}
		}
	}

	sealed class OpCodeHandler_RegIb3 : OpCodeHandler {
		readonly int index;

		public OpCodeHandler_RegIb3(int index) {
			Debug.Assert(0 <= index && index <= 7);
			this.index = index;
		}

		static readonly (Code code, Register register)[] noRexPrefix = new(Code code, Register register)[8] {
			(Code.Mov_AL_Ib, Register.AL),
			(Code.Mov_CL_Ib, Register.CL),
			(Code.Mov_DL_Ib, Register.DL),
			(Code.Mov_BL_Ib, Register.BL),
			(Code.Mov_AH_Ib, Register.AH),
			(Code.Mov_CH_Ib, Register.CH),
			(Code.Mov_DH_Ib, Register.DH),
			(Code.Mov_BH_Ib, Register.BH),
		};

		static readonly (Code code, Register register)[] withRexPrefix = new(Code code, Register register)[16] {
			(Code.Mov_AL_Ib, Register.AL),
			(Code.Mov_CL_Ib, Register.CL),
			(Code.Mov_DL_Ib, Register.DL),
			(Code.Mov_BL_Ib, Register.BL),
			(Code.Mov_SPL_Ib, Register.SPL),
			(Code.Mov_BPL_Ib, Register.BPL),
			(Code.Mov_SIL_Ib, Register.SIL),
			(Code.Mov_DIL_Ib, Register.DIL),
			(Code.Mov_R8L_Ib, Register.R8L),
			(Code.Mov_R9L_Ib, Register.R9L),
			(Code.Mov_R10L_Ib, Register.R10L),
			(Code.Mov_R11L_Ib, Register.R11L),
			(Code.Mov_R12L_Ib, Register.R12L),
			(Code.Mov_R13L_Ib, Register.R13L),
			(Code.Mov_R14L_Ib, Register.R14L),
			(Code.Mov_R15L_Ib, Register.R15L),
		};

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			(Code code, Register register) info;
			if ((state.flags & StateFlags.HasRex) != 0)
				info = withRexPrefix[index + (int)state.extraBaseRegisterBase];
			else
				info = noRexPrefix[index];
			instruction.InternalCode = info.code;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = info.register;
			instruction.InternalOp2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_RegIz2 : OpCodeHandler {
		readonly int index;

		public OpCodeHandler_RegIz2(int index) {
			Debug.Assert(0 <= index && index <= 7);
			this.index = index;
		}

		static readonly (Code code, Register register)[] info16 = new(Code code, Register register)[16] {
			(Code.Mov_AX_Iw, Register.AX),
			(Code.Mov_CX_Iw, Register.CX),
			(Code.Mov_DX_Iw, Register.DX),
			(Code.Mov_BX_Iw, Register.BX),
			(Code.Mov_SP_Iw, Register.SP),
			(Code.Mov_BP_Iw, Register.BP),
			(Code.Mov_SI_Iw, Register.SI),
			(Code.Mov_DI_Iw, Register.DI),
			(Code.Mov_R8W_Iw, Register.R8W),
			(Code.Mov_R9W_Iw, Register.R9W),
			(Code.Mov_R10W_Iw, Register.R10W),
			(Code.Mov_R11W_Iw, Register.R11W),
			(Code.Mov_R12W_Iw, Register.R12W),
			(Code.Mov_R13W_Iw, Register.R13W),
			(Code.Mov_R14W_Iw, Register.R14W),
			(Code.Mov_R15W_Iw, Register.R15W),
		};

		static readonly (Code code, Register register)[] info32 = new(Code code, Register register)[16] {
			(Code.Mov_EAX_Id, Register.EAX),
			(Code.Mov_ECX_Id, Register.ECX),
			(Code.Mov_EDX_Id, Register.EDX),
			(Code.Mov_EBX_Id, Register.EBX),
			(Code.Mov_ESP_Id, Register.ESP),
			(Code.Mov_EBP_Id, Register.EBP),
			(Code.Mov_ESI_Id, Register.ESI),
			(Code.Mov_EDI_Id, Register.EDI),
			(Code.Mov_R8D_Id, Register.R8D),
			(Code.Mov_R9D_Id, Register.R9D),
			(Code.Mov_R10D_Id, Register.R10D),
			(Code.Mov_R11D_Id, Register.R11D),
			(Code.Mov_R12D_Id, Register.R12D),
			(Code.Mov_R13D_Id, Register.R13D),
			(Code.Mov_R14D_Id, Register.R14D),
			(Code.Mov_R15D_Id, Register.R15D),
		};

		static readonly (Code code, Register register)[] info64 = new(Code code, Register register)[16] {
			(Code.Mov_RAX_Iq, Register.RAX),
			(Code.Mov_RCX_Iq, Register.RCX),
			(Code.Mov_RDX_Iq, Register.RDX),
			(Code.Mov_RBX_Iq, Register.RBX),
			(Code.Mov_RSP_Iq, Register.RSP),
			(Code.Mov_RBP_Iq, Register.RBP),
			(Code.Mov_RSI_Iq, Register.RSI),
			(Code.Mov_RDI_Iq, Register.RDI),
			(Code.Mov_R8_Iq, Register.R8),
			(Code.Mov_R9_Iq, Register.R9),
			(Code.Mov_R10_Iq, Register.R10),
			(Code.Mov_R11_Iq, Register.R11),
			(Code.Mov_R12_Iq, Register.R12),
			(Code.Mov_R13_Iq, Register.R13),
			(Code.Mov_R14_Iq, Register.R14),
			(Code.Mov_R15_Iq, Register.R15),
		};

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			(Code code, Register register) info;
			int index = this.index + (int)state.extraBaseRegisterBase;
			if (state.operandSize == OpSize.Size32) {
				info = info32[index];
				instruction.InternalCode = info.code;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = info.register;
				instruction.InternalOp2Kind = OpKind.Immediate32;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else if (state.operandSize == OpSize.Size64) {
				info = info64[index];
				instruction.InternalCode = info.code;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = info.register;
				instruction.InternalOp2Kind = OpKind.Immediate64;
				instruction.InternalImmediate64_lo = decoder.ReadUInt32();
				instruction.InternalImmediate64_hi = decoder.ReadUInt32();
			}
			else {
				info = info16[index];
				instruction.InternalCode = info.code;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = info.register;
				instruction.InternalOp2Kind = OpKind.Immediate16;
				instruction.InternalImmediate16 = decoder.ReadUInt16();
			}
		}
	}

	sealed class OpCodeHandler_PushIb2 : OpCodeHandler {
		readonly Code code16;
		readonly Code code64;

		public OpCodeHandler_PushIb2(Code code16, Code code64) {
			this.code16 = code16;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 1;
			if (state.operandSize != OpSize.Size16) {
				instruction.InternalCode = code64;
				instruction.InternalOp1Kind = OpKind.Immediate8to64;
				instruction.InternalImmediate8 = decoder.ReadIb();
			}
			else {
				instruction.InternalCode = code16;
				instruction.InternalOp1Kind = OpKind.Immediate8to16;
				instruction.InternalImmediate8 = decoder.ReadIb();
			}
		}
	}

	sealed class OpCodeHandler_PushIz : OpCodeHandler {
		readonly Code code16;
		readonly Code code64;

		public OpCodeHandler_PushIz(Code code16, Code code64) {
			this.code16 = code16;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 1;
			if (state.operandSize != OpSize.Size16) {
				instruction.InternalCode = code64;
				instruction.InternalOp1Kind = OpKind.Immediate32to64;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else {
				instruction.InternalCode = code16;
				instruction.InternalOp1Kind = OpKind.Immediate16;
				instruction.InternalImmediate16 = decoder.ReadUInt16();
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ev_Ib : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_Ev_Ib(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 3;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.Op1Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.Op1Register = (int)index + Register.RAX;
				else
					instruction.Op1Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalMemorySize = MemorySize.Int32;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalMemorySize = MemorySize.Int64;
				else
					instruction.InternalMemorySize = MemorySize.Int16;
				decoder.ReadOpMem_m64(ref instruction);
			}
			if (state.operandSize == OpSize.Size32) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
				instruction.InternalCode = code32;
				instruction.InternalOp3Kind = OpKind.Immediate8to32;
				instruction.InternalImmediate8 = decoder.ReadIb();
			}
			else if (state.operandSize == OpSize.Size64) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
				instruction.InternalCode = code64;
				instruction.InternalOp3Kind = OpKind.Immediate8to64;
				instruction.InternalImmediate8 = decoder.ReadIb();
			}
			else {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
				instruction.InternalCode = code16;
				instruction.InternalOp3Kind = OpKind.Immediate8to16;
				instruction.InternalImmediate8 = decoder.ReadIb();
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ev_Ib_REX : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;
		readonly MemorySize memSize32;
		readonly MemorySize memSize64;

		public OpCodeHandler_Gv_Ev_Ib_REX(Register baseReg, Code code32, Code code64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
			memSize32 = MemorySize.Unknown;
			memSize64 = MemorySize.Unknown;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 3;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.W) != 0)
					instruction.InternalMemorySize = memSize64;
				else
					instruction.InternalMemorySize = memSize32;
				decoder.ReadOpMem_m64(ref instruction);
				if (memSize32 == MemorySize.Unknown)
					decoder.SetInvalidInstruction();
			}
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_Gv_Ev_32_64 : OpCodeHandlerModRM {
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_Gv_Ev_32_64(Code code32, Code code64) {
			code = code64;
			memSize = MemorySize.Unknown;
		}

		public OpCodeHandler_Gv_Ev_32_64(Code code32, Code code64, MemorySize memSize32, MemorySize memSize64) {
			code = code64;
			memSize = memSize64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction);
				if (memSize == MemorySize.Unknown)
					decoder.SetInvalidInstruction();
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ev_Iz : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_Ev_Iz(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 3;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.Op1Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.Op1Register = (int)index + Register.RAX;
				else
					instruction.Op1Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalMemorySize = MemorySize.Int32;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalMemorySize = MemorySize.Int64;
				else
					instruction.InternalMemorySize = MemorySize.Int16;
				decoder.ReadOpMem_m64(ref instruction);
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
				instruction.InternalOp3Kind = OpKind.Immediate32;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
				instruction.InternalOp3Kind = OpKind.Immediate32to64;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
				instruction.InternalOp3Kind = OpKind.Immediate16;
				instruction.InternalImmediate16 = decoder.ReadUInt16();
			}
		}
	}

	sealed class OpCodeHandler_Yb_Reg : OpCodeHandler {
		readonly Code code;
		readonly Register reg;

		public OpCodeHandler_Yb_Reg(Code code, Register reg) {
			this.code = code;
			this.reg = reg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp1Kind = OpKind.MemoryESRDI;
			else
				instruction.InternalOp1Kind = OpKind.MemoryESEDI;
			instruction.InternalMemorySize = MemorySize.UInt8;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = reg;
		}
	}

	sealed class OpCodeHandler_Yv_Reg : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly Register reg16;
		readonly Register reg32;
		readonly Register reg64;

		public OpCodeHandler_Yv_Reg(Code code16, Code code32, Code code64, Register reg16, Register reg32, Register reg64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.reg16 = reg16;
			this.reg32 = reg32;
			this.reg64 = reg64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp1Kind = OpKind.MemoryESRDI;
			else
				instruction.InternalOp1Kind = OpKind.MemoryESEDI;
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				instruction.InternalMemorySize = MemorySize.UInt32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = reg32;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				instruction.InternalMemorySize = MemorySize.UInt64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = reg64;
			}
			else {
				instruction.InternalCode = code16;
				instruction.InternalMemorySize = MemorySize.UInt16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = reg16;
			}
		}
	}

	sealed class OpCodeHandler_Yv_Reg2 : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Register reg16;
		readonly Register reg32;

		public OpCodeHandler_Yv_Reg2(Code code16, Code code32, Register reg16, Register reg32) {
			this.code16 = code16;
			this.code32 = code32;
			this.reg16 = reg16;
			this.reg32 = reg32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp1Kind = OpKind.MemoryESRDI;
			else
				instruction.InternalOp1Kind = OpKind.MemoryESEDI;
			if (state.operandSize == OpSize.Size16) {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = reg16;
				instruction.InternalMemorySize = MemorySize.UInt16;
			}
			else {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = reg32;
				instruction.InternalMemorySize = MemorySize.UInt32;
			}
		}
	}

	sealed class OpCodeHandler_Reg_Xb : OpCodeHandler {
		readonly Code code;
		readonly Register reg;

		public OpCodeHandler_Reg_Xb(Code code, Register reg) {
			this.code = code;
			this.reg = reg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = reg;
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp2Kind = OpKind.MemorySegRSI;
			else
				instruction.InternalOp2Kind = OpKind.MemorySegESI;
			instruction.InternalMemorySize = MemorySize.UInt8;
		}
	}

	sealed class OpCodeHandler_Reg_Xv : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly Register reg16;
		readonly Register reg32;
		readonly Register reg64;

		public OpCodeHandler_Reg_Xv(Code code16, Code code32, Code code64, Register reg16, Register reg32, Register reg64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.reg16 = reg16;
			this.reg32 = reg32;
			this.reg64 = reg64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp2Kind = OpKind.MemorySegRSI;
			else
				instruction.InternalOp2Kind = OpKind.MemorySegESI;
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = reg32;
				instruction.InternalMemorySize = MemorySize.UInt32;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = reg64;
				instruction.InternalMemorySize = MemorySize.UInt64;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = reg16;
				instruction.InternalMemorySize = MemorySize.UInt16;
			}
		}
	}

	sealed class OpCodeHandler_Reg_Xv2 : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly Register reg16;
		readonly Register reg32;
		readonly Register reg64;

		public OpCodeHandler_Reg_Xv2(Code code16, Code code32, Code code64, Register reg16, Register reg32, Register reg64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.reg16 = reg16;
			this.reg32 = reg32;
			this.reg64 = reg64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp2Kind = OpKind.MemorySegRSI;
			else
				instruction.InternalOp2Kind = OpKind.MemorySegESI;
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = reg32;
				instruction.InternalMemorySize = MemorySize.UInt32;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = reg64;
				instruction.InternalMemorySize = MemorySize.UInt32;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = reg16;
				instruction.InternalMemorySize = MemorySize.UInt16;
			}
		}
	}

	sealed class OpCodeHandler_Reg_Yb : OpCodeHandler {
		readonly Code code;
		readonly Register reg;

		public OpCodeHandler_Reg_Yb(Code code, Register reg) {
			this.code = code;
			this.reg = reg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = reg;
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp2Kind = OpKind.MemoryESRDI;
			else
				instruction.InternalOp2Kind = OpKind.MemoryESEDI;
			instruction.InternalMemorySize = MemorySize.UInt8;
		}
	}

	sealed class OpCodeHandler_Reg_Yv : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly Register reg16;
		readonly Register reg32;
		readonly Register reg64;

		public OpCodeHandler_Reg_Yv(Code code16, Code code32, Code code64, Register reg16, Register reg32, Register reg64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.reg16 = reg16;
			this.reg32 = reg32;
			this.reg64 = reg64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp2Kind = OpKind.MemoryESRDI;
			else
				instruction.InternalOp2Kind = OpKind.MemoryESEDI;
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = reg32;
				instruction.InternalMemorySize = MemorySize.UInt32;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = reg64;
				instruction.InternalMemorySize = MemorySize.UInt64;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = reg16;
				instruction.InternalMemorySize = MemorySize.UInt16;
			}
		}
	}

	sealed class OpCodeHandler_Yb_Xb : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_Yb_Xb(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			if (state.addressSize == OpSize.Size64) {
				instruction.InternalOp1Kind = OpKind.MemoryESRDI;
				instruction.InternalOp2Kind = OpKind.MemorySegRSI;
			}
			else {
				instruction.InternalOp1Kind = OpKind.MemoryESEDI;
				instruction.InternalOp2Kind = OpKind.MemorySegESI;
			}
			instruction.InternalMemorySize = MemorySize.UInt8;
		}
	}

	sealed class OpCodeHandler_Yv_Xv : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Yv_Xv(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.addressSize == OpSize.Size64) {
				instruction.InternalOp1Kind = OpKind.MemoryESRDI;
				instruction.InternalOp2Kind = OpKind.MemorySegRSI;
			}
			else {
				instruction.InternalOp1Kind = OpKind.MemoryESEDI;
				instruction.InternalOp2Kind = OpKind.MemorySegESI;
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				instruction.InternalMemorySize = MemorySize.UInt32;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				instruction.InternalMemorySize = MemorySize.UInt64;
			}
			else {
				instruction.InternalCode = code16;
				instruction.InternalMemorySize = MemorySize.UInt16;
			}
		}
	}

	sealed class OpCodeHandler_Xb_Yb : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_Xb_Yb(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			if (state.addressSize == OpSize.Size64) {
				instruction.InternalOp1Kind = OpKind.MemorySegRSI;
				instruction.InternalOp2Kind = OpKind.MemoryESRDI;
			}
			else {
				instruction.InternalOp1Kind = OpKind.MemorySegESI;
				instruction.InternalOp2Kind = OpKind.MemoryESEDI;
			}
			instruction.InternalMemorySize = MemorySize.UInt8;
		}
	}

	sealed class OpCodeHandler_Xv_Yv : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Xv_Yv(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.addressSize == OpSize.Size64) {
				instruction.InternalOp1Kind = OpKind.MemorySegRSI;
				instruction.InternalOp2Kind = OpKind.MemoryESRDI;
			}
			else {
				instruction.InternalOp1Kind = OpKind.MemorySegESI;
				instruction.InternalOp2Kind = OpKind.MemoryESEDI;
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				instruction.InternalMemorySize = MemorySize.UInt32;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				instruction.InternalMemorySize = MemorySize.UInt64;
			}
			else {
				instruction.InternalCode = code16;
				instruction.InternalMemorySize = MemorySize.UInt16;
			}
		}
	}

	sealed class OpCodeHandler_Ev_Sw : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Ev_Sw(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.Op0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.Op0Register = (int)index + Register.RAX;
				else
					instruction.Op0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = MemorySize.UInt16;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = decoder.ReadOpSw();
		}
	}

	sealed class OpCodeHandler_Gv_M : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_M(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = MemorySize.Unknown;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Sw_Ev : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Sw_Ev(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = decoder.ReadOpSw();
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.Op1Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.Op1Register = (int)index + Register.RAX;
				else
					instruction.Op1Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = MemorySize.UInt16;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Reg_Ob : OpCodeHandler {
		readonly Code code;
		readonly Register reg;

		public OpCodeHandler_Reg_Ob(Code code, Register reg) {
			this.code = code;
			this.reg = reg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = reg;
			Debug.Assert(state.addressSize == OpSize.Size32 || state.addressSize == OpSize.Size64);
			decoder.displIndex = state.instructionLength;
			if (state.addressSize == OpSize.Size64) {
				instruction.InternalSetMemoryDisplSize(4);
				state.flags |= StateFlags.Addr64;
				instruction.InternalMemoryAddress64_lo = decoder.ReadUInt32();
				instruction.InternalMemoryAddress64_hi = decoder.ReadUInt32();
				instruction.InternalOp2Kind = OpKind.Memory64;
			}
			else {
				instruction.InternalSetMemoryDisplSize(3);
				instruction.MemoryDisplacement = decoder.ReadUInt32();
				//instruction.InternalMemoryIndexScale = 0;
				//instruction.MemoryBase = Register.None;
				//instruction.MemoryIndex = Register.None;
				instruction.InternalOp2Kind = OpKind.Memory;
			}
			instruction.InternalMemorySize = MemorySize.UInt8;
		}
	}

	sealed class OpCodeHandler_Ob_Reg : OpCodeHandler {
		readonly Code code;
		readonly Register reg;

		public OpCodeHandler_Ob_Reg(Code code, Register reg) {
			this.code = code;
			this.reg = reg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			Debug.Assert(state.addressSize == OpSize.Size32 || state.addressSize == OpSize.Size64);
			decoder.displIndex = state.instructionLength;
			if (state.addressSize == OpSize.Size64) {
				instruction.InternalSetMemoryDisplSize(4);
				state.flags |= StateFlags.Addr64;
				instruction.InternalMemoryAddress64_lo = decoder.ReadUInt32();
				instruction.InternalMemoryAddress64_hi = decoder.ReadUInt32();
				instruction.InternalOp1Kind = OpKind.Memory64;
			}
			else {
				instruction.InternalSetMemoryDisplSize(3);
				instruction.MemoryDisplacement = decoder.ReadUInt32();
				//instruction.InternalMemoryIndexScale = 0;
				//instruction.MemoryBase = Register.None;
				//instruction.MemoryIndex = Register.None;
				instruction.InternalOp1Kind = OpKind.Memory;
			}
			instruction.InternalMemorySize = MemorySize.UInt8;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = reg;
		}
	}

	sealed class OpCodeHandler_Reg_Ov : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly Register reg16;
		readonly Register reg32;
		readonly Register reg64;

		public OpCodeHandler_Reg_Ov(Code code16, Code code32, Code code64, Register reg16, Register reg32, Register reg64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.reg16 = reg16;
			this.reg32 = reg32;
			this.reg64 = reg64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			Debug.Assert(state.addressSize == OpSize.Size32 || state.addressSize == OpSize.Size64);
			decoder.displIndex = state.instructionLength;
			if (state.addressSize == OpSize.Size64) {
				instruction.InternalSetMemoryDisplSize(4);
				state.flags |= StateFlags.Addr64;
				instruction.InternalMemoryAddress64_lo = decoder.ReadUInt32();
				instruction.InternalMemoryAddress64_hi = decoder.ReadUInt32();
				instruction.InternalOp2Kind = OpKind.Memory64;
			}
			else {
				instruction.InternalSetMemoryDisplSize(3);
				instruction.MemoryDisplacement = decoder.ReadUInt32();
				//instruction.InternalMemoryIndexScale = 0;
				//instruction.MemoryBase = Register.None;
				//instruction.MemoryIndex = Register.None;
				instruction.InternalOp2Kind = OpKind.Memory;
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = reg32;
				instruction.InternalMemorySize = MemorySize.UInt32;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = reg64;
				instruction.InternalMemorySize = MemorySize.UInt64;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = reg16;
				instruction.InternalMemorySize = MemorySize.UInt16;
			}
		}
	}

	sealed class OpCodeHandler_Ov_Reg : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly Register reg16;
		readonly Register reg32;
		readonly Register reg64;

		public OpCodeHandler_Ov_Reg(Code code16, Code code32, Code code64, Register reg16, Register reg32, Register reg64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.reg16 = reg16;
			this.reg32 = reg32;
			this.reg64 = reg64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			decoder.displIndex = state.instructionLength;
			if (state.addressSize == OpSize.Size64) {
				instruction.InternalSetMemoryDisplSize(4);
				state.flags |= StateFlags.Addr64;
				instruction.InternalMemoryAddress64_lo = decoder.ReadUInt32();
				instruction.InternalMemoryAddress64_hi = decoder.ReadUInt32();
				instruction.InternalOp1Kind = OpKind.Memory64;
			}
			else {
				instruction.InternalSetMemoryDisplSize(3);
				instruction.MemoryDisplacement = decoder.ReadUInt32();
				//instruction.InternalMemoryIndexScale = 0;
				//instruction.MemoryBase = Register.None;
				//instruction.MemoryIndex = Register.None;
				instruction.InternalOp1Kind = OpKind.Memory;
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				instruction.InternalMemorySize = MemorySize.UInt32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = reg32;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				instruction.InternalMemorySize = MemorySize.UInt64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = reg64;
			}
			else {
				instruction.InternalCode = code16;
				instruction.InternalMemorySize = MemorySize.UInt16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = reg16;
			}
		}
	}

	sealed class OpCodeHandler_BranchIw : OpCodeHandler {
		readonly Code code64;

		public OpCodeHandler_BranchIw(Code code64) => this.code64 = code64;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code64;
			instruction.InternalOpCount = 1;
			instruction.InternalOp1Kind = OpKind.Immediate16;
			instruction.InternalImmediate16 = decoder.ReadUInt16();
		}
	}

	sealed class OpCodeHandler_BranchSimple : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_BranchSimple(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			//instruction.InternalOpCount = 0;
		}
	}

	sealed class OpCodeHandler_Iw_Ib : OpCodeHandler {
		readonly Code code16;
		readonly Code code64;

		public OpCodeHandler_Iw_Ib(Code code16, Code code64) {
			this.code16 = code16;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize != OpSize.Size16)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			instruction.InternalOpCount = 2;
			instruction.InternalOp1Kind = OpKind.Immediate16;
			instruction.InternalImmediate16 = decoder.ReadUInt16();
			instruction.InternalOp2Kind = OpKind.Immediate8_Enter;
			instruction.InternalImmediate8_Enter = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_Reg_Ib2 : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Register reg16;
		readonly Register reg32;

		public OpCodeHandler_Reg_Ib2(Code code16, Code code32, Register reg16, Register reg32) {
			this.code16 = code16;
			this.code32 = code32;
			this.reg16 = reg16;
			this.reg32 = reg32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			if (state.operandSize != OpSize.Size16) {
				instruction.InternalCode = code32;
				instruction.Op0Register = reg32;
			}
			else {
				instruction.InternalCode = code16;
				instruction.Op0Register = reg16;
			}
			instruction.InternalOp2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_IbReg2 : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Register reg16;
		readonly Register reg32;

		public OpCodeHandler_IbReg2(Code code16, Code code32, Register reg16, Register reg32) {
			this.code16 = code16;
			this.code32 = code32;
			this.reg16 = reg16;
			this.reg32 = reg32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			instruction.InternalOp1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			if (state.operandSize != OpSize.Size16) {
				instruction.InternalCode = code32;
				instruction.Op1Register = reg32;
			}
			else {
				instruction.InternalCode = code16;
				instruction.Op1Register = reg16;
			}
		}
	}

	sealed class OpCodeHandler_eAX_DX : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;

		public OpCodeHandler_eAX_DX(Code code16, Code code32) {
			this.code16 = code16;
			this.code32 = code32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.operandSize != OpSize.Size16) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = Register.EAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = Register.AX;
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = Register.DX;
		}
	}

	sealed class OpCodeHandler_DX_eAX : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;

		public OpCodeHandler_DX_eAX(Code code16, Code code32) {
			this.code16 = code16;
			this.code32 = code32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = Register.DX;
			if (state.operandSize != OpSize.Size16) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = Register.EAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = Register.AX;
			}
		}
	}

	sealed class OpCodeHandler_Eb_Ib : OpCodeHandlerModRM {
		readonly Code code;
		readonly MemorySize memSize;
		readonly HandlerFlags flags;

		public OpCodeHandler_Eb_Ib(Code code) {
			this.code = code;
			memSize = MemorySize.UInt8;
		}

		public OpCodeHandler_Eb_Ib(Code code, MemorySize memSize) {
			this.code = code;
			this.memSize = memSize;
		}

		public OpCodeHandler_Eb_Ib(Code code, HandlerFlags flags) {
			this.code = code;
			memSize = MemorySize.UInt8;
			this.flags = flags;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				uint index = state.rm + state.extraBaseRegisterBase;
				if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction);
				if ((flags & HandlerFlags.XacquireRelease) != 0)
					decoder.SetXacquireRelease(ref instruction, flags);
			}
			instruction.InternalOp2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_Eb_1 : OpCodeHandlerModRM {
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_Eb_1(Code code) {
			this.code = code;
			memSize = MemorySize.UInt8;
		}

		public OpCodeHandler_Eb_1(Code code, MemorySize memSize) {
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				uint index = state.rm + state.extraBaseRegisterBase;
				if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction);
			}
			instruction.InternalOp2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = 1;
			state.flags |= StateFlags.NoImm;
		}
	}

	sealed class OpCodeHandler_Eb_CL : OpCodeHandlerModRM {
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_Eb_CL(Code code) {
			this.code = code;
			memSize = MemorySize.UInt8;
		}

		public OpCodeHandler_Eb_CL(Code code, MemorySize memSize) {
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				uint index = state.rm + state.extraBaseRegisterBase;
				if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = Register.CL;
		}
	}

	sealed class OpCodeHandler_Eb : OpCodeHandlerModRM {
		readonly Code code;
		readonly MemorySize memSize;
		readonly HandlerFlags flags;

		public OpCodeHandler_Eb(Code code) {
			this.code = code;
			memSize = MemorySize.UInt8;
		}

		public OpCodeHandler_Eb(Code code, MemorySize memSize) {
			this.code = code;
			this.memSize = memSize;
		}

		public OpCodeHandler_Eb(Code code, HandlerFlags flags) {
			this.code = code;
			memSize = MemorySize.UInt8;
			this.flags = flags;
		}

		public OpCodeHandler_Eb(Code code, MemorySize memSize, HandlerFlags flags) {
			this.code = code;
			this.memSize = memSize;
			this.flags = flags;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 1;
			if (state.mod == 3) {
				uint index = state.rm + state.extraBaseRegisterBase;
				if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction);
				if ((flags & HandlerFlags.XacquireRelease) != 0)
					decoder.SetXacquireRelease(ref instruction, flags);
			}
		}
	}

	sealed class OpCodeHandler_Eb_Gb : OpCodeHandlerModRM {
		readonly Code code;
		readonly HandlerFlags flags;

		public OpCodeHandler_Eb_Gb(Code code) => this.code = code;

		public OpCodeHandler_Eb_Gb(Code code, HandlerFlags flags) {
			this.code = code;
			this.flags = flags;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			uint index;
			if (state.mod == 3) {
				index = state.rm + state.extraBaseRegisterBase;
				if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = MemorySize.UInt8;
				decoder.ReadOpMem_m64(ref instruction);
				if ((flags & HandlerFlags.XacquireRelease) != 0)
					decoder.SetXacquireRelease(ref instruction, flags);
			}
			index = state.reg + state.extraRegisterBase;
			if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
				index += 4;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)index + Register.AL;
		}
	}

	sealed class OpCodeHandler_Gb_Eb : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Gb_Eb(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			uint index = state.reg + state.extraRegisterBase;
			if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
				index += 4;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)index + Register.AL;

			if (state.mod == 3) {
				index = state.rm + state.extraBaseRegisterBase;
				if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = MemorySize.UInt8;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_M : OpCodeHandlerModRM {
		readonly Code codeW0;
		readonly Code codeW1;
		readonly MemorySize memorySizeW0;
		readonly MemorySize memorySizeW1;

		public OpCodeHandler_M(Code codeW0, Code codeW1, MemorySize memorySizeW0, MemorySize memorySizeW1) {
			this.codeW0 = codeW0;
			this.codeW1 = codeW1;
			this.memorySizeW0 = memorySizeW0;
			this.memorySizeW1 = memorySizeW1;
		}

		public OpCodeHandler_M(Code codeW0, MemorySize memorySizeW0) {
			this.codeW0 = codeW0;
			codeW1 = codeW0;
			this.memorySizeW0 = memorySizeW0;
			memorySizeW1 = memorySizeW0;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalCode = codeW1;
			else
				instruction.InternalCode = codeW0;
			instruction.InternalOpCount = 1;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if ((state.flags & StateFlags.W) != 0)
					instruction.InternalMemorySize = memorySizeW1;
				else
					instruction.InternalMemorySize = memorySizeW0;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_M_REXW : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;
		readonly MemorySize memSize32;
		readonly MemorySize memSize64;
		readonly HandlerFlags flags32;
		readonly HandlerFlags flags64;

		public OpCodeHandler_M_REXW(Code code32, Code code64, MemorySize memSize32, MemorySize memSize64) {
			this.code32 = code32;
			this.code64 = code64;
			this.memSize32 = memSize32;
			this.memSize64 = memSize64;
		}

		public OpCodeHandler_M_REXW(Code code32, Code code64, MemorySize memSize32, MemorySize memSize64, HandlerFlags flags32, HandlerFlags flags64) {
			this.code32 = code32;
			this.code64 = code64;
			this.memSize32 = memSize32;
			this.memSize64 = memSize64;
			this.flags32 = flags32;
			this.flags64 = flags64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code32;
			instruction.InternalOpCount = 1;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				HandlerFlags flags;
				if ((state.flags & StateFlags.W) != 0) {
					instruction.InternalMemorySize = memSize64;
					flags = flags64;
				}
				else {
					instruction.InternalMemorySize = memSize32;
					flags = flags32;
				}
				decoder.ReadOpMem_m64(ref instruction);
				if ((flags & HandlerFlags.XacquireRelease) != 0)
					decoder.SetXacquireRelease(ref instruction, flags);
			}
		}
	}

	sealed class OpCodeHandler_MemBx : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_MemBx(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 1;
			//instruction.MemoryDisplacement = 0;
			//instruction.InternalMemoryIndexScale = 0;
			//instruction.InternalSetMemoryDisplSize(0);
			if (state.addressSize == OpSize.Size64)
				instruction.MemoryBase = Register.RBX;
			else if (state.addressSize == OpSize.Size32)
				instruction.MemoryBase = Register.EBX;
			else
				instruction.MemoryBase = Register.BX;
			instruction.MemoryIndex = Register.AL;
			instruction.InternalMemorySize = MemorySize.UInt8;
			instruction.InternalOp1Kind = OpKind.Memory;
		}
	}

	sealed class OpCodeHandler_VW : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code codeR;
		readonly Code codeM;
		readonly MemorySize memSize;

		public OpCodeHandler_VW(Register baseReg, Code codeR, Code codeM, MemorySize memSize) {
			this.baseReg = baseReg;
			this.codeR = codeR;
			this.codeM = codeM;
			this.memSize = memSize;
		}

		public OpCodeHandler_VW(Register baseReg, Code code, MemorySize memSize) {
			this.baseReg = baseReg;
			codeR = code;
			codeM = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				instruction.InternalCode = codeR;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.InternalCode = codeM;
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_WV : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_WV(Register baseReg, Code code, MemorySize memSize) {
			this.baseReg = baseReg;
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
		}
	}

	sealed class OpCodeHandler_rDI_VX_RX : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_rDI_VX_RX(Register baseReg, Code code, MemorySize memSize) {
			this.baseReg = baseReg;
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;
			Debug.Assert(state.addressSize == OpSize.Size32 || state.addressSize == OpSize.Size64);
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp1Kind = OpKind.MemorySegRDI;
			else
				instruction.InternalOp1Kind = OpKind.MemorySegEDI;
			instruction.InternalMemorySize = memSize;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.Op2Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_rDI_P_N : OpCodeHandlerModRM {
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_rDI_P_N(Code code, MemorySize memSize) {
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;
			Debug.Assert(state.addressSize == OpSize.Size32 || state.addressSize == OpSize.Size64);
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp1Kind = OpKind.MemorySegRDI;
			else
				instruction.InternalOp1Kind = OpKind.MemorySegEDI;
			instruction.InternalMemorySize = memSize;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.reg + Register.MM0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.Op2Register = (int)state.rm + Register.MM0;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_VM : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_VM(Register baseReg, Code code, MemorySize memSize) {
			this.baseReg = baseReg;
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_MV : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_MV(Register baseReg, Code code, MemorySize memSize) {
			this.baseReg = baseReg;
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
		}
	}

	sealed class OpCodeHandler_VQ : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_VQ(Register baseReg, Code code, MemorySize memSize) {
			this.baseReg = baseReg;
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)state.rm + Register.MM0;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_P_Q : OpCodeHandlerModRM {
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_P_Q(Code code, MemorySize memSize) {
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + Register.MM0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)state.rm + Register.MM0;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Q_P : OpCodeHandlerModRM {
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_Q_P(Code code, MemorySize memSize) {
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)state.rm + Register.MM0;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.reg + Register.MM0;
		}
	}

	sealed class OpCodeHandler_MP : OpCodeHandlerModRM {
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_MP(Code code, MemorySize memSize) {
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.reg + Register.MM0;
		}
	}

	sealed class OpCodeHandler_P_Q_Ib : OpCodeHandlerModRM {
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_P_Q_Ib(Code code, MemorySize memSize) {
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + Register.MM0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)state.rm + Register.MM0;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction);
			}
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_P_W : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_P_W(Register baseReg, Code code, MemorySize memSize) {
			this.baseReg = baseReg;
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + Register.MM0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_P_R : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_P_R(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + Register.MM0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_P_Ev : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_P_Ev(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Register gpr;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				gpr = Register.EAX;
			}
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + Register.MM0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.W) != 0)
					instruction.InternalMemorySize = MemorySize.UInt64;
				else
					instruction.InternalMemorySize = MemorySize.UInt32;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_P_Ev_Ib : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;
		readonly MemorySize memSize32;
		readonly MemorySize memSize64;

		public OpCodeHandler_P_Ev_Ib(Code code32, Code code64, MemorySize memSize32, MemorySize memSize64) {
			this.code32 = code32;
			this.code64 = code64;
			this.memSize32 = memSize32;
			this.memSize64 = memSize64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Register gpr;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				gpr = Register.EAX;
			}
			instruction.InternalOpCount = 3;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + Register.MM0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.W) != 0)
					instruction.InternalMemorySize = memSize64;
				else
					instruction.InternalMemorySize = memSize32;
				decoder.ReadOpMem_m64(ref instruction);
			}
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_Ev_P : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Ev_P(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Register gpr;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				gpr = Register.EAX;
			}
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if ((state.flags & StateFlags.W) != 0)
					instruction.InternalMemorySize = MemorySize.UInt64;
				else
					instruction.InternalMemorySize = MemorySize.UInt32;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.reg + Register.MM0;
		}
	}

	sealed class OpCodeHandler_Gv_W : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code codeW0;
		readonly Code codeW1;
		readonly MemorySize memSize;

		public OpCodeHandler_Gv_W(Register baseReg, Code codeW0, Code codeW1, MemorySize memSize) {
			this.baseReg = baseReg;
			this.codeW0 = codeW0;
			this.codeW1 = codeW1;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = codeW1;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = codeW0;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_V_Ev : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code codeW0;
		readonly Code codeW1;

		public OpCodeHandler_V_Ev(Register baseReg, Code codeW0, Code codeW1) {
			this.baseReg = baseReg;
			this.codeW0 = codeW0;
			this.codeW1 = codeW1;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = codeW1;
			else
				instruction.InternalCode = codeW0;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase) + (state.operandSize == OpSize.Size64 ? Register.RAX : Register.EAX);
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if (state.operandSize == OpSize.Size64)
					instruction.InternalMemorySize = MemorySize.Int64;
				else
					instruction.InternalMemorySize = MemorySize.Int32;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VWIb : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code codeW0;
		readonly Code codeW1;
		readonly MemorySize memSize;

		public OpCodeHandler_VWIb(Register baseReg, Code code, MemorySize memSize) {
			this.baseReg = baseReg;
			codeW0 = code;
			codeW1 = code;
			this.memSize = memSize;
		}

		public OpCodeHandler_VWIb(Register baseReg, Code codeW0, Code codeW1, MemorySize memSize) {
			this.baseReg = baseReg;
			this.codeW0 = codeW0;
			this.codeW1 = codeW1;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalCode = codeW1;
			else
				instruction.InternalCode = codeW0;
			instruction.InternalOpCount = 3;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction);
			}
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_RIb : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_RIb(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else
				decoder.SetInvalidInstruction();
			instruction.InternalOp2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_Ed_V_Ib : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;
		readonly MemorySize memSize32;
		readonly MemorySize memSize64;

		public OpCodeHandler_Ed_V_Ib(Register baseReg, Code code32, Code code64, MemorySize memSize32, MemorySize memSize64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
			this.memSize32 = memSize32;
			this.memSize64 = memSize64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Register gpr;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				gpr = Register.EAX;
			}
			instruction.InternalOpCount = 3;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if ((state.flags & StateFlags.W) != 0)
					instruction.InternalMemorySize = memSize64;
				else
					instruction.InternalMemorySize = memSize32;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VX_Ev : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_VX_Ev(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Register gpr;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				gpr = Register.EAX;
			}
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.XMM0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.W) != 0)
					instruction.InternalMemorySize = MemorySize.UInt64;
				else
					instruction.InternalMemorySize = MemorySize.UInt32;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Ev_VX : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Ev_VX(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Register gpr;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				gpr = Register.EAX;
			}
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if ((state.flags & StateFlags.W) != 0)
					instruction.InternalMemorySize = MemorySize.UInt64;
				else
					instruction.InternalMemorySize = MemorySize.UInt32;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)(state.reg + state.extraRegisterBase) + Register.XMM0;
		}
	}

	sealed class OpCodeHandler_VX_E_Ib : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;
		readonly MemorySize memSize32;
		readonly MemorySize memSize64;

		public OpCodeHandler_VX_E_Ib(Register baseReg, Code code32, Code code64, MemorySize memSize32, MemorySize memSize64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
			this.memSize32 = memSize32;
			this.memSize64 = memSize64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Register gpr;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				gpr = Register.EAX;
			}
			instruction.InternalOpCount = 3;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.W) != 0)
					instruction.InternalMemorySize = memSize64;
				else
					instruction.InternalMemorySize = memSize32;
				decoder.ReadOpMem_m64(ref instruction);
			}
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_Gv_RX : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_RX(Register baseReg, Code code32, Code code64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code32;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + ((state.flags & StateFlags.W) != 0 ? Register.RAX : Register.EAX);
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_B_MIB : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_B_MIB(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg & 3) + Register.BND0;
			if (state.mod == 3) {
				// Should never be reached
				decoder.SetInvalidInstruction();
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = MemorySize.UInt64;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_MIB_B : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_MIB_B(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				// Should never be reached
				decoder.SetInvalidInstruction();
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = MemorySize.UInt64;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)(state.reg & 3) + Register.BND0;
		}
	}

	sealed class OpCodeHandler_B_BM : OpCodeHandlerModRM {
		readonly Code code64;
		readonly MemorySize memSize64;

		public OpCodeHandler_B_BM(Code code32, Code code64, MemorySize memSize32, MemorySize memSize64) {
			this.code64 = code64;
			this.memSize64 = memSize64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code64;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg & 3) + Register.BND0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm & 3) + Register.BND0;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize64;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_BM_B : OpCodeHandlerModRM {
		readonly Code code64;
		readonly MemorySize memSize64;

		public OpCodeHandler_BM_B(Code code32, Code code64, MemorySize memSize32, MemorySize memSize64) {
			this.code64 = code64;
			this.memSize64 = memSize64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code64;
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.rm & 3) + Register.BND0;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize64;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)(state.reg & 3) + Register.BND0;
		}
	}

	sealed class OpCodeHandler_B_Ev : OpCodeHandlerModRM {
		readonly Code code64;

		public OpCodeHandler_B_Ev(Code code32, Code code64) => this.code64 = code64;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code64;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg & 3) + Register.BND0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = MemorySize.UInt64;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Mv_Gv_REXW : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;
		readonly MemorySize memSize32;
		readonly MemorySize memSize64;

		public OpCodeHandler_Mv_Gv_REXW(Code code32, Code code64, MemorySize memSize32, MemorySize memSize64) {
			this.code32 = code32;
			this.code64 = code64;
			this.memSize32 = memSize32;
			this.memSize64 = memSize64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if ((state.flags & StateFlags.W) != 0)
					instruction.InternalMemorySize = memSize64;
				else
					instruction.InternalMemorySize = memSize32;
				decoder.ReadOpMem_m64(ref instruction);
			}
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
		}
	}

	sealed class OpCodeHandler_Gv_N_Ib_REX : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_N_Ib_REX(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code32;
			instruction.InternalOpCount = 3;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + ((state.flags & StateFlags.W) != 0 ? Register.RAX : Register.EAX);
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)state.rm + Register.MM0;
			}
			else
				decoder.SetInvalidInstruction();
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_Gv_N : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_N(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code32;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + ((state.flags & StateFlags.W) != 0 ? Register.RAX : Register.EAX);
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)state.rm + Register.MM0;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_VN : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_VN(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)state.rm + Register.MM0;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_Gv_Mv : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_Mv(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalMemorySize = MemorySize.UInt32;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalMemorySize = MemorySize.UInt64;
				else
					instruction.InternalMemorySize = MemorySize.UInt16;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Mv_Gv : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Mv_Gv(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalMemorySize = MemorySize.UInt32;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalMemorySize = MemorySize.UInt64;
				else
					instruction.InternalMemorySize = MemorySize.UInt16;
				decoder.ReadOpMem_m64(ref instruction);
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
		}
	}

	sealed class OpCodeHandler_Gv_Eb_REX : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_Eb_REX(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			if (state.mod == 3) {
				uint index = state.rm + state.extraBaseRegisterBase;
				if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = MemorySize.UInt8;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ev_REX : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_Ev_REX(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase) + ((state.flags & StateFlags.W) != 0 ? Register.RAX : Register.EAX);
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.W) != 0)
					instruction.InternalMemorySize = MemorySize.UInt64;
				else
					instruction.InternalMemorySize = MemorySize.UInt32;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_GvM_VX_Ib : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;
		readonly MemorySize memSize32;
		readonly MemorySize memSize64;

		public OpCodeHandler_GvM_VX_Ib(Register baseReg, Code code32, Code code64, MemorySize memSize32, MemorySize memSize64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
			this.memSize32 = memSize32;
			this.memSize64 = memSize64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Register gpr;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				gpr = Register.EAX;
			}
			instruction.InternalOpCount = 3;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if ((state.flags & StateFlags.W) != 0)
					instruction.InternalMemorySize = memSize64;
				else
					instruction.InternalMemorySize = memSize32;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
		}
	}
}
#endif
