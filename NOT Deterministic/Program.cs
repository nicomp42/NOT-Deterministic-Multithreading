/*
 * Bill Nicholson
 * nicholdw@ucmail.uc.edu
 * Threading example that is NOT deterministic
 * Run the app several times and observe that the balance changes.
 * The code is not atomic. It will not execute deterministically.
 */

using System;
using System.Threading;

public class BankAccount {
    private double balance;
    public BankAccount(double balance){this.balance = balance;}
    public void Deposit(double amount) {
        double tmp = balance;
        Thread.Sleep((new Random()).Next(1000));    // Add in some randomness for the real world
        balance = tmp + amount;
    }
    public void Withdraw(double amount) {
        double tmp = balance;
        Thread.Sleep((new Random()).Next(1000));    // Add in some randomness for the real world
        balance = tmp - amount;
    }
    public double getBalance() {return balance;}
}

public class Deposit
{
    BankAccount bankAccount;
    double amount;
    public Deposit(BankAccount bankAccount, double amount) {this.bankAccount = bankAccount; this.amount = amount;}
    // This method that will be called when the thread is started, then the thread will end when the methods end
    public void MakeDeposit()
    {
        bankAccount.Deposit(amount);
    }
};

public class Withdraw
{
    BankAccount bankAccount;
    double amount;
    public Withdraw(BankAccount bankAccount, double amount) {this.bankAccount = bankAccount; this.amount = amount;}
    // This method that will be called when the thread is started, then the thread will end when the methos ends
    public void MakeWithdrawal()
    {
        bankAccount.Withdraw(amount);
    }
};

public class Simple
{
    // Entry Point
    public static int Main()
    {
        Console.WriteLine("NON Deterministic thread example");

        // Some poor schlep has $500 in their account. Not for long...
        BankAccount bankAccount = new BankAccount(500.0);

        // Create the deposit and withdraw threads. These represent two physical ATMs or banks, etc.
        Deposit deposit = new Deposit(bankAccount, 10);
        Withdraw withdraw = new Withdraw(bankAccount, 100);

        // Create the thread object, passing in the desired method via a ThreadStart delegate. This does not start the thread.
        Thread depositThread = new Thread(new ThreadStart(deposit.MakeDeposit));
        Thread withdrawThread = new Thread(new ThreadStart(withdraw.MakeWithdrawal));

        // Start the thread
        depositThread.Start();
        withdrawThread.Start();

        // Wait until both Threads finish. This prevents the main method from finishing first, which would automagically kill the other threads. 
        depositThread.Join();
        withdrawThread.Join();

        Console.WriteLine();
        Console.WriteLine("main() has finished");
        Console.WriteLine("The account balance is now " + bankAccount.getBalance());

        return 0;
    }
}
