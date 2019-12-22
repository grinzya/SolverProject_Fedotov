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
            sm.l_ogn = 24d;
            sm.l_1 = 1;
            sm.l_2 = 0.2d;
            sm.c1 = 1d;
            sm.c2 = 8d;
            sm.x0 = 200;
            sm.t_rab = 900d;
            sm.t_okr = 20d;
            sm.t_np = 40d;
            sm.a_nar = 100d;
            sm.a_rab = 500d;
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
            model.AddConstraint("K_sigma", (sm.t_np - (sm.t_okr + ((sm.t_rab - sm.t_okr) / (((1/sm.a_rab) + (choose[1] / sm.l_1) + (choose[2]/sm.l_2) + (1/sm.a_nar))*sm.a_nar))))>=0);

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

                MessageBox.Show(reportStr);
            }
            catch (Exception ex)
            {
                MessageBox.Show("При решении задачи оптимизации возникла исключительная ситуация.");
            }
        }
    }
}
