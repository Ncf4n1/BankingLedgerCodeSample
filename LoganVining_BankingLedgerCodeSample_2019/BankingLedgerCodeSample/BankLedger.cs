/*******************************************************
 * Name: Logan Vining
 * Date: May 5, 2019
 * File: BankLedger.cs
 * 
 * Description: Class representing a standard banking ledger
 *              that executes the basic functionality of a
 *              ledger including:
 *              - Creating a user account
 *              - Logging into the account
 *              - Depositing and Withdrawing funds
 *              - Viewing available funds
 *              - Viewing transaction history by account
 *              - Logging out of the user account
 ********************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace BankingLedgerCodeSample
{
    class BankLedger
    {
        private readonly List<UserBankAccount> bankAccountList = new List<UserBankAccount>();

        /*
         * Method that handles the creation of a new user account.
         * Allows the user to enter a username and password.
         * NOTE: The password input is shown with the typical asterisk
         * format while the user types their password
         */
        public void CreateUserAccount()
        {
            Console.WriteLine("************************");
            Console.WriteLine("User Account Creation");
            Console.WriteLine("************************");

            Console.Write("Please enter a username: ");
            string username = Console.ReadLine();

            // Once the user enters a username, ensure an account with that name doesn't already exist
            foreach (UserBankAccount bankAccount in bankAccountList)
            {
                if (username == bankAccount.Username)
                {
                    Console.WriteLine("An account with that username already exists.");
                    Console.WriteLine();
                    return;
                }
            }

            while (true)
            {
                // If the user has entered a previously unknown username, allow them to
                // create and verify their account password
                Console.Write("Please enter a password: ");
                string plainTextFirstPassword = EnterUserPassword();
                Console.WriteLine();

                Console.Write("Please enter your password again: ");
                string plainTextVerifyingPassword = EnterUserPassword();
                Console.WriteLine();

                //Ensure the passwords match and reprompt the user for verification again if they don't match
                if (plainTextFirstPassword.Equals(plainTextVerifyingPassword))
                {
                    UserBankAccount newUserAccount = new UserBankAccount(username, plainTextVerifyingPassword);
                    bankAccountList.Add(newUserAccount);
                    Console.WriteLine();
                    Console.WriteLine("You have successfully created a new account!");
                    Console.WriteLine();
                    return;
                }
                else
                {
                    Console.WriteLine("The passwords you've entered do not match. Please enter and verify your password again.");
                    Console.WriteLine();
                }
            }
        }

        /*
         * Method that handles a user logging into their account and handles situations
         * of no account existing with the given username, the user attempting to log in
         * too many times (3 attempts), and handling incorrect password entries
         * 
         */ 
        public void LoginToAccount()
        {
            int loginAttempts = 0;
            bool foundMatchingUsername = false;
            bool loginSuccessful = false;

            Console.WriteLine("************************");
            Console.WriteLine("User Account Login");
            Console.WriteLine("************************");

            // We must allow the user to log in while they are under
            // the login limit and haven't already logged in
            while ( (loginAttempts < 3) && !loginSuccessful )
            {
                loginAttempts++;

                Console.Write("Please enter your username: ");
                string enteredUsername = Console.ReadLine();

                foreach (UserBankAccount bankAccount in bankAccountList)
                {
                    if (enteredUsername == bankAccount.Username)
                    {
                        foundMatchingUsername = true;
                        break;
                    }
                }

                // Once an account has been found for the user, prompt them to enter their password
                if (foundMatchingUsername)
                {
                    Console.Write("Please enter your password: ");
                    string plainTextPassword = EnterUserPassword();

                    foreach (UserBankAccount bankAccount in bankAccountList)
                    {
                        // Once the user has successfully logged in, show the user options available for a banking user
                        if (bankAccount.CheckUserEnteredPassword(plainTextPassword))
                        {
                            loginSuccessful = true;
                            Console.WriteLine();
                            Console.WriteLine();
                            DisplayUserMenu(bankAccount);
                        }
                    }

                    if (!loginSuccessful)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Incorrect password. Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("There seems to be no account with that username.");
                    Console.WriteLine("Please create an account before attempting to login.");
                    Console.WriteLine();
                    break;
                }



                if (loginAttempts == 3 && !loginSuccessful)
                {
                    Console.WriteLine("Too many login attempts!");
                }
            }
        }

        /* 
         * Helper method that allows the user to enter a password
         * while creating an account and logging into an account
         */
        private string EnterUserPassword()
        {
            StringBuilder plainTextPasswordBuilder = new StringBuilder();

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                // Finish capturing once the Enter key is pressed
                if (key.Key == ConsoleKey.Enter)
                    break;

                // Check if the user is hitting the backspace key to delete the correct characters
                if (key.Key == ConsoleKey.Backspace && plainTextPasswordBuilder.Length > 0)
                {
                    Console.Write("\b \b");
                    plainTextPasswordBuilder.Remove(plainTextPasswordBuilder.Length - 1, 1);
                }
                else
                {
                    Console.Write("*");
                    plainTextPasswordBuilder.Append(key.KeyChar);
                }
            }

            return plainTextPasswordBuilder.ToString();
        }

        /*
         * Method that operates as the menu a user sees once they successfully log in
         * The menu includes the ability to make deposits and withdrawals, view their account
         * balance and the corresponding transaction history, and log out once they are finished
         */ 
        public void DisplayUserMenu(UserBankAccount bankAccount)
        {
            // Continue to show the menu to the user until they want to log out
            while (true)
            {
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("1) Make a Deposit");
                Console.WriteLine("2) Make a Withdrawal");
                Console.WriteLine("3) View available funds");
                Console.WriteLine("4) View transaction history");
                Console.WriteLine("5) Log out");
                Console.Write("Please make a selection: ");

                string loggedInUserInput = Console.ReadLine();

                int loggedInUserSelection = ProcessUserMenuSelection(loggedInUserInput, 1, 5);

                // Once user makes a correct selection, process the option accordingly
                switch (loggedInUserSelection)
                {
                    case 1:
                        bankAccount.MakeDeposit();
                        break;

                    case 2:
                        bankAccount.MakeWithdrawal();
                        break;

                    case 3:
                        bankAccount.ViewAvailableFunds();
                        break;

                    case 4:
                        bankAccount.ViewTransactionHistory();
                        break;

                    case 5:
                        return;

                }
            }
        }

        /*
         * Method that serves as the line of interaction with the user by
         * displaying the available options for the ledger, processing the
         * user's input, and directing them accordingly
         */
        public void DisplayLoginMenu()
        {
            // Continue to show the menu to the user until they enter valid input
            // or want to exit the ledger program
            while (true)
            {
                Console.WriteLine("***********************************************");
                Console.WriteLine("Welcome to the World's Greatest Banking Ledger!");
                Console.WriteLine("***********************************************");

                Console.WriteLine("1) Create a new account");
                Console.WriteLine("2) Log into an existing account");
                Console.WriteLine("3) Exit Bank Ledger");
                Console.Write("Please make a selection: ");

                string userInput = Console.ReadLine();

                int userSelection = ProcessUserMenuSelection(userInput, 1, 3);

                // Once user makes a correct selection, process the option accordingly
                switch (userSelection)
                {
                    case 1:
                        CreateUserAccount();
                        break;

                    case 2:
                        LoginToAccount();
                        break;

                    case 3:
                        return;
                }
            }
        }

        /* 
         * Private helper function that processes and handles any
         * errors in the user's inputs for the login menu and user account menu
         */
        private int ProcessUserMenuSelection(string userMenuInput, int minimumValidOption, int maximumValidOption)
        {
            // Check the user's input for invalid characters and invalid options and handle accordingly
            if (Int32.TryParse(userMenuInput, out int userSelection))
            {
                if (userSelection < minimumValidOption || userSelection > maximumValidOption)
                {
                    Console.WriteLine("That is not an available option. Please make a valid selection.");
                }
                else
                {
                    userSelection = Int32.Parse(userMenuInput);
                }
            }
            else
            {
                Console.WriteLine("That input is invalid. Please make a valid selection.");
            }

            Console.WriteLine();

            return userSelection;
        }
    }
}
