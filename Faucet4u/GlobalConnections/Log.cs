using API.DatabaseModels;
using Dapper;
using API.GlobalConnections.Variable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Faucet4u.GlobalConnections
{
    public class Log
    {
        public async static void Info(Logs ModelValues)
        {
            try
            {
                ModelValues.Type = "Info";
                await ModelValues.SaveAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                File.AppendAllText(OtherVariable.SecondaryLogsPath, ex.ToString() + Environment.NewLine);
            }

        }

        public async static void Hack(Logs ModelValues)
        {
            try
            {
                ModelValues.Type = "Hack";
                await ModelValues.SaveAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                File.AppendAllText(OtherVariable.SecondaryLogsPath, ex.ToString() + Environment.NewLine);
            }

        }

        public async static void Cheat(Logs ModelValues)
        {
            try
            {
                ModelValues.Type = "Cheat";
                await ModelValues.SaveAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                File.AppendAllText(OtherVariable.SecondaryLogsPath, ex.ToString() + Environment.NewLine);
            }

        }

        public async static void Error(Logs ModelValues)
        {
            try
            {
                ModelValues.Type = "Error";
                await ModelValues.SaveAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                File.AppendAllText(OtherVariable.SecondaryLogsPath, ex.ToString() + Environment.NewLine);
            }

        }
    }
}
