using System;

namespace my
{
	public static class SCHEME
	{
		//Параметры схемы
		public const double a=-0.25;
		public const double b=0;
		public const double c=0.25;
		//Параметры схемы
	}

	class Program
	{
		public delegate double fi1 (double x);

		public delegate double fi2 (double x);

		public static double func_fi1 (double x)
		{
			return Math.Sin (4.0*Math.PI*x);
		}

		public static double func_fi2 (double x)
		{
			return Math.Cos (4.0*Math.PI*x);
		}

		public static void Solve2 (fi1 fi,
		                           int n_x,
		                           int n_t,
		                           double tau,
		                           double h,
		                           double k,
		                           string filename,
		                           double my_t
		)
		{
			double [] gamma1=new double[n_x+1];

			//Вычисление значений нулевой строки сетки
			for (int i = 0; i <= n_x; i++)
			{
				gamma1 [i]=fi (i*h);
			}
			//Вычисление значений нулевой строки сетки

			//Вычисление строк 1 - T НАЧАЛО
			for (int t = 1; t <= n_t && my_t!=0; t++)
			{
				double coef=k/2;

				double [] alpha1=new double[n_x+1];
				double [] beta1=new double[n_x+1];
				alpha1 [0]=beta1 [0]=0;

				for (Int32 i = 1; i < n_x; i++)
				{
					alpha1 [i]=(coef)/(coef*alpha1 [i-1]+1);
					beta1 [i]=(-coef*beta1 [i-1])/(1+coef*alpha1 [i-1]);
					gamma1 [i]=(gamma1 [i]-coef*gamma1 [i-1])/(1+coef*alpha1 [i-1]); //
				}
				double [] w=new double[n_x+1];
				double[] z=new double[n_x+1];

				w [n_x]=w [0]=0;
				z [n_x]=z [0]=1;
				for (int i=n_x-1; i>0; i--)
				{  
					z [i]=alpha1 [i]*z [i+1]+beta1 [i];
					w [i]=alpha1 [i]*w [i+1]+gamma1 [i];
				}

				coef=(gamma1 [0]-coef*w [n_x-1]+coef*w [1])/(1+coef*z [n_x-1]-coef*z [1]); //
				for (int i = 0; i < n_x+1; i++)
				{
					gamma1 [i]=w [i]+coef*z [i];
				}
				if (my_t==t*tau)
					break;

			}
			using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
			{
				for (int i = 0; i <= n_x; i++)
				{
					file.WriteLine (gamma1 [i]);
				}

			}

		}

		static void Main (string [] args)
		{

			//Ввод данных
			/*Console.WriteLine ("Введите T");
			int T=int.Parse (Console.ReadLine ());
			Console.WriteLine ("Введите количество шагов по x");
			int n_x=int.Parse (Console.ReadLine ()); //количество шагов по x
			Console.WriteLine ("Введите количество шагов по t");
			int n_t=int.Parse (Console.ReadLine ());*/
			int T=1;
			int n_x=200;
			int n_t=1000;
			Console.WriteLine ("Введите необходимое время сечения");
			double my_t=double.Parse (Console.ReadLine ());
			//Ввод данных

			double tau=((double)T/n_t); //шаг по t
			double h=((double)1/n_x); //шаг по x

			double k1=-SCHEME.a*tau/h;
			double k2=-SCHEME.c*tau/h;

			Solve2 (func_fi1, n_x, n_t, tau, h, k1, "out1.txt", my_t);
			Solve2 (func_fi2, n_x, n_t, tau, h, k2, "out2.txt", my_t);
		}
	}
}
