#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Data.OleDb;
using System.Windows.Forms;
#endregion

namespace GradedUnitGame
{
    class DatabaseInt
    {
        #region attributes

        //initialises the datarow collection
        DataRowCollection dataRowC;

        //tells the game how many rows to display
        int rowDraw = 0; 

        //initialises the dataset 
        DataSet dataSet = new DataSet();

        //initialises a new connection  
        OleDbConnection connection = null;

        //tells the game where to look for the database
        string conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:/Users/Siobhan/Documents/GitHub/Graded-Unit/GradedUnitGame/HighScoresDB.accdb";

        //initialises the default transaction value as null
        OleDbTransaction transaction = null;

        //the integer for playerId
        int playerID;
        #endregion

        //checks how many rows are in the database 
        public int CheckRows()
        {
            rowDraw = 1;
            rowDraw = dataSet.Tables["HighScores"].Rows.Count;
            //returns how many rows to draw
            return rowDraw;
        }

        //constructor
        public void DataBaseInt()
        {

        }

        //reads the high scores from the database
        public void ReadDatabase(String gameMode)
        {
            //clears the dataset
            dataSet.Clear();
            string modeSelect;
            //displays the highscores for the chosen mode
            if (gameMode == "CoOp") 
            {
                modeSelect = "SELECT Player.Name, HighScores.Score, HighScores.ModeType FROM (HighScores INNER JOIN Player ON HighScores.[user_id] = Player.ID AND HighScores.[user_id] = Player.ID) WHERE (HighScores.ModeType = 'CoOp') ORDER BY HighScores.Score DESC, Player.Name;";
            }
            else if (gameMode == "Arcade")
                { 
                modeSelect = "SELECT Player.Name, HighScores.Score, HighScores.ModeType FROM (HighScores INNER JOIN Player ON HighScores.[user_id] = Player.ID AND HighScores.[user_id] = Player.ID) WHERE (HighScores.ModeType = 'Comp') ORDER BY HighScores.Score DESC, Player.Name;";
                }
            else
            {
                modeSelect = "SELECT Player.Name, HighScores.Score, HighScores.ModeType FROM (HighScores INNER JOIN Player ON HighScores.[user_id] = Player.ID AND HighScores.[user_id] = Player.ID) WHERE (HighScores.ModeType = 'Comp') ORDER BY HighScores.Score DESC, Player.Name;";
            }
            try
            {
                connection = new OleDbConnection(conString);
            }
            catch (Exception x)
            {
                MsgboxScreen message = new MsgboxScreen("Error: Failed to create a database connection! \n{0}" + x.Message, true);
                return;
            }
            try
            {
                //sets command as the query we want
                OleDbCommand accessCmd = new OleDbCommand(modeSelect, connection);
                //sets the data adapter to the query and location we want 
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(accessCmd);

                connection.Open();
                dataAdapter.Fill(dataSet, "HighScores");
            }
            catch (Exception ex)
            {
                MsgboxScreen message = new MsgboxScreen("Error: Failed to retrieve the requested data! \n{0}" + ex.Message, true); 
            }
            finally 
            {
                connection.Close(); 
            }
            try
            {
                //passes the values to the data collection
                dataRowC = dataSet.Tables["HighScores"].Rows;
                rowDraw = 1;
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
                return;
            }
          }

        //writes to the database
        public void WriteDatabase(String gameMode, int playerScore)
        {
            try
            {
                connection = new OleDbConnection(conString);
                //opens connection
                connection.Open(); 
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
                return;
            }

            OleDbCommand Cmd = new OleDbCommand();
            transaction = connection.BeginTransaction();

            Cmd.Connection = connection;
            Cmd.Transaction = transaction;
            try
            {
                Cmd.CommandText = "INSERT INTO HighScores(Score,ModeType,user_id) VALUES(@Score,@Mode,@id);";
                //Values to be added
                Cmd.Parameters.AddWithValue("@Score", playerScore);
                Cmd.Parameters.AddWithValue("@Mode", gameMode);
                Cmd.Parameters.AddWithValue("@id", playerID);
                //executes the command
                Cmd.ExecuteNonQuery(); 
                //commits changes
                transaction.Commit(); 
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
            }

            connection.Close();

        }

        //adds the value name to the player database 
        public void AddNameDB(string playerName)
        {
            try
            {
                connection = new OleDbConnection(conString);
                connection.Open();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
                return;
            }
            try
            {
                OleDbCommand Cmd = new OleDbCommand();
                Cmd.Connection = connection;
                //starts the transaction
                transaction = connection.BeginTransaction();
                Cmd.Transaction = transaction;
                //the SQL query
                Cmd.CommandText = "INSERT INTO Player(Name) VALUES(@Name);";
                Cmd.Parameters.AddWithValue("@Name", playerName);
                //starts query
                Cmd.ExecuteNonQuery();
                Cmd.CommandText = "SELECT @@IDENTITY;";
                playerID = (int)Cmd.ExecuteScalar();
                //commits
                transaction.Commit();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
            }

            connection.Close();
        }

        //draws the rows on the database 
        public void Draw(SpriteFont gameFont, SpriteBatch sBatch, int count)
        {
            foreach (DataRow dataR in dataRowC)
            {
                //Draws the player name
                sBatch.DrawString(gameFont, dataR[0].ToString(), new Vector2(5, 100 + count), Color.White); 
                //draws the player score
                sBatch.DrawString(gameFont, dataR[1].ToString(), new Vector2(250, 100 + count), Color.White); 
                //draws the gamemode
                sBatch.DrawString(gameFont, dataR[2].ToString(), new Vector2(350, 100 + count), Color.White);
                count += 50;
               
            }
        }
    }
}
