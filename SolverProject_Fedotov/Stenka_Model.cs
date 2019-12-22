using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverProject_Fedotov
{
    class Stenka_Model
    {
        /// <summary>
        /// Коэффициент теплопроводности материала огнеупорной стенки, Вт/(м×°С)
        /// </summary>
        public double l_ogn { get; set; }

        /// <summary>
        /// Коэффициент теплопроводности 1-го материала теплоизоляции, Вт/(м×°С)
        /// </summary>
        public double l_1 { get; set; }

        /// <summary>
        /// Коэффициент теплопроводности 2-го материала теплоизоляции, Вт/(м×°С)
        /// </summary>
        public double l_2 { get; set; }

        /// <summary>
        /// Суммарная толщина изоляции, мм
        /// </summary>
        public double x0 { get; set; }

        /// <summary>
        /// Стоимость 1-го материала изоляции, усл.ед./м2
        /// </summary>
        public double c1 { get; set; }

        /// <summary>
        /// Стоимость 2-го материала изоляции, усл.ед./м2
        /// </summary>
        public double c2 { get; set; }

        /// <summary>
        /// Температура, °С  рабочей среды в печи
        /// </summary>
        public double t_rab { get; set; }

        /// <summary>
        /// Температура, °С  окружающей среды
        /// </summary>
        public double t_okr { get; set; }

        /// <summary>
        /// Температура, °С  наружной поверхности
        /// </summary>
        public double t_np { get; set; }

        /// <summary>
        /// Коэффициент теплоотдачи, Вт/(м2×°С) внутренний
        /// </summary>
        public double a_rab { get; set; }

        /// <summary>
        /// Коэффициент теплоотдачи, Вт/(м2×°С) наружный
        /// </summary>
        public double a_nar { get; set; }

    }
}
