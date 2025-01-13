using static Task29NoGUI.Malgrange;

namespace Task29NoGUI
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("===Малгранж===");
            Malgrange.Solve("/Users/michael/Documents/University (original)/2 course/casd/casd-labs/Task29NoGUI/Task29NoGUI/MalgrangeOrgraph.txt");
            Console.WriteLine("===Проталкивание предпотока===");
            PushRelabelMaxFlow.Solve("/Users/michael/Documents/University (original)/2 course/casd/casd-labs/Task29NoGUI/Task29NoGUI/PushRelabel.txt");
            Console.WriteLine("===Брон-Кербош===");
            BronKerbosch.Solve("/Users/michael/Documents/University (original)/2 course/casd/casd-labs/Task29NoGUI/Task29NoGUI/BronKerbosch.txt");
        }
    }
}

