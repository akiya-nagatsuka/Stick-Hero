using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGL;
using SharpGL.SceneGraph.Assets;

namespace Stick_Hero
{
    public partial class StickHero : Form
    {
        public StickHero()
        {
            InitializeComponent();
            gameOverLabel.Visible = false;
            Random rnd = new Random();
            column2Width = (float)rnd.Next(2, 6) / 10;
            column2Gap = (float)rnd.Next(6, 10) / 10;
            column2move = column2Gap;
            //SharpGL.OpenGL gl = this.openGLControl1.OpenGL;
            //gl.Enable(OpenGL.GL_TEXTURE_2D);
           // gl.Create(SharpGL.Version.OpenGLVersion.OpenGL4_4, openGLControl1., 100,100,32, "background.jpg");
            
        }
        float column1Width = 0.7f;
        float column1Gap;
        float column1move = -0.5f;
        float column2Width; // при инициализации задаётся случайно
        float column2Gap; 
        float column2move;
        float bridge1Move = -0.5f;
        float bridge1Length = 0.0f;
        float bridge1Length2;
        float bridge1Width = 0.05f;
        float bridge1Width2;
        float bridge2Move = -0.5f;
        float bridge2Length = 0.0f;
        float bridge2Length2;
        float bridge2Width = 0.05f;
        float bridge2Width2;


        bool bridge1Waiting = true;
        bool bridge1Growing = false;
        bool bridge1Falling = false;
        bool bridge1Falled = false;
        bool bridge1Moving1 = false;
        bool bridge1Moved = false;
        bool bridge1Moving2 = false;

        bool bridge2Waiting = false;
        bool bridge2Growing = false;
        bool bridge2Falling = false;
        bool bridge2Falled = false;
        bool bridge2Moving1 = false;
        bool bridge2Moved = true;
        bool bridge2Moving2 = false;

        bool gameOver1 = false;
        bool gameOver2 = false;
        int score = 0;

        bool column1Snap1 = true;
        bool column1Snap2 = false;
        bool column1Left = false;
        bool column1Center = false;
        bool column1Rigth = false;
        bool column2Snap1 = false;
        bool column2Snap2 = true;
        bool column2Left = false;
        bool column2Center = false;
        bool column2Rigth = false;
        bool rand1 = false;
        bool rand2 = true;
        float downY = 0.0f;
        float downY2;
        float rot1 = 0;
        float rot2 = 0;
        //Texture texture = new Texture();

        void DrawColumn(float columnMove, float columnWidth) {
            OpenGL gl = openGLControl1.OpenGL;
            gl.LoadIdentity();
            gl.Translate(columnMove, -0.3f, -6.0f);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(0, 0.5f, 1.0f);
            gl.Vertex(0, 0, 5.0f);
            gl.Vertex(-columnWidth, 0, 5.0f);
            gl.Vertex(-columnWidth, -1.0f, 5.0f);
            gl.Vertex(0, -1.0f, 5.0f);
            gl.Color(0,0,1.0f);
            gl.Vertex(0, 0, 5.0f);
            gl.Vertex(-columnWidth, 0, 5.0f);
            gl.Vertex(-columnWidth, -0.105f/ ((float)openGLControl1.Height / (float)openGLControl1.Width), 5.0f);
            gl.Vertex(0, -0.105f / ((float)openGLControl1.Height / (float)openGLControl1.Width), 5.0f);
            gl.End();
        }
        void DrawBridge(float rot, float bridgeMove, float bridgeWidth, float bridgeLength)
        {
            OpenGL gl = openGLControl1.OpenGL;
            gl.LoadIdentity();
            gl.Translate(bridgeMove, -0.33f, -6.0f);
            gl.Rotate(rot, 0, 0, 1.0f);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(0, 0, 1.0f);
            gl.Vertex(bridgeWidth, downY2, 5.0f);
            gl.Vertex(bridgeWidth, bridgeLength, 5.0f);
            gl.Vertex(-bridgeWidth, bridgeLength, 5.0f);
            gl.Vertex(-bridgeWidth, -downY2, 5.0f);
            gl.End();
        }

        private void openGLControl1_OpenGLDraw(object sender, SharpGL.RenderEventArgs args)
        {
            OpenGL gl = openGLControl1.OpenGL;
            //texture.Bind(gl);
           // gl.TexCoord(0.0f,0.0f); gl.Vertex(-1.0f,-1.0f, -6.0f);
           // gl.TexCoord(1.0f, 0.0f); gl.Vertex(1.0f, -1.0f, -6.0f);
           // gl.TexCoord(1.0f, 1.0f); gl.Vertex(1.0f, 1.0f, -6.0f);
           // gl.TexCoord(0.0f, 1.0f); gl.Vertex(-1.0f, 1.0f, -6.0f);

            int H; //SCORE
            int W;
            //получаем высоту окна 
            H = (int)this.Height-(int)this.Height/10;
            W = (int)this.Width-10;
            //вычитаем высоту текста
           

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.DrawText(W/2, H, 0, 0, 0, "", 40, Convert.ToString(score));
            //GROWING
            if (bridge1Growing)
            {
                bridge1Length += 0.05f;
                DrawBridge(0.0f,-0.5f,0.05f,bridge1Length);
            }
            if (bridge2Growing)
            {
                bridge2Length += 0.05f;
                DrawBridge(0.0f, -0.5f, 0.05f, bridge2Length);
            }
            //FALLING
            if (bridge1Falling)
            {
                if (rot1 > -90.0f) //не упал ли?
                {
                    rot1 -= 10.0f;             //скорость падения
                    bridge1Length2 = bridge1Length * (1 + -rot1 * ((float)openGLControl1.Height / (float)openGLControl1.Width - 1) / 90);// в 45 градусах сильно величивается.
                    bridge1Width2 = bridge1Width / (1 + -rot1 * ((float)openGLControl1.Height / (float)openGLControl1.Width - 1) / 90);
                    DrawBridge(rot1, -0.5f, bridge1Width2, bridge1Length2);              
                }
                else
                {
                    bridge1Falling = false;
                    bridge1Falled = true;
                    DrawBridge(rot1, -0.5f, bridge1Width2, bridge1Length2);
                }
            }
            if (bridge2Falling)
            {
                if (rot2 > -90.0f) //не упал ли?
                {
                    rot2 -= 10.0f;             //скорость падения                                               
                    bridge2Length2 = bridge2Length * (1 + -rot2 * ((float)openGLControl1.Height / (float)openGLControl1.Width - 1) / 90);// в 45 градусах сильно величивается.
                    bridge2Width2 = bridge2Width / (1 + -rot2 * ((float)openGLControl1.Height / (float)openGLControl1.Width - 1) / 90);
                    DrawBridge(rot2, -0.5f, bridge2Width2, bridge2Length2);
                }
                else
                {
                    bridge2Falling = false;
                    bridge2Falled = true;
                    DrawBridge(rot2, -0.5f, bridge2Width2, bridge2Length2);              
                }
            }
            //FALLED
            if (bridge1Falled)
                if (bridge1Length2 >= 0.5f + column2Gap - column2Width && bridge1Length2 <= 0.5f + column2Gap)
                {
                    column1Left = true;
                    column2Center = true;
                    bridge1Moving1 = true;
                    bridge1Falled = false;
                    bridge2Moving2 = true;
                    bridge2Moved = false;
                    score++;
                }
                else
                {
                    gameOver1 = true;
                    gameOverLabel.Visible = true;
                }//GAMEOVER
            if (bridge2Falled)
                if (bridge2Length2 >= 0.5f + column1Gap - column1Width && bridge2Length2 <= 0.5f + column1Gap)
                {
                    column2Left = true;
                    column1Center = true;
                    bridge2Moving1 = true;
                    bridge2Falled = false;
                    bridge1Moving2 = true;
                    bridge1Moved = false;
                    score++;
                }
                else
                {
                    gameOver2 = true;
                    gameOverLabel.Visible = true;
                }//GAMEOVER
                 //BRIDGEMOVING1
            if (bridge1Moving1)
            {
                if (bridge1Move > -0.7 - bridge1Length2)
                    bridge1Move -= 0.1f;
                else { bridge1Moved = true; bridge1Moving1 = false; bridge2Moving2 = true; }
                DrawBridge(rot1, bridge1Move, bridge1Width2, bridge1Length2);
            }
            if (bridge2Moving1)
            {
                if (bridge2Move > -0.7 - bridge2Length2)
                    bridge2Move -= 0.1f;
                else { bridge2Moved = true; bridge2Moving1 = false; bridge1Moving2 = true; }
                DrawBridge(rot2, bridge2Move, bridge2Width2, bridge2Length2);
            }
            //BRIDGEMOVED
            if (bridge1Moved)
                DrawBridge(rot1, bridge1Move, bridge1Width2, bridge1Length2);
            if (bridge2Moved)
                DrawBridge(rot2, bridge2Move, bridge2Width2, bridge2Length2);
            //BRIDGEMOVING2
            if (bridge1Moving2)
            {
                if (bridge1Move > -0.9f - column2Gap)
                    bridge1Move -= 0.1f;
                else { bridge1Moving2 = false; }
                DrawBridge(rot1, bridge1Move, bridge1Width2, bridge1Length2);         
            }
            if (bridge2Moving2)
            {
                if (bridge2Move > -1.9f)
                    bridge2Move -= 0.1f;
                else { bridge2Moving2 = false; }
                DrawBridge(rot2, bridge2Move, bridge2Width2, bridge2Length2);
            }
            if (gameOver1)
            {
                gameOverLabel.Visible = true;
                DrawBridge(rot1, -0.5f, bridge1Width2, bridge1Length2);    
            }
            if (gameOver2)
            {
                gameOverLabel.Visible = true;
                DrawBridge(rot2, -0.5f, bridge2Width2, bridge2Length2);   
            }
            //ПЕРВАЯ КОЛОННА
            if (column1Snap1 || column1Snap2) //Колонна стоит
            {
                DrawColumn(column1move, column1Width);
            }

            if (column1Left)
            {
                if (column1move > -1.0f)
                    column1move -= 0.1f;
                else
                {
                    column1Left = false;
                    rand1 = true;

                }
                DrawColumn(column1move, column1Width);
            }
            if (column1Rigth)
            {
                if (rand1)                              //Новая первая колонна
                {
                    Random rnd = new Random();
                    column1Gap = (float)rnd.Next(6, 10) / 10;
                    column1move = 2.0f;
                    column1Width = (float)rnd.Next(2, 6) / 10;
                    rand1 = false;
                }
                if (column1move > column1Gap)
                { column1move -= 0.5f;if (column1move < column1Gap) column1move = column1Gap; }
                else
                {
                    column1Snap2 = true;
                    column1Rigth = false;
                    bridge2Waiting = true;
                    bridge2Length = 0.0f;
                    bridge2Move = -0.5f;
                    rot2 = 0.0f;
                }
                DrawColumn(column1move, column1Width);


            }
            if (column1Center)
            {
                if (column1move > -0.5f)
                {
                    column1move -= 0.1f;
                
                }
                else
                {
                    column1move = -0.5f;
                    column1Center = false;
                    column1Snap1 = true;
                    column2Rigth = true;
                }
                DrawColumn(column1move, column1Width);
            }
            //ВТОРАЯ КОЛОННА
            
            if (column2Snap1 || column2Snap2) //Колонна стоит
            {
                DrawColumn(column2move, column2Width);
                
            }

            if (column2Left)
            {
                if (column2move > -1.0f)
                    column2move -= 0.1f;
                else
                {

                    column2Left = false;
                    rand2 = true;

                }
                DrawColumn(column2move, column2Width);
            }
            if (column2Rigth)
            {
                if (rand2)
                {
                    Random rnd = new Random();
                    column2Gap = (float)rnd.Next(6, 10) / 10;
                    column2move = 2.0f;
                    column2Width = (float)rnd.Next(2, 6) / 10;
                    rand2 = false;
                }
                if (column2move > column2Gap)
                { column2move -= 0.5f; if (column2move < column2Gap) column2move = column2Gap; }
                else
                {
                    column2Snap2 = true;
                    column2Rigth = false;
                    bridge1Waiting = true;
                    bridge1Length = 0.0f;
                    bridge1Move = -0.5f;
                    rot1 = 0.0f;
                }
                DrawColumn(column2move, column2Width);


            }
            if (column2Center)
            {
                if (column2move > -0.5f)
                { column2move -= 0.1f;  }
                else
                {
                    column2move = -0.5f;
                    column2Center = false;
                    column2Snap1 = true;
                    column1Rigth = true;

                }
                DrawColumn(column2move, column2Width);
            }
            


            //   gl.MatrixMode(OpenGL.GL_PROJECTION);
            //   gl.LoadIdentity();
            //     gl.Ortho(0.0, (double)openGLControl1.Width, (double)openGLControl1.Height, 0.0, -6.0, 5.0);
            //   gl.MatrixMode(OpenGL.GL_MODELVIEW);
            //   gl.Disable(OpenGL.GL_DEPTH_TEST);

            gl.Flush();
        }


        private void openGLControl1_OpenGLInitialized(object sender, EventArgs e)
        {
            //  Get the OpenGL object.
            OpenGL gl = openGLControl1.OpenGL;

            //  Set the clear color.
           gl.ClearColor(1, 1, 0, 0);

        }

        private void openGLControl1_Resized(object sender, EventArgs e)
        {
         //   OpenGL gl = openGLControl1.OpenGL;
         //   gl.MatrixMode(OpenGL.GL_PROJECTION);
          //  gl.LoadIdentity();
           
          //      gl.Ortho(0, (double)openGLControl1.Width, (double)openGLControl1.Height, 0, -1, 1);
            
         //   gl.MatrixMode(OpenGL.GL_MODELVIEW);
         //   gl.Disable(OpenGL.GL_DEPTH_TEST);

        }

        private void openGLControl1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'W')
            {
                bridge1Length += 0.1f ;
            }
        }

        private void openGLControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && !gameOver1 && !gameOver2)
            {
                if (bridge2Waiting)
                {
                    bridge2Growing = true;
                    bridge2Waiting = false;                
                }
                if (bridge1Waiting)
                {
                    bridge1Growing = true;
                    bridge1Waiting = false;
                }
            }
        }

        private void openGLControl1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (bridge1Growing)
                {
                    bridge1Growing = false;
                    bridge1Falling = true;
                }
                if (bridge2Growing)
                {
                    bridge2Growing = false;
                    bridge2Falling = true;
                }
            }
        }
    }
}
