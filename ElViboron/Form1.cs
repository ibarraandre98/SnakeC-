using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElViboron
{
    public partial class Form1 : Form
    {
        Bitmap drawArea;
        int W, H;
        int xScr = 20, yScr = 20;
        List<Puntos> ar = new List<Puntos>();
        int opc = 2;
        Random random = new Random();
        int xComida = 0, yComida = 0;
        Graphics g;
        int finalx = 10, finaly = 10;
        bool cancelar = false;
        int tipoJuego = 2;
        public Form1()
        {
            InitializeComponent();
            areaDibujo();

        }


        private void areaDibujo()
        {
            W = pbDibujo.Size.Width;
            H = pbDibujo.Size.Height;
            drawArea = new Bitmap(W, H);
        }

        private void btnArriba_Click(object sender, EventArgs e)
        {
            opc = 1;
        }

        private void repintado()
        {

            g = Graphics.FromImage(drawArea);
            Pen myPen = new Pen(Color.White);
            g.Clear(Color.Black);
            g.Dispose();
            pbDibujo.Image = drawArea;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            con = 1;

            switch (opc)
            {
                case 1:
                    haciaArriba();
                    break;
                case 2:
                    haciaDerecha();
                    break;
                case 3:
                    haciaAbajo();
                    break;
                case 4:
                    haciaIzquierda();
                    break;
            }

            
            pintarComida();
            chocarConmigo();
            if(tipoJuego == 1)
            {
                pintarTodasLasParedes();
                chocarConParedes();
            }
            else
            {
                clasico();
            }
            cancelar = false;
        }

        private void chocarConParedes()
        {
            if(ar.ElementAt(0).x <= 0 || ar.ElementAt(0).x >= finalx
                || ar.ElementAt(0).y <= 0 || ar.ElementAt(0).y >= finaly)
            {
                timer.Stop();
                MessageBox.Show("Perdiste");
                reiniciarJuego();
            }
        }

        private void clasico()
        {
            if (ar.ElementAt(0).x <= 0 || ar.ElementAt(0).x >= finalx
                || ar.ElementAt(0).y <= 0 || ar.ElementAt(0).y >= finaly)
            {
                if (ar.ElementAt(0).x > finalx)
                {
                    ar.ElementAt(0).x = 0;
                }
                else if (ar.ElementAt(0).x < 0)
                {
                    ar.ElementAt(0).x = finalx;
                }
                else if (ar.ElementAt(0).y > finaly)
                {
                    ar.ElementAt(0).y = 0;
                }
                else if (ar.ElementAt(0).y < 0)
                {
                    ar.ElementAt(0).y = finaly;
                }
            }
        }

        private void reiniciarJuego()
        {
            if (MessageBox.Show("¿Quieres jugar de nuevo?", "GAME OVER", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Restart();
            }
            else
            {
                Application.Exit();
            }
        }
        private void pintarTodasLasParedes()
        {
            int i = 0, j = 0;
            //Pared izquierda
            for (j = 0; j < finaly; j++)
            {
                pintarParedes(i, j);
            }
            j = 0;
            //Pared arriba
            for (i = 0; i < finalx; i++)
            {
                pintarParedes(i, j);
            }

            i = finalx;
            //Pared derecha
            for (j = 0; j < finaly; j++)
            {
                pintarParedes(i, j);
            }

            j = finaly - 1;
            //Pared abajo
            for (i = 1; i < finalx; i++)
            {
                pintarParedes(i, j);
            }
        }
        private void chocarConmigo()
        {
            int cont = 0;
            foreach (var pos in ar)
            {
                if (cont > 0)
                {
                    if (ar.ElementAt(0).x == pos.x && ar.ElementAt(0).y == pos.y)
                    {
                        timer.Stop();
                        MessageBox.Show("Perdiste");
                        reiniciarJuego();
                    }
                }
                cont++;
            }

        }

        private bool comidaEnViboron(int xComida, int yComida)
        {
            foreach (var pos in ar)
            {
                if (xComida == pos.x && yComida == pos.y)
                {
                    return true;
                }
            }
            return false;
        }

        private void pintarComida()
        {
            if (xComida == ar.ElementAt(0).x && yComida == ar.ElementAt(0).y)
            {
                if(timer.Interval > 20)
                    timer.Interval = timer.Interval - 10;
                ar.Add(new Puntos(ar.ElementAt(ar.Count - 1).x, ar.ElementAt(ar.Count - 1).y));
                generadordeXYComida();
                if (comidaEnViboron(xComida, yComida) == false)
                {
                    pintarComida(xComida, yComida);
                }
                else
                {
                    generadordeXYComida();
                }
            }
            else
            {
                if (comidaEnViboron(xComida, yComida) == false)
                {
                    pintarComida(xComida, yComida);
                }
                else
                {
                    generadordeXYComida();
                }
            }
        }

        private void generadordeXYComida()
        {
            xComida = random.Next(1, finalx-1);
            yComida = random.Next(1, finaly-2);
        }

        private void haciaDerecha()
        {
            int cabeza = 0;
            int banx1 = 0, banx2;
            int bany1 = 0, bany2;
            for (int i = 0; i < ar.Count; i++)
            {
                if (i == cabeza)
                {
                    //x
                    banx1 = ar.ElementAt(i).x;
                    ar.ElementAt(i).x = ar.ElementAt(i).x + 1;

                    //y
                    bany1 = ar.ElementAt(i).y;
                }
                else
                {
                    //x
                    banx2 = ar.ElementAt(i).x;
                    ar.ElementAt(i).x = banx1;
                    banx1 = banx2;

                    //y
                    bany2 = ar.ElementAt(i).y;
                    ar.ElementAt(i).y = bany1;
                    bany1 = bany2;
                }
            }
            repintado();
            foreach (var item in ar)
            {
                drawSegment(item.x, item.y);
            }
        }

        private void haciaIzquierda()
        {
            int cabeza = 0;
            int banx1 = 0, banx2;
            int bany1 = 0, bany2;
            for (int i = 0; i < ar.Count; i++)
            {
                if (i == cabeza)
                {
                    //x
                    banx1 = ar.ElementAt(i).x;
                    ar.ElementAt(i).x = ar.ElementAt(i).x - 1;

                    //y
                    bany1 = ar.ElementAt(i).y;
                }
                else
                {
                    //x
                    banx2 = ar.ElementAt(i).x;
                    ar.ElementAt(i).x = banx1;
                    banx1 = banx2;

                    //y
                    bany2 = ar.ElementAt(i).y;
                    ar.ElementAt(i).y = bany1;
                    bany1 = bany2;
                }
            }
            repintado();
            foreach (var item in ar)
            {
                drawSegment(item.x, item.y);
            }
        }


        private void haciaAbajo()
        {
            int cabeza = 0;
            int banx1 = 0, banx2;
            int bany1 = 0, bany2;
            for (int i = 0; i < ar.Count; i++)
            {
                if (i == cabeza)
                {
                    //x
                    banx1 = ar.ElementAt(i).x;


                    //y
                    bany1 = ar.ElementAt(i).y;
                    ar.ElementAt(i).y = ar.ElementAt(i).y + 1;
                }
                else
                {
                    //x
                    banx2 = ar.ElementAt(i).x;
                    ar.ElementAt(i).x = banx1;
                    banx1 = banx2;

                    //y
                    bany2 = ar.ElementAt(i).y;
                    ar.ElementAt(i).y = bany1;
                    bany1 = bany2;
                }
            }
            repintado();
            foreach (var item in ar)
            {
                drawSegment(item.x, item.y);
            }
        }

        private void haciaArriba()
        {
            int cabeza = 0;
            int banx1 = 0, banx2;
            int bany1 = 0, bany2;
            for (int i = 0; i < ar.Count; i++)
            {
                if (i == cabeza)
                {
                    //x
                    banx1 = ar.ElementAt(i).x;


                    //y
                    bany1 = ar.ElementAt(i).y;
                    ar.ElementAt(i).y = ar.ElementAt(i).y - 1;
                }
                else
                {
                    //x
                    banx2 = ar.ElementAt(i).x;
                    ar.ElementAt(i).x = banx1;
                    banx1 = banx2;

                    //y
                    bany2 = ar.ElementAt(i).y;
                    ar.ElementAt(i).y = bany1;
                    bany1 = bany2;
                }
            }
            repintado();
            foreach (var item in ar)
            {
                drawSegment(item.x, item.y);
            }
        }

        private void btnIzquierda_Click(object sender, EventArgs e)
        {
            opc = 4;
        }

        private void btnAbajo_Click(object sender, EventArgs e)
        {
            opc = 3;
        }

        private void btnDerecha_Click(object sender, EventArgs e)
        {
            opc = 2;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Up && opc != 3 && cancelar!=true)
            {
                opc = 1;
                cancelar = true;
            }
            else if (e.KeyData == Keys.Right && opc != 4 && cancelar != true)
            {
                opc = 2;
                cancelar = true;
            }
            else if (e.KeyData == Keys.Down && opc != 1 && cancelar != true)
            {
                opc = 3;
                cancelar = true;
            }
            else if (e.KeyData == Keys.Left && opc != 2 && cancelar != true)
            {
                opc = 4;
                cancelar = true;
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            
            repintado();
            generadordeXYComida();



            ar.Add(new Puntos(4, 1));
            ar.Add(new Puntos(3, 1));
            ar.Add(new Puntos(2, 1));
            ar.Add(new Puntos(1, 1));

            foreach (var item in ar)
            {
                drawSegment(item.x, item.y);
            }
            finalx = ((this.Width - 20) / 34) - 1;
            finaly = ((this.Height - 20) / 34) - 1;
        }
        int con = 1;
        private void drawSegment(int x, int y)
        {
            Graphics g;
            int xs, ys;

            xs = x * 34 + xScr;
            ys = y * 34 + yScr;

            g = Graphics.FromImage(drawArea);
            SolidBrush brocha;
            if (con == 1)
            {
                brocha = new SolidBrush(Color.Blue);
            }
            else
            {
                brocha = new SolidBrush(Color.Green);
            }
            

            g.FillRectangle(brocha, xs, ys, 8, 8);
            g.FillRectangle(brocha, xs, ys + 10, 8, 8);
            g.FillRectangle(brocha, xs, ys + 20, 8, 8);

            g.FillRectangle(brocha, xs + 10, ys, 8, 8);
            g.FillRectangle(brocha, xs + 10, ys + 10, 8, 8);
            g.FillRectangle(brocha, xs + 10, ys + 20, 8, 8);

            g.FillRectangle(brocha, xs + 20, ys, 8, 8);
            g.FillRectangle(brocha, xs + 20, ys + 10, 8, 8);
            g.FillRectangle(brocha, xs + 20, ys + 20, 8, 8);
            con++;
        }


        private void pintarComida(int x, int y)
        {

            int xs, ys;

            xs = x * 34 + xScr;
            ys = y * 34 + yScr;

            g = Graphics.FromImage(drawArea);


            SolidBrush brocha = new SolidBrush(Color.Red);

            g.FillRectangle(brocha, xs, ys, 8, 8);
            g.FillRectangle(brocha, xs, ys + 10, 8, 8);
            g.FillRectangle(brocha, xs, ys + 20, 8, 8);

            g.FillRectangle(brocha, xs + 10, ys, 8, 8);
            g.FillRectangle(brocha, xs + 10, ys + 10, 8, 8);
            g.FillRectangle(brocha, xs + 10, ys + 20, 8, 8);

            g.FillRectangle(brocha, xs + 20, ys, 8, 8);
            g.FillRectangle(brocha, xs + 20, ys + 10, 8, 8);
            g.FillRectangle(brocha, xs + 20, ys + 20, 8, 8);
        }

        private void pintarParedes(int i, int j)
        {
            int xs, ys;
            int x, y;
            x = i;
            y = j;
            xs = x * 34 + xScr;
            ys = y * 34 + yScr;

            g = Graphics.FromImage(drawArea);

            SolidBrush brocha = new SolidBrush(Color.White);

            g.FillRectangle(brocha, xs, ys, 8, 8);
            g.FillRectangle(brocha, xs, ys + 10, 8, 8);
            g.FillRectangle(brocha, xs, ys + 20, 8, 8);

            g.FillRectangle(brocha, xs + 10, ys, 8, 8);
            g.FillRectangle(brocha, xs + 10, ys + 10, 8, 8);
            g.FillRectangle(brocha, xs + 10, ys + 20, 8, 8);

            g.FillRectangle(brocha, xs + 20, ys, 8, 8);
            g.FillRectangle(brocha, xs + 20, ys + 10, 8, 8);
            g.FillRectangle(brocha, xs + 20, ys + 20, 8, 8);
        }

    }
}
