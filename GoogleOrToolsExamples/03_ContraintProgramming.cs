using Google.OrTools.Sat;

namespace testortool;

public static class ContraintProgramming 
{
    public static void SimpleConstraint()
    {
        // Creates the model.
        CpModel model = new CpModel();

        // variables
        int num_vals = 3;
        IntVar x = model.NewIntVar(0, num_vals - 1, "x");
        IntVar y = model.NewIntVar(0, num_vals - 1, "y");
        IntVar z = model.NewIntVar(0, num_vals - 1, "z");

        // constraints
        model.Add(x != y);

        // run solve
        CpSolver solver = new CpSolver();
        CpSolverStatus status = solver.Solve(model);

        if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
        {
            Console.WriteLine("x = " + solver.Value(x));
            Console.WriteLine("y = " + solver.Value(y));
            Console.WriteLine("z = " + solver.Value(z));
        }
        else
        {
            Console.WriteLine("No solution found.");
        }
    }

    #region SolutionPrinter
    public class VarArraySolutionPrinter : CpSolverSolutionCallback
    {
        public int SolutionsFound { get; private set; }
        public IntVar[] Variables { get; }
        public VarArraySolutionPrinter(params IntVar[] variables)
        {
            this.Variables = variables;
        }

        public override void OnSolutionCallback()
        {
            {
                Console.WriteLine($"Solution #{SolutionsFound}: time = {WallTime():F2} s");
                foreach (IntVar v in Variables)
                {
                    Console.WriteLine($"  {v} = {Value(v)}");
                }
                SolutionsFound++;
            }
        }
    }
    #endregion

    
    public static void SimpleConstraintAllSolutions()
    {
        // Creates the model.
        CpModel model = new CpModel();

        // variables
        int num_vals = 3;
        IntVar x = model.NewIntVar(0, num_vals - 1, "x");
        IntVar y = model.NewIntVar(0, num_vals - 1, "y");
        IntVar z = model.NewIntVar(0, num_vals - 1, "z");

        // constraints
        model.Add(x != y);

        // run solve
        CpSolver solver = new CpSolver();
        VarArraySolutionPrinter cb = new VarArraySolutionPrinter( x, y, z );
        solver.StringParameters = "enumerate_all_solutions:true";
        CpSolverStatus status = solver.Solve(model, cb);
        solver.Solve(model, cb);

        Console.WriteLine($"Number of solutions found: {cb.SolutionsFound}");
    }


    public static void MaximizeContraint()
    {
        // Creates the model.
        CpModel model = new CpModel();

        // variables
        int varUpperBound = new int[] { 50, 45, 37 }.Max();
        IntVar x = model.NewIntVar(0, varUpperBound, "x");
        IntVar y = model.NewIntVar(0, varUpperBound, "y");
        IntVar z = model.NewIntVar(0, varUpperBound, "z");

        // constraints
        model.Add(2 * x + 7 * y + 3 * z <= 50);
        model.Add(3 * x - 5 * y + 7 * z <= 45);
        model.Add(5 * x + 2 * y - 6 * z <= 37);

        // extra optimization
        model.Maximize(2 * x + 2 * y + 3 * z);

        // solver
        CpSolver solver = new CpSolver();
        CpSolverStatus status = solver.Solve(model);

        if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
        {
            Console.WriteLine($"Maximum of objective function: {solver.ObjectiveValue}");
            Console.WriteLine("x = " + solver.Value(x));
            Console.WriteLine("y = " + solver.Value(y));
            Console.WriteLine("z = " + solver.Value(z));
        }
        else
        {
            Console.WriteLine("No solution found.");
        }

        Console.WriteLine("Statistics");
        Console.WriteLine($"  conflicts: {solver.NumConflicts()}");
        Console.WriteLine($"  branches : {solver.NumBranches()}");
        Console.WriteLine($"  wall time: {solver.WallTime()}s");
    }
}