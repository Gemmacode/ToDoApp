using GemmaTodoData.Entity;
using GemmaTodoData.My_DbContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GemmaTodoApp
{
    public partial class Form1 : Form
    {
        TodoItem todoitem = new TodoItem();
        List<int> selectedTaskIds = new List<int>();

        public Form1()
        {
            InitializeComponent();
        }



        private void ClearButton_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void Clear()
        {
            TaskTestBox.Text = string.Empty;
            AddButton.Text = "Add";
            DeleteButton.Enabled = false;
            todoitem.Id = 0;
            this.ActiveControl = TaskTestBox;

            DeleteButton.Text = "Delete";
        }


        private void AddButton_Click(object sender, EventArgs e)
        {
            try
            {

                string task = TaskTestBox.Text.Trim();

                if (string.IsNullOrEmpty(task))
                {
                    MessageBox.Show("Please enter a task.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                if (System.Text.RegularExpressions.Regex.IsMatch(task, "[!@#$%^&*(),.?\":{}|<>]"))
                {
                    MessageBox.Show("Task cannot contain special characters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                todoitem.CreatedAt = DateTime.Now;
                todoitem.UpdatedAt = DateTime.Now;
                todoitem.Task = task;

                using (GemmaDbContext item = new GemmaDbContext())
                {
                    if (todoitem.Id == 0)
                        item.TodoItems.Add(todoitem);
                    else
                        item.Entry(todoitem).State = EntityState.Modified;
                    item.SaveChanges(); 
                }
                Clear();
                LoadData();
                MessageBox.Show("Added successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            this.ActiveControl = TaskTestBox;
            LoadData();
            timer1.Start();

        }

        void LoadData()
        {
            using (GemmaDbContext item = new GemmaDbContext())
            {
                dataGridView1.DataSource = item.TodoItems.ToList<TodoItem>();
            }
            Clear();

        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index != -1)
            {
                todoitem.Id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);
                using (GemmaDbContext item = new GemmaDbContext())
                {
                    todoitem = item.TodoItems.Where(x => x.Id == todoitem.Id).FirstOrDefault();
                    TaskTestBox.Text = todoitem.Task;

                }
                AddButton.Text = "Update";
                DeleteButton.Enabled = true;
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
           

            if (MessageBox.Show("Are you sure you want to delete all selected tasks?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (GemmaDbContext context = new GemmaDbContext())
                {
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        int taskId = Convert.ToInt32(row.Cells["Id"].Value);
                        var taskToDelete = context.TodoItems.Find(taskId);
                        if (taskToDelete != null)
                        {
                            context.TodoItems.Remove(taskToDelete);
                        }
                    }

                    context.SaveChanges();
                    LoadData();


                }

                MessageBox.Show("Selected task(s) deleted successfully");
                selectedTaskIds.Clear();
            }
        }


          private void SearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                string searchItem = SearchText.Text.Trim();

                using (GemmaDbContext item = new GemmaDbContext())
                {
                    var searchResults = item.TodoItems
                        .Where(v =>
                            v.Task.Contains(searchItem) ||
                            v.Id.ToString().Contains(searchItem) || 
                            v.CreatedAt.ToString().Contains(searchItem) || 
                            v.UpdatedAt.ToString().Contains(searchItem) 
                        )
                        .ToList();

                    dataGridView1.DataSource = searchResults;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            int currentX = label4.Location.X;
            currentX -= 3;
            if (currentX + label4.Width < 0)
            {

                currentX = this.Width;
            }
            label4.Location = new Point(currentX, label4.Location.Y);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 1)
            {


                DeleteButton.Enabled = true;
                DeleteButton.Text = "DeletAll";



            }
            else if (dataGridView1.SelectedRows.Count == 1)
            {

                DeleteButton.Enabled = true;
                DeleteButton.Text = "Delete";

            }
            else
            {

                DeleteButton.Enabled = false;

            }
        }
    }
}

