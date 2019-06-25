/*******************************************************
 * Name: Logan Vining
 * Date: May 5, 2019
 * File: UserBankAccount.cs
 * 
 * Description: Class representing a bank account for a user
 *              that provides functionality for securely
 *              hashing and storing the user's password with the MD5
 *              hash and checking if two hashes match for
 *              password verification. An account also provides
 *              the standard functionality of making a deposit
 *              and withdrawal, viewing the account balance and
 *              the corresponding transaction history.
 ********************************************************/

using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace BankingLedgerCodeSample
{
    class UserBankAccount
    {
        /*
         * Simple nested class used to track the current user's history of transactions
         */
        class TransactionHistoryNode
        {
            private double amountForTransaction;                // Holds amount for current transaction
            public double AmountForTransaction
            {
                get
                {
                    return this.amountForTransaction;
                }
            }

            private char typeOfTransaction;                     // Holds whether the transaction was a deposit or withdrawal
            public char TypeOfTransaction
            {
                get
                {
                    return this.typeOfTransaction;
                }
            }

            public TransactionHistoryNode(double desiredTransactionAmount, char transactionType)
            {
                this.amountForTransaction = desiredTransactionAmount;
                this.typeOfTransaction = transactionType;
            }
        }


        private string username;                                                                        //Holds the account username usable strictly within the account

        public string Username                                                                          //Publicly accessible getters and setters for the username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
            }
        }

        private string hashedUserPassword;                                                              //Holds the MD5 hashed password for the user account    
        private double accountBalance;                                                                  //Holds the available funds for this account

        //Dictionary that maps the deposit/withdrawal amount to a letter indicating if it was a deposit or withdrawal
        //Simple mechanism for printing the transaction history for this account
        private readonly LinkedList<TransactionHistoryNode> userTransactionHistory = new LinkedList<TransactionHistoryNode>();

        private readonly MD5 hasher = MD5.Create();                                                     //Hasher that allows secure passwords to be stored

        public UserBankAccount(string username, string plainTextPassword)
        {
            this.username = username;
            this.hashedUserPassword = GetMD5Hash(plainTextPassword);
            this.accountBalance = 0.00;
        }

        /*
         * Private helper method that calculates and returns the MD5 hash of the provided password for secure password storage
         */
        private string GetMD5Hash(string plaintTextPassword)
        {
            byte[] hashedData = hasher.ComputeHash(Encoding.UTF8.GetBytes(plaintTextPassword));

            StringBuilder hashBuilder = new StringBuilder();

            foreach (byte data in hashedData)
            {
                hashBuilder.Append(data.ToString());
            }

            return hashBuilder.ToString();
        }

        /*
         * Helper method that simply checks if two hashed passwords are equal
         * NOTE: This method uses a string comparer to check the two hashes with
         * case-insensitivity
         */
        public bool CheckUserEnteredPassword(string plainTextPassword)
        {
            string userEnteredHashedPassword = GetMD5Hash(plainTextPassword);
            StringComparer hashComparer = StringComparer.OrdinalIgnoreCase;

            if (hashComparer.Compare(userEnteredHashedPassword, this.hashedUserPassword) == 0)
            {
                return true;
            }

            return false;
        }

        /*
         * Core method that prompts the user to make a deposit into their account
         * and handles any incorrect input by the user
         */
        public void MakeDeposit()
        {
            Console.Write("How much would you like to deposit? ");
            string userDepositInput = Console.ReadLine();

            if (Double.TryParse(userDepositInput, out double depositAmount))
            {
                this.accountBalance += depositAmount;
                userTransactionHistory.AddLast(new TransactionHistoryNode(depositAmount, 'D'));
                Console.WriteLine("Your deposit was successfully made!");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("That is not a valid monetary value. Please enter an amount without any extra symbols.");
                Console.WriteLine();
            }
        }

        /*
         * Core method that prompts the user to withdraw funds from their account
         * and handle any incorrect input from the user
         */
        public void MakeWithdrawal()
        {
            Console.Write("How much would you like to withdraw? ");
            string userWithdrawInput = Console.ReadLine();

            if (Double.TryParse(userWithdrawInput, out double withdrawAmount))
            {
                if (withdrawAmount <= this.accountBalance)
                {
                    this.accountBalance -= withdrawAmount;
                    userTransactionHistory.AddLast(new TransactionHistoryNode(withdrawAmount, 'W'));
                    Console.WriteLine("Your withdrawal was successfully made!");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("You do not have enough funds to withdraw ${0:0.00}", withdrawAmount);
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("That is not a valid monetary value. Please enter an amount without any extra symbols.");
                Console.WriteLine();
            }
        }

        /*
         * Core method that displays the account balance to the user
         */
        public void ViewAvailableFunds()
        {
            Console.WriteLine("Your account currently holds ${0:0.00}", this.accountBalance);
            Console.WriteLine();
        }

        /*
         * Core method that shows the user their transaction history by
         * looping through the transaction history dictionary and clearly
         * showing each transaction line by line
         */
        public void ViewTransactionHistory()
        {
            Console.WriteLine("***************************");
            Console.WriteLine("Transaction History for {0}", this.username);
            Console.WriteLine("****************************");

            if (userTransactionHistory.Count == 0)
            {
                Console.WriteLine("No transactions yet.");
                Console.WriteLine();
                return;
            }

            foreach (TransactionHistoryNode currentTransaction in userTransactionHistory)
            {
                if (currentTransaction.TypeOfTransaction == 'D')
                {
                    Console.WriteLine("Deposited ${0:0.00}", currentTransaction.AmountForTransaction);
                }
                else if (currentTransaction.TypeOfTransaction == 'W')
                {
                    Console.WriteLine("Withdrew ${0:0.00}", currentTransaction.AmountForTransaction);
                }
            }

            Console.WriteLine();
        }
    }
}
