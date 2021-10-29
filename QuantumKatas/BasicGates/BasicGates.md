# Basic Gates Kata

**Basic Gates** quantum kata is a series of exercises designed
to get you familiar with the basic quantum gates in Q#.
It covers the following topics:
* basic single-qubit and multi-qubit gates,
* adjoint and controlled gates,
* using gates to modify the state of a qubit.

Each task is wrapped in one operation preceded by the description of the task.
Your goal is to fill in the blank (marked with `// ...` comments)
with some Q# code that solves the task. To verify your answer, run the cell using Ctrl/⌘+Enter.

Most tasks in this kata can be done using exactly one gate.
None of the tasks require measurement, and the tests are written so as to fail if qubit state is measured.

The tasks are given in approximate order of increasing difficulty; harder ones are marked with asterisks.

To begin, first prepare this notebook for execution (if you skip this step, you'll get "Syntax does not match any known patterns" error when you try to execute Q# code in the next cells):


```qsharp
%package Microsoft.Quantum.Katas::0.12.20070124
```


    Adding package Microsoft.Quantum.Katas::0.12.20070124: done!





<ul><li>Microsoft.Quantum.Standard::0.12.20070124</li><li>Microsoft.Quantum.Katas::0.12.20070124</li></ul>



> The package versions in the output of the cell above should always match. If you are running the Notebooks locally and the versions do not match, please install the IQ# version that matches the version of the `Microsoft.Quantum.Katas` package.
> <details>
> <summary><u>How to install the right IQ# version</u></summary>
> For example, if the version of `Microsoft.Quantum.Katas` package above is 0.1.2.3, the installation steps are as follows:
>
> 1. Stop the kernel.
> 2. Uninstall the existing version of IQ#:
>        dotnet tool uninstall microsoft.quantum.iqsharp -g
> 3. Install the matching version:
>        dotnet tool install microsoft.quantum.iqsharp -g --version 0.1.2.3
> 4. Reinstall the kernel:
>        dotnet iqsharp install
> 5. Restart the Notebook.
> </details>


In all tasks you need to implement the transformation exactly, without introducing any global phase. You can read more about the global phase of quantum states [here](../tutorials/Qubit/Qubit.ipynb#Relative-and-Global-Phase).

## Part I. Single-Qubit Gates


### Theory

* A list of most common gates can be found in [this Wikipedia article](https://en.wikipedia.org/wiki/Quantum_logic_gate).
* [Quirk](http://algassert.com/quirk) is a convenient tool for visualizing the effect of gates on qubit states.

### Q# materials

* Basic gates provided in Q# belong to the `Microsoft.Quantum.Intrinsic` namespace and are listed [here](https://docs.microsoft.com/qsharp/api/qsharp/microsoft.quantum.intrinsic).

> Note that all operations in this section have `is Adj+Ctl` in their signature.
This means that they should be implemented in a way that allows Q# 
to compute their adjoint and controlled variants automatically.
Since each task is solved using only intrinsic gates, you should not need to put any special effort in this.

### Task 1.1. State flip: $|0\rangle$ to $|1\rangle$ and vice versa

**Input:** A qubit in state $|\psi\rangle = \alpha |0\rangle + \beta |1\rangle$.

**Goal:**  Change the state of the qubit to $\alpha |1\rangle + \beta |0\rangle$.

**Example:**

If the qubit is in state $|0\rangle$, change its state to $|1\rangle$.

If the qubit is in state $|1\rangle$, change its state to $|0\rangle$.

> Note that this operation is self-adjoint: applying it for a second time
> returns the qubit to the original state. 


```qsharp
%kata T101_StateFlip_Test 

operation StateFlip (q : Qubit) : Unit is Adj+Ctl {
    // The Pauli X gate will change the |0⟩ state to the |1⟩ state and vice versa.
    // Type X(q);
    // Then run the cell using Ctrl/⌘+Enter.
       X(q);
    // ...
}
```

    The starting state:




                    <table style="table-layout: fixed; width: 100%">
                        <thead>
                            
                        <tr>
                            <th>Qubit IDs</th>
                            <td span="3">0</td>
                        </tr>
                    
                            <tr>
                                <th style="width: 20ch)">Basis state (little endian)</th>
                                <th style="width: 20ch">Amplitude</th>
                                <th style="width: calc(100% - 26ch - 20ch)">Meas. Pr.</th>
                                <th style="width: 6ch">Phase</th>
                            </tr>
                        </thead>

                        <tbody>
                            
                            <tr>
                                <td>$\left|0\right\rangle$</td>
                                <td>$0.6000 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="36"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(0deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>


                            <tr>
                                <td>$\left|1\right\rangle$</td>
                                <td>$0.8000 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="64.00000000000001"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(0deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>
                        
                        </tbody>
                    </table>



    The desired state:




                    <table style="table-layout: fixed; width: 100%">
                        <thead>
                            
                        <tr>
                            <th>Qubit IDs</th>
                            <td span="3">0</td>
                        </tr>
                    
                            <tr>
                                <th style="width: 20ch)">Basis state (little endian)</th>
                                <th style="width: 20ch">Amplitude</th>
                                <th style="width: calc(100% - 26ch - 20ch)">Meas. Pr.</th>
                                <th style="width: 6ch">Phase</th>
                            </tr>
                        </thead>

                        <tbody>
                            
                            <tr>
                                <td>$\left|0\right\rangle$</td>
                                <td>$0.8000 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="64.00000000000001"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(0deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>


                            <tr>
                                <td>$\left|1\right\rangle$</td>
                                <td>$0.6000 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="36"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(0deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>
                        
                        </tbody>
                    </table>



    The actual state:




                    <table style="table-layout: fixed; width: 100%">
                        <thead>
                            
                        <tr>
                            <th>Qubit IDs</th>
                            <td span="3">0</td>
                        </tr>
                    
                            <tr>
                                <th style="width: 20ch)">Basis state (little endian)</th>
                                <th style="width: 20ch">Amplitude</th>
                                <th style="width: calc(100% - 26ch - 20ch)">Meas. Pr.</th>
                                <th style="width: 6ch">Phase</th>
                            </tr>
                        </thead>

                        <tbody>
                            
                            <tr>
                                <td>$\left|0\right\rangle$</td>
                                <td>$0.8000 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="64.00000000000001"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(0deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>


                            <tr>
                                <td>$\left|1\right\rangle$</td>
                                <td>$0.6000 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="36"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(0deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>
                        
                        </tbody>
                    </table>






    Success!



### Task 1.2. Basis change: $|0\rangle$ to $|+\rangle$ and $|1\rangle$ to $|-\rangle$ (and vice versa)

**Input**: A qubit in state $|\psi\rangle = \alpha |0\rangle + \beta |1\rangle$.

**Goal**:  Change the state of the qubit as follows:
* If the qubit is in state $|0\rangle$, change its state to $|+\rangle = \frac{1}{\sqrt{2}} \big(|0\rangle + |1\rangle\big)$.
* If the qubit is in state $|1\rangle$, change its state to $|-\rangle = \frac{1}{\sqrt{2}} \big(|0\rangle - |1\rangle\big)$.
* If the qubit is in superposition, change its state according to the effect on basis vectors.

> Note:  
> $|+\rangle$ and $|-\rangle$ form a different basis for single-qubit states, called X basis.  
> $|0\rangle$ and $|1\rangle$ are called Z basis.



```qsharp
%kata T102_BasisChange_Test 

operation BasisChange (q : Qubit) : Unit is Adj+Ctl {
    // ...
    H(q);
}
```

    The starting state:




                    <table style="table-layout: fixed; width: 100%">
                        <thead>
                            
                        <tr>
                            <th>Qubit IDs</th>
                            <td span="3">0</td>
                        </tr>
                    
                            <tr>
                                <th style="width: 20ch)">Basis state (little endian)</th>
                                <th style="width: 20ch">Amplitude</th>
                                <th style="width: calc(100% - 26ch - 20ch)">Meas. Pr.</th>
                                <th style="width: 6ch">Phase</th>
                            </tr>
                        </thead>

                        <tbody>
                            
                            <tr>
                                <td>$\left|0\right\rangle$</td>
                                <td>$0.6000 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="36"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(0deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>


                            <tr>
                                <td>$\left|1\right\rangle$</td>
                                <td>$0.8000 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="64.00000000000001"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(0deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>
                        
                        </tbody>
                    </table>



    The desired state:




                    <table style="table-layout: fixed; width: 100%">
                        <thead>
                            
                        <tr>
                            <th>Qubit IDs</th>
                            <td span="3">0</td>
                        </tr>
                    
                            <tr>
                                <th style="width: 20ch)">Basis state (little endian)</th>
                                <th style="width: 20ch">Amplitude</th>
                                <th style="width: calc(100% - 26ch - 20ch)">Meas. Pr.</th>
                                <th style="width: 6ch">Phase</th>
                            </tr>
                        </thead>

                        <tbody>
                            
                            <tr>
                                <td>$\left|0\right\rangle$</td>
                                <td>$0.9899 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="98.00000000000001"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(0deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>


                            <tr>
                                <td>$\left|1\right\rangle$</td>
                                <td>$-0.1414 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="2.0000000000000027"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(180deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>
                        
                        </tbody>
                    </table>



    The actual state:




                    <table style="table-layout: fixed; width: 100%">
                        <thead>
                            
                        <tr>
                            <th>Qubit IDs</th>
                            <td span="3">0</td>
                        </tr>
                    
                            <tr>
                                <th style="width: 20ch)">Basis state (little endian)</th>
                                <th style="width: 20ch">Amplitude</th>
                                <th style="width: calc(100% - 26ch - 20ch)">Meas. Pr.</th>
                                <th style="width: 6ch">Phase</th>
                            </tr>
                        </thead>

                        <tbody>
                            
                            <tr>
                                <td>$\left|0\right\rangle$</td>
                                <td>$0.9899 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="97.99999999999999"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(0deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>


                            <tr>
                                <td>$\left|1\right\rangle$</td>
                                <td>$-0.1414 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="2.0000000000000027"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(180deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>
                        
                        </tbody>
                    </table>






    Success!




```qsharp

```

### Task 1.3. Sign flip: $|+\rangle$  to $|-\rangle$  and vice versa.

**Input**: A qubit in state $|\psi\rangle = \alpha |0\rangle + \beta |1\rangle$.

**Goal** :  Change the qubit state to $\alpha |0\rangle - \beta |1\rangle$ (flip the sign of $|1\rangle$ component of the superposition).



```qsharp
%kata T103_SignFlip_Test 

operation SignFlip (q : Qubit) : Unit is Adj+Ctl {
    // ...
    Z(q);
}
```

    The starting state:




                    <table style="table-layout: fixed; width: 100%">
                        <thead>
                            
                        <tr>
                            <th>Qubit IDs</th>
                            <td span="3">0</td>
                        </tr>
                    
                            <tr>
                                <th style="width: 20ch)">Basis state (little endian)</th>
                                <th style="width: 20ch">Amplitude</th>
                                <th style="width: calc(100% - 26ch - 20ch)">Meas. Pr.</th>
                                <th style="width: 6ch">Phase</th>
                            </tr>
                        </thead>

                        <tbody>
                            
                            <tr>
                                <td>$\left|0\right\rangle$</td>
                                <td>$0.6000 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="36"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(0deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>


                            <tr>
                                <td>$\left|1\right\rangle$</td>
                                <td>$0.8000 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="64.00000000000001"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(0deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>
                        
                        </tbody>
                    </table>



    The desired state:




                    <table style="table-layout: fixed; width: 100%">
                        <thead>
                            
                        <tr>
                            <th>Qubit IDs</th>
                            <td span="3">0</td>
                        </tr>
                    
                            <tr>
                                <th style="width: 20ch)">Basis state (little endian)</th>
                                <th style="width: 20ch">Amplitude</th>
                                <th style="width: calc(100% - 26ch - 20ch)">Meas. Pr.</th>
                                <th style="width: 6ch">Phase</th>
                            </tr>
                        </thead>

                        <tbody>
                            
                            <tr>
                                <td>$\left|0\right\rangle$</td>
                                <td>$0.6000 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="36"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(0deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>


                            <tr>
                                <td>$\left|1\right\rangle$</td>
                                <td>$-0.8000 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="64.00000000000001"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(180deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>
                        
                        </tbody>
                    </table>



    The actual state:




                    <table style="table-layout: fixed; width: 100%">
                        <thead>
                            
                        <tr>
                            <th>Qubit IDs</th>
                            <td span="3">0</td>
                        </tr>
                    
                            <tr>
                                <th style="width: 20ch)">Basis state (little endian)</th>
                                <th style="width: 20ch">Amplitude</th>
                                <th style="width: calc(100% - 26ch - 20ch)">Meas. Pr.</th>
                                <th style="width: 6ch">Phase</th>
                            </tr>
                        </thead>

                        <tbody>
                            
                            <tr>
                                <td>$\left|0\right\rangle$</td>
                                <td>$0.6000 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="36"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(0deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>


                            <tr>
                                <td>$\left|1\right\rangle$</td>
                                <td>$-0.8000 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="64.00000000000001"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(180deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>
                        
                        </tbody>
                    </table>






    Success!



### Task 1.4. Amplitude change: $|0\rangle$ to $\cos{α} |0\rangle + \sin{α} |1\rangle$.

**Inputs:**

1. Angle α, in radians, represented as Double.
2. A qubit in state $|\psi\rangle = \beta |0\rangle + \gamma |1\rangle$.

**Goal:**  Change the state of the qubit as follows:
- If the qubit is in state $|0\rangle$, change its state to $\cos{α} |0\rangle + \sin{α} |1\rangle$.
- If the qubit is in state $|1\rangle$, change its state to $-\sin{α} |0\rangle + \cos{α} |1\rangle$.
- If the qubit is in superposition, change its state according to the effect on basis vectors.

> This is the first operation in this kata that is not self-adjoint, i.e., applying it for a second time
> does not return the qubit to the original state.


```qsharp
%kata T104_AmplitudeChange_Test

operation AmplitudeChange (alpha : Double, q : Qubit) : Unit is Adj+Ctl {
    // ...
    Ry(2.0 * alpha, q);
}
```

    Applying amplitude change with alpha = 1.0471975511965976
    The starting state:




                    <table style="table-layout: fixed; width: 100%">
                        <thead>
                            
                        <tr>
                            <th>Qubit IDs</th>
                            <td span="3">0</td>
                        </tr>
                    
                            <tr>
                                <th style="width: 20ch)">Basis state (little endian)</th>
                                <th style="width: 20ch">Amplitude</th>
                                <th style="width: calc(100% - 26ch - 20ch)">Meas. Pr.</th>
                                <th style="width: 6ch">Phase</th>
                            </tr>
                        </thead>

                        <tbody>
                            
                            <tr>
                                <td>$\left|0\right\rangle$</td>
                                <td>$0.6000 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="36"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(0deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>


                            <tr>
                                <td>$\left|1\right\rangle$</td>
                                <td>$0.8000 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="64.00000000000001"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(0deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>
                        
                        </tbody>
                    </table>



    The desired state:




                    <table style="table-layout: fixed; width: 100%">
                        <thead>
                            
                        <tr>
                            <th>Qubit IDs</th>
                            <td span="3">0</td>
                        </tr>
                    
                            <tr>
                                <th style="width: 20ch)">Basis state (little endian)</th>
                                <th style="width: 20ch">Amplitude</th>
                                <th style="width: calc(100% - 26ch - 20ch)">Meas. Pr.</th>
                                <th style="width: 6ch">Phase</th>
                            </tr>
                        </thead>

                        <tbody>
                            
                            <tr>
                                <td>$\left|0\right\rangle$</td>
                                <td>$-0.3928 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="15.430780618346942"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(180deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>


                            <tr>
                                <td>$\left|1\right\rangle$</td>
                                <td>$0.9196 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="84.56921938165307"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(0deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>
                        
                        </tbody>
                    </table>



    The actual state:




                    <table style="table-layout: fixed; width: 100%">
                        <thead>
                            
                        <tr>
                            <th>Qubit IDs</th>
                            <td span="3">0</td>
                        </tr>
                    
                            <tr>
                                <th style="width: 20ch)">Basis state (little endian)</th>
                                <th style="width: 20ch">Amplitude</th>
                                <th style="width: calc(100% - 26ch - 20ch)">Meas. Pr.</th>
                                <th style="width: 6ch">Phase</th>
                            </tr>
                        </thead>

                        <tbody>
                            
                            <tr>
                                <td>$\left|0\right\rangle$</td>
                                <td>$-0.3928 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="15.430780618346942"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(180deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>


                            <tr>
                                <td>$\left|1\right\rangle$</td>
                                <td>$0.9196 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="84.56921938165307"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(0deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>
                        
                        </tbody>
                    </table>






    Success!



### Task 1.5. Phase flip

**Input:** A qubit in state $|\psi\rangle = \alpha |0\rangle + \beta |1\rangle$.

**Goal:** Change the qubit state to $\alpha |0\rangle + \color{red}i\beta |1\rangle$ (add a relative phase $i$ to $|1\rangle$ component of the superposition).



```qsharp
%kata T105_PhaseFlip_Test

operation PhaseFlip (q : Qubit) : Unit is Adj+Ctl {
    // ...
    S(q);
}
```

    The starting state:




                    <table style="table-layout: fixed; width: 100%">
                        <thead>
                            
                        <tr>
                            <th>Qubit IDs</th>
                            <td span="3">0</td>
                        </tr>
                    
                            <tr>
                                <th style="width: 20ch)">Basis state (little endian)</th>
                                <th style="width: 20ch">Amplitude</th>
                                <th style="width: calc(100% - 26ch - 20ch)">Meas. Pr.</th>
                                <th style="width: 6ch">Phase</th>
                            </tr>
                        </thead>

                        <tbody>
                            
                            <tr>
                                <td>$\left|0\right\rangle$</td>
                                <td>$0.6000 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="36"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(0deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>


                            <tr>
                                <td>$\left|1\right\rangle$</td>
                                <td>$0.8000 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="64.00000000000001"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(0deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>
                        
                        </tbody>
                    </table>



    The desired state:




                    <table style="table-layout: fixed; width: 100%">
                        <thead>
                            
                        <tr>
                            <th>Qubit IDs</th>
                            <td span="3">0</td>
                        </tr>
                    
                            <tr>
                                <th style="width: 20ch)">Basis state (little endian)</th>
                                <th style="width: 20ch">Amplitude</th>
                                <th style="width: calc(100% - 26ch - 20ch)">Meas. Pr.</th>
                                <th style="width: 6ch">Phase</th>
                            </tr>
                        </thead>

                        <tbody>
                            
                            <tr>
                                <td>$\left|0\right\rangle$</td>
                                <td>$0.6000 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="36"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(0deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>


                            <tr>
                                <td>$\left|1\right\rangle$</td>
                                <td>$0.0000 + 0.8000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="64.00000000000001"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(90deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>
                        
                        </tbody>
                    </table>



    The actual state:




                    <table style="table-layout: fixed; width: 100%">
                        <thead>
                            
                        <tr>
                            <th>Qubit IDs</th>
                            <td span="3">0</td>
                        </tr>
                    
                            <tr>
                                <th style="width: 20ch)">Basis state (little endian)</th>
                                <th style="width: 20ch">Amplitude</th>
                                <th style="width: calc(100% - 26ch - 20ch)">Meas. Pr.</th>
                                <th style="width: 6ch">Phase</th>
                            </tr>
                        </thead>

                        <tbody>
                            
                            <tr>
                                <td>$\left|0\right\rangle$</td>
                                <td>$0.0000 + 0.6000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="36"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(90deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>


                            <tr>
                                <td>$\left|1\right\rangle$</td>
                                <td>$-0.8000 + 0.0000 i$</td>
                                <td>
                                    <progress
                                        max="100"
                                        value="64.00000000000001"
                                        style="width: 100%;"
                                    >
                                </td>
                                
                                <td style="transform: rotate(180deg);
                   text-align: center;">
                                 ↑
                                </td>
                            
                            </tr>
                        
                        </tbody>
                    </table>






    Success!



### Task 1.6. Phase change

**Inputs:**

1. Angle α, in radians, represented as Double.
2. A qubit in state $|\psi\rangle = \beta |0\rangle + \gamma |1\rangle$.

**Goal:**  Change the state of the qubit as follows:
- If the qubit is in state $|0\rangle$, don't change its state.
- If the qubit is in state $|1\rangle$, change its state to $e^{i\alpha} |1\rangle$.
- If the qubit is in superposition, change its state according to the effect on basis vectors: $\beta |0\rangle + \color{red}{e^{i\alpha}} \gamma |1\rangle$.



```qsharp
%kata T106_PhaseChange_Test

operation PhaseChange (alpha : Double, q : Qubit) : Unit is Adj+Ctl {
    // ...
}
```

### Task 1.7. Global phase change
**Input:** A qubit in state $|\psi\rangle = \beta |0\rangle + \gamma |1\rangle$.

**Goal**: Change the state of the qubit to $- \beta |0\rangle - \gamma |1\rangle$.

> Note: this change on its own is not observable - there is no experiment you can do on a standalone qubit to figure out whether it acquired the global phase or not. 
> However, you can use a controlled version of this operation to observe the global phase it introduces. 
> This is used in later katas as part of more complicated tasks.

<details>
  <summary><b>Need a hint? Click here</b></summary>
  Can you apply one of the rotation gates? Take a look at the functions in the Microsoft.Quantum.Math package to use a common mathematical constant and remember to import the package using the open directive.    
</details>


```qsharp
%kata T107_GlobalPhaseChange_Test

operation GlobalPhaseChange (q : Qubit) : Unit is Adj+Ctl {
    // ...
}
```

### Task 1.8. Bell state change - 1

**Input:** Two entangled qubits in Bell state $|\Phi^{+}\rangle = \frac{1}{\sqrt{2}} \big(|00\rangle + |11\rangle\big)$.

**Goal:**  Change the two-qubit state to $|\Phi^{-}\rangle = \frac{1}{\sqrt{2}} \big(|00\rangle - |11\rangle\big)$.



```qsharp
%kata T108_BellStateChange1_Test

operation BellStateChange1 (qs : Qubit[]) : Unit is Adj+Ctl {
    // ...
}
```

### Task 1.9. Bell state change - 2

**Input:** Two entangled qubits in Bell state $|\Phi^{+}\rangle = \frac{1}{\sqrt{2}} \big(|00\rangle + |11\rangle\big)$.

**Goal:**  Change the two-qubit state to $|\Psi^{+}\rangle = \frac{1}{\sqrt{2}} \big(|01\rangle + |10\rangle\big)$.


```qsharp
%kata T109_BellStateChange2_Test

operation BellStateChange2 (qs : Qubit[]) : Unit is Adj+Ctl {
    // ...
}
```

### Task 1.10. Bell state change - 3

**Input:** Two entangled qubits in Bell state $|\Phi^{+}\rangle = \frac{1}{\sqrt{2}} \big(|00\rangle + |11\rangle\big)$.

**Goal:**  Change the two-qubit state, without adding a global phase, to $|\Psi^{-}\rangle = \frac{1}{\sqrt{2}} \big(|01\rangle - |10\rangle\big)$.

<details>
    <summary><b>Need a hint? Click here</b></summary>
A similar transformation could be done using a single <b>Y</b> gate on the first qubit. However the <b>Y</b> gate also adds a global phase (more on that can be found <a href="../tutorials/Qubit/Qubit.ipynb#Relative-and-Global-Phase">here</a>) which is not intended in this exercise.  

Look for a solution, possibly with multiple gates, which has a very similar transformation as the <b>Y</b> gate but without adding a global phase.    
</details>


```qsharp
%kata T110_BellStateChange3_Test

operation BellStateChange3 (qs : Qubit[]) : Unit is Adj+Ctl {
    // ...
}
```

## Part II. Multi-Qubit Gates

> In the following tasks the order of qubit states in task description matches the order of qubits in the array (i.e., $|10\rangle$ state corresponds to `qs[0]` in state $|1\rangle$ and `qs[1]` in state $|0\rangle$).
> 
> Note also that the states shown in test output use little-endian notation (similarly to `DumpMachine`), see this [tutorial section](../tutorials/MultiQubitSystems/MultiQubitSystems.ipynb#Endianness) for a refresher on endianness.

### Q# materials

* Using controlled and adjoint versions of gates is covered in the Q# documentation on [operation types](https://docs.microsoft.com/quantum/language/type-model#operation-and-function-types).

### Task 2.1. Two-qubit gate - 1

**Input:** Two unentangled qubits (stored in an array of length 2).
The first qubit will be in state $|\psi\rangle = \alpha |0\rangle + \beta |1\rangle$, the second - in state $|0\rangle$
(this can be written as two-qubit state  $\big(\alpha |0\rangle + \beta |1\rangle \big) \otimes |0\rangle = \alpha |00\rangle + \beta |10\rangle$.


**Goal:**  Change the two-qubit state to $\alpha |00\rangle + \beta |11\rangle$.

> Note that unless the starting state of the first qubit was $|0\rangle$ or $|1\rangle$,
> the resulting two-qubit state can not be represented as a tensor product
> of the states of individual qubits any longer; thus the qubits become entangled.


```qsharp
%kata T201_TwoQubitGate1_Test

operation TwoQubitGate1 (qs : Qubit[]) : Unit is Adj+Ctl {
    // ...
}
```

### Task 2.2. Two-qubit gate - 2

**Input:** Two unentangled qubits (stored in an array of length 2) in state $|+\rangle \otimes |+\rangle = \frac{1}{2} \big( |00\rangle + |01\rangle + |10\rangle \color{blue}+ |11\rangle \big)$.


**Goal:**  Change the two-qubit state to $\frac{1}{2} \big( |00\rangle + |01\rangle + |10\rangle \color{red}- |11\rangle \big)$.

> Note that while the starting state can be represented as a tensor product of single-qubit states,
> the resulting two-qubit state can not be represented in such a way.


```qsharp
%kata T202_TwoQubitGate2_Test

operation TwoQubitGate2 (qs : Qubit[]) : Unit is Adj+Ctl {
    // ...
}
```

### Task 2.3. Two-qubit gate - 3

**Input:** Two unentangled qubits (stored in an array of length 2) in an arbitrary two-qubit state $\alpha |00\rangle + \color{blue}\beta |01\rangle + \color{blue}\gamma |10\rangle + \delta |11\rangle$.


**Goal:**  Change the two-qubit state to $\alpha |00\rangle + \color{red}\gamma |01\rangle + \color{red}\beta |10\rangle + \delta |11\rangle$.

> This task can be solved using one intrinsic gate; as an exercise, try to express the solution using several (possibly controlled) Pauli gates.


```qsharp
%kata T203_TwoQubitGate3_Test

operation TwoQubitGate3 (qs : Qubit[]) : Unit is Adj+Ctl {
    // ...
}
```

### Task 2.4. Toffoli gate

**Input:** Three qubits (stored in an array of length 3) in an arbitrary three-qubit state 
$\alpha |000\rangle + \beta |001\rangle + \gamma |010\rangle + \delta |011\rangle + \epsilon |100\rangle + \zeta|101\rangle + \color{blue}\eta|110\rangle + \color{blue}\theta|111\rangle$.

**Goal:** Flip the state of the third qubit if the state of the first two is $|11\rangle$, i.e., change the three-qubit state to $\alpha |000\rangle + \beta |001\rangle + \gamma |010\rangle + \delta |011\rangle + \epsilon |100\rangle + \zeta|101\rangle + \color{red}\theta|110\rangle + \color{red}\eta|111\rangle$.


```qsharp
%kata T204_ToffoliGate_Test

operation ToffoliGate (qs : Qubit[]) : Unit is Adj+Ctl {
    // ...
}
```

### Task 2.5. Fredkin gate

**Input:** Three qubits (stored in an array of length 3) in an arbitrary three-qubit state 
$\alpha |000\rangle + \beta |001\rangle + \gamma |010\rangle + \delta |011\rangle + \epsilon |100\rangle + \color{blue}\zeta|101\rangle + \color{blue}\eta|110\rangle + \theta|111\rangle$.

**Goal:** Swap the states of second and third qubit if and only if the state of the first qubit is $|1\rangle$, i.e., change the three-qubit state to $\alpha |000\rangle + \beta |001\rangle + \gamma |010\rangle + \delta |011\rangle + \epsilon |100\rangle + \color{red}\eta|101\rangle + \color{red}\zeta|110\rangle + \theta|111\rangle$.


```qsharp
%kata T205_FredkinGate_Test

operation FredkinGate (qs : Qubit[]) : Unit is Adj+Ctl {
    // ...
}
```
