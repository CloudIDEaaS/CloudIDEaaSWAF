#region License
/* 
 * Copyright (C) 1999-2015 John K�ll�n.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; see the file COPYING.  If not, write to
 * the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
 */
#endregion

using Decompiler.Core;
using Decompiler.Core.Code;
using Decompiler.Core.Expressions;
using Decompiler.Core.Machine;
using Decompiler.Core.Operators;
using Decompiler.Core.Services;
using Decompiler.Core.Types;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using BitSet = Decompiler.Core.Lib.BitSet;

namespace Decompiler.Analysis
{
	using StringBuilder = System.Text.StringBuilder;
	using StringWriter = System.IO.StringWriter;

	/// <summary>
	/// Implements Interprocedural Register Liveness as described by 
	/// Robert Muth (Register Liveness of Executable Code)
	/// </summary>
	/// <remarks>
	/// Procedures need to be teased apart from each other. This is done by finding
	/// what registers are live in and live out from procedures. Once this is known,
	/// we can summarize the effects of a procedure, and use that information when
	/// rewriting call sites to SSA form.
	/// </remarks>
	public class RegisterLiveness : InstructionVisitorBase		//$REFACTOR: should be called InterproceduralLiveness
	{
		private Program prog;
		private DecompilerEventListener eventListener;
		private WorkList<BlockFlow> worklist;
		private ProgramDataFlow mpprocData;
		private State state;
		private Statement stmCur;
		private IdentifierLiveness varLive;
		private int bitUseOffset;
		private int cbitsUse;
		private IsLiveHelper isLiveHelper;

		private static TraceSwitch trace = new TraceSwitch("RegisterLiveness", "Details of register liveness analysis");

        /// <summary>
        /// Computes intraprocedural liveness of the program <paramref name="p"/>,
        /// storing the results in <paramref name="procFlow"/>.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="procFlow"></param>
        /// <returns></returns>
        public static RegisterLiveness Compute(
            Program p, 
            ProgramDataFlow procFlow, 
            DecompilerEventListener eventListener)
        {
            var live = new RegisterLiveness(p, procFlow, eventListener);
            Debug.WriteLineIf(trace.TraceError, "** Computing ByPass ****");
            live.CurrentState = new ByPassState();
            live.ProcessWorklist();

            Debug.WriteLineIf(trace.TraceError, "** Computing MayUse ****");
            live.CurrentState = new MayUseState();
            if (trace.TraceInfo) live.Dump();
            live.ProcessWorklist();

            //$REVIEW: since we never use the liveinstate, can we get rid of the following
            // four statements?
            Debug.WriteLineIf(trace.TraceError, "** Computing LiveIn ****");
            live.CurrentState = new LiveInState();
            if (trace.TraceInfo) live.Dump();
            live.ProcessWorklist();

            live.CompleteWork();
            if (trace.TraceInfo) live.Dump();

            return live;
        }

        public RegisterLiveness(
            Program prog, 
            ProgramDataFlow progFlow, 
            DecompilerEventListener eventListener)
		{
			this.prog = prog;
			this.mpprocData = progFlow;
            this.eventListener = eventListener;
            this.worklist = new WorkList<BlockFlow>();
			this.varLive = new IdentifierLiveness(prog.Architecture);
			this.isLiveHelper = new IsLiveHelper();
			AddAllBasicBlocksToWorklist();
			if (trace.TraceInfo) Dump();
		}

		private void AddAllBasicBlocksToWorklist()
		{
			foreach (Procedure proc in prog.Procedures.Values)
			{
				foreach (Block block in proc.ControlGraph.Blocks)
				{
					worklist.Add(mpprocData[block]);
				}
			}
		}
		

		/// <summary>
		/// Make summary information available in LiveIn and LiveOut for each procedure.
		/// </summary>
		private void CompleteWork()
		{
			foreach (Procedure proc in prog.Procedures.Values)
			{
				ProcedureFlow pi = mpprocData[proc];

				BlockFlow bi = mpprocData[proc.ExitBlock];
				pi.LiveOut = bi.DataOut;
				pi.grfLiveOut = bi.grfOut;

				// Remove unneeded data. Done for performance (and to give GC something to do).

				pi.ByPass = null;
			}
		}

		private void Def(Identifier id)
		{
			varLive.Def(id);
		}

		private void DumpBlock(Block block)
		{
			var sw = new StringWriter();
			block.Write(sw);
			Debug.WriteLine(sw.ToString());
		}

		private string DumpRegisters(BitSet arr)
		{
			var sb = new StringBuilder();
			var arch = prog.Architecture;
			for (int i = 0; i < arr.Count; ++i)
			{
				if (arr[i])
				{
					var reg = arch.GetRegister(i);
					if (reg != null && reg.IsAluRegister)
					{
						sb.Append(reg.Name);
						sb.Append(" ");
					}
				}
			}
			return sb.ToString();
		}

		private void InitializeWorkList()
		{
            foreach (BlockFlow bf in mpprocData.BlockFlows)
            {
                worklist.Add(bf);
            }
		}

		private void ProcessWorklist()
		{
			InitializeWorkList();
			int initial = worklist.Count;
            BlockFlow item;
            while (worklist.GetWorkItem(out item))
			{
			    eventListener.ShowProgress(string.Format("Blocks left: {0}", worklist.Count), initial - worklist.Count, initial);
				ProcessBlock(item);
			}
		}

		public Procedure Procedure { get; set; }

		/// <summary>
		/// Processes the data flow of a block by beginning with
		/// the dataOut after the last instruction, simluates all instructions
		/// until the beginning of the block is reached, and percolates any
		/// *changes* to predecessor blocks to the worklist for futher processing.
		/// </summary>
		/// <param name="item">The block dataflow item we are about to process</param>
		public void ProcessBlock(BlockFlow item)
		{
			bool t = trace.TraceInfo;
			if (t)
			{
				Debug.Write(item.Block.Procedure.Name + ", ");
				DumpBlock(item.Block);
			}
			
			varLive.BitSet = new BitSet(item.DataOut);
			varLive.Grf = item.grfOut;
            varLive.LiveStorages = new Dictionary<Storage,int>(item.StackVarsOut);
			Debug.WriteLineIf(t, string.Format("   out: {0}", DumpRegisters(varLive.BitSet)));
			Procedure = item.Block.Procedure;		// Used by statements because we need to look up registers using identifiers and the procedure frame.
			StatementList stms = item.Block.Statements;
			for (int i = stms.Count - 1; i >= 0; --i)
			{
				stmCur = stms[i];
				Debug.WriteLineIf(t, stms[i].Instruction);
				stmCur.Instruction.Accept(this);
				if (t) Debug.WriteLineIf(t, string.Format("\tin: {0}", DumpRegisters(varLive.BitSet)));
			}

			if (item.Block == item.Block.Procedure.EntryBlock)
			{
				PropagateToProcedureSummary(varLive, item.Block.Procedure);
			}
			else
			{
				PropagateToPredecessorBlocks(item);
			}
		}

		private void Dump()
		{
			foreach (Procedure proc in prog.Procedures.Values)
			{
				StringWriter sw = new StringWriter();
				ProcedureFlow flow = mpprocData[proc];
				sw.WriteLine(proc.Name);
				DataFlow.EmitRegisters(prog.Architecture, "\tByPass: ", flow.ByPass, sw); sw.WriteLine();
				DataFlow.EmitRegisters(prog.Architecture, "\tMayUse: ", flow.MayUse, sw); sw.WriteLine();
				Debug.WriteLine(sw.ToString());
			}
		}

		private void Dump(bool enable, string s, BitSet a)
		{
			if (enable)
			{
				StringWriter sw = new StringWriter();
				sw.Write("{0}: ", s);
				ProcedureFlow.EmitRegisters(prog.Architecture, "", a, sw);
				Debug.WriteLine(sw.ToString());
			}
		}


        // Consults the instruction ci and determines which of the parameters on stack are live.
		public void MarkLiveStackParameters(CallInstruction ci)
		{
            var pc = ci.Callee as ProcedureConstant;
            if (pc == null)
                return;
            var callee = pc.Procedure as Procedure;
            if (callee == null)
                return;         //$TODO: use signature to mark stack liveness.
			int cBegin = callee.Frame.GetStackArgumentSpace();
			foreach (Identifier id in Procedure.Frame.Identifiers)
			{
				var sl = id.Storage as StackLocalStorage;
				if (sl == null)
					continue;
				if (-ci.CallSite.StackDepthOnEntry <= sl.StackOffset && sl.StackOffset < cBegin - ci.CallSite.StackDepthOnEntry)
				{
					Use(id);
				}
			}
		}

		public void MergeBlockInfo(Block block)
        {
            BlockFlow blockFlow = mpprocData[block];
			if (state.MergeBlockInfo(varLive, blockFlow))
			{
                worklist.Add(blockFlow);
            }
		}
		
		public bool MergeIntoProcedureFlow(IdentifierLiveness varLive, ProcedureFlow flow)
		{
            if (varLive.BitSet[0x1F]) varLive.ToString();
			bool fChange = false;
			if (!(varLive.BitSet & ~flow.Summary).IsEmpty)
			{
				flow.Summary |= varLive.BitSet;
				fChange = true;
			}
			if ((varLive.Grf & ~flow.grfSummary) != 0)
			{
				flow.grfSummary |= varLive.Grf;
				fChange = true;
			}
			foreach (KeyValuePair<Storage,int> de in varLive.LiveStorages)
			{
				StackArgumentStorage sa = de.Key as StackArgumentStorage;
				if (sa == null)
					continue;
				int bits = de.Value;

				object o = flow.StackArguments[sa];
				if (o != null)
				{
					int bitsOld = (int) o;
					if (bitsOld < bits)
					{
						flow.StackArguments[sa] = bits;
						fChange = true;
					}
				}
				else
				{
					flow.StackArguments[sa] = bits;
					fChange = true;
				}
			}
			return fChange;
		}

		/// <summary>
		/// Propagates the live-out from a call instruction to
		/// the exit block of the function the call statement invokes.
		/// </summary>
		/// <param name="stm"></param>
		private void PropagateToCalleeExitBlocks(Statement stm)
		{
			BitSet liveOrig = new BitSet(varLive.BitSet);
			uint grfOrig = varLive.Grf;
            var stackOrig = new Dictionary<Storage, int>(varLive.LiveStorages);
			foreach (Procedure p in prog.CallGraph.Callees(stm))
			{
				var flow = mpprocData[p];
				varLive.BitSet = liveOrig - flow.PreservedRegisters;
				varLive.LiveStorages = new Dictionary<Storage,int>();
				MergeBlockInfo(p.ExitBlock);
				varLive.BitSet = new BitSet(liveOrig);
				varLive.Grf = grfOrig;
                varLive.LiveStorages = new Dictionary<Storage, int>(stackOrig);
			}
		}

		/// <summary>
		/// Propagates liveness information to predecessor blocks, but only
		/// if there are changes to their dataOut fields.
		/// </summary>
		/// <param name="item"></param>
		private void PropagateToPredecessorBlocks(BlockFlow item)
		{
			foreach (Block p in item.Block.Pred)
			{
				MergeBlockInfo(p);
			}
		}

		/// <summary>
		/// When liveness analysis reaches the entry block of the procedure,
		/// update the procedure summary information with the current set of
		/// live registers.
		/// </summary>
		/// <param name="p"></param>
		public void PropagateToProcedureSummary(IdentifierLiveness varLive, Procedure p)
		{
			ProcedureFlow flow = mpprocData[p];
            state.ApplySavedRegisters(flow, varLive);
			var change = MergeIntoProcedureFlow(varLive, flow);
			if (change)
			{
				Debug.WriteLineIf(trace.TraceInfo, flow.EmitRegisters(prog.Architecture, p.Name + " summary:", flow.Summary));
				state.UpdateSummary(flow);
                foreach (Statement stmCaller in prog.CallGraph.CallerStatements(p))
				{
					Debug.WriteLineIf(trace.TraceVerbose, string.Format("Propagating to {0} (block {1} in {2}", stmCaller.Instruction.ToString(), stmCaller.Block.Name, stmCaller.Block.Procedure.Name));
					worklist.Add(mpprocData[stmCaller.Block]);
				}
			}
		}

		private void Use(Identifier id)
		{
			varLive.Use(id, bitUseOffset, cbitsUse);
		}
 
		public IdentifierLiveness IdentifierLiveness
		{
			get { return varLive; }
		}

		public void VisitAssignmentInner(Identifier idDst, Expression src)
		{
			Def(idDst);
			bitUseOffset = varLive.DefOffset;
			cbitsUse = varLive.DefBitSize;
			src.Accept(this);
		}

		public void VisitCopy(Identifier idDst, Identifier idSrc)
		{
			if (isLiveHelper.IsLive(idDst, this.varLive))
			{
				VisitAssignmentInner(idDst, idSrc);
			}
			else
			{
				Def(idDst);
			}
		}

		#region InstructionVisitor methods ////////////////////////////////////////////////////////////

		private void Dump(string caption, IdentifierLiveness vl)
		{
			StringWriter sw = new System.IO.StringWriter();
			vl.Write(sw, caption);
			Debug.WriteLine(sw.ToString());
		}	

		public override void VisitAssignment(Assignment a)
		{
			Identifier idSrc = a.Src as Identifier;
			if (idSrc != null)
			{
				VisitCopy(a.Dst, idSrc);
			}
			else
			{
				VisitAssignmentInner(a.Dst, a.Src);
			}
		}

		public override void VisitApplication(Application appl)
		{
			appl.Procedure.Accept(this);
			for (int i = 0; i < appl.Arguments.Length; ++i)
			{
				bitUseOffset = 0;
				cbitsUse = 0;
				var outArg = appl.Arguments[i] as OutArgument;
                if (outArg != null)
                {
					Identifier id = outArg.Expression as Identifier;
                    if (id != null)
                    {
                        Def(id);
                    }
				}
			}
			for (int i = 0; i < appl.Arguments.Length; ++i)
			{	
				OutArgument u = appl.Arguments[i] as OutArgument;
				if (u == null || !(u.Expression is Identifier))
				{
					appl.Arguments[i].Accept(this);
				}
			}
		}

		public override void VisitBinaryExpression(BinaryExpression binExp)
		{
			if (binExp.Operator is ConditionalOperator ||
                binExp.Operator == BinaryOperator.IMul ||
                binExp.Operator == BinaryOperator.FMul ||
				binExp.Operator == BinaryOperator.SMul ||
				binExp.Operator == BinaryOperator.UMul ||
				binExp.Operator == BinaryOperator.SDiv ||
				binExp.Operator == BinaryOperator.UDiv)
			{
				bitUseOffset = 0;
				cbitsUse = 0;
			}
			binExp.Left.Accept(this);

			if (binExp.Operator == BinaryOperator.Shl ||
				binExp.Operator == BinaryOperator.Sar ||
                binExp.Operator == BinaryOperator.IMul ||
                binExp.Operator == BinaryOperator.FMul ||
				binExp.Operator == BinaryOperator.SMul ||
				binExp.Operator == BinaryOperator.UMul ||
				binExp.Operator == BinaryOperator.SDiv ||
				binExp.Operator == BinaryOperator.UDiv)
			{
				bitUseOffset = 0;
				cbitsUse = 0;
			}
			binExp.Right.Accept(this);
		}

		public override void VisitCallInstruction(CallInstruction ci)
		{
            ProcedureSignature sig = GetProcedureSignature(ci.Callee);
			if (sig != null && sig.ParametersValid)		
			{
                var procCallee = ((ProcedureConstant) ci.Callee).Procedure;
                var ab = new ApplicationBuilder(
                    prog.Architecture, 
                    Procedure.Frame, 
                    ci.CallSite,
                    new ProcedureConstant(prog.Platform.PointerType, procCallee), 
                    sig, 
                    false);
				if (sig.ReturnValue != null)
				{
                    varLive.Def(ab.Bind(sig.ReturnValue));
				}
				foreach (Identifier arg in sig.Parameters)
				{
					if (arg.Storage is OutArgumentStorage)
					{
                        varLive.Def(ab.Bind(arg));
					}
				}

				foreach (Identifier arg in sig.Parameters)
				{
					if (!(arg.Storage is OutArgumentStorage))
					{
                        varLive.Use(ab.Bind(arg));
					}
				}
			}
			else
			{
                var pc = ci.Callee as ProcedureConstant;
                if (pc == null)
                    return;
                var procCallee = pc.Procedure as Procedure;
                if (procCallee == null)
                    return;
                if (varLive.BitSet[0x1F]) varLive.ToString();

                if (state.PropagateThroughExitNodes)
				{
					PropagateToCalleeExitBlocks(stmCur);
				}

				// Update trash information.

				ProcedureFlow pi = mpprocData[procCallee];
				ProcedureFlow item = mpprocData[Procedure];
                if (varLive.BitSet[0x1F]) varLive.ToString();

				// The registers that are still live before a call are those
				// that were live after the call and were bypassed by the called function
				// or used by the called function.
				varLive.BitSet = pi.MayUse | ((pi.ByPass    | ~pi.TrashedRegisters) & varLive.BitSet);
				varLive.Grf = pi.grfMayUse | ((pi.grfByPass | ~pi.grfTrashed) & varLive.Grf);
                if (varLive.BitSet[0x1F]) varLive.ToString();
				// Any stack parameters are also considered live.
				MarkLiveStackParameters(ci);
			}
		}

        private ProcedureSignature GetProcedureSignature(Expression expr)
        {
            var pc = expr as ProcedureConstant;
            if (pc == null) return null;
            return pc.Procedure.Signature;
        }

        public override void VisitCast(Cast cast)
        {
            base.VisitCast(cast);
        }

		public override void VisitConditionOf(ConditionOf cof)
		{
			bitUseOffset = 0;
			cbitsUse = 0;
			base.VisitConditionOf (cof);
		}

		public override void VisitIdentifier(Identifier id)
		{
			Use(id);
		}

		public override void VisitMemoryAccess(MemoryAccess access)
		{
			bitUseOffset = 0;
			cbitsUse = 0;
			access.EffectiveAddress.Accept(this);
		}

		public override void VisitSegmentedAccess(SegmentedAccess access)
		{
			bitUseOffset = 0;
			cbitsUse = 0;
			base.VisitSegmentedAccess(access);
		}

        public override void VisitOutArgument(OutArgument outArg)
        {
            base.VisitOutArgument(outArg);
        }

		public override void VisitStore(Store store)
		{
			store.Dst.Accept(this);
			bitUseOffset = 0;
			cbitsUse = store.Dst.DataType.BitSize;
			store.Src.Accept(this);
		}


		#endregion // Visitor Methods //////////////////////////////////////

        public State CurrentState
        {
            get { return state; }
            set
            {
                if (value == null) 
                    throw new ArgumentNullException();
                state = value;
                foreach (ProcedureFlow pi in mpprocData.ProcedureFlows)
                {
                    state.InitializeProcedureFlow(pi);
                }
                foreach (BlockFlow bi in mpprocData.BlockFlows)
                {
                    state.InitializeBlockFlow(bi.Block, mpprocData, bi.Block.Procedure.ExitBlock == bi.Block);
                }
            }
        }

		public abstract class State
		{
			private bool propagateThroughExitNodes;

			public State(bool prop) 
			{
				this.propagateThroughExitNodes = prop;
			}

			public abstract void InitializeBlockFlow(Block blow, ProgramDataFlow flow, bool isExitBlock);

			public abstract void InitializeProcedureFlow(ProcedureFlow flow);

			public abstract void ApplySavedRegisters(ProcedureFlow procFlow, IdentifierLiveness varLive);

            /// <summary>
            /// Merges the liveness information accumulated in <paramref name="varLive"/> into <paramref name="blockFlow"/>
            /// </summary>
            /// <param name="varLive">Current liveness state</param>
            /// <param name="blockFlow">liveness information associated with a block</param>
            /// <returns>Return true if merging the livness information changed the information in the block flow.</returns>
			public virtual bool MergeBlockInfo(IdentifierLiveness varLive, BlockFlow blockFlow)
			{
				bool ret = false;
				if (!(varLive.BitSet & (~blockFlow.DataOut)).IsEmpty)
				{
					blockFlow.DataOut |= varLive.BitSet;
					ret = true;
				}
				if ((varLive.Grf & (~blockFlow.grfOut)) != 0)
				{
					blockFlow.grfOut |= varLive.Grf;
					ret = true;
				}
				IDictionary<Storage, int> dict = blockFlow.StackVarsOut;
				foreach (KeyValuePair<Storage,int> de in varLive.LiveStorages)
				{
					if (!dict.ContainsKey(de.Key))
					{
						dict.Add(de.Key, de.Value);
						ret = true;
					}
				}
				return ret;
			}

			public bool PropagateThroughExitNodes
			{
				get { return this.propagateThroughExitNodes; }
			}

			public abstract void UpdateSummary(ProcedureFlow item);
		}


		public class ByPassState : State
		{
			public ByPassState() : base(false)
			{
			}

			public override void ApplySavedRegisters(ProcedureFlow procFlow, IdentifierLiveness varLive)
			{
//				varLive.BitSet &= ~procFlow.PreservedRegisters;
			}

			public override void InitializeBlockFlow(Block block, ProgramDataFlow flow, bool isExitBlock)
			{
				BlockFlow bf = flow[block];
				if (isExitBlock && block.Procedure.Signature.ParametersValid)
				{
					Identifier ret = block.Procedure.Signature.ReturnValue;
					if (ret != null)
					{
						RegisterStorage rs = ret.Storage as RegisterStorage;
						if (rs != null)
							rs.SetAliases(bf.DataOut, true);
					}
					foreach (Identifier id in block.Procedure.Signature.Parameters)
					{
						OutArgumentStorage os = id.Storage as OutArgumentStorage;
						if (os == null)
							continue;
						RegisterStorage rs = os.OriginalIdentifier.Storage as RegisterStorage;
						if (rs != null) 
						{
							rs.SetAliases(bf.DataOut, true);
						}
					}
				}
                else if (bf.TerminatesProcess)
                {
                    bf.DataOut.SetAll(false);
                }
                else
				{
					bf.DataOut.SetAll(isExitBlock);
					bf.DataOut &= ~flow[block.Procedure].PreservedRegisters;
				}
			}

			public override void InitializeProcedureFlow(ProcedureFlow flow)
			{
				flow.Summary = flow.ByPass;
				flow.Summary.SetAll(false);
			}

			public override void UpdateSummary(ProcedureFlow item)
			{
				item.ByPass = item.Summary.Clone();
			}

			public override bool MergeBlockInfo(IdentifierLiveness varLive, BlockFlow blockFlow)
			{
				bool ret = false;
				if (!(~varLive.BitSet & blockFlow.DataOut).IsEmpty)
				{
					blockFlow.DataOut |= varLive.BitSet;
					ret = true;
				}
				if ((~varLive.Grf & blockFlow.grfOut) != 0)
				{
					blockFlow.grfOut |= varLive.Grf;
					ret = true;
				}
				return ret;
			}
		}


		private class MayUseState : State
		{
			public MayUseState() : base(false)
			{
			}

			public override void ApplySavedRegisters(ProcedureFlow procFlow, IdentifierLiveness varLive)
			{
			}

			public override void InitializeBlockFlow(Block block, ProgramDataFlow flow, bool isExitBlock)
			{
				flow[block].DataOut.SetAll(false);
			}

			public override void InitializeProcedureFlow(ProcedureFlow flow)
			{
				flow.ByPass = flow.Summary.Clone();
				flow.Summary.SetAll(false);
				flow.grfByPass = flow.grfSummary;
				flow.grfSummary = 0;
			}

			public override void UpdateSummary(ProcedureFlow item)
			{
				item.MayUse = item.Summary.Clone();
			}
		}


		private class LiveInState : State
		{
			public LiveInState() : base(true)
			{
			}

			public override void InitializeBlockFlow(Block block, ProgramDataFlow flow, bool isExitBlock)
			{
				flow[block].DataOut.SetAll(false);
			}

			public override void InitializeProcedureFlow(ProcedureFlow flow)
			{
				flow.MayUse = new BitSet(flow.Summary);
				flow.Summary.SetAll(false);
				flow.grfMayUse = flow.grfSummary;
				flow.grfSummary = 0;
			}

			public override void ApplySavedRegisters(ProcedureFlow procFlow, IdentifierLiveness varLive)
			{
			}

			public override void UpdateSummary(ProcedureFlow item)
			{
				// Used to update LiveIn, but it was never used.
			}
		}

		/// <summary>
		/// Determines whether an identifier is live in <paramref>liveStat</paramref>.
		/// </summary>
		public class IsLiveHelper : StorageVisitor<bool>
		{
			private bool retval;
			private IdentifierLiveness liveState;

			public bool IsLive(Identifier id, IdentifierLiveness liveState)
			{
				retval = false;
				this.liveState = liveState;
				retval = id.Storage.Accept(this);
				return retval;
			}

			#region StorageVisitor Members

			public bool VisitTemporaryStorage(TemporaryStorage temp)
			{
				return liveState.LiveStorages.ContainsKey(temp);
			}

			public bool VisitStackArgumentStorage(StackArgumentStorage stack)
			{
				return liveState.LiveStorages.ContainsKey(stack);
			}

			public bool VisitOutArgumentStorage(OutArgumentStorage arg)
			{
				throw new NotImplementedException();
			}

			public bool VisitMemoryStorage(MemoryStorage global)
			{
				throw new NotImplementedException();
			}

			public bool VisitRegisterStorage(RegisterStorage reg)
			{
				//$REFACTOR: make SetAliases be a bitset of Register.
				BitSet b = new BitSet(liveState.BitSet.Count);
				reg.SetAliases(b, true);
				return !(liveState.BitSet & b).IsEmpty;
			}

			public bool VisitFpuStackStorage(FpuStackStorage fpu)
			{
				return liveState.LiveStorages.ContainsKey(fpu);
			}

			public bool VisitFlagGroupStorage(FlagGroupStorage grf)
			{
				return (grf.FlagGroupBits & liveState.Grf) != 0;
			}

			public bool VisitSequenceStorage(SequenceStorage seq)
			{
				var f = seq.Head.Storage.Accept(this);
				if (!f)
					f = seq.Tail.Storage.Accept(this);
                return f;
			}

			public bool VisitStackLocalStorage(StackLocalStorage local)
			{
				return liveState.LiveStorages.ContainsKey(local);
			}

			#endregion

		}

	}
}
