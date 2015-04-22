#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Data.OleDb;
using System.Data.Common;
using System.Windows.Forms;
using System.IO; 
#endregion

namespace GradedUnitGame
{
    class DatabaseInt
    {
      
        private void ReadDatabase()
        {
            //tells the game where to look for the database
            string conString = "Provider=Microsoft.JET.OLEDB.4.0;data source=E:/Graded Unit/GradedUnitGame/HighScoresDB.accdb";
            OleDbConnection con = new OleDbConnection(conString);
           //tells the game what sql command to query the database with
            String sql = "SELECT * FROM Score";
            OleDbCommand cmd = new OleDbCommand(sql, con);
              
            //opens connections to the database           
            con.Open();


            OleDbDataReader reader;
            reader = cmd.ExecuteReader();

            while (reader.Read()) 
            {
                Console.Write(reader.GetString(0).ToString() + " ," );
                Console.Write(reader.GetString(1).ToString() + " ," );
                Console.WriteLine("");
             }
        
            //closes connections to the database
            reader.Close();
            con.Close();
            }

        //todo write to the database
        private void WriteDatabase()
        {
           
        }

    }
}
