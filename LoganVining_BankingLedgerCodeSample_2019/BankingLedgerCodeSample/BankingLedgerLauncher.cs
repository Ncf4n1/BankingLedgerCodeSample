/*******************************************************
 * Name: Logan Vining
 * Date: May 5, 2019
 * File: BankingLedgerLauncher.cs
 * 
 * Description: Program that creates an instance of a Bank Ledger
 *              the user can directly interact with, beginning
 *              with the login menu.
 ********************************************************/

using System;

namespace BankingLedgerCodeSample
{
    class BankingLedgerLauncher
    {
        static void Main(string[] args)
        {
            BankLedger bankLedger = new BankLedger();

            bankLedger.DisplayLoginMenu();
        }
    }
}
