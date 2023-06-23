using Google.OrTools.LinearSolver;

namespace testortool;

/*
 * Combine linear programming with variables that can be integers (or booleans 0-1)
 * MPSolver
 * CP-SAT solver => contraint programming solver that uses SAT [better to use when most of values are booleans]
 * Original CP solver => contraint programming solver
 */
public static class MixedIntegerProgramming
{

    public static void Geometry()
    {
        // Create the linear solver with the SCIP backend.
        Solver solver = Solver.CreateSolver("SCIP");
        if (solver is null) return;

        // variables
        Variable x = solver.MakeIntVar(0.0, double.PositiveInfinity, "x");
        Variable y = solver.MakeIntVar(0.0, double.PositiveInfinity, "y");
        Console.WriteLine("Number of variables = " + solver.NumVariables());

        // constraints
        solver.Add(x + 7 * y <= 17.5);
        solver.Add(x <= 3.5);
        Console.WriteLine("Number of constraints = " + solver.NumConstraints());

        // objective
        solver.Maximize(x + 10 * y);

        // run solve
        Solver.ResultStatus resultStatus = solver.Solve();
        if (resultStatus != Solver.ResultStatus.OPTIMAL)
        {
            Console.WriteLine("The problem does not have an optimal solution!");
            return;
        }
        Console.WriteLine("Solution:");
        Console.WriteLine("Objective value = " + solver.Objective().Value());
        Console.WriteLine("x = " + x.SolutionValue());
        Console.WriteLine("y = " + y.SolutionValue());
        Console.WriteLine("Problem solved in " + solver.WallTime() + " milliseconds");
        Console.WriteLine("Problem solved in " + solver.Iterations() + " iterations");
        Console.WriteLine("Problem solved in " + solver.Nodes() + " branch-and-bound nodes");        
    }
}