using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eos_genesis_stats
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Account> Accounts = ToList("snapshot.csv");
            Console.WriteLine($"The total number of genesis eos addresses: {Accounts.Count()}");
            Console.WriteLine($"The total number of eos coins that made it into the snapshot are: {GetTotal(Accounts)}");
            Console.WriteLine($"The average eos account has: {GetAverage(Accounts)} coins.");
            Console.WriteLine($"The median balance is: {GetMedian(Accounts)}.");
            Console.WriteLine($"The lowest balance is: {GetLowestBalance(Accounts)}.");
            Console.WriteLine($"The top 10 addresses own {GetPercentage(10, Accounts)}% of all the eos coins.");
            Console.WriteLine($"The top 100 addresses own {GetPercentage(100, Accounts)}% of all the eos coins.");
            Console.WriteLine($"The top 1000 addresses own {GetPercentage(1000, Accounts)}% of all the eos coins.");
            Console.WriteLine($"The top 10000 addresses own {GetPercentage(10000, Accounts)}% of all the eos coins.");
            Console.WriteLine($"Block.one holds 10% of the supply: 100000000.0100.");
            Console.ReadLine();
        }

        private static List<Account> ToList(string FileName)
        {
            string _Path = Path.Combine(Environment.CurrentDirectory, @"Data\", FileName);
            var Lines = File.ReadLines(_Path);
            Account _Account = new Account();
            List<Account> Accounts = new List<Account>();
            foreach (var Line in Lines)
            {
                string LineWithNoQuotations = Line.Replace("\"", String.Empty);
                string[] Data = LineWithNoQuotations.Split(',');
                _Account.Balance = Decimal.Parse(Data[3]);
                _Account.Address = Data[2];
                Accounts.Add(_Account);
                _Account = new Account();
            }

            return Accounts;
        }

        private static decimal GetAverage(List<Account> Accounts)
        {
            return Math.Round(GetTotal(Accounts) / Accounts.Count(), 4);
        }

        private static decimal GetLowestBalance(List<Account> Accounts)
        {
            decimal Min = decimal.MaxValue;
            foreach (var Account in Accounts)
            {
                if (Account.Balance < Min)
                    Min = Account.Balance;
            }

            return Min;
        }

        private static decimal GetMedian(List<Account> Accounts)
        {
            Accounts = Accounts.OrderBy(A => A.Balance).ToList();

            // Even number of accounts, so no more calculations are needed
            return Accounts.ElementAt((Accounts.Count()/2) - 1).Balance;
        }

        private static decimal GetTotal(List<Account> Accounts)
        {
            decimal Total = 0;
            foreach (var Account in Accounts)
                Total += Account.Balance;

            return Total;
        }

        private static decimal GetTotal(List<Account> Accounts, int Index)
        {
            decimal Total = 0;
            for (int i = 0; i < Index; i++)
                Total += Accounts.ElementAt(i).Balance;

            return Total;
        }

        private static decimal GetPercentage(int Index, List<Account> Accounts)
        {
            Accounts = Accounts.OrderByDescending(A => A.Balance).ToList();
            decimal GrandTotal = GetTotal(Accounts);
            decimal TotalToIndex = GetTotal(Accounts, Index);

            return (TotalToIndex / GrandTotal) * 100;
        }
    }
}
