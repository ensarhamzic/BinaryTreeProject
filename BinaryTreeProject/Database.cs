using BinaryTreeProject.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BinaryTreeProject
{
    public class Database
    {
        public string connectionString;
        public MySqlConnection connection;
        private MySqlCommand command;
        private MySqlDataReader reader;
        private MySqlDataAdapter adapter;

        public Database()
        {
            connectionString = @"server=localhost;userid=admin;password=root;database=binarytree"; // Promeniti parametre
            connection = new MySqlConnection(connectionString);
            command = new MySqlCommand();
        }

        public bool SaveTree(string treeName, List<Node> treeNodes)
        {
            try
            {
                int insertedTreeId;
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"select id from tree where name='{treeName}'";
                reader = command.ExecuteReader();
                if (reader.Read()) // if name already exists, delete it so tree can be overwritten
                {
                    int foundTreeId = reader.GetInt32(0);
                    reader.Close();
                    // this also deletes all nodes in the nodes table (on delete cascade)
                    command.CommandText = $"delete from tree where id = {foundTreeId};";
                    command.ExecuteNonQuery();
                }
                if (reader.IsClosed == false)
                    reader.Close();

                command.CommandText = $"insert into tree (name) values ('{treeName}');";
                command.ExecuteNonQuery();
                insertedTreeId = (int)command.LastInsertedId;

                string valuesString = "";

                foreach (var node in treeNodes)
                {
                    char? side;
                    // determine on which parents side is current node

                    if (node.ParentNode == null)
                        side = null;
                    else if (node.ParentNode.LeftNode == node)
                        side = 'L';
                    else
                        side = 'R';

                    string parentId;
                    
                    if (node.ParentNode == null)
                        parentId = "null";
                    else
                        parentId = node.ParentNode.ID.ToString();
                    // Building query string
                    valuesString += $"({node.ID}, {insertedTreeId}, {node.Value}, {parentId},";
                    if(side == null)
                        valuesString += $"null)";
                    else
                        valuesString += $"'{side}')";


                    if (node != treeNodes.Last())
                        valuesString += ",";
                    else
                        valuesString += ";";
                }

                command.CommandText = $"insert into node values {valuesString}";
                command.ExecuteNonQuery();

                return true;
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public DataTable LoadTree(string treeName)
        {
            DataTable dt = new DataTable();
            try
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"select id from tree where name='{treeName}';";
                reader = command.ExecuteReader();
                if (reader.Read()) // if found tree, select all nodes
                {
                    int foundTreeId = reader.GetInt32(0);
                    reader.Close();
                    // gets all nodes
                    command.CommandText = $"select * from node where tree_id = {foundTreeId};";
                    adapter = new MySqlDataAdapter(command);
                    adapter.SelectCommand = command;
                    adapter.SelectCommand.Connection = connection;
                    adapter.Fill(dt);
                    return dt;
                }
                if (reader.IsClosed == false)
                    reader.Close();
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return null;
        }
    }
}
