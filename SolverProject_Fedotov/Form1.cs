using Microsoft.SolverFoundation.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SolverProject_Fedotov
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stenka_Model sm = new Stenka_Model();
            if (Tb_l_ogn.Text == "" || Tb_l_1.Text == "" || Tb_l_2.Text == "" || Tb_C1.Text == "" || Tb_C2.Text == "" || Tb_x0.Text == "" || Tb_trab.Text == "" || Tb_tokr.Text == "" || Tb_tnp.Text == "" || Tb_anar.Text == "" || Tb_arab.Text == "")
            {

                MessageBox.Show("Не все поля заполнены!", "Ошибка");
                return;
            }
            else
            {
                tp_Graph.Parent = Tab_cont;
                chart1.Series[0].Points.Clear();
                sm.l_ogn = Double.Parse(Tb_l_ogn.Text);
                sm.l_1 = Double.Parse(Tb_l_1.Text);
                sm.l_2 = Double.Parse(Tb_l_2.Text);
                sm.c1 = Double.Parse(Tb_C1.Text);
                sm.c2 = Double.Parse(Tb_C2.Text);
                sm.x0 = Double.Parse(Tb_x0.Text);
                sm.t_rab = Double.Parse(Tb_trab.Text);
                sm.t_okr = Double.Parse(Tb_tokr.Text);
                sm.t_np = Double.Parse(Tb_tnp.Text);
                sm.a_nar = Double.Parse(Tb_anar.Text);
                sm.a_rab = Double.Parse(Tb_arab.Text);
                List<SolverRow> solverList = new List<SolverRow>();
                solverList.Add(new SolverRow { xId = 1, Koef_C = sm.c1 });
                solverList.Add(new SolverRow { xId = 2, Koef_C = sm.c2 });
                SolverContext cntxt = SolverContext.GetContext();
                Model model = cntxt.CreateModel();
                Set users = new Set(Domain.Any, "users");

                //Parameter l_ogn = new Parameter(Domain.Real, "l_ogn", users);
                //l_ogn.SetBinding ()
                Parameter Koef_C = new Parameter(Domain.Real, "Koef_C", users);
                Koef_C.SetBinding(solverList, "Koef_C", "xId");
                model.AddParameter(Koef_C);

                Decision choose = new Decision(Domain.RealNonnegative, "choose", users);
                model.AddDecisions(choose);
                model.AddGoal("goal", GoalKind.Minimize, Model.Sum(Model.ForEach(users, xId => choose[xId] * Koef_C[xId])));

                //model.AddConstraint("c_choose", Model.ForEach(users, xId => (min[xId] <= choose[xId] <= max[xId])));

                model.AddConstraint("X1", choose[1] >= 0);
                model.AddConstraint("X2", choose[2] >= 0);
                model.AddConstraint("X_Sum", Model.Sum(Model.ForEach(users, xId => choose[xId])) <= sm.x0 / 1000d);
                model.AddConstraint("Temp", sm.t_okr <= sm.t_np);
                model.AddConstraint("K_sigma", (sm.t_np - (sm.t_okr + ((sm.t_rab - sm.t_okr) / (((1 / sm.a_rab) + (choose[1] / sm.l_1) + (choose[2] / sm.l_2) + (1 / sm.a_nar)) * sm.a_nar)))) >= 0);

                try
                {
                    Solution solution = cntxt.Solve();
                    Report report = solution.GetReport();

                    String reportStr = "";

                    for (int i = 0; i < solverList.Count; i++)
                    {
                        reportStr += "Значение X" + (i + 1).ToString() + ": " + choose.GetDouble(solverList[i].xId) + "\n";
                    }
                    reportStr += "\n" + report.ToString();

                    //MessageBox.Show(reportStr);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("При решении задачи оптимизации возникла исключительная ситуация.");
                }
                double x1 = Math.Round(choose.GetDouble(solverList[0].xId), 3);
                double x2 = Math.Round(choose.GetDouble(solverList[1].xId), 3);
                this.chart1.Series[0].Points.AddXY("Толщина стенок, м", x1);
                this.chart1.Series[1].Points.AddXY("Толщина стенок, м", x2);
                dataGridView1.Rows.Add(x1, x2);
                
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tp_Graph.Parent = null;
            Tb_l_ogn.Text = "24";
            Tb_l_1.Text = "1";
            Tb_l_2.Text = "0,2";
            Tb_C1.Text = "1";
            Tb_C2.Text = "8";
            Tb_x0.Text = "200";
            Tb_trab.Text = "900";
            Tb_tokr.Text = "20";
            Tb_tnp.Text = "40";
            Tb_anar.Text = "100";
            Tb_arab.Text = "500";
        }

        private void Tb_l_2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8 && number != 44) //цифры, клавиша BackSpace и запятая а ASCII
            {
                e.Handled = true;
            }
        }
    }
}
