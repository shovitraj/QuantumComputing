// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

//////////////////////////////////////////////////////////////////////
// This file contains testing harness for all tasks.
// You should not modify anything in this file.
// The tasks themselves can be found in Tasks.qs file.
//////////////////////////////////////////////////////////////////////

namespace Quantum.Kata.BasicGates {
    
    open Microsoft.Quantum.Arrays;
    open Microsoft.Quantum.Intrinsic;
    open Microsoft.Quantum.Canon;
    open Microsoft.Quantum.Convert;
    open Microsoft.Quantum.Math;
    open Microsoft.Quantum.Diagnostics;
    
    //////////////////////////////////////////////////////////////////
    // Part I. Single-Qubit Gates
    //////////////////////////////////////////////////////////////////

    // The tests in part I are written to test controlled versions of operations instead of plain ones.
    // This is done to verify that the tasks don't add a global phase to the implementations.
    // Global phase is not relevant physically, but it can be very confusing for a beginner to consider R1 vs Rz,
    // so the tests use controlled version of the operations which converts the global phase into a relative phase
    // and makes it possible to detect.

    // ------------------------------------------------------
    // Helper wrapper to represent operation on one qubit 
    // as an operation on an array of one qubits
    operation ArrayWrapper (op : (Qubit => Unit is Adj+Ctl), qs : Qubit[]) : Unit is Adj+Ctl {
        op(qs[0]);
    }

    // ------------------------------------------------------
    // Helper wrapper to represent controlled variant of operation on one qubit 
    // as an operation on an array of two qubits
    operation ArrayWrapperControlled (op : (Qubit => Unit is Adj+Ctl), qs : Qubit[]) : Unit is Adj+Ctl {
        Controlled op([qs[0]], qs[1]);
    }


    // ------------------------------------------------------
    // Helper operation to show the difference between the reference solution and the learner's one
    operation DumpDiff (N : Int, 
                        statePrep : (Qubit[] => Unit is Adj+Ctl),
                        testImpl : (Qubit[] => Unit is Adj+Ctl),
                        refImpl : (Qubit[] => Unit is Adj+Ctl)
                       ) : Unit {
        using (qs = Qubit[N]) {
            // Prepare the input state and show it
            statePrep(qs);
            Message("The starting state:");
            DumpMachine();

            // Apply the reference solution and show result
            refImpl(qs);
            Message("The desired state:");
            DumpMachine();
            ResetAll(qs);

            // Prepare the input state again for test implementation
            statePrep(qs);
            // Apply learner's solution and show result
            testImpl(qs);
            Message("The actual state:");
            DumpMachine();
            ResetAll(qs);
        }
    }


    // Used for single-qubit operations that are unlikely to introduce the extra global phase
    operation DumpDiffOnOneQubit (testImpl : (Qubit => Unit is Adj+Ctl),
                                  refImpl : (Qubit => Unit is Adj+Ctl)) : Unit {
        DumpDiff(1, ArrayWrapper(Ry(2.0 * ArcCos(0.6), _), _), 
                ArrayWrapper(testImpl, _), 
                ArrayWrapper(refImpl, _));
    }


    // ------------------------------------------------------
    operation T101_StateFlip_Test () : Unit {
        DumpDiffOnOneQubit(StateFlip, StateFlip_Reference);
        AssertOperationsEqualReferenced(2, ArrayWrapperControlled(StateFlip, _), 
                                           ArrayWrapperControlled(StateFlip_Reference, _));
    }
    
    
    // ------------------------------------------------------
    operation T102_BasisChange_Test () : Unit {
        DumpDiffOnOneQubit(BasisChange, BasisChange_Reference);
        AssertOperationsEqualReferenced(2, ArrayWrapperControlled(BasisChange, _), 
                                           ArrayWrapperControlled(BasisChange_Reference, _));
    }
    
    
    // ------------------------------------------------------
    operation T103_SignFlip_Test () : Unit {
        DumpDiffOnOneQubit(SignFlip, SignFlip_Reference);
        AssertOperationsEqualReferenced(2, ArrayWrapperControlled(SignFlip, _), 
                                           ArrayWrapperControlled(SignFlip_Reference, _));
    }
    
    
    // ------------------------------------------------------
    operation T104_AmplitudeChange_Test () : Unit {
        // pick one rotation angle on which to show difference between solutions
        let dumpAlpha = ((2.0 * PI()) * IntAsDouble(6)) / 36.0;
        Message($"Applying amplitude change with alpha = {dumpAlpha}");
        DumpDiffOnOneQubit(AmplitudeChange(dumpAlpha, _), AmplitudeChange_Reference(dumpAlpha, _));

        for (i in 0 .. 36) {
            let alpha = ((2.0 * PI()) * IntAsDouble(i)) / 36.0;
            AssertOperationsEqualReferenced(2, ArrayWrapperControlled(AmplitudeChange(alpha, _), _), 
                                               ArrayWrapperControlled(AmplitudeChange_Reference(alpha, _), _));
        }
    }
    
    
    // ------------------------------------------------------
    operation T105_PhaseFlip_Test () : Unit {
        DumpDiffOnOneQubit(PhaseFlip, PhaseFlip_Reference);
        AssertOperationsEqualReferenced(2, ArrayWrapperControlled(PhaseFlip, _), 
                                           ArrayWrapperControlled(PhaseFlip_Reference, _));
    }
    
    
    // ------------------------------------------------------
    operation T106_PhaseChange_Test () : Unit {
        let dumpAlpha = ((2.0 * PI()) * IntAsDouble(10)) / 36.0;
        Message($"Applying phase change with alpha = {dumpAlpha}");
        DumpDiffOnOneQubit(PhaseChange(dumpAlpha,_), PhaseChange_Reference(dumpAlpha,_));

        for (i in 0 .. 36) {
            let alpha = ((2.0 * PI()) * IntAsDouble(i)) / 36.0;
            AssertOperationsEqualReferenced(2, ArrayWrapperControlled(PhaseChange(alpha, _), _), 
                                               ArrayWrapperControlled(PhaseChange_Reference(alpha, _), _));
        }
    }
    
    
    // ------------------------------------------------------
    // State prep for showing the controlled version of single-qubit operation
    operation StatePrepForControlled (qs : Qubit[]) : Unit is Adj+Ctl {
        H(qs[0]);
        Ry(2.0 * ArcCos(0.6), qs[1]);
    }

    operation T107_GlobalPhaseChange_Test () : Unit {
        // use the controlled version of unitaries for showing the difference, since it's hard to observe on non-controlled versions
        Message("Showing effect of controlled-GlobalPhaseChange");
        DumpDiff(2, StatePrepForControlled, 
                ArrayWrapperControlled(GlobalPhaseChange, _), 
                ArrayWrapperControlled(GlobalPhaseChange_Reference, _));

        AssertOperationsEqualReferenced(2, ArrayWrapperControlled(GlobalPhaseChange, _), 
                                           ArrayWrapperControlled(GlobalPhaseChange_Reference, _));
    }
    

    // ------------------------------------------------------
    // 0 - |Φ⁺⟩ = (|00⟩ + |11⟩) / sqrt(2)
    // 1 - |Φ⁻⟩ = (|00⟩ - |11⟩) / sqrt(2)
    // 2 - |Ψ⁺⟩ = (|01⟩ + |10⟩) / sqrt(2)
    // 3 - |Ψ⁻⟩ = (|01⟩ - |10⟩) / sqrt(2)
    operation StatePrep_BellState (qs : Qubit[], state : Int) : Unit is Adj+Ctl {
        
        H(qs[0]);
        CNOT(qs[0], qs[1]);
            
        // now we have |00⟩ + |11⟩ - modify it based on state arg
        if (state % 2 == 1) {
            // negative phase
            Z(qs[1]);
        }
            
        if (state / 2 == 1) {
            X(qs[1]);
        }
    }
    
    
    // ------------------------------------------------------
    operation VerifyBellStateConversion (testOp : (Qubit[] => Unit is Adj+Ctl), startState : Int, targetState : Int) : Unit {
        // (note the use of controlled versions of operations to keep track of the phase potentially acquired by testOp)
        using (qs = Qubit[3]) {
            H(qs[0]);

            // prepare Bell state startState
            Controlled StatePrep_BellState([qs[0]], (Rest(qs), startState));
            
            // apply operation that needs to be tested
            Controlled testOp([qs[0]], Rest(qs));
            
            // verify the result by applying adjoint of state prep for target state
            Controlled Adjoint StatePrep_BellState([qs[0]], (Rest(qs), targetState));
            
            H(qs[0]);

            // assert that all qubits end up in |0⟩ state
            AssertAllZero(qs);
        }
    }


    // ------------------------------------------------------
    operation T108_BellStateChange1_Test () : Unit {
        DumpDiff(2, StatePrep_BellState(_, 0), BellStateChange1, BellStateChange1_Reference);
        VerifyBellStateConversion(BellStateChange1, 0, 1);
    }
    
    
    // ------------------------------------------------------
    operation T109_BellStateChange2_Test () : Unit {
        DumpDiff(2, StatePrep_BellState(_, 0), BellStateChange2, BellStateChange2_Reference);
        VerifyBellStateConversion(BellStateChange2, 0, 2);
    }
    
    
    // ------------------------------------------------------
    operation T110_BellStateChange3_Test () : Unit {
        DumpDiff(2, StatePrep_BellState(_, 0), BellStateChange3, BellStateChange3_Reference);
        Message("If the desired and the actual states match but the test doesn't pass, check whether your solution introduces a global phase; it shouldn't!");
        VerifyBellStateConversion(BellStateChange3, 0, 3);
    }
    
    
    // ------------------------------------------------------
    operation StatePrepRy (qs : Qubit[]) : Unit is Adj+Ctl {
        Ry(2.0 * (2.0 * PI() * 6.0) / 36.0, Head(qs));
    }

    operation T201_TwoQubitGate1_Test () : Unit {
        DumpDiff(2, StatePrepRy, TwoQubitGate1, TwoQubitGate1_Reference);

        // Note that the way the problem is formulated, we can't just compare two unitaries,
        // we need to create a specific input state and check that the output state is correct
        using (qs = Qubit[2]) {
            for (i in 0 .. 36) {
                let alpha = ((2.0 * PI()) * IntAsDouble(i)) / 36.0;
                
                within {
                    // prepare state cos(α) * |0⟩ + sin(α) * |1⟩
                    Ry(2.0 * alpha, qs[0]);
                }
                apply {
                    // apply operation that needs to be tested
                    TwoQubitGate1(qs);

                    // apply adjoint reference operation
                    Adjoint TwoQubitGate1_Reference(qs);
                }
                
                // assert that all qubits end up in |0⟩ state
                AssertAllZero(qs);
            }
        }
    }
    
    
    // ------------------------------------------------------
    operation T202_TwoQubitGate2_Test () : Unit {
        DumpDiff(2, ApplyToEachCA(H, _), TwoQubitGate2, TwoQubitGate2_Reference);
        using (qs = Qubit[2]) {
            within {
                // prepare |+⟩ ⊗ |+⟩ state
                ApplyToEachCA(H, qs);
            } apply {
                // apply operation that needs to be tested
                TwoQubitGate2(qs);
            
                // apply adjoint reference operation
                Adjoint TwoQubitGate2_Reference(qs);
            }
            
            // assert that all qubits end up in |0⟩ state
            AssertAllZero(qs);
        }
    }
    
    
    // ------------------------------------------------------
    // Prepare a state for tests 2.3-2.5
    operation StatePrepMiscAmplitudes (qs : Qubit[]) : Unit is Adj+Ctl {
        let alphas = [5.0, 10.0, 15.0];
        for (index in 0 .. Length(qs) - 1) {
            Ry(2.0 * (alphas[index] + IntAsDouble(index + 1)), qs[index]);
        }
    }
    

    // ------------------------------------------------------
    operation SwapWrapper (qs : Qubit[]) : Unit is Adj {
        SWAP(qs[0], qs[1]);
    }
    
    
    operation T203_TwoQubitGate3_Test () : Unit {
        DumpDiff(2, StatePrepMiscAmplitudes, TwoQubitGate3, TwoQubitGate3_Reference);
        AssertOperationsEqualReferenced(2, SwapWrapper, TwoQubitGate3_Reference);
        AssertOperationsEqualReferenced(2, TwoQubitGate3, TwoQubitGate3_Reference);
    }
    
    
    // ------------------------------------------------------
    operation T204_ToffoliGate_Test () : Unit {
        DumpDiff(3, StatePrepMiscAmplitudes, ToffoliGate, ToffoliGate_Reference);
        AssertOperationsEqualReferenced(3, ToffoliGate, ToffoliGate_Reference);
    }
    
    
    // ------------------------------------------------------
    operation T205_FredkinGate_Test () : Unit {
        DumpDiff(3, StatePrepMiscAmplitudes, FredkinGate, FredkinGate_Reference);
        AssertOperationsEqualReferenced(3, FredkinGate, FredkinGate_Reference);
    }
}
